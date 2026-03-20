\ double primitive fillers 									19mar26jaw

UNDEF-WORDS
decimal

\ double word set

\ adds double-precision d2 to double-precision d1
: d+ ( d1 d2 -- d )
	rot + >r tuck + swap over u> r> swap - ;

\ substracts double-precision d2 from double-precision d1
: d- ( d1 d2 -- d )
	dnegate d+ ;

\ negates double-precision d1
: dnegate ( d1 -- d2 )
	invert swap negate tuck 0= - ;

\ multiplies double-precision d1 with 2
: d2* ( d1 -- d2 )
	2dup d+ ;

\ divide double-precision d1 by 2
: d2/ ( d1 -- d2 )
	dup 1 and >r 2/ swap 2/ [ 1 8 cells 1- lshift 1- ] Literal and
	r> IF
		[ 1 8 cells 1- lshift ] Literal +
	THEN swap ;

\ adds the single-precision integer n to the double-precision integer d1
: m+ ( d1 n -- d2 )
	s>d d+ ;

: dabs ( d -- ud ) \ double d-abs
	dup 0< IF dnegate THEN ;

: d0< nip 0< ;

: du< ( u1 u2 -- f )
	2 pick over xor 0< IF 2nip d0< ELSE d- d0< THEN ;

: d0= ( d -- f )
	or 0= ;

: d= ( d1 d2 -- f ) 
	rot xor >r xor r> d0= ;

ALL-WORDS


