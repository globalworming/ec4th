\ (parse-word) in AVR assembpler 14mar26jaw

| Code (parse-word) ( c-addr u -- word-addr word-len )
\G Skip whitesapce and scan non whitespace in the input
\G AVR assembler implementation to speed up interpreting performance

    temp0 tosl mov,
    loadtos
    zl tosl movw,

    ?rom-address-cp,
    7 $ brcs,

    \ ===== RAM =====

1 $:
    temp0 zero cp,
    4 $ breq,
    temp1 ldZ+,
    temp0 dec,
    temp1 $21 cpi,
    1 $ brlo,

    tosl zl movw,
    tosl 1 sbiw,
    wl 1 ldi,

2 $:
    temp0 zero cp,
    3 $ breq,
    temp1 ldZ+,
    temp0 dec,
    temp1 $21 cpi,
    3 $ brlo,
    wl inc,
    2 $ rjmp,

3 $:
    savetos
    tosl wl mov,
    tosh zero mov,
    do_next rjmp,

4 $:
    tosl zl movw,
    savetos
    tosl zero mov,
    tosh zero mov,
    do_next rjmp,

    \ ===== ROM =====
7 $:
    zh-mask-rom-address,

8 $:
    temp0 zero cp,
    4 $ breq,
    temp1 lpmZ+,
    temp0 dec,
    temp1 $21 cpi,
    8 $ brlo,

    tosl zl movw,
    tosl 1 sbiw,
    wl 1 ldi,

9 $:
    temp0 zero cp,
    5 $ breq,
    temp1 lpmZ+,
    temp0 dec,
    temp1 $21 cpi,
    5 $ brlo,
    wl inc,
    9 $ rjmp,

5 $:
    tosh-pm-forth,
    savetos
    tosl wl mov,
    tosh zero mov,
    do_next rjmp,

End-Code+
