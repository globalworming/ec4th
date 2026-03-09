 Create mach-file ," +/ec4th/target/avr/mach-common.fs"

 s" avr.sym" r/w create-file throw value fd-symbol-table

include +/gforth/cross.fs


unlock

0 Constant rom-at-0000


rom-at-0000 [IF]
$c100 $07ff region ram-dictionary
$0000 $ffff region rom-dictionary
[ELSE]
$0100 $07ff region ram-dictionary
$8000 $ffff region rom-dictionary
[THEN]

\ return stack should have 64 bytes
ram-dictionary $40 steal-from-end region return-stack
ram-dictionary $40 steal-from-end region data-stack

setup-target

lock


include +/ec4th/target/avr/prim.fs
include +/ec4th/target/avr/io/uart.fs
\ include +/ec4th/target/avr/io/dot_s.fs
include +/ec4th/target/avr/io/emit_key.fs

include +/eckernel/core/stack.fs
\ include +/eckernel/core/dictionary.fs

include +/eckernel/core/bitlogic.fs
include +/eckernel/core/io.fs

include +/eckernel/primitives/minimal.fs

include +/eckernel/nio/dothex.fs

include +/eckernel/core/interpreter.fs

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

include +/gforth/ec/mirror.fs

: print-hello ." Hello world" cr ;

: test-execute \ expect: Hello world\nBack again
  ['] print-hello execute
  ." Back again" cr ;

: test-find \ expect: 0
  ['] print-hello
  s" print-hello" sfind .sx bye ;

\ TODO: add mirrorram
 : xy 
   mirrorram 
   \ test-find
   4711 .x cr
   /line .x cr
   tib .x cr
   sp0 .x sp@ .x 1 2 3 .sx  depth .x drop drop drop cr ." hello world" cr words cr quit bye ;

\ vektor belegen
 ' xy >body init-ip !

\ Set up last and forth-wordlist with the address of the last word's
\ link field
UNLOCK tlast @ LOCK 
1 cells - dup 20 tdump
last !

 unlock
.unresolved
.regions
fd-symbol-table close-file throw

 \ dictionary dump-region

 dictionary extent save-region avr.img
