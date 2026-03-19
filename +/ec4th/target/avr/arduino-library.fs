

\ There is no port A on Adruino

hex
\ Port B
23 constant pinb
24 constant ddrb
25 constant portb
\ Port C
26 constant pinc
27 constant ddrc
28 constant portc
\ Port D
29 constant pind
2A constant ddrd
2B constant portd

' c@ Alias io@
' c! Alias io!

decimal
: ms ( u-millis -- ) \ https://forth-standard.org/standard/facility/MS
\G Wait until milliseconds passed
  1000 um* dmicros BEGIN 2dup dmicros 2swap d- 5 pick 5 pick d- 0>= nip UNTIL 2drop 2drop ;
