using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDSecretCollectibleHud : MonoBehaviour
    {
        [Header("Display")]
        [SerializeField] private bool visible = true;
        [SerializeField] private float marginLeft = 18f;
        [SerializeField] private float marginTop = 18f;
        [SerializeField] private float badgeHeight = 28f;
        [SerializeField] private float badgeGap = 8f;
        [SerializeField] private float appearPulseDuration = 0.75f;

        [Header("Colors")]
        [SerializeField] private Color panelColor = new Color(0.025f, 0.035f, 0.045f, 0.78f);
        [SerializeField] private Color gameBoyColor = new Color(0.25f, 0.95f, 0.45f, 0.92f);
        [SerializeField] private Color batteryColor = new Color(1.00f, 0.78f, 0.20f, 0.92f);
        [SerializeField] private Color cartridgeColor = new Color(0.55f, 0.42f, 1.00f, 0.92f);
        [SerializeField] private Color textColor = new Color(0.92f, 0.98f, 1.00f, 1f);

        private BDGameBoyInventory inventory;
        private GUIStyle badgeStyle;
        private GUIStyle smallStyle;
        private Texture2D whiteTexture;
        private float lastSecretCollectedAt = -999f;
        private BDSecretCollectibleKind lastSecretKind;

        private void Awake()
        {
            whiteTexture = Texture2D.whiteTexture;
        }

        private void OnDisable()
        {
            UnsubscribeInventory();
        }

        private void Update()
        {
            ResolveInventory();
        }

        private void ResolveInventory()
        {
            if (inventory != null)
                return;

            Transform player = BDTargetFinder.FindPlayer();

            if (player == null)
            {
                BDPlayerMarker marker = FindFirstObjectByType<BDPlayerMarker>();
                if (marker != null)
                    player = marker.transform;
            }

            if (player == null)
                return;

            inventory = player.GetComponent<BDGameBoyInventory>();
            if (inventory != null)
                inventory.SecretCollected += OnSecretCollected;
        }

        private void UnsubscribeInventory()
        {
            if (inventory != null)
                inventory.SecretCollected -= OnSecretCollected;

            inventory = null;
        }

        private void OnSecretCollected(BDSecretCollectibleKind kind)
        {
            lastSecretKind = kind;
            lastSecretCollectedAt = Time.time;
        }

        private void OnGUI()
        {
            if (!visible)
                return;

            ResolveInventory();

            if (inventory == null || !inventory.HasAnySecretCollectible)
                return;

            EnsureStyles();

            float x = marginLeft;
            float y = marginTop;
            int count = CountVisibleBadges();

            if (count <= 0)
                return;

            // Small dark backing. It appears only after the player has found at least one secret.
            float panelWidth = EstimatePanelWidth();
            Rect panel = new Rect(x - 6f, y - 5f, panelWidth + 12f, badgeHeight + 10f);
            DrawRect(panel, panelColor);

            if (inventory.HasGameBoy)
                DrawBadge(ref x, y, "GB", gameBoyColor, BDSecretCollectibleKind.GameBoy);

            if (inventory.BatteryCount > 0)
                DrawBadge(ref x, y, inventory.BatteryCount > 1 ? $"BAT x{inventory.BatteryCount}" : "BAT", batteryColor, BDSecretCollectibleKind.Battery);

            if (inventory.HasGameCartridge)
                DrawBadge(ref x, y, "CART", cartridgeColor, BDSecretCollectibleKind.GameCartridge);
        }

        private int CountVisibleBadges()
        {
            int count = 0;

            if (inventory == null)
                return 0;

            if (inventory.HasGameBoy)
                count++;
            if (inventory.BatteryCount > 0)
                count++;
            if (inventory.HasGameCartridge)
                count++;

            return count;
        }

        private float EstimatePanelWidth()
        {
            float width = 0f;

            if (inventory.HasGameBoy)
                width += 48f + badgeGap;
            if (inventory.BatteryCount > 0)
                width += (inventory.BatteryCount > 1 ? 76f : 58f) + badgeGap;
            if (inventory.HasGameCartridge)
                width += 72f + badgeGap;

            return Mathf.Max(48f, width - badgeGap);
        }

        private void DrawBadge(ref float x, float y, string label, Color color, BDSecretCollectibleKind kind)
        {
            float width = Mathf.Max(48f, label.Length * 11f + 22f);
            Rect rect = new Rect(x, y, width, badgeHeight);

            float pulse01 = 0f;
            if (lastSecretKind == kind)
                pulse01 = Mathf.Clamp01(1f - ((Time.time - lastSecretCollectedAt) / Mathf.Max(0.01f, appearPulseDuration)));

            float scale = 1f + pulse01 * 0.14f;
            Rect scaled = ScaleRect(rect, scale);

            DrawRect(scaled, color);
            GUI.Box(scaled, GUIContent.none);
            GUI.Label(scaled, label, badgeStyle);

            x += width + badgeGap;
        }

        private Rect ScaleRect(Rect rect, float scale)
        {
            Vector2 center = rect.center;
            float width = rect.width * scale;
            float height = rect.height * scale;
            return new Rect(center.x - width * 0.5f, center.y - height * 0.5f, width, height);
        }

        private void DrawRect(Rect rect, Color color)
        {
            Color old = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, whiteTexture);
            GUI.color = old;
        }

        private void EnsureStyles()
        {
            if (badgeStyle != null)
                return;

            badgeStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            badgeStyle.normal.textColor = textColor;

            smallStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleLeft
            };
            smallStyle.normal.textColor = textColor;
        }
    }
}
