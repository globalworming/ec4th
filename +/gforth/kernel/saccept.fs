\ a very simple accept approach

\ Copyright (C) 1995,1996,1997,1998,1999 Free Software Foundation, Inc.

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

require ./io.fs

Variable echo
\ Defer accept

: accept-echo ( adr len -- len )
  over + over ( start end pnt )
  BEGIN
   key dup #del = IF drop #bs THEN
   dup bl u<
   IF	dup #cr = IF space drop nip swap - EXIT THEN
        dup #lf <> \ ignore lf
        IF 
           #bs = IF 3 pick over <> 
    	   IF 1 chars - #bs emit bl emit #bs emit ELSE bell THEN THEN
        ELSE
           drop
        THEN
   ELSE	>r 2dup <> IF r> dup emit over c! char+ ELSE r> drop bell THEN
   THEN 
  AGAIN ;

: accept-noecho ( adr len -- len )
  over + over ( start end pnt )
  BEGIN
   key dup #del = IF drop #bs THEN
   dup bl u<
   IF	dup #cr = IF drop nip swap - EXIT THEN
        dup #lf <> \ ignore lf
        IF 
           #bs = IF 3 pick over <> 
    	   IF 1 chars - THEN THEN
        ELSE
           drop
        THEN
   ELSE	>r 2dup <> IF r> over c! char+ ELSE r> drop THEN
   THEN 
  AGAIN ;
  
\ ' accept-echo is accept

: accept
  echo @ IF accept-echo ELSE over >r accept-noecho $13 emit r> over 4 min type cr THEN ;

: (xon)
  $11 emit ;

' (xon) is .status



