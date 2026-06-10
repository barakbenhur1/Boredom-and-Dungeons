from __future__ import annotations

import argparse
import hashlib
import json
import shutil
from pathlib import Path


def sha256(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("backup", type=Path)
    parser.add_argument("--repo", type=Path, default=Path.cwd())
    args = parser.parse_args()
    backup = args.backup.resolve()
    repo = args.repo.resolve()
    manifest_path = backup / "backup_manifest.json"
    if not manifest_path.is_file():
        raise SystemExit(f"BLOCKED: backup manifest not found: {manifest_path}")

    manifest = json.loads(manifest_path.read_text(encoding="utf-8"))
    for entry in manifest["files"]:
        destination = repo / entry["path"]
        if entry["existed"]:
            source = backup / "files" / entry["path"]
            destination.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(source, destination)
            if sha256(destination) != entry["sha256"]:
                raise SystemExit(
                    f"BLOCKED: rollback verification failed: {entry['path']}"
                )
        elif destination.exists():
            destination.unlink()

    print("PASS: rollback restored every owned path byte-for-byte")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
