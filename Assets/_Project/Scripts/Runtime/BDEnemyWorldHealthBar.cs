using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDEnemyWorldHealthBar : MonoBehaviour
    {
        [Header("Visibility")]
        [SerializeField] private bool hideWhenFull = true;
        [SerializeField] private float visibleAfterDamageSeconds = 3.5f;
        [SerializeField] private float maxVisibleDistance = 42f;

        [Header("Layout")]
        [SerializeField] private float width = 1.25f;
        [SerializeField] private float height = 0.105f;
        [SerializeField] private float verticalOffset = 0.55f;
        [SerializeField] private float fillInset = 0.012f;

        [Header("Smoothing")]
        [SerializeField] private float fillSmoothing = 18f;
        [SerializeField] private float alphaSmoothing = 16f;

        private BDHealth health;
        private CharacterController controller;
        private Renderer[] ownerRenderers;

        private Transform root;
        private Transform background;
        private Transform fill;

        private Renderer backgroundRenderer;
        private Renderer fillRenderer;

        private MaterialPropertyBlock block;
        private float displayed01 = 1f;
        private float targetAlpha;
        private float currentAlpha;
        private float showUntilTime;
        private bool built;

        private static Material sharedMaterial;
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            health = GetComponent<BDHealth>();
            controller = GetComponent<CharacterController>();
            ownerRenderers = GetComponentsInChildren<Renderer>(includeInactive: true);
            block = new MaterialPropertyBlock();

            if (health != null)
            {
                health.HealthChanged += OnHealthChanged;
                health.Died += OnDied;
            }

            BuildBar();
            UpdateVisibility(force: true);
        }

        private void LateUpdate()
        {
            if (health == null || health.IsDead)
            {
                SetBarActive(false);
                return;
            }

            if (!built)
                BuildBar();

            UpdatePositionAndRotation();
            UpdateFill();
            UpdateVisibility(force: false);
            ApplyAlpha();
        }

        private void BuildBar()
        {
            if (built)
                return;

            GameObject rootGo = new GameObject("BD_Enemy_HP_Bar");
            root = rootGo.transform;
            root.SetParent(transform, worldPositionStays: false);
            root.localPosition = Vector3.up * ResolveBarHeight();
            root.localRotation = Quaternion.identity;
            root.localScale = Vector3.one;

            background = CreateBarPart("BD_Enemy_HP_Back", new Vector3(width, height, 0.035f), out backgroundRenderer).transform;
            background.SetParent(root, worldPositionStays: false);
            background.localPosition = Vector3.zero;
            background.localRotation = Quaternion.identity;

            fill = CreateBarPart("BD_Enemy_HP_Fill", new Vector3(width - fillInset * 2f, height * 0.62f, 0.045f), out fillRenderer).transform;
            fill.SetParent(root, worldPositionStays: false);
            fill.localPosition = new Vector3(0f, 0f, -0.012f);
            fill.localRotation = Quaternion.identity;

            built = true;
        }

        private GameObject CreateBarPart(string name, Vector3 scale, out Renderer renderer)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
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
            }

            return go;
        }

        private float ResolveBarHeight()
        {
            if (controller != null)
                return Mathf.Max(1.25f, controller.height + verticalOffset);

            Bounds bounds = new Bounds(transform.position, Vector3.one);
            bool hasBounds = false;

            if (ownerRenderers != null)
            {
                for (int i = 0; i < ownerRenderers.Length; i++)
                {
                    Renderer renderer = ownerRenderers[i];
                    if (renderer == null)
                        continue;

                    if (!hasBounds)
                    {
                        bounds = renderer.bounds;
                        hasBounds = true;
                    }
                    else
                    {
                        bounds.Encapsulate(renderer.bounds);
                    }
                }
            }

            if (hasBounds)
                return Mathf.Max(1.25f, bounds.size.y + verticalOffset);

            return 1.85f;
        }

        private void UpdatePositionAndRotation()
        {
            if (root == null)
                return;

            root.localPosition = Vector3.up * ResolveBarHeight();

            Camera camera = Camera.main;
            if (camera != null)
                root.rotation = Quaternion.LookRotation(camera.transform.forward, Vector3.up);
        }

        private void UpdateFill()
        {
            if (fill == null || health == null)
                return;

            float hp01 = Mathf.Clamp01(health.CurrentHealth / Mathf.Max(1f, health.MaxHealth));
            displayed01 = Mathf.Lerp(displayed01, hp01, 1f - Mathf.Exp(-fillSmoothing * Time.deltaTime));

            Vector3 scale = fill.localScale;
            scale.x = Mathf.Max(0.001f, (width - fillInset * 2f) * displayed01);
            fill.localScale = scale;

            float leftOffset = -((width - fillInset * 2f) * (1f - displayed01)) * 0.5f;
            fill.localPosition = new Vector3(leftOffset, 0f, -0.012f);
        }

        private void UpdateVisibility(bool force)
        {
            if (health == null || root == null)
                return;

            float hp01 = Mathf.Clamp01(health.CurrentHealth / Mathf.Max(1f, health.MaxHealth));
            bool shouldShow = !hideWhenFull || hp01 < 0.999f;

            if (visibleAfterDamageSeconds > 0f)
                shouldShow = shouldShow && Time.time <= showUntilTime;

            Camera camera = Camera.main;
            if (camera != null)
            {
                float effectiveMaxDistance = BDPerformanceGuard.ReducedEffects ? maxVisibleDistance * 0.65f : maxVisibleDistance;
                float distance = Vector3.Distance(camera.transform.position, transform.position);
                if (distance > effectiveMaxDistance)
                    shouldShow = false;
            }

            targetAlpha = shouldShow ? 1f : 0f;

            if (force)
                currentAlpha = targetAlpha;

            SetBarActive(currentAlpha > 0.01f || targetAlpha > 0.01f);
        }

        private void ApplyAlpha()
        {
            currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, 1f - Mathf.Exp(-alphaSmoothing * Time.deltaTime));

            if (currentAlpha <= 0.01f && targetAlpha <= 0.01f)
            {
                SetBarActive(false);
                return;
            }

            ApplyRendererColor(backgroundRenderer, new Color(0.04f, 0.04f, 0.045f, 0.82f * currentAlpha), 0.0f);
            ApplyRendererColor(fillRenderer, ResolveFillColor(currentAlpha), 0.25f);
        }

        private Color ResolveFillColor(float alpha)
        {
            float hp01 = health != null ? Mathf.Clamp01(health.CurrentHealth / Mathf.Max(1f, health.MaxHealth)) : 1f;
            Color high = new Color(0.25f, 1f, 0.42f, alpha);
            Color mid = new Color(1f, 0.80f, 0.16f, alpha);
            Color low = new Color(1f, 0.18f, 0.10f, alpha);

            if (hp01 > 0.50f)
                return Color.Lerp(mid, high, (hp01 - 0.50f) / 0.50f);

            return Color.Lerp(low, mid, hp01 / 0.50f);
        }

        private void ApplyRendererColor(Renderer renderer, Color color, float emissionMultiplier)
        {
            if (renderer == null)
                return;

            renderer.GetPropertyBlock(block);
            block.SetColor(BaseColorId, color);
            block.SetColor(ColorId, color);
            block.SetColor(EmissionColorId, color * emissionMultiplier);
            renderer.SetPropertyBlock(block);
        }

        private void SetBarActive(bool active)
        {
            if (root != null && root.gameObject.activeSelf != active)
                root.gameObject.SetActive(active);
        }

        private void OnHealthChanged(BDHealth changedHealth, float current, float max)
        {
            showUntilTime = Time.time + Mathf.Max(0f, visibleAfterDamageSeconds);

            if (root != null)
                SetBarActive(true);
        }

        private void OnDied(BDHealth deadHealth)
        {
            if (root != null)
                Destroy(root.gameObject);
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.HealthChanged -= OnHealthChanged;
                health.Died -= OnDied;
            }

            if (root != null)
                Destroy(root.gameObject);
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

            if (shader != null)
            {
                sharedMaterial = new Material(shader);
                sharedMaterial.color = Color.white;

                if (sharedMaterial.HasProperty("_EmissionColor"))
                {
                    sharedMaterial.EnableKeyword("_EMISSION");
                    sharedMaterial.SetColor("_EmissionColor", Color.black);
                }

                return sharedMaterial;
            }

            Material builtIn = Resources.GetBuiltinResource<Material>("Default-Material.mat");
            if (builtIn != null)
                sharedMaterial = new Material(builtIn);

            return sharedMaterial;
        }
    }
}
