
0 [IF]

Current shortcomings:
- DO .. LOOP and I not implemented in a portable manner
  Problem is I and R@ are aliases in the target. This works
  only if target and host have the same return stack layout.
  (some holds for J K and so on... )

[THEN]

unlock >cross

: ?xt-see ( xt | 0 -- )
  ?dup IF xt-see THEN ;

: .ghostinfo ( ghost -- )
  cr ." --GHOSTNAME: " dup .ghost
  cr ." --MAGIC: " dup >magic @ 
  CASE
    <fwd> OF ." <fwd>" ENDOF
    <imm> OF ." <imm>" ENDOF
    <res> OF ." <res>" ENDOF
    <skip> OF ." <skip>" ENDOF
    <do:> OF ." <do:>" ENDOF
    dup .
  ENDCASE
  cr ." --TARGET-ADDR: " dup >link @ .addr
  cr ." --EXEC: " dup >exec @ ?xt-see
  cr ." --EXEC2: " dup >exec2 @  ?xt-see
  cr ." --EXEC-COMPILE: " dup >exec-compile @  ?xt-see
  cr ." --DO:GHOST: " dup >do:ghost @ ?dup IF recurse THEN 
  drop ;


: xt-dbg ( xt -- )
\ debugging aid
  nestxt ?exit (debug) leave-d ;

: isee ( "name" -- )
  ghost >exec2 @ xt-see ;

: idbg ( "name" -- )
  ghost >ghost-xt @ xt-dbg ;

>minimal

: !! idbg ;

\ dictionary setup
also forth definitions previous

Vocabulary instant-words
' instant-words >wordlist Constant instant-words-wordlist

Vocabulary instant-prims
: >instant-prims
  also instant-prims definitions previous ;

Vocabulary instant-alternatives
: >instant-alternatives
  also instant-alternatives definitions previous ;

>cross

Variable skip-colon
skip-colon off

: sc{ skip-colon @ ABORT" INSTANT: skip-colon should be off"
      skip-colon on ;

: }sc skip-colon off ;

: TPAI postpone sc{ postpone TPA postpone }sc ; immediate

pa: lit, dup TPAI lit, POSTPONE literal ;pa
pa: alit, dup TPAI alit, POSTPONE literal ;pa


\ Compiling of XTs : colon, colonmark, colon-resolve

Struct
  \ must be fist, so we can perfom it
  cell% field colonmark-hostxt
  cell% field colonmark-origaddr
End-Struct colonmark-struct


: placestruct ( .. u2 u1 align size -- addr )
\g the stack contents are places cell by cell
\g into the newly allocated space of the structure
\g the TOS will be the last element stored.
\g Usage: 1 2 xy-struct placestruct
  over 1 cells <> ABORT" placestruct only for cells"
  tuck %allocate throw dup >r 
  over + swap 1 cells / 0
  ?DO 1 cells - tuck ! LOOP drop r> ;  

: ghost>icfa ( ghost -- host-cfa )
  dup >created @ IF   >ghost-xt @ EXIT THEN
  dup >exec @ ?dup IF ['] NoExec <> IF >exec @ EXIT THEN THEN
  dup >exec2 @ ?dup IF nip EXIT THEN 
  drop -1 ABORT" INSTANT: missing execution semantics" ;


: tcfa>icfa ( tcfa -- host-cfa )
\ dup .addr
\ dup .ghost space
  gdiscover2 0= 
  IF   cr ." Referenced XT: " .addr -1 ABORT" INSTANT: missing ghost" THEN
  ghost>icfa ;

pa: colon, 
  dup TPA colon, 
  skip-colon @ IF drop ELSE tcfa>icfa compile, THEN ;pa

\ perform will do it
 : PERFORMFORWARD ( colonmark-struct-addr -- ) 
  @ dup 0 = ABORT" forward reference of call not resolved"
  EXECUTE ;

pa: colonmark, ( -- addr )
  skip-colon @
  IF
    -1 \ hostxt unknown, skipped
    TPA colonmark,
    skip-colon on
    ( hostxt origaddr ) colonmark-struct placestruct 
  ELSE 
    0 \ hostxt unknown
    TPAI colonmark,
    ( hostxt origaddr ) colonmark-struct placestruct 
    \ we have to use perfrom here otherwise forward references to r@
    \ won't work, FIXME: other solution?!
    dup postpone literal POSTPONE perform
  THEN ;pa

pa: colon-resolve ( tcfa addr -- )
  dup colonmark-hostxt @ 0= IF
    \ skip-colon was no active
    2dup swap tcfa>icfa swap !
  THEN
  colonmark-origaddr @ TPAI colon-resolve ;pa


\ Stack for control structures
20 cells Constant cs-stack-size
Create cs-stack cs-stack-size allot
Variable cs-sp
cs-stack cs-sp !

: cs-push ( n -- )
  cs-sp @ tuck ! cell+ dup cs-sp ! 
  cs-stack - cs-stack-size u> ABORT" cs-stack overflow" ;

: cs-pop ( -- n )
  cs-sp @ 1 cells - dup cs-stack u< ABORT" cs-stack underflow"
  dup @ swap cs-sp ! ;

pa: cs-swap ( x1 x2 -- x2 x1 )
  cs-pop cs-pop swap cs-push cs-push 1 cs-roll ;pa



\ ahead if then else begin while again until repeat

pa: if, ( CS: -- x )
  TPAI if, cs-push POSTPONE if ;pa

pa: else, ( CS: x -- x )
  cs-pop TPAI else, cs-push POSTPONE else ;pa

pa: then, ( CS: x -- )
  POSTPONE then cs-pop TPAI then, ;pa

pa: begin, ( CS: -- x )
  TPAI begin, cs-push POSTPONE begin ;pa

pa: while, ( CS: x -- x x2 )
  cs-pop TPAI while, swap cs-push cs-push POSTPONE while ;pa

pa: again, ( CS: x -- )
  POSTPONE again cs-pop TPAI again, ;pa

pa: until, ( CS: x -- )
  POSTPONE until cs-pop TPAI until, ;pa

pa: repeat, ( CS: x1 x2 -- )
  POSTPONE repeat cs-pop cs-pop swap TPAI repeat, ;pa



\ do ?do loop +loop for next leave ?leave

pa: do, ( CS: x )
  TPAI do, cs-push POSTPONE DO ;pa

pa: ?do, ( CS: x )
  TPAI ?do, cs-push POSTPONE ?DO ;pa

pa: for, ( CS: x )
  TPAI for, cs-push POSTPONE FOR ;pa

pa: loop, ( CS: x -- )
  POSTPONE LOOP cs-pop TPAI loop, ;pa

pa: +loop, ( CS: x -- )
  POSTPONE +LOOP cs-pop TPAI +loop, ;pa

pa: next, ( CS: x -- )
  POSTPONE NEXT cs-pop TPAI next, ;pa

pa: leave, ( -- )
  POSTPONE LEAVE TPAI leave, ;pa

pa: ?leave, ( -- )
  POSTPONE ?LEAVE TPAI ?leave, ;pa

Cond: ." 
  save-input sc{ cond-target }sc restore-input throw
  POSTPONE ." ;Cond

Cond: S" 
  sc{ cond-target }sc 
  last-string 2@ swap POSTPONE literal POSTPONE literal ;Cond

Cond: C" 
  sc{ cond-target }sc 
  last-string 2@ drop tchar - POSTPONE literal ;Cond

Cond: ABORT" 
  save-input sc{ cond-target }sc restore-input throw
  POSTPONE abort" ;Cond

pa: fini, TPAI fini, ;pa

Cond: EXIT sc{ cond-target }sc POSTPONE exit ;Cond

Cond: postpone 
  >in @ sc{ cond-target }sc >in ! 
  bl word gfind 0= throw
  \ if the postponed word has special executing semantics we, take these
  dup >exec-compile @ ?dup
  IF   nip compile, EXIT
  THEN
  dup >magic @ <imm> =
  IF   ghost>icfa compile, 
  ELSE ghost>cfa POSTPONE Literal POSTPONE colon, THEN ;Cond

\ MAXU MINI and MAXI call in turn lit,, we just take care that nothing bad happens
: t>hcell? ( -- )
  tcell 1 cells u> 
  ABORT" Sorry, target cells size is greater than host cell size" ;

Cond: MAXU t>hcell? cond-target ;Cond
Cond: MINI t>hcell? cond-target ;Cond
Cond: MAXI t>hcell? cond-target ;Cond

pa: dodoes, ( -- )
  TPAI dodoes, ;pa

\ order

Defer colon-definition-finish ( xt -- )
: define-colon-exec2 ( xt -- )
  Last-Header-Ghost @ >exec2 ! ;

0 \ANSI drop 1
[IF]
: start-definition ( adr len -- x )
  2drop :noname ;
[ELSE]
: start-definition ( adr len -- x )
  >r >r
  get-current instant-words-wordlist set-current
  r> r> header, set-current here (:noname) ;
[THEN]

pa: colon-start 
  last-header-ghost @ >ghostname start-definition
  postpone [ 
  ['] define-colon-exec2 IS colon-definition-finish 
  ;pa

pa: colon-end 
   Warnings @ >r Warnings off
   ] postpone ; 
   r> Warnings !
   colon-definition-finish 
;pa

: idoes> ( -- pfa )
\g similar to gdoes> in cross, perhaps we can unify it
  executed-ghost @
  >link @ X >body ;

: (iidh) :noname postpone idoes> postpone [ 
  ['] define-colon-exec2 IS colon-definition-finish
  ;

' (iidh) IS instant-interpret-does>-hook

: tdoes> ( ghost -- ) 
  >do:ghost @
  dup >link @ !does 
  >exec2 @ last-header-ghost @ >exec2 ! ;

: define-colon-does> ( xt -- ) 
  Last-Header-Ghost @ >do:ghost @ >exec2 ! ;

Cond: DOES> 
  \ copied from does>
\  last-header-ghost @ cr ." DOES> " .ghost
  \ finish simulation compilation of constructor part
  >r last-header-ghost @ POSTPONE literal POSTPONE tdoes> 
     postpone ; colon-definition-finish r>
  \ define >do:ghost if needed
  Last-Header-Ghost @ >do:ghost @ 0=
  IF align here ghostheader Last-Header-Ghost @ >do:ghost ! THEN
  \ finish compilation of consturtor part in target
  sc{ compile (does>) doeshandler, }sc resolve-does>-part
  \ TOS is  stackdepth of cross-compiler
  >r ] :noname POSTPONE idoes> postpone [ r>
  ['] define-colon-does> IS colon-definition-finish
  ;Cond

>cross

: (setup-prim-semantics) ( -- )
  last-prim-ghost @ >ghostname
  ['] instant-prims >wordlist search-wordlist
  0= ABORT" no simulting primitive found"
  last-prim-ghost @ >exec ! ;

' (setup-prim-semantics) IS setup-prim-semantics

: (setup-execution-semantics) ( -- )
  last-header-ghost @ >ghostname
  ['] target >wordlist search-wordlist
  0= ?EXIT
  last-header-ghost @ >exec ! 
  ;			
  ' (setup-execution-semantics) IS setup-execution-semantics

: (texecute) ( target-xt -- )
  dup >r
  tcfa>icfa EXECUTE r> drop ;				
  ' (texecute) IS texecute

\ immediate restrict					29aug01jaw

>cross

: restrictabort
  -1 ABORT" INSTANT: restricted, may not be interpreted" ;

>target

: immediate ( -- )
  X immediate
  \ we keep compile semantics if defined, this is for ( \ \g...
  \ S" and so on. BTW: If we redefine a definition we also
  \ create another ghost and the compile-time behavior goes
  \ away
  lastghost >exec-compile @ ?EXIT
  lastghost >ghost-xt @ lastghost >exec-compile ! ;


: restrict ( -- )
  X restrict restrict ;
\  ['] restrictabort lastghost >exec ! ;

\ cross vocabularies					24aug01jaw

>cross

Variable voc-list

struct
  cell% field voc-link
  \ wordlist for ghosts of this vocabulary in the host system
  cell% field voc-ghostwordlist
  \ pointer to wordlist in target
  cell% field voc-target
  \ last pointer of this vocabulary
  cell% field voc-last
  \ name of wordlist if defined by vocabulary
  cell% field voc-name
end-struct voc-struct

Create forth-voc
  voc-list linked 
  ghosts-wordlist , 
  0 ,
  0 ,
  s" forth" string,

Variable current-voc
20 Constant wids-max
Variable wids-num
Variable wids wids-max cells allot

forth-voc wids ! 1 wids-num !

: x-wordlist ( adr len target-wid -- cross-wid )
\G define a wordlist and give it a name (may be  0 0)
  wordlist align voc-list linked , , 0 , string, align  ;  

: twid>xwid ( target-wid -- cross-wid | 0 )
  voc-list
  BEGIN @ dup WHILE 2dup voc-target @ =
        IF nip EXIT THEN
  REPEAT nip 
  -1 ABORT" Instant: supposed wordlist-id is wrong"
  ;

: xwid>twid ( cross-wid -- target-wid | 0 )
  voc-target @ ;

: atleast1 ( n -- )
  1 < ABORT" CROSS: at least one entry expected in word-list" ;

: (set-context) ( xwid -- )
  wids-num @ atleast1 wids ! ;

: (get-context) ( -- xwid )
  wids-num @ 0> IF wids @ ELSE 0 THEN ;

: .voc-name ( xwid -- )
\G prints name of wordlist or target addr
  dup voc-name count ?dup
  IF type space drop ELSE drop voc-target @ .addr THEN ;

Create voc-name-buf 300 allot

: setup-target-search-order ( -- )
  turnkey 
  wids-num @ cells wids +
  BEGIN dup wids u> WHILE 
        1 cells - dup @ voc-ghostwordlist @ swap
  REPEAT drop 
  get-order wids-num @ + set-order ;
' setup-target-search-order IS +++]]-hook

: isearch-ghosts ( adr len -- cfa true | 0 )
  wids-num @ cells wids + >r wids
  BEGIN dup r@ u< WHILE 
        >r 2dup r@ @ voc-ghostwordlist @ search-wordlist
        IF rdrop rdrop nip nip true EXIT THEN
        r> cell+
  REPEAT drop rdrop 2drop 0 ;
  ' isearch-ghosts IS search-ghosts

>target

: vocabulary ( "name" -- )
  [G'] Vocabulary >exec2 @ ?dup
  IF   >in @ bl word count voc-name-buf place >in !
       execute 0 voc-name-buf !
  ELSE bl word count 0 x-wordlist
  THEN ;

: wordlist ( -- wid )
  [G'] wordlist >exec2 @ ?dup
  IF   execute ( S -- twid ) >r
       voc-name-buf count r@ x-wordlist r>
  ELSE drop 0 x-wordlist
  THEN ;

: search-wordlist ( target-addr len twid -- 0 | xt +-1 )
\ searches for string in word-list
  \ dup count type space
  >r >r >image r> r>
  twid>xwid voc-ghostwordlist @ search-wordlist
  IF   >body dup ghost>cfa swap 
       immediate? IF 1 ELSE -1 THEN
  ELSE 0 THEN ;

: find ( c-addr -- c-addr 0 | xt +-1 )
  dup >r th-count gsearch
  \ FIXME: better check forward reference here?
  dup IF rdrop >link @ ELSE r> THEN swap ;

: vocnumokay? ( n -- n )
  dup 0< ABORT" CROSS: search-order underflow" 
  dup wids-max > ABORT" CROSS: search-order overflow" ;

: get-order ( -- wid1 ... widn n )
  wids-num @ cells wids +
  BEGIN dup wids u> WHILE 
        1 cells - dup @ xwid>twid swap
  REPEAT drop wids-num @ ;

: set-order ( wid1 ... widn n -- )
  vocnumokay? dup wids-num ! cells wids + >r wids
  BEGIN dup r@ u< WHILE 
        swap twid>xwid over ! cell+
  REPEAT drop rdrop 
  setup-target-search-order ;

: get-current ( -- wid ) current-voc @ xwid>twid ;
: set-current ( wid -- ) 
  twid>xwid 
  dup current-voc !
  dup voc-last TO tlast
  voc-ghostwordlist @ TO current-ghosts ;

: also X get-order dup atleast1 >r dup r> 1+ X set-order ;
: previous X get-order nip 1- X set-order ;

: definitions ( -- ) X get-order over X set-current discard ;

: only ( -- ) forth-voc wids ! 1 wids-num ! ;

X only X definitions

: forth ( -- )
  forth-voc (set-context) ;  

: set-context ( twid -- )
    twid>xwid (set-context)
    setup-target-search-order
;

: order ( -- )
  wids-num @ cells wids + >r wids
  BEGIN dup r@ u< WHILE 
        dup @ .voc-name cell+
  REPEAT drop rdrop
  3 spaces current-voc @ 
  ?dup IF .voc-name ELSE ." #CURRENT-UNKNOWN#" THEN  cr order ;

: vocs ( -- )
  voc-list BEGIN @ dup WHILE dup .voc-name REPEAT drop ;

: words ( -- )
  (get-context) ?dup 
  IF voc-ghostwordlist @ >r 
     get-order r> swap 1+ set-order words
     get-order nip 1- set-order
  THEN ;



: words: ( xt "name" "name2" ... -- )
  >r
  BEGIN
     >in @ bl word swap >in ! c@ 0>
  WHILE 
     >in @ ' swap >in !
     r@ EXECUTE
  REPEAT
  rdrop ;

: oops -1 ABORT" this primitive should never be executed" ;

>cross

: transfer-wordlist-pointers ( -- )
  voc-list
  BEGIN @ dup WHILE >r r@ voc-last @ tcell - r@ voc-target @ 
        X cell+ \ FIXME: use wordlist-id of target?!
        ( S last wordlist-id-addr ) X ! r>
  REPEAT drop
  [G'] set-order >exec2 @ ?dup IF
      >r X get-order r> EXECUTE
  THEN
  [G'] set-current >exec2 @ ?dup IF
      >r X get-current r> EXECUTE
  THEN ;

>instant-prims

: @ X @ ;
: ! X ! ;
: c@ X c@ ;
: c! X c! ;
: 2@ X 2@ ;
: 2! X 2! ;

: cells X cells ;
: chars X chars ;
: cell+ X cell+ ;
: char+ X char+ ;
: 1+ 1 + ;
: 1- 1 - ;

' oops Alias ;s
' oops Alias branch
' oops Alias ?branch
' oops Alias lit

\ FIXME: for this here we have to find a solution!!
' oops Alias sp@
' oops Alias sp!
' oops Alias rp@
' oops Alias rp!

\ threading
: noop ;
: execute texecute ;

\ arithmetic set for 32 bit target and host (1 to 1 mapping)

' Alias words: + - 2* or and xor 2/ lshift rshift 0= = u< d+ um*

\ stack words, should work for 32 and 16 bit targets (1 to 1 mapping)




\ data stack							22aug01jaw

\ It is gereally a good idea to use the same cell size or more
\ as in the host system. So we can transport host pointers
\ (e.g. for get-order or save-input) on the "simulated" target stack

' Alias words: dup drop swap over rot nip 2drop 2dup

\ return stack							22aug01jaw

\ taking over I as-is has a problem (see intro)

' Alias words: >r r> r@ i

\ misc words

' Alias words: toupper 

: (find-samelen) ( u f83name1 -- u f83name2/0 )
    BEGIN  2dup cell+ c@ $1F and <> WHILE  @  dup 0= UNTIL  THEN ;

\ IO

: (emit) emit ;
: (key) key ;
: (key?) key? ;
: (type) BEGIN dup WHILE
    >r dup X c@ (emit) 1+ r> 1- REPEAT 2drop ;

\ daniels stuff

\ C calls

' oops Alias data4
' oops Alias addr4
' oops Alias ccalldata
' oops Alias ccalladdr
' oops Alias functionNames
' oops Alias functionAddrs


' oops Alias IOBUF0






>cross

: tscratch there $400 + X aligned ;
: wordscratch tscratch ;
: s"scratch tscratch $40 + ;

: >scratch ( adr len taddr -- taddr len )
  2dup X c! dup >r X char+ swap ht-move r> ;

>target

: pick pick ;
: depth depth ;


' throw ALIAS throw 
: catch tcfa>icfa catch ;


\ parsing stuff

: save-input save-input ;
: restore-input restore-input ;

: evaluate th-count evaluate ;

\ : >in -1 ABORT" don't use >in!!" ;

: word ( c -- target-addr ) 
  word count 
  wordscratch >scratch ;

: parse -1 ABORT" uses parse" ;


\ gforth conditional compiling stuff

: [AGAIN] -1 ABORT" uses [AGAIN]" ;

>minimal

: ?! ghost .ghostinfo ;


: s" [char] " parse s"scratch >scratch X count ;


lock





