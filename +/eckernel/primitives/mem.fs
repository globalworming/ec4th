
require +/eckernel/compat/undef-words.fs

require stackmanipulation.fs

UNDEF-WORDS
decimal

\ assume one byte chars for ec all the time
' 1+ Alias char+ ( c_addr1 -- c_addr2 )
: noop ;
' noop Alias chars ( c_addr1 -- c_addr2 )

[IFUNDEF] cell
\G Size of cell in chars, same as "1 cells"
1 cells Constant cell
[THEN]

defined? cell+ 0= cell 2 = and [IF]
' 2+ Alias cell+
[THEN]

\ adds the length of a cell to a_addr1 and returns a_addr2
: cell+ ( a_addr1 -- a_addr2 )
	cell + ;

defined? cells 0= cell 2 = and [IF]
' 2* Alias cells
[THEN]

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

: aligned ( c_addr -- a_addr ) \ core 
    [ cell 1- ] Literal + [ -1 cells ] Literal and ;

\ FIXME: move to nio
\ read value from variable and prints
\ : ?  ( addr -- )
\   @ . ;

: bounds ( addr u -- addr+u addr )
	over + swap ;

\ add n to the integer saved at a_addr
: +! ( n a_addr -- )
	tuck @ + swap ! ;

\ TODO: keep 8>> and 8<<?

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


: +! ( n a_addr -- )
	tuck @ + swap ! ;

: 2! ( w1 w2 a_addr -- )
	tuck ! cell+ ! ;

: 2@ ( a_addr -- w1 w2 )
	dup cell+ @ swap @ ;

: cmove> ( c_from c_to u -- )
	dup 0= IF
		drop 2drop exit
	THEN rot over + -rot bounds swap 1-
	DO
		1- dup c@ I c! -1
	+LOOP drop ;

: cmove
	0 ?DO
		over I + c@ over I + c!
	LOOP 2drop ;

: move ( c_from c_to ucount -- )
	>r 2dup u< IF
		r> cmove>
	ELSE
		r> cmove
	THEN ;

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

: on  ( a-addr -- ) \ gforth
    \G Set the (value of the) variable  at @i{a-addr} to @code{true}.
    true  swap ! ;
: off ( a-addr -- ) \ gforth
    \G Set the (value of the) variable at @i{a-addr} to @code{false}.
    false swap ! ;

\ : allot ( n -- )        tdp +! ;

ALL-WORDS
