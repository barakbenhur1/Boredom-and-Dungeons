using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBossSummonEmitter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BDBossEncounterController encounter;
        [SerializeField] private BDBossSharedSummonBudget budget;
        [SerializeField] private Transform spawnedEnemiesParent;

        [Header("Fixed summon pool")]
        [Tooltip(
            "For a fixed summon identity, place only one prefab here. " +
            "For the final boss, multiple prefabs may be used randomly."
        )]
        [SerializeField] private GameObject[] summonPrefabs;

        [Header("Spawn points")]
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float fallbackSpawnRadius = 2.2f;

        [Header("Wave")]
        [SerializeField, Min(1)] private int minimumPerWave = 2;
        [SerializeField, Min(1)] private int maximumPerWave = 2;
        [SerializeField] private bool randomizePrefab = true;
        [SerializeField] private bool randomizeSpawnPoint = true;

        [Header("Automatic timing")]
        [SerializeField] private bool summonAutomatically;
        [SerializeField] private float firstWaveDelay = 1.5f;
        [SerializeField] private float waveCooldown = 10f;

        [Header("Conditions")]
        [SerializeField] private bool requireActiveEncounter = true;
        [SerializeField] private bool stopWhenOwnerHealthIsZero = true;
        [SerializeField] private BDBossHealthChannel ownerHealthChannel;

        [Header("Debug")]
        [SerializeField] private bool logSummons;

        private float nextAutomaticWaveAt;

        public BDBossSharedSummonBudget Budget => budget;

        private void Awake()
        {
            ResolveReferences();
        }

        private void OnEnable()
        {
            ResolveReferences();
            ScheduleFirstWave();
        }

        private void OnValidate()
        {
            minimumPerWave = Mathf.Max(1, minimumPerWave);
            maximumPerWave = Mathf.Max(
                minimumPerWave,
                maximumPerWave
            );
            fallbackSpawnRadius =
                Mathf.Max(0f, fallbackSpawnRadius);
            firstWaveDelay = Mathf.Max(0f, firstWaveDelay);
            waveCooldown = Mathf.Max(0.05f, waveCooldown);
        }

        private void Update()
        {
            if (!summonAutomatically)
                return;

            if (!CanEmitterSummon())
                return;

            if (Time.time < nextAutomaticWaveAt)
                return;

            TrySummonWave();
            nextAutomaticWaveAt =
                Time.time + Mathf.Max(0.05f, waveCooldown);
        }

        public int TrySummonWave()
        {
            if (!CanEmitterSummon() ||
                budget == null ||
                summonPrefabs == null ||
                summonPrefabs.Length == 0)
            {
                return 0;
            }

            int requested = Random.Range(
                minimumPerWave,
                maximumPerWave + 1
            );

            int allowed = budget.ClampToAvailable(requested);

            if (allowed <= 0)
                return 0;

            int spawned = 0;

            for (int i = 0; i < allowed; i++)
            {
                GameObject prefab = ResolvePrefab(i);

                if (prefab == null)
                    continue;

                ResolveSpawnPose(
                    i,
                    out Vector3 position,
                    out Quaternion rotation
                );

                if (budget.TrySpawn(
                        prefab,
                        position,
                        rotation,
                        spawnedEnemiesParent,
                        out GameObject instance))
                {
                    spawned++;

                    if (logSummons)
                    {
                        Debug.Log(
                            $"{name} summoned {instance.name}. " +
                            $"Alive={budget.AliveSummons}/" +
                            $"{budget.MaximumAliveSummons}",
                            this
                        );
                    }
                }
            }

            return spawned;
        }

        public bool TrySummonSingle(GameObject prefab)
        {
            if (!CanEmitterSummon() ||
                budget == null ||
                prefab == null)
            {
                return false;
            }

            ResolveSpawnPose(
                budget.AliveSummons,
                out Vector3 position,
                out Quaternion rotation
            );

            return budget.TrySpawn(
                prefab,
                position,
                rotation,
                spawnedEnemiesParent,
                out _
            );
        }

        public void ScheduleFirstWave()
        {
            nextAutomaticWaveAt =
                Time.time + Mathf.Max(0f, firstWaveDelay);
        }

        public void ConfigureAutomaticTiming(
            bool enabled,
            float initialDelay,
            float cooldown)
        {
            summonAutomatically = enabled;
            firstWaveDelay = Mathf.Max(0f, initialDelay);
            waveCooldown = Mathf.Max(0.05f, cooldown);
            ScheduleFirstWave();
        }

        private bool CanEmitterSummon()
        {
            ResolveReferences();

            if (budget == null || budget.IsFull)
                return false;

            if (requireActiveEncounter &&
                (encounter == null ||
                 !encounter.IsCombatActive))
            {
                return false;
            }

            if (stopWhenOwnerHealthIsZero &&
                ownerHealthChannel != null &&
                ownerHealthChannel.IsAtZero)
            {
                return false;
            }

            return true;
        }

        private void ResolveReferences()
        {
            if (encounter == null)
            {
                encounter =
                    GetComponentInParent<BDBossEncounterController>();
            }

            if (budget == null)
            {
                budget =
                    GetComponentInParent<BDBossSharedSummonBudget>();
            }

            if (spawnedEnemiesParent == null &&
                budget != null)
            {
                spawnedEnemiesParent = budget.transform;
            }
        }

        private GameObject ResolvePrefab(int waveIndex)
        {
            if (summonPrefabs == null ||
                summonPrefabs.Length == 0)
            {
                return null;
            }

            if (summonPrefabs.Length == 1)
                return summonPrefabs[0];

            int index = randomizePrefab
                ? Random.Range(0, summonPrefabs.Length)
                : waveIndex % summonPrefabs.Length;

            return summonPrefabs[index];
        }

        private void ResolveSpawnPose(
            int waveIndex,
            out Vector3 position,
            out Quaternion rotation)
        {
            Transform spawnPoint = ResolveSpawnPoint(waveIndex);

            if (spawnPoint != null)
            {
                position = spawnPoint.position;
                rotation = spawnPoint.rotation;
                return;
            }

            float angle =
                waveIndex * 137.5f * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(angle),
                0f,
                Mathf.Sin(angle)
            ) * fallbackSpawnRadius;

            position = transform.position + offset;
            Vector3 lookDirection =
                offset.sqrMagnitude > 0.001f
                    ? -offset.normalized
                    : transform.forward;

            rotation = Quaternion.LookRotation(
                lookDirection,
                Vector3.up
            );
        }

        private Transform ResolveSpawnPoint(int waveIndex)
        {
            if (spawnPoints == null ||
                spawnPoints.Length == 0)
            {
                return null;
            }

            int startIndex = randomizeSpawnPoint
                ? Random.Range(0, spawnPoints.Length)
                : waveIndex % spawnPoints.Length;

            for (int offset = 0;
                 offset < spawnPoints.Length;
                 offset++)
            {
                int index =
                    (startIndex + offset) % spawnPoints.Length;

                if (spawnPoints[index] != null)
                    return spawnPoints[index];
            }

            return null;
        }
    }
}
