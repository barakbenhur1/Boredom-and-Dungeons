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
        private enum TutorialEnemyRole
        {
            Sword,
            Ranged,
            Small,
            MiniBoss
        }

        private enum TutorialLearningState
        {
            NotSeen,
            Introduced,
            Attempted,
            Performed,
            Demonstrated,
            MasteredForTutorial
        }

        private sealed class TutorialEnemyActor
        {
            public Image Image;
            public TutorialEnemyRole Role;
            public Vector2 Position;
            public Vector2 SpawnPosition;
            public float Health;
            public float MaximumHealth;
            public float NextActionAt;
            public float ActionStartedAt;
            public bool Active;
            public bool AttackCommitted;
            public bool DamageApplied;
            public bool UsesProjectileAttack;
            public int AttackSequence;
            public bool Dead;
        }

        private const float TutorialGroundY = -108f;
        private const float TutorialJumpVelocity = 390f;
        private const float TutorialGravity = 980f;
        private const float TutorialJumpObstacleX = -760f;
        private const float TutorialWallJumpWallX = 4380f;
        private const float TutorialWallJumpPlatformMinX = 3960f;
        private const float TutorialWallJumpPlatformMaxX = 4270f;
        private const float TutorialWallJumpPlatformY = -10f;
        private const float TutorialWallJumpPlatformStandingY = 8f;
        private const float TutorialWallJumpUpperGroundMinX = 4407f;
        private const float TutorialWallJumpUpperGroundMaxX = 4620f;
        private const float TutorialWallJumpUpperGroundY = 46f;
        private const float TutorialWallJumpUpperGroundStandingY = 64f;
        private const float TutorialHazardStationX = 3460f;
        private const float TutorialMountedStationX = 2540f;
        private const float TutorialSecretBranchX = 3820f;
        private const float TutorialCombinedStationX = 4740f;
        private const float TutorialMiniBossStationX = 5280f;
        private const float TutorialFinishX = 6120f;
        private const int TutorialMagazineSize = 4;

        private readonly List<TutorialEnemyActor> firstLaunchTutorialActors =
            new List<TutorialEnemyActor>(8);
        private readonly Dictionary<string, TutorialLearningState>
            firstLaunchTutorialLearningStates =
                new Dictionary<string, TutorialLearningState>(16);
        private Image firstLaunchTutorialEnemySecondary;
        private Image firstLaunchTutorialEnemyTertiary;
        private Image firstLaunchTutorialRangedEnemy;
        private Image firstLaunchTutorialMiniBoss;
        private Image firstLaunchTutorialSecret;
        private Image firstLaunchTutorialFinishGate;
        private Image firstLaunchTutorialJumpObstacle;
        private Image firstLaunchTutorialWallJumpWall;
        private Image firstLaunchTutorialWallJumpPlatform;
        private Image firstLaunchTutorialWallJumpUpperGround;
        private Image firstLaunchTutorialBossAttackTelegraph;
        private Image firstLaunchTutorialBossImpactFlash;
        private Text firstLaunchTutorialHealthText;
        private Text firstLaunchTutorialAmmoText;
        private float firstLaunchTutorialVerticalVelocity;
        private float firstLaunchTutorialGroundedY = TutorialGroundY;
        private float firstLaunchTutorialInvulnerableUntil;
        private float firstLaunchTutorialNextEnemyAttackAt;
        private float firstLaunchTutorialReloadCompletesAt;
        private float firstLaunchTutorialMiniBossPhaseStartedAt;
        private float firstLaunchTutorialCheckpointX;
        private float firstLaunchTutorialMountedCurrentSpeed;
        private int firstLaunchTutorialPlayerHealth;
        private int firstLaunchTutorialAmmo;
        private bool firstLaunchTutorialGrounded;
        private bool firstLaunchTutorialWallJumpPlatformReached;
        private bool firstLaunchTutorialWallJumpConsumed;
        private bool firstLaunchTutorialSecretCollected;
        private bool firstLaunchTutorialCombinedStarted;
        private bool firstLaunchTutorialMiniBossPhaseTwo;
        private bool firstLaunchTutorialMiniBossDeathStarted;
        private bool firstLaunchTutorialFinishGateOpen;
        private bool firstLaunchTutorialProductionLightResolved;
        private bool firstLaunchTutorialProductionHeavyResolved;
        private int firstLaunchTutorialHintLevel;
        private float firstLaunchTutorialHintAt;
        private float firstLaunchTutorialPlayerDeathStartedAt;
        private float firstLaunchTutorialRunStartedAt;
        private Text firstLaunchTutorialBossHealthText;
        private int firstLaunchTutorialDisplayedHealth = -1;
        private int firstLaunchTutorialDisplayedAmmo = -1;
        private int firstLaunchTutorialDisplayedBossHealth = -1;
        private bool firstLaunchTutorialDisplayedReloading;
        private bool firstLaunchTutorialDisplayedBossVisible;
        private string firstLaunchTutorialDisplayedBossState = string.Empty;
        private bool firstLaunchTutorialPlayerDeathActive;
        private bool firstLaunchTutorialRespawnResetApplied;
        private TutorialEnemyActor firstLaunchTutorialHazardKnockbackActor;
        private Vector2 firstLaunchTutorialHazardKnockbackStart;
        private Vector2 firstLaunchTutorialHazardKnockbackTarget;
        private float firstLaunchTutorialHazardKnockbackStartedAt;
        private bool firstLaunchTutorialHazardKnockbackActive;
        private bool firstLaunchTutorialHazardKnockbackImpactApplied;

        private void InitializeFirstLaunchTutorialProductionCourse()
        {
            if (firstLaunchTutorialCourseRoot == null)
                return;

            firstLaunchTutorialJumpObstacle = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Jump Root",
                TutorialJumpObstacleX,
                TutorialGroundY - 4f,
                64f,
                58f,
                new Color(0.24f, 0.16f, 0.10f, 1f)
            );
            firstLaunchTutorialWallJumpWall = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Wall Jump Wall",
                TutorialWallJumpWallX,
                TutorialGroundY + 42f,
                54f,
                300f,
                new Color(0.18f, 0.28f, 0.38f, 1f)
            );
            firstLaunchTutorialWallJumpPlatform = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Wall Jump High Ground",
                (ResolveFirstLaunchTutorialWallJumpPlatformMinX() + ResolveFirstLaunchTutorialWallJumpPlatformMaxX()) * 0.5f,
                TutorialWallJumpPlatformY - 22f,
                ResolveFirstLaunchTutorialWallJumpPlatformMaxX() - ResolveFirstLaunchTutorialWallJumpPlatformMinX(),
                44f,
                new Color(0.24f, 0.38f, 0.28f, 1f)
            );
            firstLaunchTutorialWallJumpUpperGround = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Wall Jump Upper Ground",
                (ResolveFirstLaunchTutorialWallJumpUpperGroundMinX() +
                 ResolveFirstLaunchTutorialWallJumpUpperGroundMaxX()) * 0.5f,
                TutorialWallJumpUpperGroundY - 22f,
                ResolveFirstLaunchTutorialWallJumpUpperGroundMaxX() -
                    ResolveFirstLaunchTutorialWallJumpUpperGroundMinX(),
                44f,
                new Color(0.24f, 0.38f, 0.28f, 1f)
            );
            DecorateFirstLaunchTutorialWallJumpGeometry();
            firstLaunchTutorialWallJumpWall.gameObject.SetActive(true);
            firstLaunchTutorialWallJumpPlatform.gameObject.SetActive(true);
            firstLaunchTutorialWallJumpUpperGround.gameObject.SetActive(true);
            firstLaunchTutorialBossAttackTelegraph = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Boss Attack Telegraph",
                0f,
                TutorialGroundY - 16f,
                300f,
                20f,
                new Color(1f, 0.64f, 0.16f, 0.88f)
            );
            firstLaunchTutorialBossImpactFlash = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Boss Impact Flash",
                0f,
                TutorialGroundY + 18f,
                84f,
                104f,
                new Color(1f, 0.18f, 0.12f, 0.82f)
            );
            firstLaunchTutorialBossAttackTelegraph.gameObject.SetActive(false);
            firstLaunchTutorialBossImpactFlash.gameObject.SetActive(false);
            firstLaunchTutorialEnemySecondary = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Enemy Secondary",
                0f,
                TutorialGroundY,
                49f,
                77f,
                new Color(0.82f, 0.18f, 0.24f, 1f)
            );
            firstLaunchTutorialEnemyTertiary = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Enemy Tertiary",
                0f,
                TutorialGroundY,
                45f,
                68f,
                new Color(0.86f, 0.28f, 0.18f, 1f)
            );
            firstLaunchTutorialRangedEnemy = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Ranged Enemy",
                0f,
                TutorialGroundY,
                52f,
                74f,
                new Color(0.66f, 0.16f, 0.70f, 1f)
            );
            firstLaunchTutorialMiniBoss = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Mini Boss",
                TutorialMiniBossStationX + 220f,
                TutorialGroundY + 10f,
                112f,
                132f,
                new Color(0.74f, 0.10f, 0.22f, 1f)
            );
            firstLaunchTutorialSecret = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Optional Secret",
                TutorialSecretBranchX + 150f,
                TutorialGroundY + 82f,
                34f,
                44f,
                new Color(0.28f, 1f, 0.72f, 1f)
            );
            firstLaunchTutorialFinishGate = CreateTutorialEntity(
                firstLaunchTutorialCourseRoot,
                "Tutorial Finish Gate",
                TutorialFinishX,
                TutorialGroundY + 24f,
                44f,
                154f,
                new Color(0.24f, 0.72f, 1f, 0.92f)
            );
            // Progression remains softly clamped, but the former vertical
            // lesson-divider line is intentionally not created.

            if (firstLaunchTutorialProgress != null)
            {
                firstLaunchTutorialHealthText = CreateText(
                    firstLaunchTutorialProgress.rectTransform.parent,
                    "Tutorial Health",
                    string.Empty,
                    -300f,
                    326f,
                    220f,
                    22f,
                    14,
                    TextAnchor.MiddleLeft,
                    new Color(1f, 0.50f, 0.52f, 1f),
                    FontStyle.Bold
                );
                firstLaunchTutorialAmmoText = CreateText(
                    firstLaunchTutorialProgress.rectTransform.parent,
                    "Tutorial Ammo",
                    string.Empty,
                    300f,
                    326f,
                    220f,
                    22f,
                    14,
                    TextAnchor.MiddleRight,
                    new Color(0.42f, 0.88f, 1f, 1f),
                    FontStyle.Bold
                );
                firstLaunchTutorialBossHealthText = CreateText(
                    firstLaunchTutorialProgress.rectTransform.parent,
                    "Tutorial Mini Boss Health",
                    string.Empty,
                    0f,
                    300f,
                    430f,
                    20f,
                    13,
                    TextAnchor.MiddleCenter,
                    new Color(1f, 0.34f, 0.42f, 1f),
                    FontStyle.Bold
                );
            }

            firstLaunchTutorialActors.Clear();
            firstLaunchTutorialLearningStates.Clear();
            RegisterTutorialActor(firstLaunchTutorialEnemy, TutorialEnemyRole.Sword);
            RegisterTutorialActor(firstLaunchTutorialEnemySecondary, TutorialEnemyRole.Small);
            RegisterTutorialActor(firstLaunchTutorialEnemyTertiary, TutorialEnemyRole.Small);
            RegisterTutorialActor(firstLaunchTutorialRangedEnemy, TutorialEnemyRole.Ranged);
            RegisterTutorialActor(firstLaunchTutorialMiniBoss, TutorialEnemyRole.MiniBoss);

            firstLaunchTutorialVerticalVelocity = 0f;
            firstLaunchTutorialGrounded = true;
            firstLaunchTutorialWallJumpConsumed = false;
            firstLaunchTutorialPlayerHealth = 6;
            firstLaunchTutorialAmmo = TutorialMagazineSize;
            firstLaunchTutorialCheckpointX = firstLaunchTutorialPlayerWorldPosition.x;
            firstLaunchTutorialMountedCurrentSpeed = 0f;
            firstLaunchTutorialSecretCollected = false;
            firstLaunchTutorialCombinedStarted = false;
            firstLaunchTutorialMiniBossPhaseTwo = false;
            firstLaunchTutorialMiniBossDeathStarted = false;
            firstLaunchTutorialFinishGateOpen = false;
            firstLaunchTutorialPlayerDeathActive = false;
            firstLaunchTutorialRespawnResetApplied = false;
            firstLaunchTutorialPlayerDeathStartedAt = 0f;
            firstLaunchTutorialRunStartedAt = Time.unscaledTime;
            ResetFirstLaunchTutorialV108Transactions();
            firstLaunchTutorialDisplayedHealth = -1;
            firstLaunchTutorialDisplayedAmmo = -1;
            firstLaunchTutorialDisplayedBossHealth = -1;
            firstLaunchTutorialDisplayedReloading = false;
            firstLaunchTutorialDisplayedBossVisible = false;
            firstLaunchTutorialDisplayedBossState = string.Empty;
            firstLaunchTutorialHazardKnockbackActor = null;
            firstLaunchTutorialHazardKnockbackStart = Vector2.zero;
            firstLaunchTutorialHazardKnockbackTarget = Vector2.zero;
            firstLaunchTutorialHazardKnockbackStartedAt = 0f;
            firstLaunchTutorialHazardKnockbackActive = false;
            firstLaunchTutorialHazardKnockbackImpactApplied = false;
            firstLaunchTutorialHintLevel = 0;
            firstLaunchTutorialHintAt = Time.unscaledTime + 7f;
            SetProductionEntitiesVisible(false);
            ResetFirstLaunchTutorialRespawnPresentation();
            UpdateFirstLaunchTutorialProductionHud();
        }

        private void RegisterTutorialActor(Image image, TutorialEnemyRole role)
        {
            if (image == null)
                return;
            firstLaunchTutorialActors.Add(new TutorialEnemyActor
            {
                Image = image,
                Role = role,
                MaximumHealth = role == TutorialEnemyRole.MiniBoss ? 12f : 2f,
                Health = role == TutorialEnemyRole.MiniBoss ? 12f : 2f
            });
        }

        private void DecorateFirstLaunchTutorialWallJumpGeometry()
        {
            Color outline = new Color(0.075f, 0.075f, 0.13f, 1f);
            Color stoneDark = new Color(0.13f, 0.18f, 0.28f, 1f);
            Color stoneLight = new Color(0.40f, 0.48f, 0.58f, 1f);
            Color mossDark = new Color(0.08f, 0.24f, 0.18f, 1f);
            Color mossLight = new Color(0.28f, 0.58f, 0.30f, 1f);

            AddOutline(
                firstLaunchTutorialWallJumpWall.gameObject,
                outline,
                4f
            );
            for (int row = 0; row < 7; row++)
            {
                float y = -126f + row * 42f;
                CreatePanel(
                    firstLaunchTutorialWallJumpWall.rectTransform,
                    "Wall Stone Light " + row,
                    row % 2 == 0 ? -12f : 10f,
                    y,
                    22f,
                    8f,
                    stoneLight
                ).raycastTarget = false;
                CreatePanel(
                    firstLaunchTutorialWallJumpWall.rectTransform,
                    "Wall Stone Shadow " + row,
                    row % 2 == 0 ? 10f : -12f,
                    y - 12f,
                    18f,
                    8f,
                    stoneDark
                ).raycastTarget = false;
            }

            DecorateFirstLaunchTutorialPlatform(
                firstLaunchTutorialWallJumpPlatform,
                mossDark,
                mossLight,
                stoneDark,
                stoneLight
            );
            DecorateFirstLaunchTutorialPlatform(
                firstLaunchTutorialWallJumpUpperGround,
                mossDark,
                mossLight,
                stoneDark,
                stoneLight
            );
        }

        private void DecorateFirstLaunchTutorialPlatform(
            Image platform,
            Color mossDark,
            Color mossLight,
            Color stoneDark,
            Color stoneLight)
        {
            if (platform == null)
                return;

            RectTransform rect = platform.rectTransform;
            CreatePanel(
                rect,
                "Pixel Grass Dark",
                0f,
                14f,
                rect.sizeDelta.x,
                8f,
                mossDark
            ).raycastTarget = false;
            CreatePanel(
                rect,
                "Pixel Grass Light",
                -8f,
                20f,
                rect.sizeDelta.x - 16f,
                4f,
                mossLight
            ).raycastTarget = false;
            CreatePanel(
                rect,
                "Pixel Stone Highlight",
                -12f,
                -2f,
                rect.sizeDelta.x - 24f,
                6f,
                stoneLight
            ).raycastTarget = false;
            CreatePanel(
                rect,
                "Pixel Stone Shadow",
                10f,
                -14f,
                rect.sizeDelta.x - 20f,
                8f,
                stoneDark
            ).raycastTarget = false;
        }

        private void DisposeFirstLaunchTutorialProductionCourse()
        {
            firstLaunchTutorialActors.Clear();
            firstLaunchTutorialLearningStates.Clear();
            firstLaunchTutorialEnemySecondary = null;
            firstLaunchTutorialEnemyTertiary = null;
            firstLaunchTutorialRangedEnemy = null;
            firstLaunchTutorialMiniBoss = null;
            firstLaunchTutorialSecret = null;
            firstLaunchTutorialFinishGate = null;
            firstLaunchTutorialJumpObstacle = null;
            firstLaunchTutorialWallJumpWall = null;
            firstLaunchTutorialWallJumpPlatform = null;
            firstLaunchTutorialWallJumpUpperGround = null;
            firstLaunchTutorialBossAttackTelegraph = null;
            firstLaunchTutorialBossImpactFlash = null;
            firstLaunchTutorialHealthText = null;
            firstLaunchTutorialAmmoText = null;
            firstLaunchTutorialBossHealthText = null;
            firstLaunchTutorialPlayerDeathActive = false;
            firstLaunchTutorialRespawnResetApplied = false;
            ResetFirstLaunchTutorialRespawnPresentation();
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;
            firstLaunchTutorialReloadCompletesAt = 0f;
            firstLaunchTutorialHazardKnockbackActor = null;
            firstLaunchTutorialHazardKnockbackActive = false;
            firstLaunchTutorialHazardKnockbackImpactApplied = false;
            ResetFirstLaunchTutorialV108Transactions();
        }

        private void UpdateFirstLaunchTutorialProductionCourse(float elapsed)
        {
            if (UpdateFirstLaunchTutorialPlayerDeath())
            {
                UpdateFirstLaunchTutorialProductionHud();
                return;
            }

            UpdateFirstLaunchTutorialJumpPhysics();
            UpdateFirstLaunchTutorialReload();
            // BD ORDER-INDEPENDENT MOUNTED RANGED UPDATE V10.11.24
            UpdateFirstLaunchTutorialMountedRangedSequenceV101124();
            UpdateFirstLaunchTutorialEnemyProjectileTransaction();
            UpdateFirstLaunchTutorialBossFanProjectiles();
            UpdateFirstLaunchTutorialEnemyTransactions();
            UpdateFirstLaunchTutorialProductionHeldCombat();
            UpdateFirstLaunchTutorialMountedImpact();
            UpdateFirstLaunchTutorialHazardKnockbackSequence();
            UpdateFirstLaunchTutorialHintEscalation();
            UpdateFirstLaunchTutorialProductionHud();

            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.SidePath:
                    UpdateFirstLaunchTutorialSecretRoute();
                    break;
                case FirstLaunchTutorialStep.CombinedEncounter:
                    UpdateFirstLaunchTutorialCombinedEncounter();
                    break;
                case FirstLaunchTutorialStep.MiniBossIntro:
                    // Final-test combat begins only after the player confirms
                    // the persistent explanation.
                    break;
                case FirstLaunchTutorialStep.MiniBossPhaseOne:
                case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                    UpdateFirstLaunchTutorialMiniBoss();
                    break;
                case FirstLaunchTutorialStep.MiniBossDefeated:
                    UpdateFirstLaunchTutorialMiniBossDeath(elapsed);
                    break;
            }
        }

        private void SetFirstLaunchTutorialLearningState(
            string actionId,
            TutorialLearningState state)
        {
            TutorialLearningState current;
            if (firstLaunchTutorialLearningStates.TryGetValue(actionId, out current) &&
                current >= state)
            {
                return;
            }
            firstLaunchTutorialLearningStates[actionId] = state;
        }

        private void ResetFirstLaunchTutorialHintEscalation()
        {
            firstLaunchTutorialHintLevel = 0;
            firstLaunchTutorialHintAt = Time.unscaledTime + 7f;
        }

        private void UpdateFirstLaunchTutorialHintEscalation()
        {
            if (!firstLaunchTutorialInstructionRequested ||
                firstLaunchTutorialExitOpen ||
                firstLaunchTutorialTransitionOut ||
                Time.unscaledTime < firstLaunchTutorialHintAt)
            {
                return;
            }

            firstLaunchTutorialHintLevel++;
            firstLaunchTutorialHintAt = Time.unscaledTime + 6f;
            if (firstLaunchTutorialFeedback == null)
                return;

            switch (firstLaunchTutorialHintLevel)
            {
                case 1:
                    firstLaunchTutorialFeedback.text = "TRY THE HIGHLIGHTED ACTION";
                    break;
                case 2:
                    firstLaunchTutorialFeedback.text = "MOVE CLOSER TO THE HIGHLIGHTED OBJECT";
                    break;
                default:
                    firstLaunchTutorialFeedback.text = "WATCH THE KEYCAP DEMONSTRATION";
                    break;
            }
            firstLaunchTutorialFeedbackClearAt = Time.unscaledTime + 2.2f;
        }

        private float ResolveFirstLaunchTutorialTravelSpeed(Vector2 movement)
        {
            if (!firstLaunchTutorialMounted)
            {
                firstLaunchTutorialMountedCurrentSpeed = 0f;
                return firstLaunchTutorialStep ==
                        FirstLaunchTutorialStep.WallJump
                    ? 250f
                    : TutorialFootMoveSpeed;
            }

            float target = Mathf.Abs(movement.x) > 0.01f
                ? TutorialMountedMoveSpeed
                : 0f;
            float rate = target > firstLaunchTutorialMountedCurrentSpeed
                ? 330f
                : 460f;
            firstLaunchTutorialMountedCurrentSpeed = Mathf.MoveTowards(
                firstLaunchTutorialMountedCurrentSpeed,
                target,
                rate * Time.unscaledDeltaTime
            );
            return firstLaunchTutorialMountedCurrentSpeed;
        }

        private void UpdateFirstLaunchTutorialJumpPhysics()
        {
            if (firstLaunchTutorialMounted)
            {
                firstLaunchTutorialVerticalVelocity = 0f;
                firstLaunchTutorialGrounded = true;
                firstLaunchTutorialGroundedY = TutorialGroundY;
                firstLaunchTutorialPlayerWorldPosition.y =
                    TutorialGroundY;
                return;
            }

            ReleaseFirstLaunchTutorialWallJumpSupportIfNeeded();
            if (firstLaunchTutorialGrounded)
            {
                firstLaunchTutorialPlayerWorldPosition.y = firstLaunchTutorialGroundedY;
                return;
            }

            firstLaunchTutorialVerticalVelocity -= TutorialGravity * Time.unscaledDeltaTime;
            float previousY = firstLaunchTutorialPlayerWorldPosition.y;
            firstLaunchTutorialPlayerWorldPosition.y +=
                firstLaunchTutorialVerticalVelocity * Time.unscaledDeltaTime;

            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.WallJump &&
                firstLaunchTutorialVerticalVelocity <= 0f &&
                firstLaunchTutorialPlayerWorldPosition.x >=
                    ResolveFirstLaunchTutorialWallJumpPlatformMinX() &&
                firstLaunchTutorialPlayerWorldPosition.x <=
                    ResolveFirstLaunchTutorialWallJumpPlatformMaxX() &&
                previousY >= TutorialWallJumpPlatformStandingY &&
                firstLaunchTutorialPlayerWorldPosition.y <=
                    TutorialWallJumpPlatformStandingY)
            {
                firstLaunchTutorialGroundedY =
                    TutorialWallJumpPlatformStandingY;
                firstLaunchTutorialPlayerWorldPosition.y =
                    TutorialWallJumpPlatformStandingY;
                firstLaunchTutorialVerticalVelocity = 0f;
                firstLaunchTutorialGrounded = true;
                firstLaunchTutorialWallJumpConsumed = false;
                firstLaunchTutorialWallJumpPlatformReached = true;
                ShowFirstLaunchTutorialSuccess("PLATFORM REACHED — JUMP RIGHT");
                return;
            }

            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.WallJump &&
                firstLaunchTutorialWallJumpPlatformReached &&
                firstLaunchTutorialVerticalVelocity <= 0f &&
                firstLaunchTutorialPlayerWorldPosition.x >=
                    ResolveFirstLaunchTutorialWallJumpUpperGroundMinX() &&
                firstLaunchTutorialPlayerWorldPosition.x <=
                    ResolveFirstLaunchTutorialWallJumpUpperGroundMaxX() &&
                previousY >= TutorialWallJumpUpperGroundStandingY &&
                firstLaunchTutorialPlayerWorldPosition.y <=
                    TutorialWallJumpUpperGroundStandingY)
            {
                firstLaunchTutorialGroundedY =
                    TutorialWallJumpUpperGroundStandingY;
                firstLaunchTutorialPlayerWorldPosition.y =
                    TutorialWallJumpUpperGroundStandingY;
                firstLaunchTutorialVerticalVelocity = 0f;
                firstLaunchTutorialGrounded = true;
                firstLaunchTutorialWallJumpConsumed = false;
                ShowFirstLaunchTutorialSuccess("ABOVE THE WALL — CONTINUE RIGHT");
                return;
            }

            if (firstLaunchTutorialPlayerWorldPosition.y > firstLaunchTutorialGroundedY)
                return;

            firstLaunchTutorialPlayerWorldPosition.y = firstLaunchTutorialGroundedY;
            firstLaunchTutorialVerticalVelocity = 0f;
            firstLaunchTutorialGrounded = true;
            firstLaunchTutorialWallJumpConsumed = false;
            // BD HORSE-FREE OPENING HANDOFF V10.11.30.27
            // The opening room owns only Move + Jump. Landing beyond the root
            // queues the dedicated Quick Attack room; no horse is spawned or
            // referenced in the opening room.
            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.Jump &&
                firstLaunchTutorialPlayerWorldPosition.x >=
                    TutorialJumpObstacleX + 24f)
            {
                SetFirstLaunchTutorialLearningState(
                    "Jump",
                    TutorialLearningState.Demonstrated
                );
                ShowFirstLaunchTutorialSuccess("JUMP CLEARED");
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.AttackEnemy);
                SetFirstLaunchTutorialInstructionRequested(true);
            }
        }

        private void RequestFirstLaunchTutorialJump()
        {
            if (firstLaunchTutorialMounted ||
                !IsFirstLaunchTutorialMechanicUnlocked(2) ||
                firstLaunchTutorialContinuousHandoffActive)
            {
                return;
            }

            if (!firstLaunchTutorialGrounded)
            {
                if (firstLaunchTutorialStep != FirstLaunchTutorialStep.WallJump ||
                    firstLaunchTutorialWallJumpConsumed ||
                    Mathf.Abs(
                        firstLaunchTutorialPlayerWorldPosition.x -
                        ResolveFirstLaunchTutorialWallJumpWallX()) > 70f)
                {
                    return;
                }

                firstLaunchTutorialWallJumpConsumed = true;
                firstLaunchTutorialLastMoveDirection = Vector2.left;
                firstLaunchTutorialPlayerWorldPosition.x =
                    ResolveFirstLaunchTutorialWallJumpWallX() - 82f;
                firstLaunchTutorialVerticalVelocity =
                    TutorialJumpVelocity * 1.08f;
                ShowFirstLaunchTutorialSuccess("WALL JUMP");
                PlayClick();
                return;
            }

            firstLaunchTutorialGrounded = false;
            firstLaunchTutorialWallJumpConsumed = false;
            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.WallJump &&
                firstLaunchTutorialGroundedY ==
                    TutorialWallJumpPlatformStandingY)
            {
                firstLaunchTutorialGroundedY = TutorialGroundY;
                firstLaunchTutorialVerticalVelocity =
                    TutorialJumpVelocity * 1.28f;
            }
            else
            {
                firstLaunchTutorialVerticalVelocity = TutorialJumpVelocity;
            }
            SetFirstLaunchTutorialLearningState(
                "Jump",
                TutorialLearningState.Performed
            );
            firstLaunchTutorialMovementActive = true;
            PlayClick();
        }

        private bool ReadFirstLaunchTutorialJumpPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                return true;
            }
            if (Gamepad.current != null &&
                Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                return true;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                return true;
            }
#endif
            return false;
        }

        private bool TryHandleFirstLaunchTutorialProductionAction(
            BDModernHandheldControlTarget.ControlAction action)
        {
            if (action ==
                    BDModernHandheldControlTarget.ControlAction.ContextBackSettings &&
                !firstLaunchTutorialMounted)
            {
                RequestFirstLaunchTutorialJump();
                return true;
            }

            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.SidePath &&
                action == BDModernHandheldControlTarget.ControlAction.Confirm)
            {
                TryCollectFirstLaunchTutorialSecret();
                return true;
            }

            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.HazardKnockback &&
                (action == BDModernHandheldControlTarget.ControlAction.Primary ||
                 action == BDModernHandheldControlTarget.ControlAction.Credits))
            {
                ResolveFirstLaunchTutorialHazardKnockback();
                return true;
            }

            if ((firstLaunchTutorialStep == FirstLaunchTutorialStep.CombinedEncounter ||
                 firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseOne ||
                 firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseTwo) &&
                action == BDModernHandheldControlTarget.ControlAction.Primary)
            {
                if (firstLaunchTutorialMounted)
                {
                    ShowFirstLaunchTutorialSuccess("ON HORSE: RANGED ATTACKS ONLY");
                    return true;
                }
                firstLaunchTutorialPrimaryHoldStartedAt = Time.unscaledTime;
                firstLaunchTutorialProductionLightResolved = false;
                return true;
            }

            if ((firstLaunchTutorialStep == FirstLaunchTutorialStep.CombinedEncounter ||
                 firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseOne ||
                 firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseTwo) &&
                action == BDModernHandheldControlTarget.ControlAction.Credits)
            {
                if (firstLaunchTutorialMounted)
                {
                    ShowFirstLaunchTutorialSuccess("ON HORSE: RANGED ATTACKS ONLY");
                    return true;
                }
                if (TryResolveFirstLaunchTutorialContextParry())
                    return true;
                firstLaunchTutorialGrappleHoldStartedAt = Time.unscaledTime;
                firstLaunchTutorialProductionHeavyResolved = false;
                return true;
            }

            if ((firstLaunchTutorialStep == FirstLaunchTutorialStep.CombinedEncounter ||
                 firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseOne ||
                 firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseTwo ||
                 firstLaunchTutorialStep == FirstLaunchTutorialStep.RangedAttack) &&
                action == BDModernHandheldControlTarget.ControlAction.Progression)
            {
                FireFirstLaunchTutorialProductionShot();
                return true;
            }

            return false;
        }

        private bool TryResolveFirstLaunchTutorialContextParry()
        {
            float now = Time.unscaledTime;
            for (int i = 0; i < firstLaunchTutorialActors.Count; i++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[i];
                if (!actor.Active || actor.Dead || !actor.AttackCommitted)
                    continue;

                float attackElapsed = now - actor.ActionStartedAt;
                if (attackElapsed < 0.30f || attackElapsed > 0.58f)
                    continue;
                if (Vector2.Distance(actor.Position, firstLaunchTutorialPlayerWorldPosition) >
                    (actor.Role == TutorialEnemyRole.Ranged ? 520f : 270f))
                {
                    continue;
                }

                actor.AttackCommitted = false;
                actor.DamageApplied = false;
                actor.NextActionAt = now + 1.25f;
                firstLaunchTutorialInvulnerableUntil = now + 0.32f;
                SetFirstLaunchTutorialLearningState(
                    "Parry",
                    TutorialLearningState.Demonstrated
                );
                PlayFirstLaunchTutorialParryAnimation(false);
                ShowFirstLaunchTutorialSuccess("PARRY");
                return true;
            }
            return false;
        }

        private void UpdateFirstLaunchTutorialProductionHeldCombat()
        {
            // Held light/spin and heavy/grapple arbitration is global and owned
            // by UpdateFirstLaunchTutorialHeldActions for every tutorial step.
        }

        private void ResolveFirstLaunchTutorialProductionSpin()
        {
            // BD ATOMIC TWO-TARGET SPIN ROUTING V10.11.28
            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.SpinAttack)
            {
                PlayFirstLaunchTutorialSpinAttackAnimation(
                    advancesLesson: true
                );
                return;
            }

            bool hit = false;
            for (int i = 0; i < firstLaunchTutorialActors.Count; i++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[i];
                if (!actor.Active || actor.Dead ||
                    Vector2.Distance(
                        actor.Position,
                        firstLaunchTutorialPlayerWorldPosition) > 210f)
                {
                    continue;
                }
                bool damaged = TryApplyFirstLaunchTutorialLessonDamageV101125(
                    actor,
                    0.82f,
                    FirstLaunchTutorialDamageSourceV101125.Spin
                );
                if (damaged && actor.Role != TutorialEnemyRole.MiniBoss)
                {
                    actor.Position.x += Mathf.Sign(
                        actor.Position.x - firstLaunchTutorialPlayerWorldPosition.x
                    ) * 54f;
                }
                hit |= damaged;
            }
            PlayFirstLaunchTutorialSpinAttackAnimation(false);
            SetFirstLaunchTutorialLearningState(
                "Spin",
                hit ? TutorialLearningState.Demonstrated :
                      TutorialLearningState.Attempted
            );
            if (!hit)
                ShowFirstLaunchTutorialSuccess("NO TARGET HIT");
        }
        private void ResolveFirstLaunchTutorialProductionHook()
        {
            TutorialEnemyActor actor =
                FindClosestLivingTutorialActor(520f);
            if (actor == null)
            {
                PlayFirstLaunchTutorialGrappleAnimation(false);
                ShowFirstLaunchTutorialSuccess(
                    "THE HOOK FOUND NOTHING"
                );
                return;
            }

            BeginFirstLaunchTutorialProductionHookTransaction(
                actor,
                0.5f
            );
            PlayFirstLaunchTutorialGrappleAnimation(false);
            SetFirstLaunchTutorialLearningState(
                "Grapple",
                TutorialLearningState.Demonstrated
            );
        }

        private void ResolveFirstLaunchTutorialHazardKnockback()
        {
            if (firstLaunchTutorialHazardKnockbackActive)
                return;

            TutorialEnemyActor actor =
                FindClosestLivingTutorialActor(260f, requireForward: false);
            if (actor == null ||
                firstLaunchTutorialHazard == null ||
                !firstLaunchTutorialHazard.gameObject.activeInHierarchy)
            {
                ShowFirstLaunchTutorialSuccess(
                    "ATTACK THE ENEMY TOWARD THE HAZARD"
                );
                return;
            }

            firstLaunchTutorialHazardKnockbackActor = actor;
            firstLaunchTutorialHazardKnockbackStart = actor.Position;
            firstLaunchTutorialHazardKnockbackTarget =
                firstLaunchTutorialHazardWorldPosition +
                new Vector2(-8f, 20f);
            firstLaunchTutorialHazardKnockbackStartedAt = Time.unscaledTime;
            firstLaunchTutorialHazardKnockbackActive = true;
            firstLaunchTutorialHazardKnockbackImpactApplied = false;
            actor.AttackCommitted = false;
            actor.DamageApplied = false;
            PlayFirstLaunchTutorialHeavyAttackAnimation(false);
            ShowFirstLaunchTutorialSuccess("KNOCK IT INTO THE HAZARD");
        }

        private void UpdateFirstLaunchTutorialHazardKnockbackSequence()
        {
            if (!firstLaunchTutorialHazardKnockbackActive)
                return;

            TutorialEnemyActor actor =
                firstLaunchTutorialHazardKnockbackActor;
            if (actor == null)
            {
                firstLaunchTutorialHazardKnockbackActive = false;
                return;
            }

            float elapsed =
                Time.unscaledTime -
                firstLaunchTutorialHazardKnockbackStartedAt;
            float travel = Mathf.Clamp01(elapsed / 0.56f);
            float eased = travel * travel * (3f - 2f * travel);
            Vector2 position = Vector2.Lerp(
                firstLaunchTutorialHazardKnockbackStart,
                firstLaunchTutorialHazardKnockbackTarget,
                eased
            );
            position.y += Mathf.Sin(travel * Mathf.PI) * 58f;
            actor.Position = position;
            if (actor.Image != null)
            {
                actor.Image.gameObject.SetActive(true);
                float spin = Mathf.Lerp(0f, -160f, eased);
                actor.Image.rectTransform.localRotation =
                    Quaternion.Euler(0f, 0f, spin);
            }

            if (firstLaunchTutorialHazard != null)
            {
                float pulse = travel < 1f
                    ? 1f + Mathf.Sin(travel * Mathf.PI) * 0.10f
                    : 1f + Mathf.Sin(elapsed * 24f) * 0.06f;
                firstLaunchTutorialHazard.rectTransform.localScale =
                    new Vector3(pulse, pulse, 1f);
            }

            if (!firstLaunchTutorialHazardKnockbackImpactApplied &&
                travel >= 1f)
            {
                firstLaunchTutorialHazardKnockbackImpactApplied = true;
                TryApplyFirstLaunchTutorialLessonDamageV101125(
                    actor,
                    actor.Health,
                    FirstLaunchTutorialDamageSourceV101125.Hazard
                ); /* BD LESSON DAMAGE OWNERSHIP V10.11.25 */
                ShowFirstLaunchTutorialSuccess("ENVIRONMENT IMPACT");
            }

            if (elapsed < 0.92f)
                return;

            if (firstLaunchTutorialHazard != null)
                firstLaunchTutorialHazard.rectTransform.localScale = Vector3.one;
            firstLaunchTutorialHazardKnockbackActor = null;
            firstLaunchTutorialHazardKnockbackActive = false;
            firstLaunchTutorialHazardKnockbackImpactApplied = false;
            SetFirstLaunchTutorialLearningState(
                "HazardKnockback",
                TutorialLearningState.Demonstrated
            );
            SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.SidePath);
        }

        private void UpdateFirstLaunchTutorialMountedImpact()
        {
            if (firstLaunchTutorialStep != FirstLaunchTutorialStep.MountedImpact ||
                !firstLaunchTutorialMounted ||
                !firstLaunchTutorialMovementActive)
            {
                return;
            }

            // BD STABLE WORLD-OWNED MOUNTED IMPACT TARGET V10.11.30.31
            // The room layout owns one fixed target. Never manufacture or move a
            // replacement relative to the player: that created the visible
            // tether where turning dragged the enemy at a constant distance.
            TutorialEnemyActor actor = null;
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor candidate = firstLaunchTutorialActors[index];
                if (candidate.Image != firstLaunchTutorialEnemy)
                    continue;
                actor = candidate;
                break;
            }

            if (actor == null || !actor.Active || actor.Dead ||
                actor.Role == TutorialEnemyRole.MiniBoss)
            {
                return;
            }

            float requiredContact =
                TutorialMountedCollisionRadius + TutorialEnemyCollisionRadius;
            if (Vector2.Distance(
                    firstLaunchTutorialPlayerWorldPosition,
                    actor.Position) > requiredContact)
            {
                return;
            }

            bool impactApplied =
                TryApplyFirstLaunchTutorialLessonDamageV101125(
                    actor,
                    actor.Health,
                    FirstLaunchTutorialDamageSourceV101125.MountedImpact
                );
            if (!impactApplied || !actor.Dead)
                return;

            SetFirstLaunchTutorialLearningState(
                "MountedImpact",
                TutorialLearningState.Demonstrated
            );
            ShowFirstLaunchTutorialSuccess("MOUNTED IMPACT");
            SetFirstLaunchTutorialStep(
                FirstLaunchTutorialStep.DismountHorse
            );
        }

        private bool ResolveFirstLaunchTutorialFreeSpinAtImpact()
        {
            bool hit = false;
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (!actor.Active || actor.Dead ||
                    actor.Image == null ||
                    !actor.Image.gameObject.activeInHierarchy)
                {
                    continue;
                }
                Vector2 delta =
                    actor.Position - firstLaunchTutorialPlayerWorldPosition;
                if (delta.magnitude > 210f)
                    continue;
                ApplyFirstLaunchTutorialActorDamage(actor, 0.82f);
                if (!actor.Dead &&
                    actor.Role != TutorialEnemyRole.MiniBoss)
                {
                    actor.Position.x += Mathf.Sign(delta.x) * 54f;
                }
                hit = true;
            }
            return hit;
        }

        // BD MOUNTED IMPACT PLAYER-RELATIVE TARGET RETIRED V10.11.30.31
        // The Mounted Impact room layout is the only target owner. A helper that
        // fabricated a replacement at player position + 150 was removed because
        // it coupled enemy world position to rider movement and caused the tether.

        private void ResolveFirstLaunchTutorialProductionMelee(float damage, bool heavy)
        {
            float range = heavy ? 210f : 165f;
            TutorialEnemyActor actor =
                FindClosestLivingTutorialActor(range);
            if (actor == null)
            {
                ShowFirstLaunchTutorialSuccess("NO TARGET IN RANGE");
                return;
            }

            BeginFirstLaunchTutorialMeleeTransaction(
                actor,
                damage,
                range,
                heavy
            );
        }
        private void FireFirstLaunchTutorialProductionShot()
        {
            if (!firstLaunchTutorialMounted &&
                firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.CombinedEncounter &&
                firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.MiniBossPhaseOne &&
                firstLaunchTutorialStep !=
                    FirstLaunchTutorialStep.MiniBossPhaseTwo)
            {
                ShowFirstLaunchTutorialSuccess("MOUNT FIRST");
                return;
            }
            if (Time.unscaledTime < firstLaunchTutorialReloadCompletesAt)
                return;
            if (firstLaunchTutorialAmmo <= 0)
            {
                BeginFirstLaunchTutorialReload();
                return;
            }

            bool bossStep =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseOne ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseTwo;
            TutorialEnemyActor actor = bossStep
                ? FindTutorialBoss()
                : FindClosestLivingTutorialActor(520f);
            if (actor != null && (!actor.Active || actor.Dead))
                actor = null;
            Vector2 target = actor != null
                ? actor.Position
                : firstLaunchTutorialPlayerWorldPosition +
                    ResolveFirstLaunchTutorialActionDirection() * 360f;
            bool advancesMountedShotLesson =
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.RangedAttack;
            firstLaunchTutorialAmmo--;
            BeginFirstLaunchTutorialShotTransaction(
                actor,
                1f,
                charged: false,
                advancesLesson: advancesMountedShotLesson
            );
            PlayFirstLaunchTutorialRangedAttackAnimation(
                target,
                advancesLesson: advancesMountedShotLesson,
                chargedShot: false
            );
            if (firstLaunchTutorialAmmo <= 0)
                BeginFirstLaunchTutorialReload();
        }
        private void BeginFirstLaunchTutorialReload()
        {
            if (Time.unscaledTime < firstLaunchTutorialReloadCompletesAt)
                return;
            firstLaunchTutorialReloadCompletesAt =
                Time.unscaledTime + 1.15f;
            ShowFirstLaunchTutorialSuccess("RELOADING");
        }
        private void UpdateFirstLaunchTutorialReload()
        {
            if (firstLaunchTutorialReloadCompletesAt <= 0f ||
                Time.unscaledTime < firstLaunchTutorialReloadCompletesAt)
            {
                return;
            }

            firstLaunchTutorialReloadCompletesAt = 0f;
            firstLaunchTutorialAmmo = TutorialMagazineSize;
            ShowFirstLaunchTutorialSuccess("READY");

            if (firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.RangedAttack ||
                firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.Reload)
            {
                firstLaunchTutorialMountedShotReloadConfirmedV101124 = true;
            }
            else if (firstLaunchTutorialStep ==
                         FirstLaunchTutorialStep.ChargedShot)
            {
                firstLaunchTutorialChargedShotReloadConfirmedV101124 = true;
            }

            UpdateFirstLaunchTutorialMountedRangedSequenceV101124();
        }

        private TutorialEnemyActor FindClosestLivingTutorialActor(
            float range,
            bool requireForward = true)
        {
            TutorialEnemyActor closest = null;
            float best = range * range;
            Vector2 forward = ResolveFirstLaunchTutorialActionDirection();
            for (int i = 0; i < firstLaunchTutorialActors.Count; i++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[i];
                if (!actor.Active || actor.Dead || actor.Image == null ||
                    !actor.Image.gameObject.activeInHierarchy)
                {
                    continue;
                }
                Vector2 delta =
                    actor.Position - firstLaunchTutorialPlayerWorldPosition;
                if (requireForward &&
                    Vector2.Dot(delta.normalized, forward) < 0.35f)
                {
                    continue;
                }
                float distance = delta.sqrMagnitude;
                if (distance >= best)
                    continue;
                best = distance;
                closest = actor;
            }
            return closest;
        }

        private TutorialEnemyActor PrepareFirstLaunchTutorialPrimaryMeleeTarget(
            float requiredHealth)
        {
            for (int index = 0; index < firstLaunchTutorialActors.Count; index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (actor.Image != firstLaunchTutorialEnemy)
                    continue;

                actor.Role = TutorialEnemyRole.Small;
                actor.Position = firstLaunchTutorialEnemyWorldPosition;
                actor.SpawnPosition = actor.Position;
                actor.MaximumHealth = Mathf.Max(1f, requiredHealth);
                actor.Health = actor.MaximumHealth;
                actor.Active = true;
                actor.Dead = false;
                actor.AttackCommitted = false;
                actor.DamageApplied = false;
                actor.UsesProjectileAttack = false;
                actor.NextActionAt = float.PositiveInfinity;
                if (actor.Image != null)
                    actor.Image.gameObject.SetActive(true);
                return actor;
            }
            return null;
        }

        private bool HasFirstLaunchTutorialAtomicSpinPairInRange()
        {
            TutorialEnemyActor left;
            TutorialEnemyActor right;
            return TryGetFirstLaunchTutorialAtomicSpinPair(out left, out right);
        }

        private bool ResolveFirstLaunchTutorialAtomicSpinPairAtImpact()
        {
            TutorialEnemyActor left;
            TutorialEnemyActor right;
            if (!TryGetFirstLaunchTutorialAtomicSpinPair(out left, out right))
                return false;

            ApplyFirstLaunchTutorialActorDamage(left, left.Health);
            ApplyFirstLaunchTutorialActorDamage(right, right.Health);
            return left.Dead && right.Dead;
        }

        private bool TryGetFirstLaunchTutorialAtomicSpinPair(
            out TutorialEnemyActor left,
            out TutorialEnemyActor right)
        {
            left = null;
            right = null;
            const float spinRadius = 210f;
            for (int index = 0; index < firstLaunchTutorialActors.Count; index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (!actor.Active || actor.Dead ||
                    actor.Role == TutorialEnemyRole.MiniBoss ||
                    actor.Image == null ||
                    !actor.Image.gameObject.activeInHierarchy)
                {
                    continue;
                }

                Vector2 delta =
                    actor.Position - firstLaunchTutorialPlayerWorldPosition;
                if (delta.magnitude > spinRadius || Mathf.Abs(delta.x) < 12f)
                    continue;
                if (delta.x < 0f)
                {
                    if (left == null ||
                        delta.sqrMagnitude <
                        (left.Position - firstLaunchTutorialPlayerWorldPosition)
                            .sqrMagnitude)
                    {
                        left = actor;
                    }
                }
                else
                {
                    if (right == null ||
                        delta.sqrMagnitude <
                        (right.Position - firstLaunchTutorialPlayerWorldPosition)
                            .sqrMagnitude)
                    {
                        right = actor;
                    }
                }
            }
            return left != null && right != null;
        }

        private bool IsFirstLaunchTutorialTargetInFront(
            Vector2 target,
            float range)
        {
            Vector2 delta =
                target - firstLaunchTutorialPlayerWorldPosition;
            return delta.magnitude <= range &&
                   Vector2.Dot(
                       delta.normalized,
                       ResolveFirstLaunchTutorialActionDirection()
                   ) >= 0.35f;
        }

        private void ApplyFirstLaunchTutorialActorDamage(
            TutorialEnemyActor actor,
            float damage)
        {
            // BD LESSON DAMAGE OWNERSHIP V10.11.25
            // Inputs and animations stay available. Only the required lesson
            // mechanic may reduce the active lesson target's health.
            if (!IsFirstLaunchTutorialLessonDamageAllowedV101125(
                    actor,
                    firstLaunchTutorialDamageSourceV101125))
            {
                NotifyFirstLaunchTutorialWrongDamageSourceV101125();
                return;
            }

            if (actor == null || actor.Dead || damage <= 0f)
                return;
            if (actor.Role == TutorialEnemyRole.MiniBoss)
            {
                if (Time.unscaledTime -
                        firstLaunchTutorialMiniBossPhaseStartedAt < 0.55f)
                {
                    return;
                }
                bool ordinaryProjectileImpact =
                    firstLaunchTutorialPendingShotTarget == actor &&
                    firstLaunchTutorialPendingShotImpactResolved &&
                    !firstLaunchTutorialPendingShotCharged;
                if (actor.AttackCommitted && !ordinaryProjectileImpact)
                {
                    ShowFirstLaunchTutorialSuccess(
                        "WAIT FOR RECOVERY — ATTACK NOW"
                    );
                    return;
                }
            }
            actor.Health = Mathf.Max(0f, actor.Health - damage);
            if (actor.Health > 0f)
                return;
            actor.Dead = true;
            actor.Active = false;
            if (actor.Image != null)
                actor.Image.gameObject.SetActive(false);
        }
        private void UpdateFirstLaunchTutorialEnemyTransactions()
        {
            float now = Time.unscaledTime;
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor =
                    firstLaunchTutorialActors[index];
                if (!actor.Active || actor.Dead)
                    continue;

                bool usesProjectile =
                    actor.Role == TutorialEnemyRole.Ranged ||
                    (actor.Role == TutorialEnemyRole.MiniBoss &&
                     actor.UsesProjectileAttack);

                if (actor.AttackCommitted)
                {
                    float attackElapsed =
                        now - actor.ActionStartedAt;
                    float impactTime = usesProjectile ? 0.56f : 0.62f;
                    if (!actor.DamageApplied &&
                        attackElapsed >= impactTime)
                    {
                        actor.DamageApplied = true;
                        if (actor.Role == TutorialEnemyRole.MiniBoss)
                        {
                            ShowFirstLaunchTutorialSuccess(
                                actor.UsesProjectileAttack
                                    ? "THREE-SHOT FAN RELEASE — MOVE"
                                    : ResolveFirstLaunchTutorialBossPhysicalImpactLabel()
                            );
                        }
                        if (usesProjectile)
                        {
                            if (actor.Role == TutorialEnemyRole.MiniBoss)
                                LaunchFirstLaunchTutorialBossFan(actor);
                            else
                                LaunchFirstLaunchTutorialEnemyProjectile(actor);
                        }
                        else
                        {
                            float range = actor.Role ==
                                    TutorialEnemyRole.MiniBoss
                                ? ResolveFirstLaunchTutorialBossPhysicalRange()
                                : actor.Role == TutorialEnemyRole.Small
                                    ? 94f
                                    : 122f;
                            bool physicalHit = actor.Role ==
                                    TutorialEnemyRole.MiniBoss
                                ? IsFirstLaunchTutorialBossPhysicalHit(actor)
                                : Vector2.Distance(
                                      actor.Position,
                                      firstLaunchTutorialPlayerWorldPosition) <=
                                  range;
                            if (physicalHit &&
                                now >= firstLaunchTutorialInvulnerableUntil)
                            {
                                DamageFirstLaunchTutorialPlayer(1);
                            }
                        }
                    }

                    if (attackElapsed >= 0.94f)
                    {
                        actor.AttackCommitted = false;
                        actor.DamageApplied = false;
                        if (actor.Role == TutorialEnemyRole.MiniBoss)
                            ShowFirstLaunchTutorialSuccess("RECOVERY — ATTACK NOW");
                        actor.NextActionAt = now +
                            (actor.Role == TutorialEnemyRole.MiniBoss
                                ? 1.45f
                                : 1.20f);
                    }
                    continue;
                }

                if (now < actor.NextActionAt ||
                    now < firstLaunchTutorialNextEnemyAttackAt ||
                    Vector2.Distance(
                        actor.Position,
                        firstLaunchTutorialPlayerWorldPosition) > 500f)
                {
                    continue;
                }

                if (actor.Role == TutorialEnemyRole.MiniBoss)
                {
                    actor.AttackSequence++;
                    float bossDistance = Vector2.Distance(
                        actor.Position,
                        firstLaunchTutorialPlayerWorldPosition
                    );
                    actor.UsesProjectileAttack = bossDistance > 280f;
                    ConfigureFirstLaunchTutorialBossPhysicalAttack(
                        actor,
                        bossDistance
                    );
                }
                else
                {
                    actor.UsesProjectileAttack =
                        actor.Role == TutorialEnemyRole.Ranged;
                }

                actor.ActionStartedAt = now;
                actor.AttackCommitted = true;
                actor.DamageApplied = false;
                if (actor.Role == TutorialEnemyRole.MiniBoss)
                {
                    ShowFirstLaunchTutorialSuccess(
                        actor.UsesProjectileAttack
                            ? "THREE-SHOT FAN — MOVE BETWEEN LANES"
                            : ResolveFirstLaunchTutorialBossPhysicalWindupLabel()
                    );
                }
                firstLaunchTutorialNextEnemyAttackAt = now + 0.90f;
                break;
            }
        }

        private void DamageFirstLaunchTutorialPlayer(int amount)
        {
            if (amount <= 0 || Time.unscaledTime < firstLaunchTutorialInvulnerableUntil)
                return;
            firstLaunchTutorialPlayerHealth = Mathf.Max(0, firstLaunchTutorialPlayerHealth - amount);
            BeginFirstLaunchTutorialPlayerHitReaction();
            firstLaunchTutorialInvulnerableUntil = Time.unscaledTime + 0.75f;
            ShowFirstLaunchTutorialSuccess("HIT");
            if (firstLaunchTutorialPlayerHealth > 0)
                return;

            firstLaunchTutorialPlayerDeathActive = true;
            firstLaunchTutorialRespawnResetApplied = false;
            firstLaunchTutorialPlayerDeathStartedAt = Time.unscaledTime;
            BeginFirstLaunchTutorialRespawnPresentation();
            firstLaunchTutorialMovementActive = false;
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;
            firstLaunchTutorialReloadCompletesAt = 0f;
            CancelFirstLaunchTutorialEnemyProjectile();
            ShowFirstLaunchTutorialSuccess("TRY AGAIN");
        }

        private bool UpdateFirstLaunchTutorialPlayerDeath()
        {
            if (!firstLaunchTutorialPlayerDeathActive)
                return false;

            const float poseDuration = 0.48f;
            const float coverStartsAt = 0.20f;
            const float coverOpaqueAt = 0.68f;
            const float revealEndsAt = 1.18f;

            float elapsed =
                Time.unscaledTime - firstLaunchTutorialPlayerDeathStartedAt;
            float poseProgress = Mathf.Clamp01(elapsed / poseDuration);
            float coverProgress = Mathf.Clamp01(
                (elapsed - coverStartsAt) /
                Mathf.Max(0.01f, coverOpaqueAt - coverStartsAt)
            );

            if (firstLaunchTutorialPlayer != null)
            {
                firstLaunchTutorialPlayer.rectTransform.localRotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        Mathf.Lerp(0f, -78f, poseProgress)
                    );
                firstLaunchTutorialPlayer.rectTransform.localScale =
                    new Vector3(
                        1f,
                        Mathf.Lerp(1f, 0.70f, poseProgress),
                        1f
                    );

                Color playerColor = firstLaunchTutorialPlayer.color;
                playerColor.a = 1f - coverProgress;
                firstLaunchTutorialPlayer.color = playerColor;
            }

            SetFirstLaunchTutorialRespawnOverlayAlpha(coverProgress);

            if (!firstLaunchTutorialRespawnResetApplied &&
                elapsed >= coverOpaqueAt)
            {
                firstLaunchTutorialRespawnResetApplied = true;
                ResetFirstLaunchTutorialToCheckpoint();
            }

            if (elapsed < coverOpaqueAt)
                return true;

            float revealProgress = Mathf.Clamp01(
                (elapsed - coverOpaqueAt) /
                Mathf.Max(0.01f, revealEndsAt - coverOpaqueAt)
            );
            SetFirstLaunchTutorialRespawnOverlayAlpha(1f - revealProgress);

            if (firstLaunchTutorialPlayer != null)
            {
                Color playerColor = firstLaunchTutorialPlayer.color;
                playerColor.a = revealProgress;
                firstLaunchTutorialPlayer.color = playerColor;
            }

            if (elapsed < revealEndsAt)
                return true;

            firstLaunchTutorialPlayerDeathActive = false;
            firstLaunchTutorialRespawnResetApplied = false;
            ResetFirstLaunchTutorialRespawnPresentation();
            return true;
        }

        private void BeginFirstLaunchTutorialRespawnPresentation()
        {
            if (firstLaunchTutorialRespawnOverlay != null)
            {
                firstLaunchTutorialRespawnOverlay.gameObject.SetActive(true);
                firstLaunchTutorialRespawnOverlay.transform.SetAsLastSibling();
            }
            SetFirstLaunchTutorialRespawnOverlayAlpha(0f);
        }

        private void SetFirstLaunchTutorialRespawnOverlayAlpha(float alpha)
        {
            float value = Mathf.Clamp01(alpha);
            if (firstLaunchTutorialRespawnOverlay != null)
            {
                Color overlayColor = firstLaunchTutorialRespawnOverlay.color;
                overlayColor.a = value;
                firstLaunchTutorialRespawnOverlay.color = overlayColor;
            }

            if (firstLaunchTutorialRespawnLabel != null)
            {
                Color labelColor = firstLaunchTutorialRespawnLabel.color;
                labelColor.a = Mathf.SmoothStep(0f, 1f, value);
                firstLaunchTutorialRespawnLabel.color = labelColor;
            }
        }

        private void ResetFirstLaunchTutorialRespawnPresentation()
        {
            SetFirstLaunchTutorialRespawnOverlayAlpha(0f);
            if (firstLaunchTutorialRespawnOverlay != null)
                firstLaunchTutorialRespawnOverlay.gameObject.SetActive(false);

            if (firstLaunchTutorialPlayer != null)
            {
                Color playerColor = firstLaunchTutorialPlayer.color;
                playerColor.a = 1f;
                firstLaunchTutorialPlayer.color = playerColor;
                firstLaunchTutorialPlayer.rectTransform.localRotation =
                    Quaternion.identity;
                firstLaunchTutorialPlayer.rectTransform.localScale =
                    Vector3.one;
            }
        }

        private void ResetFirstLaunchTutorialToCheckpoint()
        {
            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseOne ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseTwo ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossDefeated)
            {
                firstLaunchTutorialMiniBossPhaseTwo = false;
                firstLaunchTutorialMiniBossDeathStarted = false;
                firstLaunchTutorialStep = FirstLaunchTutorialStep.MiniBossPhaseOne;
                firstLaunchTutorialStepStartedAt = Time.unscaledTime;
            }

            firstLaunchTutorialPlayerHealth = 6;
            firstLaunchTutorialAmmo = TutorialMagazineSize;
            firstLaunchTutorialReloadCompletesAt = 0f;
            firstLaunchTutorialMounted = false;
            firstLaunchTutorialMountedCurrentSpeed = 0f;
            firstLaunchTutorialGrounded = true;
            firstLaunchTutorialVerticalVelocity = 0f;
            firstLaunchTutorialWallJumpConsumed = false;
            firstLaunchTutorialPlayerWorldPosition = new Vector2(firstLaunchTutorialCheckpointX, TutorialGroundY);
            if (firstLaunchTutorialPlayer != null)
            {
                firstLaunchTutorialPlayer.rectTransform.localRotation =
                    Quaternion.identity;
                firstLaunchTutorialPlayer.rectTransform.localScale = Vector3.one;
            }
            firstLaunchTutorialPrimaryHoldStartedAt = -1f;
            firstLaunchTutorialGrappleHoldStartedAt = -1f;
            firstLaunchTutorialHealHoldStartedAt = -1f;
            firstLaunchTutorialChargedShotPendingStartedAt = -1f;
            firstLaunchTutorialChargedShotStartedAt = -1f;
            ResetFirstLaunchTutorialV108Transactions();
            ConfigureFirstLaunchTutorialProductionStep(firstLaunchTutorialStep);
            ShowFirstLaunchTutorialSuccess("CHECKPOINT RESTORED");
        }

        private void UpdateFirstLaunchTutorialSecretRoute()
        {
            if (firstLaunchTutorialSecretCollected)
            {
                if (firstLaunchTutorialPlayerWorldPosition.x >=
                        firstLaunchTutorialLessonScreenCenterX + 210f)
                    SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.CombinedEncounter);
                return;
            }
            if (firstLaunchTutorialPlayerWorldPosition.x >=
                    firstLaunchTutorialLessonScreenCenterX + 260f)
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.CombinedEncounter);
        }

        private void TryCollectFirstLaunchTutorialSecret()
        {
            if (firstLaunchTutorialSecretCollected || firstLaunchTutorialSecret == null)
                return;
            Vector2 secretPosition = new Vector2(
                firstLaunchTutorialLessonScreenCenterX + 150f,
                TutorialGroundY + 82f
            );
            if (Vector2.Distance(firstLaunchTutorialPlayerWorldPosition, secretPosition) > 110f)
                return;
            firstLaunchTutorialSecretCollected = true;
            firstLaunchTutorialSecret.gameObject.SetActive(false);
            ShowFirstLaunchTutorialSuccess("SECRET FOUND");
        }

        private void UpdateFirstLaunchTutorialCombinedEncounter()
        {
            if (!firstLaunchTutorialCombinedStarted)
            {
                firstLaunchTutorialCombinedStarted = true;
                firstLaunchTutorialCheckpointX =
                    firstLaunchTutorialLessonScreenCenterX - 190f;
            }
            if (CountLivingTutorialActors(TutorialEnemyRole.MiniBoss) == 0 &&
                CountLivingNonBossTutorialActors() == 0)
            {
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.WallJump);
            }
        }

        private void UpdateFirstLaunchTutorialMiniBoss()
        {
            TutorialEnemyActor boss = FindTutorialBoss();
            if (boss == null || boss.Dead)
            {
                if (!firstLaunchTutorialMiniBossDeathStarted)
                    SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.MiniBossDefeated);
                return;
            }

            if (!firstLaunchTutorialMiniBossPhaseTwo && boss.Health <= boss.MaximumHealth * 0.5f)
            {
                float remainingHealth = boss.Health;
                firstLaunchTutorialMiniBossPhaseTwo = true;
                firstLaunchTutorialMiniBossPhaseStartedAt = Time.unscaledTime;
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.MiniBossPhaseTwo);
                TutorialEnemyActor phaseBoss = FindTutorialBoss();
                if (phaseBoss != null)
                    phaseBoss.Health = remainingHealth;
                SpawnTutorialActor(
                    firstLaunchTutorialEnemySecondary,
                    TutorialEnemyRole.Small,
                    firstLaunchTutorialLessonScreenCenterX - 120f,
                    2f
                );
                SpawnTutorialActor(
                    firstLaunchTutorialEnemyTertiary,
                    TutorialEnemyRole.Small,
                    firstLaunchTutorialLessonScreenCenterX + 120f,
                    2f
                );
            }
        }
        private void UpdateFirstLaunchTutorialMiniBossDeath(float elapsed)
        {
            firstLaunchTutorialMiniBossDeathStarted = true;
            if (firstLaunchTutorialProjectile != null)
                firstLaunchTutorialProjectile.gameObject.SetActive(false);
            if (firstLaunchTutorialMiniBoss != null)
            {
                float t = Mathf.Clamp01(elapsed / 0.9f);
                firstLaunchTutorialMiniBoss.rectTransform.localScale =
                    new Vector3(1f + t * 0.12f, 1f - t * 0.82f, 1f);
                if (t >= 1f)
                    firstLaunchTutorialMiniBoss.gameObject.SetActive(false);
            }
            if (elapsed >= 1.05f && !firstLaunchTutorialFinishGateOpen)
            {
                firstLaunchTutorialFinishGateOpen = true;
                if (firstLaunchTutorialFinishGate != null)
                    firstLaunchTutorialFinishGate.gameObject.SetActive(true);
                SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.Collectible);
            }
        }

        private int CountLivingNonBossTutorialActors()
        {
            int count = 0;
            for (int i = 0; i < firstLaunchTutorialActors.Count; i++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[i];
                if (actor.Role != TutorialEnemyRole.MiniBoss &&
                    actor.Active &&
                    !actor.Dead &&
                    actor.Image != null &&
                    actor.Image.gameObject.activeInHierarchy)
                {
                    count++;
                }
            }
            return count;
        }

        private int CountLivingTutorialActors(TutorialEnemyRole role)
        {
            int count = 0;
            for (int i = 0; i < firstLaunchTutorialActors.Count; i++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[i];
                if (actor.Role == role &&
                    actor.Active &&
                    !actor.Dead &&
                    actor.Image != null &&
                    actor.Image.gameObject.activeInHierarchy)
                {
                    count++;
                }
            }
            return count;
        }

        private TutorialEnemyActor FindTutorialBoss()
        {
            for (int i = 0; i < firstLaunchTutorialActors.Count; i++)
            {
                if (firstLaunchTutorialActors[i].Role == TutorialEnemyRole.MiniBoss)
                    return firstLaunchTutorialActors[i];
            }
            return null;
        }

        private void SpawnTutorialActor(
            Image image,
            TutorialEnemyRole role,
            float x,
            float health)
        {
            TutorialEnemyActor matched = null;
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                if (firstLaunchTutorialActors[index].Image == image)
                {
                    matched = firstLaunchTutorialActors[index];
                    break;
                }
            }
            if (matched == null)
                return;

            bool preserveVisibleActor =
                matched.Active &&
                !matched.Dead &&
                matched.Image != null &&
                matched.Image.gameObject.activeInHierarchy;
            float spawnX = preserveVisibleActor
                ? matched.Position.x
                : x;
            float visibleLeftEdge = firstLaunchTutorialCameraWorldX -
                TutorialWorldViewportHalfWidth - 72f;
            float visibleRightEdge = firstLaunchTutorialCameraWorldX +
                TutorialWorldViewportHalfWidth + 72f;
            if (!preserveVisibleActor &&
                spawnX > visibleLeftEdge &&
                spawnX < visibleRightEdge)
            {
                // New actors are prepared beyond the next screen edge. Existing
                // visible actors keep their current world position across phase
                // changes and are never teleported or respawned in view.
                spawnX = visibleRightEdge + 96f;
            }

            matched.Role = role;
            matched.Position = new Vector2(spawnX, TutorialGroundY);
            matched.SpawnPosition = matched.Position;
            matched.MaximumHealth = health;
            matched.Health = health;
            matched.Active = true;
            matched.Dead = false;
            matched.AttackCommitted = false;
            matched.DamageApplied = false;
            matched.UsesProjectileAttack = role == TutorialEnemyRole.Ranged;
            matched.AttackSequence = preserveVisibleActor
                ? matched.AttackSequence
                : 0;
            matched.NextActionAt = Time.unscaledTime + 0.80f;
            if (matched.Image == firstLaunchTutorialEnemy)
                firstLaunchTutorialEnemyWorldPosition = matched.Position;
            if (matched.Image != null)
                matched.Image.gameObject.SetActive(true);
        }

        private void ConfigureFirstLaunchTutorialProductionStep(FirstLaunchTutorialStep step)
        {
            ResetFirstLaunchTutorialBossChargeState();
            ResetFirstLaunchTutorialMountedRangedSequenceForStepV101124(step);
            bool ownsPrimaryActor =
                step == FirstLaunchTutorialStep.JumpAttack ||
                step == FirstLaunchTutorialStep.SpinAttack ||
                step == FirstLaunchTutorialStep.Grapple ||
                step == FirstLaunchTutorialStep.HazardKnockback ||
                step == FirstLaunchTutorialStep.RangedAttack ||
                step == FirstLaunchTutorialStep.Reload ||
                step == FirstLaunchTutorialStep.ChargedShot ||
                step == FirstLaunchTutorialStep.MountedImpact ||
                step == FirstLaunchTutorialStep.SidePath ||
                step == FirstLaunchTutorialStep.CombinedEncounter ||
                step == FirstLaunchTutorialStep.MiniBossIntro ||
                step == FirstLaunchTutorialStep.MiniBossPhaseOne ||
                step == FirstLaunchTutorialStep.MiniBossPhaseTwo ||
                step == FirstLaunchTutorialStep.MiniBossDefeated ||
                step == FirstLaunchTutorialStep.Collectible;

            if (ownsPrimaryActor)
                SetProductionEntitiesVisible(false);
            else
                HideAdditionalFirstLaunchTutorialActors();

            switch (step)
            {
                case FirstLaunchTutorialStep.JumpAttack:
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        firstLaunchTutorialLessonScreenCenterX + 76f,
                        1f
                    );
                    TutorialEnemyActor jumpTarget =
                        FindClosestLivingTutorialActor(1000f, requireForward: false);
                    if (jumpTarget != null)
                    {
                        jumpTarget.Position =
                            new Vector2(
                                firstLaunchTutorialLessonScreenCenterX + 76f,
                                TutorialGroundY
                            );
                        jumpTarget.SpawnPosition = jumpTarget.Position;
                        jumpTarget.NextActionAt = float.PositiveInfinity;
                    }
                    break;
                case FirstLaunchTutorialStep.SpinAttack:
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        firstLaunchTutorialLessonScreenCenterX - 54f,
                        1f
                    );
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemySecondary,
                        TutorialEnemyRole.Small,
                        firstLaunchTutorialLessonScreenCenterX + 54f,
                        1f
                    );
                    break;
                case FirstLaunchTutorialStep.Grapple:
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        firstLaunchTutorialLessonScreenCenterX + 150f,
                        2f
                    );
                    break;
                case FirstLaunchTutorialStep.HazardKnockback:
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        firstLaunchTutorialLessonScreenCenterX - 70f,
                        1f
                    );
                    if (firstLaunchTutorialHazard != null)
                    {
                        firstLaunchTutorialHazardWorldPosition =
                            new Vector2(
                                firstLaunchTutorialLessonScreenCenterX + 38f,
                                TutorialGroundY - 34f
                            );
                        firstLaunchTutorialHazard.gameObject.SetActive(true);
                    }
                    break;
                case FirstLaunchTutorialStep.RangedAttack:
                    firstLaunchTutorialAmmo = 1;
                    firstLaunchTutorialReloadCompletesAt = 0f;
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        firstLaunchTutorialLessonScreenCenterX + 130f,
                        1f
                    );
                    break;
                case FirstLaunchTutorialStep.Reload:
                    break;
                case FirstLaunchTutorialStep.ChargedShot:
                    firstLaunchTutorialAmmo = TutorialMagazineSize;
                    firstLaunchTutorialReloadCompletesAt = 0f;
                    firstLaunchTutorialChargedShotAutoFired = false;
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        firstLaunchTutorialLessonScreenCenterX + 260f,
                        4f
                    );
                    break;
                case FirstLaunchTutorialStep.MountedImpact:
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Small,
                        firstLaunchTutorialLessonScreenCenterX + 210f,
                        1f
                    );
                    break;
                case FirstLaunchTutorialStep.SidePath:
                    if (firstLaunchTutorialSecret != null)
                        firstLaunchTutorialSecret.gameObject.SetActive(!firstLaunchTutorialSecretCollected);
                    break;
                case FirstLaunchTutorialStep.CombinedEncounter:
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemy,
                        TutorialEnemyRole.Sword,
                        firstLaunchTutorialLessonScreenCenterX + 20f,
                        2f
                    );
                    SpawnTutorialActor(
                        firstLaunchTutorialRangedEnemy,
                        TutorialEnemyRole.Ranged,
                        firstLaunchTutorialLessonScreenCenterX + 260f,
                        2f
                    );
                    SpawnTutorialActor(
                        firstLaunchTutorialEnemySecondary,
                        TutorialEnemyRole.Small,
                        firstLaunchTutorialLessonScreenCenterX + 140f,
                        1f
                    );
                    break;
                case FirstLaunchTutorialStep.MiniBossIntro:
                    firstLaunchTutorialAmmo = TutorialMagazineSize;
                    firstLaunchTutorialReloadCompletesAt = 0f;
                    SpawnTutorialActor(
                        firstLaunchTutorialMiniBoss,
                        TutorialEnemyRole.MiniBoss,
                        firstLaunchTutorialLessonScreenCenterX + 80f,
                        12f
                    );
                    break;
                case FirstLaunchTutorialStep.MiniBossPhaseOne:
                case FirstLaunchTutorialStep.MiniBossPhaseTwo:
                    firstLaunchTutorialAmmo = Mathf.Max(
                        firstLaunchTutorialAmmo,
                        TutorialBossChargedShotAmmoCost
                    );
                    SpawnTutorialActor(
                        firstLaunchTutorialMiniBoss,
                        TutorialEnemyRole.MiniBoss,
                        firstLaunchTutorialLessonScreenCenterX + 80f,
                        12f
                    );
                    break;
                case FirstLaunchTutorialStep.MiniBossDefeated:
                    if (firstLaunchTutorialMiniBoss != null)
                        firstLaunchTutorialMiniBoss.gameObject.SetActive(true);
                    break;
                case FirstLaunchTutorialStep.Collectible:
                    if (firstLaunchTutorialFinishGate != null)
                        firstLaunchTutorialFinishGate.gameObject.SetActive(true);
                    break;
            }
                    // BD FOCUSED LESSON ACTOR INITIAL SYNC V10.11.30
            UpdateFirstLaunchTutorialFocusedLessonTargetV101130();
}

        private void HideAdditionalFirstLaunchTutorialActors()
        {
            for (int i = 0; i < firstLaunchTutorialActors.Count; i++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[i];
                if (actor.Image == firstLaunchTutorialEnemy)
                    continue;
                actor.AttackCommitted = false;
                bool outsideView = Mathf.Abs(
                    actor.Position.x - firstLaunchTutorialCameraWorldX
                ) > TutorialWorldViewportHalfWidth + 96f;
                if (actor.Dead || outsideView)
                {
                    actor.Active = false;
                    actor.Dead = true;
                    if (actor.Image != null)
                        actor.Image.gameObject.SetActive(false);
                }
            }
            if (firstLaunchTutorialSecret != null)
                firstLaunchTutorialSecret.gameObject.SetActive(false);
            if (firstLaunchTutorialFinishGate != null)
                firstLaunchTutorialFinishGate.gameObject.SetActive(true);
            EnsureFirstLaunchTutorialPersistentCourseGeometryVisible();
        }

        private void SetProductionEntitiesVisible(bool value)
        {
            for (int i = 0; i < firstLaunchTutorialActors.Count; i++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[i];
                bool outsideView = Mathf.Abs(
                    actor.Position.x - firstLaunchTutorialCameraWorldX
                ) > TutorialWorldViewportHalfWidth + 96f;
                if (value || actor.Dead || outsideView)
                {
                    actor.Active = value;
                    actor.Dead = !value;
                    if (actor.Image != null)
                        actor.Image.gameObject.SetActive(value);
                }
            }
            if (firstLaunchTutorialSecret != null)
                firstLaunchTutorialSecret.gameObject.SetActive(false);
            if (firstLaunchTutorialFinishGate != null)
                firstLaunchTutorialFinishGate.gameObject.SetActive(true);
            EnsureFirstLaunchTutorialPersistentCourseGeometryVisible();
        }

        private void RenderFirstLaunchTutorialProductionCourse()
        {
            for (int i = 0; i < firstLaunchTutorialActors.Count; i++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[i];
                if (actor.Image == null || !actor.Active || actor.Dead ||
                    !actor.Image.gameObject.activeInHierarchy)
                {
                    continue;
                }
                actor.Image.rectTransform.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(actor.Position);
                if (actor.AttackCommitted)
                {
                    float pulse = 1f + 0.08f * Mathf.Sin(
                        (Time.unscaledTime - actor.ActionStartedAt) * 18f
                    );
                    actor.Image.rectTransform.localScale =
                        new Vector3(pulse, 1f / Mathf.Max(0.92f, pulse), 1f);
                }
                else
                {
                    actor.Image.rectTransform.localScale = Vector3.one;
                }
                if (actor.Image == firstLaunchTutorialEnemy)
                    firstLaunchTutorialEnemyWorldPosition = actor.Position;
            }
            RenderFirstLaunchTutorialBossAttack();
        }

        private void RenderFirstLaunchTutorialBossAttack()
        {
            TutorialEnemyActor boss = FindTutorialBoss();
            bool active =
                boss != null &&
                boss.Active &&
                !boss.Dead &&
                boss.AttackCommitted &&
                (firstLaunchTutorialStep ==
                     FirstLaunchTutorialStep.MiniBossPhaseOne ||
                 firstLaunchTutorialStep ==
                     FirstLaunchTutorialStep.MiniBossPhaseTwo);
            if (!active)
            {
                SetTutorialEntityActive(
                    firstLaunchTutorialBossAttackTelegraph,
                    false
                );
                SetTutorialEntityActive(
                    firstLaunchTutorialBossImpactFlash,
                    false
                );
                return;
            }

            float elapsed = Time.unscaledTime - boss.ActionStartedAt;
            bool projectileAttack = boss.UsesProjectileAttack;
            float impactTime = projectileAttack ? 0.56f : 0.62f;
            Vector2 direction =
                firstLaunchTutorialPlayerWorldPosition - boss.Position;
            if (direction.sqrMagnitude < 0.001f)
                direction = Vector2.left;
            direction.Normalize();

            Vector2 playerTarget =
                firstLaunchTutorialPlayerWorldPosition +
                new Vector2(0f, 18f);
            if (elapsed < impactTime)
            {
                float windup = Mathf.Clamp01(elapsed / impactTime);
                float eased = windup * windup * (3f - 2f * windup);
                float pulse =
                    0.5f + 0.5f * Mathf.Sin(elapsed * 24f);

                SetTutorialEntityActive(
                    firstLaunchTutorialBossAttackTelegraph,
                    true
                );
                SetTutorialEntityActive(
                    firstLaunchTutorialBossImpactFlash,
                    false
                );

                RectTransform telegraph =
                    firstLaunchTutorialBossAttackTelegraph.rectTransform;
                if (projectileAttack)
                {
                    // A compact charge orb replaces the old full-length beam.
                    // The released projectile is rendered by the projectile
                    // transaction only after this readable windup completes.
                    Vector2 chargePosition =
                        boss.Position +
                        new Vector2(direction.x * 46f, 34f);
                    telegraph.anchoredPosition =
                        SnapFirstLaunchTutorialWorldPosition(chargePosition);
                    float chargeSize = Mathf.Lerp(22f, 62f, eased);
                    telegraph.sizeDelta =
                        new Vector2(chargeSize, chargeSize);
                    telegraph.localRotation = Quaternion.Euler(
                        0f,
                        0f,
                        45f + elapsed * 92f
                    );
                    telegraph.localScale = Vector3.one;
                    firstLaunchTutorialBossAttackTelegraph.color =
                        Color.Lerp(
                            new Color(0.16f, 0.72f, 1f, 0.72f),
                            new Color(0.82f, 0.28f, 1f, 0.98f),
                            Mathf.Clamp01(eased * 0.72f + pulse * 0.28f)
                        );
                    boss.Image.rectTransform.localScale =
                        new Vector3(
                            0.92f - eased * 0.08f,
                            1.08f + eased * 0.10f,
                            1f
                        );
                    boss.Image.rectTransform.localRotation =
                        Quaternion.Euler(0f, 0f, -direction.x * 7f);
                }
                else
                {
                    // The slam now warns on the floor where it will land,
                    // rather than drawing a generic line through the scene.
                    Vector2 warningPosition =
                        firstLaunchTutorialPlayerWorldPosition +
                        new Vector2(0f, -42f);
                    telegraph.anchoredPosition =
                        SnapFirstLaunchTutorialWorldPosition(warningPosition);
                    telegraph.sizeDelta = new Vector2(
                        Mathf.Lerp(96f, 196f, eased),
                        Mathf.Lerp(10f, 24f, pulse)
                    );
                    telegraph.localRotation = Quaternion.identity;
                    telegraph.localScale = Vector3.one;
                    firstLaunchTutorialBossAttackTelegraph.color =
                        Color.Lerp(
                            new Color(1f, 0.68f, 0.12f, 0.62f),
                            new Color(1f, 0.10f, 0.06f, 0.96f),
                            Mathf.Clamp01(eased * 0.74f + pulse * 0.26f)
                        );
                    boss.Image.rectTransform.localScale =
                        new Vector3(
                            1.08f + eased * 0.20f,
                            0.94f - eased * 0.22f,
                            1f
                        );
                    boss.Image.rectTransform.localRotation =
                        Quaternion.Euler(0f, 0f, -direction.x * 12f * eased);
                }
                return;
            }

            SetTutorialEntityActive(
                firstLaunchTutorialBossAttackTelegraph,
                false
            );
            bool impactVisible = elapsed < impactTime + 0.24f;
            SetTutorialEntityActive(
                firstLaunchTutorialBossImpactFlash,
                impactVisible
            );
            if (!impactVisible)
            {
                boss.Image.rectTransform.localScale = Vector3.one;
                boss.Image.rectTransform.localRotation = Quaternion.identity;
                return;
            }

            RectTransform impact =
                firstLaunchTutorialBossImpactFlash.rectTransform;
            Vector2 endpoint = projectileAttack
                ? playerTarget
                : firstLaunchTutorialPlayerWorldPosition +
                    new Vector2(0f, -12f);
            impact.anchoredPosition =
                SnapFirstLaunchTutorialWorldPosition(endpoint);
            float impactProgress = Mathf.Clamp01(
                (elapsed - impactTime) / 0.24f
            );
            float impactPulse = Mathf.Sin(impactProgress * Mathf.PI);
            impact.sizeDelta = projectileAttack
                ? new Vector2(54f, 54f)
                : new Vector2(138f, 92f);
            impact.localRotation = projectileAttack
                ? Quaternion.Euler(0f, 0f, 45f + impactProgress * 90f)
                : Quaternion.identity;
            impact.localScale = Vector3.one * (1f + impactPulse * 0.48f);
            firstLaunchTutorialBossImpactFlash.color = projectileAttack
                ? new Color(0.40f, 0.86f, 1f, 0.90f)
                : new Color(1f, 0.16f, 0.08f, 0.86f);
            boss.Image.rectTransform.localScale = projectileAttack
                ? new Vector3(1.12f, 0.90f, 1f)
                : new Vector3(0.82f, 1.24f, 1f);
            boss.Image.rectTransform.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    direction.x * (projectileAttack ? -8f : 12f)
                );
        }
        private void UpdateFirstLaunchTutorialProductionHud()
        {
            if (firstLaunchTutorialHealthText != null &&
                firstLaunchTutorialDisplayedHealth != firstLaunchTutorialPlayerHealth)
            {
                firstLaunchTutorialDisplayedHealth = firstLaunchTutorialPlayerHealth;
                firstLaunchTutorialHealthText.text =
                    "HEARTS " + firstLaunchTutorialPlayerHealth + " / 6";
            }

            bool reloading = Time.unscaledTime < firstLaunchTutorialReloadCompletesAt;
            if (firstLaunchTutorialAmmoText != null &&
                (firstLaunchTutorialDisplayedAmmo != firstLaunchTutorialAmmo ||
                 firstLaunchTutorialDisplayedReloading != reloading))
            {
                firstLaunchTutorialDisplayedAmmo = firstLaunchTutorialAmmo;
                firstLaunchTutorialDisplayedReloading = reloading;
                firstLaunchTutorialAmmoText.text = reloading
                    ? "RELOADING"
                    : "AMMO " + firstLaunchTutorialAmmo + " / " + TutorialMagazineSize;
            }

            if (firstLaunchTutorialBossHealthText == null)
                return;

            TutorialEnemyActor boss = FindTutorialBoss();
            bool visible = boss != null && boss.Active && !boss.Dead &&
                (firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossIntro ||
                 firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseOne ||
                 firstLaunchTutorialStep == FirstLaunchTutorialStep.MiniBossPhaseTwo);
            if (firstLaunchTutorialDisplayedBossVisible != visible)
            {
                firstLaunchTutorialDisplayedBossVisible = visible;
                firstLaunchTutorialBossHealthText.gameObject.SetActive(visible);
            }
            if (!visible)
                return;

            int displayedBossHealth = Mathf.CeilToInt(boss.Health);
            string bossState = firstLaunchTutorialStep ==
                    FirstLaunchTutorialStep.MiniBossIntro
                ? "PRESS INTERACT WHEN READY"
                : ResolveFirstLaunchTutorialBossState(boss);
            if (firstLaunchTutorialDisplayedBossHealth ==
                    displayedBossHealth &&
                firstLaunchTutorialDisplayedBossState == bossState)
            {
                return;
            }

            firstLaunchTutorialDisplayedBossHealth =
                displayedBossHealth;
            firstLaunchTutorialDisplayedBossState = bossState;
            firstLaunchTutorialBossHealthText.text =
                "FINAL TEST  " + displayedBossHealth +
                " / " + Mathf.CeilToInt(boss.MaximumHealth) +
                "\n" + bossState;
        }
    }
}
