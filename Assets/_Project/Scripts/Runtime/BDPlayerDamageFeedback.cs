using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDPlayerDamageFeedback : MonoBehaviour
    {
        [SerializeField] private float damageFlashDuration = 0.18f;
        [SerializeField] private float deathOverlayAlpha = 0.55f;
        [SerializeField] private bool disableControlOnDeath = true;

        private BDHealth health;
        private BDPlayerController playerController;
        private BDPlayerCombat playerCombat;
        private CharacterController characterController;
        private BDPlayerDismountLeap dismountLeap;

        private float previousHealth;
        private float flashTimer;
        private bool deathApplied;
        private Texture2D whiteTexture;

        private void Awake()
        {
            whiteTexture = Texture2D.whiteTexture;
            health = GetComponent<BDHealth>();
            playerController = GetComponent<BDPlayerController>();
            playerCombat = GetComponent<BDPlayerCombat>();
            characterController = GetComponent<CharacterController>();
            dismountLeap = GetComponent<BDPlayerDismountLeap>();

            previousHealth = health != null ? health.CurrentHealth : 0f;

            if (health != null)
            {
                health.HealthChanged += OnHealthChanged;
                health.Died += OnDied;
            }
        }

        private void Update()
        {
            if (flashTimer > 0f)
                flashTimer -= Time.deltaTime;

            if (health != null && health.IsDead && !deathApplied)
                ApplyDeath();
        }

        private void OnHealthChanged(BDHealth changedHealth, float current, float max)
        {
            if (current < previousHealth)
                flashTimer = damageFlashDuration;

            previousHealth = current;
        }

        private void OnDied(BDHealth deadHealth)
        {
            ApplyDeath();
        }

        private void ApplyDeath()
        {
            if (deathApplied)
                return;

            deathApplied = true;
            flashTimer = Mathf.Max(flashTimer, damageFlashDuration);

            if (!disableControlOnDeath)
                return;

            if (playerController != null)
                playerController.enabled = false;

            if (playerCombat != null)
                playerCombat.enabled = false;

            if (dismountLeap != null)
                dismountLeap.enabled = false;

            // Keep CharacterController enabled so the body stays collidable,
            // but no player script should move it anymore.
        }

        private void OnGUI()
        {
            if (!BDGameplayUiVisibility.IsGameplayHudVisible)
                return;

            if (flashTimer > 0f)
            {
                float alpha = Mathf.Clamp01(flashTimer / damageFlashDuration) * 0.35f;
                DrawOverlay(new Color(1f, 0f, 0f, alpha));
            }

            if (health != null &&
                health.IsDead &&
                !BDCharacterDeathAnimation.IsDeathPresentationActive(health))
            {
                DrawOverlay(new Color(0f, 0f, 0f, deathOverlayAlpha));
            }
        }

        private void DrawOverlay(Color color)
        {
            Color old = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), whiteTexture);
            GUI.color = old;
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.HealthChanged -= OnHealthChanged;
                health.Died -= OnDied;
            }
        }
    }
}
