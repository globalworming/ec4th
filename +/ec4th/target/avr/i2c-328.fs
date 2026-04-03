\ Generic i2c and lcd definitions for Arduino / ATmega328

\ --- TWI register addresses (data space) ---
$B8 constant TWBR          \ bit rate register
$B9 constant TWSR          \ status register
$BB constant TWDR          \ data register
$BC constant TWCR          \ control

\ --- TWCR bit masks ---
$80 constant TWINT         \ interrupt flag
$20 constant TWSTA         \ start condition
$10 constant TWSTO         \ stop condition
$04 constant TWEN          \ TWI enable

: i2c-init  ( -- )
\ Initialise TWI at ~100kHz (assuming 16MHz clock)
\ SCL = 16e6 / (16 + 2 * 72 * 1) = 100kHz
    \ set bit rate
    72 TWBR io!             
    \ prescaler = 1
     0 TWSR io! ;

: i2c-init-fast  ( -- )
\ Initialise TWI at ~400kHz (assuming 16MHz clock)
\ SCL = 16e6 / (16 + 2 * 18 * 1) = 400kHz
\ TODO: the higher speed does not gain much at the moment, maybe remove
    \ set bit rate
    18 TWBR io!             
    \ prescaler = 1
     0 TWSR io! ;

: i2c-wait  ( -- )
\g Wait until TWI operation completes
    begin  TWCR io@ TWINT and  until ;

: i2c-start  ( -- )
\G Send START condition
    [ TWINT TWSTA or TWEN or ] literal TWCR io! ;

: i2c-stop  ( -- )
\ Send STOP condition
    i2c-wait
    [ TWINT TWSTO or TWEN or ] literal TWCR io! ;

: i2c-write  ( byte -- )
\ Transmit one byte
    i2c-wait TWDR io! [ TWINT TWEN or ] literal TWCR io! ;

\ I2C bus probing

| : i2c-status  ( -- status )
\ Read TWI status (upper 5 bits)
    TWSR io@ $F8 and ;

| : i2c-ack?  ( 7bit-addr -- flag )
\ Probe a 7-bit address; return true if ACK
    i2c-start
    2*             \ shift address into bits 7-1, W bit = 0
    i2c-write
    i2c-wait
    i2c-status $18 =      \ $18 = SLA+W ACK received
    i2c-stop ;

: i2c-scan  ( -- )
\G Init I2C bus with 100khz and scan I2C bus and print results in grid
\G Scans 7-bit addresses $08..$77 (skips reserved ranges)
    i2c-init-fast cr
    ."     0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F"
    $80 0 do
        \ row header every 16 addresses
        i $0F and 0= if
            cr i .byte space
        then
        i 8 < over $78 > or if
            \ reserved ranges: print blanks
            ."  . "
        else
            \ print address if device found
            i i2c-ack? if i .byte else ." -- " then
        then
    loop
    cr ;

\  I2C LCD driver for PCF8574 backpack + HD44780
\  4-bit mode — assumes i2c-init/start/stop/write from above
\  LCD 7-bit address is always passed on the stack.

\ --- PCF8574 pin mapping ------------------------------------
\  P7 P6 P5 P4  P3  P2  P1  P0
\  D7 D6 D5 D4  BL  EN  RW  RS
$01 constant lcd-rs         \ register select (0=cmd, 1=data)
$04 constant lcd-en         \ enable strobe
$08 constant lcd-bl         \ backlight

: lcd-i2c!  ( byte i2c-addr -- )
\ Low-level: send one byte to the PCF8574 
    i2c-start
    2* i2c-write             \ SLA+W
    i2c-write                \ data byte
    i2c-stop ;

: lcd-strobe  ( nibble-byte i2c-addr -- )
\ Pulse enable while keeping other signals
    over lcd-en or over lcd-i2c!  \ EN high
    lcd-i2c! ;                    \ EN low

: lcd-send  ( byte flags i2c-addr -- )
\ Send a full byte as two 4-bit nibbles
\ flags = RS and/or BL bits to OR in
    >r                        \ byte flags          R: addr
    over $F0 and  over or     \ byte flags (hi|fl)  R: addr
    r@ lcd-strobe             \ byte flags          R: addr
    swap 4 lshift $F0 and or  \ (lo|fl)             R: addr
    r> lcd-strobe ;

: lcd-cmd   ( byte i2c-addr -- )  lcd-bl swap lcd-send ;
: lcd-emit  ( char i2c-addr -- )  [ lcd-bl lcd-rs or ] literal swap lcd-send ;

: lcd-init  ( i2c-addr -- )
\ HD44780 initialisation (by-the-datasheet)
    i2c-init
    \ at least 40ms pause, actually only needed at cold start
    50 ms
    \ Force 8-bit mode three times, then switch to 4-bit
    dup $30 lcd-bl or swap lcd-strobe  5 ms
    dup $30 lcd-bl or swap lcd-strobe  1 ms
    dup $30 lcd-bl or swap lcd-strobe  1 ms
    dup $20 lcd-bl or swap lcd-strobe  1 ms
    \ Function set: 4-bit, 2 lines, 5×8 font
    dup $28 swap lcd-cmd  1 ms
    \ Display on, cursor off, blink off
    dup $0C swap lcd-cmd  1 ms
    \ Clear display
    dup $01 swap lcd-cmd  2 ms
    \ Entry mode: increment, no shift
    $06 swap lcd-cmd  1 ms ;

: lcd-clear  ( i2c-addr -- )      
\ Clear display and cursor to 0,0
    $01 swap lcd-cmd  2 ms ;
: lcd-home   ( i2c-addr -- )      
\ Cursor to 0,0 without clearing
    $02 swap lcd-cmd  2 ms ;

\ Set cursor: col 0-15/19, row 0-1 (or 0-3)
\ Row offsets: 0=$00, 1=$40, 2=$14, 3=$54
const create lcd-rows  $00 c, $40 c, $14 c, $54 c,

: lcd-at  ( col row i2c-addr -- )
    >r lcd-rows + c@ + $80 or r> lcd-cmd ;

: lcd-type  ( addr len i2c-addr -- )
\ Print a counted string
    -rot bounds do i c@ over lcd-emit loop drop ;

: lcd-number ( n i2c-addr -- )
\ Print a number to the lcd
\ TODO: maybe remove
    >r s>d <# #s #> r> lcd-type ;
