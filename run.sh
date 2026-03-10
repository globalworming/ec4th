#!/bin/sh

set -e

gforth -e "fpath= .|`pwd`" avrtest.fs -e bye 
avr-objcopy -I binary -O elf32-avr --change-section-vma .data=0x0000 --rename-section .data=.text avr.img avr.elf
avr-objcopy -O ihex -R .eeprom avr.elf avr.hex
simavr -m atmega328p -f 16000000 avr.elf
