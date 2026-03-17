
Start-macros
\ make constants available for assembler
include avr/constants.fs
End-macros

include avr/macros.fs
include avr/vectors328.fs

decimal
Label init-ip    	0 ,
Label error-ip		0 ,
Label init-ramend return-stack-init addr>data ,
Label init-dataStack data-stack-init addr>data ,

\ FIXME: remove
label xxx
\	sendZero temp1 mov,
	\ temp1 have to be filled with data that should be transmitted
	temp0 UCSR0A in/lds, temp0 5 sbrs, xxx rjmp, \ wait for empty transmit buffer
	temp1 UDR0 out/sts, \ put data (temp1) into buffer sends the data
label xxx2
	temp1 $0a ldi,
	temp0 UCSR0A in/lds, temp0 5 sbrs, xxx2 rjmp, \ wait for empty transmit buffer
	\ FIXME: that should be swapped
	temp1 UDR0 out/sts, \ put data (temp1) into buffer sends the data
	ret,
    here jmp-reset jmp!
end-label+

include avr/usart-init.fs
include avr/timer0-ticks.fs
include avr/prim.fs
include avr/muldiv.fs
