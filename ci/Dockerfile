FROM ubuntu:24.04

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        gdb-avr \
        gforth \
        python3 \
        make \
        simavr \
        binutils-avr \
        wget \
        ca-certificates \
    && rm -rf /var/lib/apt/lists/*

RUN wget -q -O /usr/local/bin/wokwi-cli \
      https://github.com/wokwi/wokwi-cli/releases/latest/download/wokwi-cli-linuxstatic-x64 \
    && chmod +x /usr/local/bin/wokwi-cli

WORKDIR /workspace

COPY . /workspace
