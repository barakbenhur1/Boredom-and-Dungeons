#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 tools/bd_stability_source_scan.py
echo
echo "Source scan passed. Open Unity and run:"
echo "Boredom And Dungeons -> Validation -> Rebuild Prototype And Run Gate"
