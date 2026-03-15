\ Basic numeric output                                          15mar26jaw

\ Recursive numeric output implementation for arbitrary base
\ For .error we should be able to output the exception number
\ although there is a dictionary overflow. 

[IFUNDEF] todigit
 : todigit ( u -- c ) 
    9 over < 7 and + [char] 0 + ;
[THEN]

| : u.1x ( u base -- )
    tuck 0 swap um/mod ?dup IF rot RECURSE ELSE nip THEN todigit emit ;

: u.x ( u base -- )
\G Output unsigned number with arbirary base.
\G Only uses stack for the output and does not rely on available dictionary space
\G as pictured numeric output would do.
    over 0= IF 2drop '0 emit ELSE u.1x THEN ;

: .x ( n base -- )
\G Output signed number with arbirary base
\G Only uses stack for the output and does not rely on available dictionary space
\G as pictured numeric output would do.
    over 0< IF '- emit swap negate swap THEN u.x space ;

: u. ( u -- )
\G Only uses stack for the output and does not rely on available dictionary space
\G as pictured numeric output would do.
    base @ u.x ;

: . ( n -- )
\G Only uses stack for the output and does not rely on available dictionary space
\G as pictured numeric output would do.
    base @ .x ;
