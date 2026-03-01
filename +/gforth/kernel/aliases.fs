\ run-time routine headers

\ Copyright (C) 1997,1998 Free Software Foundation, Inc.

\ This file is part of Gforth.

\ Gforth is free software; you can redistribute it and/or
\ modify it under the terms of the GNU General Public License
\ as published by the Free Software Foundation; either version 2
\ of the License, or (at your option) any later version.

\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program; if not, write to the Free Software
\ Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111, USA.

-2 Alias: :docol
-3 Alias: :docon
-4 Alias: :dovar
-5 Alias: :douser
-6 Alias: :dodefer
-7 Alias: :dofield
-8 Alias: :dodoes
-9 Alias: :doesjump
-&10 first-primitive
Primitive noop
Primitive lit
Primitive execute
Primitive perform
has? skipbranchprims 0= [IF]
has? glocals [IF]
Primitive branch-lp+!#
[THEN]
Primitive branch
Primitive ?branch
has? glocals [IF]
Primitive ?branch-lp+!#
[THEN]
has? xconds [IF]
Primitive ?dup-?branch
Primitive ?dup-0=-?branch
[THEN]
[THEN]
has? skiploopprims 0= [IF]
Primitive (next)
has? glocals [IF]
Primitive (next)-lp+!#
[THEN]
Primitive (loop)
has? glocals [IF]
Primitive (loop)-lp+!#
[THEN]
Primitive (+loop)
has? glocals [IF]
Primitive (+loop)-lp+!#
[THEN]
has? xconds [IF]
Primitive (-loop)
has? glocals [IF]
Primitive (-loop)-lp+!#
[THEN]
Primitive (s+loop)
has? glocals [IF]
Primitive (s+loop)-lp+!#
[THEN]
[THEN]
Primitive unloop
Primitive (for)
Primitive (do)
Primitive (?do)
has? xconds [IF]
Primitive (+do)
Primitive (u+do)
Primitive (-do)
Primitive (u-do)
[THEN]
Primitive i
Primitive i'
Primitive j
Primitive k
[THEN]
Primitive move
Primitive cmove
Primitive cmove>
Primitive fill
Primitive compare
Primitive -text
Primitive toupper
Primitive capscomp
Primitive -trailing
Primitive /string
Primitive +
Primitive under+
Primitive -
Primitive negate
Primitive 1+
Primitive 1-
Primitive max
Primitive min
Primitive abs
Primitive *
Primitive /
Primitive mod
Primitive /mod
Primitive 2*
Primitive 2/
Primitive fm/mod
Primitive sm/rem
Primitive m*
Primitive um*
Primitive um/mod
Primitive m+
Primitive d+
Primitive d-
Primitive dnegate
Primitive d2*
Primitive d2/
Primitive and
Primitive or
Primitive xor
Primitive invert
Primitive rshift
Primitive lshift
Primitive 0=
Primitive 0<>
Primitive 0<
Primitive 0>
Primitive 0<=
Primitive 0>=
Primitive =
Primitive <>
Primitive <
Primitive >
Primitive <=
Primitive >=
Primitive u=
Primitive u<>
Primitive u<
Primitive u>
Primitive u<=
Primitive u>=
has? dcomps [IF]
Primitive d=
Primitive d<>
Primitive d<
Primitive d>
Primitive d<=
Primitive d>=
Primitive d0=
Primitive d0<>
Primitive d0<
Primitive d0>
Primitive d0<=
Primitive d0>=
Primitive du=
Primitive du<>
Primitive du<
Primitive du>
Primitive du<=
Primitive du>=
[THEN]
Primitive within
Primitive sp@
Primitive sp!
Primitive rp@
Primitive rp!
has? floating [IF]
Primitive fp@
Primitive fp!
[THEN]
Primitive ;s
Primitive >r
Primitive r>
Primitive rdrop
Primitive 2>r
Primitive 2r>
Primitive 2r@
Primitive 2rdrop
Primitive over
Primitive drop
Primitive swap
Primitive dup
Primitive rot
Primitive -rot
Primitive nip
Primitive tuck
Primitive ?dup
Primitive pick
Primitive 2drop
Primitive 2dup
Primitive 2over
Primitive 2swap
Primitive 2rot
Primitive 2nip
Primitive 2tuck
Primitive @
Primitive !
Primitive +!
Primitive c@
Primitive c!
Primitive 2!
Primitive 2@
Primitive cell+
Primitive cells
Primitive char+
Primitive (chars)
Primitive count
Primitive (f83find)
has? hash [IF]
Primitive (hashfind)
Primitive (tablefind)
Primitive (hashkey)
Primitive (hashkey1)
[THEN]
Primitive (parse-white)
Primitive aligned
Primitive faligned
Primitive >body
has? standardthreading has? compiler and [IF]
Primitive >code-address
Primitive >does-code
Primitive code-address!
Primitive does-code!
Primitive does-handler!
Primitive /does-handler
Primitive threading-method
[THEN]
Primitive key-file
Primitive key?-file
has? os [IF]
Primitive stdin
Primitive stdout
Primitive stderr
Primitive form
Primitive flush-icache
Primitive (bye)
Primitive (system)
Primitive getenv
Primitive open-pipe
Primitive close-pipe
Primitive time&date
Primitive ms
Primitive allocate
Primitive free
Primitive resize
Primitive strerror
Primitive strsignal
Primitive call-c
[THEN]
has? file [IF]
Primitive close-file
Primitive open-file
Primitive create-file
Primitive delete-file
Primitive rename-file
Primitive file-position
Primitive reposition-file
Primitive file-size
Primitive resize-file
Primitive read-file
Primitive read-line
[THEN]
Primitive write-file
Primitive emit-file
has? file [IF]
Primitive flush-file
Primitive file-status
[THEN]
has? floating [IF]
Primitive f=
Primitive f<>
Primitive f<
Primitive f>
Primitive f<=
Primitive f>=
Primitive f0=
Primitive f0<>
Primitive f0<
Primitive f0>
Primitive f0<=
Primitive f0>=
Primitive d>f
Primitive f>d
Primitive f!
Primitive f@
Primitive df@
Primitive df!
Primitive sf@
Primitive sf!
Primitive f+
Primitive f-
Primitive f*
Primitive f/
Primitive f**
Primitive fnegate
Primitive fdrop
Primitive fdup
Primitive fswap
Primitive fover
Primitive frot
Primitive fnip
Primitive ftuck
Primitive float+
Primitive floats
Primitive floor
Primitive fround
Primitive fmax
Primitive fmin
Primitive represent
Primitive >float
Primitive fabs
Primitive facos
Primitive fasin
Primitive fatan
Primitive fatan2
Primitive fcos
Primitive fexp
Primitive fexpm1
Primitive fln
Primitive flnp1
Primitive flog
Primitive falog
Primitive fsin
Primitive fsincos
Primitive fsqrt
Primitive ftan
Primitive fsinh
Primitive fcosh
Primitive ftanh
Primitive fasinh
Primitive facosh
Primitive fatanh
Primitive sfloats
Primitive dfloats
Primitive sfaligned
Primitive dfaligned
[THEN]
has? glocals [IF]
Primitive @local#
Primitive @local0
Primitive @local1
Primitive @local2
Primitive @local3
has? floating [IF]
Primitive f@local#
Primitive f@local0
Primitive f@local1
[THEN]
Primitive laddr#
Primitive lp+!#
Primitive lp-
Primitive lp+
Primitive lp+2
Primitive lp!
Primitive >l
has? floating [IF]
Primitive f>l
Primitive fpick
[THEN]
[THEN]
has? OS [IF]
Primitive open-lib
Primitive lib-sym
Primitive icall0
Primitive icall1
Primitive icall2
Primitive icall3
Primitive icall4
Primitive icall5
Primitive icall6
Primitive icall20
Primitive fcall0
Primitive fcall1
Primitive fcall2
Primitive fcall3
Primitive fcall4
Primitive fcall5
Primitive fcall6
Primitive fcall20
[THEN]
Primitive up!
Primitive wcall
has? file [IF]
Primitive open-dir
Primitive read-dir
Primitive close-dir
Primitive filename-match
[THEN]
Primitive newline
has? os [IF]
Primitive utime
Primitive cputime
[THEN]
has? floating [IF]
Primitive v*
Primitive faxpy
[THEN]
has? file [IF]
Primitive (read-line)
[THEN]
