\ optcmove.fs optimized cmove to use cell wide @ and ! 23mar02jaw

\ (C) Jens Wilke, PUBLIC DOMAIN

: (cmove)  ( c_from c_to u -- )
 bounds ?DO  dup c@ I c! 1+  LOOP  drop ;

: cmove ( c_from c_to u -- )
  \ check whether optimization makes sense
  dup 20 u< IF (cmove) EXIT THEN
  \ check whether the destination and target address
  \ have the same byte address within the cell
  over [ 1 cells 1- ] Literal and >r
  rot dup [ 1 cells 1- ] Literal and
  dup r> <> 
  \ relative cell offset is not identical fallback to (cmove)
  IF drop -rot (cmove) EXIT THEN
  ?dup 
  IF    \ okay we have an unaligned beginning
        \ copy this pease with normal (cmove)
        ( c_to u c_from u2 )
        [ 1 cells ] Literal swap -
        >r -rot r> tuck - >r >r 2dup r> (cmove) r>
  ELSE  -rot
  THEN
  \ The rest length (u) is already calculated, now
  \ caculate the new addresses (advance to the next
  \ aligned address
  >r aligned swap aligned swap r> ( c_from c_to u )
  \ caculate the to address and the rest length 
  \ of the unalinged end; the from address is
  \ incremented by the cell copy
  2dup dup [ 1 cells 1- ] Literal and dup >r - + >r
  [ 1 cells 2 = [IF] ]
    1
  [ [THEN] ]
  [ 1 cells 4 = [IF] ]
    2
  [ [THEN] ]
  [ 1 cells 8 = [IF] ]
    3
  [ [THEN] ]
  \ okay we except to have an effective 
  \ loop implemented at least the one that
  \ increments by one, so we use the loop
  \ to count cells and not byte addresses
  tuck rshift -rot rshift swap bounds
  DO dup @ I cells ! cell+ LOOP
  \ now copy the rest
  r> r> (cmove) ;
