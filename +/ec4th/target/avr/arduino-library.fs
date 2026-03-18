

\ There is no port A on Adruino

hex
\ Port B
23 constant PINB
24 constant DDRB
25 constant PORTB
\ Port C
26 constant PINC
27 constant DDRC
28 constant PORTC
\ Port D
29 constant PIND
2A constant DDRD
2B constant PORTD


decimal
: delay ( u-millis -- ) 
\G Wait until milliseconds passed
\ correctly handles overflow
  1000 um* dmicros BEGIN 2dup dmicros 2swap d- 5 pick 5 pick d- 0>= nip UNTIL 2drop 2drop ;
