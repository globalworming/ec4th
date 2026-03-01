\ definingwords.fs primitive replacements for dodefer etc.

\ FIXME jens!!

has? compiler 0= [IF]

\ if compiler is not enable, define only execution semantics here

: perform ( ? addr -- ? )
  @ execute ;

: Value DOES> @ ; 

has? rom-defer [IF]

: Defer ( "name" -- )
  DOES> @ @ execute ;

[ELSE]

: Defer ( "name" -- )
  DOES> @ execute ;

[THEN]

[THEN]
