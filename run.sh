#!/bin/sh

gforth -e "fpath= .|`pwd`" avrtest.fs -e bye
avr-objcopy   --change-section-vma .data=0x0000   --rename-section .data=.text   avr-plain.elf avr.elf
simavr -m atmega328p -f 16000000 avr.elf
