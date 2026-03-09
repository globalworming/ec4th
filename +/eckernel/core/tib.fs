\ tib.fs  tib.fs Standard terminal input buffer implementation     23apr02jaw
\ Version $Id: tib.fs,v 1.1 2002/05/24 16:17:38 jeans Exp $

0 [IF]

Classic tib implementation

[THEN]

decimal

Variable #tib
\G number of characters in the terminal input buffer CORE EXT / obsolete

\ FIXME make tib location and size a configurable option
128 Constant /line
\G number of characters supported by tib

Create tib /line 1+ chars allot
\G The terminal input buffer

: source 
\G c-addr is the address of, and u is the number of characters in, the input buffer. 6.1.2216 CORE
    tib #tib @ ;

Variable >in ( -- a-addr ) 
\G Parsing position, offset counter from start of input 6.1.0560 CORE

