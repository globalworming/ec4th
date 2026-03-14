\ Value and defer                                     17mar93py / 14mar26jaw

' Constant Alias Value

doer? :dodefer [IF]
: Defer ( "name" -- )
    Header Reveal dodefer: cfa, ['] noop , ;
[ELSE]
: Defer ( "name" -- )
    Create ['] noop A, DOES> @ execute ;
[THEN]

: [is] ( compilation "name" -- ; run-time xt -- ) \ gforth bracket-is
\g At run-time, changes the @code{defer}red word @var{name} to
\g execute @var{xt}.
    ' >body postpone ALiteral postpone ! ; immediate restrict

: is ( "name" xt -- ) \ gforth
\g Changes the @code{defer}red word @var{name} to execute @var{xt}.
    state @ IF [is] ELSE ' >body ! THEN ; immediate restrict

' is alias to
' [is] alias [to]
