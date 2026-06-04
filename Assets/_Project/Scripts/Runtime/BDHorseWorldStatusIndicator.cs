using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseHealth))]
    public sealed class BDHorseWorldStatusIndicator : MonoBehaviour
    {
        [SerializeField] private Vector3 worldOffset = new Vector3(0f, 2.45f, 0f);
        [SerializeField] private float width = 92f;
        [SerializeField] private float height = 10f;
        [SerializeField] private bool alwaysShow = true;

        private BDHorseHealth health;
        private Camera mainCamera;
        private GUIStyle labelStyle;
        private Texture2D whiteTexture;

        private void Awake()
        {
            health = GetComponent<BDHorseHealth>();
            whiteTexture = Texture2D.whiteTexture;
        }

        private void Update()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        private void OnGUI()
        {
            if (health == null || mainCamera == null)
                return;

            if (!alwaysShow && !health.IsFainted && health.CurrentHealth >= health.MaxHealth)
                return;

            EnsureStyle();

            Vector3 screen = mainCamera.WorldToScreenPoint(transform.position + worldOffset);
            if (screen.z <= 0f)
                return;

            float x = screen.x - width * 0.5f;
            float y = Screen.height - screen.y;

            float ratio = Mathf.Clamp01(health.CurrentHealth / Mathf.Max(1f, health.MaxHealth));

            DrawBar(new Rect(x, y, width, height), ratio);

            string text = health.IsFainted ? "HORSE FAINTED" :
                          ratio < 0.55f ? "HORSE INJURED" :
                          "HORSE";

            GUI.Label(new Rect(x - 25f, y - 22f, width + 50f, 20f), text, labelStyle);

            if (health.IsFainted)
                GUI.Label(new Rect(x - 45f, y + 13f, width + 90f, 20f), "HOLD F TO REVIVE", labelStyle);
            else if (ratio < 0.98f)
                GUI.Label(new Rect(x - 45f, y + 13f, width + 90f, 20f), "HOLD F TO HEAL", labelStyle);
        }

        private void DrawBar(Rect rect, float ratio)
        {
            Color old = GUI.color;

            GUI.color = new Color(0f, 0f, 0f, 0.7f);
            GUI.DrawTexture(rect, whiteTexture);

            GUI.color = health.IsFainted
                ? new Color(0.5f, 0.5f, 0.5f, 0.95f)
                : new Color(0.10f, 0.65f, 0.95f, 0.95f);

            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width * ratio, rect.height), whiteTexture);

            GUI.color = Color.white;
            GUI.Box(rect, GUIContent.none);

            GUI.color = old;
        }

        private void EnsureStyle()
        {
            if (labelStyle != null)
                return;

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12,
                fontStyle = FontStyle.Bold
            };
            labelStyle.normal.textColor = Color.white;
        }
    }
}
