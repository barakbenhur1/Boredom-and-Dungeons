using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDMeleeSlashArcVisual : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.13f;
        [SerializeField] private bool heavy;
        [SerializeField] private bool vertical;

        private float timer;
        private MeshRenderer meshRenderer;
        private MaterialPropertyBlock block;
        private Vector3 verticalStartPosition;
        private Vector3 verticalEndPosition;
        private Quaternion baseRotation;
        private static Material sharedMaterial;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        // BD PARRY CANCELS ACTIVE PLAYER SLASH V23R12
        // Parry freezes must never leave a slash mesh suspended until the next attack.
        public static void ClearAllActive()
        {
            BDMeleeSlashArcVisual[] visuals =
                Object.FindObjectsByType<BDMeleeSlashArcVisual>(
                    FindObjectsSortMode.None
                );

            for (int index = 0; index < visuals.Length; index++)
            {
                BDMeleeSlashArcVisual visual = visuals[index];
                if (visual != null)
                    Object.Destroy(visual.gameObject);
            }
        }

        public static void Spawn(
            Vector3 origin,
            Vector3 direction,
            float range,
            float radius,
            bool isHeavy)
        {
            SpawnInternal(origin, direction, range, radius, isHeavy, false);
        }

        // BD TRUE VERTICAL AIRBORNE MELEE ARC V23R11
        // This is the same light/heavy mesh language as the grounded slash,
        // rotated into a vertical plane and animated from high to low.
        public static void SpawnVertical(
            Vector3 origin,
            Vector3 direction,
            float range,
            float radius,
            bool isHeavy)
        {
            SpawnInternal(origin, direction, range, radius, isHeavy, true);
        }

        private static void SpawnInternal(
            Vector3 origin,
            Vector3 direction,
            float range,
            float radius,
            bool isHeavy,
            bool isVertical)
        {
            if (!BDPerformanceGuard.AllowCosmeticSpawn(isHeavy ? 0.75f : 0.55f))
                return;

            direction.y = 0f;
            if (direction.sqrMagnitude < 0.001f)
                direction = Vector3.forward;
            direction.Normalize();

            string objectName = isVertical
                ? (isHeavy
                    ? "BD_Heavy_Vertical_Airborne_Slash"
                    : "BD_Light_Vertical_Airborne_Slash")
                : (isHeavy ? "BD_Heavy_Slash_Arc" : "BD_Light_Slash_Arc");

            GameObject go = new GameObject(objectName);
            Quaternion facingRotation =
                Quaternion.LookRotation(direction, Vector3.up);

            // BD CORRECT LOCAL-X AIRBORNE ROTATION V23R19G
            // Airborne Light/Heavy reuse the exact selected grounded arc size,
            // thickness and angle. The only shape change is a 90-degree local-X
            // rotation that makes the same attack perpendicular to the player.
            Quaternion rotation = isVertical
                ? facingRotation * Quaternion.AngleAxis(-90f, Vector3.right)
                : facingRotation;

            float forwardDistance = Mathf.Clamp(
                range * (isHeavy ? 0.52f : 0.48f),
                isHeavy ? 0.92f : 0.82f,
                isHeavy ? 1.58f : 1.38f
            );
            Vector3 frontCenter = origin + direction * forwardDistance;

            Vector3 start = isVertical
                ? frontCenter + Vector3.up * (isHeavy ? 1.28f : 1.15f)
                : origin + Vector3.up * (isHeavy ? 1.05f : 0.95f);

            Vector3 end = isVertical
                ? frontCenter + Vector3.up * (isHeavy ? 0.20f : 0.24f)
                : start;

            go.transform.SetPositionAndRotation(start, rotation);

            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();

            float arcDegrees = isHeavy ? 92f : 68f;
            float innerRadius = Mathf.Max(0.35f, range * 0.32f);
            float outerRadius = Mathf.Max(
                innerRadius + 0.25f,
                range + radius * (isHeavy ? 0.60f : 0.35f)
            );

            filter.sharedMesh = BuildArcMesh(
                innerRadius,
                outerRadius,
                arcDegrees,
                isHeavy ? 18 : 14,
                verticalPlane: false
            );

            Material material = GetSharedMaterial();
            if (material != null)
                renderer.sharedMaterial = material;

            BDMeleeSlashArcVisual visual = go.AddComponent<BDMeleeSlashArcVisual>();
            visual.Configure(isHeavy, isVertical, start, end, rotation);
        }

        private void Configure(
            bool isHeavy,
            bool isVertical,
            Vector3 start,
            Vector3 end,
            Quaternion rotation)
        {
            heavy = isHeavy;
            vertical = isVertical;
            verticalStartPosition = start;
            verticalEndPosition = end;
            baseRotation = rotation;
            lifetime = BDPerformanceGuard.ResolveEffectLifetime(
                vertical
                    ? (heavy ? 0.24f : 0.17f)
                    : (heavy ? 0.17f : 0.115f)
            );
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
            float eased = 1f - Mathf.Pow(1f - t, 3f);

            if (vertical)
            {
                transform.position = Vector3.Lerp(
                    verticalStartPosition,
                    verticalEndPosition,
                    eased
                );

                transform.rotation = baseRotation;

                // Keep the selected grounded Light/Heavy arc geometry intact.
                // Only its plane and world position differ while airborne.
                float expand = 1f + t * (heavy ? 0.28f : 0.18f);
                transform.localScale = Vector3.one * expand;
            }
            else
            {
                float expand = 1f + t * (heavy ? 0.28f : 0.18f);
                transform.localScale = new Vector3(expand, 1f, expand);
            }

            float alpha = 1f - Mathf.SmoothStep(0.36f, 1f, t);

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

        private static Mesh BuildArcMesh(
            float innerRadius,
            float outerRadius,
            float arcDegrees,
            int segments,
            bool verticalPlane)
        {
            Mesh mesh = new Mesh();
            mesh.name = verticalPlane
                ? "BD_Vertical_Melee_Slash_Arc_Mesh"
                : "BD_Melee_Slash_Arc_Mesh";

            segments = Mathf.Max(3, segments);
            int vertexCount = (segments + 1) * 2;

            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] uvs = new Vector2[vertexCount];
            int[] triangles = new int[segments * 12];

            float start = -arcDegrees * 0.5f;
            float step = arcDegrees / segments;

            for (int i = 0; i <= segments; i++)
            {
                float angle = (start + step * i) * Mathf.Deg2Rad;
                float sin = Mathf.Sin(angle);
                float cos = Mathf.Cos(angle);

                int innerIndex = i * 2;
                int outerIndex = innerIndex + 1;

                if (verticalPlane)
                {
                    vertices[innerIndex] = new Vector3(sin * innerRadius, cos * innerRadius, 0f);
                    vertices[outerIndex] = new Vector3(sin * outerRadius, cos * outerRadius, 0f);
                }
                else
                {
                    vertices[innerIndex] = new Vector3(sin * innerRadius, 0f, cos * innerRadius);
                    vertices[outerIndex] = new Vector3(sin * outerRadius, 0f, cos * outerRadius);
                }

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

                // Back faces keep the vertical arc visible from every approved camera yaw.
                triangles[tri++] = c;
                triangles[tri++] = b;
                triangles[tri++] = a;
                triangles[tri++] = d;
                triangles[tri++] = b;
                triangles[tri++] = c;
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

                if (sharedMaterial.HasProperty("_Cull"))
                    sharedMaterial.SetFloat("_Cull", 0f);

                return sharedMaterial;
            }

            Material builtIn = Resources.GetBuiltinResource<Material>("Default-Material.mat");
            if (builtIn != null)
                sharedMaterial = new Material(builtIn);

            return sharedMaterial;
        }
    }
}
