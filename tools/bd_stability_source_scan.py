#!/usr/bin/env python3
from __future__ import annotations

import json
import re
import sys
from collections import defaultdict
from dataclasses import asdict, dataclass
from datetime import datetime, timezone
from pathlib import Path


@dataclass
class Finding:
    severity: str
    code: str
    path: str
    message: str


def find_project_root(start: Path) -> Path:
    for candidate in [start.resolve(), *start.resolve().parents]:
        if (candidate / "Assets/_Project/Scripts").is_dir():
            return candidate

    raise SystemExit(
        "ERROR: Run this script from the Boredom-and-Dungeons "
        "project root."
    )


def relative(root: Path, path: Path) -> str:
    try:
        return path.resolve().relative_to(root.resolve()).as_posix()
    except ValueError:
        return str(path)


def scan_runtime_unityeditor(
    root: Path,
    findings: list[Finding],
) -> None:
    runtime = root / "Assets/_Project/Scripts/Runtime"

    if not runtime.is_dir():
        findings.append(
            Finding(
                "BLOCKER",
                "RUNTIME_FOLDER_MISSING",
                "Assets/_Project/Scripts/Runtime",
                "Runtime scripts directory is missing.",
            )
        )
        return

    for path in sorted(runtime.rglob("*.cs")):
        text = path.read_text(
            encoding="utf-8",
            errors="replace",
        )

        if re.search(r"^\s*using\s+UnityEditor\s*;", text, re.M):
            findings.append(
                Finding(
                    "BLOCKER",
                    "UNITYEDITOR_IN_RUNTIME",
                    relative(root, path),
                    "Runtime source imports UnityEditor.",
                )
            )

        runtime_installers = len(
            re.findall(
                r"\[\s*RuntimeInitializeOnLoadMethod",
                text,
            )
        )

        if runtime_installers:
            findings.append(
                Finding(
                    "INFO",
                    "RUNTIME_INITIALIZER",
                    relative(root, path),
                    f"Contains {runtime_installers} runtime "
                    "initializer(s); verify idempotence and that scene "
                    "wiring is not duplicated.",
                )
            )


def scan_duplicate_meta_guids(
    root: Path,
    findings: list[Finding],
) -> None:
    assets = root / "Assets/_Project"

    if not assets.is_dir():
        return

    owners: dict[str, Path] = {}

    for path in sorted(assets.rglob("*.meta")):
        guid = ""

        try:
            for line in path.read_text(
                encoding="utf-8",
                errors="replace",
            ).splitlines():
                if line.startswith("guid:"):
                    guid = line.split(":", 1)[1].strip()
                    break
        except OSError as error:
            findings.append(
                Finding(
                    "WARNING",
                    "META_READ_FAILED",
                    relative(root, path),
                    str(error),
                )
            )
            continue

        if not guid:
            continue

        if guid in owners:
            findings.append(
                Finding(
                    "BLOCKER",
                    "DUPLICATE_META_GUID",
                    relative(root, path),
                    f"GUID {guid} is also used by "
                    f"{relative(root, owners[guid])}.",
                )
            )
        else:
            owners[guid] = path


def scan_missing_meta(
    root: Path,
    findings: list[Finding],
) -> None:
    assets = root / "Assets/_Project"

    if not assets.is_dir():
        return

    tracked_suffixes = {
        ".cs",
        ".prefab",
        ".unity",
        ".asset",
        ".mat",
        ".controller",
        ".anim",
        ".png",
        ".jpg",
        ".jpeg",
        ".wav",
        ".mp3",
        ".ogg",
    }

    for path in sorted(assets.rglob("*")):
        if not path.is_file():
            continue

        if path.suffix.lower() not in tracked_suffixes:
            continue

        meta = Path(str(path) + ".meta")

        if not meta.exists():
            findings.append(
                Finding(
                    "WARNING",
                    "MISSING_META",
                    relative(root, path),
                    "Unity .meta file is missing. Unity may generate "
                    "one, but references must be reviewed before commit.",
                )
            )


def scan_duplicate_type_definitions(
    root: Path,
    findings: list[Finding],
) -> None:
    scripts = root / "Assets/_Project/Scripts"

    if not scripts.is_dir():
        return

    declarations: dict[str, list[tuple[Path, bool]]] = defaultdict(list)

    namespace_pattern = re.compile(
        r"\bnamespace\s+([A-Za-z_][A-Za-z0-9_.]*)"
    )

    type_pattern = re.compile(
        r"\b(?P<partial>partial\s+)?"
        r"(?:class|struct|interface|enum)\s+"
        r"(?P<name>[A-Za-z_][A-Za-z0-9_]*)"
    )

    for path in sorted(scripts.rglob("*.cs")):
        text = path.read_text(
            encoding="utf-8",
            errors="replace",
        )

        namespace_match = namespace_pattern.search(text)
        namespace = (
            namespace_match.group(1)
            if namespace_match
            else "<global>"
        )

        for match in type_pattern.finditer(text):
            line_start = text.rfind("\n", 0, match.start()) + 1
            indentation = text[line_start:match.start()]
            indentation_width = len(indentation.expandtabs(4))

            # Project sources use block namespaces. Namespace-level type
            # declarations are indented by at most four spaces; deeper
            # declarations are nested helper types and may legitimately reuse
            # names such as State, Runner or Bootstrap in multiple owners.
            if indentation_width > 4:
                continue

            full_name = f"{namespace}.{match.group('name')}"
            is_partial = bool(match.group("partial"))
            declarations[full_name].append((path, is_partial))

    for full_name, entries in sorted(declarations.items()):
        if len(entries) <= 1:
            continue

        if all(is_partial for _, is_partial in entries):
            continue

        paths = ", ".join(
            relative(root, path)
            for path, _ in entries
        )

        findings.append(
            Finding(
                "BLOCKER",
                "DUPLICATE_TYPE_DEFINITION",
                paths,
                f"{full_name} is declared {len(entries)} times "
                "without every declaration being partial.",
            )
        )


def scan_obsolete_minimap_alignment(
    root: Path,
    findings: list[Finding],
) -> None:
    obsolete = (
        root
        / "Assets/_Project/Scripts/Runtime/UI/Minimap"
        / "BDMinimapPerspectiveAlignment.cs"
    )

    real_minimap = (
        root
        / "Assets/_Project/Scripts/Runtime"
        / "BDMazeMinimap.cs"
    )

    if obsolete.exists() and real_minimap.exists():
        findings.append(
            Finding(
                "WARNING",
                "OBSOLETE_MINIMAP_ALIGNMENT_PRESENT",
                relative(root, obsolete),
                "The project minimap is drawn by BDMazeMinimap.OnGUI. "
                "The camera-alignment component should not be active "
                "at the same time.",
            )
        )


def write_reports(
    root: Path,
    findings: list[Finding],
) -> tuple[Path, Path]:
    report_dir = (
        root
        / "Library"
        / "BoredomAndDungeons"
        / "StabilityReports"
    )
    report_dir.mkdir(parents=True, exist_ok=True)

    blocker_count = sum(
        item.severity == "BLOCKER"
        for item in findings
    )
    warning_count = sum(
        item.severity == "WARNING"
        for item in findings
    )
    info_count = sum(
        item.severity == "INFO"
        for item in findings
    )

    status = "PASS" if blocker_count == 0 else "BLOCKED"
    stamp = datetime.now(timezone.utc).strftime("%Y%m%d_%H%M%S")

    payload = {
        "generated_utc": datetime.now(timezone.utc).isoformat(),
        "status": status,
        "blockers": blocker_count,
        "warnings": warning_count,
        "info": info_count,
        "findings": [asdict(item) for item in findings],
    }

    json_path = report_dir / f"source_scan_{stamp}.json"
    text_path = report_dir / f"source_scan_{stamp}.txt"

    json_path.write_text(
        json.dumps(payload, ensure_ascii=False, indent=2),
        encoding="utf-8",
    )

    lines = [
        "Boredom & Dungeons Source Stability Scan",
        f"Status: {status}",
        f"Blockers: {blocker_count}",
        f"Warnings: {warning_count}",
        f"Info: {info_count}",
        "",
    ]

    for item in findings:
        lines.extend(
            [
                f"[{item.severity}] {item.code}",
                f"  Path: {item.path}",
                f"  {item.message}",
                "",
            ]
        )

    text_path.write_text(
        "\n".join(lines),
        encoding="utf-8",
    )

    (report_dir / "source_scan_latest.json").write_text(
        json_path.read_text(encoding="utf-8"),
        encoding="utf-8",
    )

    (report_dir / "source_scan_latest.txt").write_text(
        text_path.read_text(encoding="utf-8"),
        encoding="utf-8",
    )

    return json_path, text_path


def main() -> int:
    root = find_project_root(Path.cwd())
    findings: list[Finding] = []

    scan_runtime_unityeditor(root, findings)
    scan_duplicate_meta_guids(root, findings)
    scan_missing_meta(root, findings)
    scan_duplicate_type_definitions(root, findings)
    scan_obsolete_minimap_alignment(root, findings)

    severity_order = {
        "BLOCKER": 0,
        "WARNING": 1,
        "INFO": 2,
    }

    findings.sort(
        key=lambda item: (
            severity_order.get(item.severity, 99),
            item.code,
            item.path,
        )
    )

    json_path, text_path = write_reports(root, findings)

    blocker_count = sum(
        item.severity == "BLOCKER"
        for item in findings
    )
    warning_count = sum(
        item.severity == "WARNING"
        for item in findings
    )

    print("Boredom & Dungeons Source Stability Scan")
    print(
        "Status:",
        "PASS" if blocker_count == 0 else "BLOCKED",
    )
    print("Blockers:", blocker_count)
    print("Warnings:", warning_count)
    print("Report:", text_path)
    print("JSON:", json_path)

    for item in findings:
        print(
            f"[{item.severity}] {item.code}: "
            f"{item.path} — {item.message}"
        )

    return 1 if blocker_count else 0


if __name__ == "__main__":
    raise SystemExit(main())
