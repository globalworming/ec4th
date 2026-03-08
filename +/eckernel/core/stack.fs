\ +/eckernel/core/stack.fs
require +/eckernel/core/vars.fs
UNDEF-WORDS


\ FIXME: muss später weg
: 1+ 
    1 + ;

: drop ( w -- )
    IF THEN ;

: 2drop ( w w -- )
    drop drop ;

: dup ( w -- w w )
    sp@ @ ;

: ?dup ( w -- w )
    dup IF dup THEN ;

: rot ( w1 w2 w3 -- w2 w3 w1 )
    >r swap r> swap ;

: -rot ( w1 w2 w3 -- w3 w1 w2 )
    rot rot ;

: over ( w1 w2 -- w1 w2 w1 )
	sp@ cell+ @ ;

: 2dup ( w1 w2 -- w1 w2 w1 w2 )
    over over ;

: pick ( u -- w )
	1+ cells sp@ + @ ;

: 2over ( w1 w2 w3 w4 -- w1 w2 w3 w4 w1 w2 )
    3 pick 3 pick ;

: 2swap ( w1 w2 w3 w4 -- w3 w4 w1 w2 )
    rot >r rot r> ;

: 2rot ( w1 w2 w3 w4 w5 w6 -- w3 w4 w5 w6 w1 w2 )
    >r >r 2swap r> r> 2swap ;

: depth ( -- +n ) \ core 
    sp@ sp0 @ swap - cell / ;

: 2rdrop ( -- )
	r> r> drop r> drop >r ;

: i' ( -- w )
    r> r> r> dup itmp ! >r >r >r itmp @ ;


: I ( -- n1 )
    r> r> swap over >r >r ;

: j ( -- n ) \ rp@ cell+ cell+ cell+ @ ;
	r> r> r> r> dup itmp ! >r >r >r >r itmp @ ;



' i Alias r@ ( -- w ; R: w -- w )


ALL-WORDS 
