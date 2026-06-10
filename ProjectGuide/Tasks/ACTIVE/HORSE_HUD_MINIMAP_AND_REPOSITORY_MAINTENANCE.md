# Horse, Contextual HUD, Minimap and Repository Maintenance

```text
Status: IMPLEMENTED BY LOCAL PATCH / UNITY VERIFICATION REQUIRED
Delivery: local patch only; no Git/GitHub write operation
```

## Implemented scope

- exact 8% horse horizontal slowdown per full missing 30% health band;
- slower horse healing and explicit ring/mote animation;
- removal of above-horse icons/cards;
- horse health visible only while on foot beside the horse and hidden while mounted;
- player health visible while stationary and briefly after damage/healing;
- ammo visible from ranged press through release, including long hold;
- professional fades and contextual UI policy;
- green horse dot, fog-safe red regular/guard dots, larger mini-boss dots and large boss hexagons;
- minimap safe-idle dim instead of full disappearance;
- safe local repository measurement/cleanup and detailed before/after reports;
- correction of false merge-marker detection in the local package validator.

## Non-goals

- no shop/NPC markers until the authoritative entities exist;
- no balance changes outside horse healing rate and approved injury speed bands;
- no lossy asset optimization, history rewrite or LFS migration;
- no work-queue reordering.

## Verification gate

1. clean Unity compilation;
2. `Boredom And Dungeons → TEST EVERYTHING`;
3. focused threshold, healing, HUD, minimap and input tests;
4. supported resolution checks;
5. repository report review;
6. clean-clone/build verification locally where available;
7. user visual acceptance;
8. local commit by the user only.

## Exact resume point

After all verification passes, record real evidence in Current, Verification and QA History, create the local commit, then return to the existing `WORK_QUEUE.md` order without inserting or reordering unrelated work.
