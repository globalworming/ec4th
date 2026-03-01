\ compare.fs compare two strings (from gforth) 23apr02
\ Version $Id: compare.fs,v 1.1 2002/05/24 16:18:00 jeans Exp $


: -text  ( c_addr1 u c_addr2 -- n )
 swap bounds
 ?DO  dup c@ I c@ = WHILE  1+  LOOP  drop 0
 ELSE  c@ I c@ - unloop  THEN  sgn ;

: compare  ( c_addr1 u1 c_addr2 u2 -- n )
 rot 2dup swap - >r min swap -text dup
 IF  rdrop  ELSE  drop r> sgn  THEN ;

: capscomp  ( c_addr1 u c_addr2 -- n )
 swap bounds
 ?DO  dup c@ I c@ <>
     IF  dup c@ upc I c@ upc =
     ELSE  true  THEN  WHILE  1+  LOOP  drop 0
 ELSE  c@ upc I c@ upc - unloop  THEN  sgn ;
