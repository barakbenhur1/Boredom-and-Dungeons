from __future__ import annotations
import argparse,hashlib,json,shutil
from pathlib import Path
def sha(p):
 h=hashlib.sha256()
 with Path(p).open('rb') as f:
  for c in iter(lambda:f.read(1048576),b''): h.update(c)
 return h.hexdigest()
def main():
 a=argparse.ArgumentParser();a.add_argument('backup');a.add_argument('--repo',default='.');x=a.parse_args();b=Path(x.backup).resolve();r=Path(x.repo).resolve();d=json.loads((b/'manifest.json').read_text())
 for e in d['files']:
  dst=r/e['path'];src=b/'files'/e['path']
  if e['existed']:
   dst.parent.mkdir(parents=True,exist_ok=True);shutil.copy2(src,dst)
   if sha(dst)!=e['sha256']: raise RuntimeError('rollback mismatch: '+e['path'])
  elif dst.exists(): dst.unlink()
 print('PASS: repository restored byte-for-byte from V10.9.8 backup')
if __name__=='__main__': main()
