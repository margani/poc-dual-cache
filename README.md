# Dual Cache PoC

This is the PoC for using two redis instances with two ConnectionMultiplexers so it reads from and writes to both using two separate Database objects from two separate ConnectionMultiplexer.

The result is that after running TestPutGet method, it writes to both and the values are the same when reading from both.
