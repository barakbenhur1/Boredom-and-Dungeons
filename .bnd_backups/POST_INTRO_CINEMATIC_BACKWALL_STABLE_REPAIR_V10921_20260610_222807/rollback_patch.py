from __future__ import annotations
import argparse, hashlib, json, shutil
from pathlib import Path

def digest(path: Path): return hashlib.sha256(path.read_bytes()).hexdigest()
def main():
    p=argparse.ArgumentParser(); p.add_argument("backup"); p.add_argument("--repo", required=True); a=p.parse_args()
    backup=Path(a.backup); repo=Path(a.repo); manifest=json.loads((backup/"manifest.json").read_text())
    for rel, existed in manifest["files"].items():
        dst=repo/rel; src=backup/"files"/rel
        if existed:
            dst.parent.mkdir(parents=True, exist_ok=True); shutil.copy2(src,dst)
            if digest(src)!=digest(dst): raise RuntimeError("rollback hash mismatch: "+rel)
        elif dst.exists():
            if dst.is_dir(): shutil.rmtree(dst)
            else: dst.unlink()
    print("PASS: repository restored byte-for-byte from backup", flush=True)
    return 0
if __name__=="__main__": raise SystemExit(main())
