using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDSpinAttackVisual : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.30f;
        [SerializeField] private float rotations = 1.55f;

        private Transform owner;
        private float radius;
        private float timer;
        private MeshRenderer[] arcRenderers;
        private MaterialPropertyBlock propertyBlock;

        private static Material sharedMaterial;
        private static readonly int BaseColorId =
            Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId =
            Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId =
            Shader.PropertyToID("_EmissionColor");

        public static void Spawn(
            Transform attackOwner,
            float attackRadius,
            float animationDuration)
        {
            if (attackOwner == null)
                return;

            if (!BDPerformanceGuard.AllowCosmeticSpawn(0.78f))
                return;

            GameObject root =
                new GameObject("BD_Spin_Attack_AOE_Visual");

            BDSpinAttackVisual visual =
                root.AddComponent<BDSpinAttackVisual>();

            visual.Configure(
                attackOwner,
                attackRadius,
                animationDuration
            );
        }

        private void Configure(
            Transform attackOwner,
            float attackRadius,
            float animationDuration)
        {
            owner = attackOwner;
            radius = Mathf.Max(0.65f, attackRadius);
            lifetime = BDPerformanceGuard.ResolveEffectLifetime(
                Mathf.Max(0.18f, animationDuration)
            );

            transform.position =
                owner.position + Vector3.up * 0.92f;

            propertyBlock = new MaterialPropertyBlock();
            arcRenderers = new MeshRenderer[3];

            for (int index = 0;
                 index < arcRenderers.Length;
                 index++)
            {
                GameObject arc =
                    new GameObject($"BD_Spin_Arc_{index + 1}");

                arc.transform.SetParent(
                    transform,
                    worldPositionStays: false
                );

                arc.transform.localRotation =
                    Quaternion.Euler(
                        0f,
                        index * 120f,
                        0f
                    );

                MeshFilter filter =
                    arc.AddComponent<MeshFilter>();

                MeshRenderer renderer =
                    arc.AddComponent<MeshRenderer>();

                filter.sharedMesh = BuildArcMesh(
                    radius * 0.30f,
                    radius,
                    102f,
                    20
                );

                Material material = GetSharedMaterial();
                if (material != null)
                    renderer.sharedMaterial = material;

                arcRenderers[index] = renderer;
            }
        }

        private void Update()
        {
            if (owner == null)
            {
                Destroy(gameObject);
                return;
            }

            timer += Time.unscaledDeltaTime;

            float duration = Mathf.Max(0.01f, lifetime);
            float t = Mathf.Clamp01(timer / duration);
            float eased = 1f - Mathf.Pow(1f - t, 3f);

            transform.position =
                owner.position + Vector3.up * 0.92f;

            transform.rotation = Quaternion.Euler(
                0f,
                eased * 360f * rotations,
                0f
            );

            float scale =
                Mathf.Lerp(0.72f, 1.08f, eased);

            transform.localScale =
                new Vector3(scale, 1f, scale);

            float fadeIn =
                Mathf.Clamp01(t / 0.12f);

            float fadeOut =
                1f - Mathf.Clamp01(
                    (t - 0.52f) / 0.48f
                );

            float alpha = fadeIn * fadeOut;

            if (arcRenderers != null)
            {
                for (int index = 0;
                     index < arcRenderers.Length;
                     index++)
                {
                    MeshRenderer renderer = arcRenderers[index];
                    if (renderer == null)
                        continue;

                    float phase =
                        0.82f + index * 0.08f;

                    Color color = new Color(
                        0.72f + phase * 0.10f,
                        0.88f,
                        1f,
                        alpha * 0.72f
                    );

                    renderer.GetPropertyBlock(propertyBlock);
                    propertyBlock.SetColor(BaseColorId, color);
                    propertyBlock.SetColor(ColorId, color);
                    propertyBlock.SetColor(
                        EmissionColorId,
                        color * 1.65f
                    );
                    renderer.SetPropertyBlock(propertyBlock);
                }
            }

            if (timer >= lifetime)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            MeshFilter[] filters =
                GetComponentsInChildren<MeshFilter>();

            for (int index = 0;
                 index < filters.Length;
                 index++)
            {
                if (filters[index] != null &&
                    filters[index].sharedMesh != null)
                {
                    Destroy(filters[index].sharedMesh);
                }
            }
        }

        private static Mesh BuildArcMesh(
            float innerRadius,
            float outerRadius,
            float arcDegrees,
            int segments)
        {
            Mesh mesh = new Mesh
            {
                name = "BD_Spin_Attack_Arc_Mesh"
            };

            segments = Mathf.Max(3, segments);

            int vertexCount = (segments + 1) * 2;
            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] uvs = new Vector2[vertexCount];
            int[] triangles = new int[segments * 6];

            float start = -arcDegrees * 0.5f;
            float step = arcDegrees / segments;

            for (int index = 0;
                 index <= segments;
                 index++)
            {
                float angle =
                    (start + step * index) * Mathf.Deg2Rad;

                float sin = Mathf.Sin(angle);
                float cos = Mathf.Cos(angle);

                int innerIndex = index * 2;
                int outerIndex = innerIndex + 1;

                vertices[innerIndex] =
                    new Vector3(
                        sin * innerRadius,
                        0f,
                        cos * innerRadius
                    );

                vertices[outerIndex] =
                    new Vector3(
                        sin * outerRadius,
                        0f,
                        cos * outerRadius
                    );

                float u = index / (float)segments;
                uvs[innerIndex] = new Vector2(u, 0f);
                uvs[outerIndex] = new Vector2(u, 1f);
            }

            int triangle = 0;

            for (int index = 0;
                 index < segments;
                 index++)
            {
                int a = index * 2;
                int b = a + 1;
                int c = a + 2;
                int d = a + 3;

                triangles[triangle++] = a;
                triangles[triangle++] = b;
                triangles[triangle++] = c;
                triangles[triangle++] = c;
                triangles[triangle++] = b;
                triangles[triangle++] = d;
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

            Shader shader =
                Shader.Find("Universal Render Pipeline/Lit");

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
