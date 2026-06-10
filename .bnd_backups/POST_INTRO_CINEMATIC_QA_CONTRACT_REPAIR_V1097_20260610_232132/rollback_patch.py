from __future__ import annotations
import argparse, hashlib, json, shutil
from pathlib import Path

def sha(path: Path) -> str:
 h=hashlib.sha256()
 with path.open('rb') as f:
  for chunk in iter(lambda:f.read(1024*1024),b''): h.update(chunk)
 return h.hexdigest()

def main():
 ap=argparse.ArgumentParser(); ap.add_argument('backup'); ap.add_argument('--repo',default='.')
 a=ap.parse_args(); b=Path(a.backup).resolve(); r=Path(a.repo).resolve()
 data=json.loads((b/'manifest.json').read_text())
 for e in data['files']:
  dst=r/e['path']; src=b/'files'/e['path']
  if e['existed']:
   dst.parent.mkdir(parents=True,exist_ok=True); shutil.copy2(src,dst)
   if sha(dst)!=e['sha256']: raise RuntimeError('restore hash mismatch: '+e['path'])
  elif dst.exists():
   if dst.is_dir(): shutil.rmtree(dst)
   else: dst.unlink()
 print('PASS: repository restored byte-for-byte from backup')
if __name__=='__main__': main()
