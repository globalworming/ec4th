15 Constant boardlength
16 Constant cellsize
\ 8 constant byte m+sste das selbe wie cells sein
\ : scherz boardlength 1 <= abort" Soll das nen Scherz sein?" ;
VARIABLE row
VARIABLE lösungscount
2VARIABLE columnflag 
2VARIABLE diagonale/
2VARIABLE diagonale\

\ xxxxxxxx for testing xxxxxxxxxxx
\ : 2+! ( w1 w2 a_addr -- )
\   tuck +! cell+ +! ;
\ : 2? dup cell +  ? ? ;
\   : testvalues 
\       %10001000 %00000101 diagonale/ 2!
\      %10001000 %00000101 diagonale\ 2!
\      %10001000 %00000101 columnflag 2!
\  ;
\  
\  : testprint 2 base ! columnflag 2? diagonale/ 2? diagonale\ 2? decimal ;
\ xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
: allfieldstrue! \ funktioniert
0 0 columnflag 2!
0 0 diagonale\ 2!
0 0 diagonale/ 2!
;

: columntrue?  ( column -- flag ) \ getestet und mit qualitätsmerkmal "funktioniert" ausgestattet
    columnflag 2@  rot dup cellsize  < if
	1 swap lshift rot drop  
      ELSE 
	1 swap cellsize - lshift nip
      THEN
      AND 0= 
;	

: diagonaletrue? ( column -- flag )
      
  dup diagonale\ 2@ rot   
      row @ - boardlength +    
      dup cellsize < IF
	1 swap lshift rot drop
      ELSE
	1 swap cellsize - lshift nip
      THEN
      AND 0=
   
  SWAP diagonale/ 2@ rot 
     row @ +           
      dup cellsize < IF
	1 swap lshift rot drop
      ELSE
	1 swap cellsize - lshift nip
      THEN
      AND 0= AND 
;

: canBePlacedHere? ( column -- flag ) 
  dup columntrue? SWAP diagonaletrue? AND  
;
: columnfree ( column -- )
    dup cellsize < IF
      1 swap lshift NEGATE 0 swap
    ELSE
      1 swap cellsize - lshift NEGATE 0
    THEN
    columnflag 2+! ;

: diagonale\free
   row @ - boardlength + 
    dup cellsize < if
     1 swap lshift NEGATE 0 swap 
    ELSE
     1 swap cellsize - lshift NEGATE 0 
    THEN
    diagonale\ 2+! ;

: diagonale/free
     row  @ + 
      dup cellsize < IF
	1 swap lshift NEGATE 0 swap
      ELSE
	1 swap cellsize - lshift NEGATE 0 
      THEN
      diagonale/ 2+! ;

: markFieldsAsFree ( column -- ) \ funktioniert

    dup columnfree
    dup diagonale\free
    diagonale/free
   
;      

: markColumn ( column -- )
    columnflag 2@ rot 
    dup cellsize < IF
      1 swap lshift or 
    ELSE
      1 swap cellsize - lshift rot or swap
    THEN
    columnflag 2!
;
: markDiag\ ( column -- )
     diagonale\ 2@ rot 
    row @ - boardlength + 
     dup cellsize < IF
       1 swap lshift or
     ELSE
       1 swap cellsize - lshift rot or  swap
     THEN
     diagonale\ 2!
;
: markDiag/ 
    diagonale/ 2@ rot 
    row @ +   
    dup cellsize < IF
      1 swap lshift or 
    ELSE
      1 swap cellsize - lshift rot or  swap
    THEN
    diagonale/ 2!
;    
: markFieldsAsOccupied ( column -- )
    dup markColumn
    dup markDiag\
    markDiag/
;
: printResult ( boardlength*column -- boardlength*column )
 13 emit 10 emit    boardlength 0 DO
     row  @ I - 64 + EMIT I PICK 1 + . \ zeile spalte in Form von z.B A1 B7 C4  
    LOOP CR
    ." Ist Loesung Nummer " lösungscount ?  
    1 lösungscount +!
;

: firstcolum ( -- 0 ) 
    0
;

: nextcolum ( n -- n 1 + )
    1 +
;

: row+  
    1 row +! 
;

: row- 
    -1 row +! 
;

: finished?
    row  @ -1 =
;

: checkvalues ( column1 column2 ... columnN -- column )
   dup boardlength < IF EXIT THEN        
   row  @ 0 = IF drop row- EXIT THEN      \ wenn row 0 erreicht, wird nur noch für finished row- gesetzt
   drop row- dup markFieldsAsFree        
   nextcolum recurse                     
;
\ VARIABLE count
: placequeens ( column -- )
BEGIN  
    dup canBePlacedHere?  IF            \ prüft flags
	dup markFieldsAsOccupied	
	row+		  		\ nächste Zeile
	row  @ boardlength = IF		 \ wenn über das Spielfeld hinaus
	    printResult			 \ gib die Lösung aus
	    row-			 \ gehe zurück aufs Spielfeld
	    markFieldsAsFree		 
	    row-			 \ vorherige Zeile 
	    dup markFieldsAsFree		 
	    nextcolum			 
	ELSE				 
	    firstcolum
	THEN
    ELSE 				
	nextcolum 			
    THEN
    checkvalues
finished? UNTIL ;

: .results 
    1 lösungscount !
    allfieldstrue!
    0 row !
    firstcolum placequeens
;


