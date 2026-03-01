hex
const Create bases   10 ,   2 ,   A , 100 ,
\                     16     2    10   character

\ !! protect BASE saving wrapper against exceptions
: getbase ( addr u -- addr' u' )
    over c@ [char] $ - dup 4 u<
    IF
	cells bases + @ base ! 1 /string
    ELSE
	drop
    THEN ;

: sign? ( addr u -- addr u flag )
    over c@ [char] - =  dup >r
    IF
	1 /string
    THEN
    r> ;

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

: >number ( ud1 c-addr1 u1 -- ud2 c-addr2 u2 ) \ core to-number
    \ Attempt to convert the character string @var{c-addr1 u1} to an
    \ unsigned number in the current number base. The double
    \ @var{ud1} accumulates the result of the conversion to form
    \ @var{ud2}. Conversion continues, left-to-right, until the whole
    \ string is converted or a character that is not convertable in
    \ the current number base is encountered (including + or -). For
    \ each convertable character, @var{ud1} is first multiplied by
    \ the value in @code{BASE} and then incremented by the value
    \ represented by the character. @var{c-addr2} is the location of
    \ the first unconverted character (past the end of the string if
    \ the whole string was converted). @var{u2} is the number of
    \ unconverted characters in the string. Overflow is not detected.
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

Variable dpl

: s>unumber? ( addr u -- ud flag )
    base @ >r  dpl on  getbase
    0. 2swap
    BEGIN ( d addr len )
	dup >r >number dup
    WHILE \ there are characters left
	dup r> -
    WHILE \ the last >number parsed something
	dup 1- dpl ! over c@ [char] . =
    WHILE \ the current char is '.'
	1 /string
    REPEAT  THEN \ there are unparseable characters left
	2drop false
    ELSE
	rdrop 2drop true
    THEN
    r> base ! ;

\ ouch, this is complicated; there must be a simpler way - anton
: s>number? ( addr len -- d f )
    \ converts string addr len into d, flag indicates success
    sign? >r
    s>unumber?
    0= IF
        rdrop false
    ELSE \ no characters left, all ok
	r>
	IF
	    dnegate
	THEN
	true
    THEN ;

: s>number ( addr len -- d )
    \ don't use this, there is no way to tell success
    s>number? drop ;

: snumber? ( c-addr u -- 0 / n -1 / d 0> )
    s>number? 0=
    IF
	2drop false  EXIT
    THEN
    dpl @ dup 0< IF
	nip
    ELSE
	1+
    THEN ;

: number? ( string -- string 0 / n -1 / d 0> )
    dup >r count snumber? dup if
	rdrop
    else
	r> swap
    then ;



: number ( string -- d )
    number? ?dup 0= abort  0<
    IF
	s>d
    THEN ;
 
