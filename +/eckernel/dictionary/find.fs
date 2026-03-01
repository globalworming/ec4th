\ find.fs

hex

Variable last
\G points to the last defined word

has? f83headerstring [IF]

080 Constant alias-mask
040 Constant immediate-mask
020 Constant restrict-mask
01f Constant lcount-mask

[ELSE]

." non f83 header strings unsupported" AUTSCH

[THEN]

\ schema of a word

\ LFA                        NFA                                                  CFA                                                BODY
\ <link to next word / cell> <length+flags / byte> n * <name chars> n * <padding> <link to doer / cell> <link to does part / cell> n <xt / cell>

: search ( adr len lfa -- nfa | 0 )
  BEGIN dup WHILE >r 2dup r@ cell+ count 01f and 
        comparedict 0=
        IF 2drop r> cell+ EXIT THEN r> @
  REPEAT nip nip ;

\ FIXME: make this defered word as hook for wordlist support
: find-name ( c-addr u -- nt | 0 ) 
  last @ search ;

has? f83headerstring [IF]

: name>string ( nfa -- addr count )
  dup char+ swap c@ lcount-mask and ;

: name>xt ( nfa -- xt +-1 | 0 ) 
  dup c@ swap name>string + cfaligned swap ( flags xt )
  dup alias-mask and IF swap @ swap THEN
  immediate-mask and IF -1 ELSE 1 THEN 
  ;
[THEN]



: sfind ( c-addr u -- 0 / xt +-1  ) \ gforth-obsolete
  find-name dup IF name>xt THEN ;

: find ( c-addr -- xt +-1 | c-addr 0 ) \ core,search
  dup count sfind dup IF rot drop THEN ;
