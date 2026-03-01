
decimal

: upc ( c1 -- c2 )
\G Convert ASCII c1 to uppercase. Input values from ASCII a-z are converted to A-Z,
\G all other values are unchanged
\G Compatiblity F83, OpenBoot (0x81)
   dup [ char z 1+ char a 1- ] literal literal within
   IF  [ char a char A - ] literal - THEN ;

: digit   ( char base -- n true | char false )
\G Convert a single character to a number in the given base.
\G Compatibility F83, Open Boot (0xa3)
  >r upc
  dup dup [ char A 1- ] literal u>
  IF   [ char A 10 - ] literal
  ELSE \ chars between 9 and a (exclusive) are wrong
       dup [char] 9 u> or
       [char] 0
  THEN
  - dup r> u< 
  IF nip true EXIT THEN
  drop false ;


0 [IF]
\ first version, taken from gforth and reworked
\ the eforth version is shorter
| : accumulate ( +d0 digit base - +d1 )
  >r swap  r@  um* drop rot  r>  um* d+ ;

: (>number) ( ud1 c-addr1 u1 base -- ud2 c-addr2 u2 )
  >r 0 
  ?DO    count J digit
  WHILE  swap J swap >r accumulate r>
  LOOP   0 rdrop EXIT
  THEN   drop 1- I' I - unloop rdrop ;
[THEN]

: (>number) ( ud1 c-addr1 u1 base -- ud2 c-addr2 u2 )
  >r 
  BEGIN dup
  WHILE >r dup >r c@ J digit
  WHILE swap J um* drop rot J um* d+ r> char+ r> 1-
  REPEAT drop r> r> THEN r> drop ;

\ defined in strings/string>number.fs
\ : >number ( ud1 c-addr1 u1 -- ud2 c-addr2 u2 )
\ convert to double number until bad char
\ Compatibility AnsForth (6.1.0570)
\  base @ (>number) ;

: $number ( c-addr1 u1 -- u2 )
\G Convert a string to a number
\G Compatibility Open Boot
  >r >r 0 0 r> r> >number 2drop drop ;




