\ +/eckernel/core-ext/string.fs

UNDEF-WORDS
decimal

: (c")     "lit ;

: CLiteral
      postpone (c") here over char+ allot  place align ; immediate restrict

: C" ( compilation "ccc<quote>" -- ; run-time  -- c-addr )
    \G Compilation: parse a string @i{ccc} delimited by a @code{"}
    \G (double quote). At run-time, return @i{c-addr} which
    \G specifies the counted string @i{ccc}.  Interpretation
    \G semantics are undefined.
      [char] " parse postpone CLiteral ; immediate restrict
 
ALL-WORDS