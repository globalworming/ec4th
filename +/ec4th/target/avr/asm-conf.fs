\ Assembler Configuration file
\ configure maxlabel, limitperlabel and device as you need them.

\ relative-addresses constants
10 Constant maxlabel \ limit for maximal labels
5 Constant limitperlabel \ maximum for targeting a label


Create branch-dest maxlabel cells allot
Create branch-src maxlabel limitperlabel * cells allot

\ save zeros to the branch variables that are used to resolve local labels
: savebranch0
  maxlabel 0 DO
    0 branch-dest I cells + !
    limitperlabel 0 DO
      0 branch-src J limitperlabel * I + cells + !
    LOOP
  LOOP ;
savebranch0


\ Device-Flags
\ 2 tiny 11/12 not
\ 4 only mega
\ 8 no one
\ 16 for mega16 +
\ 32 only tiny 13/2313 and mega
\ 64 only tiny 11/12/26 not
\ flags
 1 Constant flag-all
 2 Constant flag-pop
 2 Constant flag-push
 2 Constant flag-ijmp
 2 Constant flag-icall
 2 Constant flag-sts
 2 Constant flag-adiw
 2 Constant flag-sbiw
 2 Constant flag-ldd
 2 Constant flag-std
 4 Constant flag-mul
 4 Constant flag-muls
 4 Constant flag-mulsu
 4 Constant flag-fmul
 4 Constant flag-fmuls
 4 Constant flag-fmulsu
 8 Constant flag-elpm
 8 Constant flag-eijmp
 8 Constant flag-eicall
16 Constant flag-break
16 Constant flag-call
16 Constant flag-jmp
32 Constant flag-spm
32 Constant flag-movw
64 Constant flag-lds

\ devices
flag-all
flag-pop or flag-push or flag-ijmp or  flag-icall or flag-sts or flag-adiw or
flag-sbiw or flag-ldd or flag-std or
flag-mul or flag-muls or flag-fmul or flag-fmulsu or flag-mulsu or
flag-break or flag-call or flag-jmp or
flag-spm or flag-movw or
flag-lds or
  Constant atmega-16+

flag-all
flag-pop or flag-push or flag-ijmp or  flag-icall or flag-sts or flag-adiw or
flag-sbiw or flag-ldd or flag-std or
flag-mul or flag-muls or flag-fmul or flag-fmulsu or flag-mulsu or
flag-spm or flag-movw or
flag-lds or
  Constant atmega-8

flag-all
flag-pop or flag-push or flag-ijmp or  flag-icall or flag-sts or flag-adiw or
flag-sbiw or flag-ldd or flag-std or
flag-spm or flag-movw or
flag-lds or
  Constant tiny-13/2313

flag-all
flag-pop or flag-push or flag-ijmp or  flag-icall or flag-sts or flag-adiw or
flag-sbiw or flag-ldd or flag-std or
flag-lds or
  Constant at-90s/43

flag-all
flag-pop or flag-push or flag-ijmp or  flag-icall or flag-sts or flag-adiw or
flag-sbiw or flag-ldd or flag-std or
  Constant tiny-26

flag-all
  Constant tiny-11/12

atmega-16+ Value device \ The device we use.

\ regsisters
 0 constant r0
 1 constant r1
 2 constant r2
 3 constant r3
 4 constant r4
 5 constant r5
 6 constant r6
 7 constant r7
 8 constant r8
 9 constant r9
10 constant r10
11 constant r11
12 constant r12
13 constant r13
14 constant r14
15 constant r15
16 constant r16
17 constant r17
18 constant r18
19 constant r19
20 constant r20
21 constant r21
22 constant r22
23 constant r23
24 constant r24
25 constant r25
26 constant r26
27 constant r27
28 constant r28
29 constant r29
30 constant r30
31 constant r31
' r26 alias XL
' r27 alias XH
' r28 alias YL
' r29 alias YH
' r30 alias ZL
' r31 alias ZH

0 Value test-flag
0 Value op-xt
: all-models flag-all to test-flag ;

\ ~********execution and test commands********~

: (op?:)
  create does> -1 abort" not supported" ;

: op:
  test-flag device and IF op-xt execute ELSE drop (op?:) THEN ;