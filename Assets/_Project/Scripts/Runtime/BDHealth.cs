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

            RequestDamageCameraShake();
            HealthChanged?.Invoke(this, currentHealth, maxHealth);
        }

        public void ApplyDamage(float amount)
        {
            if (IsDead)
                return;

            if (TryCancelPlayerDamageWithParry())
                return;

            if (ShouldIgnoreDamageDuringDodge())
                return;

            float damage = Mathf.Abs(amount);
            currentHealth = Mathf.Max(0f, currentHealth - damage);

            if (logDamage)
                Debug.Log($"{name} took {damage:0.0} damage. HP {currentHealth:0.0}/{maxHealth:0.0}");

            RequestDamageCameraShake();
            HealthChanged?.Invoke(this, currentHealth, maxHealth);

            if (currentHealth <= 0f)
            {
                Died?.Invoke(this);

                if (destroyOnDeath)
                    Destroy(gameObject);
            }
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
