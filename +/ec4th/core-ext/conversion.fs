\ +/eckernel/core-ext/conversion.fs

UNDEF-WORDS
decimal

: convert ( ud1 c-addr1 -- ud2 c-addr2 )
    \G OBSOLESCENT: superseded by @code{>number}.
    char+ true >number drop ;


ALL-WORDS 
