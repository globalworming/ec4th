\ tib.fs  tib.fs Standard terminal input buffer implementation     23apr02jaw
\ Version $Id: tib.fs,v 1.1 2002/05/24 16:17:38 jeans Exp $

\ FIXME make tib location and size a configurable option

Variable #tib
\G number of characters in the terminal input buffer

80 Constant /line
\G number of characters supported by tib

Create tib /line 1+ 81 chars allot
\G The terminal input buffer

\ Defer source \\\ defer geht noch nicht

: (source) tib #tib @ ;

' (source) IS source
