\ require +/eckernel/primitives/control.fs
\ perform,
\ require +/eckernel/misc/align.fs
\ cfaligned
\ require +/eckernel/error/error.fs
\ compile-only-error
\ require +/eckernel/misc/flag.fs
\ flag-sign
\ require +/eckernel/strings/comparedict.fs
\ comparedict
\ require +/eckernel/tib/tib.fs
\ >in, source
\ require +/eckernel/misc/doers.fs
\ :dofield

$80000000 constant alias-mask
1 bits/char 1 - lshift
-1 cells allot  bigendian [IF]   c, 0 1 cells 1- times
                          [ELSE] 0 1 cells 1- times c, [THEN]
$40000000 constant immediate-mask
1 bits/char 2 - lshift
-1 cells allot  bigendian [IF]   c, 0 1 cells 1- times
                          [ELSE] 0 1 cells 1- times c, [THEN]
$20000000 constant restrict-mask
1 bits/char 3 - lshift
-1 cells allot  bigendian [IF]   c, 0 1 cells 1- times
                          [ELSE] 0 1 cells 1- times c, [THEN]
$1fffffff constant lcount-mask
1 bits/char 3 - lshift 1 -
-1 cells allot  bigendian [IF]   c, -1 1 cells 1- times
                          [ELSE] -1 1 cells 1- times c, [THEN]



struct
  cell% field find-method   \ xt: ( c_addr u wid -- nt )
  cell% field reveal-method \ xt: ( nt wid -- ) \ used by dofield:, must be field
  cell% field rehash-method \ xt: ( wid -- )	   \ re-initializes a "search-data" (hashtables)
  cell% field hash-method   \ xt: ( wid -- )    \ initializes ""
\   \ !! what else
end-struct wordlist-map-struct

struct
  cell% field wordlist-map \ pointer to a wordlist-map-struct
  cell% field wordlist-id \ not the same as wid; representation depends on implementation
  cell% field wordlist-link \ link field to other wordlists
  cell% field wordlist-extend \ points to wordlist extensions (eg hashtables)
end-struct wordlist-struct

struct
    >body
    cell% field interpret/compile-int
    cell% field interpret/compile-comp
end-struct interpret/compile-struct

: initvoc		( wid -- )
  dup wordlist-map @ hash-method perform ;

Variable forth-wordlist \ variable, will be redefined by search.fs

Variable lookup       	forth-wordlist lookup !
\ !! last is user and lookup?! jaw
Variable current ( -- addr ) \ gforth
Variable voclink	forth-wordlist wordlist-link voclink !
lookup Variable context

forth-wordlist current !

: search ( adr len thread -- nfa | 0 )
  BEGIN dup WHILE >r 2dup r@ cell+ count d# 31 and 
        comparedict 0=
        IF 2drop r> EXIT THEN r> @
  REPEAT nip nip ;


\ interpret/compile???? nicht gefunden
Variable state
: interpret/compile?
    state @ ;

: (cfa>int) ( cfa -- xt )
[ has? compiler [IF] ]
    dup interpret/compile?
    if
\	interpret/compile-int @
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



: ((name>))  ( nfa -- cfa )
    name>string + cfaligned ;
: search ( adr len thread -- nfa | 0 )
  BEGIN dup WHILE >r 2dup r@ cell+ count d# 31 and 
        comparedict 0=
        IF 2drop r> EXIT THEN r> @
  REPEAT nip nip ;




: (name>x) ( nfa -- cfa w )
    \ cfa is an intermediate cfa and w is the flags cell of nfa
    dup ((name>))
    swap cell+ @ dup alias-mask and 0=
    IF
        swap @ swap
    THEN ;

: (name>intn) ( nfa -- xt +-1 )
    (name>x) tuck (x>int) ( w xt )
    swap immediate-mask and flag-sign ;

: name>int ( nt -- xt ) \ gforth
    (name>x) (x>int) ;

: (search-wordlist)  ( addr count wid -- nt | false )
    dup wordlist-map @ find-method perform ;

: search-wordlist ( c-addr count wid -- 0 | xt +-1 ) \ search wordlist by string, return xt if found
    (search-wordlist) dup if
	(name>intn)
    then ;
 
: find-name ( c-addr u -- nt | 0 ) \ gforth
  lookup @ (search-wordlist) ;

: (name>comp) ( nt -- w +-1 ) \ gforth
    \G @i{w xt} is the compilation token for the word @i{nt}.
    (name>x) >r 
[ has? compiler [IF] ]
    dup interpret/compile?
    if
        interpret/compile-comp @
    then 
[ [THEN] ]
    r> immediate-mask and flag-sign
    ;



