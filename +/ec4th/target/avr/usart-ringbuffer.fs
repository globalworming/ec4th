


start-macros
also cross 
tib-region area
dup 0= error" Needs tib-region"
dup 255 u> error" tib region should be 255 maximum"
Constant buffer-size \ maximum 255
Constant buffer-start
\ send XOFF already when 2 chars are in the buffer.
2 Constant throttle-threshold
previous 
end-macros

Label uart-rx-isr
    temp0 push,
    temp0 sreg in/lds,
    temp0 push,
    temp1 push,
    zl push,
    zh push,

    temp0 udr0 in/lds,

    \ TODO detect overflow / full buffer

    \ store into buffer
    ZL buffer-start lowbyte ldi,
    ZH buffer-start highbyte ldi,
    zl write-offset add,
    zh zero adc,
    temp0 stz, 

    \ increment and wrap around
    write-offset inc, 
    write-offset buffer-size cpi, 
    0 $ brne,
    write-offset clr,
    0 $:

    \ debug write ofset
    \ temp0 write-offset mov,
    \ temp0 char 0 ldi,
    \ temp0 write-offset add,
    \ temp0 UDR0 out/sts,

    \ throttle
    \ compute characters in buffer into temp0
1 [IF]
    clc,
    temp0 write-offset mov,
    temp0 read-offset sbc,
    1 $ brcc,
    clc,
    temp0 0 buffer-size - $ff and subi,
    \ send XOFF once if at threshold
    1 $:
    temp0 throttle-threshold cpi,
    2 $ brne,
    temp1 $13 ldi,
    \ temp1 '^ ldi,
    temp1 UDR0 out/sts, 
    \ transmit rcall,
    2 $:
[THEN]

    zh pop,
    zl pop,
    temp1 pop,
    temp0 pop,
    temp0 sreg out/sts, \ reversed!
    temp0 pop,
    reti,
    uart-rx-isr jmp-usart_rxc jmp!
End-label+

label receive-char
    cli,
    read-offset write-offset cp,
    0 $ brne,
    sei,
    receive-char rjmp,

    0 $:
    ZL buffer-start lowbyte ldi,
    ZH buffer-start highbyte ldi,

    zl read-offset add,
    zh zero adc,
    temp1 ldz,

    read-offset inc, 
    read-offset buffer-size cpi, \ wrap around if needed
    1 $ brne,
    read-offset clr,
    1 $:

    sei,

    \ send XON, if transmit register is not empty wait, to make sure the 
    read-offset write-offset cp,
    2 $ brne,
    temp1 push,
    temp1 $11 ldi,
    transmit rcall,
    temp1 pop,
    2 $:

    ret,
End-label+
