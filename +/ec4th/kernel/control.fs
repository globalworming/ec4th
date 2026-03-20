\ +/eckernel/core/control.fs

UNDEF-WORDS
decimal

\ : ELSE ; \ FIXME: target?
\ : +loop ; \ FIXME: ist (+loop) oder im target? 
\ : DO ; \ FIXME: target?
\ : EXIT ; \ FIXME: target?

: (+loop) ( n -- )
    r> swap
    r> r> 2dup - >r
    2 pick r@ + r@ xor 0< 0=
    3 pick r> xor 0< 0= or IF
      >r + >r dup @ + >r
    ELSE
      >r >r drop cell+ >r
    THEN ;
 

ALL-WORDS