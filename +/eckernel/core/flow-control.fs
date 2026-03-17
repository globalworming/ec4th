\ flow control: IF THEN ELSE and DO LOOP

\ This is from GForth cbr.fs

\ Structural Conditionals, based on branches               12dec92py

\ Copyright (C) 1995,1996,1997,1999 Free Software Foundation, Inc.

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

: ?struc      ( flag -- )       abort" unstructured " ;
: sys?        ( sys -- )        dup 0= ?struc ;
: >mark       ( -- sys )        here  0 , ;
: >resolve    ( sys -- )        here over - swap ! ;
: <resolve    ( sys -- )        here - , ;


\ GForth special, leave out in ec4th
\ : BUT       sys? swap ;                      immediate restrict
\ : YET       sys? dup ;                       immediate restrict

\ Structural Conditionals                              12dec92py

: AHEAD     postpone branch >mark ;           immediate restrict
: IF        postpone ?branch >mark ;          immediate restrict
: THEN      sys? dup @ ?struc >resolve ;     immediate restrict
: ELSE      sys? postpone AHEAD swap postpone THEN ;
                                             immediate restrict

' THEN Alias ENDIF immediate restrict

: BEGIN     here ;                           immediate restrict
: WHILE     sys? postpone IF swap ;           immediate restrict
: AGAIN     sys? postpone branch  <resolve ;  immediate restrict
: UNTIL     sys? postpone ?branch <resolve ;  immediate restrict
: REPEAT    over 0= ?struc postpone AGAIN postpone THEN ;
                                             immediate restrict

\ This is from GForth cloop.fs

\ Structural Conditionals, loops part                  12dec92py

\ Structural Conditionals                              12dec92py

Variable leavings

: (leave)   here  leavings @ ,  leavings ! ;
: LEAVE     postpone branch  (leave) ;  immediate restrict
: ?LEAVE    postpone 0= postpone ?branch  (leave) ;
                                             immediate restrict

: DONE   ( addr -- )  leavings @
  BEGIN  2dup u<=  WHILE  dup @ swap >resolve  REPEAT
  leavings ! drop ;                          immediate restrict

\ Structural Conditionals                              12dec92py

: DO        postpone (do)   here ;            immediate restrict

: ?DO       postpone (?do)  (leave) here ;
                                             immediate restrict

\ GForth special, leave out in ec4th
\ : FOR       postpone (for)  here ;            immediate restrict

: loop]     dup <resolve 2 cells - postpone done postpone unloop ;

: LOOP      sys? postpone (loop)  loop] ;     immediate restrict
: +LOOP     sys? postpone (+loop) loop] ;     immediate restrict

\ GForth special, leave out in ec4th
\ : NEXT      sys? postpone (next)  loop] ;     immediate restrict

: EXIT postpone ;s ; immediate restrict
: ?EXIT postpone IF postpone EXIT postpone THEN ; immediate restrict


