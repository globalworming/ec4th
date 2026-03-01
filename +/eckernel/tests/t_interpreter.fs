\ to include in gforth
require +/eckernel/interpret/interpret.fs 

create teststring ," ."

: t_interpreter
cr ." T interpreter" cr
." E 40 " cr
40 teststring count  
." O "
interpreter \ should execute .
;