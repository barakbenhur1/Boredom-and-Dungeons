using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDMeleeSlashArcVisual : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.13f;
        [SerializeField] private bool heavy;

        private float timer;
        private MeshRenderer meshRenderer;
        private MaterialPropertyBlock block;
        private static Material sharedMaterial;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        public static void Spawn(Vector3 origin, Vector3 direction, float range, float radius, bool isHeavy)
        {
            if (!BDPerformanceGuard.AllowCosmeticSpawn(isHeavy ? 0.75f : 0.55f))
                return;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                direction = Vector3.forward;

            direction.Normalize();

            GameObject go = new GameObject(isHeavy ? "BD_Heavy_Slash_Arc" : "BD_Light_Slash_Arc");
            go.transform.position = origin + Vector3.up * (isHeavy ? 1.05f : 0.95f);
            go.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();

            float arcDegrees = isHeavy ? 92f : 68f;
            float innerRadius = Mathf.Max(0.35f, range * 0.32f);
            float outerRadius = Mathf.Max(innerRadius + 0.25f, range + radius * (isHeavy ? 0.60f : 0.35f));
            filter.sharedMesh = BuildArcMesh(innerRadius, outerRadius, arcDegrees, isHeavy ? 18 : 14);

            Material material = GetSharedMaterial();
            if (material != null)
                renderer.sharedMaterial = material;

            BDMeleeSlashArcVisual visual = go.AddComponent<BDMeleeSlashArcVisual>();
            visual.Configure(isHeavy);
        }

        private void Configure(bool isHeavy)
        {
            heavy = isHeavy;
            lifetime = BDPerformanceGuard.ResolveEffectLifetime(heavy ? 0.17f : 0.115f);
        }

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            block = new MaterialPropertyBlock();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / Mathf.Max(0.01f, lifetime));

            float alpha = 1f - t;
            float expand = 1f + t * (heavy ? 0.28f : 0.18f);
            transform.localScale = new Vector3(expand, 1f, expand);

            if (meshRenderer != null)
            {
                Color color = heavy
                    ? new Color(1f, 0.45f, 0.08f, alpha * 0.76f)
                    : new Color(0.90f, 0.95f, 1f, alpha * 0.56f);

                meshRenderer.GetPropertyBlock(block);
                block.SetColor(BaseColorId, color);
                block.SetColor(ColorId, color);
                block.SetColor(EmissionColorId, color * (heavy ? 2.0f : 1.2f));
                meshRenderer.SetPropertyBlock(block);
            }

            if (timer >= lifetime)
                Destroy(gameObject);
        }

        private static Mesh BuildArcMesh(float innerRadius, float outerRadius, float arcDegrees, int segments)
        {
            Mesh mesh = new Mesh();
            mesh.name = "BD_Melee_Slash_Arc_Mesh";

            segments = Mathf.Max(3, segments);
            int vertexCount = (segments + 1) * 2;

            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] uvs = new Vector2[vertexCount];
            int[] triangles = new int[segments * 6];

            float start = -arcDegrees * 0.5f;
            float step = arcDegrees / segments;

            for (int i = 0; i <= segments; i++)
            {
                float angle = (start + step * i) * Mathf.Deg2Rad;
                float sin = Mathf.Sin(angle);
                float cos = Mathf.Cos(angle);

                int innerIndex = i * 2;
                int outerIndex = innerIndex + 1;

                vertices[innerIndex] = new Vector3(sin * innerRadius, 0f, cos * innerRadius);
                vertices[outerIndex] = new Vector3(sin * outerRadius, 0f, cos * outerRadius);

                float u = i / (float)segments;
                uvs[innerIndex] = new Vector2(u, 0f);
                uvs[outerIndex] = new Vector2(u, 1f);
            }

            int tri = 0;
            for (int i = 0; i < segments; i++)
            {
                int a = i * 2;
                int b = a + 1;
                int c = a + 2;
                int d = a + 3;

                triangles[tri++] = a;
                triangles[tri++] = b;
                triangles[tri++] = c;

                triangles[tri++] = c;
                triangles[tri++] = b;
                triangles[tri++] = d;
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
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
                    sharedMaterial.SetColor("_EmissionColor", Color.white);
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
