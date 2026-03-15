\ +/eckernel/core/arithmetics.fs

require ../double/arithmetics.fs

UNDEF-WORDS
decimal

\ : + ; \ wohl im target

\ : - ; \ wohl im target

: * ( n1 n2 -- n3 ) \ core
	um* drop ;

: */ ( n1 n2 n3 -- n4 ) \ core
    */mod nip ;

: */mod ( n1 n2 n3 -- n4 n5 ) \ core
    >r m* r> sm/rem ; 

: /mod ( n1 n2 -- n3 n4 ) \ core
    >r s>d r> fm/mod ;

: mod ( n1 n2 -- n3 ) \ core
    /mod swap drop ;



\ FIXME: move



: abs ( +-n1 - +n1 ) \ core
    max-n and ; 

: fm/mod ( d1 n1 -- n2 n3 ) \ core
    dup >r dup 0< IF
      negate >r dnegate r>
    THEN
    over 0< IF
      tuck + swap
    THEN
    um/mod
    r> 0< IF
      swap negate swap
    THEN ;

: sm/rem ( d1 n1 -- n2 n3 ) \ core
    over >r dup >r abs -rot
    dabs rot um/mod
    r> r@ xor 0< IF
      negate
    THEN
    r> 0< IF
      swap negate swap
    THEN ;

: m* ( n1 n2 -- d ) \ core
    2dup 0< and >r
    2dup swap 0< and >r
    um* r> - r> - ;

\ multiplies unsigned number u1 with u2 and returns the unsigned
\ double-precision ud
: um* ( u1 u2 -- ud ) \ core
    >r >r 0 0 r> r> [ 8 cells ] literal 0 DO
      over >r dup >r 0< and d2*+ drop
      r> 2* r> swap
    LOOP 2drop ;


\ divide ud by u1. Returns quotient u3 and remainder u2
: um/mod ( ud u1 -- u2 u3 ) \ core
    0 swap [ 8 cells 1 + ] literal 0 ?DO
      /modstep 
    LOOP drop swap 1 rshift or swap ;

ALL-WORDS