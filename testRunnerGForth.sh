set -ex 
SUITEFILE="testInGForth.fs"
RESULTS=resultsGforth.fs
concatStrToSuite() {
  echo "\n $*">>$SUITEFILE
}
cat testsuites/gForthDictInitialize.fs > $SUITEFILE
for SUITE in testsuites/*.suite.fs ; do
concatStrToSuite "wordlist Constant $SUITE $SUITE dup >order set-current"
concatStrToSuite "$(cat $SUITE) \n "
concatStrToSuite " $SUITE test Root get-current 3 set-order test-all order "
done
concatStrToSuite ": end .\" End_of_suite \" ; end"
           
# for SUITE in testsuites/*.suite.fs ; do
#  concatStrToSuite "$(  awk ' /: test-all /  {print $2 }' $SUITE )"
  #searchs file for colondefinition and adds word execution to suite 
# done

gforth $SUITEFILE > $RESULTS &
GFORTH=$!
sleep 1
#for line in $(cat $RESULTS) ; do
#  if [ "$line" = "End_of_suite" ] ; then
  export FAILEDCASES=0
  bash automTestAuswertung.sh < $RESULTS
#  rm $SUITEFILE
#  fi
# done 