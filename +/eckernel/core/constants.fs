\ +/eckernel/core/constants.fs 

NEEDED? FIXME: remove

UNDEF-WORDS
decimal

32 Constant bl
\G Space character


\ FIXME: kann später weg, ist in memory definiert
: cells ( n1 -- n2 ) \ core
    [ cell 2/ dup [IF] ]
	2*
    [ [THEN] 2/ dup [IF] ]
	2*
    [ [THEN] 2/ dup [IF] ]
	2*
    [ [THEN] 2/ dup [IF] ]
	2*
    [ [THEN] drop ] ;



1 cells Constant cell
\G Size of cell in chars, same as "1 cells"


1 1 cells 8 * 1- lshift
Constant min-n
min-n invert Constant max-n
\G The minimal signed integer. This is used for the comparison primitives to mask out the MSB

8 Constant bits/byte

250 Constant maxtiblength


\ FIXME
-1 Constant f83headerstring

\ cell Constant tcell

ALL-WORDS