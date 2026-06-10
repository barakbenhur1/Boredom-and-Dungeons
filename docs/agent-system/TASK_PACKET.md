# Compact Task Packet

Use this exact structure when spawning a specialist.

```text
GOAL:
One-sentence user-visible outcome.

OWNER:
agent_name

REQS:
- REQ-001: immutable requirement
- REQ-002: immutable requirement

PATHS/SYMBOLS:
- exact/path/File.cs :: SymbolName
- exact/path/Scene.unity

KNOWN FACTS:
- evidence already collected by root
- relevant existing convention
- prior command result

NON-GOALS:
- explicitly excluded work

EXPECTED OUTPUT:
- code/design/review artifact
- files the agent may edit

CHEAPEST VALIDATION:
- exact targeted command or test

RETURN FORMAT:
STATUS
REQS
FILES
EVIDENCE
RISKS
HANDOFF
```

## Requirement ledger rule

Requirements retain the same IDs through design, implementation, review, QA, documentation, and final reporting. A requirement may be split into subrequirements, but it may not disappear or be silently reworded.
