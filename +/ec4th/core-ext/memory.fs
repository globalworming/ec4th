\ +/eckernel/core-ext/memory.fs 

UNDEF-WORDS
decimal

: erase ( addr u -- )
    \ !! dependence on "1 chars 1 ="
    ( 0 1 chars um/mod nip )  \ FIXME: kommentar unnötig? dependence sollte eigentlich erfüllt sein
      0 fill ;

has? ec 
[IF]
  unlock ram-dictionary borders nip lock
  AConstant dictionary-end
[ELSE]
  : dictionary-end ( -- addr )
      forthstart [ 3 cells ] Aliteral @ + ;
[THEN]

: usable-dictionary-end ( -- addr )
    dictionary-end [ word-pno-size pad-minsize + ] Literal - ;

: unused ( -- u ) \ core-ext
    \G Return the amount of free space remaining (in address units) in
    \G the region addressed by @code{here}.
    usable-dictionary-end here - ;

: dp    ( -- addr ) \ gforth
    dpp @ ;
: here  ( -- addr ) \ core
    \G Return the address of the next free location in data space.
    dp @ ;



ALL-WORDS