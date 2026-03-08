\ +/eckernel/core-ext/stack.fs

UNDEF-WORDS
decimal

: 2>r ( w1 w2 -- )
    swap r> swap >r swap >r >r ;

: 2r> ( -- w1 w2 )
    r> r> swap r> swap >r swap ;

: 2r@ ( -- w1 w2 )
    i' j ;

: nip ( n1 n2 - n2 )
    swap drop ;

: tuck ( n1 n2 - n2 n1 n2 )
    dup -rot ;

ALL-WORDS 
