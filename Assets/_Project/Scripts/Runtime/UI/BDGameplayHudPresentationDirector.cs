using UnityEngine;

namespace BoredomAndDungeons
{
    /// <summary>
    /// Owns contextual visibility and fade timing for persistent gameplay HUD
    /// widgets. Presenters remain responsible for drawing their own content.
    /// </summary>
    [DefaultExecutionOrder(160)]
    [DisallowMultipleComponent]
    public sealed class BDGameplayHudPresentationDirector : MonoBehaviour
    {
        [Header("Visibility Timing")]
        [SerializeField, Min(0.10f)]
        private float healthEventHoldSeconds = 2.75f;

        [SerializeField, Min(0.05f)]
        private float fadeInSeconds = 0.16f;

        [SerializeField, Min(0.05f)]
        private float fadeOutSeconds = 0.28f;

        [SerializeField, Min(0.5f)]
        private float horseProximityRange = 3.75f;

        private BDHealth playerHealth;
        private BDHorseHealth horseHealth;
        private BDPlayerCombat playerCombat;
        private BDPlayerController playerController;
        private BDHorseController horseController;
        private Transform playerTransform;

        private float playerHealthAlpha;
        private float horseHealthAlpha;
        private float ammoAlpha;
        private float playerHealthForcedUntil = -999f;
        private float previousPlayerHealth;
        private bool playerHealthSnapshotReady;

        public float PlayerHealthAlpha => playerHealthAlpha;
        public float HorseHealthAlpha => horseHealthAlpha;
        public float AmmoAlpha => ammoAlpha;
        public float StatusPanelAlpha =>
            Mathf.Max(playerHealthAlpha, horseHealthAlpha);

        public bool IsPlayerStationary { get; private set; }
        public bool IsPlayerNearHorse { get; private set; }

        public void Bind(
            BDHealth newPlayerHealth,
            BDHorseHealth newHorseHealth,
            BDPlayerCombat newPlayerCombat,
            BDPlayerController newPlayerController,
            BDHorseController newHorseController)
        {
            if (playerHealth != newPlayerHealth)
            {
                UnsubscribePlayerHealth();
                playerHealth = newPlayerHealth;
                SubscribePlayerHealth();
            }

            horseHealth = newHorseHealth;
            playerCombat = newPlayerCombat;
            playerController = newPlayerController;
            horseController = newHorseController;

            if (playerController != null)
                playerTransform = playerController.transform;
            else if (playerHealth != null)
                playerTransform = playerHealth.transform;
        }

        private void OnDisable()
        {
            playerHealthAlpha = 0f;
            horseHealthAlpha = 0f;
            ammoAlpha = 0f;
        }

        private void OnDestroy()
        {
            UnsubscribePlayerHealth();
        }

        private void Update()
        {
            bool globallyVisible =
                BDGameplayUiVisibility.IsGameplayHudVisible;

            IsPlayerStationary =
                globallyVisible && ResolvePlayerStationary();
            IsPlayerNearHorse =
                globallyVisible && ResolvePlayerNearHorse();

            bool forcePlayerHealth =
                Time.unscaledTime < playerHealthForcedUntil;

            bool showPlayerHealth =
                globallyVisible &&
                playerHealth != null &&
                (IsPlayerStationary ||
                 forcePlayerHealth ||
                 playerHealth.IsDead);

            bool showHorseHealth =
                globallyVisible &&
                horseHealth != null &&
                IsPlayerNearHorse;

            bool showAmmo =
                globallyVisible &&
                playerCombat != null &&
                playerCombat.IsRangedInputHeld;

            playerHealthAlpha = MoveAlpha(
                playerHealthAlpha,
                showPlayerHealth
            );
            horseHealthAlpha = MoveAlpha(
                horseHealthAlpha,
                showHorseHealth
            );
            ammoAlpha = MoveAlpha(
                ammoAlpha,
                showAmmo
            );
        }

        private bool ResolvePlayerStationary()
        {
            if (horseController != null &&
                horseController.IsMounted &&
                IsBoundRider(horseController.Rider))
            {
                return horseController.IsMountedStationary;
            }

            if (playerController == null)
                return false;

            return
                playerController.IsGrounded &&
                !playerController.HasMoveInput &&
                playerController.LastMoveWorldDirection.sqrMagnitude <=
                    0.0025f;
        }

        private bool ResolvePlayerNearHorse()
        {
            if (horseHealth == null || playerTransform == null)
                return false;

            // The horse bar is intentionally a proximity-only inspection
            // surface. Riding the horse does not count as standing beside it;
            // mounted play stays visually clean until the rider dismounts and
            // approaches the horse on foot.
            if (horseController != null &&
                horseController.IsMounted &&
                IsBoundRider(horseController.Rider))
            {
                return false;
            }

            Vector3 offset =
                playerTransform.position -
                horseHealth.transform.position;
            offset.y = 0f;

            float range = Mathf.Max(0.5f, horseProximityRange);
            return offset.sqrMagnitude <= range * range;
        }

        private bool IsBoundRider(Transform rider)
        {
            if (rider == null || playerTransform == null)
                return false;

            return
                rider == playerTransform ||
                rider.IsChildOf(playerTransform) ||
                playerTransform.IsChildOf(rider);
        }

        private float MoveAlpha(float current, bool visible)
        {
            float duration = visible
                ? Mathf.Max(0.05f, fadeInSeconds)
                : Mathf.Max(0.05f, fadeOutSeconds);

            return Mathf.MoveTowards(
                current,
                visible ? 1f : 0f,
                Time.unscaledDeltaTime / duration
            );
        }

        private void SubscribePlayerHealth()
        {
            if (playerHealth == null)
            {
                playerHealthSnapshotReady = false;
                return;
            }

            previousPlayerHealth = playerHealth.CurrentHealth;
            playerHealthSnapshotReady = true;
            playerHealth.HealthChanged += HandlePlayerHealthChanged;
        }

        private void UnsubscribePlayerHealth()
        {
            if (playerHealth != null)
                playerHealth.HealthChanged -= HandlePlayerHealthChanged;

            playerHealthSnapshotReady = false;
        }

        private void HandlePlayerHealthChanged(
            BDHealth changedHealth,
            float currentHealth,
            float maximumHealth)
        {
            if (!playerHealthSnapshotReady)
            {
                previousPlayerHealth = currentHealth;
                playerHealthSnapshotReady = true;
                return;
            }

            if (Mathf.Abs(currentHealth - previousPlayerHealth) > 0.01f)
            {
                playerHealthForcedUntil = Mathf.Max(
                    playerHealthForcedUntil,
                    Time.unscaledTime +
                    Mathf.Max(0.10f, healthEventHoldSeconds)
                );
            }

            previousPlayerHealth = currentHealth;
        }
    }
}
