\ +/eckernel/core/strings.fs
\ require +/eckernel/core-ext/comparison.fs
\ within
\ require +/eckernel/core-ext/stack.fs
\ tuck

UNDEF-WORDS
decimal


: /string ( c_addr1 u1 n -- c_addr2 u2 )
\G Adjust the character string by u1 characters. The resulting
\G character string begins at c-addr1 plus n characters (giving
\G c_addr2) and is u1 minus n charactes long.
\G Compatibility ANS (17.6.1.0245)
    tuck - >r chars + r> ;


: upc ( c1 -- c2 )
\G Convert ASCII c1 to uppercase. Input values from ASCII a-z are converted to A-Z,
\G all other values are unchanged
    dup [ char z 1+ char a 1- ] literal literal within
    IF  [ char a char A - ] literal - THEN ;

: type ( addr u -- ) \ core
    0 DO
      dup c@ (emit) 1+
    LOOP drop ;

: count ( c_addr1 -- c_addr u ) \ core
    dup 1+ swap c@ ;

ALL-WORDS 
