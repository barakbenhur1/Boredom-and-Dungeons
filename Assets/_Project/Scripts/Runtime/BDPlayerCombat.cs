using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDPlayerCombat : MonoBehaviour
    {
        [Header("Melee")]
        [SerializeField] private float lightDamage = 25f;
        [SerializeField] private float heavyDamage = 55f;
        [SerializeField] private float attackRange = 2.25f;
        [SerializeField] private float attackRadius = 1.15f;
        [SerializeField] private float meleeStartForwardOffset = 0.12f;
        [SerializeField] private bool meleeSoftAimAssistEnabled = true;
        [SerializeField] private float meleeSoftAimAssistRange = 2.65f;
        [SerializeField] private float meleeSoftAimAssistConeDegrees = 56f;
        [SerializeField] private float meleeSoftAimAssistMaxAngleDegrees = 24f;
        [SerializeField] private float lightCooldown = 0.28f;
        [SerializeField] private float heavyCooldown = 0.85f;
        [SerializeField] private float lightKnockbackStrength = 7.0f;
        [SerializeField] private float heavyKnockbackStrength = 19.0f;
        [SerializeField] private float lightKnockLockDuration = 0.08f;
        [SerializeField] private float heavyKnockLockDuration = 0.20f;
        [SerializeField] private float lightHitStaggerDuration = 0.070f;
        [SerializeField] private float heavyHitStaggerDuration = 0.145f;
        [SerializeField] private bool spawnMeleeImpactFeedback = true;
        [SerializeField] private bool spawnMeleeSlashArc = true;
        [SerializeField] private float lightHitCameraShakeStrength = 0.10f;
        [SerializeField] private float heavyHitCameraShakeStrength = 0.32f;
        [SerializeField] private float lightHitCameraShakeDuration = 0.08f;
        [SerializeField] private float heavyHitCameraShakeDuration = 0.16f;
        [SerializeField] private float lightHitStopDuration = 0.025f;
        [SerializeField] private float heavyHitStopDuration = 0.055f;
        [SerializeField] private float lightHitStopTimeScale = 0.55f;
        [SerializeField] private float heavyHitStopTimeScale = 0.12f;

        [Header("Combat Mouse Aim")]
        [SerializeField] private bool attacksFollowMousePoint = true;
        [SerializeField] private float combatMouseAimMinDistance = 0.35f;
        [SerializeField] private float combatMouseAimMaxRayDistance = 350f;

        [Header("Ranged")]
        [SerializeField] private float rangedDamage = 75f;
        [SerializeField] private float rangedCooldown = 0.18f;
        [SerializeField] private float rangedProjectileSpeed = 16f;
        [SerializeField] private float rangedProjectileLifetime = 4.5f;
        [SerializeField] private float rangedProjectileHitRadius = 1.05f;
        [SerializeField] private float rangedProjectileKnockback = 9f;
        [SerializeField] private float rangedSpawnForwardOffset = 1.35f;
        [SerializeField] private float rangedSpawnHeight = 1.25f;
        [SerializeField] private int rangedMagazineSize = 3;
        [SerializeField] private float rangedReloadDuration = 6.0f;

        [Header("Charged Ranged Shot")]
        // BD CHARGED SHOT SYSTEM
        [SerializeField] private bool enableChargedShot = true;
        // BD TAP-VS-HOLD CHARGED SHOT FIX
        [SerializeField] private float chargedShotHoldThreshold = 0.22f;
        [SerializeField] private float chargedShotBaseDuration = 0.90f;
        [SerializeField] private float chargedShotSecondsPerAdditionalAmmo = 0.45f;
        [SerializeField] private float chargedShotMaximumDuration = 3.20f;
        [SerializeField] private float chargedProjectileScalePerExtraAmmo = 0.22f;
        [SerializeField] private float chargedHitRadiusPerExtraAmmo = 0.10f;
        [SerializeField] private float chargedKnockbackPerExtraAmmo = 0.28f;
        [SerializeField] private float chargedSpeedPerExtraAmmo = 0.035f;

        [Header("Debug")]
        [SerializeField] private bool showCombatDebug = false;

        private float nextLightAllowedAt;
        private float nextHeavyAllowedAt;
        private float nextRangedAllowedAt;
        private int rangedAmmo;
        private float reloadEndsAt;
        private bool reloading;
        private string lastCombatAction = "none";
        private BDHorseController cachedMountedHorseCheck;
        private float nextMountedHorseResolveAt;

        // BD LANDING ATTACK VISUAL EXCLUSIVITY
        private float suppressStandardMeleeVisualUntilUnscaled = -999f;

        // BD BOOST API: runtime modifiers
        private int boostAdditionalMagazineCapacity;
        private float boostReloadDurationReduction;
        private float boostWeaponDamageMultiplier = 1f;
        private float boostMinimumReloadDuration = 1f;

        private bool chargedShotCharging;
        private float chargedShotStartedAtUnscaled;
        private float chargedShotRequiredDuration;
        private int chargedShotReservedAmmo;
        private BDChargedShotChargeVisual chargedShotChargeVisual;
        private bool rangedPressPending;
        private float rangedPressStartedAtUnscaled;
        private static readonly Collider[] MeleeHitBuffer = new Collider[64];
        private static readonly BDHealth[] MeleeHealthBuffer = new BDHealth[32];
        private static readonly Collider[] MeleeAssistBuffer = new Collider[32];

        public int RangedAmmo => rangedAmmo;
        public int RangedMagazineSize =>
            Mathf.Max(1, rangedMagazineSize + boostAdditionalMagazineCapacity);
        public float EffectiveRangedReloadDuration =>
            Mathf.Max(
                boostMinimumReloadDuration,
                rangedReloadDuration - boostReloadDurationReduction
            );
        public float WeaponDamageMultiplier =>
            Mathf.Max(0.01f, boostWeaponDamageMultiplier);
        public bool IsChargingRangedShot => chargedShotCharging;
        public int ChargedShotReservedAmmo => chargedShotReservedAmmo;
        public float ChargedShotRequiredDuration =>
            chargedShotRequiredDuration;
        public float ChargedShotProgress01 =>
            !chargedShotCharging
                ? 0f
                : Mathf.Clamp01(
                    (Time.unscaledTime -
                     chargedShotStartedAtUnscaled) /
                    Mathf.Max(0.01f, chargedShotRequiredDuration)
                );
        public bool IsReloading => reloading;
        public float RangedReloadRemaining => reloading ? Mathf.Max(0f, reloadEndsAt - Time.time) : 0f;
        public float RangedReloadProgress01
        {
            get
            {
                if (!reloading)
                    return 1f;

                float duration = EffectiveRangedReloadDuration;
                return Mathf.Clamp01(1f - ((reloadEndsAt - Time.time) / duration));
            }
        }

        public void SuppressNextStandardMeleeVisual(
            float maximumWaitSeconds = 0.18f)
        {
            suppressStandardMeleeVisualUntilUnscaled =
                Time.unscaledTime +
                Mathf.Max(0.02f, maximumWaitSeconds);
        }

        private void Awake()
        {
            rangedAmmo = RangedMagazineSize;
        }

        private void OnDisable()
        {
            ClearPendingRangedPress();
            CancelChargedShot();
        }

        private void Update()
        {
            TickReload();

            bool mounted = IsMountedOnHorse();

            if (!mounted)
            {
                if (ReadLightAttackPressed())
                    TryMeleeAttack(lightDamage, lightCooldown, ref nextLightAllowedAt, "light");

                if (ReadHeavyAttackPressed())
                    TryMeleeAttack(heavyDamage, heavyCooldown, ref nextHeavyAllowedAt, "heavy");
            }
            else
            {
                // While mounted, sword attacks are intentionally disabled.
                // Mounted combat is ranged-only.
                if (ReadLightAttackPressed() || ReadHeavyAttackPressed())
                    lastCombatAction = "mounted melee disabled";
            }

            TickChargedRangedAttack();
            EnsureAutomaticReloadForEmptyMagazine();
        }


        private bool IsMountedOnHorse()
        {
            if (cachedMountedHorseCheck == null || (Application.isPlaying && Time.time >= nextMountedHorseResolveAt))
            {
                cachedMountedHorseCheck = FindFirstObjectByType<BDHorseController>();
                nextMountedHorseResolveAt = Application.isPlaying ? Time.time + 0.25f : 0f;
            }

            if (cachedMountedHorseCheck == null || !cachedMountedHorseCheck.IsMounted || cachedMountedHorseCheck.Rider == null)
                return false;

            Transform rider = cachedMountedHorseCheck.Rider;
            return rider == transform || transform.IsChildOf(rider) || rider.IsChildOf(transform);
        }


        // BD AUTO-RELOAD WATCHDOG FIX
        private void EnsureAutomaticReloadForEmptyMagazine()
        {
            if (rangedAmmo > 0 || reloading)
                return;

            if (chargedShotCharging)
                return;

            StartReloadFromEmptyMagazine(
                "empty magazine watchdog"
            );
        }

        private void StartReloadFromEmptyMagazine(string reason)
        {
            if (rangedAmmo > 0)
                return;

            reloading = true;
            reloadEndsAt =
                Time.time + EffectiveRangedReloadDuration;

            lastCombatAction =
                $"ranged reload: {reason}";
        }

        private void TickReload()
        {
            if (!reloading)
                return;

            if (Time.time < reloadEndsAt)
                return;

            rangedAmmo = RangedMagazineSize;
            reloading = false;
            lastCombatAction = "ranged reload complete";
        }

        private void BeginReloadIfNeeded()
        {
            if (rangedAmmo > 0 || reloading)
                return;

            StartReloadFromEmptyMagazine(
                "requested automatically"
            );
        }

        private void TryMeleeAttack(float damage, float cooldown, ref float nextAllowedAt, string label)
        {
            if (Time.time < nextAllowedAt)
                return;

            nextAllowedAt = Time.time + Mathf.Max(0.01f, cooldown);

            Vector3 baseAim = GetCombatAimDirection();
            Vector3 aim = attacksFollowMousePoint && TryGetMousePointCombatAim(out _)
                ? baseAim
                : ResolveMeleeSoftAimDirection(baseAim);

            ApplyCombatFacing(aim);

            Vector3 capsuleStart = transform.position + Vector3.up * 1f + aim * Mathf.Max(0f, meleeStartForwardOffset);
            Vector3 capsuleEnd = transform.position + Vector3.up * 1f + aim * Mathf.Max(meleeStartForwardOffset + 0.05f, attackRange);
            Vector3 feedbackCenter = Vector3.Lerp(capsuleStart, capsuleEnd, 0.65f);

            SpawnMeleeSlashArc(aim, label == "heavy");

            // Capsule catches enemies that are too close to the player.
            // The old endpoint sphere could miss enemies already inside the swing / body range.
            int overlapCount = Physics.OverlapCapsuleNonAlloc(
                capsuleStart,
                capsuleEnd,
                attackRadius,
                MeleeHitBuffer,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            int hitCount = 0;
            int uniqueHealthCount = 0;
            float effectiveDamage = damage * WeaponDamageMultiplier;

            for (int i = 0; i < overlapCount; i++)
            {
                Collider hit = MeleeHitBuffer[i];
                if (hit == null)
                    continue;

                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                    continue;

                if (hit.GetComponentInParent<BDPlayerMarker>() != null)
                    continue;

                if (hit.GetComponentInParent<BDHorseHealth>() != null)
                    continue;

                BDHealth health = hit.GetComponentInParent<BDHealth>();
                if (health == null || health.IsDead)
                    continue;

                if (AlreadyHitThisSwing(health, uniqueHealthCount))
                    continue;

                if (uniqueHealthCount < MeleeHealthBuffer.Length)
                    MeleeHealthBuffer[uniqueHealthCount++] = health;

                bool heavyHit = label == "heavy";
                health.ApplyDamage(effectiveDamage);
                RequestEnemyHitStagger(health, heavyHit ? heavyHitStaggerDuration : lightHitStaggerDuration);
                RequestEnemyHitFlash(health, heavyHit);
                TriggerMeleeHitFeedback(health, hit, feedbackCenter, aim, heavyHit);

                BDKnockbackReceiver receiver = health.GetComponent<BDKnockbackReceiver>();
                if (receiver == null && health.GetComponent<CharacterController>() != null)
                    receiver = health.gameObject.AddComponent<BDKnockbackReceiver>();

                if (receiver != null)
                {
                    Vector3 knockDirection = health.transform.position - transform.position;
                    knockDirection.y = 0f;

                    if (knockDirection.sqrMagnitude < 0.001f)
                        knockDirection = aim;

                    receiver.AddKnockback(
                        knockDirection,
                        heavyHit ? heavyKnockbackStrength : lightKnockbackStrength,
                        heavyHit ? heavyKnockLockDuration : lightKnockLockDuration
                    );
                }

                hitCount++;
            }

            for (int i = 0; i < uniqueHealthCount; i++)
                MeleeHealthBuffer[i] = null;

            lastCombatAction = $"{label} melee capsule hits={hitCount}";
        }

        private bool AlreadyHitThisSwing(BDHealth health, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (MeleeHealthBuffer[i] == health)
                    return true;
            }

            return false;
        }




        private Vector3 ResolveMeleeSoftAimDirection(Vector3 baseAim)
        {
            baseAim.y = 0f;

            if (baseAim.sqrMagnitude < 0.001f)
                baseAim = transform.forward;

            baseAim.y = 0f;

            if (baseAim.sqrMagnitude < 0.001f)
                baseAim = Vector3.forward;

            baseAim.Normalize();

            if (!meleeSoftAimAssistEnabled)
                return baseAim;

            int count = Physics.OverlapSphereNonAlloc(
                transform.position + Vector3.up * 0.9f,
                meleeSoftAimAssistRange,
                MeleeAssistBuffer,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            BDHealth bestHealth = null;
            Vector3 bestDirection = baseAim;
            float bestScore = float.MaxValue;
            float halfCone = Mathf.Clamp(meleeSoftAimAssistConeDegrees * 0.5f, 1f, 89f);

            for (int i = 0; i < count; i++)
            {
                Collider hit = MeleeAssistBuffer[i];
                if (hit == null)
                    continue;

                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                    continue;

                if (hit.GetComponentInParent<BDPlayerMarker>() != null)
                    continue;

                if (hit.GetComponentInParent<BDHorseHealth>() != null)
                    continue;

                BDHealth health = hit.GetComponentInParent<BDHealth>();
                if (health == null || health.IsDead)
                    continue;

                Vector3 toTarget = health.transform.position - transform.position;
                toTarget.y = 0f;

                float distance = toTarget.magnitude;
                if (distance <= 0.001f || distance > meleeSoftAimAssistRange)
                    continue;

                Vector3 direction = toTarget / distance;
                float angle = Vector3.Angle(baseAim, direction);

                if (angle > halfCone)
                    continue;

                // Prefer close targets and targets closer to the current aim.
                float score = angle * 1.35f + distance * 9.0f;
                if (score >= bestScore)
                    continue;

                bestScore = score;
                bestHealth = health;
                bestDirection = direction;
            }

            for (int i = 0; i < count; i++)
                MeleeAssistBuffer[i] = null;

            if (bestHealth == null)
                return baseAim;

            float signedAngle = Vector3.SignedAngle(baseAim, bestDirection, Vector3.up);
            float clampedAngle = Mathf.Clamp(signedAngle, -meleeSoftAimAssistMaxAngleDegrees, meleeSoftAimAssistMaxAngleDegrees);
            Vector3 assisted = Quaternion.AngleAxis(clampedAngle, Vector3.up) * baseAim;
            assisted.y = 0f;

            if (assisted.sqrMagnitude < 0.001f)
                return baseAim;

            return assisted.normalized;
        }

        private void SpawnMeleeSlashArc(
            Vector3 aim,
            bool heavySwing)
        {
            if (Time.unscaledTime <=
                suppressStandardMeleeVisualUntilUnscaled)
            {
                suppressStandardMeleeVisualUntilUnscaled = -999f;
                return;
            }

            suppressStandardMeleeVisualUntilUnscaled = -999f;

            if (!spawnMeleeSlashArc)
                return;

            Vector3 origin =
                transform.position +
                aim.normalized * 0.25f;

            BDMeleeSlashArcVisual.Spawn(
                origin,
                aim,
                attackRange,
                attackRadius,
                heavySwing
            );
        }



        private void RequestEnemyHitFlash(BDHealth health, bool heavyHit)
        {
            if (health == null || health.IsDead)
                return;

            BDEnemyHitFlashReceiver flash = health.GetComponent<BDEnemyHitFlashReceiver>();
            if (flash == null && health.GetComponent<CharacterController>() != null)
                flash = health.gameObject.AddComponent<BDEnemyHitFlashReceiver>();

            if (flash == null)
                return;

            if (heavyHit)
                flash.FlashHeavy();
            else
                flash.FlashLight();
        }

        private void RequestEnemyHitStagger(BDHealth health, float duration)
        {
            if (health == null || health.IsDead)
                return;

            BDHitStaggerReceiver stagger = health.GetComponent<BDHitStaggerReceiver>();
            if (stagger == null && health.GetComponent<CharacterController>() != null)
                stagger = health.gameObject.AddComponent<BDHitStaggerReceiver>();

            if (stagger != null)
                stagger.RequestStagger(duration);
        }

        private void TriggerMeleeHitFeedback(BDHealth targetHealth, Collider hitCollider, Vector3 attackCenter, Vector3 attackDirection, bool heavyHit)
        {
            if (targetHealth == null)
                return;

            BDDamageFlashFeedback flash = targetHealth.GetComponent<BDDamageFlashFeedback>();
            if (flash == null && targetHealth.GetComponentInChildren<Renderer>() != null)
                flash = targetHealth.gameObject.AddComponent<BDDamageFlashFeedback>();

            if (flash != null)
                flash.TriggerImpactFlash(heavyHit);

            if (!spawnMeleeImpactFeedback)
                return;

            Vector3 impactPosition = targetHealth.transform.position + Vector3.up * 1.0f;

            if (hitCollider != null)
            {
                Vector3 closest = hitCollider.ClosestPoint(attackCenter);
                if (closest.sqrMagnitude > 0.001f)
                    impactPosition = closest + Vector3.up * 0.05f;
            }

            BDMeleeImpactBurst.Spawn(impactPosition, attackDirection, heavyHit);

            BDGameFeelEvents.RequestCameraShake(
                heavyHit ? heavyHitCameraShakeStrength : lightHitCameraShakeStrength,
                heavyHit ? heavyHitCameraShakeDuration : lightHitCameraShakeDuration
            );

            BDHitStop.Request(
                heavyHit ? heavyHitStopDuration : lightHitStopDuration,
                heavyHit ? heavyHitStopTimeScale : lightHitStopTimeScale
            );

            if (heavyHit)
                BDGameFeelAudio.PlayHeavyHit();
            else
                BDGameFeelAudio.PlayLightHit();
        }

        private void TickChargedRangedAttack()
        {
            if (!enableChargedShot)
            {
                if (ReadRangedAttackPressed())
                    TryRangedAttack();

                return;
            }

            if (chargedShotCharging)
            {
                TickActiveChargedShot();
                return;
            }

            if (rangedPressPending)
            {
                TickPendingRangedPress();
                return;
            }

            if (!ReadRangedAttackPressed())
                return;

            if (Time.time < nextRangedAllowedAt ||
                reloading)
            {
                return;
            }

            if (rangedAmmo <= 0)
            {
                BeginReloadIfNeeded();
                return;
            }

            if (rangedAmmo == 1)
            {
                TryRangedAttack();
                return;
            }

            BeginPendingRangedPress();
        }


        private void BeginPendingRangedPress()
        {
            rangedPressPending = true;
            rangedPressStartedAtUnscaled = Time.unscaledTime;
            lastCombatAction = "ranged press pending";
        }

        private void TickPendingRangedPress()
        {
            if (!rangedPressPending)
                return;

            if (reloading || rangedAmmo <= 0)
            {
                ClearPendingRangedPress();
                BeginReloadIfNeeded();
                return;
            }

            float heldDuration =
                Time.unscaledTime -
                rangedPressStartedAtUnscaled;

            if (ReadRangedAttackReleased() ||
                !ReadRangedAttackHeld())
            {
                ClearPendingRangedPress();
                TryRangedAttack();
                return;
            }

            if (heldDuration <
                Mathf.Max(0.05f, chargedShotHoldThreshold))
            {
                return;
            }

            ClearPendingRangedPress();
            BeginChargedShot();
        }

        private void ClearPendingRangedPress()
        {
            rangedPressPending = false;
            rangedPressStartedAtUnscaled = 0f;
        }

        private void BeginChargedShot()
        {
            chargedShotCharging = true;
            chargedShotStartedAtUnscaled = Time.unscaledTime;
            chargedShotReservedAmmo = Mathf.Max(2, rangedAmmo);

            int additionalAmmo =
                Mathf.Max(0, chargedShotReservedAmmo - 2);

            chargedShotRequiredDuration =
                Mathf.Min(
                    Mathf.Max(
                        0.10f,
                        chargedShotMaximumDuration
                    ),
                    Mathf.Max(
                        0.10f,
                        chargedShotBaseDuration
                    ) +
                    additionalAmmo *
                    Mathf.Max(
                        0f,
                        chargedShotSecondsPerAdditionalAmmo
                    )
                );

            Vector3 direction = GetCombatAimDirection();
            ApplyCombatFacing(direction);

            chargedShotChargeVisual =
                BDChargedShotChargeVisual.Spawn(
                    transform,
                    direction,
                    chargedShotReservedAmmo,
                    chargedShotRequiredDuration
                );

            lastCombatAction =
                $"charging {chargedShotReservedAmmo} ammo " +
                $"{chargedShotRequiredDuration:0.00}s";
        }

        private void TickActiveChargedShot()
        {
            if (reloading || rangedAmmo <= 0)
            {
                CancelChargedShot();
                return;
            }

            Vector3 direction = GetCombatAimDirection();
            ApplyCombatFacing(direction);

            float progress = ChargedShotProgress01;

            if (chargedShotChargeVisual != null)
            {
                chargedShotChargeVisual.SetCharge(
                    progress,
                    direction
                );
            }

            // Completion is checked before release, so releasing on the
            // exact completion frame still fires the charged projectile.
            if (progress >= 1f)
            {
                FireChargedShot(direction);
                return;
            }

            if (ReadRangedAttackReleased() ||
                !ReadRangedAttackHeld())
            {
                CancelChargedShot();
            }
        }

        private void FireChargedShot(Vector3 direction)
        {
            // Consume every projectile currently left.
            int ammoToConsume = Mathf.Max(0, rangedAmmo);

            if (ammoToConsume <= 0)
            {
                CancelChargedShot();
                BeginReloadIfNeeded();
                return;
            }

            nextRangedAllowedAt =
                Time.time + Mathf.Max(0.01f, rangedCooldown);

            rangedAmmo -= ammoToConsume;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                direction = GetCombatAimDirection();

            if (direction.sqrMagnitude < 0.001f)
                direction = transform.forward;

            direction.Normalize();
            ApplyCombatFacing(direction);

            Vector3 spawnPosition =
                transform.position +
                direction * rangedSpawnForwardOffset;

            spawnPosition.y = ResolveProjectileSpawnY();

            float extraAmmo = Mathf.Max(0f, ammoToConsume - 1f);
            float projectileScaleMultiplier =
                1f +
                extraAmmo *
                Mathf.Max(
                    0f,
                    chargedProjectileScalePerExtraAmmo
                );

            float projectileSpeedMultiplier =
                1f +
                extraAmmo *
                Mathf.Max(
                    0f,
                    chargedSpeedPerExtraAmmo
                );

            float projectileHitRadiusMultiplier =
                1f +
                extraAmmo *
                Mathf.Max(
                    0f,
                    chargedHitRadiusPerExtraAmmo
                );

            float projectileKnockbackMultiplier =
                1f +
                extraAmmo *
                Mathf.Max(
                    0f,
                    chargedKnockbackPerExtraAmmo
                );

            GameObject projectile =
                GameObject.CreatePrimitive(PrimitiveType.Sphere);

            projectile.name =
                $"BD_Player_Charged_Projectile_x{ammoToConsume}";

            projectile.transform.position = spawnPosition;
            projectile.transform.rotation =
                Quaternion.LookRotation(direction, Vector3.up);
            projectile.transform.localScale =
                Vector3.one *
                0.55f *
                projectileScaleMultiplier;

            Renderer renderer = projectile.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material projectileMaterial =
                    CreateProjectileMaterial();

                if (projectileMaterial != null)
                    renderer.sharedMaterial = projectileMaterial;
            }

            Collider collider = projectile.GetComponent<Collider>();

            if (collider != null)
                Destroy(collider);

            BDPlayerRangedProjectile projectileLogic =
                projectile.AddComponent<BDPlayerRangedProjectile>();

            projectileLogic.Configure(
                direction,
                rangedProjectileSpeed *
                    projectileSpeedMultiplier,
                rangedDamage *
                    WeaponDamageMultiplier *
                    ammoToConsume,
                rangedProjectileLifetime,
                rangedProjectileHitRadius *
                    projectileHitRadiusMultiplier,
                rangedProjectileKnockback *
                    projectileKnockbackMultiplier,
                transform
            );

            BDRangedAttackVisuals.AddProjectileTrail(
                projectile,
                playerProjectile: true
            );

            BDChargedProjectileVisual.Attach(
                projectile,
                ammoToConsume
            );

            BDRangedAttackVisuals.SpawnMuzzleFlash(
                spawnPosition,
                direction,
                playerProjectile: true
            );

            BDChargedShotVisualUtility.SpawnChargedMuzzleBurst(
                spawnPosition,
                direction,
                ammoToConsume
            );

            if (chargedShotChargeVisual != null)
                chargedShotChargeVisual.ReleaseToProjectile();

            chargedShotChargeVisual = null;
            chargedShotCharging = false;
            chargedShotRequiredDuration = 0f;
            chargedShotReservedAmmo = 0;

            BDGameFeelAudio.PlayRangedShot();
            BDGameFeelEvents.RequestCameraShake(
                Mathf.Clamp(
                    0.10f + ammoToConsume * 0.045f,
                    0.16f,
                    0.42f
                ),
                0.14f
            );

            lastCombatAction =
                $"charged shot x{ammoToConsume} " +
                $"ammo={rangedAmmo}";

            StartReloadImmediatelyAfterChargedShot();
        }


        private void StartReloadImmediatelyAfterChargedShot()
        {
            rangedAmmo = 0;
            reloading = false;

            StartReloadFromEmptyMagazine(
                "charged shot emptied magazine"
            );
        }

        private void CancelChargedShot()
        {
            if (!chargedShotCharging &&
                chargedShotChargeVisual == null)
            {
                return;
            }

            if (chargedShotChargeVisual != null)
                chargedShotChargeVisual.CancelCharge();

            chargedShotChargeVisual = null;
            chargedShotCharging = false;
            chargedShotStartedAtUnscaled = 0f;
            chargedShotRequiredDuration = 0f;
            chargedShotReservedAmmo = 0;
            lastCombatAction = "charged shot cancelled";
        }

        private void TryRangedAttack()
        {
            if (Time.time < nextRangedAllowedAt)
                return;

            if (reloading)
                return;

            if (rangedAmmo <= 0)
            {
                BeginReloadIfNeeded();
                return;
            }

            nextRangedAllowedAt = Time.time + Mathf.Max(0.01f, rangedCooldown);
            rangedAmmo--;

            Vector3 direction = GetCombatAimDirection();

            ApplyCombatFacing(direction);

            Vector3 spawnPosition = transform.position + direction * rangedSpawnForwardOffset;
            spawnPosition.y = ResolveProjectileSpawnY();

            GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectile.name = "BD_Player_Ranged_Projectile";
            projectile.transform.position = spawnPosition;
            projectile.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            projectile.transform.localScale = Vector3.one * 0.55f;

            Renderer renderer = projectile.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material projectileMaterial = CreateProjectileMaterial();
                if (projectileMaterial != null)
                    renderer.sharedMaterial = projectileMaterial;
            }

            Collider collider = projectile.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            BDPlayerRangedProjectile projectileLogic = projectile.AddComponent<BDPlayerRangedProjectile>();
            projectileLogic.Configure(
                direction,
                rangedProjectileSpeed,
                rangedDamage * WeaponDamageMultiplier,
                rangedProjectileLifetime,
                rangedProjectileHitRadius,
                rangedProjectileKnockback,
                transform
            );

            BDRangedAttackVisuals.AddProjectileTrail(projectile, playerProjectile: true);
            BDRangedAttackVisuals.SpawnMuzzleFlash(spawnPosition, direction, playerProjectile: true);

            BDGameFeelAudio.PlayRangedShot();
            lastCombatAction = $"ranged fired ammo={rangedAmmo}";

            if (rangedAmmo <= 0)
                BeginReloadIfNeeded();
        }

        private Vector3 GetCombatAimDirection()
        {
            // Combat aiming is intentionally stricter than movement aiming.
            // Movement may use camera/model rules, but attacks should go where the mouse points.
            if (attacksFollowMousePoint && TryGetMousePointCombatAim(out Vector3 mouseAim))
                return mouseAim;

            if (TryGetControllerFacingAim(out Vector3 controllerAim))
                return controllerAim;

            Vector3 forward = transform.forward;
            forward.y = 0f;

            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.forward;

            return forward.normalized;
        }

        private bool TryGetMousePointCombatAim(out Vector3 aim)
        {
            aim = Vector3.zero;

            Camera camera = Camera.main;
            if (camera == null)
                return false;

            Vector2 mousePosition;

#if ENABLE_INPUT_SYSTEM
            Mouse mouse = Mouse.current;
            if (mouse == null)
                return false;

            mousePosition = mouse.position.ReadValue();
#else
            mousePosition = Input.mousePosition;
#endif

            Ray ray = camera.ScreenPointToRay(mousePosition);

            // Use the player's current ground level as the combat aim plane.
            // This makes melee/ranged aim match the visible mouse point without changing movement.
            Plane playerGroundPlane = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));

            if (!playerGroundPlane.Raycast(ray, out float enter))
                return false;

            if (enter < 0f || enter > combatMouseAimMaxRayDistance)
                return false;

            Vector3 target = ray.GetPoint(enter);
            Vector3 direction = target - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < combatMouseAimMinDistance * combatMouseAimMinDistance)
                return false;

            aim = direction.normalized;
            return true;
        }

        private void ApplyCombatFacing(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            // Mounted combat rule:
            // Shooting while mounted aims at the mouse point, but it must NOT rotate the horse
            // and must NOT disturb the mounted movement/facing state.
            if (IsMountedOnHorse())
            {
                lastCombatAction = "mounted ranged mouse aim no turn";
                return;
            }

            // On foot, attacks should rotate the player to face the attack direction.
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = targetRotation;
        }

        private bool TryGetControllerFacingAim(out Vector3 aim)
        {
            aim = Vector3.zero;

            // Mounted priority:
            // While riding, the player controller may be disabled or stale.
            // The horse owns the active facing aim, so ranged attacks must use it first.
            BDHorseController horse = FindFirstObjectByType<BDHorseController>();
            if (horse != null && horse.IsMounted && horse.Rider != null)
            {
                if (horse.Rider == transform || transform.IsChildOf(horse.Rider) || horse.Rider.IsChildOf(transform))
                {
                    aim = horse.LastMountedAimDirection;
                    aim.y = 0f;

                    if (aim.sqrMagnitude > 0.001f)
                    {
                        aim.Normalize();
                        return true;
                    }

                    Vector3 horseForward = horse.transform.forward;
                    horseForward.y = 0f;

                    if (horseForward.sqrMagnitude > 0.001f)
                    {
                        aim = horseForward.normalized;
                        return true;
                    }
                }
            }

            // On foot: use player controller facing aim.
            BDPlayerController playerController = GetComponent<BDPlayerController>();
            if (playerController != null && playerController.LastLookDirection.sqrMagnitude > 0.001f)
            {
                aim = playerController.LastLookDirection;
                aim.y = 0f;

                if (aim.sqrMagnitude > 0.001f)
                {
                    aim.Normalize();
                    return true;
                }
            }

            return false;
        }

        private float ResolveProjectileSpawnY()
        {
            Vector3 origin = transform.position + Vector3.up * 4f;
            RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, 20f, ~0, QueryTriggerInteraction.Ignore);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;

                if (hit.normal.y < 0.35f)
                    continue;

                if (hit.collider.GetComponentInParent<BDPlayerMarker>() != null)
                    continue;

                if (hit.collider.GetComponentInParent<BDHorseHealth>() != null)
                    continue;

                if (hit.collider.GetComponentInParent<BDHealth>() != null)
                    continue;

                if (hit.collider.GetComponentInParent<BDMinimapRoom>() != null)
                    continue;

                return hit.point.y + rangedSpawnHeight;
            }

            return rangedSpawnHeight;
        }

        private Material CreateProjectileMaterial()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            if (shader == null)
                shader = Shader.Find("Sprites/Default");

            if (shader == null)
                shader = Shader.Find("Hidden/InternalErrorShader");

            Material material = null;

            if (shader != null)
            {
                material = new Material(shader);
            }
            else
            {
                Material builtIn = Resources.GetBuiltinResource<Material>("Default-Material.mat");
                if (builtIn != null)
                    material = new Material(builtIn);
            }

            // Last-resort safety: return null instead of throwing. Caller keeps default primitive material.
            if (material == null)
                return null;

            material.color = new Color(0.65f, 0.92f, 1f, 1f);

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", new Color(0.4f, 0.9f, 1f, 1f));
            }

            return material;
        }

        public void ApplyBoostModifiers(
            int additionalMagazineCapacity,
            float reloadDurationReduction,
            float weaponDamageMultiplier,
            float minimumReloadDuration,
            bool grantAddedAmmo)
        {
            int previousMagazineSize = RangedMagazineSize;

            boostAdditionalMagazineCapacity =
                Mathf.Max(0, additionalMagazineCapacity);
            boostReloadDurationReduction =
                Mathf.Max(0f, reloadDurationReduction);
            boostWeaponDamageMultiplier =
                Mathf.Max(0.01f, weaponDamageMultiplier);
            boostMinimumReloadDuration =
                Mathf.Max(0.1f, minimumReloadDuration);

            int updatedMagazineSize = RangedMagazineSize;

            if (grantAddedAmmo &&
                updatedMagazineSize > previousMagazineSize)
            {
                int gainedCapacity =
                    updatedMagazineSize - previousMagazineSize;

                rangedAmmo = Mathf.Min(
                    updatedMagazineSize,
                    rangedAmmo + gainedCapacity
                );
            }
            else
            {
                rangedAmmo =
                    Mathf.Min(rangedAmmo, updatedMagazineSize);
            }

            if (reloading)
            {
                reloadEndsAt = Mathf.Min(
                    reloadEndsAt,
                    Time.time + EffectiveRangedReloadDuration
                );
            }
        }

        private bool ReadLightAttackPressed()
        {
#if ENABLE_INPUT_SYSTEM
            Mouse mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame)
                return true;

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.jKey.wasPressedThisFrame)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
                return true;
#endif

            return false;
        }

        private bool ReadHeavyAttackPressed()
        {
#if ENABLE_INPUT_SYSTEM
            Mouse mouse = Mouse.current;
            if (mouse != null && mouse.rightButton.wasPressedThisFrame)
                return true;

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.kKey.wasPressedThisFrame)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K))
                return true;
#endif

            return false;
        }

        private bool ReadRangedAttackPressed()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.qKey.wasPressedThisFrame)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Q))
                return true;
#endif

            return false;
        }

        private bool ReadRangedAttackHeld()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;

            if (keyboard != null && keyboard.qKey.isPressed)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.Q))
                return true;
#endif

            return false;
        }

        private bool ReadRangedAttackReleased()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;

            if (keyboard != null &&
                keyboard.qKey.wasReleasedThisFrame)
            {
                return true;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyUp(KeyCode.Q))
                return true;
#endif

            return false;
        }

        private void OnGUI()
        {
            if (!showCombatDebug)
                return;

            GUI.Box(new Rect(12, 210, 440, 118), "B&D Combat");
            GUI.Label(new Rect(24, 240, 410, 22), $"Last: {lastCombatAction}");
            GUI.Label(new Rect(24, 262, 410, 22), $"Ranged ammo: {rangedAmmo} / {RangedMagazineSize}");
            GUI.Label(new Rect(24, 284, 410, 22), $"Reloading: {reloading}");
            GUI.Label(new Rect(24, 306, 410, 22), $"Aim: {GetCombatAimDirection().x:0.00}, {GetCombatAimDirection().z:0.00}");
        }
    }
}
