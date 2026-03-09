\ +/eckernel/strings/conversion.fs


ASDASDASDASD



\ FIXME: die Kommentare deuten nichts gutes an, was genau aber zu fixen ist ist unbekannt
: s>number? ( addr len -- d f )
    \ converts string addr len into d, flag indicates success
    sign? >r
    s>unumber?
    0= IF
        rdrop false
    ELSE \ no characters left, all ok
	r>
	IF
	    dnegate
	THEN
	true
    THEN ;

: s>number ( addr len -- d )
    \ don't use this, there is no way to tell success
    s>number? drop ;

: snumber? ( c-addr u -- 0 / n -1 / d 0> )
    s>number? 0=
    IF
	2drop false EXIT
    THEN
    dpl @ dup 0< IF
	nip
    ELSE
	1+
    THEN ; 
