\ +/eckernel/core/compilerstate

UNDEF-WORDS
decimal

\ User state ( -- a-addr )
\ 0 state !

[IFUNDEF] (
: ( ( compilation 'ccc<close-paren>' -- ; run-time -- )
    [char] ) parse 2drop ; immediate
[THEN]
[IFUNDEF] \


: \ ( -- ) 
    postpone \ ;


\ FIXME: Welche ist die richtige
\ : \ ( compilation 'ccc<newline>' -- ; run-time -- )
\    [ has? file [IF] ]
\    blk @
\    IF
\	>in @ c/l / 1+ c/l * >in !
\	EXIT
\    THEN
\    [ [THEN] ]
\    source >in ! drop ; immediate
[THEN]
 


ALL-WORDS