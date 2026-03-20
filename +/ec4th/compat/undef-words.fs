\ +/eckernel/compat/undef-words.fs define undef-words if not cross compiling 20100218;jw

s" gforthcross" environment? [IF] drop [ELSE]

\ If we compile not in cross make undef-words a noop. The idea is that we
\ only define a minimal wordset, so the high level primitive replacements
\ will be always needed and don't need a conditional compile.

: UNDEF-WORDS ;

: ALL-WORDS ;

[THEN]
