using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDGuardianSpawnVfx : MonoBehaviour
    {
        [Header("Timing")]
        [SerializeField] private float duration = 1.0f;
        [SerializeField] private float riseHeight = 0.75f;
        [SerializeField] private bool destroyWhenDone = true;

        [Header("Visuals")]
        [SerializeField] private Color ringColor = new Color(0.55f, 0.05f, 0.85f, 0.72f);
        [SerializeField] private Color smokeColor = new Color(0.06f, 0.02f, 0.08f, 0.42f);
        [SerializeField] private Color flashColor = new Color(0.95f, 0.35f, 1.00f, 0.75f);

        private float startedAt;
        private Transform ring;
        private Transform smoke;
        private Transform flash;
        private Material ringMaterial;
        private Material smokeMaterial;
        private Material flashMaterial;

        public float Duration => Mathf.Max(0.05f, duration);
        public bool IsDone => Time.time - startedAt >= Duration;

        public static BDGuardianSpawnVfx Create(Vector3 position, float duration = 1.0f)
        {
            GameObject root = new GameObject("BD_Guardian_Spawn_VFX");
            root.transform.position = position;

            BDGuardianSpawnVfx vfx = root.AddComponent<BDGuardianSpawnVfx>();
            vfx.duration = Mathf.Max(0.05f, duration);
            return vfx;
        }

        private void Awake()
        {
            startedAt = Time.time;
            BuildVisuals();
        }

        private void Update()
        {
            float t = Mathf.Clamp01((Time.time - startedAt) / Duration);
            float easeOut = 1f - Mathf.Pow(1f - t, 3f);
            float pulse = 0.5f + Mathf.Sin(Time.time * 18f) * 0.5f;

            if (ring != null)
            {
                float ringScale = Mathf.Lerp(0.35f, 2.75f, easeOut);
                ring.localScale = new Vector3(ringScale, 0.025f, ringScale);
                ring.localRotation = Quaternion.Euler(90f, Time.time * 130f, 0f);
            }

            if (smoke != null)
            {
                float smokeScale = Mathf.Lerp(0.65f, 2.1f, easeOut);
                smoke.localScale = Vector3.one * smokeScale;
                smoke.localPosition = Vector3.up * Mathf.Lerp(0.05f, riseHeight, easeOut);
            }

            if (flash != null)
            {
                float flashScale = Mathf.Lerp(0.2f, 1.2f, Mathf.Clamp01(t * 2.0f));
                flash.localScale = Vector3.one * flashScale;
            }

            SetAlpha(ringMaterial, ringColor.a * (1f - t) * (0.72f + pulse * 0.28f));
            SetAlpha(smokeMaterial, smokeColor.a * (1f - t));
            SetAlpha(flashMaterial, flashColor.a * Mathf.Clamp01(1f - Mathf.Abs(t - 0.55f) * 3.5f));

            if (IsDone && destroyWhenDone)
                Destroy(gameObject);
        }

        private void BuildVisuals()
        {
            ring = CreateCylinder("BD_Guardian_Spawn_Teleport_Ring", transform, new Vector3(0f, 0.025f, 0f), new Vector3(1f, 0.025f, 1f), ringColor).transform;
            ringMaterial = ring.GetComponent<Renderer>().sharedMaterial;

            smoke = CreateSphere("BD_Guardian_Spawn_Smoke", transform, new Vector3(0f, 0.15f, 0f), Vector3.one, smokeColor).transform;
            smokeMaterial = smoke.GetComponent<Renderer>().sharedMaterial;

            flash = CreateSphere("BD_Guardian_Spawn_Flash", transform, new Vector3(0f, 0.85f, 0f), Vector3.one * 0.25f, flashColor).transform;
            flashMaterial = flash.GetComponent<Renderer>().sharedMaterial;
        }

        private static GameObject CreateCylinder(string objectName, Transform parent, Vector3 localPosition, Vector3 localScale, Color color)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = objectName;
            go.transform.SetParent(parent, worldPositionStays: false);
            go.transform.localPosition = localPosition;
            go.transform.localScale = localScale;

            Collider collider = go.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = CreateTransparentMaterial(color);

            return go;
        }

        private static GameObject CreateSphere(string objectName, Transform parent, Vector3 localPosition, Vector3 localScale, Color color)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = objectName;
            go.transform.SetParent(parent, worldPositionStays: false);
            go.transform.localPosition = localPosition;
            go.transform.localScale = localScale;

            Collider collider = go.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = CreateTransparentMaterial(color);

            return go;
        }

        private static void SetAlpha(Material material, float alpha)
        {
            if (material == null)
                return;

            Color color = material.color;
            color.a = Mathf.Clamp01(alpha);
            material.color = color;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);
        }

        private static Material CreateTransparentMaterial(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Sprites/Default");

            Material material = new Material(shader);
            material.color = color;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);

            if (material.HasProperty("_Mode"))
                material.SetFloat("_Mode", 3f);

            if (material.HasProperty("_Surface"))
                material.SetFloat("_Surface", 1f);

            if (material.HasProperty("_SrcBlend"))
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);

            if (material.HasProperty("_DstBlend"))
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            if (material.HasProperty("_ZWrite"))
                material.SetInt("_ZWrite", 0);

            material.EnableKeyword("_ALPHABLEND_ON");
            material.renderQueue = 3000;
            return material;
        }
    }
}
