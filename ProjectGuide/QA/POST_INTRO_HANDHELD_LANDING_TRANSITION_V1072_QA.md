# Post-Intro Landing Transition and Clean Installer Exit V10.7.2 QA

## Corrected implementation

V10.7.2 retains the verified post-BBH landing targets from V10.7.1 and corrects two delivery defects:

1. Runtime token validation scans Runtime sources only. The editor validator is checked separately and cannot trigger its own forbidden-token rules.
2. Installer cleanup is registered before checksum/preflight and runs on every exit path.

## Required transition behavior

- The cinematic begins immediately after BBH.
- Its first landing page may be the first-launch tutorial choice or the normal Main Menu.
- PLAY TUTORIAL does not cancel the already-running/consumed landing transition.
- The transition never runs after tutorial completion, skip completion, gameplay return or internal menu navigation.

## Required installer behavior

- PASS, failure, warning, information and cleanup use distinct terminal colors when supported.
- `NO_COLOR=1` keeps readable textual prefixes without ANSI sequences.
- Success removes package ZIPs and extracted installer files.
- Preflight failure removes package ZIPs and extracted installer files.
- Post-write failure restores the repository, verifies restoration, removes the failed-attempt backup and removes package residue.
- Unknown local source hashes remain blocked before writes.
