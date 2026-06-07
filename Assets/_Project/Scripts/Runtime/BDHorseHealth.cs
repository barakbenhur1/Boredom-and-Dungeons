using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDHorseHealth : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float faintThreshold = 1f;
        [SerializeField] private bool logDamage = true;
        [Header("Clean Game Start")]
        [SerializeField] private float startupDamageProtectionSeconds = 2.50f;
        private float startupDamageProtectionUntil = -999f;

        [Header("Hit Burst / Buck")]
        [SerializeField] private int hitsToTriggerBuck = 2;
        [SerializeField] private float hitBurstWindow = 2.2f;
        [SerializeField] private float buckCooldown = 2.5f;

        private float currentHealth;
        private float healingProtectionUntil;
        private bool healingFloorLockActive;
        private float healingLockedFloor;
        private int recentHits;
        private float burstWindowStartedAt = -999f;
        private float lastBuckAt = -999f;

        public event Action<BDHorseHealth> Fainted;
        public event Action<BDHorseHealth> Recovered;
        public event Action<BDHorseHealth, float, float> HealthChanged;
        public event Action<BDHorseHealth> DamageBurstTriggered;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsFainted => currentHealth <= faintThreshold;
        public float Injury01 => 1f - Mathf.Clamp01(currentHealth / maxHealth);
        public int RecentHits => recentHits;
        public bool IsHealingProtected => Time.time < healingProtectionUntil;
        public bool HasHealingFloorLock => healingFloorLockActive;
        public bool IsStartupProtected =>
            Time.unscaledTime <
            startupDamageProtectionUntil;

        private void Awake()
        {
            currentHealth = maxHealth;

            startupDamageProtectionUntil =
                Time.unscaledTime +
                Mathf.Max(
                    0f,
                    startupDamageProtectionSeconds
                );
            EnsureHealAvailabilityIndicator();
        }

        private void EnsureHealAvailabilityIndicator()
        {
            if (!Application.isPlaying)
                return;

            if (GetComponent<BDHorseHealAvailabilityIndicator>() != null)
                return;

            gameObject.AddComponent<BDHorseHealAvailabilityIndicator>();
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


        // BD HORSE CLEAN START V1
        public void ResetForCleanGameStart(
            float protectionSeconds)
        {
            currentHealth = maxHealth;
            recentHits = 0;
            burstWindowStartedAt = -999f;
            lastBuckAt = -999f;
            healingProtectionUntil = -999f;
            healingFloorLockActive = false;
            healingLockedFloor = 0f;

            startupDamageProtectionUntil =
                Time.unscaledTime +
                Mathf.Max(
                    0f,
                    Mathf.Max(
                        startupDamageProtectionSeconds,
                        protectionSeconds
                    )
                );

            HealthChanged?.Invoke(
                this,
                currentHealth,
                maxHealth
            );
        }

        public void BeginHealingProtection(float duration)
        {
            healingProtectionUntil = Mathf.Max(healingProtectionUntil, Time.time + Mathf.Max(0f, duration));
        }


        public void LockHealingFloor(float floor)
        {
            healingLockedFloor = Mathf.Clamp(floor, faintThreshold, maxHealth);
            healingFloorLockActive = true;
        }

        public void ClearHealingFloorLock()
        {
            healingFloorLockActive = false;
            healingLockedFloor = 0f;
        }

        public void ApplyDamage(float amount)
        {
            if (BDHorseCleanRunStartGuard.IsActive ||
                IsStartupProtected)
            {
                if (logDamage)
                {
                    Debug.Log(
                        $"{name} horse startup damage ignored: " +
                        $"{Mathf.Abs(amount):0.0}"
                    );
                }

                return;
            }


            if (IsHealingProtected)
            {
                if (logDamage)
                    Debug.Log($"{name} horse damage ignored during healing protection.");
                return;
            }

            if (healingFloorLockActive)
            {
                if (currentHealth < healingLockedFloor)
                {
                    currentHealth = healingLockedFloor;
                    HealthChanged?.Invoke(this, currentHealth, maxHealth);
                }

                if (logDamage)
                    Debug.Log($"{name} horse damage ignored while healing floor is locked.");
                return;
            }

            float before = currentHealth;
            float damage = Mathf.Abs(amount);

            currentHealth = Mathf.Max(faintThreshold, currentHealth - damage);

            RegisterRecentHit();

            if (logDamage)
            {
                Debug.Log($"{name} horse took {damage:0.0} damage. HP {currentHealth:0.0}/{maxHealth:0.0}. Recent hits: {recentHits}");
            }

            HealthChanged?.Invoke(this, currentHealth, maxHealth);

            if (before > faintThreshold && currentHealth <= faintThreshold)
                Fainted?.Invoke(this);
        }

        public void Heal(float amount)
        {
            bool wasFainted = IsFainted;

            currentHealth = Mathf.Min(maxHealth, currentHealth + Mathf.Abs(amount));
            HealthChanged?.Invoke(this, currentHealth, maxHealth);

            if (wasFainted && !IsFainted)
                Recovered?.Invoke(this);
        }

        private void RegisterRecentHit()
        {
            float now = Time.time;

            if (now - burstWindowStartedAt > hitBurstWindow)
            {
                burstWindowStartedAt = now;
                recentHits = 0;
            }

            recentHits++;

            if (recentHits >= hitsToTriggerBuck && now - lastBuckAt >= buckCooldown)
            {
                lastBuckAt = now;
                recentHits = 0;
                burstWindowStartedAt = now;
                DamageBurstTriggered?.Invoke(this);
            }
        }
    }
}
