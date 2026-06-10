from __future__ import annotations
import argparse
import hashlib
import json
import shutil
from pathlib import Path

def digest(path: Path) -> str:
    h = hashlib.sha256()
    with path.open('rb') as f:
        for chunk in iter(lambda: f.read(1024 * 1024), b''):
            h.update(chunk)
    return h.hexdigest()

def main() -> int:
    p = argparse.ArgumentParser()
    p.add_argument('backup')
    p.add_argument('--repo', default='.')
    args = p.parse_args()
    backup = Path(args.backup).resolve()
    repo = Path(args.repo).resolve()
    manifest = json.loads((backup / 'manifest.json').read_text(encoding='utf-8'))
    for rel, expected in manifest['files'].items():
        src = backup / 'files' / rel
        dst = repo / rel
        dst.parent.mkdir(parents=True, exist_ok=True)
        shutil.copy2(src, dst)
        if digest(dst) != expected:
            raise SystemExit(f'rollback hash mismatch: {rel}')
    print('PASS: documentation rollback restored every owned file byte-for-byte')
    return 0

if __name__ == '__main__':
    raise SystemExit(main())
