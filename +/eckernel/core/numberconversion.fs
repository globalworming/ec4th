\ +/eckernel/core/numberconversion.fs
require +/eckernel/double/arithmetics.fs
UNDEF-WORDS

: decimal ( -- ) \ core
    10 base ! ;

: <# ( -- )  \ core
    pad hld ! ;

: hold ( c -- ) \ core
    -1 chars hld +!@ c! ;

: todigit ( u -- c ) 
    9 over < 7 and + [char] 0 + ;

: # ( d -- d )  \ core
    base @ ud/mod rot todigit hold ;

: #s ( d -- d ) \ core
    BEGIN # 2dup or 0= UNTIL ;

: #> ( d -- a u )  \ core
    2drop hld @ pad over - ;

: sign ( n -- )  \ core
    0< IF [char] - hold THEN ;

ALL-WORDS