.PHONY: all clean

SYM ?= output/ec4th-arduino-nano-regular.sym
OUT ?= doc/forth2012-core-wordset-coverage.md

all:
	bash build.sh

forth2012-report:
	python3 tools/forth2012_wordset_report.py "$(SYM)" "$(OUT)"

clean:
	rm -rf output

wokwi-tests:
	mkdir -p output/test
	set -a; . ./.env; set +a; \
		wokwi-cli \
			--scenario wokwi-scenario/first.test.yaml \
			--timeout 1000 \
			--serial-log-file output/test/wokwi.serial.log
