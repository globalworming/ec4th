\ +/eckernel/core-ext/io.fs

UNDEF-WORDS
decimal

: hex ( -- ) \ core-ext
    \G Set @code{base} to &16 (hexadecimal).
    10 base ! ;

: .(   ( compilation,interpretation "ccc<paren>" -- )
  \G Compilation and interpretation semantics: Parse a string @i{ccc}
  \G delimited by a @code{)} (right parenthesis). Display the
  \G string. This is often used to display progress information during
  \G compilation; see examples below.
    [char] ) parse type ; immediate

 : .r ( n1 n2 -- ) \ core-ext	dot-r
    \G Display @var{n1} right-aligned in a field @var{n2} characters wide. If more than
    \G @var{n2} characters are needed to display the number, all digits are displayed.
    \G If appropriate, @var{n2} must include a character for a leading ``-''.
      >r s>d r> d.r ;

: source-id ( -- 0 | -1 | fileid ) \ core-ext,file source-i-d
    \G Return 0 (the input source is the user input device), -1 (the
    \G input source is a string being processed by @code{evaluate}) or
    \G a @i{fileid} (the input source is the file specified by
    \G @i{fileid}).
      loadfile @ dup 0= IF  drop sourceline# 0 min  THEN ;

: save-input ( -- xn .. x1 n ) \ core-ext
    \G The @i{n} entries @i{xn - x1} describe the current state of the
    \G input source specification, in some platform-dependent way that can
    \G be used by @code{restore-input}.
      >in @
      loadfile @
      if
	loadfile @ file-position throw
	[IFDEF] #fill-bytes #fill-bytes @ [ELSE] #tib @ 1+ [THEN] 0 d-
      else
	blk @
	linestart @
      then
      sourceline#
      >tib @
      source-id
      6 ;

: restore-input ( xn .. x1 n -- flag ) \ core-ext
    \G Attempt to restore the input source specification to the state
    \G described by the @i{n} entries @i{xn - x1}. @i{flag} is
    \G true if the restore fails.  In Gforth it fails pretty often
    \G (and sometimes with a @code{throw}).
      6 <> -12 and throw
      source-id <> -12 and throw
      >tib !
      >r ( line# )
      loadfile @ 0<>
      if
	loadfile @ reposition-file throw
	refill 0= -36 and throw \ should never throw
      else
	linestart !
	blk !
	sourceline# r@ <> blk @ 0= and loadfile @ 0= and
	if
	    drop rdrop true EXIT
	then
      then
      r> loadline !
      >in !
      false ;


ALL-WORDS