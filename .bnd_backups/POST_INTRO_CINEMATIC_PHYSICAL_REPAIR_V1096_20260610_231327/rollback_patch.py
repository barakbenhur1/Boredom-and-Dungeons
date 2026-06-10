from __future__ import annotations
import argparse,json,shutil,hashlib
from pathlib import Path
def sha(p):
 h=hashlib.sha256();
 with p.open('rb') as f:
  for c in iter(lambda:f.read(1024*1024),b''): h.update(c)
 return h.hexdigest()
def main():
 a=argparse.ArgumentParser(); a.add_argument('backup'); a.add_argument('--repo',default='.'); n=a.parse_args(); b=Path(n.backup).resolve(); r=Path(n.repo).resolve(); data=json.loads((b/'manifest.json').read_text())
 for e in data['files']:
  dst=r/e['path']
  if e['existed']:
   src=b/'files'/e['path']; dst.parent.mkdir(parents=True,exist_ok=True); shutil.copy2(src,dst)
  elif dst.exists():
   if dst.is_dir(): shutil.rmtree(dst)
   else: dst.unlink()
 for e in data['files']:
  dst=r/e['path']
  if e['existed'] and (not dst.exists() or sha(dst)!=e['sha256']): raise SystemExit('restore mismatch: '+e['path'])
  if not e['existed'] and dst.exists(): raise SystemExit('restore left new path: '+e['path'])
 print('PASS: repository restored byte-for-byte from backup')
 return 0
if __name__=='__main__': raise SystemExit(main())
