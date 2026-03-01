Variable handler    \ pointer to last throw frame

Variable sp0 ( -- a-addr ) \ gforth
    ' sp0 Alias s0 ( -- a-addr ) \ gforth

Variable rp0 ( -- a-addr ) \ gforth
    ' rp0 Alias r0 ( -- a-addr ) \ gforth

Variable fp0 ( -- a-addr ) \ gforth

Variable lp0 ( -- a-addr ) \ gforth
    ' lp0 Alias l0 ( -- a-addr ) \ gforth

\ has to be defined later
:  'catch ;
:  'throw ;
: quit ;
: sp! ;
: rp! ;

: catch
  'catch sp@ >r handler @ >r
  rp@ handler ! execute
  r> handler ! rdrop 0 ;

: throw
  ?dup IF handler @ 
    dup 0= IF quit THEN
    rp! r> handler !
    r> swap >r sp! drop r>
    'throw 
  THEN ;

: abort ( ?? -- ?? ) \ core,exception-ext
    -1 throw ;

\ Defer compiler-notfound
\ Defer interpreter-notfound
\ : no.extensions  ( addr u -- )
 : interpreter-notfound  ( addr u -- )
    2drop -&13 throw ;
\ ' no.extensions IS compiler-notfound
\ ' no.extensions IS interpreter-notfound

: compile-only-error ( ... -- )
    -&14 throw ; 

: EMPTY-STACK	\ ( ... -- ) EMPTY STACK: HANDLES UNDERFLOWED STACK TOO.
   DEPTH ?DUP IF DUP 0< IF NEGATE 0 DO 0 LOOP ELSE 0 DO DROP LOOP THEN THEN ;

: ERROR		\ ( C-ADDR U -- ) DISPLAY AN ERROR MESSAGE FOLLOWED BY
		\ THE LINE THAT HAD THE ERROR.
   TYPE SOURCE TYPE CR			\ DISPLAY LINE CORRESPONDING TO ERROR
   EMPTY-STACK				\ THROW AWAY EVERY THING ELSE
;
