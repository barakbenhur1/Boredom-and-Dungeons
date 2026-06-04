using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDPlayerDodgeAfterimage : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.20f;
        [SerializeField] private Color color = new Color(0.35f, 0.90f, 1f, 0.46f);
        [SerializeField] private float startLength = 1.15f;
        [SerializeField] private float endLength = 1.95f;
        [SerializeField] private float startWidth = 0.36f;
        [SerializeField] private float endWidth = 0.08f;

        private float timer;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private MaterialPropertyBlock block;
        private Mesh mesh;
        private static Material sharedMaterial;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        public static void Spawn(Vector3 position, Quaternion rotation, float radius, float height)
        {
            if (!BDPerformanceGuard.AllowCosmeticSpawn(0.62f))
                return;

            Vector3 forward = rotation * Vector3.forward;
            forward.y = 0f;

            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.forward;

            forward.Normalize();

            Vector3 groundPosition = ResolveGroundPosition(position);
            Quaternion groundRotation = Quaternion.LookRotation(forward, Vector3.up);

            GameObject go = new GameObject("BD_Player_Dodge_Ground_Trail");
            go.transform.position = groundPosition;
            go.transform.rotation = groundRotation;

            BDPlayerDodgeAfterimage visual = go.AddComponent<BDPlayerDodgeAfterimage>();

            float safeRadius = Mathf.Max(0.22f, radius);
            visual.Configure(safeRadius);
        }

        private static Vector3 ResolveGroundPosition(Vector3 position)
        {
            Vector3 rayOrigin = position + Vector3.up * 1.25f;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 4.0f, ~0, QueryTriggerInteraction.Ignore))
                return hit.point + Vector3.up * 0.045f;

            position.y += 0.045f;
            return position;
        }

        private void Configure(float radius)
        {
            startLength = Mathf.Max(0.85f, radius * 2.1f);
            endLength = Mathf.Max(1.45f, radius * 3.3f);
            startWidth = Mathf.Max(0.26f, radius * 0.85f);
            endWidth = Mathf.Max(0.04f, radius * 0.18f);
            lifetime = BDPerformanceGuard.ResolveEffectLifetime(0.20f);
        }

        private void Awake()
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            block = new MaterialPropertyBlock();

            mesh = new Mesh();
            mesh.name = "BD_Dodge_Ground_Trail_Mesh";
            meshFilter.sharedMesh = mesh;

            Material material = GetSharedMaterial();
            if (material != null)
                meshRenderer.sharedMaterial = material;

            RebuildMesh(0f);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / Mathf.Max(0.01f, lifetime));

            RebuildMesh(t);
            ApplyColor(t);

            if (timer >= lifetime)
                Destroy(gameObject);
        }

        private void RebuildMesh(float t)
        {
            if (mesh == null)
                return;

            float length = Mathf.Lerp(startLength, endLength, t);
            float frontWidth = Mathf.Lerp(startWidth, endWidth, t);
            float backWidth = Mathf.Lerp(startWidth * 0.18f, endWidth * 0.10f, t);
            float sideFade = Mathf.Lerp(0.28f, 0.04f, t);

            // Flat 2D tapered streak on XZ plane.
            Vector3[] vertices =
            {
                new Vector3(-backWidth, 0f, -length * 0.38f),
                new Vector3(backWidth, 0f, -length * 0.38f),
                new Vector3(frontWidth, 0f, length * 0.30f),
                new Vector3(sideFade, 0f, length * 0.48f),
                new Vector3(0f, 0f, length * 0.58f),
                new Vector3(-sideFade, 0f, length * 0.48f),
                new Vector3(-frontWidth, 0f, length * 0.30f),
            };

            int[] triangles =
            {
                0, 1, 2,
                0, 2, 6,
                6, 2, 3,
                6, 3, 5,
                5, 3, 4
            };

            Vector2[] uvs =
            {
                new Vector2(0.45f, 0f),
                new Vector2(0.55f, 0f),
                new Vector2(1f, 0.68f),
                new Vector2(0.62f, 0.90f),
                new Vector2(0.50f, 1f),
                new Vector2(0.38f, 0.90f),
                new Vector2(0f, 0.68f),
            };

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }

        private void ApplyColor(float t)
        {
            if (meshRenderer == null)
                return;

            float alpha = Mathf.Pow(1f - t, 1.65f);
            Color c = color;
            c.a *= alpha;

            meshRenderer.GetPropertyBlock(block);
            block.SetColor(BaseColorId, c);
            block.SetColor(ColorId, c);
            block.SetColor(EmissionColorId, c * Mathf.Lerp(1.4f, 0.2f, t));
            meshRenderer.SetPropertyBlock(block);
        }

        private static Material GetSharedMaterial()
        {
            if (sharedMaterial != null)
                return sharedMaterial;

            Shader shader = Shader.Find("Sprites/Default");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Hidden/InternalErrorShader");

            if (shader != null)
            {
                sharedMaterial = new Material(shader);
                sharedMaterial.color = Color.white;
                return sharedMaterial;
            }

            Material builtIn = Resources.GetBuiltinResource<Material>("Default-Material.mat");
            if (builtIn != null)
                sharedMaterial = new Material(builtIn);

            return sharedMaterial;
        }
    }
}
