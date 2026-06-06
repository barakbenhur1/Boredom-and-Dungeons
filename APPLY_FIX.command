#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/apply_remove_obsolete_camera_minimap_fields.py
echo
echo "Done. Return to Unity and wait for compilation."
echo "Then run: Boredom And Dungeons -> TEST EVERYTHING"
