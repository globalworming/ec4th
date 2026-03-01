\ +/eckernel/core-ext/compiler.fs

UNDEF-WORDS
decimal

: [compile] ( compilation "name" -- ; run-time ? -- ? ) \ core-ext bracket-compile
    comp' drop
    dup [ comp' exit drop ] aliteral = if
	execute \ EXIT has default compilation semantics, perform them
    else
	compile,
    then ; immediate
 
ALL-WORDS