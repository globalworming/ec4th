\ defined in nio/dothex.fs
\ : .sx ( -- )
\ Same as .s with hexadecimal numbers.
\  depth
\  dup [char] < emit false (.x) [char] > emit space dup
\  0 ?DO dup pick .x 1- LOOP drop ;

