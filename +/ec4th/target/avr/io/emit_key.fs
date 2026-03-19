decimal

\ ##############################################################################
\ ##############################Input / Output##################################
\ ##############################################################################

\ FIXME: change to emit / put I/O directly here
\ sends a ASCII-char to standard output
code (emit) ( S: n-- ; R: -- )
	temp1 tosl mov, transmit rcall, temp1 clr,
	loadtos
	do_next rjmp,
end-code


\ receives one char to data stack
code key ( -- char )
	savetos
	receive-char rcall,
	tosl temp1 mov, 
	tosh clr,
	do_next rjmp,
end-code

code key? ( -- f )
	savetos
	tosl clr,
    read-offset write-offset cp,
    0 $ breq,
	tosl dec,
	0 $:
	tosh tosl mov,
	do_next rjmp,
end-code


\ TODO: provide direct reading from the USART as an option without ringbuffer?
0 [IF]

\ if there is any char in receive buffer it returns a true flag to data stack
code key? ( S: --f ; R: -- )
	savetos
	tosl clr, tosh clr,
	temp0 UCSR0A in/lds,
	temp0 7 sbrs,
	do_next rjmp,
	tosl 255 ldi, tosh 255 ldi,
	do_next rjmp,
end-code

[THEN]