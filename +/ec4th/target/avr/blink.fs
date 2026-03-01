\ uses Port B (at Arduino Board Nano it's used by a LED), to blink several times
label blink
r16 255 ldi, r16 DDRB out/sts, \ set data direction of Port B to output
r20 5 ldi, \ times to blink

\ START Blink!
label blinkLoop
	r16 255 ldi, r16 PORTB out/sts, \ turn LED on

\ Time the LED is on
	r22 10 ldi, \ increase for longer time on, or decrease for opposite
	label WarteA3 r21 155 ldi,
		label WarteA2 temp1 255 ldi,
			label WarteA1 temp1 dec, WarteA1 brne,
		r21 dec, WarteA2 brne,
	r22 dec, WarteA3 brne,

	r16 clr, r16 PORTB out, \ turn LED off

\ Time the LED is off
	r22 10 ldi, \ increase for longer time off, or decrease for opposite
	label WarteB3 r21 155 ldi,
		label WarteB2 temp1 255 ldi,
			label WarteB1 temp1 dec, WarteB1 brne,
		r21 dec, WarteB2 brne,
	r22 dec, WarteB3 brne,

	r20 dec,
	blinkLoop brne,

end-label