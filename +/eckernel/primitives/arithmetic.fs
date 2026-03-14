\ arith.fs arithmetic primitives replacement 02sep23jaw

UNDEF-WORDS

decimal

\ TODO: put some more replacements for or, - and 2*

\ ###Shifts###

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

\ ###Arithmetics +-*/###

\ adds one to the TOS
: 1+ ( S: n1--n2 ; R: -- )
	1 + ;

\ substracts one from the TOS
: 1- ( S: n1--n2 ; R: -- )
	1 - ;

\ add two to TOS, this is also aliased to cell+ and common address artihmentic
: 2+ ( S: n1--n2 ; R: -- )
	2 + ;

\ multiplies n1 with n2
: * ( n1 n2 -- n3 )
	um* drop ;

\ divide n1 by n2
: / ( n1 n2 -- n3 )
	/mod nip ;

\ returns the remainder of n1 divided by n2
: mod ( n1 n2 -- n3 )
	/mod drop ;

\ returns the quotient and remainder of n1 divided by n2
: /mod ( n1 n2 -- n3 n4 )
	>r s>d r> fm/mod ;

\ adds n1 to n3.
: under+ ( n1 n2 n3 -- n4 n2 )
	rot + swap ;

ALL-WORDS
