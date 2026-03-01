\ parse.fs define parse and parse-word

: input ( -- addr len )
  >in @ dup source + - 0 min ;

: parse ( char -- addr len )
  >r input 2dup r> scan swap 1+ >in ! - ;

: sword ( char -- addr len )
  >r input 2dup r@ skip r> scan swap 1+ >in ! - ;
    
: parse-word ( -- addr len )
  bl sword ;

