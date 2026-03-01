
require +/eckernel/compat/undef-words.fs

require stackmanipulation.fs

UNDEF-WORDS
decimal

\ FIXME: move to nio
\ read value from variable and prints
: ?  ( addr -- )
  @ . ;

\ add n to the integer saved at a_addr
: +! ( n a_addr -- )
	tuck @ + swap ! ;

\ divides 8 times by 2. Same as 8 rshift.
: 8>> ( S: n1--n2 ; R: -- )
	2/ 2/ 2/ 2/ 2/ 2/ 2/ 2/ ;

\ multiplies 8 times with 2. Same as 8 lshift.
: 8<< ( S: n1--n2 ; R: -- )
	2* 2* 2* 2* 2* 2* 2* 2* ;

\ stores two values at a_addr, first value w1 then w2.
: 2! ( w1 w2 a_addr -- )
	tuck ! cell+ ! ;
\ adds two values to values in a_addr
: 2+! ( w1 w2 a_addr -- )
  tuck +! cell+ +! ;
\ fetches two values from a_addr.
: 2@ ( a_addr -- w1 w2 )
	dup cell+ @ swap @ ;

\ adds the length of a cell to a_addr1 and returns a_addr2
: cell+ ( a_addr1 -- a_addr2 )
	cell + ;

\ FIXME: -> ?! n1 cell -> n1 cell/2 cell/2 -> n1 cell/2 cell -> usw...
: cells ( n1 -- n2 )
	[ cell 2/ dup [IF] ]
		2*
	[ [THEN] 2/ dup [IF] ]
		2*
	[ [THEN] 2/ dup [IF] ]
		2*
	[ [THEN] 2/ dup [IF] ]
		2*
	[ [THEN] drop ] ;

\ adds the length of a char to c_addr1 and returns c_addr2
: char+ ( c_addr1 -- c_addr2 )
	1+ ;

\ FIXME: chars tut nichts? ist es wirklich immediate
\ : chars ( n1 -- n2 ) \ core
\ ; immediate


: +! ( n a_addr -- )
	tuck @ + swap ! ;

: 2! ( w1 w2 a_addr -- )
	tuck ! cell+ ! ;

: 2@ ( a_addr -- w1 w2 )
	dup cell+ @ swap @ ;


: char+ ( c_addr1 -- c_addr2 )
	1+ ;

: (chars) ( n1 -- n2 )
	;

: move ( c_from c_to ucount -- )
	>r 2dup u< IF
		r> cmove>
	ELSE
		r> cmove
	THEN ;

: cmove> ( c_from c_to u -- )
	dup 0= IF
		drop 2drop exit
	THEN rot over + -rot bounds swap 1-
	DO
		1- dup c@ I c! -1
	+LOOP drop ;
\ already defined in strings/compare.fs
\ : compare ( c_addr1 u1 c_addr2 u2 -- n )
\	rot 2dup swap - >r min swap -text dup
\	IF
\		rdrop
\	ELSE
\		drop r> sgn
\	THEN ;

\ saves c to all bytes from c_addr to c_addr+u
: fill ( S: c_addr u c -- ; R: -- )
  -rot bounds ?DO
    dup I c!
  LOOP drop ;

\ puts zero to TOS and handle it over to fill
: erase ( S: c_addr u -- c_addr u c ; R: -- )
	0 fill ;

: +!@ ( n1 a-addr -- n2 )
	tuck @ + tuck rot ! ;

: bounds ( addr u -- addr+u addr )
	over + swap ;

: cmove
	0 ?DO
		over I + c@ over I + c!
	LOOP 2drop ;

\ : allot ( n -- )        tdp +! ;

ALL-WORDS
