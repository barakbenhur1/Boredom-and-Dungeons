#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/apply_horse_and_landing_animation_fix.py
echo
echo "Done. Return to Unity and wait for compilation."
