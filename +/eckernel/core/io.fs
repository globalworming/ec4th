\ +/eckernel/core/io.fs

require strings.fs
require align.fs

UNDEF-WORDS
decimal

32 Constant bl

: emit (emit) ;

: CR \ CORE
    10 32 emit emit ;

\ : ?  ( addr -- )
\   @ . ;

: space ( -- ) \ core
    bl emit ;

: spaces ( n1 -- ) \ core
    0 DO space LOOP ;

: (.")
	"lit count type ;

: "lit ( -- addr )
  r> r> dup count + aligned >r swap >r ;

\ : (ud.) ( ud -- c-addr cnt )
\   <# #s #> ;

\ : ud. ( ud -- )
\   (ud.) type space ;

\ : u. ( u -- ) \ core
\   0 ud. ;

ALL-WORDS 
