Decimal

start-macros
include constants.fs

\ Aliases
' r2 alias ZEROL
' r3 alias ZEROH
' r14 alias temp4
' r15 alias temp5
' r16 alias temp0
' r17 alias temp1
' r18 alias temp2
' r19 alias temp3
' r20 alias temp6
' r21 alias temp7
' r22 alias WL
' r23 alias WH
' r24 alias TOSL
' r25 alias TOSH
' XL alias IPL
' XH alias IPH
' r11 alias ramStart
' r12 alias sendZero


\ loads the value at address pointed by Y to stack
: loadtos
  tosl ldY+, tosh ldY+, ;

\ saves the value of stack to the address pointed by Y
: savetos
  tosh st-Y, tosl st-Y, ;

\ reads addr and jumps to it indirectly
: jumpOverByAddress ( addr -- )
  ZL over lowbyte ldi, ZH swap highbyte ldi,
	temp0 lpmz+, temp1 lpmz,
	ZL temp0 movw,
	ZH lsr, ZL ror,
	ijmp, ;

: end-code+
  savebranch0 end-code ;

: end-label+
  savebranch0 end-label	 ;






end-macros