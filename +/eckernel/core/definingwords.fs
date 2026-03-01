\ +/eckernel/core/definingwords.fs

UNDEF-WORDS
decimal

 \ FIXME
\ : create ; 
\ : : ;
\ : does> ;

[IFUNDEF] Constant
: Constant  \ core 
    does> @ @ ;
[THEN]

[IFUNDEF] Value
: Value does> @ ;
[THEN]

[IFUNDEF] Defer
: Defer does> @ execute ;
[THEN]

[IFUNDEF] AUser
: uallot ( n -- ) \ gforth
    udp @ swap udp +! ;

: User ( "name" -- ) \ gforth
    Header reveal douser: cfa, cell uallot , ;

: AUser ( "name" -- ) \ gforth
    User ;
[THEN]
ALL-WORDS
