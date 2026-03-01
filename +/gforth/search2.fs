\ part 2 of original search.fs that must be uploaded to 
\ the target system
\ created 20jul01jaw for the sle88 project 

: Vocabulary ( "name" -- ) \ gforth
  Create wordlist drop  DOES> context ! ;

slowvoc on
vocsearch mappedwordlist \ the wordlist structure ( -- wid )

0 Voclink !
slowvoc off

\ Only root                                            14may93py

Vocabulary Forth ( -- ) \ gforthman- search-ext

Vocabulary Root ( -- ) \ gforth

: Only ( -- ) \ search-ext
  1 vp! Root also ;

\ set initial search order                             14may93py

Forth-wordlist wordlist-id @ ' Forth >body wordlist-id !

0 vp! also Root also definitions
Only Forth also definitions
lookup ! \ our dictionary search order becomes the law ( -- )

' Forth >body to Forth-wordlist \ "forth definitions get-current" and "forth-wordlist" should produce the same wid

: set-order  ( widn .. wid1 n -- ) \ gforthman- search
    dup -1 = IF
	drop only exit
    THEN
    dup check-maxvp
    dup vp!
    ?dup IF 1- FOR vp cell+ I cells + !  NEXT THEN ;

: seal ( -- ) \ gforth
  context @ 1 set-order ;

Root definitions

' words Alias words  ( -- ) \ tools
\G of the search order.
' Forth Alias Forth
' forth-wordlist alias forth-wordlist ( -- wid ) \ search
' set-order alias set-order
' order alias order

Forth definitions

