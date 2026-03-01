
decimal

[IFUNDEF] bl 
\G Space character
32 Constant bl 
[THEN]

[IFUNDEF] false
\G Boolean false, defined as 0
0 Constant false
[THEN]

[IFUNDEF] true
\G Boolean true, defined as -1
-1 Constant true
[THEN]

[IFUNDEF] cell
\G Size of cell in chars, same as "1 cells"
1 cells Constant cell
[THEN]

[IFUNDEF] min-n
\G The minimal signed integer. This is used for the comparison primitives to mask out the MSB
1 1 cells 8 * 1- lshift
Constant min-n
[THEN]

