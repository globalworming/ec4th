
# build and run in sim

bash build.sh
simavr -m atmega328p -f 16000000 output/*.elf
