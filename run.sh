#!/bin/sh

gforth -e "fpath= .|`pwd`" avrtest.fs -e bye
