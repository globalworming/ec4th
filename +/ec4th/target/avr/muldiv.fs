\ ################ Wormis kram

code um* ( n1 n2 - dn )
	temp0 ldy+, temp1 ldy+,
	temp2 clr, temp3 clr,
	temp4 clr, temp5 clr,
	temp7 clr, temp6 15 ldi,
	4 $ rjmp,

	3 $:
	tosl lsl, tosh rol,

	4 $:
	temp6 temp7 CP,
	1 $ BRCS,
	temp2 lsl, temp3 rol,
	temp4 rol, temp5 rol,
	temp7 INC,
	tosh 7 BST,
	3 $ BRTC,
	CLC,
	temp2 temp0 add,
	temp3 temp1 adc,
	temp4 zerol adc,
	temp5 zeroh adc,
	3 $ rjmp,

	1 $:
	temp5 st-y, temp4 st-y,

	\  zeroh st-y, temp7 st-y,
	loadtos
	temp3 st-y, temp2 st-y,
	do_next rjmp,
	end-code+


code um/mod ( dn1 n2 -- remainder Quotient) 
	temp6 ldy+, temp7 ldy+, \ highcell of dn1
	temp4 ldy+, temp5 ldy+, \ lowcell of dn1
	temp0 17 ldi, \ loopcounter
	temp1 clr, \ carry

\	temp7 tosh cp,		for testing if  n2 < high dn1, this would result in double quotient 
\	temp6 tosl cpc,
\	1 $ BRSH,
\	temp2 tosl mov,		for testing if n2 != 0
\	temp2 tosh or, 
\	1 $ BREQ,


	0 $:
	temp0 1 SUBI,
	1 $ BRCS,
	zerol temp1 cp,
	2 $ BRNE,
	tosh temp7 cp,
	2 $ BRLO,
	tosh temp7 cp,
	4 $ BREQ,
	clc,
	3 $ rjmp,
	4 $:
	temp6 tosl cp,
	2 $ BRSH,
	clc,
	3 $ rjmp,
	2 $:
	temp6 tosl sub,
	temp7 tosh sbc,
	sec,
	3 $:
	temp4 rol, temp5 rol,
	temp6 rol, temp7 rol,
	temp1 clr, temp1 rol,
	0 $ rjmp,
	1 $:
	temp7 lsr, temp6 ror,
	temp6 temp1 or,
	temp7 st-y, temp6 st-y,
	tosl temp4 movw,
	do_next rjmp,
end-code+
