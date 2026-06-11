from __future__ import annotations
import argparse
import hashlib
import json
import shutil
from pathlib import Path

def digest(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()

def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument('backup')
    parser.add_argument('--repo', required=True)
    args = parser.parse_args()
    backup = Path(args.backup)
    repo = Path(args.repo)
    manifest = json.loads((backup / 'manifest.json').read_text(encoding='utf-8'))
    for relative, existed in manifest['files'].items():
        target = repo / relative
        source = backup / 'files' / relative
        if existed:
            target.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(source, target)
            if digest(source) != digest(target):
                raise RuntimeError('rollback hash mismatch: ' + relative)
        elif target.exists():
            if target.is_dir(): shutil.rmtree(target)
            else: target.unlink()
    print('PASS: repository restored byte-for-byte from backup', flush=True)
    return 0

if __name__ == '__main__':
    raise SystemExit(main())
