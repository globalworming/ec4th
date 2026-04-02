#!/usr/bin/env python3
from __future__ import annotations

import argparse
from dataclasses import dataclass
from datetime import datetime
from pathlib import Path


CORE_WORDS = [
    "!",
    "#",
    "#>",
    "#S",
    "'",
    "(",
    "*",
    "*/",
    "*/MOD",
    "+",
    "+!",
    "+LOOP",
    ",",
    "-",
    ".",
    ".\"",
    "/",
    "/MOD",
    "0<",
    "0=",
    "1+",
    "1-",
    "2!",
    "2*",
    "2/",
    "2@",
    "2DROP",
    "2DUP",
    "2OVER",
    "2SWAP",
    ":",
    ";",
    "<",
    "<#",
    "=",
    ">",
    ">BODY",
    ">IN",
    ">NUMBER",
    ">R",
    "?DUP",
    "@",
    "ABORT",
    "ABORT\"",
    "ABS",
    "ACCEPT",
    "ALIGN",
    "ALIGNED",
    "ALLOT",
    "AND",
    "BASE",
    "BEGIN",
    "BL",
    "C!",
    "C,",
    "C@",
    "CELL+",
    "CELLS",
    "CHAR",
    "CHAR+",
    "CHARS",
    "CONSTANT",
    "COUNT",
    "CR",
    "CREATE",
    "DECIMAL",
    "DEPTH",
    "DO",
    "DOES>",
    "DROP",
    "DUP",
    "ELSE",
    "EMIT",
    "ENVIRONMENT?",
    "EVALUATE",
    "EXECUTE",
    "EXIT",
    "FILL",
    "FIND",
    "FM/MOD",
    "HERE",
    "HOLD",
    "I",
    "IF",
    "IMMEDIATE",
    "INVERT",
    "J",
    "KEY",
    "LEAVE",
    "LITERAL",
    "LOOP",
    "LSHIFT",
    "M*",
    "MAX",
    "MIN",
    "MOD",
    "MOVE",
    "NEGATE",
    "OR",
    "OVER",
    "POSTPONE",
    "QUIT",
    "R>",
    "R@",
    "RECURSE",
    "REPEAT",
    "ROT",
    "RSHIFT",
    "S\"",
    "S>D",
    "SIGN",
    "SM/REM",
    "SOURCE",
    "SPACE",
    "SPACES",
    "STATE",
    "SWAP",
    "THEN",
    "TYPE",
    "U.",
    "U<",
    "UM*",
    "UM/MOD",
    "UNLOOP",
    "UNTIL",
    "VARIABLE",
    "WHILE",
    "WORD",
    "XOR",
    "[",
    "[']",
    "[CHAR]",
    "]",
]

CORE_EXT_WORDS = [
    ".(",
    ".R",
    "0<>",
    "0>",
    "2>R",
    "2R>",
    "2R@",
    ":NONAME",
    "<>",
    "?DO",
    "ACTION-OF",
    "AGAIN",
    "BUFFER:",
    "C\"",
    "CASE",
    "COMPILE,",
    "DEFER",
    "DEFER!",
    "DEFER@",
    "ENDCASE",
    "ENDOF",
    "ERASE",
    "FALSE",
    "HEX",
    "HOLDS",
    "IS",
    "MARKER",
    "NIP",
    "OF",
    "PAD",
    "PARSE",
    "PARSE-NAME",
    "PICK",
    "REFILL",
    "RESTORE-INPUT",
    "ROLL",
    "S\\\"",
    "SAVE-INPUT",
    "SOURCE-ID",
    "TO",
    "TRUE",
    "TUCK",
    "U.R",
    "U>",
    "UNUSED",
    "VALUE",
    "WITHIN",
    "[COMPILE]",
    "\\",
]


@dataclass(frozen=True)
class Symbol:
    address: str
    name: str


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Generate a Forth 2012 core/core-ext coverage report from a .sym file."
    )
    parser.add_argument("sym_file", type=Path, help="Input symbol file")
    parser.add_argument("output_file", type=Path, help="Output markdown file")
    parser.add_argument(
        "--label",
        help="Optional display label for the build; defaults to the .sym file name",
    )
    return parser.parse_args()


def load_symbols(path: Path) -> dict[str, list[Symbol]]:
    symbols: dict[str, list[Symbol]] = {}
    for raw_line in path.read_text(encoding="utf-8").splitlines():
        line = raw_line.strip()
        if not line or ":" not in line:
            continue
        address, name = line.split(":", 1)
        symbol = Symbol(address=address, name=name)
        symbols.setdefault(name.casefold(), []).append(symbol)
    return symbols


def render_section(
    title: str, words: list[str], symbols: dict[str, list[Symbol]]
) -> tuple[str, int, int]:
    covered = []
    missing = []
    for word in words:
        matches = symbols.get(word.casefold(), [])
        if matches:
            covered.append((word, matches[0]))
        else:
            missing.append(word)

    lines = [
        f"## {title}",
        "",
        f"Coverage: **{len(covered)}/{len(words)}**",
        "",
    ]

    lines.extend(["### Included", ""])
    if covered:
        lines.append(" ".join(f"`{word}`" for word, _symbol in covered))
    else:
        lines.append("None")
    lines.append("")

    lines.extend(["### Missing", ""])
    if missing:
        lines.append(" ".join(f"`{word}`" for word in missing))
    else:
        lines.append("None")
    lines.append("")

    return "\n".join(lines), len(covered), len(words)


def main() -> int:
    args = parse_args()
    symbols = load_symbols(args.sym_file)
    label = args.label or args.sym_file.name

    core_section, core_covered, core_total = render_section(
        "Core Word Set", CORE_WORDS, symbols
    )
    core_ext_section, core_ext_covered, core_ext_total = render_section(
        "Core Extension Word Set", CORE_EXT_WORDS, symbols
    )

    report = "\n".join(
        [
            "# Forth 2012 Wordset Coverage",
            "",
            f"Build: **{label}**  ",
            f"Symbol file: `{args.sym_file}`  ",
            f"Generated: `{datetime.now().isoformat(timespec='seconds')}`",
            "",
            "Matching is case-insensitive and otherwise exact.",
            "",
            "## Summary",
            "",
            f"- Core: **{core_covered}/{core_total}**",
            f"- Core Ext: **{core_ext_covered}/{core_ext_total}**",
            "",
            core_section,
            core_ext_section,
        ]
    )

    args.output_file.parent.mkdir(parents=True, exist_ok=True)
    args.output_file.write_text(report, encoding="utf-8")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
