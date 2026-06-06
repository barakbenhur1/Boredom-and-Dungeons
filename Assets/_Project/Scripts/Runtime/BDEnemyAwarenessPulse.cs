using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-120)]
    [DisallowMultipleComponent]
    public sealed class BDEnemyAwarenessPulse : MonoBehaviour
    {
        private static readonly BindingFlags FieldFlags =
            BindingFlags.Instance |
            BindingFlags.NonPublic;

        private static readonly Dictionary<System.Type, FieldInfo>
            TargetFields =
                new Dictionary<System.Type, FieldInfo>();

        [SerializeField] private float refreshInterval = 0.08f;

        private MonoBehaviour[] enemyBrains;
        private BDHealth health;
        private float nextRefreshAt;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void EnsureBootstrap()
        {
            if (Object.FindFirstObjectByType<Bootstrap>() != null)
                return;

            GameObject root =
                new GameObject(
                    "B&D Enemy Awareness Bootstrap"
                );

            root.AddComponent<Bootstrap>();
        }

        private void Awake()
        {
            health = GetComponent<BDHealth>();
            CacheEnemyBrains();
            ApplyMinimumRanges();
        }

        private void OnEnable()
        {
            nextRefreshAt = -999f;
        }

        private void Update()
        {
            if (health != null && health.IsDead)
                return;

            if (Time.unscaledTime < nextRefreshAt)
                return;

            nextRefreshAt =
                Time.unscaledTime +
                Mathf.Max(
                    0.04f,
                    refreshInterval
                );

            Transform player =
                BDTargetFinder.FindPlayer();

            if (player == null)
            {
                BDTargetFinder.ClearCache();
                return;
            }

            for (int index = 0;
                 index < enemyBrains.Length;
                 index++)
            {
                MonoBehaviour brain =
                    enemyBrains[index];

                if (brain == null || !brain.enabled)
                    continue;

                FieldInfo targetField =
                    GetTargetField(
                        brain.GetType()
                    );

                if (targetField == null)
                    continue;

                targetField.SetValue(
                    brain,
                    player
                );
            }
        }

        private void CacheEnemyBrains()
        {
            List<MonoBehaviour> brains =
                new List<MonoBehaviour>();

            AddIfPresent<BDSwordEnemy>(brains);
            AddIfPresent<BDPatrolGuardEnemy>(brains);
            AddIfPresent<BDChargerEnemy>(brains);
            AddIfPresent<BDJumperEnemy>(brains);
            AddIfPresent<BDTrapLayerEnemy>(brains);
            AddIfPresent<BDRangedShooterEnemy>(brains);
            AddIfPresent<BDExitBlockerEnemy>(brains);
            AddIfPresent<BDEnemyExitInterference>(brains);

            enemyBrains = brains.ToArray();
        }

        private void AddIfPresent<T>(
            List<MonoBehaviour> brains)
            where T : MonoBehaviour
        {
            T brain = GetComponent<T>();

            if (brain != null)
                brains.Add(brain);
        }

        private static FieldInfo GetTargetField(
            System.Type type)
        {
            if (TargetFields.TryGetValue(
                    type,
                    out FieldInfo cached))
            {
                return cached;
            }

            FieldInfo field =
                type.GetField(
                    "target",
                    FieldFlags
                ) ??
                type.GetField(
                    "player",
                    FieldFlags
                );

            TargetFields[type] = field;
            return field;
        }

        private void ApplyMinimumRanges()
        {
            MonoBehaviour[] components =
                GetComponents<MonoBehaviour>();

            for (int index = 0;
                 index < components.Length;
                 index++)
            {
                MonoBehaviour component =
                    components[index];

                if (component == null)
                    continue;

                ApplyMinimum(
                    component,
                    "aggroRange",
                    18.5f
                );

                ApplyMinimum(
                    component,
                    "guardRadius",
                    17f
                );

                ApplyMinimum(
                    component,
                    "chaseGiveUpRadius",
                    24f
                );

                ApplyMinimum(
                    component,
                    "attackRange",
                    1.95f
                );

                ApplyMinimum(
                    component,
                    "windupRange",
                    7.2f
                );

                ApplyMinimum(
                    component,
                    "plantRange",
                    5f
                );

                ApplyMinimum(
                    component,
                    "maxShootRange",
                    16.5f
                );

                ApplyMinimum(
                    component,
                    "maxJumpDistance",
                    10.5f
                );
            }
        }

        private static void ApplyMinimum(
            MonoBehaviour component,
            string fieldName,
            float minimum)
        {
            FieldInfo field =
                component.GetType().GetField(
                    fieldName,
                    FieldFlags
                );

            if (field == null ||
                field.FieldType != typeof(float))
            {
                return;
            }

            float value =
                (float)field.GetValue(component);

            if (value < minimum)
            {
                field.SetValue(
                    component,
                    minimum
                );
            }
        }

        private sealed class Bootstrap : MonoBehaviour
        {
            private float nextScanAt;

            private void Update()
            {
                if (Time.unscaledTime < nextScanAt)
                    return;

                nextScanAt =
                    Time.unscaledTime + 0.35f;

                BDHealth[] candidates =
                    Object.FindObjectsByType<BDHealth>(
                        FindObjectsInactive.Exclude,
                        FindObjectsSortMode.None
                    );

                for (int index = 0;
                     index < candidates.Length;
                     index++)
                {
                    BDHealth candidate =
                        candidates[index];

                    if (candidate == null ||
                        candidate.IsDead ||
                        candidate.GetComponent<BDPlayerMarker>() != null ||
                        candidate.GetComponent<BDHorseHealth>() != null)
                    {
                        continue;
                    }

                    if (!LooksLikeEnemy(candidate.gameObject))
                        continue;

                    if (candidate.GetComponent<
                            BDEnemyAwarenessPulse>() == null)
                    {
                        candidate.gameObject.AddComponent<
                            BDEnemyAwarenessPulse>();
                    }
                }
            }

            private static bool LooksLikeEnemy(
                GameObject candidate)
            {
                return
                    candidate.GetComponent<BDSwordEnemy>() != null ||
                    candidate.GetComponent<BDPatrolGuardEnemy>() != null ||
                    candidate.GetComponent<BDChargerEnemy>() != null ||
                    candidate.GetComponent<BDJumperEnemy>() != null ||
                    candidate.GetComponent<BDTrapLayerEnemy>() != null ||
                    candidate.GetComponent<BDRangedShooterEnemy>() != null ||
                    candidate.GetComponent<BDExitBlockerEnemy>() != null ||
                    candidate.GetComponent<BDEnemyExitInterference>() != null;
            }
        }
    }
}
