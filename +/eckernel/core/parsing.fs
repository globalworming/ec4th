\ +/eckernel/core/parsing.fs

UNDEF-WORDS
decimal

: skip   ( addr1 n1 char -- addr2 n2 ) 
    >r 
    BEGIN dup WHILE over c@ r@  = WHILE 1 /string
    REPEAT THEN rdrop ;

: scan   ( addr1 n1 char -- addr2 n2 )
    >r 
    BEGIN dup WHILE over c@ r@ <> WHILE 1 /string
    REPEAT THEN rdrop ;

: input ( -- addr len )
    >in @ dup source + - 0 min ;

: sword ( char -- addr len )
    >r input 2dup r@ skip r> scan swap 1+ >in ! - ;

 
: word   ( char "<chars>ccc<char>-- c-addr ) \ core
    sword here place  bl here count + c!  here ;

: parse-word ( -- addr len )
  bl sword ;

: (parse-white) ( c_addr1 u1 -- c_addr2 u2 )
    BEGIN
      dup
    WHILE
      over c@ bl <=
      WHILE
	1 /string
      REPEAT
      THEN 2dup
    BEGIN
      dup
      WHILE
	over c@ bl >
	WHILE
	  1 /string
	REPEAT
	THEN nip - ;

: name ( -- c-addr count ) 
    source 2dup >r >r >in @ /string (parse-white)
    2dup + r> - 1+ r> min >in ! ;

 ALL-WORDS