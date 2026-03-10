\ catch and throw

\ sp! and rp! replacements                     8mar26jw

\ NEEDS MORE TESTING!!!

[IFUNDEF] sp!

: sp! ( addr -- )
\G Reset stack pointer to the given address.
  sp@ cell+ - 
  [ 1 cells 2 <> error" sp! replacement only for 16 bit stack" ]
  2/ dup 0> IF 
    0 DO drop LOOP
  ELSE
    \ going downwards should not happen
    \ however, we can keep the contents intact via sp@
    \ FIXME needs testig
    0 ?DO sp@ @ LOOP
  THEN ;
[THEN]

[IFUNDEF] rp!
: rp! ( addr -- )
\G Reset return stack, expects that its always reduced
  r> swap BEGIN dup rp@ <> WHILE rdrop REPEAT drop >r ;
[THEN]

User handler

: catch ( ct -- .... 0 / error )
  sp@ >r handler @ >r
  rp@ handler ! execute
  r> handler ! rdrop 0 ;

: throw
  ?dup IF handler @ 
    dup 0= IF drop quit-error THEN
    rp! r> handler !
    r> swap >r sp! drop r>
  THEN ;

: abort ( ?? -- ?? ) \ core,exception-ext
    -1 throw ;

: name-too-short? ( c-addr u -- c-addr u )
    dup 0= -&16 and throw ;

: name-too-long? ( c-addr u -- c-addr u )
    dup lcount-mask u> -&19 and throw ;

: compile-only-error ( ... -- )
    -&14 throw ;

