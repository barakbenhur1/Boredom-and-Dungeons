using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDCollectibleGuardianSpawner : MonoBehaviour
    {
        [Header("Trigger")]
        [SerializeField] private float triggerRadius = 7.5f;
        [SerializeField] private bool spawnOnlyOnce = true;

        [Header("Guardians")]
        [SerializeField] private int swordGuardians = 2;
        [SerializeField] private int chargerGuardians = 1;
        [SerializeField] private float spawnDistance = 5.8f;
        [SerializeField] private float spawnArcDegrees = 140f;
        [SerializeField] private float swordHealth = 170f;
        [SerializeField] private float chargerHealth = 230f;

        [Header("Debug")]
        [SerializeField] private bool logSpawn = true;

        private bool spawned;
        private Transform player;

        public void Configure(float radius, int swords, int chargers, float distance)
        {
            triggerRadius = Mathf.Max(1.5f, radius);
            swordGuardians = Mathf.Max(0, swords);
            chargerGuardians = Mathf.Max(0, chargers);
            spawnDistance = Mathf.Max(2.5f, distance);
        }

        private void Update()
        {
            if (spawned && spawnOnlyOnce)
                return;

            if (player == null)
                player = BDTargetFinder.FindPlayer();

            if (player == null)
                return;

            Vector3 delta = player.position - transform.position;
            delta.y = 0f;

            if (delta.sqrMagnitude > triggerRadius * triggerRadius)
                return;

            SpawnGuardians(player);
        }

        private void SpawnGuardians(Transform playerTransform)
        {
            spawned = true;

            Vector3 awayFromPlayer = transform.position - playerTransform.position;
            awayFromPlayer.y = 0f;

            if (awayFromPlayer.sqrMagnitude < 0.001f)
                awayFromPlayer = -playerTransform.forward;

            awayFromPlayer.y = 0f;

            if (awayFromPlayer.sqrMagnitude < 0.001f)
                awayFromPlayer = Vector3.forward;

            awayFromPlayer.Normalize();

            int total = swordGuardians + chargerGuardians;
            int index = 0;

            for (int i = 0; i < swordGuardians; i++)
            {
                Vector3 position = ResolveSpawnPosition(awayFromPlayer, index, total);
                CreateGuardian(position, GuardianType.Sword);
                index++;
            }

            for (int i = 0; i < chargerGuardians; i++)
            {
                Vector3 position = ResolveSpawnPosition(awayFromPlayer, index, total);
                CreateGuardian(position, GuardianType.Charger);
                index++;
            }

            if (logSpawn)
                Debug.Log($"B&D Collectible guardians spawned near {name}: swords={swordGuardians}, chargers={chargerGuardians}");
        }

        private Vector3 ResolveSpawnPosition(Vector3 baseDirection, int index, int total)
        {
            total = Mathf.Max(1, total);
            float t = total == 1 ? 0.5f : index / Mathf.Max(1f, total - 1f);
            float angle = Mathf.Lerp(-spawnArcDegrees * 0.5f, spawnArcDegrees * 0.5f, t);
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * baseDirection;
            Vector3 position = transform.position + direction.normalized * spawnDistance;
            position.y = transform.position.y + 1.05f;
            return position;
        }

        private enum GuardianType
        {
            Sword,
            Charger
        }

        private GameObject CreateGuardian(Vector3 position, GuardianType type)
        {
            GameObject guardian = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            guardian.name = type == GuardianType.Sword ? "BD_Collectible_Guardian_Sword" : "BD_Collectible_Guardian_Charger";
            guardian.transform.position = position;

            CapsuleCollider capsule = guardian.GetComponent<CapsuleCollider>();
            if (capsule != null)
                Destroy(capsule);

            CharacterController controller = guardian.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.38f;
            controller.center = Vector3.zero;
            controller.stepOffset = 0.25f;

            Renderer renderer = guardian.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color color = type == GuardianType.Sword
                    ? new Color(0.19f, 0.04f, 0.05f, 1f)
                    : new Color(0.26f, 0.05f, 0.03f, 1f);

                renderer.sharedMaterial = CreateMaterial(color, color * 0.25f);
            }

            BDHealth health = guardian.AddComponent<BDHealth>();
            health.SetMaxHealth(type == GuardianType.Sword ? swordHealth : chargerHealth, true);

            guardian.AddComponent<BDEnemyBootstrap>();
            guardian.AddComponent<BDKnockbackReceiver>();
            guardian.AddComponent<BDEnemyGroundStick>();
            guardian.AddComponent<BDEnemyCollisionDiscipline>();

            if (type == GuardianType.Sword)
                guardian.AddComponent<BDSwordEnemy>();
            else
                guardian.AddComponent<BDChargerEnemy>();

            CreateDarkAura(guardian.transform, type);
            return guardian;
        }

        private static void CreateDarkAura(Transform parent, GuardianType type)
        {
            GameObject aura = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            aura.name = type == GuardianType.Sword ? "BD_Guardian_Dark_Aura" : "BD_Guardian_Heavy_Dark_Aura";
            aura.transform.SetParent(parent, worldPositionStays: false);
            aura.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            aura.transform.localScale = type == GuardianType.Sword ? Vector3.one * 2.9f : Vector3.one * 3.35f;

            Collider collider = aura.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            Renderer renderer = aura.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = CreateTransparentMaterial(new Color(0.02f, 0.0f, 0.0f, type == GuardianType.Sword ? 0.24f : 0.30f));
        }

        private static Material CreateMaterial(Color color, Color emission)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Sprites/Default");

            Material material = new Material(shader);
            material.color = color;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", emission);
            }

            return material;
        }

        private static Material CreateTransparentMaterial(Color color)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Sprites/Default");

            Material material = new Material(shader);
            material.color = color;

            if (material.HasProperty("_Mode"))
                material.SetFloat("_Mode", 3f);

            if (material.HasProperty("_SrcBlend"))
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);

            if (material.HasProperty("_DstBlend"))
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            if (material.HasProperty("_ZWrite"))
                material.SetInt("_ZWrite", 0);

            material.renderQueue = 3000;
            return material;
        }
    }
}
