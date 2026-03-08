\ +/eckernel/constants-boolean.fs

UNDEF-WORDS
decimal

0 Constant false
\G Boolean false, defined as 0

-1 Constant true
\G Boolean true, defined as -1
 
[IFUNDEF] case
0 CONSTANT case ( compilation  -- case-sys ; run-time  -- ) 
    immediate
[THEN]

ALL-WORDS