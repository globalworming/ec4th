 Create mach-file ," +/ec4th/target/avr/mach-common.fs"

 s" avr.sym" r/w create-file throw value fd-symbol-table

include +/gforth/cross.fs


unlock

0 Constant rom-at-0000


rom-at-0000 [IF]
$c100 $07ff region ram-dictionary
$0000 $ffff region rom-dictionary
[ELSE]
\ FIXME: why 7ff?
$0100 $07ff region ram-dictionary
$8000 $8000 region rom-dictionary
[THEN]

\ return stack needs to be quite deep depending on the 
\ prmitives
ram-dictionary $80 steal-from-end region return-stack
ram-dictionary $40 steal-from-end region data-stack

setup-target

lock


include +/ec4th/target/atmega328.fs
include +/ec4th/target/avr/io/uart.fs
\ include +/ec4th/target/avr/io/dot_s.fs
include +/ec4th/target/avr/io/emit_key.fs

include +/eckernel/core/stack.fs
\ include +/eckernel/core/dictionary.fs

include +/eckernel/core/io.fs

include +/eckernel/primitives/minimal.fs

include +/eckernel/nio/dothex.fs
include +/eckernel/debug/dump.fs

include +/eckernel/core/compiler.fs
include +/eckernel/core/interpreter.fs
include +/eckernel/core/flow-control.fs


include +/eckernel/primitives/doers.fs

\ da muss jens ran:
\ include +/eckernel/todo/int.fs

\ include +/gforth/kernel/int.fs
\ include +/gforth/kernel/comp.fs

\ Codes for testing the Forth-System
\ include +/gforth/arch/misc/tt.fs
\	include nqueens.fs



\ include Forth-System Initiliziation Helping files
\ include +/gforth/ec/mirror.fs

\ ##############################################################################
\ #############################Code#############################################
\ ##############################################################################

Decimal

>auto

\ Set up last and forth-wordlist with the address of the last word's
UNLOCK tlast @ LOCK 
1 cells -
forth-wordlist !
[IFDEF] current 
  forth-wordlist current !
  >ram unlock tdp @ lock dp !
[THEN]

include +/gforth/ec/mirror.fs

 : boot 
   mirrorram
  ." ec4th" cr
   quit bye ;

\ vektor belegen
 ' boot >body init-ip !



 unlock
.unresolved
.regions
fd-symbol-table close-file throw

 \ dictionary dump-region

 dictionary extent save-region avr.img
