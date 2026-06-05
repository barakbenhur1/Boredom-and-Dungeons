#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/apply_projectile_knockback_update.py
echo
echo "Done. Open Unity and wait for compilation."
