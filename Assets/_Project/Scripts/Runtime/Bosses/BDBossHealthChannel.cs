using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    public enum BDBossZeroHealthPolicy
    {
        KnockOut = 0,
        StayActiveAndIgnoreFurtherDamage = 1,
        DieImmediately = 2
    }

    [DisallowMultipleComponent]
    public sealed class BDBossHealthChannel : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] private string channelId = "boss";
        [SerializeField] private string displayName = "Boss";

        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private BDBossZeroHealthPolicy zeroHealthPolicy = BDBossZeroHealthPolicy.DieImmediately;
        [SerializeField] private bool refillOnAwake = true;

        private float currentHealth;
        private BDBossLifeState lifeState = BDBossLifeState.Alive;

        public event Action<BDBossHealthChannel, float, float> HealthChanged;
        public event Action<BDBossHealthChannel, BDBossLifeState> LifeStateChanged;
        public event Action<BDBossHealthChannel> ReachedZero;

        public string ChannelId => channelId;
        public string DisplayName => displayName;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public float NormalizedHealth => maxHealth <= 0f ? 0f : Mathf.Clamp01(currentHealth / maxHealth);
        public BDBossLifeState LifeState => lifeState;
        public bool IsAtZero => currentHealth <= 0f;
        public bool AcceptsDamage => lifeState == BDBossLifeState.Alive && currentHealth > 0f;

        private void Awake()
        {
            if (refillOnAwake)
                ResetHealth(maxHealth);
            else
                currentHealth = Mathf.Clamp(currentHealth, 0f, Mathf.Max(1f, maxHealth));
        }

        public void Configure(string id, string label, float health, BDBossZeroHealthPolicy policy, bool refill)
        {
            channelId = string.IsNullOrWhiteSpace(id) ? "boss" : id.Trim();
            displayName = string.IsNullOrWhiteSpace(label) ? channelId : label.Trim();
            maxHealth = Mathf.Max(1f, health);
            zeroHealthPolicy = policy;

            if (refill)
                ResetHealth(maxHealth);
            else
                currentHealth = Mathf.Min(currentHealth, maxHealth);
        }

        public bool ApplyDamage(float amount)
        {
            if (!AcceptsDamage)
                return false;

            float damage = Mathf.Abs(amount);
            if (damage <= 0f)
                return false;

            currentHealth = Mathf.Max(0f, currentHealth - damage);
            HealthChanged?.Invoke(this, currentHealth, maxHealth);

            if (currentHealth <= 0f)
                ResolveZeroHealth();

            return true;
        }

        public void Heal(float amount)
        {
            if (lifeState != BDBossLifeState.Alive || currentHealth <= 0f)
                return;

            currentHealth = Mathf.Min(maxHealth, currentHealth + Mathf.Abs(amount));
            HealthChanged?.Invoke(this, currentHealth, maxHealth);
        }

        public void ResetHealth(float newMaxHealth)
        {
            maxHealth = Mathf.Max(1f, newMaxHealth);
            currentHealth = maxHealth;
            SetLifeState(BDBossLifeState.Alive);
            HealthChanged?.Invoke(this, currentHealth, maxHealth);
        }

        public void RestoreForNextPhase(float newMaxHealth)
        {
            ResetHealth(newMaxHealth);
        }

        public void MarkDead()
        {
            currentHealth = 0f;
            HealthChanged?.Invoke(this, currentHealth, maxHealth);
            SetLifeState(BDBossLifeState.Dead);
        }

        private void ResolveZeroHealth()
        {
            ReachedZero?.Invoke(this);

            switch (zeroHealthPolicy)
            {
                case BDBossZeroHealthPolicy.KnockOut:
                    SetLifeState(BDBossLifeState.KnockedOut);
                    break;

                case BDBossZeroHealthPolicy.StayActiveAndIgnoreFurtherDamage:
                    SetLifeState(BDBossLifeState.CriticalAtZero);
                    break;

                default:
                    SetLifeState(BDBossLifeState.Dead);
                    break;
            }
        }

        private void SetLifeState(BDBossLifeState next)
        {
            if (lifeState == next)
                return;

            lifeState = next;
            LifeStateChanged?.Invoke(this, lifeState);
        }
    }
}
