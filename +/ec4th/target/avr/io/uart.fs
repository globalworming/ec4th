decimal

\ ##############################################################################
\ ##############################Input / Output##################################
\ ##############################################################################



\ temp1 stores the ascii-char that should be transmitted, it's cleared afterwards
\ temp0 is used too, so make sure to save it before using the transmit function
label transmit
	\ temp1 have to be filled with data that should be transmitted
	temp0 UCSR0A in/lds, temp0 5 sbrs, transmit rjmp, \ wait for empty transmit buffer
    \ FIXME: out/sts needs to be reversed
	temp1 UDR0 out/sts, \ put data (temp1) into buffer sends the data
	ret,
label receive-char
	temp0 UCSR0A in/lds, 
    temp0 7 sbrs, 
    receive-char rjmp, 
    temp1 UDR0 in/lds,
	ret,
End-label

start-macros

\ transmit macro that saves temp0/1 and pops them back after char c has been transmitted
: tr ( c -- )
	temp0 push, temp1 push,
	temp1 swap ldi, transmit rcall,
	temp1 pop, temp0 pop, ;

: printOutRegContent ( register -- )
    temp1 swap mov,
    transmit rcall,
;

: trReg ( register -- ) \ prints registercontent as HEX and does a CR

    temp0 push,
    temp1 push,
    temp2 push,

    temp0 swap mov,	\ copy register
    temp0 push, 	\ copy to returnstack
    temp0 lsr, temp0 lsr,
    temp0 lsr, temp0 lsr, \ upper 4 bit
    temp1 '0 ldi,	\ is  number between '0 and '9
    temp0 temp1 add,
    temp1 '9 ldi,
    temp2 7 ldi,
    temp1 temp0 sub,
    temp1 7 SBRC, \ when bigger '9, add temp2 to make it a alphanumeric
    temp0 temp2 add,
    temp0 printOutRegContent

    temp0 pop,
    temp0 %1111 andi, \ lower 4 bit
    temp1 '0 ldi,	\ is  number between '0 and '9
    temp0 temp1 add,
    temp1 '9 ldi,
    temp1 temp0 sub,
    temp1 7 SBRC, \ when bigger '9, add temp2 to make it a alphanumeric
    temp0 temp2 add,
    temp0 printOutRegContent
 \   transmit rcall,
 \   temp1 13 ldi,
 \   transmit rcall,
 \   temp1 10 ldi,
    transmit rcall,

    temp2 pop,
    temp1 pop,
    temp0 pop,
 ;



end-macros

