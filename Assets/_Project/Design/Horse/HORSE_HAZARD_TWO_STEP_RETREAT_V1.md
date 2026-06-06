# Horse Hazard Two-Step Retreat V1

**Status: APPROVED — PENDING IMPLEMENTATION**

Status date: **2026-06-06**

## Requirement

When the horse detects lava, a hole, a chasm, or missing ground ahead, it must no longer stop in place for one second.

It must:

1. detect the unsafe direction before contact;
2. choose a safe direction away from it;
3. move backward for roughly two short horse steps;
4. finish the retreat quickly;
5. return to the behavior that was active before the retreat;
6. repeat the retreat when unsafe input continues;
7. avoid backing into another unsafe area or obstacle.

The motion must look like a short backward movement rather than a teleport or a long flee sequence. Mounted rider placement must remain correct.

Use the existing horse hazard-safety and movement flow. Do not add a second hazard detector.

## QA

Add the checks to the existing `Boredom And Dungeons -> TEST EVERYTHING` command. No additional required QA command is allowed.

Play Mode must cover mounted movement, return-to-player movement, safe-spot movement, repeated unsafe input, lava, and holes/chasms.
