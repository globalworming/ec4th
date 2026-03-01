: test-umdurchmod
  cr
  ." C um/mod" cr

  ." T 0 0 1 um/mod" cr
  ." E 0 0" cr
  ." O " 0 0 1 um/mod .x .x cr
 
  ." T 20 0 4 um/mod" cr
  ." E 5 0" cr
  ." O " 20 0 4 um/mod .x .x cr

  ." T test dat: 0 1 256 um/mod" cr
  ." E 100 0" cr 
  ." O " 0 1 256 um/mod .x .x cr

  ." T test dat: 0 1 1024 um/mod" cr
  ." E 40 0" cr
  ." O " 0 1 1024 um/mod .x .x cr

  ." T test dat: 0 1 2048 um/mod" cr
  ." E 20 0" cr
  ." O " 0 1 2048 um/mod .x .x cr

  ." T test dat: -1 0 0 um/mod" cr
  ." E Error" cr
  ." O " 0 0 0 um/mod .x .x cr

  ." T test dat: 1 0 0 um/mod" cr
  ." E Error" cr
  ." O " 1 0 0  um/mod .x .x cr

  ." T test dat: 0 1 0 um/mod" cr
  ." E Error" cr
  ." O " 0 1 0 um/mod .x .x cr

  ." T test dat: 32768 0 2 um/mod" cr
  ." E 4000 0" cr
  ." O " 32768 0 2 um/mod .x .x cr

  ." T test dat: 32768 0 16384 um/mod" cr
  ." E 2 0" cr
  ." O " 32768 0 16384 um/mod .x .x cr

  ." T test dat: 0 2 1 um/mod" cr
  ." E Error" cr
  ." O " 0 2 1 um/mod .x .x cr

  ." T test dat: 0 -1 -2 um/mod" cr
  ." E Error " cr
  ." O " 0 -1 -2 um/mod .x .x cr

  ." T test dat: 0 -2 -1 um/mod" cr
  ." E Error  " cr
  ." O " 0 -2 -1 um/mod .x .x cr

  ." T test dat: -1 0 1 um/mod" cr
  ." E FFFF 0" cr
  ." O " -1 0 1 um/mod .x .x cr

  ." T test dat: -1 0 2 um/mod" cr
  ." E 7FFF 1" cr
  ." O " -1 0 2 um/mod .x .x cr

  ." T test dat: 30 0 7 um/mod" cr
  ." E 4 2" cr
  ." O " 30 0 7 um/mod .x .x cr

  ." T test dat: 1000 0 999 um/mod" cr
  ." E 1 1" cr
  ." O " 1000 0 999 um/mod .x .x cr

  ." T test dat: 9045 0 2901 um/mod" cr
  ." E 3 156" cr
  ." O " 9045 0 2901 um/mod .x .x cr

  ." T test dat: -14586 0 27873 um/mod" cr
  ." E 1 5A25" cr
  ." O " -14586 0 27873 um/mod .x .x cr

  ." T test dat: 1097 0 18554 um/mod" cr
  ." E 0 449" cr
  ." O " 1097 0 18554 um/mod .x .x cr

  ." T test dat: -1580 0 1211 um/mod" cr
  ." E 34 3D8" cr
  ." O " -1580 0 1211 um/mod .x .x cr

  ." T test dat: 0 21289 26789  um/mod" cr
  ." E CB70 60D0 " cr
  ." O " 0 21289 26789  um/mod .x .x cr

  ." T test dat: 0 5950 6137  um/mod" cr
  ." E F833 165" cr
  ." O "0 5950 6137  um/mod .x .x cr

  ." T test dat: 0 16200 18250 um/mod" cr
  ." E E33E 1E14" cr
  ." O " 0 16200 18250 um/mod .x .x cr

  ." T test dat: 22134 5845 17439 um/mod" cr
  ." E 55CE 3A84" cr
  ." O " 22134 5845 17439 um/mod .x .x cr

  ." T test dat: 16940 15354 26272 um/mod" cr
  ." E 959D 320C" cr
  ." O " 16940 15354 26272 um/mod .x .x cr

  ." T test dat:23191 4972 26216 um/mod" cr
  ." E 308E CE7"  cr
  ." O " 23191 4972 26216 um/mod .x .x cr 
  ." F " cr
;

