\ convertpad.fs Numeric conversion words <# #... working on pad 05mar02jaw

\ Credits: Basic code is from  F83
\ digit is taken from EForth (but renamed to todigit because digit is something else in
\       F83 and Open Boot
\ ud/mod is taken from GForth

\ FIXME: Move to other sections
decimal
\ FIXME in seperate datei
\ : todigit ( u -- c ) 
\ 9 over < 7 and + [char] 0 + ;

\ already defined in primitives/mem.fs
\ : +!@ ( n1 a-addr -- n2 )
\ add n1 to the cell value at addr and return the result
\  tuck @ + tuck rot ! ;

\ already defined in primitives/double.fs
\ : ud/mod ( ud1 u2 -- urem udquot )
\ Divide a ud1 by u2.
\ Compatibility GForth
\  >r 0 r@ um/mod r> swap >r um/mod r> ;

\ here starts the real thing

\ already defined in primitives/all.fs
\ Variable hld

: <# ( -- ) 
\G Start numeric conversion.
\G Compatibility F83, ANS-CORE 6.1.0490
  pad hld ! ;

: hold ( c -- )
\G Save the char for numeric output later.
\G Compatibility F83, ANS-CORE 6.1.1670 
  -1 chars hld +!@ c! ;

: # ( d -- d ) 
\G Compatibility F83, ANS-CORE 6.1.0030
  base @ ud/mod rot todigit hold ;

: #s ( d -- d ) 
\G Compatibilty F83, ANS-CORE  6.1.0050 
   BEGIN # 2dup or 0= UNTIL ;

: #> ( d -- a u ) 
\G Compatibility F83, ANS-CORE 6.1.0040 
   2drop hld @ pad over - ;

: sign ( n -- ) 
\G Compatibility F83, ANS-CORE 6.1.2210
   0< IF [char] - hold THEN ;
