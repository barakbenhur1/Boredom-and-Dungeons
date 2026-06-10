<!-- BND_TERMINAL_OUTPUT_V1081_REGRESSION_GATE:BEGIN -->
## Regression verification

Any package revised after 2026-06-10 must include automated checks proving: ANSI semantic colors appear on a pseudo-terminal; `NO_COLOR=1`, `TERM=dumb` and redirected output contain no escape sequences; all paths retain explicit `PASS`, `BLOCKED`, `WARNING`, `INFO` or `CLEANED` prefixes. A package that prints only unformatted output on an interactive terminal does not satisfy this standard.
<!-- BND_TERMINAL_OUTPUT_V1081_REGRESSION_GATE:END -->

# Terminal Output Standard

## Purpose

Repository installers, repair tools, validators and maintenance scripts must make their result immediately readable without forcing the operator to search through unformatted terminal text.

## Required semantic palette

When standard ANSI color output is available:

- `PASS` / success: bold green.
- `BLOCKED` / `ERROR` / failure: bold red.
- `WARNING`: bold yellow.
- `INFO` / next action / neutral status: cyan or blue.
- `CLEANED` / cleanup completion: magenta.

Color is supplementary, never the only signal. Every message must retain an explicit textual prefix so logs remain understandable when copied, redirected or viewed without color.

## Compatibility

- Enable color only for an interactive terminal.
- Respect the `NO_COLOR` environment variable.
- Disable ANSI sequences when `TERM=dumb` or output is redirected.
- Do not use platform-specific color utilities when portable ANSI escape sequences are sufficient.

## Mandatory cleanup on every exit path

Every ZIP installer must register cleanup before checksum verification or preflight begins.

Cleanup runs after success, checksum failure, preflight blocking, post-write validation failure, interruption handled by the shell, or any other non-zero exit.

The cleanup removes:
- the exact source ZIP from `~/Downloads`;
- superseded installer ZIPs explicitly owned by the package;
- `.package_payload`;
- `.package_tools`;
- extracted patch manifests, checksum files, verification notes and command wrappers.

A failed package must not leave installer residue merely so it can be rerun. The corrected package can be downloaded again.

A backup created for a successful repository write is an intentional rollback artifact and may remain. If post-write validation fails, the installer restores the repository, verifies the restoration, and removes the now-unnecessary failed-attempt backup. If restoration cannot be verified, the backup is retained and reported in red.

## Failure behavior

- Print the blocking reason immediately in red.
- List every individual problem on its own red-prefixed line.
- State explicitly that no repository content was overwritten when preflight stops the operation.
- Never print a green success summary after a failed command.
- State that cleanup is being performed, not that the source ZIP is being preserved.

## Success behavior

- Print successful checksum verification in green.
- Print changed-file and backup information as cyan `INFO` entries.
- Print cleanup actions in magenta.
- End with one green completion line and one cyan next-action line before the final cleanup handler runs.

## Scope

This standard is mandatory for all future ZIP installers and repository command-line tools. Any installer revised for another reason must adopt this cleanup contract at the same time.
