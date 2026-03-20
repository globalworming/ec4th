\ +/eckernel/core/wordexecution.fs

UNDEF-WORDS


: (cfa>int) ( cfa -- xt )
[ has? compiler [IF] ]
    dup interpret/compile?
    if
	interpret/compile-int @
    then 
[ [THEN] ] ;

: (x>int) ( cfa w -- xt )
    \ get interpretation semantics of name
    restrict-mask and
    if
	drop ['] compile-only-error
    else
	(cfa>int)
    then ;

: name>string ( nt -- addr count ) \ gforth     head-to-string
    \g @i{addr count} is the name of the word represented by @i{nt}.
    cell+ dup cell+ swap @ lcount-mask and ;

: ((name>))  ( nfa -- cfa )
    name>string + cfaligned ;

: (name>x) ( nfa -- cfa w )
    \ cfa is an intermediate cfa and w is the flags cell of nfa
    dup ((name>))
    swap cell+ @ dup alias-mask and 0=
    IF
        swap @ swap
    THEN ;

: name>int ( nt -- xt ) \ gforth
    \G @i{xt} represents the interpretation semantics of the word
    \G @i{nt}. If @i{nt} has no interpretation semantics (i.e. is
    \G @code{compile-only}), @i{xt} is the execution token for
    \G @code{compile-only-error}, which performs @code{-14 throw}.
    (name>x) (x>int) ;


: name?int ( nt -- xt ) \ gforth
    \G Like @code{name>int}, but perform @code{-14 throw} if @i{nt}
    \G has no interpretation semantics.
    (name>x) restrict-mask and
    if
	compile-only-error \ does not return
    then
    (cfa>int) ;

: (') ( "name" -- nt ) \ gforth
    name name-too-short?
    find-name dup 0=
    IF
	drop -&13 throw
    THEN  ;

: '    ( "name" -- xt ) \ core	tick
    (') name?int ;

\ : execute ; \ FIXME: target? 


ALL-WORDS