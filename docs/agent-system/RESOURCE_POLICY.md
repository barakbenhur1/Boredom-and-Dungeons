# Resource and Quality Policy

## Objective

Minimize total model calls, repeated context, command runtime, and output tokens without reducing correctness.

## Default budgets

- Simple task: zero subagents.
- Medium task: one specialist.
- Complex task: two specialists initially; three total only when justified.
- Agent nesting: none beyond root-to-specialist.
- Concurrent threads: three including the root.
- Initial code reading: project map plus about 5–10 relevant files.
- Expensive full test/build: once at the integration stage unless a failure demands another run.

Budgets are defaults, not excuses to stop before correctness.

## Context compression

The root converts conversation history into immutable `REQ-*` statements and sends only the relevant subset to each specialist. Include paths, symbols, known facts, prior command evidence, and non-goals. Do not forward the entire transcript.

## Search ladder

1. Known file/symbol.
2. Exact `rg` search.
3. Scoped `rg --files` and directory inspection.
4. Architecture/status index.
5. Broader search only after the first four fail.

Exclude generated/build/vendor/cache directories.

## Validation ladder

1. Parse/lint/static check.
2. Focused unit or editor test.
3. Affected integration/build check.
4. Full project/release gate.

The root owns the final expensive gate. Specialists report targeted evidence.

## Parallelism

Parallelize only independent read-only analysis or changes in isolated worktrees. Parallelism that duplicates repository discovery or touches overlapping files is prohibited.

## Model policy

- Root, architecture, difficult implementation, performance, and release-critical QA: `gpt-5.5`.
- Narrow design review, routing evidence, content review, and lightweight advisory work: `gpt-5.4-mini`.
- Raise reasoning effort only after concrete complexity or failure; do not start every task at high effort.

## Stop conditions

Stop exploring when the execution path and acceptance evidence are sufficient. Stop delegating when one owner can finish. Stop retrying when the command and inputs have not changed. Do not stop implementation until every requirement is verified or explicitly blocked with evidence.
