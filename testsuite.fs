Create mach-file ," +/ec4th/target/avr/mach-common.fs"
include +/gforth/cross.fs

unlock

$F100 $0700 region ram-dictionary
$0000 $7300 region rom-dictionary

setup-target

lock
include +/ec4th/target/avr/forth-conf.fs
include +/ec4th/target/avr/prim.fs
include +/ec4th/target/avr/io/uart.fs
include +/ec4th/target/avr/io/dot_s.fs
include +/ec4th/target/avr/io/emit_key.fs

UNDEF-WORDS include +/eckernel/primitives/all.fs ALL-WORDS

\ include +/gforth/ec/mirror.fs

\ include +/eckernel/nio/dothex.fs
\ include +/eckernel/nio/convertpad.fs


Decimal

>auto

\ : ende
\ ." End_of_suite " 
\ ;

 \ platzhalter um zu schaun ob das file richtig eingefügt wird
: test-test  .S ; 
 

  : xy  

 test-test

  ; 
 ' xy >body init-ip !
 unlock
 .unresolved
 dictionary dump-region

 dictionary extent save-region avr.img
