\ tib.fs standard tib implementation

\ Variable tibstack		\ saves >tib in execute
\ Variable >tib		\ pointer to terminal input buffer

Variable #tib ( -- a-addr ) \ lenght of input
Variable >in ( -- a-addr ) \ offset counter from start of input
0 #tib !
0 >in ! \ char number currently processed in tib
 
250 Constant maxtiblength
Create tib maxtiblength chars allot

: source ( -- c-addr u ) \ addr of t-i-b and lenght of input line
    tib #tib @ ;
 
