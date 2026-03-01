\ +/eckernel/core/align.fs

\ require +/eckernel/core-ext/control.fs


\ (?do)

UNDEF-WORDS
decimal

: align ( -- )  \  core
    here dup aligned swap 
    ?DO bl c, 
    LOOP ;

: aligned ( c_addr -- a_addr ) \ core 
    [ cell 1- ] Literal + [ -1 cells ] Literal and ;

ALL-WORDS