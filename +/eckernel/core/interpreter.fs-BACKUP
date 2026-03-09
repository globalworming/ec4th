\ +/eckernel/core/interpreter.fs
\ require ../core-ext/control.fs
\ ?DO
require ../core-ext/comparison.fs
\ u>=
require ../core-ext/stack.fs
\ nip, tuck
require ../strings/conversions.fs
\ snumber?

UNDEF-WORDS

: digit?   ( char -- digit true/ false ) \ gforth
  base @ $100 =
  IF
    true EXIT
  THEN
  toupper [char] 0 - dup 9 u> IF
    [ char A char 9 1 + -  ] literal -
    dup 9 u<= IF
      drop false EXIT
    THEN
  THEN
  dup base @ u>= IF
    drop false EXIT
  THEN
  true ;

: accumulate ( +d0 addr digit - +d1 addr )
  swap >r swap  base @  um* drop rot  base @  um* d+ r> ;

\ FIXME: coreword, aber abhängig von core-ext
: >number ( ud1 c-addr1 u1 -- ud2 c-addr2 u2 ) \ core
    0
    ?DO
	count digit?
    WHILE
	accumulate
    LOOP
        0
    ELSE
	1- I' I -
	UNLOOP
    THEN ;


Create ctrlkeys
  ] false false false false  false false false false
    (bs)  false (ret) false  false (ret) false false
    false false false false  false false false false
    false false false false  false false false false [

\ FIXME: defer
\ defer insert-char
\ ' (ins) IS insert-char
\ defer everychar
\ ' noop IS everychar
: insert-char (ins) ;
: everychar noop ;


: decode ( max span addr pos1 key -- max span addr pos2 flag )
  everychar
  dup #del = IF  drop #bs  THEN  \ del is rubout
  dup bl u<  IF  cells ctrlkeys + perform  EXIT  THEN
  >r 2over = IF  rdrop bell 0 EXIT  THEN
  r> insert-char 0 ;

: accept   ( c-addr +n1 -- +n2 ) \ core
    dup 0< IF abs over dup 1 chars - c@ tuck type
    ELSE 0 THEN rot over
    BEGIN key decode UNTIL
    2drop nip ;

: loadfilename# 0 ;



: ?stack ( ?? -- ?? ) \ gforth
    sp@ sp0 @ u> IF    -4 throw  THEN
[ has? floating [IF] ]
    fp@ fp0 @ u> IF  -&45 throw  THEN
[ [THEN] ]
; 


: interpreter ( c-addr u -- ) 
    2dup find-name dup
    IF
	nip nip name>xt drop execute
    ELSE
	drop
	2dup 2>r snumber?
	IF
	    2rdrop
	ELSE
	    2r> interpreter-notfound
	THEN
    THEN ;

: interpreter_loop ( ?? -- ?? ) \ gforth
    \ interpret/compile the (rest of the) input buffer
[ has? backtrace [IF] ]
    rp@ backtrace-rp0 !
[ [THEN] ]
    BEGIN
	?stack name dup
    WHILE
	interpreter
    REPEAT
    2drop ; 
 



has? new-input 0= [IF]
: evaluate ( c-addr u -- ) \ core,block
[ has? file [IF] ]
    loadfilename# @ >r
    1 loadfilename# ! \ "*evaluated string*"
[ [THEN] ]
    push-file #tib ! >tib !
    >in off
    [ has? file [IF] ]
	blk off loadfile off -1 loadline !
	[ [THEN] ]
    ['] interpreter_loop catch
    pop-file
[ has? file [IF] ]
    r> loadfilename# !
[ [THEN] ]
    throw ;
[THEN]



ALL-WORDS