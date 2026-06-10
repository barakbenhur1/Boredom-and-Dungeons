from __future__ import annotations

import argparse
import hashlib
import json
import shutil
from pathlib import Path


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def restore(backup: Path, repo: Path) -> None:
    manifest = json.loads(
        (backup / "backup_manifest.json").read_text(encoding="utf-8")
    )
    for entry in manifest["files"]:
        relative = Path(entry["path"])
        target = repo / relative
        if entry["existed"]:
            source = backup / "files" / relative
            target.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(source, target)
            if sha256(target) != entry["sha256"]:
                raise RuntimeError(f"rollback verification failed: {relative}")
        elif target.exists():
            if target.is_dir():
                shutil.rmtree(target)
            else:
                target.unlink()


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("backup")
    parser.add_argument("--repo", default=".")
    args = parser.parse_args()
    restore(Path(args.backup).resolve(), Path(args.repo).resolve())
    print("PASS: repository restored byte-for-byte from backup")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
