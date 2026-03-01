
require +/eckernel/compat/undef-words.fs
require constants.fs

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

\ divide d1 by n1 and returns the floored quotient n2,
\ also the remainder of this devision
: fm/mod ( d1 n1 -- n2 n3 )
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

\ divide d1 by n1 and returns the symmetric quotient n3 and remainder n2.
: sm/rem ( d1 n1 -- n2 n3 )
	over >r dup >r abs -rot
	dabs rot um/mod
	r> r@ xor 0< IF
		negate
	THEN
	r> 0< IF
		swap negate swap
	THEN ;

\ M* multiplies n1 with n2 and returns the double-precision product d
: m* ( n1 n2 -- d )
	2dup 0< and >r
	2dup swap 0< and >r
	um* r> - r> - ;

\ doubles ud and adds n to lowcell, c flag is set if MSB of highcell ud is set
: d2*+ ( ud n -- ud+n c ) 
	over min-n
	and >r >r 2dup d+ swap r> + swap r> ;

\ multiplies unsigned number u1 with u2 and returns the unsigned
\ double-precision ud
: um* ( u1 u2 -- ud )
	>r >r 0 0 r> r> [ 8 cells ] literal 0 DO
		over >r dup >r 0< and d2*+ drop
		r> 2* r> swap
	LOOP 2drop ;

\ IF c flag is set OR highcell dividend > divisor THEN subtract divisor from highcell dividend and add 1 to lowcell ( happens in 2d*+)
: /modstep ( ud c u1 -- ud c u1  )
	>r over r@ u< 0= or IF 
		r@ - 1
	ELSE
		0
	THEN d2*+ r> ;

\ divide ud by u1. Returns quotient u3 and remainder u2
: um/mod ( ud u1 -- u2 u3 )
	0 swap [ 8 cells 1 + ] literal 0 ?DO
		/modstep 
	LOOP drop swap 1 rshift or swap ;

\ adds the single-precision integer n to the double-precision integer d1
: m+ ( d1 n -- d2 )
	s>d d+ ;

: ud/mod ( ud1 u2 -- urem udquot ) \ gforth
  >r 0 r@ um/mod r> swap >r
  um/mod r> ;

: dabs ( d -- ud ) \ double d-abs
	dup 0< IF dnegate THEN ;

: s>d ( n -- d ) \ core		s-to-d
	dup 0< ;

ALL-WORDS


