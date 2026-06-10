from __future__ import annotations
import json,shutil,sys
from pathlib import Path

def main() -> int:
    if len(sys.argv) < 4 or sys.argv[2] != '--repo':
        print('usage: rollback_patch.py BACKUP --repo REPO', file=sys.stderr); return 2
    backup=Path(sys.argv[1]); repo=Path(sys.argv[3]); manifest=json.loads((backup/'manifest.json').read_text())
    for rel, existed in manifest['files'].items():
        dst=repo/rel; src=backup/'files'/rel
        if existed:
            dst.parent.mkdir(parents=True,exist_ok=True); shutil.copy2(src,dst)
        elif dst.exists():
            if dst.is_dir(): shutil.rmtree(dst)
            else: dst.unlink()
    print('PASS: repository restored byte-for-byte from backup')
    return 0
if __name__=='__main__': raise SystemExit(main())
