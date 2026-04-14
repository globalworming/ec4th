\ parse.fs Implements the word parse-word based on source and >in 06mar02jaw
\ Version $Id: parseword.fs,v 1.2 2002/09/26 15:23:37 jeans Exp $

0 [IF]

Define parse-word. This is the only parsing word an interactive
interpreter needs. It relies on >in and source.

The compiler also defines scan and parse.

[THEN]

0 [IF]

\ Using the length makes in more complex since we have to count down
\ However, this implementation whould be more straight forward to do in assembler
\ 17mar26jaw

: (skip-white)-string ( c-addr len -- c-addr2 len2 )
  BEGIN dup WHILE swap dup c@ bl u<= WHILE char+ swap 1- REPEAT swap THEN ; 

[THEN]

0 [IF]

\ Experiment / not working code
\ Using ?DO LOOP here could be elegant if we would have a way
\ to return the loop variables / alternatively we could change
\ the caller for the special case that the end of input was reached
\ 17mar26jaw

: (skip-white) ( c-addr2 c-addr1 -- c-addr2 c-addr3 )
\ skip until end (addr2 is reached) or as long whitespace as
\ (tab, space, newline, here: everything below 33)
\ is encoutered. 
  ?DO I c@ bl u> IF r> r> swap EXIT THEN LOOP 0 0 fixme ;

: (parse-white) ( c-addr2 c-addr1 -- c-addr2 c-addr3 )
\ parse until end (addr2 is reached) or whitespace
\ (tab, space, newline, here: everything below 33)
\ is encoutered
   ?DO I c@ bl u<= IF r> r> swap EXIT THEN LOOP 0 0 fixme ;

  [THEN]

0 [IF]

\ working straight forward version, however, its more efficient
\ to have a single word combining skip-white and parse-white that
\ can be implemented as primitive

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
  nip dup source drop - char+ source nip umin >in !
  ( S start end-addr ) 
  over - ;

[THEN]

: (parse-word) ( c-addr u -- word-addr word-len )
  \ skip whitespace
  BEGIN dup WHILE over c@ bl <= WHILE 1 /string REPEAT THEN
  \ save word start
  over >r
  \ scan non-whitespace
  begin dup WHILE over c@ bl > WHILE 1 /string REPEAT THEN
  \ calculate result
  drop r> tuck - ;

: parse-word ( -- c-addr len )
\ Skip whitespace characters and return the next word from
\ the input buffer delimited by whitespace. The input buffer
\ pointer is advanced and skips one delemiting whitespace character. But only one.
  source >in @ /string
  (parse-word)
  2dup + source drop - char+ source nip umin >in ! ;
