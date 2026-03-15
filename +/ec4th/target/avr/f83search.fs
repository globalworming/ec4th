Decimal



Code f83search ( c-addr n lfa -- nfa|0 )
\ Search F83 headers in program memory
\ keep search arguments in registers

	ZL tosl movw, \ Z is pointer into dictionary
	zh-mask-rom-address,
	loadtos
	temp0 tosl mov, \ temp0 = len
	loadtos
	yl push, \ save stack pointer
	yh push,
	yl tosl movw, \ Y is pointer to search string
	temp1 ldy+, \ temp1 = name[0]

Label f83search_loop
    tosl zl movw, \ save pointer to tos
	zl 2 adiw,    \ Z: lfa to nfa
	temp2 lpmZ+,       \ temp2=len+flags
	temp2 $1f andi,

	temp0 temp2 cp,
	0 $ brne,
	temp2 lpmZ+,       \  temp2=dict[0]

\ debug jump out
\	tosl temp1 mov,
\	tosh zero mov,
\	1 $ rjmp,

	temp1 temp2 cp,
	0 $ brne,
	temp3 temp0 mov,   \ len
	temp4 yl movw,     \ save search string pointer
2 $:
	temp3 dec,			\ compare loop
	3 $ brne,
	tosl 2 adiw,
	tosh-pm-forth,		\ we found something!
	1 $ rjmp,
3 $:
	temp1 ldy+,	
	temp2 lpmZ+,
	temp1 temp2 cp,
    2 $ breq,
	yl temp4 movw,
0 $:
	zl tosl movw,
	tosl lpmZ+, tosh lpmZ+,
	temp2 tosl mov,
	temp2 tosh or,
	1 $ breq,
	f83search_loop rjmp,
1 $:
	yh pop,
	yl pop,
	do_next rjmp,
End-Code+
