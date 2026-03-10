\ comparisons

UNDEF-WORDS \ define words only if not defined

[IFUNDEF] false
\G Boolean false, defined as 0
0 Constant false
[THEN]

[IFUNDEF] true
\G Boolean true, defined as -1
-1 Constant true
[THEN]

[IFUNDEF] min-n
\G The minimal signed integer. This is used for the comparison primitives to mask out the MSB
1 1 cells 8 * 1- lshift
Constant min-n
[THEN]

\ compares two unsigned integers u1 and u2, if u1 is less to u2 return true,
\ else false
: u< ( u1 u2 -- f )
	2dup xor 0< IF nip 0< ELSE - 0< THEN ;

\ if n is less zero return true, else false
: 0< ( n -- f )
	min-n and 0<> ;

\ if n is not equal to zero return true, else false
: 0<> ( n -- f )
	0= 0= ;

\ compares n with zero, if it is zero return a true flag,
\ else return a false flag
: 0= ( n -- f )
	IF false ELSE true THEN ;

\ compares two unsigned integers u1 and u2, if u1 is greater to u2 return true,
\ else false
: u> ( u1 u2 -- f )
	swap u< ;

\ if n is greater zero return true, else false
: 0> ( n -- f )
	negate 0< ;

\ if n is less or equal to zero, return true, else false
: 0<= ( n -- f )
	0> 0= ;

\ if n is greater or equal to zero, return true, else false
: 0>= ( n -- f )
	0< 0= ;

\ compares n1 with n2 if they are euqal return true, else false
: = ( n1 n2 -- f )
	xor 0= ;

\ compares n1 with n2 if they are not equal return true, else false
: <> ( n1 n2 -- f )
	xor 0<> ;

\ compares n1 with n2, if n1 is less to n2 return true, else false
: < ( n1 n2 -- f )
	min-n xor swap min-n xor u> ;

\ compares n1 with n2, if n1 is greater to n2 return true, else false
: > ( n1 n2 -- f )
	swap < ;

\ compares n1 with n2, if n1 is less or equal to n2 return true, else false
: <= ( n1 n2 -- f )
	> 0= ;

\ compares n1 with n2, if n1 is greater or equal to n2 return true, else false
: >= ( n1 n2 -- f )
	swap <= ;

\ compares two unsigned integers u1 and u2, if they are equal return true,
\ else false
\ : u= ( u1 u2 -- f )
\ 	xor 0= ;

\ compares two unsigned integers u1 and u2, if they are not equal return true,
\ else false
\ : u<> ( u1 u2 -- f )
\	xor 0<> ;

\ compares two unsigned integers u1 and u2,
\ if u1 is less or equal to u2 return true, else false
: u<= ( u1 u2 -- f )
	u> 0= ;

\ compares two unsigned integers u1 and u2,
\ if u1 is greater or equal to u2 return true, else false
: u>= ( u1 u2 -- f )
	swap u<= ;

\ if u1 is within the range of u2 to u3 return true, else false
: within ( u1 u2 u3 -- f )
	over - >r - r> u< ;

\ returns the greater value
: max ( S: n1 n2--n ; R: -- )
	2dup < IF swap THEN drop ;

\ returns the smaller value
: min ( S: n1 n2--n ; R: -- )
	2dup > IF swap THEN drop ;

\ TODO: move umin and umax extra?

\ core-ext
: umin ( u1 u2 -- u )
  	2dup u< IF drop ELSE nip THEN ;

\ core-ext
: umax ( u1 u2 -- u )
  	2dup U< IF nip ELSE drop THEN ;

ALL-WORDS
