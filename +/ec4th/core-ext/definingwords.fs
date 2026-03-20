\ +/eckernel/core-ext/definingwords.fs

UNDEF-WORDS
decimal

: Value does> @ ;

[IFDEF] docol,
: (:noname) ( -- colon-sys )
    \ common factor of : and :noname
    docol, ]comp defstart ] :-hook ;
[ELSE]
: (:noname) ( -- colon-sys )
    \ common factor of : and :noname
    docol: cfa, defstart ] :-hook ;
[THEN]


: :noname ( -- xt colon-sys )
    0 last !
    cfalign here (:noname) ;


ALL-WORDS 
