 
: SLiteral ( Compilation c-addr1 u ; run-time -- c-addr2 u ) \ string
    \ Compilation: compile the string specified by @i{c-addr1},
    \ @i{u} into the current definition. Run-time: return
    \ @i{c-addr2 u} describing the address and length of the
    \ string.
    postpone (S") here over char+ allot  place align ;
                                             immediate restrict