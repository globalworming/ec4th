\ 

include macros.fs
DECIMAL

\ Todo:
\ - ram anfang: [[+++ ram-dictionary borders drop +++]] - siehe Intro-Sektion, Fehler tritt auf
\ - automatisch ramgröße herausfinden und stacks richtig initialisieren


\ Label rjmp-over-initializiation-vector

\ this long jump is later patched to jump to the initialization
Label jump-over		0 jmp,

Label intro \ Initialization Block
Label init-ip    	0 ,
\ Label init-ramstart 	$f0 ,
\ Label init-romstart 	$8000 ,
\ return stack grows downwards from ramend


Label init-ramend return-stack-init addr>data ,
Label init-dataStack data-stack-init addr>data ,
Label init-uart		0 ,
Label return		ret,
return addr>pm init-uart !

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


here addr>pm 2/ jump-over cell+ !

	\ run uart initialization
\	ZL init-uart lowbyte ldi, ZH init-uart highbyte ldi,
\	temp0 lpmz+, temp1 lpmz,
\	ZL temp0 movw,
\	icall,

\ seems not be needed for sim
0 [IF]
	\ set Power Reduction Register
		temp1 PRR in/lds,
		r16 %11110001 ldi, r16 temp1 and, r16 PRR out/sts,
	\ set UCSR0A Register
		r16 %01100000 ldi, r16 UCSR0A out/sts,
	\ enable transmit and receive mode via Usart 0
		r16 %00011000 ldi, r16 UCSR0B out/sts,
	\ set UCSR0C Register Bits 1, 2 (UCSZ00, UCSZ01)
		r16 %00000110 ldi, r16 UCSR0C out/sts,
	\ set baud rate
		r16 clr, r16 UBRR0H out/sts,
[THEN]

	temp1 '~ ldi,
	xxx rcall,

\ other necessary Initializations
	r16 clr, r16 SMCR out/sts,
	\ clear ZERO/send Registers
	ZEROL clr, ZEROH clr,
	temp1 clr, sendZero clr,

\ Initialization: Instruction Pointer
	init-ip addr>pm 
	ZL over lowbyte ldi, ZH swap highbyte ldi,
	IPL lpmz+, IPH lpmz,
\ Initialization: Return Stack
	init-ramend addr>pm
	ZL over lowbyte ldi, ZH swap highbyte ldi,
	temp0 lpmz+, temp0 SPL out/sts,
	temp0 lpmz, temp0 SPH out/sts,
	temp0 clr,
\ Initialization: Data Stack
	init-dataStack addr>pm
	ZH over highbyte ldi, ZL swap lowbyte ldi,
	YL lpmz+, YH lpmz,
	YL 2 adiw,
\ Initialization: Rom Start
\ FIXME from region
\    temp0 $80 ldi,
\	mempivot temp0 mov,
	mempivot-init,

\ include of blink program at start
\	include blink.fs


Label do_next
\	temp1 '. ldi,
\	xxx rcall,
\ jumps to IP + 2 (next word to execute)
	ZL IPL movw, \ read IP to Z
	IPL 2 adiw,
	?rom-address-cp, 
	0 $ brcs,
	WL ldZ+, WH ldZ+,
	1 $ rjmp,
0 $:
	zh-mask-rom-address,     \ Remove high bit marking the rom address
	WL lpmZ+, WH lpmZ+,
1 $:
	ZL WL movw,
	?rom-address-cp, 
	2 $ brcs,
	temp0 ldZ+, temp1 ldZ+,
	ZL temp0 movw,
	zh-mask-rom-address,    \ Remove high bit marking the rom address
	ZH lsr, ZL ror, \ TODO make doer word addressed to save shifts?
	ijmp,
2 $:
	zh-mask-rom-address,
	temp0 lpmZ+, temp1 lpmZ+,
	ZL temp0 movw,
	zh-mask-rom-address,    \ Remove high bit marking the rom address
	ZH lsr, ZL ror, \ TODO make doer word addressed to save shifts?
	ijmp,
end-label+

Code: :docol
	\ start a colon defined forth-word
	IPH push, IPL push, \ push IP
	IPL WL movw,
	IPL 4 adiw, 
	do_next rjmp,
End-Code+

Code ;s
	\ end a colon defined forth-word
	IPL pop, IPH pop,
	do_next rjmp,
end-code+

Code: :dovar
	\ save variable to ram
	savetos
	tosl WL movw,
	tosl 4 adiw, 
	do_next rjmp,
End-Code+

Code: :docon
	\ save a constant
	savetos
	ZL WL movw,
	ZL 4 adiw,
	\ FIXME ram/rom unterscheidung
	zh-mask-rom-address,
	tosl lpmZ+, tosh lpmZ+,
	do_next rjmp,
End-Code+

Code: :dodoes
  	savetos
	IPH push, IPL push, \ IP sichern
	ZL WL movw, \ im Wort dann
	ZL 2 adiw,
	\ check if its a ram address
	?rom-address-cp,
	0 $ brcc,
	zh-mask-rom-address,
	IPL lpmZ+, IPH lpmZ+,
	tosl ZL movw,
	do_next rjmp,
	0 $:
	IPL ldZ+, IPH ldZ+,
	tosl ZL movw,
	do_next rjmp,
End-Code+

0 [IF]
Code execute
	\ executes forth-word thats address is on the tos
	WL tosl movw,
	loadtos
	ZL WL movw,
	\ check if its a ram address
	?rom-address-cp,
	0 $ brcc,
	0 $ rjmp,
	0 $:
	temp1 lpmz+, temp0 lpmz+,
	ZL temp0 movw,
	ZH lsr, ZL ror,
	ijmp,
End-Code+
[THEN]

code lit
\ save a literal to tos, first save current tos to data-stack
	savetos
	ZL IPL movw,
	IPL 2 adiw,
	?rom-address-cp,
	0 $ brcc,
	zh-mask-rom-address,
	tosl lpmZ+, tosh lpmZ+,
	do_next rjmp,
	0 $:
	tosl ldZ+, tosh ldZ+,
	do_next rjmp,
end-code+

\ ##############################################################################
\ ##############################Branch / Loop###################################
\ ##############################################################################

code branch
label branch
	ZL IPL movw,
	?rom-address-cp,
	0 $ brcc,
	zh-mask-rom-address,
	temp0 lpmz+, temp1 lpmz+,
	IPL temp0 add, IPH temp1 adc,
	do_next rjmp,
	temp0 ldZ+, temp1 ldZ+,
	IPL temp0 add, IPH temp1 adc,
	do_next rjmp,
end-code+

code ?branch
	tosl tosh or,
	loadtos
	branch breq,
label skip_next
	IPL 2 adiw,
	do_next rjmp,
end-code+

code (loop)
	temp0 pop, temp1 pop, \ index
	temp4 pop, temp5 pop, \ limit
	temp2 1 ldi, temp3 clr,
	temp0 temp2 add,
	temp1 temp3 adc,
	temp5 push, temp4 push,
	temp1 push, temp0 push,
	temp0 temp4 cp,
	temp1 temp5 cpc,
	branch brcs,
	skip_next rjmp,
end-code+

code unloop
	temp0 pop, temp1 pop,
	temp4 pop, temp5 pop,
	do_next rjmp,
end-code+

code (do)
	temp0 tosl movw, \ index
	loadtos
	tosh push, tosl push, \ limit
	temp1 push, temp0 push, \ index
	loadtos
	do_next rjmp,
end-code+

\ ##############################################################################
\ ##############################Stack manipulation##############################
\ ##############################################################################

\ Code here \ FIXME: Wird in High Forth gemacht
\ 	do_next rjmp,
\ End-Code+

Code swap ( S: n1 n2--n2 n1 R: -- )
	\ swap the last two stack items
	temp0 tosl movw,
	loadtos
	temp1 st-Y, temp0 st-Y,
	do_next rjmp,
End-Code+

Code drop ( S: n-- ; R: -- )
	\ drops the TOS
	loadtos
	do_next rjmp,
End-Code+

Code 2drop ( S: n1 n2-- ; R: -- )
	\ drops 2 stack items
  YL 2 adiw,
  loadtos
	do_next rjmp,
End-Code+

Code dup ( S: n--n n ; R: -- )
	\ duplicates TOS
	savetos
	do_next rjmp,
End-Code+

Code over ( S: n1 n2--n1 n2 n1 ; R: -- )
	\ duplicates the item before TOS
	savetos
	tosl 2 lddY, tosh 3 lddY,
	do_next rjmp,
End-Code+

Code - ( S: n1 n2--n3 ; R: -- )
	\ substract n2 from n1
	temp0 ldY+, temp1 ldY+,
	temp0 tosl sub, temp1 tosh sbc,
	tosl temp0 movw,
	do_next rjmp,
End-Code+

Code + ( S: n1 n2--n3 ; R: -- )
	\ adds n2 to n1
	temp0 tosl movw,
	loadtos
	tosl temp0 add, tosh temp1 adc,
	do_next rjmp,
End-Code+

Code 2/ ( S: n1--n2 ; R: -- )
	\ devides n1 by 2
	tosh asr, tosl ror,
	do_next rjmp,
End-Code+

Code 2* ( S: n1--n2 ; R: -- )
	\ multiplies n1 by 2
	tosl lsl, tosh rol,
	do_next rjmp,
End-Code+



\ ##############################################################################
\ ##############################Return Stack####################################
\ ##############################################################################

Code r> ( S: --n ; R: n-- )
	\ moves one item from return stack to TOS
	savetos
	tosl pop, tosh pop,
	do_next rjmp,
End-Code+

Code >r ( S: n-- ; R: --n )
	\ moves TOS into Return-Stack
	tosh push, tosl push,
	loadtos
	do_next rjmp,
End-Code+

Code r@ ( S: --n ; R: n--n )
	\ duplicates one item from return stack to TOS
	savetos
	tosl pop, tosh pop,
	tosh push, tosl push,
	do_next rjmp,
End-Code+

Code rp@ ( S: --addr ; R: -- )
	savetos
	tosl SPL in/lds,
	tosh SPH in/lds,
	do_next rjmp,
End-Code+

\ ##############################################################################
\ ##########################Store & Fetch System################################
\ ##############################################################################

Code @ ( S: addr--n ; R: -- )
	\ read 1 cell from RAM (or IO/CPU register)
	ZL tosl movw,
	\ check if it is not a RAM addr
	?rom-address-cp,
  \ Without local labels: here 8 + brcs,
  0 $ brcs,
	tosl ldZ+, tosh ldZ+,
	do_next rjmp,
	0 $:
	zh-mask-rom-address,
	tosl lpmZ+, tosh lpmZ+,
	do_next rjmp,
End-Code+

Code c@ ( S: addr--c ; R: -- )
	\ fetch a byte from RAM (or IO/CPU register)
	ZL tosl movw,
	tosh clr,
	\ check if it is not a RAM addr
	?rom-address-cp,
	0 $ brcs,
	tosl ldZ+,
	do_next rjmp,
	0 $:
	zh-mask-rom-address,
	tosl lpmZ+,
	do_next rjmp,
End-Code+

Code ! ( S: n addr-- ; R: -- )
	\ writes 16 bit to RAM memory (or IO/CPU regis.)
	ZL tosl movw,
	loadtos
	\ 1 tosh stdZ, 0 tosl stdZ,
	tosl stZ+, tosh stZ+,
	loadtos
	do_next rjmp,
End-Code+

Code c! ( S: c addr-- ; R: -- )
	\ store a byte to RAM address
	ZL tosl movw,
	loadtos
	tosl stZ,
	loadtos
	do_next rjmp,
End-Code+

Code cell+ ( S: n1--n2 ; R: -- )
	\ adds the size of one cell to n1
	tosl 2 adiw,
	do_next rjmp,
End-Code+

Code sp@ ( S: --addr ; R: -- )
	\ returns stack pointer position
	savetos
	tosl YL movw,
	do_next rjmp,
End-Code+

\ ##############################################################################
\ ##############################Comparisons#####################################
\ ##############################################################################

Code and ( S: n1 n2--n3 ; R: -- )
	\	logical and
	temp0 tosl movw,
	loadtos
	tosl temp0 and, tosh temp1 and,
	do_next rjmp,
End-Code+

Code or ( S: n1 n2--n3 ; R: -- )
	\ logical or
	temp0 tosl movw,
	loadtos
	tosl temp0 or, tosh temp1 or,
	do_next rjmp,
End-Code+

Code xor ( S: n1 n2--n3 ; R -- )
	\ exclusive or
	temp0 tosl movw,
	loadtos
	tosl temp0 eor, tosh temp1 eor,
	do_next rjmp,
End-Code+

\ put the CPU to sleep forever, this will exit the simavr simulator 20260307;jw
Code bye ( -- ) 
	cli,
	sleep,
End-Code+

\ include io/dot_s.fs
\ include io/emit_key.fs

Label end-primitives
  data-stack-init
End-Label+

\ sp0 expected to the a variable
here swap , Constant sp0

HEX