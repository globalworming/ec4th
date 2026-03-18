\ Value and defer                                     17mar93py / 14mar26jaw

' Constant Alias Value

doer? :dodefer [IF]
: Defer ( "name" -- )
    Header Reveal dodefer: cfa, ['] noop , ;
[ELSE]
: Defer ( "name" -- )
    Create ['] noop A, DOES> @ execute ;
[THEN]

 [IFDEF] state 
: [is] ( compilation "name" -- ; run-time xt -- ) \ gforth bracket-is
\g At run-time, changes the @code{defer}red word @var{name} to
\g execute @var{xt}.
    ' >body postpone ALiteral postpone ! ; immediate restrict
[THEN]

: is ( "name" xt -- ) \ gforth
\g Changes the @code{defer}red word @var{name} to execute @var{xt}.
    [ [IFDEF] state ]
        state @ IF [is] EXIT THEN 
    [ [THEN] ]
    ' >body ! ; immediate restrict

' is alias to
' [is] alias [to]
