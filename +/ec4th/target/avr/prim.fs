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
Label init-ramstart 	$f0 ,
\ return stack grows downwards from ramend
Label init-ramend $800 ,
Label init-dataStack $800 $32 - ,
init-datastack @ .
." HELLO"
Label init-uart		0 ,
Label return		ret,
return init-uart !

here jump-over cell+ !

	\ run uart initialization
	ZL init-uart lowbyte ldi, ZH init-uart highbyte ldi,
	temp0 lpmz+, temp1 lpmz,
	ZL temp0 movw,
	icall,

\ other necessary Initializations
	r16 clr, r16 SMCR out/sts,
	\ clear ZERO/send Registers
	ZEROL clr, ZEROH clr,
	temp1 clr, sendZero clr,

\ Initialization: Instruction Pointer
	ZL init-ip lowbyte ldi, ZH init-ip highbyte ldi,
	IPL lpmz+, IPH lpmz,
\ Initialization: Return Stack
	ZL init-ramend lowbyte ldi, ZH init-ramend highbyte ldi,
	temp0 lpmz+, temp0 SPL out/sts,
	temp0 lpmz, temp0 SPH out/sts,
	temp0 clr,
\ Initialization: Data Stack
	ZH init-dataStack highbyte ldi, ZL init-dataStack lowbyte ldi,
	YL lpmz+, YH lpmz,
	YL 2 adiw,
\ Initialization: Ram Start
	ZL init-ramstart lowbyte ldi, ZH init-ramstart highbyte ldi,
	ramstart lpmz,

\ include of blink program at start
\	include blink.fs


Label do_next
	\ jumps to IP + 2 (next word to execute)
	ZL IPL movw, \ read IP
	IPL 2 adiw,
	\ check if its a ram address
	ramstart ZH cpc,
	0 $ brcs,
	WL lpmZ+, WH lpmZ+,
	1 $ rjmp,
0 $:
	WL ldZ+, WH ldZ+,
1 $:
	ZL WL movw,
	\ check if its a ram address
	ramstart ZH cpc,
	2 $ brcs,
	temp0 lpmZ+, temp1 lpmZ+,
	ZL temp0 movw,
	\ TODO ijmp is word addressed instructions are always 16 bit, translate in compiler
	ZH lsr, ZL ror,
	ijmp,
2 $:
	temp0 ldZ+, temp1 ldZ+,
	ZL temp0 movw,
	\ TODO ijmp is word addressed instructions are always 16 bit, translate in compiler
	ZH lsr, ZL ror,
	ijmp,
end-label+

Code: :docol
	\ start a colon defined forth-word
	IPH push, IPL push, \ push IP
	IPL WL movw,
	IPL 4 adiw, \ FIXME
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
	tosl 4 adiw, \ FIXME
	do_next rjmp,
End-Code+

Code: :docon
	\ save a constant
	savetos
	ZL WL movw,
	ZL 4 adiw,
	\ FIXME ram/rom unterscheidung
	tosl lpmZ+, tosh lpmZ+,
	do_next rjmp,
End-Code+

label dodoes_ram
	IPL ldZ+, IPH ldZ+,
	tosl ZL movw,
	do_next rjmp,
end-label+

Code: :dodoes
  savetos \ Tos sichern
	IPH push, IPL push, \ IP sichern
	ZL WL movw, \ im Wort dann
	ZL 2 adiw,
	\ check if its a ram address
	ramstart ZH cpc,
	0 $ brcc,
		dodoes_ram rjmp,
	0 $:
	IPL lpmZ+, IPH lpmZ+,
	tosl ZL movw,
	do_next rjmp,
End-Code+

Code execute
	\ executes forth-word thats address is on the tos
	WL tosl movw,
	loadtos
	ZL WL movw,
	\ check if its a ram address
	ramstart ZH cpc,
	0 $ brcc,
		0 $ rjmp,
	0 $:
	temp1 lpmz+, temp0 lpmz+,
	ZL temp0 movw,
	ZH lsr, ZL ror,
	ijmp,
End-Code+

code lit
\ save a literal to tos, first save current tos to data-stack
label read_next_flashcell
	savetos
	ZL IPL movw,
	tosl lpmZ+, tosh lpmZ+,
	IPL 2 adiw, \ FIXME
	do_next rjmp,
end-code+

\ ##############################################################################
\ ##############################Branch / Loop###################################
\ ##############################################################################

code branch
	label branch
		ZL IPL movw,
		temp0 lpmz+, temp1 lpmz+,
	label branch-do
		IPL temp0 add, IPH temp1 adc,
		do_next rjmp,
end-code+

code ?branch
	tosl tosh or,
	loadtos
	branch breq,
	label skip_next
		IPL 2 adiw, \ FIXME
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
	temp1 ramstart mov,
	temp1 tosh cpc,
  \ Without local labels: here 8 + brcs,
  0 $ brcs,
	tosl lpmZ+, tosh lpmZ+,
	do_next rjmp,
	0 $:
	tosl ldZ+, tosh ldZ+,
	do_next rjmp,
End-Code+

Code c@ ( S: addr--c ; R: -- )
	\ fetch a byte from RAM (or IO/CPU register)
	ZL tosl movw,
	\ check if it is not a RAM addr
	temp1 ramstart mov,
	temp1 tosh cpc, tosh clr,
	0 $ brcs,
	tosl lpmZ+,
	do_next rjmp,
		0 $:
		tosl ldZ+,
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

Code , ( S: n-- ; R: -- )
	\ writes 16 bit to RAM at actual position
	ZL IPL movw,
	1 tosh stdZ, 0 tosl stdZ,
	loadtos
	IPL 2 adiw,
	do_next rjmp,
End-Code+

Code c, ( S: c-- ; R: -- )
	\ stores a byte to RAM at actual position
	ZL IPL movw,
	tosl stZ,
	loadtos
	IPL 1 adiw,
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
	tosh ramstart add,
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

here init-dataStack @ $f000 + , Constant sp0

HEX