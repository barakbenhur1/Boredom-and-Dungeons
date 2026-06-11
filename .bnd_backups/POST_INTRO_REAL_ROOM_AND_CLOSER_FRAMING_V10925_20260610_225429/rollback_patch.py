from __future__ import annotations
import argparse, hashlib, json, shutil
from pathlib import Path
def digest(path): return hashlib.sha256(path.read_bytes()).hexdigest()
def main():
    p=argparse.ArgumentParser(); p.add_argument("backup"); p.add_argument("--repo", required=True); a=p.parse_args()
    b=Path(a.backup); r=Path(a.repo); m=json.loads((b/"manifest.json").read_text(encoding="utf-8"))
    for rel, existed in m["files"].items():
        target=r/rel; source=b/"files"/rel
        if existed:
            target.parent.mkdir(parents=True, exist_ok=True); shutil.copy2(source,target)
            if digest(source)!=digest(target): raise RuntimeError("rollback hash mismatch: "+rel)
        elif target.exists(): target.unlink()
    print("PASS: repository restored byte-for-byte from backup", flush=True); return 0
if __name__=="__main__": raise SystemExit(main())
