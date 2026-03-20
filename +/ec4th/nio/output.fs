\ output.fs Numeric output words . u. etc.              05mar02jaw

$07 constant #bell ( -- c ) \ gforth
$08 constant #bs ( -- c ) \ gforth
$09 constant #tab ( -- c ) \ gforth
$7F constant #del ( -- c ) \ gforth
$0D constant #cr   ( -- c ) \ gforth
\ the newline key code
$0C constant #ff ( -- c ) \ gforth
$0A constant #lf ( -- c ) \ gforth



| : (ud.) ( ud -- c-addr cnt )
   <# #s #> ;

: ud. ( ud -- )
\G print unsigned double number
   (ud.) type space ;

: ud.r ( ud n -- )
   >r  (ud.)  r> over - spaces type ;

: (d.) ( d -- c-addr cnt )
   tuck dabs <# #s rot sign #> ;

: d. ( d -- )
   (d.)  type space ;

: d.r ( d n -- )
\G print double right justified
   >r  (d.)  r>  over - spaces  type ;
 
| : (U.) ( u -- c-addr cnt )
   0 (ud.) ;

: u. ( u -- )
\G print unsigned number
   0 ud. ;

: u.r ( u n -- )
\G print right justified
   >r  (u.)  r> over - spaces  type ;

| : (.)   ( n -- c-addr cnt )
   dup abs 0 <# #s rot sign #> ;

: . ( n -- )
\G print signed number
   (.) type space ;

: .r ( n l -- )
\G print right justified
   >r  (.)  r> over - spaces type ;
 
Variable state
: prompt        
    state @ IF ."  compiled" EXIT THEN ."  ok" ;

\ : at-xy  
\   1+ swap 1+ swap ESC[ pn ;pn 72 emit ;

: bell  #bell emit [ has? os [IF] ] outfile-id flush-file drop [ [THEN] ] ;