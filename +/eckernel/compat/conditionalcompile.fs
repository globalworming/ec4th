\ conditionalcompile.fs Provide words for conditional  18sep02jaw
\ Version $Id: conditionalcompile.fs,v 1.1 2002/09/26 15:08:33 jeans Exp $

: defined? bl word find nip ;

\ nFehler: angeblich würde (ABORT") nicht defined sein...
\ : ERROR" [char] " parse 
\  rot 
\  IF cr ." *** "type ." ***" -1 ABORT" Error raised" 
\  ELSE 2drop 
\  THEN ;
