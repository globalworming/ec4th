
\ FIXME: split up!


: lit ( -- w )
	r> dup @ swap cell+ >r ;

: sgn ( n -- -1/0/1 )
	dup 0= IF
		EXIT
	THEN 0< 2* 1+ ;

: toupper ( c1 -- c2 )
	dup [char] a - [ char z char a - 1 + ] Literal u< bl and - ;

\ already defined in strings/compare.fs
\ : capscomp ( c_addr1 u c_addr2 -- n )
\ 	swap bounds ?DO
\		dup c@ I c@ <>
\		IF
\			dup c@ toupper I c@ toupper =
\		ELSE
\			true
\		THEN
\		WHILE
\			1+
\	LOOP drop 0
\		ELSE
\			c@ toupper I c@ toupper - unloop
\		THEN sgn ;

: -trailing ( c_addr u1 -- c_addr u2 )
	BEGIN
		1- 2dup + c@ bl =
	WHILE
		dup 0=
	UNTIL
	ELSE
		1+
	THEN ;

\ already implemented in stings/basics.fs
\ : /string ( c_addr1 u1 n -- c_addr2 u2 )
\	tuck - >r + r> dup 0< IF
\		- 0
\	THEN ;

\ already defined in primitives/string.fs
\ : count ( c_addr1 -- c_addr2 u )
\	dup 1+ swap c@ ;

: (hashkey) ( c_addr u1 -- u2 )
	0 -rot bounds ?DO
		I c@ toupper +
	LOOP ;


Create rot-values
	5 c, 0 c, 1 c, 2 c, 3 c,  4 c, 5 c, 5 c, 5 c, 5 c,
	3 c, 5 c, 5 c, 5 c, 5 c,  7 c, 5 c, 5 c, 5 c, 5 c,
	7 c, 5 c, 5 c, 5 c, 5 c,  6 c, 5 c, 5 c, 5 c, 5 c,
	7 c, 5 c, 5 c,

: (hashkey1) ( c_addr u ubits -- ukey )
	dup rot-values + c@ over 1 swap lshift 1- >r
	tuck - 2swap r> 0 2swap bounds ?DO
		dup 4 pick lshift swap 3 pick rshift or
		I c@ toupper xor
		over and
	LOOP nip nip nip ;


\ already defined in parsing/parseword.fs
\ : (parse-white) ( c_addr1 u1 -- c_addr2 u2 )
\	BEGIN
\		dup
\	WHILE
\		over c@ bl <=
\		WHILE
\			1 /string
\		REPEAT
\		THEN 2dup
\	BEGIN
\		dup
\		WHILE
\			over c@ bl >
\			WHILE
\				1 /string
\			REPEAT
\			THEN nip - ;

: aligned ( c_addr -- a_addr )
	[ cell 1- ] Literal + [ -1 cells ] Literal and ;

: faligned ( c_addr -- f_addr )
	[ 1 floats 1- ] Literal + [ -1 floats ] Literal and ;

: >body ( xt -- a_addr )
	2 cells + ;

: >code-address ( xt -- c_addr )
	@ ;

: >does-code ( xt -- a_addr )
	cell+ @ ;

: code-address! ( c_addr xt -- )
	! ;

\ : does-code! ( a_addr xt -- )
\	dodoes: over ! cell+ ! ;

: does-handler! ( a_addr -- )
	drop ;

: /does-handler ( -- n )
	2 cells ;

: threading-method ( -- n )
	1 ;


AVariable UP


has? OS [IF] [THEN]
: up! ( a_addr -- )
	up ! ;

\ ###################

: on true swap ! ;
: off false swap ! ;

: pad    ( -- c-addr ) \ core-ext
\ c-addr is the address of a region that can be used as temp data
\ storage. At least 84 chars of space are available.
	here word-pno-size + aligned ;

\ already defined in nio/convertpad
\ : <# ( -- )
\	\ Start numeric conversion.
\	pad hld ! ;

\ already defined in nio/dothex.fs
\ : todigit ( u -- c ) 
\  9 over < 7 and + [char] 0 + ;

\ already defined in nio/convertpad.fs
\ : hold ( c -- )
\ Save the char for numeric output later.
\	-1 chars hld +!@ c! ;
\ : # ( d -- d )
\	base @ ud/mod rot todigit hold ;
\ : #s ( d -- d )
\	BEGIN # 2dup or 0= UNTIL ;
\ : sign ( n -- )
\	0< IF [char] - hold THEN ;

: #> ( d -- a u )
	2drop hld @ pad over - ;


\ : (.)   ( n -- c-addr cnt )
\	dup abs 0 <# #s rot sign #> ;

\ : .r ( n l -- ) \ print right justified
\ 	>r  (.)  r> over - spaces type ;

: :doesjump ;
\ : 2constant create does> 2@ ; FIXME

: CR 13 (emit) 10 (emit) ;
: emit (emit) ;
: space %100000 (emit) ;
: spaces 0 ?do space loop ;
