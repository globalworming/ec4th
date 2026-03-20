\ +/eckernel/core/definingwords.fs

UNDEF-WORDS
decimal


[IFUNDEF] AUser
: uallot ( n -- ) \ gforth
    udp @ swap udp +! ;

: User ( "name" -- ) \ gforth
    Header reveal douser: cfa, cell uallot , ;

: AUser ( "name" -- ) \ gforth
    User ;
[THEN]
ALL-WORDS
