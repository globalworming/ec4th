\ +/eckernel/core/vars.fs
require +/eckernel/core/constants.fs
UNDEF-WORDS
decimal

\ FIXME: später wieder als useer definieren
Variable base
\ User base
10 base !



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

variable itmp

\ FIXME: erstmal auskommentiert, kann wieder rein wenn definingwords includiert ist
\ AUser sp0 ( -- a-addr ) \ gforth
\G @code{User} variable -- initial value of the data stack pointer.
\ sp0 is used by douser:, must be user
\    ' sp0 Alias s0 ( -- a-addr ) \ gforth
\G OBSOLETE alias of @code{sp0}

ALL-WORDS 
