#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/apply_control_minimap_parry_ui_polish.py
echo
echo "Done. Return to Unity and wait for compilation."
