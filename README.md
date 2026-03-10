
## Recommended VC Code Plugins

ext install fttx.language-forth
Ctags Companion


    apt-get install simavr binutils-avr


Work in progress

bash run.sh

## Using GDB to debug AVR code

Start the simulator with:

    simavr -g -m atmega328p -f 16000000 avr.elf

The -g will not run the program immediately but wait for a debugger to connect.

And start the debugger with:

     avr-gdb avr.elf  -ex "target remote :1234" -ex 'display/i $pc'

The `display/i $pc` will print the instruction for every step.



avrdude -p atmega328p -c arduino -P /dev/ttyUSB0 -b 115200 -D -U flash:w:avr.hex:i

