\ +/eckernel/core-ext/comparison.fs

require +/eckernel/core/bitlogic.fs
\ u< 0< negate 0=
UNDEF-WORDS
decimal

: <> ( n1 n2 -- f )
    xor 0<> ;

: 0<> ( n -- f )
    0= 0= ;
 
: 0> ( n -- f )
    negate 0< ;

: within ( u1 u2 u3 -- f )
    over - >r - r> u< ;



ALL-WORDS