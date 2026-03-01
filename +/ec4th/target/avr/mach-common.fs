    2 Constant cell
    1 Constant cell<<         
    4 Constant cell>bit
    8 Constant bits/char
    8 Constant float
    2 Constant /maxalign
 false Constant bigendian
( true=big, false=little )

: prims-include  ." Include primitives" cr s" +/ec4th/target/avr/prim.fs" included ;
: asm-include    ." Include assembler" cr s" +/ec4th/target/avr/asm.fs" included ;
: >boot ;

>ENVIRON
true SetValue ec
true SetValue rom
true SetValue ITC
false SetValue compiler
