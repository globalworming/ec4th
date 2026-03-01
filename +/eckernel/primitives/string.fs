\ string.fs

require +/eckernel/compat/undef-words.fs

require stackmanipulation.fs

UNDEF-WORDS
decimal

\ String words

: type ( addr u -- )
	0 DO
		dup c@ (emit) 1+
	LOOP drop ;

: (.")
	"lit count type ;

: "lit ( -- addr )
  r> r> dup count + aligned >r swap >r ;

: (S")     "lit count ;

\ already defined in strings/compare.fs
\ : -text ( c_addr1 u c_addr2 -- n )
\	swap bounds ?DO
\		dup c@ I c@ =
\	WHILE
\		1+
\	LOOP drop 0
\	ELSE
\		c@ I c@ - unloop
\	THEN sgn ;

\ prüft die Länge u vom String bei c_addr1
: count ( c_addr1 -- c_addr u )
	dup 1+ swap c@ ;

ALL-WORDS
