#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/apply_reliable_reload_player_up_minimap_fix.py
echo
echo "Done. Return to Unity and wait for compilation."
