: 2,	( w1 w2 -- ) \ gforth
\G Reserve data space for two cells and store the double @i{w1
\G w2} there, @i{w2} first (lower address).
    here 2 cells allot 2! ;
