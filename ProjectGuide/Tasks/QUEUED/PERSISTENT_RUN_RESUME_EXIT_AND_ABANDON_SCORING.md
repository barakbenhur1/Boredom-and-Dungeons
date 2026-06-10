# Queued Task — Persistent Run Resume, Exit and Abandon Scoring

```text
Status: REQUIRED / QUEUED AFTER TUTORIAL VERIFICATION
Blocked by: first-launch tutorial full acceptance
Then blocks: final professional handheld-to-gameplay transition integration
```

## Approved requirement

Implement a non-destructive `EXIT TO MAIN MENU / SAVE & RETURN`, conditional `CONTINUE`, protected `START NEW GAME` overwrite confirmation and a distinct destructive `ABANDON RUN`.

Abandon awards 84% of the points that the existing death evaluator would award at that exact moment. The shared result screen appears first. The agreed exit animation begins only after that screen is closed.

## Required phases

1. Audit the existing run, Seed, scene reload, safe-position, player, horse, encounter, result and persistence owners.
2. Record which earlier Resume-only-for-crash rule is superseded.
3. Implement one versioned atomic snapshot owner.
4. Implement normal Save & Return.
5. Implement conditional Continue and protected New Game overwrite.
6. Implement idempotent Abandon scoring and shared result routing.
7. Integrate New Game, Continue, Save & Return and Abandon as four distinct cinematic transition intents.
8. Run automated, persistence, Play Mode, failure-recovery and user-acceptance gates.

## Exact resume point

Do not begin this task merely because the code is documented. Begin only after the first-launch tutorial package compiles, TEST EVERYTHING is clean, all input-route runs pass, timing is 5–8 minutes and the user accepts the result.
