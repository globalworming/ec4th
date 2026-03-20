\ hexonlyinput.fs h# prefix for hexadecimal numbers 26sep02jaw
\ Version $Id: hexonlyinput.fs,v 1.2 2002/10/02 16:22:23 jeans Exp $

0 [IF]

Provides h# to input hex numbers. This is
a minimalistic version of number input you find
usally in a forth interpreter. Negative numbers 
are not supported.

Normal number input does not work, all numbers
have to be prefixed by the h# word.

Compatibility:
Open Boot: No negative numbers, no compilation

[THEN]

has? compiler
error" hexonlyinput makes only sense for interpreter-only forth systems"

require eckernel/strings/charconversion.fs
require eckernel/interpreter/notfound.fs

decimal

: h# ( -- u )
\G minimalistic parsing of a hex number
\G no error checking is done
  parse-word bounds swap >r >r 0
  BEGIN r@ I' xor
  WHILE d# 4 lshift
        r@ c@ d# 16 digit drop or
        r> char+ >r
  REPEAT
  r> r> 2drop ;

Defer number

' notfound is number

