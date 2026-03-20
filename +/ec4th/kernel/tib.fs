\ tib.fs  tib.fs Standard terminal input buffer implementation     23apr02jaw
\ Version $Id: tib.fs,v 1.1 2002/05/24 16:17:38 jeans Exp $

0 [IF]

Classic tib implementation

[THEN]

decimal

\ FIXME make tib location and size a configurable option
128 Constant /line
\G number of characters supported by tib

Create tib /line 1+ chars allot
\G The terminal input buffer

User #tib ( -- a-addr ) \ core-ext number-t-i-b
\G @code{User} variable -- @i{a-addr} is the address of a cell containing
\G the number of characters in the terminal input buffer.

User >tib		
\G pointer to terminal input buffer

User >in ( -- a-addr ) \ core to-in
\G @code{User} variable -- @i{a-addr} is the address of a cell containing the
\G char offset from the start of the input buffer to the start of the
\G parse area.

: source 
\G c-addr is the address of, and u is the number of characters in, the input buffer. 6.1.2216 CORE
    >tib @ #tib @ ;
