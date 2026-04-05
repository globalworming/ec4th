.PHONY: all clean forth2012-report wokwi-tests

SYM ?= output/ec4th-arduino-nano-regular.sym
OUT ?= doc/forth2012-core-wordset-coverage.md

all:
	bash build.sh

forth2012-report:
	python3 tools/forth2012_wordset_report.py "$(SYM)" "$(OUT)"

clean:
	rm -rf output

wokwi-tests:
	if [ -f ./.env ]; then set -a; . ./.env; set +a; fi; ./bin/wokwi-tests.sh
