#!/usr/bin/env bash

set -euo pipefail

log_file="$(mktemp)"
trap 'rm -f "$log_file"' EXIT

set +e
stdbuf -oL -eL timeout 5 simavr -m atmega328p -f 16000000 output/*.hex \
  >"$log_file" 2>&1
status=$?
set -e

cat "$log_file"

if [[ "$status" -ne 0 && "$status" -ne 124 ]]; then
  exit "$status"
fi

grep -F "ec4th ok" "$log_file"
