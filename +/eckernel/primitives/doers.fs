\ Filler for doers                                10mar26jaw

\ If we leave out the compiler we need the runtime code for our defining
\ words. This file defines the defining words without the
\ interpretative/compiling part.


\ : (does>) ;    

\ doer? :dofield 0= [IF] \D compileddofillers .( DOFIELD )
\ | : (Field)  DOES> @ + ;
\ [THEN]

\ FIXME avoid these fillers
: (does>2) ;
: :doesjump ;

\ TODO add rom value support
doer? :dovalue 0= [IF]
| : Value DOES> @ ;
[THEN]

\ TODO add rom defer support
doer? :dodefer 0= [IF]
has? rom-defer [IF]
| : Defer ( "name" -- )  DOES> @ @ execute ;
[ELSE]
| : Defer ( "name" -- ) DOES> @ execute ;
[THEN]
[THEN]

\ | : 2Constant ( w1 w2 "name" -- ) 
\     DOES> ( -- w1 w2 ) 2@ ;

doer? :docon 0= [IF]
| : (Constant)  DOES> @ ;
[THEN]

doer? :douser 0= defined? up@ and [IF]
| : User DOES> @ [IFDEF] up@ up@ [ELSE] up @ [THEN] + ;
[THEN]

doer? :dovar 0= [IF]
| : Create ( "name" -- ) \ core
    DOES> ;
