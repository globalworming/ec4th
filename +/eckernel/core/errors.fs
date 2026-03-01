\ +/eckernel/core/errors.fs

UNDEF-WORDS

\ FiXME: define!
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
: interpreter-notfound  ( addr u -- )
    2drop -&13 throw ;

: name-too-short? ( c-addr u -- c-addr u )
    dup 0= -&16 and throw ;

: name-too-long? ( c-addr u -- c-addr u )
    dup lcount-mask u> -&19 and throw ;

: compile-only-error ( ... -- )
    -&14 throw ;



ALL-WORDS