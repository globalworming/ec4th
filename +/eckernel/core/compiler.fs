
: \ ( compilation 'ccc<newline>' -- ; run-time -- ) \ thisone- core-ext,block-ext backslash
    source >in ! drop ; immediate

: ( ( compilation 'ccc<close-paren>' -- ; run-time -- ) \ thisone- core,file	paren
    [char] ) parse 2drop ; immediate

Variable state
Defer prompt

: (prompt)        state @ IF ."  compiled" EXIT THEN ."  ok" ;
' (prompt) is prompt

