\ pass.fs pass pointers from cross to target and vice versa

\ set dp of eeprom region correctly so we can save it with save-sam-region
[ifdef] eeprom-here
\ eeprom-here [[+++ eeprom >rdp ! +++]]
eeprom-here eepromStart - Constant eepromLength
Create eepromMirror eepromStart here eepromLength dup allot move
: initeepromdata
  eepromMirror eepromStart eepromLength writeeepromblock .x ;
[then]



include pass.fs
