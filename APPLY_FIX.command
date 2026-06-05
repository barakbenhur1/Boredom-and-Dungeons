#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/remove_duplicate_stage16_boss_hud.py
echo
echo "Done. Return to Unity and wait for compilation."
