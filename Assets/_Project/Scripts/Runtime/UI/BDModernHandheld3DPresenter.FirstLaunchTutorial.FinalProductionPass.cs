using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private enum TutorialBossPhysicalAttackKind
        {
            Slash,
            JumpSlam
        }

        private readonly Dictionary<TutorialEnemyActor, float>
            firstLaunchTutorialActorHitUntil =
                new Dictionary<TutorialEnemyActor, float>();
        private readonly Dictionary<TutorialEnemyActor, Color>
            firstLaunchTutorialActorRestColors =
                new Dictionary<TutorialEnemyActor, Color>();
        private readonly Dictionary<TutorialEnemyActor, float>
            firstLaunchTutorialActorDefeatHideAt =
                new Dictionary<TutorialEnemyActor, float>();

        private BDTutorialFinalProductionDriver
            firstLaunchTutorialFinalProductionDriver;
        private float firstLaunchTutorialPlayerHitUntil;
        private TutorialBossPhysicalAttackKind
            firstLaunchTutorialBossPhysicalAttackKind;
        private Vector2 firstLaunchTutorialBossPhysicalTarget;

        private readonly Image[] firstLaunchTutorialBossFanProjectiles =
            new Image[3];
        private readonly Vector2[] firstLaunchTutorialBossFanStarts =
            new Vector2[3];
        private readonly Vector2[] firstLaunchTutorialBossFanTargets =
            new Vector2[3];
        private float firstLaunchTutorialBossFanStartedAt;
        private const float TutorialBossFanDuration = 0.78f;
        private bool firstLaunchTutorialBossFanActive;
        private bool firstLaunchTutorialBossFanDamageResolved;

        private FirstLaunchTutorialStep firstLaunchTutorialFinalObservedStep;
        private float firstLaunchTutorialLessonStationX;
        private bool firstLaunchTutorialTravelGateActive;
        private float firstLaunchTutorialTravelGateFeedbackAt;

        private Vector2 firstLaunchTutorialFinishGateClosedPosition;
        private float firstLaunchTutorialFinishGateOpenedAt = -1f;

        private bool firstLaunchTutorialRelicCompletionActive;
        private bool firstLaunchTutorialRelicCompletionMenuRevealed;
        private float firstLaunchTutorialRelicCompletionStartedAt;
        private Vector2 firstLaunchTutorialRelicCompletionPlayerPosition;
        private GameObject firstLaunchTutorialRelicLightRoot;
        private CanvasGroup firstLaunchTutorialRelicLightCanvasGroup;
        private Image firstLaunchTutorialRelicLightCore;
        private Image firstLaunchTutorialRelicLightRing;
        private readonly List<Image> firstLaunchTutorialRelicRays =
            new List<Image>(10);
        private Texture2D firstLaunchTutorialRelicLightTexture;
        private Sprite firstLaunchTutorialRelicLightSprite;

        private float firstLaunchTutorialFinalActionStartedAt = -1f;
        private bool firstLaunchTutorialFinalAttackWasAirborne;
        private int firstLaunchTutorialLastPointerPhysicalPulseFrame = -1;

        private void InitializeFirstLaunchTutorialFinalProductionPass()
        {
            firstLaunchTutorialActorHitUntil.Clear();
            firstLaunchTutorialActorRestColors.Clear();
            firstLaunchTutorialActorDefeatHideAt.Clear();
            firstLaunchTutorialPlayerHitUntil = 0f;
            firstLaunchTutorialBossPhysicalAttackKind =
                TutorialBossPhysicalAttackKind.Slash;
            firstLaunchTutorialBossPhysicalTarget = Vector2.zero;
            firstLaunchTutorialBossFanStartedAt = 0f;
            firstLaunchTutorialBossFanActive = false;
            firstLaunchTutorialBossFanDamageResolved = false;
            firstLaunchTutorialFinalObservedStep = firstLaunchTutorialStep;
            firstLaunchTutorialLessonStationX =
                firstLaunchTutorialPlayerWorldPosition.x;
            firstLaunchTutorialTravelGateActive = false;
            firstLaunchTutorialTravelGateFeedbackAt = 0f;
            firstLaunchTutorialFinishGateOpenedAt = -1f;
            firstLaunchTutorialRelicCompletionActive = false;
            firstLaunchTutorialRelicCompletionMenuRevealed = false;
            firstLaunchTutorialRelicCompletionStartedAt = 0f;
            firstLaunchTutorialFinalActionStartedAt = -1f;
            firstLaunchTutorialFinalAttackWasAirborne = false;

            if (firstLaunchTutorialFinishGate != null)
            {
                firstLaunchTutorialFinishGateClosedPosition =
                    firstLaunchTutorialFinishGate.rectTransform
                        .anchoredPosition;
            }

            BuildFirstLaunchTutorialBossFanProjectiles();
            BuildFirstLaunchTutorialRelicCompletionOverlay();

            if (firstLaunchTutorialFinalProductionDriver == null)
            {
                firstLaunchTutorialFinalProductionDriver =
                    gameObject.GetComponent<
                        BDTutorialFinalProductionDriver>();
                if (firstLaunchTutorialFinalProductionDriver == null)
                {
                    firstLaunchTutorialFinalProductionDriver =
                        gameObject.AddComponent<
                            BDTutorialFinalProductionDriver>();
                }
            }
            firstLaunchTutorialFinalProductionDriver.Configure(this);
        }

        private void DisposeFirstLaunchTutorialFinalProductionPass()
        {
            firstLaunchTutorialActorHitUntil.Clear();
            firstLaunchTutorialActorRestColors.Clear();
            firstLaunchTutorialActorDefeatHideAt.Clear();
            firstLaunchTutorialBossFanActive = false;
            for (int index = 0;
                 index < firstLaunchTutorialBossFanProjectiles.Length;
                 index++)
            {
                firstLaunchTutorialBossFanProjectiles[index] = null;
            }
            firstLaunchTutorialTravelGateActive = false;
            firstLaunchTutorialRelicCompletionActive = false;
            if (firstLaunchTutorialRelicLightRoot != null)
                Destroy(firstLaunchTutorialRelicLightRoot);
            firstLaunchTutorialRelicLightRoot = null;
            firstLaunchTutorialRelicLightCanvasGroup = null;
            firstLaunchTutorialRelicLightCore = null;
            firstLaunchTutorialRelicLightRing = null;
            firstLaunchTutorialRelicRays.Clear();
            if (firstLaunchTutorialRelicLightSprite != null)
                Destroy(firstLaunchTutorialRelicLightSprite);
            if (firstLaunchTutorialRelicLightTexture != null)
                Destroy(firstLaunchTutorialRelicLightTexture);
            firstLaunchTutorialRelicLightSprite = null;
            firstLaunchTutorialRelicLightTexture = null;
            if (firstLaunchTutorialFinalProductionDriver != null)
                firstLaunchTutorialFinalProductionDriver.Configure(null);
        }

        internal void TickFirstLaunchTutorialFinalProductionPass()
        {
            UpdateFirstLaunchTutorialMirroredPointerControl();
            UpdateFirstLaunchTutorialRelicCompletion();
            if (!firstLaunchTutorialActive ||
                displayedPage != EffectivePage.FirstLaunchTutorial)
            {
                return;
            }

            UpdateFirstLaunchTutorialTravelStation();
            EnsureFirstLaunchTutorialPersistentCourseGeometryVisible();
            RenderFirstLaunchTutorialFinalProductionPresentation();
            OverrideFirstLaunchTutorialAttackPresentation();
        }

        private void BuildFirstLaunchTutorialBossFanProjectiles()
        {
            if (firstLaunchTutorialCourseRoot == null)
                return;
            for (int index = 0; index < 3; index++)
            {
                Image projectile = CreateTutorialEntity(
                    firstLaunchTutorialCourseRoot,
                    "Tutorial Boss Fan Projectile " + index,
                    0f,
                    0f,
                    28f,
                    18f,
                    index == 1
                        ? new Color(0.56f, 0.94f, 1f, 1f)
                        : new Color(0.72f, 0.42f, 1f, 0.96f)
                );
                projectile.raycastTarget = false;
                AddOutline(
                    projectile.gameObject,
                    new Color(0.04f, 0.02f, 0.12f, 0.92f),
                    2f
                );
                projectile.gameObject.SetActive(false);
                firstLaunchTutorialBossFanProjectiles[index] = projectile;
            }
        }

        private void LaunchFirstLaunchTutorialBossFan(
            TutorialEnemyActor boss)
        {
            if (boss == null)
                return;

            Vector2 origin = boss.Position + new Vector2(-46f, 30f);
            Vector2 target = firstLaunchTutorialPlayerWorldPosition +
                new Vector2(0f, 18f);
            float[] offsets = { -88f, 0f, 88f };
            for (int index = 0; index < 3; index++)
            {
                firstLaunchTutorialBossFanStarts[index] = origin;
                firstLaunchTutorialBossFanTargets[index] =
                    target + new Vector2(0f, offsets[index]);
                Image projectile = firstLaunchTutorialBossFanProjectiles[index];
                if (projectile == null)
                    continue;
                projectile.gameObject.SetActive(true);
                projectile.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(origin);
            }
            firstLaunchTutorialBossFanStartedAt = Time.unscaledTime;
            firstLaunchTutorialBossFanActive = true;
            firstLaunchTutorialBossFanDamageResolved = false;
        }

        private void UpdateFirstLaunchTutorialBossFanProjectiles()
        {
            if (!firstLaunchTutorialBossFanActive)
                return;

            float progress = Mathf.Clamp01(
                (Time.unscaledTime - firstLaunchTutorialBossFanStartedAt) /
                TutorialBossFanDuration
            );
            float eased = progress * progress * (3f - 2f * progress);
            for (int index = 0; index < 3; index++)
            {
                Image projectile = firstLaunchTutorialBossFanProjectiles[index];
                if (projectile == null)
                    continue;
                Vector2 position = Vector2.Lerp(
                    firstLaunchTutorialBossFanStarts[index],
                    firstLaunchTutorialBossFanTargets[index],
                    eased
                );
                projectile.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(position);
                projectile.rectTransform.localScale = new Vector3(
                    1f + Mathf.Sin(progress * Mathf.PI) * 0.28f,
                    1f,
                    1f
                );
            }

            if (progress < 1f)
                return;

            if (!firstLaunchTutorialBossFanDamageResolved)
            {
                firstLaunchTutorialBossFanDamageResolved = true;
                Vector2 player = firstLaunchTutorialPlayerWorldPosition +
                    new Vector2(0f, 18f);
                bool hit = false;
                for (int index = 0; index < 3; index++)
                {
                    if (Vector2.Distance(
                            player,
                            firstLaunchTutorialBossFanTargets[index]) <= 66f)
                    {
                        hit = true;
                        break;
                    }
                }
                if (hit &&
                    Time.unscaledTime >= firstLaunchTutorialInvulnerableUntil)
                {
                    DamageFirstLaunchTutorialPlayer(1);
                }
            }

            firstLaunchTutorialBossFanActive = false;
            for (int index = 0; index < 3; index++)
            {
                if (firstLaunchTutorialBossFanProjectiles[index] != null)
                {
                    firstLaunchTutorialBossFanProjectiles[index]
                        .gameObject.SetActive(false);
                }
            }
        }

        private void ConfigureFirstLaunchTutorialBossPhysicalAttack(
            TutorialEnemyActor boss,
            float distance)
        {
            if (boss == null || distance > 280f)
                return;
            firstLaunchTutorialBossPhysicalAttackKind =
                boss.AttackSequence % 2 == 0
                    ? TutorialBossPhysicalAttackKind.Slash
                    : TutorialBossPhysicalAttackKind.JumpSlam;
            firstLaunchTutorialBossPhysicalTarget =
                firstLaunchTutorialPlayerWorldPosition;
        }

        private float ResolveFirstLaunchTutorialBossPhysicalRange()
        {
            return firstLaunchTutorialBossPhysicalAttackKind ==
                    TutorialBossPhysicalAttackKind.JumpSlam
                ? 214f
                : 184f;
        }

        private bool IsFirstLaunchTutorialBossPhysicalHit(
            TutorialEnemyActor boss)
        {
            if (boss == null)
                return false;
            if (firstLaunchTutorialBossPhysicalAttackKind ==
                TutorialBossPhysicalAttackKind.JumpSlam)
            {
                return Vector2.Distance(
                    firstLaunchTutorialPlayerWorldPosition,
                    firstLaunchTutorialBossPhysicalTarget
                ) <= ResolveFirstLaunchTutorialBossPhysicalRange();
            }
            return Vector2.Distance(
                firstLaunchTutorialPlayerWorldPosition,
                boss.Position
            ) <= ResolveFirstLaunchTutorialBossPhysicalRange();
        }

        private string ResolveFirstLaunchTutorialBossPhysicalWindupLabel()
        {
            return firstLaunchTutorialBossPhysicalAttackKind ==
                    TutorialBossPhysicalAttackKind.JumpSlam
                ? "JUMP SLAM — LEAVE THE MARKED RADIUS"
                : "SLASH WINDUP — EXIT THE MARKED ARC";
        }

        private string ResolveFirstLaunchTutorialBossPhysicalImpactLabel()
        {
            return firstLaunchTutorialBossPhysicalAttackKind ==
                    TutorialBossPhysicalAttackKind.JumpSlam
                ? "SLAM IMPACT — RADIUS RESOLVED"
                : "SLASH IMPACT — ARC RESOLVED";
        }

        private void BeginFirstLaunchTutorialActorHitReaction(
            TutorialEnemyActor actor,
            bool defeated)
        {
            if (actor == null || actor.Image == null)
                return;
            if (!firstLaunchTutorialActorRestColors.ContainsKey(actor))
                firstLaunchTutorialActorRestColors[actor] = actor.Image.color;
            firstLaunchTutorialActorHitUntil[actor] =
                Time.unscaledTime + 0.20f;
            if (defeated)
            {
                firstLaunchTutorialActorDefeatHideAt[actor] =
                    Time.unscaledTime + 0.24f;
                actor.Image.gameObject.SetActive(true);
            }
        }

        private void BeginFirstLaunchTutorialPlayerHitReaction()
        {
            firstLaunchTutorialPlayerHitUntil = Time.unscaledTime + 0.20f;
        }

        private void RenderFirstLaunchTutorialFinalProductionPresentation()
        {
            float now = Time.unscaledTime;
            if (firstLaunchTutorialPlayer != null)
            {
                ApplyFirstLaunchTutorialPlayerVisualPolish();
                Vector2 basePosition =
                    firstLaunchTutorialPlayer.rectTransform.anchoredPosition;
                if (now < firstLaunchTutorialPlayerHitUntil)
                {
                    float flicker = Mathf.Floor(now * 42f) % 2f == 0f
                        ? -7f
                        : 7f;
                    firstLaunchTutorialPlayer.rectTransform.anchoredPosition =
                        basePosition + new Vector2(flicker, 0f);
                    firstLaunchTutorialPlayer.color =
                        Mathf.Floor(now * 34f) % 2f == 0f
                            ? new Color(1f, 0.42f, 0.42f, 1f)
                            : Color.white;
                }
                else
                {
                    firstLaunchTutorialPlayer.color = Color.white;
                }
            }

            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (actor == null || actor.Image == null)
                    continue;
                float hitUntil;
                if (firstLaunchTutorialActorHitUntil.TryGetValue(
                        actor,
                        out hitUntil) &&
                    now < hitUntil)
                {
                    float jitter = Mathf.Floor(now * 44f) % 2f == 0f
                        ? -8f
                        : 8f;
                    actor.Image.rectTransform.anchoredPosition =
                        SnapFirstLaunchTutorialWorldPosition(actor.Position) +
                        new Vector2(jitter, 0f);
                    actor.Image.color =
                        Mathf.Floor(now * 36f) % 2f == 0f
                            ? Color.white
                            : new Color(1f, 0.22f, 0.20f, 1f);
                }
                else
                {
                    Color rest;
                    if (firstLaunchTutorialActorRestColors.TryGetValue(
                            actor,
                            out rest))
                    {
                        actor.Image.color = rest;
                    }
                }

                float hideAt;
                if (actor.Dead &&
                    firstLaunchTutorialActorDefeatHideAt.TryGetValue(
                        actor,
                        out hideAt) &&
                    now >= hideAt)
                {
                    actor.Image.gameObject.SetActive(false);
                }
            }

            RenderFirstLaunchTutorialBossPhysicalTelegraph();
            RenderFirstLaunchTutorialFinishGateMotion();
        }

        private void RenderFirstLaunchTutorialBossPhysicalTelegraph()
        {
            TutorialEnemyActor boss = FindTutorialBoss();
            if (boss == null || boss.Image == null ||
                !boss.AttackCommitted || boss.UsesProjectileAttack)
            {
                return;
            }
            float elapsed = Time.unscaledTime - boss.ActionStartedAt;
            if (elapsed >= 0.62f)
                return;
            float t = Mathf.Clamp01(elapsed / 0.62f);
            float pulse = 0.5f + 0.5f * Mathf.Sin(elapsed * 28f);
            Vector2 direction = firstLaunchTutorialPlayerWorldPosition -
                boss.Position;
            if (direction.sqrMagnitude < 0.001f)
                direction = Vector2.left;
            direction.Normalize();

            if (firstLaunchTutorialBossAttackTelegraph != null)
            {
                RectTransform telegraph =
                    firstLaunchTutorialBossAttackTelegraph.rectTransform;
                telegraph.gameObject.SetActive(true);
                if (firstLaunchTutorialBossPhysicalAttackKind ==
                    TutorialBossPhysicalAttackKind.JumpSlam)
                {
                    telegraph.anchoredPosition =
                        SnapFirstLaunchTutorialWorldPosition(
                            firstLaunchTutorialBossPhysicalTarget +
                            new Vector2(0f, -42f)
                        );
                    float diameter =
                        ResolveFirstLaunchTutorialBossPhysicalRange() * 2f;
                    telegraph.sizeDelta = new Vector2(
                        diameter,
                        18f + pulse * 12f
                    );
                    telegraph.localRotation = Quaternion.identity;
                    boss.Image.rectTransform.anchoredPosition =
                        SnapFirstLaunchTutorialWorldPosition(
                            boss.Position +
                            new Vector2(
                                direction.x * 36f * t,
                                Mathf.Sin(t * Mathf.PI) * 96f
                            )
                        );
                    boss.Image.rectTransform.localScale = new Vector3(
                        1f + t * 0.18f,
                        1f - t * 0.12f,
                        1f
                    );
                }
                else
                {
                    float range = ResolveFirstLaunchTutorialBossPhysicalRange();
                    telegraph.anchoredPosition =
                        SnapFirstLaunchTutorialWorldPosition(
                            boss.Position + direction * range * 0.5f +
                            new Vector2(0f, 10f)
                        );
                    telegraph.sizeDelta = new Vector2(
                        range,
                        22f + pulse * 14f
                    );
                    telegraph.localRotation = Quaternion.Euler(
                        0f,
                        0f,
                        direction.x >= 0f ? 0f : 180f
                    );
                    boss.Image.rectTransform.localRotation =
                        Quaternion.Euler(
                            0f,
                            0f,
                            -direction.x * Mathf.Lerp(8f, 34f, t)
                        );
                    boss.Image.rectTransform.localScale = new Vector3(
                        1f + t * 0.22f,
                        1f - t * 0.08f,
                        1f
                    );
                }
                firstLaunchTutorialBossAttackTelegraph.color =
                    Color.Lerp(
                        new Color(1f, 0.72f, 0.08f, 0.66f),
                        new Color(1f, 0.08f, 0.04f, 0.98f),
                        Mathf.Clamp01(t * 0.76f + pulse * 0.24f)
                    );
            }
        }

        private void EnsureFirstLaunchTutorialPersistentCourseGeometryVisible()
        {
            // BD CONTEXTUAL COURSE GEOMETRY V10.11.30.28
            // Lesson geometry is prepared before its room scrolls into view, but
            // it must never exist as collision/visual clutter in another room.
            bool wallJumpRoom =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.WallJump;
            if (firstLaunchTutorialWallJumpWall != null)
                firstLaunchTutorialWallJumpWall.gameObject.SetActive(wallJumpRoom);
            if (firstLaunchTutorialWallJumpPlatform != null)
                firstLaunchTutorialWallJumpPlatform.gameObject.SetActive(wallJumpRoom);
            if (firstLaunchTutorialWallJumpUpperGround != null)
                firstLaunchTutorialWallJumpUpperGround.gameObject.SetActive(wallJumpRoom);

            bool finishGateRoom =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossIntro ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseOne ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseTwo ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossDefeated ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.Collectible;
            if (firstLaunchTutorialFinishGate != null)
            {
                bool gateOpeningAnimationVisible =
                    firstLaunchTutorialFinishGateOpen &&
                    (firstLaunchTutorialFinishGateOpenedAt < 0f ||
                     Time.unscaledTime -
                         firstLaunchTutorialFinishGateOpenedAt < 0.64f);
                firstLaunchTutorialFinishGate.gameObject.SetActive(
                    finishGateRoom &&
                    (!firstLaunchTutorialFinishGateOpen ||
                     gateOpeningAnimationVisible)
                );
            }
        }

        private void RenderFirstLaunchTutorialFinishGateMotion()
        {
            if (firstLaunchTutorialFinishGate == null)
                return;
            if (firstLaunchTutorialFinishGateOpen &&
                firstLaunchTutorialFinishGateOpenedAt < 0f)
            {
                firstLaunchTutorialFinishGateOpenedAt = Time.unscaledTime;
            }
            float open = firstLaunchTutorialFinishGateOpenedAt < 0f
                ? 0f
                : Mathf.Clamp01(
                    (Time.unscaledTime -
                     firstLaunchTutorialFinishGateOpenedAt) / 0.62f
                );
            float eased = open * open * (3f - 2f * open);
            firstLaunchTutorialFinishGate.rectTransform.anchoredPosition =
                firstLaunchTutorialFinishGateClosedPosition +
                new Vector2(0f, eased * 188f);
        }

        private void UpdateFirstLaunchTutorialTravelStation()
        {
            // BD LESSON-SCREEN OWNER SUPERSEDES LEGACY TRAVEL GATE V10.11.30.19
            // The current lesson-screen system owns completion travel and the
            // black handoff. The older station gate must not hide instructions,
            // emit NEW LESSON AREA, or create a second transition authority.
            if (firstLaunchTutorialLessonScreenInitialized)
            {
                firstLaunchTutorialFinalObservedStep = firstLaunchTutorialStep;
                firstLaunchTutorialTravelGateActive = false;
                return;
            }

            if (firstLaunchTutorialStep != firstLaunchTutorialFinalObservedStep)
            {
                firstLaunchTutorialFinalObservedStep = firstLaunchTutorialStep;
                if (RequiresFirstLaunchTutorialTravelBeforeLesson(
                        firstLaunchTutorialStep))
                {
                    firstLaunchTutorialLessonStationX = Mathf.Min(
                        TutorialWorldMaxX - 220f,
                        Mathf.Max(
                            firstLaunchTutorialPlayerWorldPosition.x + 286f,
                            ResolveFirstLaunchTutorialAuthoredStationX(
                                firstLaunchTutorialStep
                            )
                        )
                    );
                    firstLaunchTutorialTravelGateActive = true;
                    firstLaunchTutorialTravelGateFeedbackAt = 0f;
                    SetFirstLaunchTutorialInstructionRequested(false);
                }
                else
                {
                    firstLaunchTutorialTravelGateActive = false;
                }
            }

            if (!firstLaunchTutorialTravelGateActive)
                return;
            if (firstLaunchTutorialPlayerWorldPosition.x >=
                firstLaunchTutorialLessonStationX - 156f)
            {
                firstLaunchTutorialTravelGateActive = false;
                SetFirstLaunchTutorialInstructionRequested(true);
                ShowFirstLaunchTutorialSuccess("NEW LESSON AREA");
            }
            else
            {
                SetFirstLaunchTutorialInstructionRequested(false);
            }
        }

        private static bool RequiresFirstLaunchTutorialTravelBeforeLesson(
            FirstLaunchTutorialStep step)
        {
            switch (step)
            {
                case FirstLaunchTutorialStep.JumpAttack:
                case FirstLaunchTutorialStep.WallJump:
                case FirstLaunchTutorialStep.HeavyAttack:
                case FirstLaunchTutorialStep.Dodge:
                case FirstLaunchTutorialStep.Parry:
                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.ChargedShot:
                case FirstLaunchTutorialStep.DismountHorse:
                case FirstLaunchTutorialStep.SpinAttack:
                case FirstLaunchTutorialStep.Grapple:
                case FirstLaunchTutorialStep.HazardKnockback:
                case FirstLaunchTutorialStep.CombinedEncounter:
                case FirstLaunchTutorialStep.MiniBossIntro:
                case FirstLaunchTutorialStep.Collectible:
                    return true;
                default:
                    return false;
            }
        }

        private static float ResolveFirstLaunchTutorialAuthoredStationX(
            FirstLaunchTutorialStep step)
        {
            switch (step)
            {
                case FirstLaunchTutorialStep.Jump:
                    return -720f;
                case FirstLaunchTutorialStep.MountHorse:
                    return -520f;
                case FirstLaunchTutorialStep.AttackEnemy:
                    return 260f;
                case FirstLaunchTutorialStep.HeavyAttack:
                    return 920f;
                case FirstLaunchTutorialStep.Dodge:
                    return 1260f;
                case FirstLaunchTutorialStep.Parry:
                    return 1640f;
                case FirstLaunchTutorialStep.JumpAttack:
                    return 1960f;
                case FirstLaunchTutorialStep.RangedAttack:
                    return 2540f;
                case FirstLaunchTutorialStep.ChargedShot:
                    return 2920f;
                case FirstLaunchTutorialStep.DismountHorse:
                    return 3320f;
                case FirstLaunchTutorialStep.SpinAttack:
                    return 3560f;
                case FirstLaunchTutorialStep.Grapple:
                    return 3920f;
                case FirstLaunchTutorialStep.WallJump:
                    return 4180f;
                case FirstLaunchTutorialStep.HazardKnockback:
                    return 4660f;
                case FirstLaunchTutorialStep.CombinedEncounter:
                    return 4920f;
                case FirstLaunchTutorialStep.MiniBossIntro:
                    return 5280f;
                case FirstLaunchTutorialStep.Collectible:
                    return 6080f;
                default:
                    return TutorialWorldMinX;
            }
        }

        private bool ShouldBlockFirstLaunchTutorialActionForTravel(
            BDModernHandheldControlTarget.ControlAction action)
        {
            if (!firstLaunchTutorialTravelGateActive)
                return false;
            bool movement = action ==
                    BDModernHandheldControlTarget.ControlAction.DPadLeft ||
                action ==
                    BDModernHandheldControlTarget.ControlAction.DPadRight;
            if (movement)
                return false;
            if (Time.unscaledTime >= firstLaunchTutorialTravelGateFeedbackAt)
            {
                firstLaunchTutorialTravelGateFeedbackAt =
                    Time.unscaledTime + 1.0f;
                ShowFirstLaunchTutorialSuccess(
                    "KEEP MOVING RIGHT — NEXT LESSON AHEAD"
                );
            }
            return true;
        }

        private float ResolveFirstLaunchTutorialLivingActorCollisionX(
            float currentX,
            float requestedX)
        {
            if (Mathf.Abs(requestedX - currentX) <= 0.001f)
                return requestedX;

            float resolvedX = requestedX;
            float direction = Mathf.Sign(requestedX - currentX);
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (actor == null || actor.Image == null ||
                    !actor.Image.gameObject.activeInHierarchy)
                {
                    continue;
                }

                bool legacyPrimaryPresentation =
                    actor.Image == firstLaunchTutorialEnemy && !actor.Active;
                if (actor.Dead && !legacyPrimaryPresentation)
                    continue;

                Vector2 actorPosition = actor.Active
                    ? actor.Position
                    : legacyPrimaryPresentation
                        ? firstLaunchTutorialEnemyWorldPosition
                        : actor.Position;
                if (Mathf.Abs(
                        actorPosition.y -
                        firstLaunchTutorialPlayerWorldPosition.y) > 72f)
                {
                    continue;
                }

                float actorWidth = Mathf.Max(32f,
                    actor.Image.rectTransform.rect.width *
                    Mathf.Abs(actor.Image.rectTransform.localScale.x));
                float clearance = 25f + actorWidth * 0.5f + 5f;
                if (direction > 0f && currentX <= actorPosition.x &&
                    resolvedX > actorPosition.x - clearance)
                {
                    resolvedX = Mathf.Min(
                        resolvedX,
                        actorPosition.x - clearance
                    );
                }
                else if (direction < 0f && currentX >= actorPosition.x &&
                         resolvedX < actorPosition.x + clearance)
                {
                    resolvedX = Mathf.Max(
                        resolvedX,
                        actorPosition.x + clearance
                    );
                }
            }
            return resolvedX;
        }

        private void PulseMirroredTutorialPhysicalControl(
            BDModernHandheldControlTarget.ControlAction action,
            FirstLaunchTutorialInputSource source)
        {
            if (source == FirstLaunchTutorialInputSource.Handheld)
                return;
            PulsePersistentControl(action);
        }

        private void UpdateFirstLaunchTutorialMirroredPointerControl()
        {
            if (!visible || hoveredTarget == null ||
                Time.frameCount == firstLaunchTutorialLastPointerPhysicalPulseFrame ||
                !ReadPointerPressed())
            {
                return;
            }
            if (hoveredTarget.Action ==
                BDModernHandheldControlTarget.ControlAction.ScreenItem)
            {
                firstLaunchTutorialLastPointerPhysicalPulseFrame =
                    Time.frameCount;
                PulsePersistentControl(
                    BDModernHandheldControlTarget.ControlAction.Confirm
                );
            }
        }

        private void OverrideFirstLaunchTutorialAttackPresentation()
        {
            if (firstLaunchTutorialActionPresentationType !=
                FirstLaunchTutorialActionPresentationType.LightAttack ||
                firstLaunchTutorialSlashEffect == null)
            {
                firstLaunchTutorialFinalActionStartedAt = -1f;
                return;
            }

            if (!Mathf.Approximately(
                    firstLaunchTutorialFinalActionStartedAt,
                    firstLaunchTutorialActionPresentationStartedAt))
            {
                firstLaunchTutorialFinalActionStartedAt =
                    firstLaunchTutorialActionPresentationStartedAt;
                firstLaunchTutorialFinalAttackWasAirborne =
                    !firstLaunchTutorialGrounded ||
                    firstLaunchTutorialStep ==
                        FirstLaunchTutorialStep.JumpAttack;
            }

            float progress = Mathf.Clamp01(
                (Time.unscaledTime -
                 firstLaunchTutorialActionPresentationStartedAt) /
                Mathf.Max(
                    0.08f,
                    firstLaunchTutorialActionPresentationDuration
                )
            );
            Vector2 direction = firstLaunchTutorialActionDirection;
            if (direction.sqrMagnitude < 0.001f)
                direction = Vector2.right;
            direction.Normalize();

            // The authoritative melee transaction resolves at 66% of the
            // action presentation. Drive the visible sword tip to the target at
            // exactly that same point so damage can never precede contact.
            float strikeProgress = SmoothestStep01(
                Mathf.Clamp01(progress / 0.66f)
            );
            if (firstLaunchTutorialFinalAttackWasAirborne)
            {
                Vector2 start = firstLaunchTutorialActionStartWorld +
                    new Vector2(direction.x * 16f, 78f);
                Vector2 end = firstLaunchTutorialActionTargetWorld +
                    new Vector2(0f, -24f);
                firstLaunchTutorialSlashEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        Vector2.Lerp(start, end, strikeProgress)
                    );
                firstLaunchTutorialSlashEffect.localRotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        Mathf.Lerp(
                            direction.x >= 0f ? 78f : 102f,
                            direction.x >= 0f ? -82f : 262f,
                            strikeProgress
                        )
                    );
                firstLaunchTutorialSlashEffect.localScale = new Vector3(
                    0.82f + strikeProgress * 0.66f,
                    1.25f,
                    1f
                );
            }
            else
            {
                firstLaunchTutorialSlashEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        Vector2.Lerp(
                            firstLaunchTutorialActionStartWorld +
                                direction * 24f,
                            firstLaunchTutorialActionTargetWorld,
                            strikeProgress
                        )
                    );
                firstLaunchTutorialSlashEffect.localRotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        direction.x >= 0f ? 0f : 180f
                    );
            }
        }

        private void BuildFirstLaunchTutorialRelicCompletionOverlay()
        {
            if (screenCanvasRoot == null ||
                firstLaunchTutorialRelicLightRoot != null)
            {
                return;
            }

            firstLaunchTutorialRelicLightTexture =
                CreateFirstLaunchTutorialRelicLightTexture(192);
            firstLaunchTutorialRelicLightSprite = Sprite.Create(
                firstLaunchTutorialRelicLightTexture,
                new Rect(
                    0f,
                    0f,
                    firstLaunchTutorialRelicLightTexture.width,
                    firstLaunchTutorialRelicLightTexture.height
                ),
                new Vector2(0.5f, 0.5f),
                100f
            );
            firstLaunchTutorialRelicLightSprite.name =
                "B&D Tutorial Relic Light Sprite";

            firstLaunchTutorialRelicLightRoot = new GameObject(
                "Tutorial Relic Completion Light",
                typeof(RectTransform),
                typeof(CanvasGroup)
            );
            firstLaunchTutorialRelicLightRoot.transform.SetParent(
                screenCanvasRoot.transform,
                false
            );
            RectTransform root =
                firstLaunchTutorialRelicLightRoot.GetComponent<RectTransform>();
            root.anchorMin = Vector2.zero;
            root.anchorMax = Vector2.one;
            root.offsetMin = Vector2.zero;
            root.offsetMax = Vector2.zero;
            firstLaunchTutorialRelicLightCanvasGroup =
                firstLaunchTutorialRelicLightRoot.GetComponent<CanvasGroup>();
            firstLaunchTutorialRelicLightCanvasGroup.interactable = false;
            firstLaunchTutorialRelicLightCanvasGroup.blocksRaycasts = false;

            firstLaunchTutorialRelicLightCore = CreatePanel(
                root,
                "Relic Magical Wash",
                0f,
                0f,
                1240f,
                1240f,
                new Color(0.58f, 1f, 0.74f, 0f)
            );
            firstLaunchTutorialRelicLightCore.sprite =
                firstLaunchTutorialRelicLightSprite;
            firstLaunchTutorialRelicLightCore.preserveAspect = true;

            firstLaunchTutorialRelicLightRing = CreatePanel(
                root,
                "Relic Expanding Ring",
                0f,
                0f,
                640f,
                640f,
                new Color(0.78f, 1f, 0.92f, 0f)
            );
            firstLaunchTutorialRelicLightRing.sprite =
                firstLaunchTutorialRelicLightSprite;
            firstLaunchTutorialRelicLightRing.preserveAspect = true;

            for (int index = 0; index < 10; index++)
            {
                Image ray = CreatePanel(
                    root,
                    "Relic Light Ray " + index,
                    0f,
                    0f,
                    28f,
                    860f,
                    new Color(0.74f, 1f, 0.86f, 0f)
                );
                ray.rectTransform.pivot = new Vector2(0.5f, 0f);
                ray.rectTransform.localRotation =
                    Quaternion.Euler(0f, 0f, index * 36f);
                firstLaunchTutorialRelicRays.Add(ray);
            }

            firstLaunchTutorialRelicLightRoot.SetActive(false);
        }

        private static Texture2D CreateFirstLaunchTutorialRelicLightTexture(
            int size)
        {
            int safe = Mathf.Max(64, size);
            Texture2D texture = new Texture2D(
                safe,
                safe,
                TextureFormat.RGBA32,
                false
            );
            texture.name = "B&D Tutorial Relic Radial Light";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            Color[] pixels = new Color[safe * safe];
            Vector2 center = Vector2.one * (safe - 1) * 0.5f;
            float radius = safe * 0.5f;
            for (int y = 0; y < safe; y++)
            {
                for (int x = 0; x < safe; x++)
                {
                    float d = Vector2.Distance(new Vector2(x, y), center) /
                        radius;
                    float alpha = Mathf.Pow(
                        Mathf.Clamp01(1f - d),
                        1.65f
                    );
                    pixels[y * safe + x] =
                        new Color(1f, 1f, 1f, alpha);
                }
            }
            texture.SetPixels(pixels);
            texture.Apply(false, true);
            return texture;
        }

        private void BeginFirstLaunchTutorialRelicCompletion()
        {
            if (firstLaunchTutorialRelicCompletionActive ||
                firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.Collectible)
            {
                return;
            }
            firstLaunchTutorialRelicCompletionActive = true;
            firstLaunchTutorialRelicCompletionMenuRevealed = false;
            firstLaunchTutorialRelicCompletionStartedAt = Time.unscaledTime;
            firstLaunchTutorialRelicCompletionPlayerPosition =
                firstLaunchTutorialPlayerWorldPosition;
            firstLaunchTutorialMovementActive = false;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            if (firstLaunchTutorialRelicLightRoot != null)
            {
                firstLaunchTutorialRelicLightRoot.SetActive(true);
                firstLaunchTutorialRelicLightRoot.transform.SetAsLastSibling();
                firstLaunchTutorialRelicLightCanvasGroup.alpha = 1f;
            }
            ShowFirstLaunchTutorialSuccess("RELIC ACQUIRED");
        }

        private void UpdateFirstLaunchTutorialRelicCompletion()
        {
            if (!firstLaunchTutorialRelicCompletionActive)
                return;

            float elapsed = Time.unscaledTime -
                firstLaunchTutorialRelicCompletionStartedAt;
            firstLaunchTutorialPlayerWorldPosition =
                firstLaunchTutorialRelicCompletionPlayerPosition;

            float lift = SmoothestStep01(
                Mathf.InverseLerp(0f, 0.38f, elapsed)
            );
            Vector2 heldWorld = firstLaunchTutorialRelicCompletionPlayerPosition +
                new Vector2(0f, Mathf.Lerp(8f, 104f, lift));
            firstLaunchTutorialCollectibleWorldPosition = heldWorld;
            if (firstLaunchTutorialCollectible != null)
            {
                firstLaunchTutorialCollectible.gameObject.SetActive(true);
                firstLaunchTutorialCollectible.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(heldWorld);
                float gemPulse = 1f +
                    Mathf.Sin(elapsed * 14f) * 0.12f;
                firstLaunchTutorialCollectible.rectTransform.localScale =
                    Vector3.one * gemPulse;
            }
            if (firstLaunchTutorialPlayer != null)
            {
                firstLaunchTutorialPlayer.rectTransform.localScale =
                    new Vector3(
                        Mathf.Sign(
                            Mathf.Abs(firstLaunchTutorialLastMoveDirection.x) >
                            0.01f
                                ? firstLaunchTutorialLastMoveDirection.x
                                : 1f
                        ) * (1f - lift * 0.05f),
                        1f + lift * 0.08f,
                        1f
                    );
            }

            float burst = SmoothestStep01(
                Mathf.InverseLerp(0.34f, 1.18f, elapsed)
            );
            float fade = 1f - SmoothestStep01(
                Mathf.InverseLerp(1.28f, 1.92f, elapsed)
            );
            if (firstLaunchTutorialRelicLightRoot != null)
            {
                firstLaunchTutorialRelicLightRoot.SetActive(true);
                firstLaunchTutorialRelicLightRoot.transform.SetAsLastSibling();
                firstLaunchTutorialRelicLightCanvasGroup.alpha = fade;
                Vector2 screenOrigin = new Vector2(
                    (heldWorld.x - firstLaunchTutorialCameraWorldX) +
                        CanvasSize.x * 0.5f,
                    heldWorld.y + CanvasSize.y * 0.5f
                );
                firstLaunchTutorialRelicLightCore.rectTransform
                    .anchoredPosition = screenOrigin - CanvasSize * 0.5f;
                firstLaunchTutorialRelicLightRing.rectTransform
                    .anchoredPosition = screenOrigin - CanvasSize * 0.5f;
                firstLaunchTutorialRelicLightCore.rectTransform.localScale =
                    Vector3.one * Mathf.Lerp(0.02f, 2.55f, burst);
                firstLaunchTutorialRelicLightRing.rectTransform.localScale =
                    Vector3.one * Mathf.Lerp(0.04f, 2.15f, burst);
                firstLaunchTutorialRelicLightCore.color = new Color(
                    0.58f,
                    1f,
                    0.74f,
                    Mathf.Clamp01(burst * 1.25f) * 0.98f
                );
                firstLaunchTutorialRelicLightRing.color = new Color(
                    0.88f,
                    1f,
                    0.96f,
                    Mathf.Sin(burst * Mathf.PI) * 0.82f
                );
                for (int index = 0;
                     index < firstLaunchTutorialRelicRays.Count;
                     index++)
                {
                    Image ray = firstLaunchTutorialRelicRays[index];
                    ray.rectTransform.anchoredPosition =
                        screenOrigin - CanvasSize * 0.5f;
                    ray.rectTransform.localScale = new Vector3(
                        Mathf.Lerp(0.12f, 1f, burst),
                        Mathf.Lerp(0.02f, 1.35f, burst),
                        1f
                    );
                    ray.color = new Color(
                        0.74f,
                        1f,
                        0.86f,
                        Mathf.Sin(burst * Mathf.PI) * 0.26f
                    );
                }
            }

            if (!firstLaunchTutorialRelicCompletionMenuRevealed &&
                elapsed >= 1.22f)
            {
                firstLaunchTutorialRelicCompletionMenuRevealed = true;
                BDFirstLaunchTutorialStateStore.MarkCompleted();
                firstLaunchTutorialFinishedThisSession = true;
                firstLaunchTutorialActive = false;
                firstLaunchTutorialExitOpen = false;
                firstLaunchTutorialTransitionOut = false;
                ClearFirstLaunchTutorialPhysicalHighlight();
                displayedPageInitialized = false;
                menuInputUnlockAt = Time.unscaledTime + 0.38f;
                menuInputNeedsRelease = true;
            }

            if (elapsed < 1.94f)
                return;
            firstLaunchTutorialRelicCompletionActive = false;
            if (firstLaunchTutorialRelicLightRoot != null)
                firstLaunchTutorialRelicLightRoot.SetActive(false);
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDTutorialFinalProductionDriver : MonoBehaviour
    {
        private BDModernHandheld3DPresenter owner;

        public void Configure(BDModernHandheld3DPresenter value)
        {
            owner = value;
        }

        private void LateUpdate()
        {
            if (owner != null)
                owner.TickFirstLaunchTutorialFinalProductionPass();
        }
    }
}
