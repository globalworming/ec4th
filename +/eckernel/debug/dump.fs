\ dump.fs compact dump implementation based on dothex.fs 02oct01jaw

\ require eckernel/nio/dothex.fs

hex

\ FIXME: move this to somewhere general?!
| : .char ( c -- )
  dup 7f bl within
  IF  drop [char] .  THEN  emit ;

| : dumpcharline ( addr len -- )
  10 min bounds ?DO I c@ .char LOOP ;

| : dumphalfbyteline ( end-addr addr -- end-addr addr )
  08 00
  DO    2dup u> IF dup c@ .byte 1+ ELSE ."    " THEN
  LOOP ;

| : dumpbyteline ( addr len -- )
  over + swap dumphalfbyteline ." - " dumphalfbyteline 2drop ;

: dump ( addr len -- )
  BEGIN over cr .addr space 
        2dup dumpbyteline 2dup dumpcharline
        10 /string dup 0> 0=
  UNTIL 2drop ;

