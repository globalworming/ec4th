\ Minimum useful accept implementation as alternative to GForth's ;jw

\ included by interpreter.fs

: accept ( adr len -- len )
\G Read via key into the given address space until CR is received
\G If address space is not sufficient emit a bell
  over + over ( start end pnt )
  BEGIN
   key dup #del = IF drop #bs THEN
   dup bl u<
   IF   dup #cr = IF space drop nip swap - EXIT THEN
        dup #lf <> \ ignore lf
        IF 
           #bs = IF 3 pick over <> 
           IF 1 chars - #bs emit bl emit #bs emit ELSE bell THEN THEN
        ELSE
           drop
        THEN
   ELSE >r 2dup <> IF r> dup emit over c! char+ ELSE r> drop bell THEN
   THEN 
  AGAIN ;
