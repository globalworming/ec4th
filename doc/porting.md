
## Return stack

The return stack starts at a high memory location and grows downwards. In ec4th there are defined fillers
for `r@`, `rp!`, `i` and `k`, which assume that `rp@` is always cell aligned and pointing to the next written return
stack element. In 8 bit machines, that would be different internally. If `$ffff` would be the internal pointer, rp@
needs to return `$fffe`. The following must be true: `: r@ rp@ cell+ @ ;`.

The rationale behind this is that if the return stack is starting from `$ffff` an initial value of `$0000` is avoided.
