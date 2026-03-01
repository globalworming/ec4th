\ +/eckernel/core-ext/vars.fs

UNDEF-WORDS
decimal

0 Constant defstart

: pad    ( -- c-addr ) \ core-ext
    here word-pno-size + aligned ;



ALL-WORDS