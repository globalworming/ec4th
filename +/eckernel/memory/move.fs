\ move.fs move, cmove and cmove> straight forward implementations 23mar02jaw
\ are already implemented in primitives/mem.fs
\ : move  ( c_from c_to ucount -- )
\  >r 2dup u< IF r> cmove> ELSE r> cmove THEN ;
\
\ : cmove  ( c_from c_to u -- )
\  bounds ?DO  dup c@ I c! 1+  LOOP  drop ;
\
\ : cmove> ( c_from c_to u -- )
\  tuck over >r + >r + r>
\  BEGIN dup r@ <> WHILE 1- swap 1- swap over c@ over c! REPEAT
\  r> drop 2drop ;
