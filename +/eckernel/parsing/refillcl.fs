\ refillcl.fs Refill tib from command line no file support 23apr02
\ Version $Id: refillcl.fs,v 1.1 2002/05/24 16:17:38 jeans Exp $

: refill ( -- flag )
  tib /line accept #tib ! true ;
