# AGENTS Notes

This file is for non-obvious repository facts. Do not restate basic tree layout or trivial commands.

## Architecture

- This repository builds a cross-compiled Forth image for ATmega328/Arduino Nano. It is not a host-native Forth project.
- [`build.sh`](./build.sh) runs the host `gforth` binary directly. The vendored `+/gforth` tree is not the active host toolchain.
- The `+/` tree is mostly reference/source material reused by the project, not a standalone build root you should assume CI or local builds use directly.
- The active target build logic is in [`build.fs`](./build.fs). It cross-generates a raw binary and symbol table, then uses `avr-objcopy` to produce `.elf` and `.hex`.

## What `+` Means

- Treat `+/gforth` as archived upstream/reference material.
- Treat `+/ec4th` as the actual embedded Forth source tree consumed by `build.fs`.
- Do not assume “vendored” means “self-hosting”. For this repo it does not. A system `gforth` is still required unless the project is explicitly changed to bootstrap its own host compiler.

## Verification Reality

- There is no top-level automated `make test` or equivalent. The real repeatable verification path today is:
  - `bash build.sh`
  - `make forth2012-report`
  - smoke-test the ELF in `simavr`
- The simulator smoke signal to look for is `ec4th ok`. That confirms the firmware booted to the interpreter prompt.
- There are Forth test files under `+/ec4th/testing` and `+/ec4th/target/avr/tests.fs`, but they are not wired into the top-level build.

## Important Generated Artifacts

- `output/ec4th-arduino-nano-regular.bin`: raw cross-compiled image
- `output/ec4th-arduino-nano-regular.sym`: symbol table used by the report generator
- `doc/forth2012-core-wordset-coverage.md`: generated from the symbol table by [`tools/forth2012_wordset_report.py`](./tools/forth2012_wordset_report.py)

## Runtime/Target Details Worth Knowing

- The target input path is interrupt-driven UART with a ring buffer in `tib-region`, implemented in [`+/ec4th/target/avr/usart-ringbuffer.fs`](./+/ec4th/target/avr/usart-ringbuffer.fs).
- Firmware attempts XON/XOFF throttling itself. The README also notes host-side software flow control is unreliable with common Arduino USB serial setups. Both statements are true at once.
- Dictionary lookup is custom and optimized. See [`+/ec4th/kernel/hashed-search.fs`](./+/ec4th/kernel/hashed-search.fs) for the JAW hash rationale; it is not generic hash-table boilerplate.
