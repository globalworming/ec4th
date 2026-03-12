\ Register allocation and helpers jaw

Decimal

start-macros

\ include constants.fs

\ Aliases
\ FIXME, needed?
' r2 alias ZEROL
\ FIXME, needed?
' r3 alias ZEROH
' r11 alias mempivot
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
' XL alias IPL \ r26
' XH alias IPH \ r27
\ FIXME, needed?
' r12 alias sendZero


\ loads the value at address pointed by Y to stack
: loadtos
  tosl ldY+, tosh ldY+, ;

\ saves the value of stack to the address pointed by Y
: savetos
  tosh st-Y, tosl st-Y, ;

\ reads addr and jumps to it indirectly
\ FIXME: remove
: jumpOverByAddressREMOVE ( addr -- )
  ZL over lowbyte ldi, ZH swap highbyte ldi,
	temp0 lpmz+, temp1 lpmz,
	ZL temp0 movw,
	ZH lsr, ZL ror,
	ijmp, ;

: end-code+
  savebranch0 end-code ;

: end-label+
  savebranch0 end-label	 ;

also cross 
dictionary extent drop Constant rom-start
ram-dictionary extent drop Constant ram-start
return-stack borders nip Constant return-stack-init
data-stack borders nip Constant data-stack-init
previous 

ram-start $1ff and $100 <> [IF] 
  ." ram dictionary must start at $x100" 1 throw
[THEN]

\ Helpers for choosing either rom (program memory) or ram (data memory)
\ depending on the memory address. If rom address is not 0, it is expected
\ the ram is before rom. In this case the normal ram starts at 100.

: zh-mask-rom-address,
\ mask rom address if rom is not starting with 0
  rom-start IF ZH $7f andi, THEN ;

: mempivot-init,
\ depending which one is on top, we use it as pivot
\ and initialise the pivot register
  rom-start IF
    rom-start highbyte 1-
  ELSE
    ram-start highbyte
  THEN
  temp0 swap ldi,
	mempivot temp0 mov, ;

: ?rom-address-cp,
\ carry is set if it is rom / program memory
  \ cp: rd rr c = rr > rd
  rom-start IF
    mempivot ZH cp,
  ELSE
    ZH mempivot cp,
  THEN ;

: addr>pm 
\ translate the dictionary address to device pm address
\ when its not starting at 0
  rom-start - ;

: addr>data 
\ translate the dictionary address to device pm address
\ when its not starting at 0. SRAM is starting at $100, however
\ the data area on the AVR starts 0. So we need to keep the $100 offset
  ram-start - $100 + ;

end-macros
