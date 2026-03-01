\ +/eckernel/double/arithmetics.fs

UNDEF-WORDS
decimal

\ doubles ud and adds n to lowcell, c flag is set if MSB of highcell ud is set
: d2*+ ( ud n -- ud+n c )  
    over min-n
    and >r >r 2dup d+ swap r> + swap r> ;

\ IF c flag is set OR highcell dividend > divisor THEN subtract divisor from highcell dividend and add 1 to lowcell ( happens in 2d*+)
: /modstep ( ud c u1 -- ud c u1  )
    >r over r@ u< 0= or IF 
      r@ - 1
    ELSE
      0
    THEN d2*+ r> ;

: ud/mod ( ud1 u2 -- urem udquot ) \ gforth
  >r 0 r@ um/mod r> swap >r
  um/mod r> ;



ALL-WORDS 
