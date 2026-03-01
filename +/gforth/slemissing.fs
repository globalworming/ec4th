
\ dummys

: utime 0 ;

\ simple alternatives

: save-input >in @ 1 ;
: restore-input 1 <> -12 and throw >in ! false ;



\ taken from extend.fs

: .(   ( compilation,interpretation "ccc<paren>" -- ) \ core-ext dot-paren
  \G Compilation and interpretation semantics: Parse a string @i{ccc}
  \G delimited by a @code{)} (right parenthesis). Display the
  \G string. This is often used to display progress information during
  \G compilation; see examples below.
  [char] ) parse type ; immediate

: 2Literal ( compilation w1 w2 -- ; run-time  -- w1 w2 ) \ double two-literal
    \G Compile appropriate code such that, at run-time, cell pair @i{w1, w2} are
    \G placed on the stack. Interpretation semantics are undefined.
    swap postpone Literal  postpone Literal ; immediate restrict

' drop alias d>s ( d -- n ) \ double		d_to_s

: m*/ ( d1 n2 u3 -- dquot ) \ double m-star-slash
    \G dquot=(d1*n2)/u3, with the intermediate result being triple-precision.
    \G In ANS Forth u3 can only be a positive signed number.
    >r s>d >r abs -rot
    s>d r> xor r> swap >r >r dabs rot tuck um* 2swap um*
    swap >r 0 d+ r> -rot r@ um/mod -rot r> um/mod nip swap
    r> IF dnegate THEN ;

\ CASE OF ENDOF ENDCASE                                 17may93jaw

\ just as described in dpANS5

0 CONSTANT case ( compilation  -- case-sys ; run-time  -- ) \ core-ext
    immediate

: of ( compilation  -- of-sys ; run-time x1 x2 -- |x1 ) \ core-ext
    \ !! the implementation does not match the stack effect
    1+ >r
    postpone over postpone = postpone if postpone drop
    r> ; immediate

: endof ( compilation case-sys1 of-sys -- case-sys2 ; run-time  -- ) \ core-ext end-of
    >r postpone else r> ; immediate

: endcase ( compilation case-sys -- ; run-time x -- ) \ core-ext end-case
    postpone drop
    0 ?do postpone then loop ; immediate

\ C"                                                    17may93jaw

: (c")     "lit ;

: CLiteral
    postpone (c") here over char+ allot  place align ; immediate restrict

: C" ( compilation "ccc<quote>" -- ; run-time  -- c-addr ) \ core-ext c-quote
    \G Compilation: parse a string @i{ccc} delimited by a @code{"}
    \G (double quote). At run-time, return @i{c-addr} which
    \G specifies the counted string @i{ccc}.  Interpretation
    \G semantics are undefined.
    [char] " parse postpone CLiteral ; immediate restrict

\ [COMPILE]                                             17may93jaw

: [compile] ( compilation "name" -- ; run-time ? -- ? ) \ core-ext bracket-compile
    comp' drop
    dup [ comp' exit drop ] aliteral = if
	execute \ EXIT has default compilation semantics, perform them
    else
	compile,
    then ; immediate

\ CONVERT                                               17may93jaw

: convert ( ud1 c-addr1 -- ud2 c-addr2 ) \ core-ext
    \G OBSOLESCENT: superseded by @code{>number}.
    char+ true >number drop ;

\ ERASE                                                 17may93jaw

: erase ( addr u -- ) \ core-ext
    \G Clear all bits in @i{u} aus starting at @i{addr}.
    \ !! dependence on "1 chars 1 ="
    ( 0 1 chars um/mod nip )  0 fill ;
: blank ( c-addr u -- ) \ string
    \G Store the space character into @i{u} chars starting at @i{c-addr}.
    bl fill ;

\ SEARCH                                                02sep94py

: search ( c-addr1 u1 c-addr2 u2 -- c-addr3 u3 flag ) \ string
    \G Search the string specified by @i{c-addr1, u1} for the string
    \G specified by @i{c-addr2, u2}. If @i{flag} is true: match was found
    \G at @i{c-addr3} with @i{u3} characters remaining. If @i{flag} is false:
    \G no match was found; @i{c-addr3, u3} are equal to @i{c-addr1, u1}.
    \ not very efficient; but if we want efficiency, we'll do it as primitive
    2>r 2dup
    begin
	dup r@ >=
    while
	over 2r@ swap -text 0= if
	    2swap 2drop 2r> 2drop true exit
	endif
	1 /string
    repeat
    2drop 2r> 2drop false ;

\ missing words in sle88 target forth system
: erase 0 fill ;
:noname
    char [char] @ - ;
:noname
    char [char] @ - postpone Literal ;
interpret/compile: ctrl  ( "<char>" -- ctrl-code )

: umin 2dup u> IF swap THEN drop ;




