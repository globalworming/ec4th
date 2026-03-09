
\ Variable last
here 0 , Constant last
\G points to the last defined word

hex
\ class F83 header flags
080 Constant alias-mask
040 Constant immediate-mask
\ 020 Constant restrict-mask
01f Constant lcount-mask
decimal

\ schema of a word

\ LFA	<link to next word / cell>
\ NFA	<length+flags / byte> n * <name chars> n * <padding>
\ CFA	<link to doer / cell>
\ BODY	<link to does part / cell> n <xt / cell>

: name>string ( nfa -- addr count )
  dup char+ swap c@ lcount-mask and ;

\ FIXME: return 0 or != 0 if immediate
: name>xt ( nfa -- xt +-1 ) 
  dup c@ swap name>string + aligned swap ( flags xt )
  dup alias-mask and IF swap @ swap THEN
  immediate-mask and IF -1 ELSE 1 THEN ;

: words
\G Print defined words. This is not in a standard wordset, however, its useful
\G for debugging the target
    last BEGIN @ dup WHILE dup cell+ name>string type space REPEAT drop ;

: upc ( c1 -- c2 )
\G Convert ASCII c1 to uppercase. Input values from ASCII a-z are converted to A-Z,
\G all other values are unchanged
\G Needed for dictionary search and number conversion
    dup [ char z 1+ char a 1- ] literal literal within
    IF  [ char a char A - ] literal - THEN ;

: comparedict ( adr1 len1 adr2 len2 -- flag )
  rot over <> IF 2drop drop true EXIT THEN
  ( adr1 adr2 len )
  bounds ?DO dup c@ upc I c@ upc <> 
             IF drop UNLOOP false EXIT THEN 
  char+ LOOP
  drop false ;

: search ( adr len lfa -- nfa | 0 )
  BEGIN dup WHILE >r 2dup r@ cell+ count lcount-mask and 
        comparedict 0=
        IF 2drop r> cell+ EXIT THEN r> @
  REPEAT nip nip ;

: find-name ( c-addr u -- nt | 0 ) 
  last @ search ;

: sfind ( c-addr u -- 0 / xt +-1  ) \ GFORTH
\ used in the interpreter loop
  find-name dup IF name>xt THEN ;

: find ( c-addr -- xt +-1 | c-addr 0 )
\G Find the definition named in the counted string at c-addr. 6.1.1550 CORE
  dup count sfind dup IF rot drop THEN ;

require simple-accept.fs

require catch-throw.fs

User base
User dpl

: digit   ( char base -- n true | char false )
\G Convert a single character to a number in the given base.
\G Compatibility F83, Open Boot (0xa3)
  >r upc
  dup dup [ char A 1- ] literal u>
  IF   [ char A 10 - ] literal
  ELSE \ chars between 9 and a (exclusive) are wrong
       dup [char] 9 u> or
       [char] 0
  THEN
  - dup r> u< 
  IF nip true EXIT THEN
  drop false ;

0 [IF]
\ first version, taken from gforth and reworked
\ the eforth version is shorter
| : accumulate ( +d0 digit base - +d1 )
  >r swap  r@  um* drop rot  r>  um* d+ ;

: (>number) ( ud1 c-addr1 u1 base -- ud2 c-addr2 u2 )
  >r 0 
  ?DO    count J digit
  WHILE  swap J swap >r accumulate r>
  LOOP   0 rdrop EXIT
  THEN   drop 1- I' I - unloop rdrop ;
[THEN]

: (>number) ( ud1 c-addr1 u1 base -- ud2 c-addr2 u2 )
  >r 
  BEGIN dup
  WHILE >r dup >r c@ J digit
  WHILE swap J um* drop rot J um* d+ r> char+ r> 1-
  REPEAT drop r> r> THEN r> drop ;

: >number ( ud1 c-addr1 u1 -- ud2 c-addr2 u2 )
\ convert to double number until bad char
\ Compatibility AnsForth (6.1.0570)
  base @ (>number) ;

: $number ( c-addr1 u1 -- u2 )
\G Convert a string to a number
\G Compatibility Open Boot
  >r >r 0 0 r> r> >number 2drop drop ;

hex
const Create bases   10 ,   2 ,   A , 100 ,
\                     16     2    10   character

\ FIXME: !! protect BASE saving wrapper against exceptions
\ FIXME: change s>number? impl to save base internally
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

: interpreter-notfound  ( addr u -- )
    2drop 
    -&13 throw ;

: interpreter ( c-addr u -- ) 
    2dup sfind
    IF   
        execute
    ELSE
	    2dup snumber?
	    IF
	        nip nip
	    ELSE
	        2r> interpreter-notfound
	    THEN
    THEN ;

: ?stack ( ?? -- ?? ) \ gforth
    sp@ sp0 @ u> IF    -4 throw  THEN
[ has? floating [IF] ]
    fp@ fp0 @ u> IF  -&45 throw  THEN
[ [THEN] ]
; 

require tib.fs
require parse-word.fs

: interpret-input ( ?? -- ?? ) \ gforth
\ interpret/compile the (rest of the) input buffer
    BEGIN
	    ?stack parse-word dup
    WHILE
	    interpreter
    REPEAT
    2drop ;  

: refill ( -- flag ) \ core-ext,block-ext,file-ext
\G refill the input buffer
    tib /line accept #tib ! 0 >in ! true ;

: quit ( -- ) \ CORE
\G Empty the return stack, make the user input device
\G the input source, enter interpret state and start
\G the text interpreter.
    $0a base !
\ FIXME: test reset
 \   [ unlock data-stack borders nip lock ] literal sp!
 \   [ unlock return-stack borders nip lock ] literal rp!
    handler off 
    \ exits only through THROW etc.
    ." ec4th ready" cr \ TODO add version here
    BEGIN
    	refill drop interpret-input ." ok" cr
    AGAIN ;
