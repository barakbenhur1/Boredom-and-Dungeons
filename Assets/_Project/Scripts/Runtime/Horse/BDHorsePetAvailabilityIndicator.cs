using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(130)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseExhaustedFollowAndPetInteraction))]
    public sealed class BDHorsePetAvailabilityIndicator : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private float heightOffset = 4.65f;
        [SerializeField] private float horizontalOffset = 1.15f;
        [SerializeField] private float pulseScale = 0.09f;
        [SerializeField] private float visualScale = 0.82f;
        [SerializeField] private float depthOffsetTowardCamera = 0.18f;
        [SerializeField] private int sortingOrder = 90;
        [SerializeField] private float pulseFrequency = 3.8f;

        [Header("Style")]
        [SerializeField] private Color availableColor =
            new Color(1f, 0.68f, 0.18f, 1f);
        [SerializeField] private Color holdColor =
            new Color(1f, 0.94f, 0.48f, 1f);
        [SerializeField] private Color activeColor =
            new Color(1f, 0.48f, 0.62f, 1f);

        private BDHorseExhaustedFollowAndPetInteraction interaction;
        private Transform indicatorRoot;
        private Transform heartRoot;
        private Renderer leftLobeRenderer;
        private Renderer rightLobeRenderer;
        private Renderer pointRenderer;
        private TextMesh label;
        private MaterialPropertyBlock block;

        private static Material sharedMaterial;

        private static readonly int BaseColorId =
            Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId =
            Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId =
            Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            interaction =
                GetComponent<BDHorseExhaustedFollowAndPetInteraction>();

            block = new MaterialPropertyBlock();
            BuildIndicator();
            SetVisible(false);
        }

        private void LateUpdate()
        {
            if (interaction == null || indicatorRoot == null)
                return;

            bool visible =
                interaction.IsPetAvailable ||
                interaction.IsPetInteractionActive ||
                interaction.PetHoldProgress01 > 0f;

            if (indicatorRoot.gameObject.activeSelf != visible)
                SetVisible(visible);

            if (!visible)
                return;

            UpdateTransform();
            UpdateStyle();
        }

        private void BuildIndicator()
        {
            if (indicatorRoot != null)
                return;

            GameObject rootObject =
                new GameObject("BD_Horse_Pet_Available_Indicator");

            indicatorRoot = rootObject.transform;
            indicatorRoot.SetParent(
                transform,
                worldPositionStays: false
            );

            indicatorRoot.localPosition =
                new Vector3(
                    horizontalOffset,
                    heightOffset,
                    0f
                );

            indicatorRoot.localRotation = Quaternion.identity;
            indicatorRoot.localScale = Vector3.one;

            GameObject heartObject =
                new GameObject("BD_Horse_Pet_Heart");

            heartRoot = heartObject.transform;
            heartRoot.SetParent(
                indicatorRoot,
                worldPositionStays: false
            );

            heartRoot.localPosition =
                new Vector3(0f, 0.12f, 0f);

            Transform leftLobe = CreatePrimitivePart(
                "BD_Horse_Pet_Heart_Left",
                PrimitiveType.Sphere,
                new Vector3(0.24f, 0.24f, 0.07f),
                out leftLobeRenderer
            );

            leftLobe.SetParent(
                heartRoot,
                worldPositionStays: false
            );

            leftLobe.localPosition =
                new Vector3(-0.105f, 0.055f, 0f);

            Transform rightLobe = CreatePrimitivePart(
                "BD_Horse_Pet_Heart_Right",
                PrimitiveType.Sphere,
                new Vector3(0.24f, 0.24f, 0.07f),
                out rightLobeRenderer
            );

            rightLobe.SetParent(
                heartRoot,
                worldPositionStays: false
            );

            rightLobe.localPosition =
                new Vector3(0.105f, 0.055f, 0f);

            Transform point = CreatePrimitivePart(
                "BD_Horse_Pet_Heart_Point",
                PrimitiveType.Cube,
                new Vector3(0.25f, 0.25f, 0.07f),
                out pointRenderer
            );

            point.SetParent(
                heartRoot,
                worldPositionStays: false
            );

            point.localPosition =
                new Vector3(0f, -0.095f, 0f);

            point.localRotation =
                Quaternion.Euler(0f, 0f, 45f);

            GameObject labelObject =
                new GameObject("BD_Horse_Pet_Label");

            labelObject.transform.SetParent(
                indicatorRoot,
                worldPositionStays: false
            );

            labelObject.transform.localPosition =
                new Vector3(0f, -0.31f, 0f);

            labelObject.transform.localRotation =
                Quaternion.identity;

            label = labelObject.AddComponent<TextMesh>();
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.fontSize = 48;
            label.characterSize = 0.027f;
            label.fontStyle = FontStyle.Bold;
            label.text = BuildIdleLabel();
            label.color = availableColor;

            MeshRenderer labelRenderer =
                label.GetComponent<MeshRenderer>();

            if (labelRenderer != null)
                labelRenderer.sortingOrder = sortingOrder + 1;
        }

        private Transform CreatePrimitivePart(
            string objectName,
            PrimitiveType primitiveType,
            Vector3 scale,
            out Renderer renderer)
        {
            GameObject part =
                GameObject.CreatePrimitive(primitiveType);

            part.name = objectName;
            part.transform.localScale = scale;

            Collider collider = part.GetComponent<Collider>();

            if (collider != null)
                Destroy(collider);

            renderer = part.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material material = GetSharedMaterial();

                if (material != null)
                    renderer.sharedMaterial = material;

                renderer.sortingOrder = sortingOrder;
            }

            return part.transform;
        }

        private void UpdateTransform()
        {
            Camera camera = Camera.main;

            if (camera != null)
            {
                indicatorRoot.position =
                    transform.position +
                    Vector3.up * heightOffset +
                    camera.transform.right * horizontalOffset -
                    camera.transform.forward *
                        depthOffsetTowardCamera;

                indicatorRoot.rotation =
                    Quaternion.LookRotation(
                        camera.transform.forward,
                        Vector3.up
                    );
            }
            else
            {
                indicatorRoot.localPosition =
                    new Vector3(
                        horizontalOffset,
                        heightOffset,
                        0f
                    );
            }

            float holdProgress =
                interaction.PetHoldProgress01;

            float pulse =
                1f +
                Mathf.Sin(
                    Time.unscaledTime * pulseFrequency
                ) *
                pulseScale;

            float holdEmphasis =
                Mathf.Lerp(
                    1f,
                    1.18f,
                    holdProgress
                );

            indicatorRoot.localScale =
                Vector3.one *
                Mathf.Clamp(
                    visualScale * pulse * holdEmphasis,
                    0.68f,
                    1.08f
                );
        }

        private void UpdateStyle()
        {
            float holdProgress =
                interaction.PetHoldProgress01;

            Color color;

            if (interaction.IsPetInteractionActive)
                color = activeColor;
            else if (holdProgress > 0f)
                color = Color.Lerp(
                    availableColor,
                    holdColor,
                    holdProgress
                );
            else
                color = availableColor;

            float pulse =
                0.65f +
                Mathf.Sin(
                    Time.unscaledTime * pulseFrequency
                ) *
                0.35f;

            float emissionStrength =
                interaction.IsPetInteractionActive
                    ? 2.5f
                    : Mathf.Lerp(
                        1.15f,
                        2.35f,
                        Mathf.Max(
                            pulse,
                            holdProgress
                        )
                    );

            Color emission =
                color * emissionStrength;

            ApplyColor(
                leftLobeRenderer,
                color,
                emission
            );

            ApplyColor(
                rightLobeRenderer,
                color,
                emission
            );

            ApplyColor(
                pointRenderer,
                color,
                emission
            );

            if (heartRoot != null)
            {
                heartRoot.localRotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        Mathf.Sin(
                            Time.unscaledTime *
                            pulseFrequency *
                            0.55f
                        ) * 4f
                    );
            }

            if (label == null)
                return;

            label.color = color;

            if (interaction.IsPetInteractionActive)
            {
                label.text = "PETTING";
                return;
            }

            if (holdProgress > 0f)
            {
                label.text =
                    $"HOLD  {Mathf.RoundToInt(holdProgress * 100f)}%";
                return;
            }

            label.text = BuildIdleLabel();
        }

        private string BuildIdleLabel()
        {
            if (interaction == null)
                return "PET";

            string key =
                interaction.DesktopPetKey.ToString();

            return string.IsNullOrWhiteSpace(key)
                ? "PET"
                : key + "  PET";
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
            if (indicatorRoot != null)
                indicatorRoot.gameObject.SetActive(visible);
        }

        private static Material GetSharedMaterial()
        {
            if (sharedMaterial != null)
                return sharedMaterial;

            Shader shader =
                Shader.Find("Universal Render Pipeline/Lit");

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            if (shader == null)
                shader = Shader.Find("Sprites/Default");

            if (shader == null)
                shader = Shader.Find(
                    "Hidden/InternalErrorShader"
                );

            if (shader != null)
            {
                sharedMaterial = new Material(shader);
                sharedMaterial.color = Color.white;
                sharedMaterial.renderQueue = 3110;

                if (sharedMaterial.HasProperty(
                        "_EmissionColor"))
                {
                    sharedMaterial.EnableKeyword(
                        "_EMISSION"
                    );

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
