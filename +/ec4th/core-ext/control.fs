\ +/eckernel/core-ext/control.fs

UNDEF-WORDS
decimal

: >resolve    ( addr -- )        
    here over - swap ! ;

: <resolve    ( addr -- )        
    here - , ;

: (?do)  ( nlimit nstart -- )
    2dup =
    IF   r> swap rot >r >r
       dup @ + >r
    ELSE r> swap rot >r >r
       cell+ >r
    THEN ;

: ?do-like ( -- do-sys )
    ( 0 0 0 >leave )
    >mark >leave
    POSTPONE begin drop do-dest ;

: ?DO ( compilation -- do-sys ; run-time w1 w2 -- | loop-sys )
    POSTPONE (?do) ?do-like ; immediate restrict

: of ( compilation  -- of-sys ; run-time x1 x2 -- |x1 ) 
    \ FIXME: the implementation does not match the stack effect
    1+ >r
    postpone over postpone = postpone if postpone drop
    r> ; immediate

: endof ( compilation case-sys1 of-sys -- case-sys2 ; run-time  -- )
    >r postpone else r> ; immediate

: endcase ( compilation case-sys -- ; run-time x -- )
    postpone drop
    0 ?do postpone then loop ; immediate

Defer again-like ( dest -- addr )
' nip IS again-like

: AGAIN ( compilation dest -- ; run-time -- )
    dest? again-like  POSTPONE branch  <resolve ; immediate restrict



 
ALL-WORDS