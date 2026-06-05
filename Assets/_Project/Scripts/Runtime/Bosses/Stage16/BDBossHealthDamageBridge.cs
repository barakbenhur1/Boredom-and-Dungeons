using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(100)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHealth))]
    [RequireComponent(typeof(BDBossHealthChannel))]
    public sealed class BDBossHealthDamageBridge : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BDHealth sourceHealth;
        [SerializeField] private BDBossHealthChannel channel;

        [Header("Channel Setup")]
        [SerializeField] private string channelId = "boss";
        [SerializeField] private string displayName = "Boss";
        [SerializeField] private BDBossZeroHealthPolicy zeroHealthPolicy =
            BDBossZeroHealthPolicy.DieImmediately;
        [SerializeField] private bool configureChannelFromSourceOnAwake = true;
        [SerializeField] private bool synchronizeHealing = true;

        private float previousSourceHealth;
        private bool initialized;

        private void Awake()
        {
            ResolveReferences();

            if (sourceHealth == null || channel == null)
                return;

            if (configureChannelFromSourceOnAwake)
            {
                channel.Configure(
                    channelId,
                    displayName,
                    sourceHealth.MaxHealth,
                    zeroHealthPolicy,
                    refill: true
                );
            }

            previousSourceHealth = sourceHealth.CurrentHealth;
            initialized = true;
        }

        private void OnEnable()
        {
            ResolveReferences();

            if (sourceHealth == null)
                return;

            sourceHealth.HealthChanged -= HandleSourceHealthChanged;
            sourceHealth.HealthChanged += HandleSourceHealthChanged;

            sourceHealth.Died -= HandleSourceDied;
            sourceHealth.Died += HandleSourceDied;
        }

        private void OnDisable()
        {
            if (sourceHealth == null)
                return;

            sourceHealth.HealthChanged -= HandleSourceHealthChanged;
            sourceHealth.Died -= HandleSourceDied;
        }

        public void Configure(
            string id,
            string label,
            BDBossZeroHealthPolicy policy)
        {
            channelId = string.IsNullOrWhiteSpace(id) ? "boss" : id;
            displayName = string.IsNullOrWhiteSpace(label)
                ? channelId
                : label;
            zeroHealthPolicy = policy;

            ResolveReferences();

            if (sourceHealth != null && channel != null)
            {
                channel.Configure(
                    channelId,
                    displayName,
                    sourceHealth.MaxHealth,
                    zeroHealthPolicy,
                    refill: true
                );

                previousSourceHealth = sourceHealth.CurrentHealth;
                initialized = true;
            }
        }

        private void ResolveReferences()
        {
            if (sourceHealth == null)
                sourceHealth = GetComponent<BDHealth>();

            if (channel == null)
                channel = GetComponent<BDBossHealthChannel>();
        }

        private void HandleSourceHealthChanged(
            BDHealth health,
            float current,
            float maximum)
        {
            if (channel == null)
                return;

            if (!initialized)
            {
                previousSourceHealth = current;
                initialized = true;
                return;
            }

            float delta = previousSourceHealth - current;

            if (delta > 0.0001f)
            {
                channel.ApplyDamage(delta);
            }
            else if (delta < -0.0001f && synchronizeHealing)
            {
                channel.Heal(-delta);
            }

            previousSourceHealth = current;
        }

        private void HandleSourceDied(BDHealth health)
        {
            if (channel == null || channel.IsAtZero)
                return;

            channel.ApplyDamage(channel.CurrentHealth);
        }
    }
}
