#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/apply_camera_minimap_enemy_safety_fix.py
echo
echo "Done. Return to Unity and run TEST EVERYTHING."
