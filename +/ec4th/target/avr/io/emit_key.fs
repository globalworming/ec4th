decimal

\ ##############################################################################
\ ##############################Input / Output##################################
\ ##############################################################################

\ sends a ASCII-char to standard output
code (emit) ( S: n-- ; R: -- )
	temp1 tosl mov, transmit rcall, temp1 clr,
	loadtos
	do_next rjmp,
end-code

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

\ receives one char to data stack
code key ( S: --char ; R: -- )
	savetos
	receive-char rcall,
	tosl temp1 mov, tosh clr, temp1 clr,
	do_next rjmp,
end-code
