\ Timer 0 is producing micro seconds time base          15mar26jaw

Label timer0-ticks-init
    ticks0 clr,
    ticks1 clr,
    ticks2 clr,

    \ TCCR0B — Timer/Counter0 Control Register B
    \ Bit  Name    Function
    \ ------------------------------------------------------------
    \ 7    FOC0A   Force Output Compare Match A
    \              writing 1 forces a compare match on channel A
    \              only active in non-PWM modes
    \
    \ 6    FOC0B   Force Output Compare Match B
    \              writing 1 forces a compare match on channel B
    \              only active in non-PWM modes
    \
    \ 5    -       Reserved
    \
    \ 4    -       Reserved
    \
    \ 3    WGM02   Waveform Generation Mode bit 2
    \              together with WGM01 and WGM00 selects timer mode
    \
    \ 2    CS02    Clock Select bit 2
    \ 1    CS01    Clock Select bit 1
    \ 0    CS00    Clock Select bit 0
    \              selects timer clock source / prescaler
    \
    \              CS02 CS01 CS00   Clock
    \              -----------------------------------
    \               0    0    0     Timer stopped
    \               0    0    1     clk / 1
    \               0    1    0     clk / 8
    \               0    1    1     clk / 64
    \               1    0    0     clk / 256
    \               1    0    1     clk / 1024
    \               1    1    0     external clock (falling edge)
    \               1    1    1     external clock (rising edge)
    temp0 %00000011 ldi, temp0 TCCR0B out/sts,

    \ TIMSK0 — Timer/Counter0 Interrupt Mask Register
    \ Bit  Name    Function
    \ ------------------------------------------------------------
    \ 7    -       Reserved
    \ 6    -       Reserved
    \ 5    -       Reserved
    \ 4    -       Reserved
    \ 3    -       Reserved
    \
    \ 2    OCIE0B  Output Compare Match B Interrupt Enable
    \              1 = enable interrupt when TCNT0 == OCR0B
    \              ISR vector: TIMER0_COMPB_vect
    \
    \ 1    OCIE0A  Output Compare Match A Interrupt Enable
    \              1 = enable interrupt when TCNT0 == OCR0A
    \              ISR vector: TIMER0_COMPA_vect
    \
    \ 0    TOIE0   Timer0 Overflow Interrupt Enable
    \              1 = enable interrupt when TCNT0 overflows
    \              ISR vector: TIMER0_OVF_vect
    temp0 %00000001 ldi, temp0 TIMSK0 out/sts,

    \ TCCR0A — Timer/Counter0 Control Register A
    \ Bit  Name    Function
    \ ------------------------------------------------------------
    \ 7    COM0A1  Compare Output Mode for Channel A bit 1
    \              with COM0A0 selects behavior of OC0A pin on compare match
    \
    \ 6    COM0A0  Compare Output Mode for Channel A bit 0
    \              COM0A1 COM0A0   Description
    \              ---------------------------------------------
    \                0      0      Normal port operation (OC0A disconnected)
    \                0      1      Toggle OC0A on compare match (CTC only)
    \                1      0      Clear OC0A on compare match (non-inverting PWM)
    \                1      1      Set OC0A on compare match (inverting PWM)
    \
    \ 5    COM0B1  Compare Output Mode for Channel B bit 1
    \              with COM0B0 selects behavior of OC0B pin on compare match
    \
    \ 4    COM0B0  Compare Output Mode for Channel B bit 0
    \              COM0B1 COM0B0   Description
    \              ---------------------------------------------
    \                0      0      Normal port operation (OC0B disconnected)
    \                0      1      Toggle OC0B on compare match (CTC only)
    \                1      0      Clear OC0B on compare match (non-inverting PWM)
    \                1      1      Set OC0B on compare match (inverting PWM)
    \
    \ 3    -       Reserved
    \
    \ 2    -       Reserved
    \
    \ 1    WGM01   Waveform Generation Mode bit 1
    \
    \ 0    WGM00   Waveform Generation Mode bit 0
    \              together with WGM02 (in TCCR0B) selects timer mode
    \
    \              WGM02 WGM01 WGM00   Mode
    \              -----------------------------------------
    \               0     0     0      Normal
    \               0     0     1      PWM, Phase Correct
    \               0     1     0      CTC (Clear Timer on Compare Match)
    \               0     1     1      Fast PWM
    \               1     0     0      Reserved
    \               1     0     1      PWM, Phase Correct (OCR0A top)
    \               1     1     0      Reserved
    \               1     1     1      Fast PWM (OCR0A top)
    temp0 %00000011 ldi, temp0 TCCR0A out/sts,
    1 $ rjmp,

Label timer0-ticks-isr
    temp0 push,
    temp0 sreg in/lds,
    temp0 push,
    ticks0 inc,
    0 $ brne,
    ticks1 inc,
    0 $ brne,
    ticks2 inc,
    0 $:
    temp0 pop,
    temp0 sreg out/sts,
    temp0 pop,
    reti,
1 $:
    timer0-ticks-isr jmp-tim0_ovf jmp!
End-Label+
