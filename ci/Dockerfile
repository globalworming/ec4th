FROM ubuntu:24.04

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        gdb-avr \
        gforth \
        python3 \
        make \
        simavr \
        binutils-avr \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /workspace

COPY . /workspace
