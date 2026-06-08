using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDEnemyDeathFeedback : MonoBehaviour
    {
        [SerializeField] private bool spawnDeathBurst = true;
        [SerializeField] private bool notifyCombatBus = true;
        [SerializeField] private float deathCameraShakeStrength = 0.12f;
        [SerializeField] private float deathCameraShakeDuration = 0.12f;
        [SerializeField] private float deathHitStopDuration = 0.020f;
        [SerializeField] private float deathHitStopTimeScale = 0.58f;

        private BDHealth health;
        private bool handled;

        private void Awake()
        {
            health = GetComponent<BDHealth>();

            if (health != null)
                health.Died += OnDied;
        }

        private void OnDied(BDHealth deadHealth)
        {
            if (handled)
                return;

            handled = true;

            BDCharacterDeathAnimation.PlayEnemyDeath(deadHealth);

            if (spawnDeathBurst)
                BDDeathBurstVisual.Spawn(transform.position);

            if (notifyCombatBus)
                BDCombatEventBus.NotifyEnemyKilled(deadHealth);

            BDGameFeelEvents.RequestCameraShake(deathCameraShakeStrength, deathCameraShakeDuration);
            BDHitStop.Request(deathHitStopDuration, deathHitStopTimeScale);
            BDGameFeelAudio.PlayEnemyDeath();
        }

        private void OnDestroy()
        {
            if (health != null)
                health.Died -= OnDied;
        }
    }
}
