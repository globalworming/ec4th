# Using i2c and an LCD display with the Arduino Nano

The ATmega328 chip come with... 

TODO: explain hardware support briefly, also TW vs. i2c naming

## Getting started

Connect the following pins:

- SDA (data): A4
- SCL (clock): A5

When connected, you can scan the I2C bus with `i2c-scan`. Here is an exmple with an LCD connected at address $27:

````
i2c-scan 
    0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F
00   .  .  .  .  .  .  .  . -- -- -- -- -- -- -- -- 
10  -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
20  -- -- -- -- -- -- -- 27 -- -- -- -- -- -- -- -- 
30  -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
40  -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
50  -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
60  -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
70  -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
~108ms  ok
````

## LCD

Support for HD44780 via PCF8574 is build in. Each word for lcd operations require the i2c-address as parameter.

For the i2c address `$27`, for example, use this to initialise the LCD:

````
$27 lcd-init
````

To print a string do:

````
s" Hola" $27 lcd-type
````

Addtional available words for the LCD: lcd-at, lcd-home, lcd-clear
