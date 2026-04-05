#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR="$( cd "`dirname $0`"; pwd )"
cd $SCRIPT_DIR/..
mkdir -p output/test

pids=()

for scenario in $( ls wokwi-scenario/*.test.yaml ); do
  base="$(basename "$scenario" .yaml)"
  wokwi-cli \
    --scenario "$scenario" \
    --timeout 500 \
    --serial-log-file "output/test/$base.serial.log" &
  pids+=("$!")
done

status=0

for pid in "${pids[@]}"; do
  wait "$pid" || status=$?
done

exit "$status"
