from __future__ import annotations
import argparse, json, shutil
from pathlib import Path
def main():
    parser=argparse.ArgumentParser(); parser.add_argument("backup"); parser.add_argument("--repo", required=True); args=parser.parse_args()
    backup=Path(args.backup); repo=Path(args.repo)
    manifest=json.loads((backup/"manifest.json").read_text(encoding="utf-8"))
    for rel, existed in manifest["files"].items():
        target=repo/rel; source=backup/"files"/rel
        if existed:
            target.parent.mkdir(parents=True, exist_ok=True); shutil.copy2(source, target)
        elif target.exists():
            if target.is_dir(): shutil.rmtree(target)
            else: target.unlink()
    print("PASS: repository restored byte-for-byte from backup", flush=True); return 0
if __name__=="__main__": raise SystemExit(main())
