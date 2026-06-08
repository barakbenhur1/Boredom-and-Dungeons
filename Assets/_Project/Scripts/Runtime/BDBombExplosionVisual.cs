using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBombExplosionVisual : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.52f;
        [SerializeField] private int ringSegments = 32;
        [SerializeField] private int sparkCount = 10;

        private Transform core;
        private Renderer coreRenderer;
        private LineRenderer ring;
        private Transform[] sparks;
        private Renderer[] sparkRenderers;
        private Vector3[] sparkDirections;
        private MaterialPropertyBlock block;
        private float radius;
        private float elapsed;
        private static Material sharedMaterial;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        public static void Spawn(Vector3 position, float explosionRadius)
        {
            if (!BDPerformanceGuard.AllowCosmeticSpawn(0.88f))
                return;

            GameObject root = new GameObject("BD_Bomb_Explosion_V23R11");
            root.transform.position = position;
            BDBombExplosionVisual visual = root.AddComponent<BDBombExplosionVisual>();
            visual.Configure(explosionRadius);
        }

        private void Configure(float explosionRadius)
        {
            radius = Mathf.Max(0.5f, explosionRadius);
            lifetime = BDPerformanceGuard.ResolveEffectLifetime(lifetime);
            block = new MaterialPropertyBlock();
            BuildCore();
            BuildRing();
            BuildSparks();
        }

        private void BuildCore()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Bomb_Explosion_Core";
            go.transform.SetParent(transform, false);
            go.transform.localPosition = Vector3.up * 0.30f;
            go.transform.localScale = Vector3.one * 0.18f;
            DestroyCollider(go);
            core = go.transform;
            coreRenderer = go.GetComponent<Renderer>();
            if (coreRenderer != null)
                coreRenderer.sharedMaterial = GetSharedMaterial();
        }

        private void BuildRing()
        {
            GameObject go = new GameObject("Bomb_Explosion_Ground_Ring");
            go.transform.SetParent(transform, false);
            go.transform.localPosition = Vector3.up * 0.055f;
            ring = go.AddComponent<LineRenderer>();
            ring.useWorldSpace = false;
            ring.loop = true;
            ring.positionCount = Mathf.Max(16, ringSegments);
            ring.startWidth = 0.10f;
            ring.endWidth = 0.10f;
            ring.numCornerVertices = 3;
            ring.numCapVertices = 3;
            ring.sharedMaterial = GetSharedMaterial();

            for (int i = 0; i < ring.positionCount; i++)
            {
                float angle = i / (float)ring.positionCount * Mathf.PI * 2f;
                ring.SetPosition(i, new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)));
            }
        }

        private void BuildSparks()
        {
            int count = Mathf.Clamp(sparkCount, 6, 16);
            sparks = new Transform[count];
            sparkRenderers = new Renderer[count];
            sparkDirections = new Vector3[count];

            for (int i = 0; i < count; i++)
            {
                float angle = i / (float)count * Mathf.PI * 2f + (i % 2) * 0.17f;
                Vector3 direction = new Vector3(
                    Mathf.Cos(angle),
                    0.26f + (i % 3) * 0.12f,
                    Mathf.Sin(angle)
                ).normalized;

                GameObject spark = GameObject.CreatePrimitive(PrimitiveType.Cube);
                spark.name = "Bomb_Explosion_Spark_" + i;
                spark.transform.SetParent(transform, false);
                spark.transform.localPosition = Vector3.up * 0.28f;
                spark.transform.localScale = new Vector3(0.08f, 0.08f, 0.20f);
                spark.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                DestroyCollider(spark);

                Renderer renderer = spark.GetComponent<Renderer>();
                if (renderer != null)
                    renderer.sharedMaterial = GetSharedMaterial();

                sparks[i] = spark.transform;
                sparkRenderers[i] = renderer;
                sparkDirections[i] = direction;
            }
        }

        private void Update()
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, lifetime));
            float burst = 1f - Mathf.Pow(1f - t, 3f);
            float alpha = 1f - Mathf.SmoothStep(0.30f, 1f, t);

            if (core != null)
            {
                float scale = Mathf.Lerp(0.18f, radius * 0.74f, burst);
                core.localScale = Vector3.one * scale;
                ApplyColor(coreRenderer, new Color(1f, 0.44f, 0.05f, alpha), 3.2f);
            }

            if (ring != null)
            {
                float ringScale = Mathf.Lerp(0.10f, radius, burst);
                ring.transform.localScale = new Vector3(ringScale, 1f, ringScale);
                Color ringColor = new Color(1f, 0.72f, 0.12f, alpha * 0.88f);
                ring.startColor = ringColor;
                ring.endColor = new Color(ringColor.r, ringColor.g, ringColor.b, 0f);
                ring.startWidth = Mathf.Lerp(0.16f, 0.045f, t);
                ring.endWidth = ring.startWidth;
            }

            if (sparks != null)
            {
                for (int i = 0; i < sparks.Length; i++)
                {
                    Transform spark = sparks[i];
                    if (spark == null)
                        continue;

                    float distance = radius * Mathf.Lerp(0.18f, 0.92f, burst);
                    Vector3 direction = sparkDirections[i];
                    spark.localPosition =
                        direction * distance +
                        Vector3.down * (t * t * radius * 0.22f);
                    spark.localScale = Vector3.one * Mathf.Lerp(0.11f, 0.025f, t);
                    ApplyColor(
                        sparkRenderers[i],
                        new Color(1f, 0.30f + (i % 2) * 0.18f, 0.04f, alpha),
                        2.6f
                    );
                }
            }

            if (elapsed >= lifetime)
                Destroy(gameObject);
        }

        private void ApplyColor(Renderer renderer, Color color, float emission)
        {
            if (renderer == null)
                return;
            renderer.GetPropertyBlock(block);
            block.SetColor(BaseColorId, color);
            block.SetColor(ColorId, color);
            block.SetColor(EmissionColorId, color * emission);
            renderer.SetPropertyBlock(block);
        }

        private static void DestroyCollider(GameObject go)
        {
            Collider collider = go != null ? go.GetComponent<Collider>() : null;
            if (collider != null)
                Object.Destroy(collider);
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
                    sharedMaterial.SetColor("_EmissionColor", Color.white * 2f);
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
