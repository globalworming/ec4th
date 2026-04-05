#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$SCRIPT_DIR/.."
mkdir -p output/test

pids=()

while IFS= read -r scenario; do
  scenario_dir="$(dirname "$scenario")"
  scenario_name="$(basename "$scenario_dir")"
  diagram="$scenario_dir/diagram.json"

  wokwi-cli \
    --scenario "$scenario" \
    --diagram-file "$diagram" \
    --timeout 500 \
    --serial-log-file "output/test/$scenario_name.serial.log" &
  pids+=("$!")
done < <(find wokwi-scenario -mindepth 2 -maxdepth 2 -name '*.test.yaml' | sort)

status=0

for pid in "${pids[@]}"; do
  wait "$pid" || status=$?
done

exit "$status"
