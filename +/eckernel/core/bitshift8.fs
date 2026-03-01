\ +/eckernel/core/bitshift8.fs

UNDEF-WORDS
decimal

: 8>> ( n1 -- n2 )
    8 rshift ;

: 8<< ( n1 -- n2 )
    8 lshift ;


ALL-WORDS 
