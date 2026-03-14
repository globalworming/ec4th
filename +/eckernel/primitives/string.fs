\ string.fs

UNDEF-WORDS
decimal

\ String literals

: (.")
	"lit count type ;

: "lit ( -- addr )
  	r> r> dup count + aligned >r swap >r ;

: (S")     "lit count ;

: type ( addr u -- )
	0 DO
		dup c@ (emit) 1+
	LOOP drop ;

: count ( c_addr1 -- c_addr u )
	dup 1+ swap c@ ;

ALL-WORDS
