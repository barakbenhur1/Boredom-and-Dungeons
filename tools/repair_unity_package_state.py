#!/usr/bin/env python3
"""Repair Unity package resolution without touching project source or Git history.

The tool removes only generated/untracked package overlays and reproducible
Unity caches while the Unity Editor is closed. Tracked or explicitly local
packages are never removed. Every mutable repository file is backed up before
change, and moved package overlays remain available for exact rollback.
"""
from __future__ import annotations

import argparse
import json
import os
import shutil
import subprocess
import sys
from datetime import datetime, timezone
from pathlib import Path
from typing import Any

PACKAGE_ID = "BND_UNITY_UI_PACKAGE_RECOVERY_V3"
REQUIRED_DEPENDENCIES = {
    "com.unity.ugui": "2.0.0",
    "com.unity.modules.ui": "1.0.0",
}
GENERATED_CACHE_PATHS = (
    "Library/PackageCache",
    "Library/ScriptAssemblies",
    "Library/Bee",
    "Library/Search",
    "Library/Artifacts",
    "Library/StateCache",
    "Library/SourceAssetDB",
    "Library/ArtifactDB",
    "Library/PackageManager",
    "Temp",
    "obj",
)
DOC_MARKER = "BND_UNITY_UI_PACKAGE_RECOVERY_V3"
MUTABLE_FILES = (
    "Packages/manifest.json",
    "Packages/packages-lock.json",
    "ProjectGuide/INDEX.md",
    "ProjectGuide/Rules/WORKFLOW.md",
    "ProjectGuide/Engineering/REPOSITORY_SIZE_REDUCTION_POLICY.md",
    "ProjectGuide/Status/BUGS.md",
    "ProjectGuide/Status/VERIFICATION.md",
    "ProjectGuide/QA/HISTORY.md",
    "ProjectGuide/QA/UNITY_UI_PACKAGE_RECOVERY_V1.md",
    "tools/repair_unity_package_state.py",
)
LOCAL_PACKAGE_PREFIXES = ("file:", "git:", "ssh:", "http:", "https:")


def run(command: list[str], cwd: Path) -> subprocess.CompletedProcess[str]:
    return subprocess.run(
        command,
        cwd=cwd,
        text=True,
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        check=False,
    )


def utc_stamp() -> str:
    return datetime.now(timezone.utc).strftime("%Y%m%dT%H%M%SZ")


def assert_repo(root: Path) -> None:
    required = (
        "README.md",
        "Assets",
        "Packages/manifest.json",
        "ProjectSettings/ProjectVersion.txt",
    )
    missing = [path for path in required if not (root / path).exists()]
    if missing:
        raise RuntimeError(
            "Not the Boredom-and-Dungeons Unity repository; missing: "
            + ", ".join(missing)
        )


def unity_editor_processes() -> list[str]:
    result = run(["ps", "-axo", "pid=,command="], Path.cwd())
    if result.returncode != 0:
        return []
    matches: list[str] = []
    for line in result.stdout.splitlines():
        lowered = line.lower()
        if "unity.app/contents/macos/unity" not in lowered:
            continue
        if "-batchmode" in lowered and "repair_unity_package_state" in lowered:
            continue
        matches.append(line.strip())
    return matches


def load_json(path: Path) -> dict[str, Any]:
    return json.loads(path.read_text(encoding="utf-8"))


def write_json(path: Path, value: dict[str, Any]) -> None:
    path.write_text(
        json.dumps(value, indent=2, ensure_ascii=False) + "\n",
        encoding="utf-8",
    )


def git_tracked(root: Path, relative: str) -> bool:
    if not (root / ".git").exists():
        return False
    result = run(["git", "ls-files", "--", relative], root)
    return result.returncode == 0 and bool(result.stdout.strip())


def backup_file(root: Path, backup: Path, relative: str, state: dict[str, Any]) -> None:
    source = root / relative
    existed = source.exists() or source.is_symlink()
    state["files"][relative] = {"existed": existed}
    if not existed:
        return
    destination = backup / "files" / relative
    destination.parent.mkdir(parents=True, exist_ok=True)
    if source.is_symlink():
        destination.symlink_to(os.readlink(source))
    elif source.is_dir():
        shutil.copytree(source, destination, symlinks=True)
    else:
        shutil.copy2(source, destination)


def restore_file(root: Path, backup: Path, relative: str, metadata: dict[str, Any]) -> None:
    destination = root / relative
    if destination.is_symlink() or destination.is_file():
        destination.unlink()
    elif destination.is_dir():
        shutil.rmtree(destination)
    if not metadata.get("existed", False):
        return
    source = backup / "files" / relative
    destination.parent.mkdir(parents=True, exist_ok=True)
    if source.is_symlink():
        destination.symlink_to(os.readlink(source))
    elif source.is_dir():
        shutil.copytree(source, destination, symlinks=True)
    else:
        shutil.copy2(source, destination)


def dependency_is_local(value: object) -> bool:
    if not isinstance(value, str):
        return False
    return value.lower().startswith(LOCAL_PACKAGE_PREFIXES)


def package_overlay_candidates(
    root: Path,
    dependencies: dict[str, Any],
) -> tuple[list[Path], list[str]]:
    package_root = root / "Packages"
    candidates: list[Path] = []
    blockers: list[str] = []
    for child in package_root.iterdir():
        if child.name in {"manifest.json", "packages-lock.json"}:
            continue
        if not child.name.startswith("com.unity."):
            continue
        if child.name not in dependencies:
            continue
        if dependency_is_local(dependencies[child.name]):
            continue
        relative = child.relative_to(root).as_posix()
        if git_tracked(root, relative):
            blockers.append(
                f"{relative} is tracked; it may be an intentional embedded package"
            )
            continue
        if child.is_symlink() or child.is_dir() or child.is_file():
            candidates.append(child)
    return candidates, blockers


def ensure_required_dependencies(manifest_path: Path) -> bool:
    manifest = load_json(manifest_path)
    dependencies = manifest.setdefault("dependencies", {})
    changed = False
    for package_name, version in REQUIRED_DEPENDENCIES.items():
        if package_name not in dependencies:
            dependencies[package_name] = version
            changed = True
    if changed:
        manifest["dependencies"] = dict(sorted(dependencies.items()))
        write_json(manifest_path, manifest)
    return changed


def lock_is_usable(lock_path: Path) -> bool:
    if not lock_path.is_file():
        return False
    try:
        lock = load_json(lock_path)
    except Exception:
        return False
    dependencies = lock.get("dependencies", {})
    return all(name in dependencies for name in REQUIRED_DEPENDENCIES)


def remove_path(path: Path) -> None:
    if path.is_symlink() or path.is_file():
        path.unlink()
    elif path.is_dir():
        shutil.rmtree(path)


def append_marked_section(path: Path, title: str, body: str) -> None:
    begin = f"<!-- {DOC_MARKER}:BEGIN -->"
    end = f"<!-- {DOC_MARKER}:END -->"
    original = path.read_text(encoding="utf-8") if path.exists() else ""
    section = (
        f"\n\n{begin}\n"
        f"## {title}\n\n"
        f"{body.rstrip()}\n"
        f"{end}\n"
    )
    if begin in original and end in original:
        prefix, remainder = original.split(begin, 1)
        _, suffix = remainder.split(end, 1)
        updated = prefix.rstrip() + section + suffix.lstrip("\n")
    else:
        updated = original.rstrip() + section
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(updated, encoding="utf-8")


def insert_index_link(index_path: Path) -> None:
    link = (
        "- [`QA/UNITY_UI_PACKAGE_RECOVERY_V1.md`]"
        "(QA/UNITY_UI_PACKAGE_RECOVERY_V1.md)"
    )
    text = index_path.read_text(encoding="utf-8")
    if link in text:
        return
    marker = "## QA"
    position = text.find(marker)
    if position < 0:
        index_path.write_text(text.rstrip() + "\n\n## QA\n\n" + link + "\n", encoding="utf-8")
        return
    line_end = text.find("\n", position)
    insertion = line_end + 1
    index_path.write_text(
        text[:insertion] + "\n" + link + "\n" + text[insertion:],
        encoding="utf-8",
    )


def write_docs(root: Path, payload: Path, report: dict[str, Any]) -> None:
    qa_source = payload / "ProjectGuide/QA/UNITY_UI_PACKAGE_RECOVERY_V1.md"
    qa_destination = root / "ProjectGuide/QA/UNITY_UI_PACKAGE_RECOVERY_V1.md"
    qa_destination.parent.mkdir(parents=True, exist_ok=True)
    shutil.copy2(qa_source, qa_destination)

    tool_source = payload / "tools/repair_unity_package_state.py"
    tool_destination = root / "tools/repair_unity_package_state.py"
    tool_destination.parent.mkdir(parents=True, exist_ok=True)
    shutil.copy2(tool_source, tool_destination)
    tool_destination.chmod(0o755)

    insert_index_link(root / "ProjectGuide/INDEX.md")

    append_marked_section(
        root / "ProjectGuide/Rules/WORKFLOW.md",
        "Unity package/cache repair safety",
        (
            "- Unity must be fully closed before deleting or rebuilding `Library`, "
            "`Temp`, `obj`, package caches, or package overlays.\n"
            "- Repository-maintenance tools must never recurse into or delete "
            "`Packages/**` as if it were a generic cache.\n"
            "- Generated, untracked `Packages/com.unity.*` overlays may be moved "
            "only after the manifest is validated, tracked/local packages are "
            "excluded, and an external rollback backup is created.\n"
            "- Package recovery must preserve `Packages/manifest.json` and a valid "
            "`packages-lock.json`; source files are not rewritten to avoid a "
            "missing package assembly."
        ),
    )
    append_marked_section(
        root / "ProjectGuide/Engineering/REPOSITORY_SIZE_REDUCTION_POLICY.md",
        "Package and Unity-cache exclusion rule",
        (
            "Repository size reduction is allowed to remove reproducible Unity "
            "caches only while the Editor is closed. It must treat `Packages/` as "
            "a protected dependency boundary, must not follow package symlinks, "
            "and must not mutate immutable package contents. Package overlays and "
            "package-resolution state are repaired separately through "
            "`tools/repair_unity_package_state.py`."
        ),
    )
    append_marked_section(
        root / "ProjectGuide/Status/BUGS.md",
        "2026-06-09 — UnityEngine.UI package resolution after cache cleanup",
        (
            "Observed blocker: `UnityEngine.UI`, `Image`, `Text`, `RawImage`, and "
            "`Outline` could not resolve even though `com.unity.ugui` and "
            "`com.unity.modules.ui` are declared. Unity also reported generated "
            "package symlinks, altered immutable package contents, and a missing "
            "`Library/Search` database file. V3 moves untracked generated package "
            "overlays to an external backup and rebuilds only reproducible package/"
            "script/search caches. Status after applying V3: recovery applied; "
            "Unity compilation and TEST EVERYTHING still require real local proof."
        ),
    )
    append_marked_section(
        root / "ProjectGuide/Status/VERIFICATION.md",
        "Unity UI package recovery V3 verification gate",
        (
            "Static recovery validates the package manifest, package lock, protected "
            "package ownership, source references, documentation, and rollback "
            "state. It does not claim Unity compilation. Reopen Unity 6000.0.76f1, "
            "wait for package resolution/import, clear the Console, and rerun "
            "`Boredom And Dungeons → TEST EVERYTHING`. Record the resulting "
            "blocker/warning/info counts before committing."
        ),
    )
    append_marked_section(
        root / "ProjectGuide/QA/HISTORY.md",
        "2026-06-09 — Unity UI dependency recovery prepared",
        (
            "TEST EVERYTHING was blocked by one compilation blocker: the UGUI "
            "assembly was unavailable to the handheld presenter and first-launch "
            "tutorial. Recovery V3 preserves source and Git state, removes only "
            "untracked generated package overlays and reproducible caches, and "
            "adds a repeatable package-repair tool. Unity verification remains "
            "pending after the Editor rebuild."
        ),
    )

    report_path = root / "ProjectGuide/QA/UNITY_UI_PACKAGE_RECOVERY_LATEST.json"
    report_path.write_text(
        json.dumps(report, indent=2, ensure_ascii=False) + "\n",
        encoding="utf-8",
    )


def validate_state(root: Path) -> list[str]:
    errors: list[str] = []
    manifest_path = root / "Packages/manifest.json"
    try:
        manifest = load_json(manifest_path)
    except Exception as error:
        return [f"invalid Packages/manifest.json: {error}"]
    dependencies = manifest.get("dependencies", {})
    for name in REQUIRED_DEPENDENCIES:
        if name not in dependencies:
            errors.append(f"missing manifest dependency: {name}")

    lock_path = root / "Packages/packages-lock.json"
    if lock_path.exists() and not lock_is_usable(lock_path):
        errors.append("packages-lock.json is invalid or missing required UGUI entries")

    overlays, blockers = package_overlay_candidates(root, dependencies)
    errors.extend(blockers)
    for overlay in overlays:
        errors.append(
            "generated package overlay still present: "
            + overlay.relative_to(root).as_posix()
        )

    source_paths = (
        root / "Assets/_Project/Scripts/Runtime/UI/BDModernHandheld3DPresenter.cs",
        root / (
            "Assets/_Project/Scripts/Runtime/UI/"
            "BDModernHandheld3DPresenter.FirstLaunchTutorial.cs"
        ),
    )
    for source_path in source_paths:
        if not source_path.is_file():
            errors.append(f"missing source file: {source_path.relative_to(root)}")
            continue
        text = source_path.read_text(encoding="utf-8")
        if "using UnityEngine.UI;" not in text:
            errors.append(
                f"source no longer imports UnityEngine.UI: {source_path.relative_to(root)}"
            )

    required_paths = (
        "tools/repair_unity_package_state.py",
        "ProjectGuide/QA/UNITY_UI_PACKAGE_RECOVERY_V1.md",
        "ProjectGuide/QA/UNITY_UI_PACKAGE_RECOVERY_LATEST.json",
    )
    for relative in required_paths:
        if not (root / relative).is_file():
            errors.append(f"missing durable recovery artifact: {relative}")
    return errors


def repair(root: Path, payload: Path, backup_root: Path) -> Path:
    assert_repo(root)
    running = unity_editor_processes()
    if running:
        raise RuntimeError(
            "Unity Editor is running. Close Unity completely and rerun.\n- "
            + "\n- ".join(running)
        )

    manifest_path = root / "Packages/manifest.json"
    manifest = load_json(manifest_path)
    dependencies = manifest.get("dependencies", {})
    candidates, blockers = package_overlay_candidates(root, dependencies)
    if blockers:
        raise RuntimeError(
            "Unsafe package ownership detected; nothing was changed:\n- "
            + "\n- ".join(blockers)
        )

    timestamp = utc_stamp()
    backup = backup_root / PACKAGE_ID / timestamp
    backup.mkdir(parents=True, exist_ok=False)
    state: dict[str, Any] = {
        "packageId": PACKAGE_ID,
        "createdUtc": timestamp,
        "repo": str(root),
        "files": {},
        "movedPackageOverlays": [],
        "removedGeneratedCaches": [],
    }
    for relative in MUTABLE_FILES:
        backup_file(root, backup, relative, state)

    ensure_required_dependencies(manifest_path)
    if (root / "Packages/packages-lock.json").exists() and not lock_is_usable(
        root / "Packages/packages-lock.json"
    ):
        remove_path(root / "Packages/packages-lock.json")
        state["regeneratePackageLock"] = True
    else:
        state["regeneratePackageLock"] = False

    overlay_backup = backup / "package_overlays"
    overlay_backup.mkdir(parents=True, exist_ok=True)
    # Re-evaluate after the manifest is corrected.
    dependencies = load_json(manifest_path).get("dependencies", {})
    candidates, blockers = package_overlay_candidates(root, dependencies)
    if blockers:
        raise RuntimeError("\n".join(blockers))
    for source in candidates:
        destination = overlay_backup / source.name
        os.rename(source, destination)
        state["movedPackageOverlays"].append(
            {
                "from": source.relative_to(root).as_posix(),
                "backup": destination.relative_to(backup).as_posix(),
                "kind": "symlink" if destination.is_symlink() else "directory",
            }
        )

    for relative in GENERATED_CACHE_PATHS:
        target = root / relative
        if not (target.exists() or target.is_symlink()):
            continue
        remove_path(target)
        state["removedGeneratedCaches"].append(relative)

    report = {
        "packageId": PACKAGE_ID,
        "appliedUtc": timestamp,
        "requiredDependencies": REQUIRED_DEPENDENCIES,
        "movedPackageOverlays": state["movedPackageOverlays"],
        "removedGeneratedCaches": state["removedGeneratedCaches"],
        "packageLockWillRegenerate": state["regeneratePackageLock"],
        "unityCompilation": "NOT RUN — reopen Unity and run TEST EVERYTHING",
    }
    write_docs(root, payload, report)

    state_path = backup / "state.json"
    state_path.write_text(
        json.dumps(state, indent=2, ensure_ascii=False) + "\n",
        encoding="utf-8",
    )
    shutil.copy2(
        Path(__file__),
        backup / "repair_unity_package_state.py",
    )
    latest = backup_root / PACKAGE_ID / "LATEST"
    latest.write_text(str(backup) + "\n", encoding="utf-8")

    errors = validate_state(root)
    if errors:
        rollback(root, backup)
        raise RuntimeError(
            "Post-repair validation failed and mutable files were restored:\n- "
            + "\n- ".join(errors)
        )
    return backup


def rollback(root: Path, backup: Path) -> None:
    state_path = backup / "state.json"
    if not state_path.is_file():
        raise RuntimeError(f"Missing rollback state: {state_path}")
    state = load_json(state_path)

    for relative, metadata in state.get("files", {}).items():
        restore_file(root, backup, relative, metadata)

    for item in reversed(state.get("movedPackageOverlays", [])):
        destination = root / item["from"]
        source = backup / item["backup"]
        if destination.is_symlink() or destination.is_file():
            destination.unlink()
        elif destination.is_dir():
            shutil.rmtree(destination)
        destination.parent.mkdir(parents=True, exist_ok=True)
        if source.exists() or source.is_symlink():
            os.rename(source, destination)


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--repo", type=Path, default=Path.cwd())
    parser.add_argument("--payload", type=Path)
    parser.add_argument("--backup-root", type=Path)
    parser.add_argument("--validate-only", action="store_true")
    parser.add_argument("--rollback", type=Path)
    args = parser.parse_args()

    root = args.repo.resolve()
    if args.rollback is not None:
        rollback(root, args.rollback.resolve())
        print(f"PASS: restored V3 mutable files and package overlays from {args.rollback}")
        print("Generated Unity caches were not restored because Unity rebuilds them.")
        return 0

    if args.validate_only:
        errors = validate_state(root)
        if errors:
            print("BLOCKED: Unity package recovery validation failed:", file=sys.stderr)
            for error in errors:
                print("- " + error, file=sys.stderr)
            return 1
        print("PASS: Unity UI package recovery static validation passed.")
        print("NOT CLAIMED: Unity compilation, package import, TEST EVERYTHING, Play Mode.")
        return 0

    if args.payload is None:
        parser.error("--payload is required for repair")
    backup_root = (
        args.backup_root.resolve()
        if args.backup_root
        else root.parent / "Boredom-and-Dungeons_PatchBackups"
    )
    backup = repair(root, args.payload.resolve(), backup_root)
    print("PASS: Unity UGUI/package cache recovery applied.")
    print(f"Rollback backup: {backup}")
    print("NOT PERFORMED: commit, push, branch, PR, merge, pull, reset, stash, clean, checkout.")
    print("NEXT: reopen Unity, wait for package import/compilation, then run TEST EVERYTHING.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
