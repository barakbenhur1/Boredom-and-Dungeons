using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(125)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseHealth))]
    public sealed class BDHorseHealAvailabilityIndicator : MonoBehaviour
    {
        [Header("Visibility")]
        [SerializeField] private float showDistance = 3.45f;
        [SerializeField] private float showWhenMissingHealth = 0.5f;
        [SerializeField] private bool hideWhileMounted = true;

        [Header("Layout")]
        [SerializeField] private float heightOffset = 3.95f;
        [SerializeField] private float horizontalOffset = -1.15f;
        [SerializeField] private float pulseScale = 0.09f;
        [SerializeField] private float visualScale = 0.82f;
        [SerializeField] private float depthOffsetTowardCamera = 0.12f;
        [SerializeField] private float pulseFrequency = 4.2f;
        [SerializeField] private int sortingOrder = 80;

        [Header("Style")]
        [SerializeField] private Color healColor =
            new Color(0.20f, 1f, 0.45f, 1f);
        [SerializeField] private Color faintedColor =
            new Color(0.25f, 0.95f, 1f, 1f);

        private BDHorseHealth horseHealth;
        private BDHorseController horseController;
        private Transform player;
        private Transform root;
        private Renderer verticalRenderer;
        private Renderer horizontalRenderer;
        private MaterialPropertyBlock block;
        private float nextPlayerResolveAt;

        private static Material sharedMaterial;
        private static readonly int BaseColorId =
            Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId =
            Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId =
            Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            horseHealth = GetComponent<BDHorseHealth>();
            horseController = GetComponent<BDHorseController>();
            block = new MaterialPropertyBlock();
            BuildIndicator();
            SetVisible(false);
        }

        private void LateUpdate()
        {
            if (horseHealth == null || root == null)
                return;

            ResolvePlayer();
            bool visible = ShouldShow();

            if (root.gameObject.activeSelf != visible)
                SetVisible(visible);

            if (!visible)
                return;

            UpdateTransform();
            UpdateColorAndPulse();
        }

        private bool ShouldShow()
        {
            if (horseHealth == null)
                return false;

            if (hideWhileMounted &&
                horseController != null &&
                horseController.IsMounted)
            {
                return false;
            }

            bool needsHealing =
                horseHealth.IsFainted ||
                horseHealth.CurrentHealth <
                    horseHealth.MaxHealth - showWhenMissingHealth;

            if (!needsHealing || player == null)
                return false;

            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;

            return toPlayer.sqrMagnitude <=
                showDistance * showDistance;
        }

        private void ResolvePlayer()
        {
            if (player != null)
                return;

            if (Application.isPlaying &&
                Time.time < nextPlayerResolveAt)
            {
                return;
            }

            nextPlayerResolveAt =
                Application.isPlaying
                    ? Time.time + 0.25f
                    : 0f;

            player = BDTargetFinder.FindPlayer();
        }

        private void BuildIndicator()
        {
            if (root != null)
                return;

            GameObject rootGo = new GameObject(
                "BD_Horse_Heal_Available_Indicator"
            );

            root = rootGo.transform;
            root.SetParent(transform, worldPositionStays: false);
            root.localRotation = Quaternion.identity;
            root.localScale = Vector3.one;

            Transform vertical = CreatePart(
                "BD_Horse_Heal_Indicator_Vertical",
                new Vector3(0.10f, 0.42f, 0.030f),
                out verticalRenderer
            );

            vertical.SetParent(root, worldPositionStays: false);
            vertical.localPosition = Vector3.zero;
            vertical.localRotation = Quaternion.identity;

            Transform horizontal = CreatePart(
                "BD_Horse_Heal_Indicator_Horizontal",
                new Vector3(0.42f, 0.10f, 0.030f),
                out horizontalRenderer
            );

            horizontal.SetParent(root, worldPositionStays: false);
            horizontal.localPosition = Vector3.zero;
            horizontal.localRotation = Quaternion.identity;
        }

        private Transform CreatePart(
            string objectName,
            Vector3 scale,
            out Renderer renderer)
        {
            GameObject go = GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

            go.name = objectName;
            go.transform.localScale = scale;

            Collider collider = go.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            renderer = go.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = GetSharedMaterial();
                if (material != null)
                    renderer.sharedMaterial = material;

                renderer.sortingOrder = sortingOrder;
            }

            return go.transform;
        }

        private void UpdateTransform()
        {
            Camera camera = Camera.main;

            if (camera != null)
            {
                root.position =
                    transform.position +
                    Vector3.up * heightOffset +
                    camera.transform.right * horizontalOffset -
                    camera.transform.forward *
                        depthOffsetTowardCamera;

                root.rotation = Quaternion.LookRotation(
                    camera.transform.forward,
                    Vector3.up
                );
            }
            else
            {
                root.localPosition = new Vector3(
                    horizontalOffset,
                    heightOffset,
                    0f
                );
            }

            float pulse =
                1f +
                Mathf.Sin(
                    Time.unscaledTime * pulseFrequency
                ) * pulseScale;

            root.localScale =
                Vector3.one *
                Mathf.Clamp(
                    visualScale * pulse,
                    0.70f,
                    0.96f
                );
        }

        private void UpdateColorAndPulse()
        {
            Color color =
                horseHealth != null && horseHealth.IsFainted
                    ? faintedColor
                    : healColor;

            float pulse =
                0.65f +
                Mathf.Sin(
                    Time.unscaledTime * pulseFrequency
                ) * 0.35f;

            Color emission = color * Mathf.Lerp(
                0.8f,
                2.1f,
                pulse
            );

            ApplyColor(verticalRenderer, color, emission);
            ApplyColor(horizontalRenderer, color, emission);
        }

        private void ApplyColor(
            Renderer renderer,
            Color color,
            Color emission)
        {
            if (renderer == null)
                return;

            renderer.GetPropertyBlock(block);
            block.SetColor(BaseColorId, color);
            block.SetColor(ColorId, color);
            block.SetColor(EmissionColorId, emission);
            renderer.SetPropertyBlock(block);
        }

        private void SetVisible(bool visible)
        {
            if (root != null)
                root.gameObject.SetActive(visible);
        }

        private static Material GetSharedMaterial()
        {
            if (sharedMaterial != null)
                return sharedMaterial;

            Shader shader = Shader.Find(
                "Universal Render Pipeline/Lit"
            );

            if (shader == null)
                shader = Shader.Find("Standard");
            if (shader == null)
                shader = Shader.Find("Unlit/Color");
            if (shader == null)
                shader = Shader.Find("Sprites/Default");
            if (shader == null)
                shader = Shader.Find("Hidden/InternalErrorShader");

            if (shader != null)
            {
                sharedMaterial = new Material(shader);
                sharedMaterial.color = Color.white;
                sharedMaterial.renderQueue = 3100;

                if (sharedMaterial.HasProperty("_EmissionColor"))
                {
                    sharedMaterial.EnableKeyword("_EMISSION");
                    sharedMaterial.SetColor(
                        "_EmissionColor",
                        Color.white
                    );
                }

                return sharedMaterial;
            }

            Material builtIn =
                Resources.GetBuiltinResource<Material>(
                    "Default-Material.mat"
                );

            if (builtIn != null)
                sharedMaterial = new Material(builtIn);

            return sharedMaterial;
        }
    }
}
