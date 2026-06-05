using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    [RequireComponent(typeof(BDCombatantProfile))]
    public sealed class BDSquareJumperMiniBoss : MonoBehaviour
    {
        private enum ActionState
        {
            Idle,
            Jumping,
            BulletHell,
            SwordAttack,
            Summoning,
            Dead
        }

        [Header("Encounter")]
        [SerializeField] private BDBossEncounterController encounter;
        [SerializeField] private BDBossSharedSummonBudget summonBudget;
        [SerializeField] private Transform target;

        [Header("Visual")]
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Color bodyColor =
            new Color(0.28f, 0.12f, 0.08f, 1f);
        [SerializeField] private Color enragedColor =
            new Color(0.78f, 0.10f, 0.06f, 1f);

        [Header("Ground Movement")]
        [SerializeField] private float aggroRange = 24f;
        [SerializeField] private float moveSpeed = 1.65f;
        [SerializeField] private float rotationSpeed = 8f;
        [SerializeField] private float preferredDistance = 5.2f;

        [Header("Jump Attack")]
        [SerializeField] private float jumpCooldown = 4.8f;
        [SerializeField] private float jumpWindup = 0.62f;
        [SerializeField] private float jumpRiseDuration = 0.34f;
        [SerializeField] private float jumpFallDuration = 0.20f;
        [SerializeField] private float jumpHeight = 4.8f;
        [SerializeField] private float landingDamage = 30f;
        [SerializeField] private float landingRadius = 2.65f;
        [SerializeField] private float landingBulletDelay = 0.08f;

        [Header("Bullet Hell")]
        [SerializeField] private float bulletHellCooldown = 3.6f;
        [SerializeField] private int normalBulletCount = 18;
        [SerializeField] private int enragedBulletCount = 28;
        [SerializeField] private int normalBulletWaves = 1;
        [SerializeField] private int enragedBulletWaves = 3;
        [SerializeField] private float bulletWaveInterval = 0.20f;
        [SerializeField] private float bulletSpeed = 7.2f;
        [SerializeField] private float bulletDamage = 9f;
        [SerializeField] private float bulletRadius = 0.28f;
        [SerializeField] private float bulletLifetime = 6f;

        [Header("Double Sword Attack")]
        [SerializeField] private float swordCooldown = 5.4f;
        [SerializeField] private float swordWindup = 0.38f;
        [SerializeField] private float swordSlashDuration = 0.18f;
        [SerializeField] private float swordRecovery = 0.32f;
        [SerializeField] private float swordDamage = 24f;
        [SerializeField] private float swordSideOffset = 1.65f;
        [SerializeField] private float swordHitRadius = 1.55f;
        [SerializeField] private float swordForwardOffset = 0.7f;

        [Header("Summoning")]
        [SerializeField] private float summonCooldown = 8.5f;
        [SerializeField] private float summonWindup = 0.62f;
        [SerializeField] private GameObject[] summonPrefabs;
        [SerializeField] private Transform[] summonPoints;
        [SerializeField] private int normalSummonMinimum = 2;
        [SerializeField] private int normalSummonMaximum = 3;
        [SerializeField] private int enragedSummonActionCap = 8;

        [Header("Enraged Phase")]
        [Range(0.05f, 0.75f)]
        [SerializeField] private float enragedHealthThreshold = 0.35f;
        [SerializeField] private float enragedMoveSpeedMultiplier = 1.35f;
        [SerializeField] private float enragedCooldownMultiplier = 0.62f;
        [SerializeField] private float enragedJumpHeightMultiplier = 1.15f;
        [SerializeField] private float enragedBulletSpeedMultiplier = 1.12f;

        [Header("Decision")]
        [SerializeField] private float decisionInterval = 0.18f;

        private CharacterController controller;
        private BDHealth health;
        private BDCombatantProfile combatantProfile;
        private Renderer[] visualRenderers;
        private Transform leftSwordPivot;
        private Transform rightSwordPivot;
        private Quaternion leftSwordRestRotation;
        private Quaternion rightSwordRestRotation;
        private Vector3 leftSwordRestPosition;
        private Vector3 rightSwordRestPosition;

        private ActionState actionState;
        private Vector3 spawnPosition;
        private Quaternion spawnRotation;
        private float nextDecisionAt;
        private float nextJumpAt;
        private float nextBulletHellAt;
        private float nextSwordAt;
        private float nextSummonAt;
        private bool enraged;
        private bool warnedMissingSummons;

        public bool IsEnraged => enraged;
        public bool IsBusy => actionState != ActionState.Idle;
        public bool IsDead => actionState == ActionState.Dead;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            combatantProfile = GetComponent<BDCombatantProfile>();

            spawnPosition = transform.position;
            spawnRotation = transform.rotation;

            ResolveReferences();
            EnsureVisualRoot();
            EnsureSwordVisuals();
            CacheSwordPose();

            combatantProfile.ConfigureLargeMiniBoss();

            if (health != null)
            {
                health.Died -= HandleDied;
                health.Died += HandleDied;
                health.HealthChanged -= HandleHealthChanged;
                health.HealthChanged += HandleHealthChanged;
            }

            ApplyBodyColor(bodyColor);
            ResetCooldowns();
        }

        private void OnEnable()
        {
            ResolveReferences();

            if (encounter != null)
            {
                encounter.StateChanged -= HandleEncounterStateChanged;
                encounter.StateChanged += HandleEncounterStateChanged;
            }
        }

        private void OnDisable()
        {
            if (encounter != null)
                encounter.StateChanged -= HandleEncounterStateChanged;
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Died -= HandleDied;
                health.HealthChanged -= HandleHealthChanged;
            }

            if (encounter != null)
                encounter.StateChanged -= HandleEncounterStateChanged;
        }

        private void Update()
        {
            if (health == null || health.IsDead || IsDead)
                return;

            ResolveTarget();

            if (target == null || !CanFight())
                return;

            if (actionState != ActionState.Idle)
                return;

            TickGroundMovement();

            if (Time.time >= nextDecisionAt)
            {
                nextDecisionAt =
                    Time.time + Mathf.Max(0.05f, decisionInterval);

                TryChooseAction();
            }
        }

        public void ConfigureEncounter(
            BDBossEncounterController newEncounter,
            BDBossSharedSummonBudget newSummonBudget)
        {
            if (encounter != null)
                encounter.StateChanged -= HandleEncounterStateChanged;

            encounter = newEncounter;
            summonBudget = newSummonBudget;

            if (isActiveAndEnabled && encounter != null)
            {
                encounter.StateChanged -= HandleEncounterStateChanged;
                encounter.StateChanged += HandleEncounterStateChanged;
            }
        }

        public void ConfigureSummons(
            GameObject[] prefabs,
            Transform[] points)
        {
            summonPrefabs = prefabs;
            summonPoints = points;
        }

        public void ConfigureVisualRoot(Transform newVisualRoot)
        {
            visualRoot = newVisualRoot;
            EnsureVisualRoot();
            EnsureSwordVisuals();
            CacheSwordPose();
        }

        private bool CanFight()
        {
            return encounter == null || encounter.IsCombatActive;
        }

        private void ResolveReferences()
        {
            if (encounter == null)
            {
                encounter =
                    GetComponentInParent<BDBossEncounterController>();
            }

            if (summonBudget == null)
            {
                summonBudget =
                    GetComponentInParent<BDBossSharedSummonBudget>();
            }

            ResolveTarget();
        }

        private void ResolveTarget()
        {
            if (target == null)
                target = BDTargetFinder.FindPlayer();
        }

        private void TickGroundMovement()
        {
            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;

            float distance = toTarget.magnitude;

            if (distance <= 0.001f || distance > aggroRange)
                return;

            Vector3 direction = toTarget.normalized;
            RotateToward(direction);

            if (distance <= preferredDistance)
                return;

            float speed = moveSpeed *
                (enraged ? enragedMoveSpeedMultiplier : 1f);

            controller.Move(
                direction *
                Mathf.Max(0f, speed) *
                Time.deltaTime
            );
        }

        private void TryChooseAction()
        {
            List<int> legalActions = new List<int>(4);

            if (Time.time >= nextJumpAt)
                legalActions.Add(0);

            if (Time.time >= nextBulletHellAt)
                legalActions.Add(1);

            if (Time.time >= nextSwordAt)
                legalActions.Add(2);

            if (Time.time >= nextSummonAt &&
                HasSummonPrefab() &&
                HasSummonCapacity())
            {
                legalActions.Add(3);
            }

            if (legalActions.Count == 0)
                return;

            int choice = legalActions[
                Random.Range(0, legalActions.Count)
            ];

            switch (choice)
            {
                case 0:
                    StartCoroutine(JumpAttackRoutine());
                    break;

                case 1:
                    StartCoroutine(BulletHellRoutine(
                        fromLanding: false
                    ));
                    break;

                case 2:
                    StartCoroutine(DoubleSwordRoutine());
                    break;

                default:
                    StartCoroutine(SummonRoutine());
                    break;
            }
        }

        private IEnumerator JumpAttackRoutine()
        {
            actionState = ActionState.Jumping;

            float cooldownMultiplier =
                enraged ? enragedCooldownMultiplier : 1f;

            nextJumpAt =
                Time.time +
                Mathf.Max(0.1f, jumpCooldown) *
                cooldownMultiplier;

            Vector3 targetPoint = target.position;
            targetPoint.y = transform.position.y;

            BDSquareJumperVisuals.SpawnGroundTelegraph(
                targetPoint,
                landingRadius,
                enraged ? enragedColor : bodyColor,
                Mathf.Max(0.05f, jumpWindup)
            );

            BDSquareJumperVisuals.SpawnChargePulse(
                transform.position,
                enraged ? enragedColor : bodyColor,
                Mathf.Max(0.05f, jumpWindup)
            );

            yield return new WaitForSeconds(
                Mathf.Max(0.05f, jumpWindup)
            );

            if (!CanContinueAction())
            {
                actionState = ActionState.Idle;
                yield break;
            }

            Vector3 start = transform.position;
            Vector3 apex =
                Vector3.Lerp(start, targetPoint, 0.42f) +
                Vector3.up *
                jumpHeight *
                (enraged
                    ? enragedJumpHeightMultiplier
                    : 1f);

            yield return MoveControllerBetween(
                start,
                apex,
                Mathf.Max(0.05f, jumpRiseDuration),
                easeOut: true
            );

            yield return MoveControllerBetween(
                transform.position,
                targetPoint,
                Mathf.Max(0.05f, jumpFallDuration),
                easeOut: false
            );

            ResolveLandingImpact();

            if (landingBulletDelay > 0f)
            {
                yield return new WaitForSeconds(
                    landingBulletDelay
                );
            }

            yield return BulletHellRoutine(
                fromLanding: true
            );

            actionState = ActionState.Idle;
        }

        private IEnumerator MoveControllerBetween(
            Vector3 from,
            Vector3 to,
            float duration,
            bool easeOut)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (!CanContinueAction())
                    yield break;

                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                float eased = easeOut
                    ? 1f - Mathf.Pow(1f - t, 3f)
                    : t * t * t;

                Vector3 desired =
                    Vector3.Lerp(from, to, eased);

                Vector3 delta =
                    desired - transform.position;

                controller.Move(delta);

                if (delta.sqrMagnitude > 0.001f)
                {
                    Vector3 flat = delta;
                    flat.y = 0f;

                    if (flat.sqrMagnitude > 0.001f)
                        RotateToward(flat.normalized);
                }

                yield return null;
            }

            controller.Move(to - transform.position);
        }

        private void ResolveLandingImpact()
        {
            BDSquareJumperVisuals.SpawnLandingImpact(
                transform.position,
                landingRadius,
                enraged ? enragedColor : bodyColor
            );

            BDGameFeelEvents.RequestCameraShake(
                enraged ? 0.48f : 0.36f,
                enraged ? 0.28f : 0.22f
            );

            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                Mathf.Max(0.1f, landingRadius),
                ~0,
                QueryTriggerInteraction.Ignore
            );

            HashSet<BDHealth> damaged =
                new HashSet<BDHealth>();

            for (int i = 0; i < hits.Length; i++)
            {
                Collider hit = hits[i];

                if (hit == null ||
                    hit.transform == transform ||
                    hit.transform.IsChildOf(transform))
                {
                    continue;
                }

                BDPlayerMarker player =
                    hit.GetComponentInParent<BDPlayerMarker>();

                if (player == null)
                    continue;

                BDHealth playerHealth =
                    player.GetComponent<BDHealth>();

                if (playerHealth != null &&
                    damaged.Add(playerHealth))
                {
                    playerHealth.ApplyDamage(landingDamage);
                }
            }

            BDHorseDamageUtility.TryDamageHorseNear(
                transform.position,
                landingRadius,
                landingDamage,
                transform
            );
        }

        private IEnumerator BulletHellRoutine(bool fromLanding)
        {
            ActionState previousState = actionState;

            if (!fromLanding)
                actionState = ActionState.BulletHell;

            float cooldownMultiplier =
                enraged ? enragedCooldownMultiplier : 1f;

            if (!fromLanding)
            {
                nextBulletHellAt =
                    Time.time +
                    Mathf.Max(0.1f, bulletHellCooldown) *
                    cooldownMultiplier;
            }

            int waves = enraged
                ? Mathf.Max(1, enragedBulletWaves)
                : Mathf.Max(1, normalBulletWaves);

            int bullets = enraged
                ? Mathf.Max(6, enragedBulletCount)
                : Mathf.Max(6, normalBulletCount);

            float baseOffset = Random.Range(0f, 360f);

            for (int wave = 0; wave < waves; wave++)
            {
                if (!CanContinueAction())
                    yield break;

                float offset =
                    baseOffset +
                    wave * (180f / bullets);

                FireRadialWave(bullets, offset);

                if (wave + 1 < waves)
                {
                    yield return new WaitForSeconds(
                        Mathf.Max(0.03f, bulletWaveInterval)
                    );
                }
            }

            if (!fromLanding)
                actionState = ActionState.Idle;
            else
                actionState = previousState;
        }

        private void FireRadialWave(
            int bulletCount,
            float angleOffset)
        {
            Vector3 origin =
                transform.position + Vector3.up * 0.75f;

            float speed = bulletSpeed *
                (enraged
                    ? enragedBulletSpeedMultiplier
                    : 1f);

            for (int i = 0; i < bulletCount; i++)
            {
                float angle =
                    angleOffset +
                    i * (360f / bulletCount);

                Vector3 direction =
                    Quaternion.Euler(0f, angle, 0f) *
                    Vector3.forward;

                BDSquareJumperProjectile.Spawn(
                    origin,
                    direction,
                    speed,
                    bulletDamage,
                    bulletRadius,
                    bulletLifetime,
                    transform,
                    encounter,
                    enraged
                );
            }

            BDSquareJumperVisuals.SpawnBulletBurst(
                origin,
                enraged ? enragedColor : bodyColor
            );
        }

        private IEnumerator DoubleSwordRoutine()
        {
            actionState = ActionState.SwordAttack;

            float cooldownMultiplier =
                enraged ? enragedCooldownMultiplier : 1f;

            nextSwordAt =
                Time.time +
                Mathf.Max(0.1f, swordCooldown) *
                cooldownMultiplier;

            RestoreSwordPose();

            Vector3 leftHitCenter =
                transform.position -
                transform.right * swordSideOffset +
                transform.forward * swordForwardOffset;

            Vector3 rightHitCenter =
                transform.position +
                transform.right * swordSideOffset +
                transform.forward * swordForwardOffset;

            BDSquareJumperVisuals.SpawnGroundTelegraph(
                leftHitCenter,
                swordHitRadius,
                new Color(0.88f, 0.78f, 0.55f, 1f),
                swordWindup
            );

            BDSquareJumperVisuals.SpawnGroundTelegraph(
                rightHitCenter,
                swordHitRadius,
                new Color(0.88f, 0.78f, 0.55f, 1f),
                swordWindup
            );

            Quaternion leftWindup =
                leftSwordRestRotation *
                Quaternion.Euler(0f, -68f, 0f);

            Quaternion rightWindup =
                rightSwordRestRotation *
                Quaternion.Euler(0f, 68f, 0f);

            yield return AnimateSwords(
                leftSwordRestRotation,
                rightSwordRestRotation,
                leftWindup,
                rightWindup,
                swordWindup
            );

            Quaternion leftSlash =
                leftSwordRestRotation *
                Quaternion.Euler(0f, 80f, 0f);

            Quaternion rightSlash =
                rightSwordRestRotation *
                Quaternion.Euler(0f, -80f, 0f);

            yield return AnimateSwords(
                leftWindup,
                rightWindup,
                leftSlash,
                rightSlash,
                swordSlashDuration
            );

            ResolveSwordDamage(
                leftHitCenter,
                rightHitCenter
            );

            BDSquareJumperVisuals.SpawnSwordImpact(
                leftHitCenter,
                rightHitCenter
            );

            yield return AnimateSwords(
                leftSlash,
                rightSlash,
                leftSwordRestRotation,
                rightSwordRestRotation,
                swordRecovery
            );

            RestoreSwordPose();
            actionState = ActionState.Idle;
        }

        private IEnumerator AnimateSwords(
            Quaternion leftFrom,
            Quaternion rightFrom,
            Quaternion leftTo,
            Quaternion rightTo,
            float duration)
        {
            float safeDuration = Mathf.Max(0.03f, duration);
            float elapsed = 0f;

            while (elapsed < safeDuration)
            {
                if (!CanContinueAction())
                    yield break;

                elapsed += Time.deltaTime;
                float t =
                    Mathf.Clamp01(elapsed / safeDuration);

                float eased =
                    t * t * (3f - 2f * t);

                if (leftSwordPivot != null)
                {
                    leftSwordPivot.localRotation =
                        Quaternion.Slerp(
                            leftFrom,
                            leftTo,
                            eased
                        );
                }

                if (rightSwordPivot != null)
                {
                    rightSwordPivot.localRotation =
                        Quaternion.Slerp(
                            rightFrom,
                            rightTo,
                            eased
                        );
                }

                yield return null;
            }
        }

        private void ResolveSwordDamage(
            Vector3 leftCenter,
            Vector3 rightCenter)
        {
            HashSet<BDHealth> damaged =
                new HashSet<BDHealth>();

            ResolveSwordDamageAt(leftCenter, damaged);
            ResolveSwordDamageAt(rightCenter, damaged);

            BDHorseDamageUtility.TryDamageHorseNear(
                leftCenter,
                swordHitRadius,
                swordDamage,
                transform
            );

            BDHorseDamageUtility.TryDamageHorseNear(
                rightCenter,
                swordHitRadius,
                swordDamage,
                transform
            );
        }

        private void ResolveSwordDamageAt(
            Vector3 center,
            HashSet<BDHealth> damaged)
        {
            Collider[] hits = Physics.OverlapSphere(
                center,
                Mathf.Max(0.1f, swordHitRadius),
                ~0,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < hits.Length; i++)
            {
                Collider hit = hits[i];

                if (hit == null)
                    continue;

                BDPlayerMarker player =
                    hit.GetComponentInParent<BDPlayerMarker>();

                if (player == null)
                    continue;

                BDHealth playerHealth =
                    player.GetComponent<BDHealth>();

                if (playerHealth != null &&
                    damaged.Add(playerHealth))
                {
                    playerHealth.ApplyDamage(swordDamage);
                }
            }
        }

        private IEnumerator SummonRoutine()
        {
            actionState = ActionState.Summoning;

            float cooldownMultiplier =
                enraged ? enragedCooldownMultiplier : 1f;

            nextSummonAt =
                Time.time +
                Mathf.Max(0.1f, summonCooldown) *
                cooldownMultiplier;

            BDSquareJumperVisuals.SpawnSummonPulse(
                transform.position,
                enraged ? enragedColor : bodyColor,
                Mathf.Max(0.05f, summonWindup)
            );

            yield return new WaitForSeconds(
                Mathf.Max(0.05f, summonWindup)
            );

            if (!CanContinueAction())
            {
                actionState = ActionState.Idle;
                yield break;
            }

            int requested;

            if (enraged)
            {
                requested = summonBudget != null
                    ? Mathf.Min(
                        summonBudget.AvailableSlots,
                        Mathf.Max(1, enragedSummonActionCap)
                    )
                    : Mathf.Max(1, enragedSummonActionCap);
            }
            else
            {
                int minimum =
                    Mathf.Max(1, normalSummonMinimum);

                int maximum =
                    Mathf.Max(
                        minimum,
                        normalSummonMaximum
                    );

                requested =
                    Random.Range(minimum, maximum + 1);
            }

            SpawnSummons(requested);
            actionState = ActionState.Idle;
        }

        private void SpawnSummons(int requested)
        {
            if (!HasSummonPrefab())
            {
                if (!warnedMissingSummons)
                {
                    Debug.LogWarning(
                        "Square Jumper has no Sword/Shooter/Patrol " +
                        "summon prefabs assigned.",
                        this
                    );
                    warnedMissingSummons = true;
                }

                return;
            }

            int count = Mathf.Max(0, requested);

            if (summonBudget != null)
            {
                count =
                    summonBudget.ClampToAvailable(count);
            }

            for (int i = 0; i < count; i++)
            {
                GameObject prefab =
                    ResolveRandomSummonPrefab();

                if (prefab == null)
                    continue;

                ResolveSummonPose(
                    i,
                    out Vector3 position,
                    out Quaternion rotation
                );

                if (summonBudget != null)
                {
                    summonBudget.TrySpawn(
                        prefab,
                        position,
                        rotation,
                        transform.parent,
                        out _
                    );
                }
                else
                {
                    Instantiate(
                        prefab,
                        position,
                        rotation,
                        transform.parent
                    );
                }
            }
        }

        private GameObject ResolveRandomSummonPrefab()
        {
            if (summonPrefabs == null ||
                summonPrefabs.Length == 0)
            {
                return null;
            }

            int start =
                Random.Range(0, summonPrefabs.Length);

            for (int offset = 0;
                 offset < summonPrefabs.Length;
                 offset++)
            {
                int index =
                    (start + offset) %
                    summonPrefabs.Length;

                if (summonPrefabs[index] != null)
                    return summonPrefabs[index];
            }

            return null;
        }

        private void ResolveSummonPose(
            int index,
            out Vector3 position,
            out Quaternion rotation)
        {
            if (summonPoints != null &&
                summonPoints.Length > 0)
            {
                for (int offset = 0;
                     offset < summonPoints.Length;
                     offset++)
                {
                    Transform point =
                        summonPoints[
                            (index + offset) %
                            summonPoints.Length
                        ];

                    if (point == null)
                        continue;

                    position = point.position;
                    rotation = point.rotation;
                    return;
                }
            }

            float angle = index * 137.5f;

            Vector3 direction =
                Quaternion.Euler(0f, angle, 0f) *
                Vector3.forward;

            position =
                transform.position +
                direction * 3.3f;

            rotation =
                Quaternion.LookRotation(
                    -direction,
                    Vector3.up
                );
        }

        private bool HasSummonPrefab()
        {
            if (summonPrefabs == null)
                return false;

            for (int i = 0; i < summonPrefabs.Length; i++)
            {
                if (summonPrefabs[i] != null)
                    return true;
            }

            return false;
        }

        private bool HasSummonCapacity()
        {
            return summonBudget == null ||
                   summonBudget.AvailableSlots > 0;
        }

        private void HandleHealthChanged(
            BDHealth source,
            float current,
            float maximum)
        {
            if (enraged ||
                maximum <= 0f ||
                current <= 0f)
            {
                return;
            }

            if (current / maximum >
                enragedHealthThreshold)
            {
                return;
            }

            enraged = true;
            ApplyBodyColor(enragedColor);

            BDSquareJumperVisuals.SpawnEnrageBurst(
                transform.position,
                enragedColor
            );

            BDGameFeelEvents.RequestCameraShake(
                0.32f,
                0.28f
            );

            nextJumpAt =
                Mathf.Min(nextJumpAt, Time.time + 0.35f);

            nextBulletHellAt =
                Mathf.Min(
                    nextBulletHellAt,
                    Time.time + 0.15f
                );

            nextSummonAt =
                Mathf.Min(
                    nextSummonAt,
                    Time.time + 0.8f
                );
        }

        private void HandleDied(BDHealth source)
        {
            StopAllCoroutines();
            actionState = ActionState.Dead;

            if (controller != null)
                controller.enabled = false;

            RestoreSwordPose();

            BDSquareJumperVisuals.SpawnDeathBurst(
                transform.position,
                enraged ? enragedColor : bodyColor
            );

            if (summonBudget != null)
                summonBudget.ClearAllSummons();

            if (encounter != null)
                encounter.MarkVictory();
        }

        private void HandleEncounterStateChanged(
            BDBossEncounterState state)
        {
            if (state == BDBossEncounterState.Dormant)
            {
                StopAllCoroutines();
                actionState = ActionState.Idle;
                enraged = false;

                if (controller != null)
                    controller.enabled = false;

                transform.position = spawnPosition;
                transform.rotation = spawnRotation;

                if (controller != null)
                    controller.enabled = true;

                if (health != null)
                {
                    health.SetMaxHealth(
                        health.MaxHealth,
                        refill: true
                    );
                }

                ApplyBodyColor(bodyColor);
                RestoreSwordPose();
                ResetCooldowns();
            }

            if (state == BDBossEncounterState.Victory ||
                state == BDBossEncounterState.Completed ||
                state == BDBossEncounterState.Failed)
            {
                StopAllCoroutines();

                if (!health.IsDead)
                    actionState = ActionState.Idle;
            }
        }

        private bool CanContinueAction()
        {
            return health != null &&
                   !health.IsDead &&
                   !IsDead &&
                   CanFight();
        }

        private void ResetCooldowns()
        {
            float now = Time.time;

            nextDecisionAt = now + 0.3f;
            nextJumpAt = now + 0.9f;
            nextBulletHellAt = now + 1.6f;
            nextSwordAt = now + 2.2f;
            nextSummonAt = now + 3.0f;
        }

        private void RotateToward(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion desired =
                Quaternion.LookRotation(
                    direction.normalized,
                    Vector3.up
                );

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    desired,
                    Mathf.Max(0.1f, rotationSpeed) *
                    Time.deltaTime
                );
        }

        private void EnsureVisualRoot()
        {
            if (visualRoot == null)
                visualRoot = transform;

            visualRenderers =
                visualRoot.GetComponentsInChildren<Renderer>(
                    includeInactive: true
                );
        }

        private void ApplyBodyColor(Color color)
        {
            if (visualRenderers == null)
                return;

            for (int i = 0; i < visualRenderers.Length; i++)
            {
                Renderer renderer = visualRenderers[i];

                if (renderer == null ||
                    renderer.transform == leftSwordPivot ||
                    renderer.transform == rightSwordPivot)
                {
                    continue;
                }

                Material material = ResolveWritableMaterial(renderer);
                material.color = color;

                if (material.HasProperty("_BaseColor"))
                    material.SetColor("_BaseColor", color);

                if (material.HasProperty("_Color"))
                    material.SetColor("_Color", color);

                if (material.HasProperty("_EmissionColor"))
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor(
                        "_EmissionColor",
                        color * (enraged ? 1.8f : 0.45f)
                    );
                }
            }
        }

        private void EnsureSwordVisuals()
        {
            leftSwordPivot = FindOrCreateSword(
                "BD_SquareJumper_LeftSword",
                new Vector3(
                    -1.35f,
                    0.15f,
                    0.22f
                ),
                -26f
            );

            rightSwordPivot = FindOrCreateSword(
                "BD_SquareJumper_RightSword",
                new Vector3(
                    1.35f,
                    0.15f,
                    0.22f
                ),
                26f
            );
        }

        private Transform FindOrCreateSword(
            string objectName,
            Vector3 localPosition,
            float yaw)
        {
            Transform parent =
                visualRoot != null
                    ? visualRoot
                    : transform;

            Transform existing =
                parent.Find(objectName);

            if (existing != null)
                return existing;

            GameObject pivotObject =
                new GameObject(objectName);

            Transform pivot = pivotObject.transform;
            pivot.SetParent(parent, false);
            pivot.localPosition = localPosition;
            pivot.localRotation =
                Quaternion.Euler(0f, yaw, 0f);

            GameObject blade =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube
                );

            blade.name = objectName + "_Blade";
            blade.transform.SetParent(pivot, false);
            blade.transform.localPosition =
                new Vector3(0f, 0.02f, 0.95f);
            blade.transform.localScale =
                new Vector3(0.16f, 0.12f, 1.9f);

            Collider bladeCollider =
                blade.GetComponent<Collider>();

            if (bladeCollider != null)
                DestroyObjectSafely(bladeCollider);

            Renderer bladeRenderer =
                blade.GetComponent<Renderer>();

            if (bladeRenderer != null)
            {
                Material material =
                    ResolveWritableMaterial(bladeRenderer);

                Color bladeColor =
                    new Color(
                        0.80f,
                        0.88f,
                        0.96f,
                        1f
                    );

                material.color = bladeColor;

                if (material.HasProperty("_BaseColor"))
                {
                    material.SetColor(
                        "_BaseColor",
                        bladeColor
                    );
                }

                if (material.HasProperty("_EmissionColor"))
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor(
                        "_EmissionColor",
                        bladeColor * 1.2f
                    );
                }
            }

            return pivot;
        }

        private void CacheSwordPose()
        {
            if (leftSwordPivot != null)
            {
                leftSwordRestRotation =
                    leftSwordPivot.localRotation;

                leftSwordRestPosition =
                    leftSwordPivot.localPosition;
            }

            if (rightSwordPivot != null)
            {
                rightSwordRestRotation =
                    rightSwordPivot.localRotation;

                rightSwordRestPosition =
                    rightSwordPivot.localPosition;
            }
        }

        private void RestoreSwordPose()
        {
            if (leftSwordPivot != null)
            {
                leftSwordPivot.localRotation =
                    leftSwordRestRotation;

                leftSwordPivot.localPosition =
                    leftSwordRestPosition;
            }

            if (rightSwordPivot != null)
            {
                rightSwordPivot.localRotation =
                    rightSwordRestRotation;

                rightSwordPivot.localPosition =
                    rightSwordRestPosition;
            }
        }

        // BD SQUARE JUMPER EDIT-MODE SAFETY FIX
        private static Material ResolveWritableMaterial(
            Renderer renderer)
        {
            if (renderer == null)
                return null;

            if (Application.isPlaying)
                return renderer.material;

            Material source = renderer.sharedMaterial;
            Shader shader =
                source != null
                    ? source.shader
                    : Shader.Find(
                        "Universal Render Pipeline/Lit"
                    );

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            Material editable =
                source != null
                    ? new Material(source)
                    : new Material(shader);

            editable.name =
                $"{renderer.gameObject.name}_EditModeMaterial";

            renderer.sharedMaterial = editable;
            return editable;
        }

        private static void DestroyObjectSafely(
            UnityEngine.Object target)
        {
            if (target == null)
                return;

            if (Application.isPlaying)
                UnityEngine.Object.Destroy(target);
            else
                UnityEngine.Object.DestroyImmediate(target);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color =
                new Color(1f, 0.25f, 0.12f, 0.7f);

            Gizmos.DrawWireSphere(
                transform.position,
                landingRadius
            );

            Gizmos.color =
                new Color(0.85f, 0.82f, 0.55f, 0.7f);

            Gizmos.DrawWireSphere(
                transform.position -
                transform.right * swordSideOffset +
                transform.forward * swordForwardOffset,
                swordHitRadius
            );

            Gizmos.DrawWireSphere(
                transform.position +
                transform.right * swordSideOffset +
                transform.forward * swordForwardOffset,
                swordHitRadius
            );
        }
    }
}
