

require +/eckernel/primitives/all.fs

." HERE"


require +/eckernel/compat/conditionalcompile.fs
require +/eckernel/debug/dotshex.fs
require +/eckernel/debug/dump.fs


require +/eckernel/error/error.fs

require +/eckernel/interpret/interpret.fs
require +/eckernel/interpret/accept.fs
require +/eckernel/memory/move.fs
require +/eckernel/memory/optcmove.fs
require +/eckernel/misc/flag.fs
require +/eckernel/misc/align.fs

\ require +/eckernel/misc/getdoers.fs

\ require +/eckernel/misc/defer.fs
require +/eckernel/nio/convertpad.fs
require +/eckernel/nio/dothex.fs
\ require +/eckernel/nio/hexonlyinput.fs \\\ compiler fehler
require +/eckernel/nio/input.fs
require +/eckernel/nio/output.fs
require +/eckernel/parsing/parse.fs
require +/eckernel/parsing/parseword.fs
require +/eckernel/parsing/refillcl.fs
require +/eckernel/parsing/tib.fs
require +/eckernel/strings/basics.fs
require +/eckernel/strings/comparedict.fs
require +/eckernel/strings/compare.fs
require +/eckernel/strings/string>number.fs
require +/eckernel/tib/tib.fs
\ require +/eckernel/compat/basic.fs
\ require +/eckernel/features/require.fs \\\ require wirklich required?
\ require +/eckernel/dictionary/structures.fs


require +/eckernel/dictionary/find.fs