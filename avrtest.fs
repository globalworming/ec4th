 Create mach-file ," +/ec4th/target/avr/mach-common.fs"
include +/gforth/cross.fs


unlock

$F100 $0700 region ram-dictionary
$0000 $7300 region rom-dictionary

setup-target

lock


include +/ec4th/target/avr/prim.fs
include +/ec4th/target/avr/io/uart.fs
\ include +/ec4th/target/avr/io/dot_s.fs
include +/ec4th/target/avr/io/emit_key.fs


include +/eckernel/core/stack.fs
include +/eckernel/core/dictionary.fs
include +/eckernel/core/bitlogic.fs
include +/eckernel/core/io.fs


include +/eckernel/primitives/control.fs
include +/eckernel/primitives/arith.fs
include +/eckernel/primitives/comparisons.fs
include +/eckernel/primitives/double.fs

include +/eckernel/nio/dothex.fs

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

\ TODO: add mirrorram
\ .sx is not working
 : xy 4711 .x sp0 .x sp@ .x ( 1 2 3 .sx )  depth .x cr ." hello world" cr bye ;

\ vektor belegen
 ' xy >body init-ip !

 unlock
.unresolved
.regions

 \ dictionary dump-region

 dictionary extent save-region avr.img
