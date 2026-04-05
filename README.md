# ec4th

## Getting started with ec4th on Arduino Nano

Assuming you have Ubuntu, install `avrdude`:

    sudo apt-get install avrdude

Also, to communicate with the Arduino, you need a terminal program. One popular choice is `tio`. Install it with

    sudo apt-get install tio

Connect your Arduino Uno to the computer and check which port it is on. Typically, `/dev/ttyUSB0` should appear.
Make sure you have permission to access `/dev/ttyUSB0`. Typically this requires that you have the group `dialout`.

Download the latest ec4th release from https://github.com/cruftex/ec4th/releases. The system image is available in
Intel hex format and has the name: `ec4th-arduino-nano-regular.hex`.

Send the image to the Arduino:

    avrdude -p atmega328p -c arduino -P /dev/ttyUSB0 -b 115200 -D -U flash:w:ec4th-arduino-nano-regular.hex:i

After flashing, its time to talk to the Forth system, start `tio` with:

    tio -b 115200 -o 1 /dev/ttyUSB0

Option `-o 1` adds a 1 milli second pause after sending each character. Depending on the command, the Forth system 
is not capable of processing the input at the full speed of the 115k bit/s. 

Implementation note: The Forth system also implements software flow control via XON/XOFF, however, I was not able to get it working. In `tio` the option `-f soft` switches on software handshake. 
However, there is a long standing open bug in the Linux kernel that this is not yet implemented for the USB chip that the Arduino uses. See: https://bugzilla.kernel.org/show_bug.cgi?id=197109




## Development setup for ec4th

Notes covering the development 

### Recommended VS Code Plugins

VS Code has two useful plugins for working on the ec4th sources.

- ext install fttx.language-forth - Syntac highlighting
- Ctags Companion - jump to definition with F12

### Install AVR simulator and bin utils

Unfortunately the simulator wants an ELF file. So, to use the simulator the binary image needs to be converted into and ELF.

    apt-get install simavr binutils-avr

### Using GDB to debug AVR code

Start the simulator with:

    simavr -g -m atmega328p -f 16000000 avr.elf

The -g will not run the program immediately wait for a debugger to connect.

And start the debugger with:

     avr-gdb avr.elf  -ex "target remote :1234" -ex 'display/i $pc'

The `display/i $pc` will print the instruction for every step.

# running wokwi scenarios

<https://docs.wokwi.com/wokwi-ci/cli-installation>

```bash
make all
# see https://wokwi.com/dashboard/ci
WOKWI_CLI_TOKEN=wok_JFXXXX.. \ 
wokwi-cli \
    --scenario wokwi-scenario/smoke.test.yaml \
    --timeout 500 \
    --serial-log-file "output/test/smoke.serial.log" 
 ```


