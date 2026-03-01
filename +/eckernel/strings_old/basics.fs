
\ existiert schon
\ : -trailing ( c_addr u1 -- c_addr u2 )
\ Remove trailing spaces from string.
\ Compatibility ANS (17.6.0170)
\  BEGIN dup WHILE 1- 2dup chars + c@ bl <> 
\  UNTIL 1+ THEN ;

: /string ( c_addr1 u1 n -- c_addr2 u2 )
\G Adjust the character string by u1 characters. The resulting
\G character string begins at c-addr1 plus n characters (giving
\G c_addr2) and is u1 minus n charactes long.
\G Compatibility ANS (17.6.1.0245)
  tuck - >r chars + r> ;

: skip   ( addr1 n1 char -- addr2 n2 ) \ gforth
    \ skip all characters equal to char
    >r
    BEGIN
	dup
    WHILE
	over c@ r@  =
    WHILE
	1 /string
    REPEAT  THEN
    rdrop ;

: scan   ( addr1 n1 char -- addr2 n2 ) \ gforth
    \ skip all characters not equal to char
    >r
    BEGIN
	dup
    WHILE
	over c@ r@ <>
    WHILE
	1 /string
    REPEAT  THEN
    rdrop ;

: place ( adr len adr )
        2dup c! char+ swap move ;

 
: +place ( adr len adr )
        2dup c@ + over c!
        dup c@ char+ + swap move ;
