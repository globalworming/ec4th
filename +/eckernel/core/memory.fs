
TODO: not in use any more remove / maybe save c@ and c!?


\ +/eckernel/core/memory.fs
require ../core-ext/stack.fs
\ tuck


UNDEF-WORDS
decimal
: source ( -- c-addr u ) \ addr of t-i-b and lenght of input line
    tib #tib @ ;
 
: cell+ ( a_addr1 -- a_addr2 ) \ core
    cell + ;

: cells ( n1 -- n2 ) \ core
    [ cell 2/ dup [IF] ]
	2*
    [ [THEN] 2/ dup [IF] ]
	2*
    [ [THEN] 2/ dup [IF] ]
	2*
    [ [THEN] 2/ dup [IF] ]
	2*
    [ [THEN] drop ] ;


: char+ ( c_addr1 -- c_addr2 ) \ core
	1+ ;

: chars ( n1 -- n2 ) \ core
      ; immediate


: +! ( n a_addr -- ) \ core
    tuck @ + swap ! ;

: +!@ ( n1 a-addr -- n2 ) \ no core, but required by core/numberconversion.fs
    tuck @ + tuck rot ! ;


: 2! ( w1 w2 a_addr -- ) \ core
    tuck ! cell+ ! ;

: 2@ ( a_addr -- w1 w2 ) \ core
    dup cell+ @ swap @ ;

: allot ( n -- ) \ core
    tdp +! ;

: bounds ( addr u -- addr+u addr )
	over + swap ;

: fill ( S: c_addr u c -- ; R: -- ) \ core
  -rot bounds ?DO
    dup I c!
  LOOP drop ;

[IFUNDEF] c@
  require +/eckernel/core/bitshift8.fs
  : c@  ( c_addr -- c ) \ core
      [ bigendian [IF] ]
	  [ cell>bit 4 = [IF] ]
	      dup [ 0 cell - ] Literal and @ swap 1 and
		IF  $FF and  ELSE  8>>  THEN  ;
	      [ [ELSE] ]
		  dup [ cell 1- ] literal and
		  tuck - @ swap [ cell 1- ] literal xor
		  0 ?DO 8>> LOOP $FF and
	      [ [THEN] ]
	  [ [ELSE] ]
	    [ cell>bit 4 = [IF] ]
	      dup [ 0 cell - ] Literal and @ swap 1 and
	      IF  8>>  ELSE  $FF and  THEN
	    [ [ELSE] ]
	    dup [ cell  1- ] literal and 
	    tuck - @ swap
	    0 ?DO 8>> LOOP 255 and
	  [ [THEN] ]
      [ [THEN] ]
  ;
[THEN]

[IFUNDEF] c!
  require +/eckernel/core/bitshift8.fs
  : c!  ( c c_addr -- ) \ core
      [ bigendian [IF] ]
	[ cell>bit 4 = [IF] ]
	  tuck 1 and IF  $FF and  ELSE  8<<  THEN >r
	  dup -2 and @ over 1 and cells masks + @ and
	  r> or swap -2 and ! ;
	  Create masks $00FF , $FF00 ,
	[ELSE] ]
	  dup [ cell 1- ] literal and dup 
	  [ cell 1- ] literal xor >r
	  - dup @ $FF r@ 0 ?DO 8<< LOOP invert and
	  rot $FF and r> 0 ?DO 8<< LOOP or swap ! ;
	[THEN]
      [ELSE] ]
      [ cell>bit 4 = [IF] ]
	tuck 1 and IF  8<<  ELSE  $FF and  THEN >r
	dup -2 and @ over 1 and cells masks + @ and
	r> or swap -2 and ! ;
	Create masks $FF00 , $00FF ,
      [ELSE] ]
	dup [ cell 1- ] literal and dup >r
	- dup @ $FF r@ 0 ?DO 8<< LOOP invert and
	rot $FF and r> 0 ?DO 8<< LOOP or swap ! ;
      [THEN]
    [THEN]
  ;
[THEN]

: ,     ( w -- ) \ core 
    \G Reserve data space for one cell and store @i{w} in the space.
    here cell allot  ! ;
: c,    ( c -- ) \ core
    here 1 chars allot c! ;

: (cmove)  ( c_from c_to u -- )
 bounds ?DO  dup c@ I c! 1+  LOOP  drop ;

: cmove ( c_from c_to u -- )
  \ check whether optimization makes sense
  dup 20 u< IF (cmove) EXIT THEN
  \ check whether the destination and target address
  \ have the same byte address within the cell
  over [ 1 cells 1- ] Literal and >r
  rot dup [ 1 cells 1- ] Literal and
  dup r> <> 
  \ relative cell offset is not identical fallback to (cmove)
  IF drop -rot (cmove) EXIT THEN
  ?dup 
  IF    \ okay we have an unaligned beginning
        \ copy this pease with normal (cmove)
        ( c_to u c_from u2 )
        [ 1 cells ] Literal swap -
        >r -rot r> tuck - >r >r 2dup r> (cmove) r>
  ELSE  -rot
  THEN
  \ The rest length (u) is already calculated, now
  \ caculate the new addresses (advance to the next
  \ aligned address
  >r aligned swap aligned swap r> ( c_from c_to u )
  \ caculate the to address and the rest length 
  \ of the unalinged end; the from address is
  \ incremented by the cell copy
  2dup dup [ 1 cells 1- ] Literal and dup >r - + >r
  [ 1 cells 2 = [IF] ]
    1
  [ [THEN] ]
  [ 1 cells 4 = [IF] ]
    2
  [ [THEN] ]
  [ 1 cells 8 = [IF] ]
    3
  [ [THEN] ]
  \ okay we except to have an effective 
  \ loop implemented at least the one that
  \ increments by one, so we use the loop
  \ to count cells and not byte addresses
  tuck rshift -rot rshift swap bounds
  DO dup @ I cells ! cell+ LOOP
  \ now copy the rest
  r> r> (cmove) ;

: cmove> ( c_from c_to u -- )
    dup 0= IF
      drop 2drop exit
    THEN rot over + -rot bounds swap 1-
    DO
      1- dup c@ I c! -1
    +LOOP drop ;

: move ( c_from c_to ucount -- ) \ core
    >r 2dup u< IF
      r> cmove>
    ELSE
      r> cmove
    THEN ;

: place  ( addr len to -- ) \ gforth
    over >r  rot over 1+  r> move c! ;

has? file 0= has? new-input 0= and [IF]
: push-file  ( -- )  r>
  tibstack @ >r  >tib @ >r  #tib @ >r
  >tib @ tibstack @ = IF  r@ tibstack +!  THEN
  tibstack @ >tib ! >in @ >r  >r ;

: pop-file   ( throw-code -- throw-code )
  r>
  r> >in !  r> #tib !  r> >tib !  r> tibstack !  >r ;
[THEN]


ALL-WORDS