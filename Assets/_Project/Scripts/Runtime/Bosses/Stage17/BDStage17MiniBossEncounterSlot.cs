using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDStage17MiniBossPrefabRegistry : MonoBehaviour
    {
        [SerializeField] private GameObject squareJumperPrefab;
        [SerializeField] private GameObject rollerPrefab;
        [SerializeField] private GameObject serpentPrefab;
        [SerializeField] private GameObject quadGunnersPrefab;

        public GameObject Resolve(BDStage17MiniBossId id)
        {
            switch (id)
            {
                case BDStage17MiniBossId.SquareJumper:
                    return squareJumperPrefab;

                case BDStage17MiniBossId.Roller:
                    return rollerPrefab;

                case BDStage17MiniBossId.Serpent:
                    return serpentPrefab;

                case BDStage17MiniBossId.QuadGunners:
                    return quadGunnersPrefab;

                default:
                    return null;
            }
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDStage17MiniBossEncounterSlot : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform spawnedEncounterParent;

        [Header("Placeholder")]
        [SerializeField] private bool createPlaceholderWhenPrefabMissing = true;
        [SerializeField] private Vector3 placeholderScale =
            new Vector3(2.2f, 2.2f, 2.2f);

        [Header("Runtime")]
        [SerializeField] private GameObject currentEncounter;

        public GameObject CurrentEncounter => currentEncounter;

        private void Reset()
        {
            spawnPoint = transform;
        }

        public GameObject Spawn(
            BDStage17MiniBossId miniBoss,
            BDStage17MiniBossRole role,
            int runSeed,
            string candidateId,
            GameObject prefab)
        {
            Clear();

            Transform resolvedSpawnPoint =
                spawnPoint != null ? spawnPoint : transform;

            if (prefab != null)
            {
                currentEncounter = Instantiate(
                    prefab,
                    resolvedSpawnPoint.position,
                    resolvedSpawnPoint.rotation,
                    spawnedEncounterParent
                );
            }
            else if (createPlaceholderWhenPrefabMissing)
            {
                currentEncounter = CreatePlaceholder(
                    miniBoss,
                    role,
                    resolvedSpawnPoint
                );
            }

            if (currentEncounter == null)
                return null;

            currentEncounter.name =
                $"BD_Stage17_{miniBoss}_{role}";

            BDStage17MiniBossIdentity identity =
                currentEncounter.GetComponent<BDStage17MiniBossIdentity>();

            if (identity == null)
            {
                identity =
                    currentEncounter.AddComponent<BDStage17MiniBossIdentity>();
            }

            identity.Configure(
                miniBoss,
                role,
                runSeed,
                candidateId
            );

            return currentEncounter;
        }

        public void Clear()
        {
            if (currentEncounter == null)
                return;

            if (Application.isPlaying)
                Destroy(currentEncounter);
            else
                DestroyImmediate(currentEncounter);

            currentEncounter = null;
        }

        private GameObject CreatePlaceholder(
            BDStage17MiniBossId miniBoss,
            BDStage17MiniBossRole role,
            Transform resolvedSpawnPoint)
        {
            PrimitiveType primitive = ResolvePrimitive(miniBoss);

            GameObject placeholder =
                GameObject.CreatePrimitive(primitive);

            placeholder.transform.position =
                resolvedSpawnPoint.position;
            placeholder.transform.rotation =
                resolvedSpawnPoint.rotation;
            placeholder.transform.localScale =
                placeholderScale;

            if (spawnedEncounterParent != null)
            {
                placeholder.transform.SetParent(
                    spawnedEncounterParent,
                    worldPositionStays: true
                );
            }

            Renderer renderer = placeholder.GetComponent<Renderer>();

            if (renderer != null)
            {
                Material material = CreatePlaceholderMaterial(
                    ResolveRoleColor(role)
                );

                if (material != null)
                    renderer.material = material;
            }

            return placeholder;
        }

        private static PrimitiveType ResolvePrimitive(
            BDStage17MiniBossId miniBoss)
        {
            switch (miniBoss)
            {
                case BDStage17MiniBossId.Roller:
                    return PrimitiveType.Sphere;

                case BDStage17MiniBossId.Serpent:
                    return PrimitiveType.Capsule;

                case BDStage17MiniBossId.QuadGunners:
                    return PrimitiveType.Cylinder;

                default:
                    return PrimitiveType.Cube;
            }
        }

        private static Color ResolveRoleColor(
            BDStage17MiniBossRole role)
        {
            switch (role)
            {
                case BDStage17MiniBossRole.GameBoyGuardian:
                    return new Color(0.24f, 0.92f, 0.46f, 1f);

                case BDStage17MiniBossRole.CartridgeGuardian:
                    return new Color(0.35f, 0.64f, 1f, 1f);

                case BDStage17MiniBossRole.PreBoss:
                    return new Color(1f, 0.40f, 0.15f, 1f);

                default:
                    return Color.white;
            }
        }

        private static Material CreatePlaceholderMaterial(Color color)
        {
            Shader shader =
                Shader.Find("Universal Render Pipeline/Lit");

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            if (shader == null)
                return null;

            Material material = new Material(shader);
            material.color = color;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * 2.1f);
            }

            return material;
        }
    }
}
