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
ram-dictionary $200 steal-from-end region return-stack
ram-dictionary $200 steal-from-end region data-stack

setup-target

lock


include +/ec4th/target/avr/prim.fs
include +/ec4th/target/avr/io/uart.fs
\ include +/ec4th/target/avr/io/dot_s.fs
include +/ec4th/target/avr/io/emit_key.fs

include +/eckernel/core/stack.fs
\ include +/eckernel/core/dictionary.fs

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

: print-hello ." Hello world" cr ;

\ link field
UNLOCK tlast @ LOCK 
1 cells - dup 20 tdump
last !

: test-execute \ expect: Hello world\nBack again
  ['] print-hello execute
  ." Back again" cr ;

: test-find \ expect: 0
  ['] print-hello
  s" print-hello" find-name name>xt drop - 0<> .sx drop ;

: test-find-nothing \ expect: 0
  s" asdfasdf" find-name .sx cr drop ;

: test-parse-word \ expect: abc
  s" abc 123" #tib ! >tib ! 0 >in ! parse-word type cr ;

: test-min-n 
  min-n .sx cr drop ;

: test-digit
  'x 10 digit .sx cr 2drop
  '8 10 digit .sx cr 2drop
  ;

: test->number 
  ." Number" cr
  0. s" 123" .sx >number .sx cr 2drop 2drop ;

: test-snumber? 
  ." snumber?" cr
  s" 123" .sx snumber? .sx cr 2drop ;

: test-sp! \ expect: 0
  sp@ >r 1 2 3 r> sp! .sx cr ;

: test-rp! \ 52c
   1324 >r rp@ 1 >r 2 >r 3 >r rp! r> .sx drop cr ;

: test-evaluate-123 \ expect: 123
  ." evaluate" cr
  s" 123" evaluate .sx cr drop ;


include +/gforth/ec/mirror.fs

\ TODO: add mirrorram
 : xy 
   mirrorram
   test-execute
   test-find
   test-find-nothing
   test-parse-word
   test-min-n
   test-digit
   test->number
   test-snumber?
   test-sp!
   test-rp!
   test-evaluate-123
   4711 .x cr
   /line .x cr
   tib .x cr
   sp0 .x sp@ .x 1 2 3 .sx  depth .x drop drop drop cr ." hello world" cr words cr quit bye ;

\ vektor belegen
 ' xy >body init-ip !

\ Set up last and forth-wordlist with the address of the last word's

 unlock
.unresolved
.regions
fd-symbol-table close-file throw

 \ dictionary dump-region

 dictionary extent save-region avr.img
