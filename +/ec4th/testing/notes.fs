
: print-hello 
  ." Hello world" cr ;

: empty-body ;

: test-execute \ expect: Hello world\nBack again
  ['] print-hello execute
  ." Back again" cr ;

: test-find \ expect: 0
  ['] print-hello
  s" print-hello" find-name name>xt drop - 0<> .sx drop ;

: test-find-nothing \ expect: 0
  s" asdfasdf" find-name .sx cr drop ;

: test-parse-word \ expect: abc
  s" abc 123" #tib ! >tib ! 0 >in ! parse-word type cr ;

: test-min-n 
  min-n .sx cr drop ;

: test-digit
  'x 10 digit .sx cr 2drop
  '8 10 digit .sx cr 2drop ;

: test->number 
  ." Number" cr
  0. s" 123" .sx >number .sx cr 2drop 2drop ;

: test-snumber? 
  ." snumber?" cr
  s" 123" .sx snumber? .sx cr 2drop ;

: test-sp! \ expect: 0
  sp@ >r 1 2 3 r> sp! .sx cr ;

: test-rp! \ 52c
   1324 >r rp@ 1 >r 2 >r 3 >r rp! r> .sx drop cr ;

: test-evaluate-123 \ expect: 123
  ." evaluate" cr
  s" 123" evaluate .sx cr drop ;

Defer deferred

\ ' print-hello is deferred
hex
' print-hello .
decimal

UNLOCK tlast @ LOCK 
dup 20 tdump

$019a 32 tdump

: test-defer deferred ;

123 Value a-test-value

UNLOCK tlast @ LOCK 
dup 32 tdump
$01a0 32 tdump

' noop 32 tdump

' empty-body 32 tdump

: testing
   print-hello
   ['] print-hello .sx cr
   execute
   ['] noop execute
   rp@ .sx cr drop
   test-defer
   rp@ .sx cr drop
   .sx cr
   a-test-value .sx cr drop 
   test-execute
   test-find
   test-find-nothing
   test-parse-word
   test-min-n
   test-digit
   test->number
   test-snumber?
   test-sp!
   test-rp!
   test-evaluate-123
   4711 .x cr
   /line .x cr
   tib .x cr
   sp0 .x sp@ .x 1 2 3 .sx  depth .x drop drop drop cr ." hello world" cr words cr
  ;