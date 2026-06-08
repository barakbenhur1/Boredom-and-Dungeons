using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private bool destroyOnDeath = false;
        [SerializeField] private bool logDamage = true;
        [SerializeField] private bool autoAddDamageFlashFeedback = true;
        [SerializeField] private bool autoAddEnemyWorldHealthBar = true;
        [SerializeField] private bool ignorePlayerDamageDuringDodge = true;
        [SerializeField] private float playerDamageCameraShakeStrength = 0.18f;
        [SerializeField] private float playerDamageCameraShakeDuration = 0.12f;
        [SerializeField] private float horseDamageCameraShakeStrength = 0.12f;
        [SerializeField] private float horseDamageCameraShakeDuration = 0.10f;

        private float currentHealth;

        public event Action<BDHealth> Died;
        public event Action<BDHealth, float, float> HealthChanged;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsDead => currentHealth <= 0f;

        private void Awake()
        {
            currentHealth = maxHealth;
            EnsureDamageFlashFeedback();
            EnsureEnemyDeathFeedback();
            EnsureCharacterDeathAnimation();
            EnsureEnemyWorldHealthBar();
            EnsureEnemyAttackTelegraph();
            EnsureEnemyProximityTelegraph();
            EnsureEnemyHitStaggerReceiver();
            EnsureEnemyHitFlashReceiver();
        }

        private void EnsureDamageFlashFeedback()
        {
            if (!autoAddDamageFlashFeedback)
                return;

            if (GetComponent<BDDamageFlashFeedback>() != null)
                return;

            Renderer[] renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
            if (renderers == null || renderers.Length == 0)
                return;

            gameObject.AddComponent<BDDamageFlashFeedback>();
        }

        private void EnsureEnemyHitFlashReceiver()
        {
            if (GetComponent<BDEnemyHitFlashReceiver>() != null)
                return;

            if (GetComponent<BDPlayerMarker>() != null)
                return;

            if (GetComponent<BDHorseHealth>() != null)
                return;

            if (GetComponent<CharacterController>() == null)
                return;

            gameObject.AddComponent<BDEnemyHitFlashReceiver>();
        }

        private void EnsureEnemyHitStaggerReceiver()
        {
            if (GetComponent<BDHitStaggerReceiver>() != null)
                return;

            if (GetComponent<BDPlayerMarker>() != null)
                return;

            if (GetComponent<BDHorseHealth>() != null)
                return;

            if (GetComponent<CharacterController>() == null)
                return;

            gameObject.AddComponent<BDHitStaggerReceiver>();
        }

        private void EnsureEnemyProximityTelegraph()
        {
            if (GetComponent<BDEnemyProximityTelegraph>() != null)
                return;

            if (GetComponent<BDPlayerMarker>() != null)
                return;

            if (GetComponent<BDHorseHealth>() != null)
                return;

            if (GetComponent<CharacterController>() == null)
                return;

            gameObject.AddComponent<BDEnemyProximityTelegraph>();
        }

        private void EnsureEnemyAttackTelegraph()
        {
            if (GetComponent<BDEnemyAttackTelegraph>() != null)
                return;

            if (GetComponent<BDPlayerMarker>() != null)
                return;

            if (GetComponent<BDHorseHealth>() != null)
                return;

            if (GetComponent<CharacterController>() == null)
                return;

            gameObject.AddComponent<BDEnemyAttackTelegraph>();
        }

        private void EnsureEnemyWorldHealthBar()
        {
            if (!autoAddEnemyWorldHealthBar)
                return;

            if (GetComponent<BDEnemyWorldHealthBar>() != null)
                return;

            if (GetComponent<BDPlayerMarker>() != null)
                return;

            if (GetComponent<BDHorseHealth>() != null)
                return;

            if (GetComponent<CharacterController>() == null)
                return;

            gameObject.AddComponent<BDEnemyWorldHealthBar>();
        }

        private void EnsureCharacterDeathAnimation()
        {
            if (GetComponent<BDCharacterDeathAnimation>() != null)
                return;

            if (GetComponent<BDPlayerMarker>() == null &&
                GetComponent<BDEnemyDeathFeedback>() == null)
            {
                return;
            }

            gameObject.AddComponent<BDCharacterDeathAnimation>();
        }

        private void EnsureEnemyDeathFeedback()
        {
            if (GetComponent<BDEnemyDeathFeedback>() != null)
                return;

            if (GetComponent<BDPlayerMarker>() != null)
                return;

            if (GetComponent<BDHorseHealth>() != null)
                return;

            if (GetComponent<CharacterController>() == null)
                return;

            gameObject.AddComponent<BDEnemyDeathFeedback>();
        }

        public void SetMaxHealth(float value, bool refill)
        {
            maxHealth = Mathf.Max(1f, value);

            if (refill)
                currentHealth = maxHealth;
            else
                currentHealth = Mathf.Min(currentHealth, maxHealth);

            HealthChanged?.Invoke(this, currentHealth, maxHealth);
        }

        public void ApplyDamage(float amount)
        {
            // BD ACTUAL COLLIDER DAMAGE ROUTING V13
            // A generic BDHealth attached to the horse delegates only to the
            // horse. A player BDHealth damages only the player. The attack does
            // not automatically damage both mounted actors.
            BDHorseHealth bridgedHorseHealth =
                GetComponent<BDHorseHealth>();
            if (bridgedHorseHealth != null)
            {
                bridgedHorseHealth.ApplyDamage(amount);
                return;
            }

            if (ShouldIgnorePlayerDamageDuringRunIntro() || IsDead)
                return;

            if (TryCancelPlayerDamageWithParry())
                return;

            if (ShouldIgnoreDamageDuringDodge())
                return;

            ApplyResolvedDamage(
                amount,
                unavoidable: false,
                critical: false
            );
        }

        // BD PLAYER SWORD CRITICAL DAMAGE ROUTING V23R15
        public void ApplyPlayerSwordDamage(
            float amount,
            bool critical)
        {
            BDHorseHealth bridgedHorseHealth =
                GetComponent<BDHorseHealth>();
            if (bridgedHorseHealth != null)
            {
                bridgedHorseHealth.ApplyDamage(amount);
                return;
            }

            if (ShouldIgnorePlayerDamageDuringRunIntro() || IsDead)
                return;

            if (TryCancelPlayerDamageWithParry())
                return;

            if (ShouldIgnoreDamageDuringDodge())
                return;

            ApplyResolvedDamage(
                amount,
                unavoidable: false,
                critical: critical
            );
        }

        public void ApplyUnavoidableDamage(float amount)
        {
            BDHorseHealth bridgedHorseHealth =
                GetComponent<BDHorseHealth>();
            if (bridgedHorseHealth != null)
            {
                bridgedHorseHealth.ApplyDamage(amount);
                return;
            }

            if (IsDead)
                return;

            ApplyResolvedDamage(
                amount,
                unavoidable: true,
                critical: false
            );
        }

        private void ApplyResolvedDamage(
            float amount,
            bool unavoidable,
            bool critical)
        {
            float damage = Mathf.Abs(amount);
            if (damage <= 0f)
                return;

            float before = currentHealth;
            currentHealth = Mathf.Max(0f, currentHealth - damage);
            float appliedDamage = before - currentHealth;
            if (appliedDamage <= 0f)
                return;

            if (logDamage)
            {
                string kind = unavoidable
                    ? " unavoidable"
                    : string.Empty;
                Debug.Log(
                    $"{name} took {appliedDamage:0.0}{kind} damage. " +
                    $"HP {currentHealth:0.0}/{maxHealth:0.0}"
                );
            }

            // BD ANIMATED PLAYER/ENEMY DAMAGE NUMBERS V23R14
            BDDamageNumberFeedback.Spawn(
                this,
                appliedDamage,
                critical
            );

            RequestDamageCameraShake();
            HealthChanged?.Invoke(this, currentHealth, maxHealth);

            // A successful hit on the actual rider collider while mounted
            // participates in the same two-hit buck burst as a horse hit.
            NotifyMountedRiderHitForBuck();
            NotifyPlayerCombatImpactForGrounding();

            if (currentHealth > 0f)
                return;

            // BD SYNCHRONOUS LETHAL DEATH PRESENTATION V23R19G
            // Start the visible pose before any menu, loot, Died callback or
            // destruction path can hide the player, large enemy or guardian.
            bool isPlayer = GetComponent<BDPlayerMarker>() != null;
            BDCombatantRank rank =
                BDCombatantProfile.ResolveRank(this);
            float deathAnimationDuration = isPlayer
                ? BDCharacterDeathAnimation.PlayPlayerDeath(this)
                : rank != BDCombatantRank.Boss
                    ? BDCharacterDeathAnimation.PlayEnemyDeath(this)
                    : 0f;

            if (!BDGameFlowSignals.TryHandleDeath(this))
                Died?.Invoke(this);

            if (destroyOnDeath && !isPlayer)
            {
                float delay = Mathf.Max(
                    deathAnimationDuration,
                    BDCharacterDeathAnimation.GetEnemyDeathDuration(this)
                ) + 0.10f;
                Destroy(gameObject, Mathf.Max(0.10f, delay));
            }
        }

        private void NotifyMountedRiderHitForBuck()
        {
            if (GetComponent<BDPlayerMarker>() == null)
                return;

            BDHorseController horse =
                FindFirstObjectByType<BDHorseController>();
            if (horse == null ||
                !horse.IsMounted ||
                horse.Rider == null)
            {
                return;
            }

            Transform riderTransform = horse.Rider;
            bool sameRider =
                riderTransform == transform ||
                transform.IsChildOf(riderTransform) ||
                riderTransform.IsChildOf(transform);

            if (!sameRider)
                return;

            BDHorseHealth horseHealth =
                horse.GetComponent<BDHorseHealth>();
            if (horseHealth != null)
                horseHealth.RegisterMountedRiderHitForBuck();
        }

        // BD PLAYER COMBAT IMPACT GROUNDING NOTIFY V23
        private void NotifyPlayerCombatImpactForGrounding()
        {
            if (GetComponent<BDPlayerMarker>() == null)
                return;

            BDPlayerHazardRecovery recovery =
                GetComponent<BDPlayerHazardRecovery>();
            if (recovery != null)
                recovery.NotifyCombatImpact();
        }

        private bool ShouldIgnorePlayerDamageDuringRunIntro()
        {
            return
                BDMountedRunIntro.IsGameplayInputLocked &&
                GetComponent<BDPlayerMarker>() != null;
        }

        private bool TryCancelPlayerDamageWithParry()
        {
            if (GetComponent<BDPlayerMarker>() == null)
                return false;

            BDPlayerParryState parryState = GetComponent<BDPlayerParryState>();
            return parryState != null && parryState.TryParryIncomingPhysicalDamage();
        }

        private void RequestDamageCameraShake()
        {
            if (!Application.isPlaying)
                return;

            if (BDNewRunFeedbackReset.IsFeedbackSuppressed)
                return;
            if (GetComponent<BDPlayerMarker>() != null)
            {
                BDGameFeelEvents.RequestCameraShake(playerDamageCameraShakeStrength, playerDamageCameraShakeDuration);
                BDGameFeelAudio.PlayDamage();
                return;
            }

            if (GetComponent<BDHorseHealth>() != null)
            {
                BDGameFeelEvents.RequestCameraShake(horseDamageCameraShakeStrength, horseDamageCameraShakeDuration);
                BDGameFeelAudio.PlayDamage();
                return;
            }
        }

        private bool ShouldIgnoreDamageDuringDodge()
        {
            if (!ignorePlayerDamageDuringDodge)
                return false;

            if (GetComponent<BDPlayerMarker>() == null)
                return false;

            BDPlayerController playerController = GetComponent<BDPlayerController>();
            return playerController != null && playerController.IsDodgeInvulnerable;
        }

        public void Heal(float amount)
        {
            if (IsDead)
                return;

            currentHealth = Mathf.Min(maxHealth, currentHealth + Mathf.Abs(amount));
            HealthChanged?.Invoke(this, currentHealth, maxHealth);
        }
    }
}
