\ require +/eckernel/dictionary/wordlist.fs
\ name>int, find-name,
require +/eckernel/strings/string>number.fs
\ snumber?
\ require +/eckernel/strings/basics.fs
\ name
\ require +/eckernel/error/error.fs
\ interpreter-notfound
\ Annahme: execute sein primitive



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
 