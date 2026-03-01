: test-plus
  ." C +" cr
  ." T 0 0 + " cr
  ." E 0" cr
  ." O " 0 0 + .x cr 
  ." T -1 1 + " cr
  ." E 0" cr
  ." O " -1 1 + .x cr 
  ." F" cr
;

\ dummys, =0 und <0 noch nicht implementiert
: =0 ; : <0 ;

: test-greaterzero
  ." C 0<" cr
  ." T 0 <0" cr
  ." E 0 " cr
  ." O" 0 <0 .x cr
  ." T 65535 <0 " cr
  ." E 1 " cr
  ." O" 65535 <0 .x cr
  ." T -1 <0" cr
  ." E 1 " cr
  ." O" -1 <0 .x cr
  ." T 1 <0" cr
  ." E 1 " cr
  ." O " 1 <0 .x cr
  ." F" cr
;
: test-equalszero
  ." C =0" cr
  ." T 0 =0" cr
  ." E 1 " cr
  ." O" 0 =0 .x cr
  ." T -1 =0" cr
  ." E 0 " cr
  ." O" -1 =0 .x cr
  ." T 32768 =0" cr
  ." E 0 " cr
  ." O" 32768 =0 .x cr
  ." F" cr
;
: test-oneplus
  ." C 1+" cr
  ." T 0 1+" cr
  ." E 1 " cr
  ." O " 0 1+ .x cr
  ." T -1 1+" cr
  ." E 0 " cr
  ." O " -1 1+ .x cr
  ." T 65535 1+" cr
  ." E 0 " cr
  ." O " 65535 1+ .x cr
  ." F" cr
;   
: test-oneminus
  ." C 1-" cr
  ." T 0 1-" cr
  ." E FFFF " cr
  ." O " 0 1- .x cr
  ." T 1 1-" cr
  ." E 0 " cr
  ." O " 1 1- .x cr
  ." T -32767 1-" cr
  ." E 8000 " cr
  ." O " -32767 1- .x cr
  ." F" cr
;   
: test-twotimes
  ." C 2*" cr
  ." T 0 2*" cr
  ." E 0 " cr
  ." O " 0 2* .x cr
  ." T 16384 2*" cr
  ." E 8000 " cr
  ." O " 16384 2* .x cr
  ." T -32768 2*" cr
  ." E 0 " cr
  ." O " -32768 2* .x cr
  ." T -1 2*" cr
  ." E FFFE " cr
  ." O " -1 2* .x cr
  ." F" cr
;   
: test-twodivide
  ." C 2/" cr
  ." T -32767 2/" cr
  ." E 4000" cr
  ." O " -32767 2/ .x cr 
  ." T 0 2/ " cr
  ." E 0" cr
  ." O " 0 2/ .x cr 
  ." T 20 2/" cr
  ." E A" cr 
  ." O " 20 2/ .x cr 
  ." T 32 2/ + " cr
  ." E F" cr
  ." O " 32 2/ .x cr 
  ." F" cr
;
: test-twoswap
  ." C 2swap" cr
  ." T 1 2 3 4" cr
  ." E 3 4 1 2" cr
  ." O " 1 2 3 4 2swap 4 n.x cr 
  ." T 1 2 3 4 2swap 2swap" cr
  ." E 4 3 2 1" cr
  ." O " 1 2 3 4 2swap 2swap 4 n.x cr 
  ." F" cr
;
: test-swap
  ." C swap" cr
  ." T 1 2" cr
  ." E 1 2" cr
  ." O " 1 2 swap .x .x cr 
  ." F" cr
;
: test-drop
  ." C drop " cr
  ." T  0 1 drop" cr
  ." E 0 " cr
  ." O " 0 1 drop .x cr 
  ." F" cr
;

: test-and
  ." C and" cr
  ." T 0 1 and" cr
  ." E 0" cr
  ." O " 0 1 and .x cr 
  ." T -1 1 and " cr
  ." E 1 " cr
  ." O " -1 1 and .x cr 
  ." T 16 17 and " cr
  ." E 10" cr
  ." O " 16 17 and cr 
  ." F" cr
;
: test-or
  ." C or" cr
  ." T 0 1 or" cr
  ." E 1" cr
  ." O " 0 1 or .x cr 
  ." T -1 1 or " cr
  ." E FFFF " cr
  ." O " -1 1 or .x cr 
  ." T 16 4 or " cr
  ." E 0" cr
  ." O " 16 4 or cr 
  ." F" cr
;
: test-xor
  ." C xor" cr
  ." T 0 1 xor" cr
  ." E 1" cr
  ." O " 0 1 xor .x cr 
  ." T -1 1 xor " cr
  ." E FFFE " cr
  ." O " -1 1 xor .x cr 
  ." T 16 4 xor " cr
  ." E 14" cr
  ." O " 16 4 xor cr 
  ." F" cr
;



: test-twodrop
  ." C 2drop " cr
  ." T 1 2 3 4 2drop " cr
  ." E 2 1" cr
  ." O " 1 2 3 4 2drop .x .x cr 
  ." F" cr
;
: test-twodup
  ." C 2dup" cr
  ." T 1 2 2dup" cr
  ." E 2 1 2 1 " cr
  ." O " 1 2 2dup 4 n.x cr 
  ." F" cr
;
: test-twoover 
  ." C 2over " cr
  ." T 1 2 3 4 2over" cr
  ." E 2 1 4 3 2 1" cr
  ." O " 1 2 3 4 2over 6 n.x   cr 
  ." F" cr
;
: test-lessthan
  ." C <" cr
  ." T 0 1 <" cr
  ." E FFFF" cr
  ." O" 0 1 < .x cr
  ." T-32767 65535 <" cr
  ." E 0" cr
  ." O" -32767 65535 < cr
  ." T -1 1 <" cr
  ." E FFFF" cr
  ." O" -1 1 < .x cr
  ." F" cr
;
: test-dup
  ." C dup" cr
  
  ." T 55 dup" cr
  ." E 77" cr
  ." O " 55 dup emit emit cr
  
  ." T -1 dup" cr
  ." E FFFF FFFF" cr
  ." O " -1 dup .x .x cr
  ." F" cr
;

: test-rot 
  ." C rot" cr
  
  ." T 1 2 3 rot" cr
  ." E 1 3 2" cr
  ." O " 1 2 3 rot .x .x .x cr
 
  ." T 1 2 3 rot rot" cr
  ." E 2 1 3" cr
  ." O " 1 2 3 rot rot .x .x .x cr
  ." F" cr
;
: test-umdurchmod

  ." C um/mod" cr

  ." T 0 0 1 um/mod" cr
  ." E 0 0" cr
  ." O " 0 0 1 um/mod .x .x cr
 
  ." T 20 0 4 um/mod" cr
  ." E 5 0" cr
  ." O " 20 0 4 um/mod .x .x cr

  ." T test dat: 0 1 256 um/mod" cr
  ." E 100 0" cr 
  ." O " 0 1 256 um/mod .x .x cr

  ." T test dat: 0 1 1024 um/mod" cr
  ." E 40 0" cr
  ." O " 0 1 1024 um/mod .x .x cr

  ." T test dat: 0 1 2048 um/mod" cr
  ." E 20 0" cr
  ." O " 0 1 2048 um/mod .x .x cr

  ." T test dat: -1 0 0 um/mod" cr
  ." E Error" cr
  ." O " 0 0 0 um/mod .x .x cr

  ." T test dat: 1 0 0 um/mod" cr
  ." E Error" cr
  ." O " 1 0 0  um/mod .x .x cr

  ." T test dat: 0 1 0 um/mod" cr
  ." E Error" cr
  ." O " 0 1 0 um/mod .x .x cr

  ." T test dat: 32768 0 2 um/mod" cr
  ." E 4000 0" cr
  ." O " 32768 0 2 um/mod .x .x cr

  ." T test dat: 32768 0 16384 um/mod" cr
  ." E 2 0" cr
  ." O " 32768 0 16384 um/mod .x .x cr

  ." T test dat: 0 2 1 um/mod" cr
  ." E Error" cr
  ." O " 0 2 1 um/mod .x .x cr

  ." T test dat: 0 -1 -2 um/mod" cr
  ." E Error " cr
  ." O " 0 -1 -2 um/mod .x .x cr

  ." T test dat: 0 -2 -1 um/mod" cr
  ." E Error  " cr
  ." O " 0 -2 -1 um/mod .x .x cr

  ." T test dat: -1 0 1 um/mod" cr
  ." E FFFF 0" cr
  ." O " -1 0 1 um/mod .x .x cr

  ." T test dat: -1 0 2 um/mod" cr
  ." E 7FFF 1" cr
  ." O " -1 0 2 um/mod .x .x cr

  ." T test dat: 30 0 7 um/mod" cr
  ." E 4 2" cr
  ." O " 30 0 7 um/mod .x .x cr

  ." T test dat: 1000 0 999 um/mod" cr
  ." E 1 1" cr
  ." O " 1000 0 999 um/mod .x .x cr

  ." T test dat: 9045 0 2901 um/mod" cr
  ." E 3 156" cr
  ." O " 9045 0 2901 um/mod .x .x cr

  ." T test dat: -14586 0 27873 um/mod" cr
  ." E 1 5A25" cr
  ." O " -14586 0 27873 um/mod .x .x cr

  ." T test dat: 1097 0 18554 um/mod" cr
  ." E 0 449" cr
  ." O " 1097 0 18554 um/mod .x .x cr

  ." T test dat: -1580 0 1211 um/mod" cr
  ." E 34 3D8" cr
  ." O " -1580 0 1211 um/mod .x .x cr

  ." T test dat: 0 21289 26789  um/mod" cr
  ." E CB70 60D0 " cr
  ." O " 0 21289 26789  um/mod .x .x cr

  ." T test dat: 0 5950 6137  um/mod" cr
  ." E F833 165" cr
  ." O "0 5950 6137  um/mod .x .x cr

  ." T test dat: 0 16200 18250 um/mod" cr
  ." E E33E 1E14" cr
  ." O " 0 16200 18250 um/mod .x .x cr

  ." T test dat: 22134 5845 17439 um/mod" cr
  ." E 55CE 3A84" cr
  ." O " 22134 5845 17439 um/mod .x .x cr

  ." T test dat: 16940 15354 26272 um/mod" cr
  ." E 959D 320C" cr
  ." O " 16940 15354 26272 um/mod .x .x cr

  ." T test dat:23191 4972 26216 um/mod" cr
  ." E 308E CE7"  cr
  ." O " 23191 4972 26216 um/mod .x .x cr 

  ." F " cr
;
 