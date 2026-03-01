\ +/eckernel/core/dictionary

UNDEF-WORDS
decimal

: >body ( xt -- a_addr ) \ core
	2 cells + ;

: here  ( -- addr ) \ core 
    dp @ ;

ALL-WORDS 
