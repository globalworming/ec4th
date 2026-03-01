#!/bin/bash

# # wertet tests aus, wenn eine bestimmte Form eigehalten wird:
# C ADD  	//zu testender Befehl
# T 1 2 ADD	// Werte
# E 3		// Erwartet
# O 3		//Ergebniss
# F 		//Finished
CAPFILE=$1
VERBOSE=$2
while read testOut ; do
  MARK="${testOut:0:1}"
  VALUE="${testOut:2}"
 case "$MARK" in
    "C")
      if [ ! $VERBOSE ]; then
	echo "im Test: $VALUE"
      fi
      FAILURE=0
    ;;
    "T")
      if [ ! $VERBOSE ]; then
	echo "$VALUE"
      fi
    ;;
    "E") 
      EXP="$VALUE"
    ;;
    "O") 
      OUT="$VALUE"
      if [ ! $VERBOSE ]; then
	echo "erwartet $EXP"
	echo "ausgabe $OUT"
      fi
      if [ "$OUT" != "$EXP" ]; then 
      FAILURE=$(( $FAILURE + 1 ))
      fi
    ;;
    "F")
      if [ ! $VERBOSE ]; then
	echo "Test beendet, Fehlschläge: $FAILURE ";
      fi
    if [ ! $FAILURE -eq 0 ]; then  
      FAILEDCASES=$(( FAILEDCASES + 1 ))
    fi  
    ;;
  esac 
done < $CAPFILE

if [ $FAILEDCASES -eq 0 ]; then
  echo "Alle Tests erfolgreich"
else
  echo "Tests fehlgeschlagen: $FAILEDCASES"
fi                
exit 1