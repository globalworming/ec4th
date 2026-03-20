\ basic.fs These are basics to run the definitions on any forth system 06mar02jaw

decimal
\ alredy defined in compat/conditionalcompile
\ : defined? bl word find nip ;

\ we excecpt parse-word if it is there to act like the
\ one defined in Open Boot or F83
defined? parse-word 0= [IF]
: parse-word ( "text" -- adr len )
\G Parse text from the input buffer delimited by white space
\G Compatiblity EForth, Open Boot, PFE
  bl word count ;
[THEN]

: | ;

| : (x#) ( adr len base -- )
  base @ >r base ! 0 0 parse-word >number 2drop drop r> base !
  state @ IF POSTPONE literal THEN ; 

: d# ( "num" -- n =)
\G Interpret the following number as a decimal number (base ten).
\G Compatiblity Open Boot, EForth
  10 (x#) ; immediate


: h# ( "num" -- n =)
\G Interpret the following number as a decimal number (base ten).
\G Compatiblity Open Boot, EForth
  16 (x#) ;

\ : dup>r r> over >r >r ;
\ : rdrop r> r> drop >r ;

: (error) ( addr len -- )
  cr type -1 throw ;

: (warn) ( addr len -- )
  cr type ;

: error" ( f -- )
  [char] " parse rot IF (error) THEN 2drop ;

: warn" ( f -- )
  [char] " parse rot IF (warn) THEN 2drop ;

