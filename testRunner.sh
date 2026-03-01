set -ex 
 #create the testsuitefile by merging
 # startcode - words - wordexecution - endcode
SUITEFILE="testsuite.fs"
CAPFILE="avr.cap"

# get cmd options
while test -n "$1"; do
  case "$1" in
    --help) 
	  echo "Alles *.suite.fs dateien aus dem Ordner testsuite werden mit initialisation.fs uns suiteend.fs zu einem Image das auf dem simduino durchläuft. Es erfolgt eine Auwertung der ergebnisse. Option: -v für minimalen output" ; exit 1 ;;
    -v)VERBOSE=TRUE; shift 1;;
    -*) echo "Fehler: $1. Benutze >sh testRunner.sh --help< für Informationen zu Optionen und Argumenten  " ; exit 1 ;;
    *) break;;
  esac
done


concatStrToSuite() {
  echo "\n $*">>$SUITEFILE
}

# add suitefiles
cat testsuites/initialisation.fs > testsuite.fs
for SUITE in testsuites/*.suite.fs ; do
  concatStrToSuite "$(cat $SUITE) \n "
done

# add wordexecution
concatStrToSuite " : xy test-all "
# searchs file for colondefinitions -> add to img
#for SUITE in testsuites/*.suite.fs ; do
#  concatStrToSuite "$(  awk ' /: test-/  {print $2 }' $SUITE )"
#done
concatStrToSuite " ende  "
concatStrToSuite " ; \n $(cat testsuites/suiteend.fs)"

# create img
gforth -e "fpath= .|`pwd`" $SUITEFILE -e bye
echo "" > $CAPFILE

# run on microcontroller
# avrdude -c stk500v1 -p m328p -b 57600 -P /dev/ttyUSB0 -U flash:w:avr.img:r
# read from USB
#cat /dev/ttyUSB0 >> minicom.cap &
# CAT=$!

~/proj-svn/simavr/examples/board_oursim/simduino avr.img  >> $CAPFILE
AVR=$!
 
for line in $(cat $CAPFILE) ; do
    if [ "$line" = "End_of_suite" ] ; then
      kill $CAT
      kill $AVR
      bash automTestAuswertung.sh $CAPFILE $VERBOSE >> "results.fs"
#   rm minicom.cap
    fi 
done 
