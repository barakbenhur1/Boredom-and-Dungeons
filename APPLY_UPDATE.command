#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/apply_charged_shot_and_dodge_visual.py
echo
echo "Done. Return to Unity and wait for compilation."
