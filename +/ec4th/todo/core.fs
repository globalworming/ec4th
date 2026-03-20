
: numchar ( n -- c )
  9 over < IF 7 + THEN [char] 0 + ;

: u. ( u -- )
  0 swap BEGIN 0 base @ um/mod swap numchar swap dup 0= UNTIL
  drop BEGIN dup WHILE emit REPEAT drop ;

