#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/apply_one_click_qa_self_scan_fix_v2.py
echo
echo "Done. Return to Unity and run:"
echo "Boredom And Dungeons -> TEST EVERYTHING"
