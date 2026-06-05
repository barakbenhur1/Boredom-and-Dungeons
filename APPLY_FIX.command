#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/apply_tap_charge_reload_minimap_stage17_fix.py
echo
echo "Done. Return to Unity and wait for compilation."
