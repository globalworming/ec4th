UNDEF-WORDS

: (>r)
	rp@ cell+ @ rp@ ! rp@ cell+ ! ;

: >r ( w -- )
	(>r) ;


: rdrop ( -- )
	r> r> drop >r ;

: 2>r ( w1 w2 -- )
	swap r> swap >r swap >r >r ;

: 2r> ( -- w1 w2 )
	r> r> swap r> swap >r swap ;

: 2r@ ( -- w1 w2 )
	i' j ;

: 2rdrop ( -- )
	r> r> drop r> drop >r ;

: over ( w1 w2 -- w1 w2 w1 )
	sp@ cell+ @ ;

: drop ( w -- )
	IF THEN ;

Variable (swap)
: swap ( w1 w2 -- w2 w1 )
	>r (swap) ! r> (swap) @ ;

: dup ( w -- w w )
	sp@ @ ;

: rot ( w1 w2 w3 -- w2 w3 w1 )
	>r swap r> swap ;

: -rot ( w1 w2 w3 -- w3 w1 w2 )
	rot rot ;

: nip ( w1 w2 -- w2 )
	swap drop ;

: tuck ( w1 w2 -- w2 w1 w2 )
	swap over ;

: ?dup ( w -- w )
	dup IF dup THEN ;

: pick ( u -- w )
	1+ cells sp@ + @ ;

: 2dup ( w1 w2 -- w1 w2 w1 w2 )
	over over ;

: 2over ( w1 w2 w3 w4 -- w1 w2 w3 w4 w1 w2 )
	3 pick 3 pick ;

: 2swap ( w1 w2 w3 w4 -- w3 w4 w1 w2 )
	rot >r rot r> ;

: 2rot ( w1 w2 w3 w4 w5 w6 -- w3 w4 w5 w6 w1 w2 )
	>r >r 2swap r> r> 2swap ;

: 2nip ( w1 w2 w3 w4 -- w3 w4 )
	2swap 2drop ;

: 2tuck ( w1 w2 w3 w4 -- w3 w4 w1 w2 w3 w4 )
	2swap 2over ;

ALL-WORDS
