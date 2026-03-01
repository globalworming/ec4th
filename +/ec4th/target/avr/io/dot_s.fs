code .controls
\ Vorzeichen ausgeben, falls negativ
	label .-
		'- tr tosl 1 sbiw, tosl com, tosh com,
		ret,
	label .moredone
		temp1 temp7 mov,
		temp1 temp6 add, temp6 clr,
		transmit rcall, ret,
	label .more
		tosl temp2 sub, tosh temp3 sbc, temp6 inc,
		tosl temp2 cp, tosh temp3 cpc, temp4 SREG in/lds,
		temp4 0 sbrc, .moredone rcall, temp4 0 sbrs, .more rcall,
		ret,
	label .less
		ZEROL sendZero cpse, temp1 temp7 mov,
		ZEROL sendZero cpse, transmit rcall,
		ret,
	label .check
		tosl temp2 cp, tosh temp3 cpc,
		temp4 SREG in/lds, temp4 0 sbrc, .less rcall,
		tosl temp2 cp, tosh temp3 cpc,
		temp4 SREG in/lds, temp4 0 sbrs, .more rcall,
		ret,
end-code

code .execute
label .ex
	temp7 '0 ldi, \ "0" in temp7 schreiben
	temp6 clr, \ Zählerregister resetten
	\ Prüfung des Vorzeichenbits
	tosh 7 sbrc, .- rcall,
label unsigned.
	temp1 clr, sendZero clr,
	\ Prüfung der Zehnerpotenzen beginnend bei 10^4
	temp2 10000 lowbyte ldi, temp3 10000 highbyte ldi,
		.check rcall,
	temp2 1000 lowbyte ldi, temp3 1000 highbyte ldi,
		.check rcall,
	temp2 100 lowbyte ldi, temp3 100 highbyte ldi,
		.check rcall,
	temp2 10 lowbyte ldi, temp3 10 highbyte ldi,
		.check rcall,
	temp1 '0 ldi, temp2 1 lowbyte ldi, temp3 1 highbyte ldi,
		sendZero temp2 add, .check rcall,
	ret,
end-code

code .
	.ex rcall, 32 tr loadtos
	do_next rjmp,
end-code

code u.
	temp7 '0 ldi, \ "0" in temp7 schreiben
	temp6 clr, \ Zählerregister resetten
	unsigned. rcall,
	32 tr loadtos
	do_next rjmp,
end-code


code depth.controls
	label .depth
		tosl dataStackStart lowbyte ldi,
		tosh dataStackStart highbyte ldi,
		tosl YL sub, tosh YH sbc,
		tosh lsr, tosl ror,
	ret,
	label zero.depth
		'0 tr 13 tr 10 tr loadtos
	do_next rjmp,
end-code

code depth
	\ legt die Anzahl der Stackitems auf den Stack
	savetos .depth rcall,
	do_next rjmp,
end-code

code .s
	\ Listet die Anzahl der Chars auf dem Stack auf und dann die einzelnen Chars
	\ an sich
	sendZero ZEROL mov,
	savetos .depth rcall,
	'< tr .ex rcall, '> tr 32 tr
	.depth rcall,
	tosl ZEROL cp, tosh ZEROH cpc,
	temp4 SREG in/lds, temp4 1 sbrc,
	zero.depth rjmp,
	YL push, YH push, \ rette Stackpointer
	YL dataStackStart lowbyte ldi, YH dataStackStart highbyte ldi,
	label .printStack
		tosh ld-Y, tosl ld-Y, .ex rcall, 32 tr
		temp3 pop, temp2 pop, temp2 push, temp3 push,
		temp2 YL sub, temp3 YH sbc,
		.printStack brne,
	YH pop, YL pop, \ stelle Stackpointer wieder her
	loadtos 13 tr 10 tr temp1 clr,
	do_next rjmp,
end-code