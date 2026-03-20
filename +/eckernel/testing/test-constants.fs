

1 8 cells 1- lshift Constant min-int
min-int 1- Constant max-int

\ FIXME: check 2constant cross compilation
-1 max-int 2Constant max-2int
0 min-int 2Constant min-2int

max-2int 2/ 2Constant hi-2int \ 001...1
min-2int 2/ 2Constant lo-2int \ 110...0

: <true> true ;
: <false> false ;
