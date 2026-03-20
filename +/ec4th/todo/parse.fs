\ parse.fs Implements the word parse based on source and >in 06mar02jaw
\ Version $Id: parse.fs,v 1.1 2002/03/06 14:25:36 jeans Exp $

\ FIXME move this to skip and scan file

: input ( -- addr len )
  >in @ dup source + - 0 min ;


: sword ( char -- addr len )
  >r input 2dup r@ skip r> scan swap 1+ >in ! - ;
    
: parse-word ( -- addr len )
  bl sword ;





: name ( -- c-addr count ) \ gforth
    source 2dup >r >r >in @ /string (parse-white)
    2dup + r> - 1+ r> min >in ! ;

: word   ( char "<chars>ccc<char>-- c-addr ) \ core
    \G Skip leading delimiters. Parse @i{ccc}, delimited by
    \G @i{char}, in the parse area. @i{c-addr} is the address of a
    \G transient region containing the parsed string in
    \G counted-string format. If the parse area was empty or
    \G contained no characters other than delimiters, the resulting
    \G string has zero length. A program may replace characters within
    \G the counted string. OBSOLESCENT: the counted string has a
    \G trailing space that is not included in its length.
    sword here place  bl here count + c!  here ;


: skip   ( addr1 n1 char -- addr2 n2 ) 
\G skip all characters equal to char
\G Compatibility GForth, F83
   >r 
   BEGIN dup WHILE over c@ r@  = WHILE 1 /string
   REPEAT THEN rdrop ;

: scan   ( addr1 n1 char -- addr2 n2 )
\G skip all characters not equal to char
\G Compatibility GForth, F83
   >r 
   BEGIN dup WHILE over c@ r@ <> WHILE 1 /string
   REPEAT THEN rdrop ;


\ here starts the real thing

: parse ( delim -- c-addr len )
\ Skip whitespace characters and return the next word from
\ the input buffer delimited by whitespace.
\ Compatibity Open Boot
  >r source >in @ /string over -rot r> scan 
  dup IF 1- THEN source nip swap - >in !
\  [ \ this operation only works if 1 chars == 1, so warn
\    1 chars 1 <> ERROR" Addressunit must be one char"
\  ]
  over - ;
