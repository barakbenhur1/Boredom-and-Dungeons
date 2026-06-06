#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/apply_remove_obsolete_camera_minimap_fields_v2.py
echo
echo "Done. Return to Unity and run TEST EVERYTHING."
