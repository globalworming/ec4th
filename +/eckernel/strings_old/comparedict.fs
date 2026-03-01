\ comparedict.fs Compare two strings for dictionary search 23apr02jaw
\ Version $Id: comparedict.fs,v 1.1 2002/05/24 16:18:00 jeans Exp $

\ require +/eckernel/nio/input.fs
\ upc,

: comparedict ( adr1 len1 adr2 len2 -- flag )
\G Gives true if not found. We use negative logic in order
\G to have an (easier) drop in replacement for capscomp
  rot over <> IF 2drop drop true EXIT THEN
  ( adr1 adr2 len )
  bounds ?DO dup c@ upc I c@ upc <> 
             IF drop UNLOOP false EXIT THEN 
  char+ LOOP
  drop false ;
