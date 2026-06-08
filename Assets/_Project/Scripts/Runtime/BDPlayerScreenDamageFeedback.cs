using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDPlayerScreenDamageFeedback : MonoBehaviour
    {
        [Header("Low Health")]
        [SerializeField] private float lowHealthThreshold01 = 0.34f;
        [SerializeField] private float criticalHealthThreshold01 = 0.18f;
        [SerializeField] private float lowHealthPulseFrequency = 3.3f;
        [SerializeField] private float maxLowHealthAlpha = 0.26f;

        [Header("Damage Pulse")]
        [SerializeField] private float damagePulseDuration = 0.32f;
        [SerializeField] private float damagePulseAlpha = 0.36f;

        [Header("Style")]
        [SerializeField] private Color damageColor = new Color(1f, 0.05f, 0.02f, 1f);
        [SerializeField] private bool showOnlyInPlayMode = true;

        private static BDPlayerScreenDamageFeedback instance;
        private static Texture2D pixel;

        private BDHealth playerHealth;
        private float previousHealth;
        private float damagePulseEndsAt;
        private float nextResolveAt;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (instance != null)
                return;

            GameObject go = new GameObject("BD_PlayerScreenDamageFeedback");
            if (!Application.isPlaying)
            {
                Object.DestroyImmediate(go);
                return;
            }

            DontDestroyOnLoad(go);
            instance = go.AddComponent<BDPlayerScreenDamageFeedback>();
        }

        private void Update()
        {
            ResolvePlayerHealthIfNeeded();
            TrackDamagePulse();
        }

        private void OnGUI()
        {
            if (!BDGameplayUiVisibility.IsGameplayHudVisible)
                return;

            if (showOnlyInPlayMode && !Application.isPlaying)
                return;

            if (playerHealth == null || playerHealth.IsDead)
                return;

            float hp01 = Mathf.Clamp01(playerHealth.CurrentHealth / Mathf.Max(1f, playerHealth.MaxHealth));
            float lowAlpha = ResolveLowHealthAlpha(hp01);
            float pulseAlpha = ResolveDamagePulseAlpha();
            float alpha = Mathf.Clamp01(Mathf.Max(lowAlpha, pulseAlpha));

            if (alpha <= 0.001f)
                return;

            EnsurePixel();
            Color old = GUI.color;

            Color color = damageColor;
            color.a = alpha;
            GUI.color = color;

            DrawVignette();

            GUI.color = old;
        }

        private void ResolvePlayerHealthIfNeeded()
        {
            if (playerHealth != null)
                return;

            if (Time.time < nextResolveAt)
                return;

            nextResolveAt = Time.time + 0.25f;

            Transform player = BDTargetFinder.FindPlayer();
            if (player == null)
                return;

            playerHealth = player.GetComponent<BDHealth>();
            previousHealth = playerHealth != null ? playerHealth.CurrentHealth : 0f;
        }

        private void TrackDamagePulse()
        {
            if (playerHealth == null)
                return;

            float current = playerHealth.CurrentHealth;

            if (current < previousHealth)
                damagePulseEndsAt = Time.time + damagePulseDuration;

            previousHealth = current;
        }

        private float ResolveLowHealthAlpha(float hp01)
        {
            if (hp01 >= lowHealthThreshold01)
                return 0f;

            float danger01 = Mathf.InverseLerp(lowHealthThreshold01, criticalHealthThreshold01, hp01);
            danger01 = 1f - Mathf.Clamp01(danger01);

            float pulse = 0.55f + Mathf.Sin(Time.time * lowHealthPulseFrequency) * 0.45f;
            float baseAlpha = Mathf.Lerp(0.08f, maxLowHealthAlpha, danger01);

            return baseAlpha * Mathf.Lerp(0.65f, 1f, pulse);
        }

        private float ResolveDamagePulseAlpha()
        {
            if (Time.time >= damagePulseEndsAt)
                return 0f;

            float remaining = damagePulseEndsAt - Time.time;
            float progress = Mathf.Clamp01(remaining / Mathf.Max(0.01f, damagePulseDuration));

            return damagePulseAlpha * progress * progress;
        }

        private void DrawVignette()
        {
            float w = Screen.width;
            float h = Screen.height;

            float borderX = Mathf.Max(34f, w * 0.075f);
            float borderY = Mathf.Max(28f, h * 0.080f);

            GUI.DrawTexture(new Rect(0, 0, w, borderY), pixel);
            GUI.DrawTexture(new Rect(0, h - borderY, w, borderY), pixel);
            GUI.DrawTexture(new Rect(0, 0, borderX, h), pixel);
            GUI.DrawTexture(new Rect(w - borderX, 0, borderX, h), pixel);

            // Stronger corners, still no text.
            GUI.DrawTexture(new Rect(0, 0, borderX * 1.35f, borderY * 1.35f), pixel);
            GUI.DrawTexture(new Rect(w - borderX * 1.35f, 0, borderX * 1.35f, borderY * 1.35f), pixel);
            GUI.DrawTexture(new Rect(0, h - borderY * 1.35f, borderX * 1.35f, borderY * 1.35f), pixel);
            GUI.DrawTexture(new Rect(w - borderX * 1.35f, h - borderY * 1.35f, borderX * 1.35f, borderY * 1.35f), pixel);
        }

        private static void EnsurePixel()
        {
            if (pixel != null)
                return;

            pixel = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            pixel.SetPixel(0, 0, Color.white);
            pixel.Apply(updateMipmaps: false, makeNoLongerReadable: true);
        }
    }
}
