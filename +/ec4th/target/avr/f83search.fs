\ f83search hand optimised f83search for AVR  18mar26jaw

Decimal

| Code f83search ( c-addr n lfa -- nfa|0 )
\ Traverse dictionary starting with word at lfa and return the nfa if 
\ a match is found.
\ c-addr n is expected to be a ram address of a lower case word
\ Implementation: keep length and first char in registers, separate loops for ram and rom

	ZL tosl movw, \ Z is pointer into dictionary

	loadtos
	temp0 tosl mov, \ temp0 = len
	loadtos
	yl push, 	\ save stack pointer
	yh push,
	yl tosl movw, \ Y is pointer to search string
	temp1 ldy+, \ temp1 = name[0]
    tosl zl movw, \ save pointer to tos

Label f83search-ram-loop
	?rom-address-cp, 
	7 $ brcs,
	zl 2 adiw,    \ Z: lfa to nfa
	temp2 ldZ+,       \ temp2=len+flags
	temp2 $1f andi,
	temp0 temp2 cp,
	4 $ brne,
	temp2 ldZ+,       \  temp2=dict[0]

\ debug jump out
\	tosl temp1 mov,
\	tosh zero mov,
\	1 $ rjmp,

	temp1 temp2 cp,
	4 $ brne,
	temp3 temp0 mov,   \ len
	temp4 yl movw,     \ save search string pointer
5 $:
	temp3 dec,			\ compare loop
	6 $ brne,
	tosl 2 adiw,
	1 $ rjmp,
6 $:
	wl ldy+,	
	wh ldZ+,
	wl wh cp,
    5 $ breq,
	yl temp4 movw,
4 $:
	zl tosl movw,
	tosl ldZ+, tosh ldZ+,
	temp2 tosl mov,		\ within ram the pointer would never be 0
	temp2 tosh or,
	1 $ breq,
	zl tosl movw,
	f83search-ram-loop rjmp,

7 $:
	zh-mask-rom-address,
    tosl zl movw, 		\ save masked pointer to tos
Label f83search-rom-loop

	zl 2 adiw,    		\ Z: lfa to nfa
	temp2 lpmZ+,       \ temp2=len+flags
	temp2 $1f andi,

	temp0 temp2 cp,
	0 $ brne,
	temp2 lpmZ+,       \  temp2=dict[0]

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
	wl ldy+,	
	wh lpmZ+,
	wl wh cp,
    2 $ breq,
	yl temp4 movw,
0 $:
	zl tosl movw,
	tosl lpmZ+, tosh lpmZ+,
	temp2 tosl mov,
	temp2 tosh or,
	zl tosl movw,  		 \ if mask is noop this is redundant
	                     \ TODO: we could reduce one instruction with a tosh mask macro
	7 $ brne,
1 $:
	yh pop,
	yl pop,
	do_next rjmp,
End-Code+
