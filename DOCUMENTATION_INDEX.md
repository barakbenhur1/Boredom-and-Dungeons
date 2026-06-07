# Documentation Index — Maintained Project Map

This index lists the documents that must remain synchronized. It is a navigation and ownership map, not a second project-status source.

## Mandatory repository documents

| File | Purpose | Update when |
|---|---|---|
| `README.md` | Public orientation and stable repository entry points | Engine/version, repository entry points, primary commands, or stable folder discovery changes |
| `START_HERE.md` | Mandatory first read and permanent operating rules | Mandatory read order, source-of-truth policy, or request-classification policy changes |
| `DEVELOPMENT_WORKFLOW.md` | Authoritative process contract | Packaging, Git handoff, request intake, QA, documentation, or repair workflow changes |
| `PROJECT_STATUS.md` | Only authoritative requirements/status/order/QA/resume source | Every material user request, implementation, priority, blocker, verification result, or next-step change |
| `DOCUMENTATION_INDEX.md` | Maintained document map | A maintained document is added, removed with approval, renamed, or changes responsibility |
| `ARCHITECTURE.md` | Stable runtime/editor/data/scene boundaries and flow diagrams | System ownership, dependencies, scene flow, major service/component boundaries, or integration paths change |
| `QA_CHECKLIST.md` | Required verification gates | QA entry points, required automated checks, Play Mode gates, performance gates, or release criteria change |
| `TECHNICAL_DECISIONS.md` | Long-lived technical decisions and rationale | A durable architectural or workflow decision is introduced, replaced, or superseded |
| `PERFORMANCE_GUIDELINES.md` | Measurement rules and optimization constraints | Target platforms, budgets, profiling policy, loading strategy, pooling, or performance-sensitive architecture changes |

## Feature documentation

Feature-specific contracts remain under `Assets/_Project/Design/` and should be grouped by domain, such as:

- `Bosses/`
- `Combat/`
- `Horse/`
- `Map/`
- `Movement/`
- `Runtime/`
- `UI/`

A feature document describes durable behavior and acceptance rules. Its current implementation/verification status remains in `PROJECT_STATUS.md`.

## Required reading by task type

### Gameplay or feature implementation

Read:

1. `START_HERE.md`
2. `DEVELOPMENT_WORKFLOW.md`
3. `PROJECT_STATUS.md`
4. `ARCHITECTURE.md`
5. the relevant `Assets/_Project/Design/**` files
6. `QA_CHECKLIST.md`

### Regression or broken package

Read:

1. `START_HERE.md`
2. `DEVELOPMENT_WORKFLOW.md`, especially failure/repair handling
3. the exact terminal/Unity error
4. `PROJECT_STATUS.md`
5. the affected source, scene, installer, and QA files

### Architecture or performance change

Read:

1. `ARCHITECTURE.md`
2. `TECHNICAL_DECISIONS.md`
3. `PERFORMANCE_GUIDELINES.md`
4. `PROJECT_STATUS.md`
5. affected design and QA files

## Documentation synchronization matrix

```mermaid
flowchart LR
    R[New or changed requirement] --> S[PROJECT_STATUS]
    R --> D[Relevant design spec]
    I[Implementation change] --> A[ARCHITECTURE if boundaries change]
    I --> T[TECHNICAL_DECISIONS if a durable choice changes]
    I --> Q[QA_CHECKLIST and automated QA]
    I --> P[PERFORMANCE_GUIDELINES if budgets or measurement change]
    N[New maintained document] --> X[DOCUMENTATION_INDEX]
    W[Workflow change] --> F[DEVELOPMENT_WORKFLOW]
    F --> H[START_HERE if mandatory behavior changes]
    H --> M[README if repository discovery changes]
```

## Anti-duplication rule

Do not create files such as `WORKING_NOW.md`, `LATEST_STATUS.md`, `PROJECT_STATUS_V2.md`, or copied roadmaps as live sources. Historical states belong in Git history.
