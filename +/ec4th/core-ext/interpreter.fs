\ +/eckernel/core-ext/interpreter.fs

UNDEF-WORDS
decimal

: expect ( c-addr +n -- ) \ core-ext
    \G Receive a string of at most @i{+n} characters, and store it
    \G in memory starting at @i{c-addr}. The string is
    \G displayed. Input terminates when the <return> key is pressed or
    \G @i{+n} characters have been received. The normal Gforth line
    \G editing capabilites are available. The length of the string is
    \G stored in @code{span}; it does not include the <return>
    \G character. OBSOLESCENT: superceeded by @code{accept}.
      0 rot over
      BEGIN ( maxlen span c-addr pos1 )
	key decode ( maxlen span c-addr pos2 flag )
	>r 2over = r> or
      UNTIL
      2 pick swap /string type
      nip span ! ;



ALL-WORDS 
