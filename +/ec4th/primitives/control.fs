\ control.fs

UNDEF-WORDS
decimal

\ reads the next flashcell where the distance to jump is written,
\ add distance to the instruction pointer.
\ The distance is calculated by the cross compiler.
: branch ( -- )
	r> dup @ + >r ;

\ Question branch checks the TOS for a flag.
\ If the flag is true just set instruction pointer the next flashcell.
\ Which contains the distance and rjmp to do_next, else jump to branch.
: ?branch ( f -- )
	0= dup \ !f !f
	r> dup @ \ !f !f IP branchoffset
	rot and + \ !f IP|IP+branchoffset
	swap 0= cell and + \ IP''
	>r ;

\ builds up the loop-sys.
\ Reads two data stack elements and put them to return stack.
: (do) ( nlimit nstart -- )
	r> swap rot >r >r >r ;

: (?do) ( nlimit nstart -- )
	2dup = IF
		r> swap rot >r >r
		dup @ + >r
	ELSE
		r> swap rot >r >r
		cell+ >r
	THEN ;

\ checks if index and limit are equal,
\ if they are set instruction pointer + one cell,
\ so it jumps over distance, else jump to branch.
\ The distance is calculated by cross compiler.
: (loop) ( -- )
	r> r> 1+ r> 2dup = IF
		>r 1- >r cell+ >r
	ELSE
		>r >r dup @ + >r
	THEN ;

\ removes the loop-sys from the return stack.
: unloop ( -- )
	r> rdrop rdrop >r ;

\ adds n to the loops index, if index is greater or equal to limit
\ set instruction pointer + one cell, else jump to branch.
\ The distance is calculated by cross compiler.
: (+loop) ( n -- )
	r> swap
	r> r> 2dup - >r
	2 pick r@ + r@ xor 0< 0=
	3 pick r> xor 0< 0= or IF
		>r + >r dup @ + >r
	ELSE
		>r >r drop cell+ >r
	THEN ;

0 [IF]

\ sets up the loop-sys with index ncount and limit 0, pushed to return stack.
: (for) ( ncount -- )
	r> swap 0 >r >r >r ;

\ compares index with limit,
\ if index is equal to limit(0) set instruction pointer + one flashcell
\ and jump to do_next, else it decreases index by 1 and jumps to branch.
: (next) ( -- )
	r> r> dup 1- >r
	IF
		dup @ + >r
	ELSE
		cell+ >r
	THEN ;

\ If nlimit is equal to nstart continue execution at a location
\ given by a word later, else set up loop-sys.
: (+do) ( nlimit nstart -- )
	swap 2dup
	r> swap >r swap >r >= IF
		dup @ +
	ELSE
		cell+
	THEN >r ;

\ set up the loop-system nearly the same way as do does it,
\ but handle both elements on stack as unsigned integers.
: (u+do) ( ulimit ustart -- )
	swap 2dup
	r> swap >r swap >r
	u>= IF
		dup @ +
	ELSE
		cell+
	THEN >r ;

: (-do) ( nlimit nstart -- )
	swap 2dup
	r> swap >r swap >r
	<= IF
		dup @ +
	ELSE
		cell+
	THEN >r ;

: (u-do) ( ulimit ustart -- )
	swap 2dup
	r> swap >r swap >r
	u<= IF
		dup @ +
	ELSE
		cell+
	THEN >r ;

[THEN]

\ : perform ( a_addr -- )
\ 	@ execute ;

ALL-WORDS
