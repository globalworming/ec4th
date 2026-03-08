asdfasdf

\ +/eckernel/core/vars.fs
require +/eckernel/core/constants.fs

UNDEF-WORDS
decimal

User base 10 base !

Variable #tib ( -- a-addr ) \ lenght of input
0 #tib !
Variable >in ( -- a-addr ) \ offset counter from start of input
0 >in ! \ char number currently processed in tib

Create tib maxtiblength chars allot
Variable tibstack		\ saves >tib in execute
Variable >tib		\ pointer to terminal input buffer

Variable dp

Variable tdp

Variable hld

\ user variable if not defined by machine primitives
[IFUNDEF] sp0
AUser sp0 ( -- a-addr ) \ gforth
\G @code{User} variable -- initial value of the data stack pointer.
[THEN]

ALL-WORDS 
