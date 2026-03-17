\ arith.fs arithmetic primitives replacement 02sep23jaw

UNDEF-WORDS

decimal

\ adds one to the TOS
: 1+ ( S: n1--n2 ; R: -- )
	1 + ;

\ substracts one from the TOS
: 1- ( S: n1--n2 ; R: -- )
	1 - ;

: 2+ ( S: n1--n2 ; R: -- )
\G Add 2 to TOS. Non standard word but embedded 16 bit systems
\G can be expected to have this since its the same as cell+
\G this should be implemented as primitive, aliased to cell+
	2 + ;

: 2* ( n1 - n2 ) \ core
\G Should be implemented as primitive since also used for cells
    dup + ;

: 2/  ( n1 -- n2 ) \ core
    dup min-n and IF 1 ELSE 0 THEN
    [ bits/byte cell * 1- ] literal 
    0 DO 2* swap dup 2* >r min-n and 
      IF 1 ELSE 0 THEN or r> swap
    LOOP nip ;

\ shifts u1 n times to right. Effectivly it is deviding u1 n times by 2
: rshift ( S: u1 n--u2 ; R: -- )
	0 ?DO 2/ [ min-n invert ] literal and LOOP ;

\ shifts u1 n times to left. Effectivly it is multiplying u1 n times with 2
: lshift ( S: u1 n--u2 ; R: -- )
	0 ?DO 2* LOOP ;

\ ###Bit manipulation###

\ inverts all bits in u1 i.e. 1010 -> 0101
: invert ( S: u1--u2 ; R: -- )
  	-1 xor ;

\ negates n1
: negate ( S: n1--n2 ; R: -- )
	invert 1+ ;

\ returns the absolute value of n i.e. -3 becomes 3
: abs ( S: n--u ; R: -- )
	dup 0< IF negate THEN ;

defined? um* 0= defined? um/mod 0= or [IF]
 \ doubles ud and adds n to lowcell, c flag is set if MSB of highcell ud is set
| : d2*+ ( ud n -- ud+n c ) 
	over min-n
	and >r >r 2dup d+ swap r> + swap r> ;
[THEN]

\ multiplies unsigned number u1 with u2 and returns the unsigned
\ double-precision ud
: um* ( u1 u2 -- ud ) \ core
    >r >r 0 0 r> r> [ 8 cells ] literal 0 DO
      over >r dup >r 0< and d2*+ drop
      r> 2* r> swap
    LOOP 2drop ;

\ multiplies n1 with n2
: * ( n1 n2 -- n3 )
	um* drop ;

\ M* multiplies n1 with n2 and returns the double-precision product d
: m* ( n1 n2 -- d )
	2dup 0< and >r
	2dup swap 0< and >r
	um* r> - r> - ;

[IFUNDEF] um/mod
\ IF c flag is set OR highcell dividend > divisor THEN subtract divisor from highcell dividend and add 1 to lowcell ( happens in 2d*+)
| : /modstep ( ud c u1 -- ud c u1  )
	>r over r@ u< 0= or IF 
		r@ - 1
	ELSE
		0
	THEN d2*+ r> ;
[THEN]

\ divide ud by u1. Returns quotient u3 and remainder u2
: um/mod ( ud u1 -- u2 u3 ) \ core
    0 swap [ 8 cells 1 + ] literal 0 ?DO
      /modstep 
    LOOP drop swap 1 rshift or swap ;

: fm/mod ( d1 n1 -- n2 n3 ) \ core
\ divide d1 by n1 and returns the floored quotient n2,
\ also the remainder of this devision
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
\ divide d1 by n1 and returns the symmetric quotient n3 and remainder n2.
    over >r dup >r abs -rot
    dabs rot um/mod
    r> r@ xor 0< IF
      negate
    THEN
    r> 0< IF
      swap negate swap
    THEN ;

: ud/mod ( ud1 u2 -- urem udquot ) \ gforth
	>r 0 r@ um/mod r> swap >r
	um/mod r> ;


\ divide n1 by n2
: / ( n1 n2 -- n3 )
	/mod nip ;

: */mod ( n1 n2 n3 -- n4 n5 ) \ core
    >r m* r> sm/rem ; 

: s>d ( n -- d ) \ core		s-to-d
	dup 0< ;

\ returns the quotient and remainder of n1 divided by n2
: /mod ( n1 n2 -- n3 n4 )
	>r s>d r> fm/mod ;

: */ ( n1 n2 n3 -- n4 ) \ core
    */mod nip ;

\ returns the remainder of n1 divided by n2
: mod ( n1 n2 -- n3 )
	/mod drop ;

: mod ( n1 n2 -- n3 ) \ core
    /mod swap drop ;

0 [IF]
\ adds n1 to n3.
: under+ ( n1 n2 n3 -- n4 n2 )
	rot + swap ;
[THEN]

ALL-WORDS
