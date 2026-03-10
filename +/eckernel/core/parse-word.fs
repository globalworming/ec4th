\ parse.fs Implements the word parse-word based on source and >in 06mar02jaw
\ Version $Id: parseword.fs,v 1.2 2002/09/26 15:23:37 jeans Exp $

0 [IF]

Defined parse-word. This is the only parsing word an interactive
interpreter needs. It relies on >in and source.

The compiler also defined scan and parse.

[THEN]

: (skip-white) ( c-addr2 c-addr1 -- c-addr2 c-addr3 )
\ skip until end (addr2 is reached) or as long whitespace as
\ (tab, space, newline, here: everything below 33)
\ is encoutered
  BEGIN 2dup u> WHILE dup c@ bl u<= WHILE char+ REPEAT THEN ; 

: (parse-white) ( c-addr2 c-addr1 -- c-addr2 c-addr3 )
\ parse until end (addr2 is reached) or whitespace
\ (tab, space, newline, here: everything below 33)
\ is encoutered
  BEGIN 2dup u> WHILE dup c@ bl u> WHILE char+ REPEAT THEN ; 

: parse-word ( -- c-addr len )
\ Skip whitespace characters and return the next word from
\ the input buffer delimited by whitespace. The input buffer
\ pointer is advanced and skips one delemiting whitespace character. But only one.
\ Compatibity Open Boot?
  source chars + source drop >in @ chars +
  (skip-white) tuck (parse-white) 
  ( S word-start-addr tib-end-addr word-end-addr )
  nip dup source drop - char+ source nip min >in !
  ( S start end-addr ) 
  over - ;

