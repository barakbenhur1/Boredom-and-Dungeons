using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseHealth))]
    public sealed class BDHorseNeedsHealingEffect : MonoBehaviour
    {
        [Header("Visibility")]
        [SerializeField] private float missingHealthThreshold = 0.5f;
        [SerializeField] private bool showWhileMounted = true;

        [Header("Visual")]
        [SerializeField] private float heightOffset = 2.25f;
        [SerializeField] private float orbitRadius = 0.62f;
        [SerializeField] private float orbitSpeed = 95f;
        [SerializeField] private float pulseSpeed = 5.5f;
        [SerializeField] private float minScale = 0.78f;
        [SerializeField] private float maxScale = 1.22f;
        [SerializeField] private Color injuredColor = new Color(1f, 0.42f, 0.08f, 1f);
        [SerializeField] private Color faintedColor = new Color(1f, 0.08f, 0.12f, 1f);

        private BDHorseHealth horseHealth;
        private BDHorseController horseController;
        private Transform root;
        private Transform[] motes;
        private Renderer[] moteRenderers;
        private MaterialPropertyBlock propertyBlock;

        private static Material sharedMaterial;
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            horseHealth = GetComponent<BDHorseHealth>();
            horseController = GetComponent<BDHorseController>();
            propertyBlock = new MaterialPropertyBlock();
            BuildEffect();
            SetVisible(false);
        }

        private void LateUpdate()
        {
            if (horseHealth == null || root == null)
                return;

            bool visible = ShouldShow();
            if (root.gameObject.activeSelf != visible)
                SetVisible(visible);

            if (!visible)
                return;

            root.localPosition = Vector3.up * heightOffset;
            root.localRotation = Quaternion.Euler(0f, Time.time * orbitSpeed, 0f);

            float pulse01 = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
            float scale = Mathf.Lerp(minScale, maxScale, pulse01);
            root.localScale = Vector3.one * scale;

            Camera camera = Camera.main;
            if (camera != null)
            {
                Vector3 direction = camera.transform.position - root.position;
                direction.y = 0f;
                if (direction.sqrMagnitude > 0.001f)
                    root.forward = -direction.normalized;
            }

            Color color = horseHealth.IsFainted ? faintedColor : injuredColor;
            Color emission = color * Mathf.Lerp(1.4f, 3.2f, pulse01);
            ApplyColor(color, emission);
        }

        private bool ShouldShow()
        {
            if (horseHealth == null)
                return false;

            if (!showWhileMounted && horseController != null && horseController.IsMounted)
                return false;

            return horseHealth.IsFainted ||
                   horseHealth.CurrentHealth < horseHealth.MaxHealth - missingHealthThreshold;
        }

        private void BuildEffect()
        {
            GameObject rootObject = new GameObject("BD_Horse_Needs_Healing_Effect");
            root = rootObject.transform;
            root.SetParent(transform, false);
            root.localPosition = Vector3.up * heightOffset;

            const int moteCount = 6;
            motes = new Transform[moteCount];
            moteRenderers = new Renderer[moteCount];

            for (int i = 0; i < moteCount; i++)
            {
                float angle = i * (360f / moteCount);
                Vector3 localPosition = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * orbitRadius;

                GameObject mote = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mote.name = $"BD_Horse_Healing_Mote_{i}";
                mote.transform.SetParent(root, false);
                mote.transform.localPosition = localPosition;
                mote.transform.localScale = Vector3.one * (i % 2 == 0 ? 0.15f : 0.11f);

                Collider collider = mote.GetComponent<Collider>();
                if (collider != null)
                    Destroy(collider);

                Renderer renderer = mote.GetComponent<Renderer>();
                if (renderer != null)
                    renderer.sharedMaterial = GetSharedMaterial();

                motes[i] = mote.transform;
                moteRenderers[i] = renderer;
            }

            GameObject crossVertical = GameObject.CreatePrimitive(PrimitiveType.Cube);
            crossVertical.name = "BD_Horse_Healing_Cross_Vertical";
            crossVertical.transform.SetParent(root, false);
            crossVertical.transform.localPosition = Vector3.zero;
            crossVertical.transform.localScale = new Vector3(0.12f, 0.46f, 0.08f);
            RemoveColliderAndAssignMaterial(crossVertical);

            GameObject crossHorizontal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            crossHorizontal.name = "BD_Horse_Healing_Cross_Horizontal";
            crossHorizontal.transform.SetParent(root, false);
            crossHorizontal.transform.localPosition = Vector3.zero;
            crossHorizontal.transform.localScale = new Vector3(0.46f, 0.12f, 0.08f);
            RemoveColliderAndAssignMaterial(crossHorizontal);

            Renderer verticalRenderer = crossVertical.GetComponent<Renderer>();
            Renderer horizontalRenderer = crossHorizontal.GetComponent<Renderer>();

            Renderer[] expanded = new Renderer[moteRenderers.Length + 2];
            for (int i = 0; i < moteRenderers.Length; i++)
                expanded[i] = moteRenderers[i];
            expanded[expanded.Length - 2] = verticalRenderer;
            expanded[expanded.Length - 1] = horizontalRenderer;
            moteRenderers = expanded;
        }

        private void ApplyColor(Color color, Color emission)
        {
            for (int i = 0; i < moteRenderers.Length; i++)
            {
                Renderer renderer = moteRenderers[i];
                if (renderer == null)
                    continue;

                renderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor(BaseColorId, color);
                propertyBlock.SetColor(ColorId, color);
                propertyBlock.SetColor(EmissionColorId, emission);
                renderer.SetPropertyBlock(propertyBlock);
            }
        }

        private void SetVisible(bool visible)
        {
            if (root != null)
                root.gameObject.SetActive(visible);
        }

        private static void RemoveColliderAndAssignMaterial(GameObject target)
        {
            Collider collider = target.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = GetSharedMaterial();
        }

        private static Material GetSharedMaterial()
        {
            if (sharedMaterial != null)
                return sharedMaterial;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            if (shader == null) shader = Shader.Find("Hidden/InternalErrorShader");

            sharedMaterial = new Material(shader);
            sharedMaterial.color = Color.white;

            if (sharedMaterial.HasProperty("_EmissionColor"))
            {
                sharedMaterial.EnableKeyword("_EMISSION");
                sharedMaterial.SetColor("_EmissionColor", Color.white);
            }

            return sharedMaterial;
        }
    }
}
