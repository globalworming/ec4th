\ +/eckernel/core/bitlogic.fs
\ require ../core-ext/constants.fs
\ true, false
\ require  ../core-ext/control.fs
\ ?DO
UNDEF-WORDS
decimal


\ compares n with zero, if it is zero return a true flag,
\ else return a false flag
: 0= ( n -- f ) \ core
    IF false ELSE true THEN ;

\ if n is less zero return true, else false
: 0< ( n -- f ) \ core
    min-n and 0= 0= ;

\ compares n1 with n2 if they are euqal return true, else false
: = ( n1 n2 -- f ) \ core
    xor 0= ;

\ compares n1 with n2, if n1 is less to n2 return true, else false
: < ( n1 n2 -- f ) \ core
    min-n xor swap min-n xor swap u< ;

\ compares n1 with n2, if n1 is greater to n2 return true, else false
: > ( n1 n2 -- f ) \ core
    swap < ;

: <=	( n1 n2 -- f )
    2dup < -rot = OR ;

: u< ( u1 u2 -- f ) \ core
    - min-n and 0= 0= ;

\ returns the greater value
: max ( S: n1 n2--n ; R: -- ) \ core
    2dup < IF swap THEN drop ;

\ returns the smaller value
: min ( S: n1 n2--n ; R: -- ) \ core
    2dup > IF swap THEN drop ;

\ shifts u1 n times to right. Effectivly it is deviding u1 n times by 2
: rshift ( S: u1 n--u2 ; R: -- ) \ core 
    0 ?DO 2/ 
   \ FIXME: zu [ min-n invert ] literal 
    min-n invert
    and LOOP ;

\ shifts u1 n times to left. Effectivly it is multiplying u1 n times with 2
: lshift ( S: u1 n--u2 ; R: -- ) \ core
    0 ?DO 2* LOOP ;

\ inverts all bits in u1 i.e. 1010 -> 0101
: invert ( S: u1--u2 ; R: -- ) \ core
    -1 xor ;

\ negates n1
: negate ( S: n1--n2 ; R: -- ) \ core
    invert 1+ ;

: on  ( a-addr -- ) \ gforth
    \G Set the (value of the) variable  at @i{a-addr} to @code{true}.
    true  swap ! ;
: off ( a-addr -- ) \ gforth
    \G Set the (value of the) variable at @i{a-addr} to @code{false}.
    false swap ! ;



ALL-WORDS 
