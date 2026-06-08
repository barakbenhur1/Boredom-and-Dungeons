using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDCharacterDeathAnimation : MonoBehaviour
    {
        // BD RENDERER-BRANCH PLAYER AND ENEMY DEATH V23R19G
        [Header("Timing")]
        [SerializeField] private float playerDeathDuration = 1.35f;
        [SerializeField] private float enemyDeathDuration = 0.72f;
        [SerializeField] private float playerFinalPoseHold = 0.32f;

        [Header("Pose")]
        [SerializeField] private float playerFallDegrees = 82f;
        [SerializeField] private float enemyFallDegrees = 72f;
        [SerializeField] private float playerDropDistance = 0.34f;
        [SerializeField] private float enemyDropDistance = 0.28f;

        private readonly List<VisualPose> visualPoses =
            new List<VisualPose>();

        private Coroutine routine;
        private bool deathStarted;
        private bool deathCompleted;
        private float activeDuration;

        public float PlayerDeathDuration =>
            Mathf.Max(0.45f, playerDeathDuration) +
            Mathf.Max(0f, playerFinalPoseHold);

        public float EnemyDeathDuration =>
            Mathf.Max(0.25f, enemyDeathDuration);

        public bool IsPlaying => deathStarted && !deathCompleted;

        public static float PlayPlayerDeath(BDHealth health)
        {
            BDCharacterDeathAnimation animation = Resolve(health);
            return animation != null
                ? animation.Play(isPlayer: true)
                : 1.65f;
        }

        public static float PlayEnemyDeath(BDHealth health)
        {
            BDCharacterDeathAnimation animation = Resolve(health);
            return animation != null
                ? animation.Play(isPlayer: false)
                : 0.72f;
        }

        public static float GetEnemyDeathDuration(BDHealth health)
        {
            BDCharacterDeathAnimation animation = Resolve(health);
            return animation != null
                ? animation.EnemyDeathDuration
                : 0.72f;
        }

        public static bool IsDeathPresentationActive(BDHealth health)
        {
            if (health == null)
                return false;

            BDCharacterDeathAnimation animation =
                health.GetComponent<BDCharacterDeathAnimation>();

            return animation != null && animation.IsPlaying;
        }

        private static BDCharacterDeathAnimation Resolve(BDHealth health)
        {
            if (health == null)
                return null;

            BDCharacterDeathAnimation animation =
                health.GetComponent<BDCharacterDeathAnimation>();

            if (animation == null)
            {
                animation =
                    health.gameObject.AddComponent<BDCharacterDeathAnimation>();
            }

            return animation;
        }

        private float Play(bool isPlayer)
        {
            float requestedDuration = isPlayer
                ? PlayerDeathDuration
                : EnemyDeathDuration;

            if (deathStarted)
                return activeDuration;

            deathStarted = true;
            deathCompleted = false;
            activeDuration = requestedDuration;

            ResolveAndCaptureVisualBranches(isPlayer);
            DisableGameplayAfterDeath(isPlayer);

            if (visualPoses.Count > 0 && isActiveAndEnabled)
            {
                routine = StartCoroutine(
                    AnimateDeath(isPlayer)
                );
            }
            else
            {
                deathCompleted = true;
            }

            return activeDuration;
        }

        private void ResolveAndCaptureVisualBranches(bool isPlayer)
        {
            visualPoses.Clear();
            HashSet<Transform> uniqueRoots =
                new HashSet<Transform>();

            Transform explicitRoot = isPlayer
                ? transform.Find("BD_Player_Visual") ??
                  transform.Find("Visual")
                : transform.Find("Visual") ??
                  transform.Find("Model") ??
                  transform.Find("Body");

            if (explicitRoot != null)
                uniqueRoots.Add(explicitRoot);

            Renderer[] renderers =
                GetComponentsInChildren<Renderer>(true);

            for (int index = 0; index < renderers.Length; index++)
            {
                Renderer renderer = renderers[index];
                if (renderer == null ||
                    renderer is LineRenderer ||
                    renderer is ParticleSystemRenderer ||
                    IsPresentationOnlyRenderer(renderer))
                {
                    continue;
                }

                Transform branch = renderer.transform;
                while (branch.parent != null &&
                       branch.parent != transform)
                {
                    branch = branch.parent;
                }

                uniqueRoots.Add(branch);
            }

            if (uniqueRoots.Count == 0)
                uniqueRoots.Add(transform);

            foreach (Transform visual in uniqueRoots)
            {
                if (visual == null)
                    continue;

                visualPoses.Add(
                    new VisualPose(
                        visual,
                        visual.localPosition,
                        visual.localRotation,
                        visual.localScale
                    )
                );
            }
        }

        private static bool IsPresentationOnlyRenderer(Renderer renderer)
        {
            string lower = renderer.name.ToLowerInvariant();
            return lower.Contains("health") ||
                   lower.Contains("telegraph") ||
                   lower.Contains("target") ||
                   lower.Contains("outline") ||
                   lower.Contains("minimap") ||
                   lower.Contains("damage_number") ||
                   lower.Contains("guardian_spawn_vfx");
        }

        private void DisableGameplayAfterDeath(bool isPlayer)
        {
            MonoBehaviour[] behaviours =
                GetComponentsInChildren<MonoBehaviour>(true);

            for (int index = 0; index < behaviours.Length; index++)
            {
                MonoBehaviour behaviour = behaviours[index];

                if (behaviour == null ||
                    behaviour == this ||
                    behaviour is BDHealth ||
                    behaviour is BDEnemyDeathFeedback ||
                    behaviour is BDEnemyLootDropper ||
                    behaviour is BDDamageFlashFeedback)
                {
                    continue;
                }

                string typeName = behaviour.GetType().Name;
                bool gameplayOwner = isPlayer
                    ? typeName.Contains("Player") ||
                      typeName.Contains("Combat") ||
                      typeName.Contains("Melee") ||
                      typeName.Contains("Ranged") ||
                      typeName.Contains("Interaction")
                    : typeName.Contains("Enemy") ||
                      typeName.Contains("Tactical") ||
                      typeName.Contains("Attack") ||
                      typeName.Contains("Shooter") ||
                      typeName.Contains("Charger") ||
                      typeName.Contains("Patrol") ||
                      typeName.Contains("Jumper") ||
                      typeName.Contains("Trap");

                if (gameplayOwner)
                    behaviour.enabled = false;
            }

            CharacterController controller =
                GetComponent<CharacterController>();
            if (controller != null)
                controller.enabled = false;

            Collider[] colliders =
                GetComponentsInChildren<Collider>(true);
            for (int index = 0; index < colliders.Length; index++)
            {
                Collider collider = colliders[index];
                if (collider != null)
                    collider.enabled = false;
            }
        }

        private IEnumerator AnimateDeath(bool isPlayer)
        {
            float motionDuration = isPlayer
                ? Mathf.Max(0.45f, playerDeathDuration)
                : Mathf.Max(0.25f, enemyDeathDuration);

            float side = isPlayer
                ? 1f
                : (GetInstanceID() & 1) == 0 ? 1f : -1f;

            float fallDegrees = isPlayer
                ? Mathf.Max(25f, playerFallDegrees)
                : Mathf.Max(25f, enemyFallDegrees);

            float elapsed = 0f;
            while (elapsed < motionDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / motionDuration);
                float eased = t * t * (3f - 2f * t);

                for (int index = 0; index < visualPoses.Count; index++)
                {
                    VisualPose pose = visualPoses[index];
                    if (pose.visual == null)
                        continue;

                    Quaternion targetRotation =
                        pose.localRotation *
                        Quaternion.Euler(
                            isPlayer ? fallDegrees : fallDegrees * 0.72f,
                            0f,
                            isPlayer ? 14f : side * 22f
                        );

                    Vector3 targetPosition =
                        pose.localPosition +
                        Vector3.down *
                        (isPlayer
                            ? Mathf.Max(0.08f, playerDropDistance)
                            : Mathf.Max(0.08f, enemyDropDistance)) +
                        Vector3.forward * (isPlayer ? 0.16f : 0.08f);

                    Vector3 targetScale =
                        Vector3.Scale(
                            pose.localScale,
                            isPlayer
                                ? new Vector3(1.12f, 0.20f, 1.12f)
                                : new Vector3(1.08f, 0.34f, 1.08f)
                        );

                    pose.visual.localPosition = Vector3.Lerp(
                        pose.localPosition,
                        targetPosition,
                        eased
                    );
                    pose.visual.localRotation = Quaternion.Slerp(
                        pose.localRotation,
                        targetRotation,
                        eased
                    );
                    pose.visual.localScale = Vector3.Lerp(
                        pose.localScale,
                        targetScale,
                        eased
                    );
                }

                yield return null;
            }

            if (isPlayer)
            {
                float hold = Mathf.Max(0f, playerFinalPoseHold);
                while (hold > 0f)
                {
                    hold -= Time.unscaledDeltaTime;
                    yield return null;
                }
            }

            deathCompleted = true;
            routine = null;
        }

        private readonly struct VisualPose
        {
            public readonly Transform visual;
            public readonly Vector3 localPosition;
            public readonly Quaternion localRotation;
            public readonly Vector3 localScale;

            public VisualPose(
                Transform target,
                Vector3 position,
                Quaternion rotation,
                Vector3 scale)
            {
                visual = target;
                localPosition = position;
                localRotation = rotation;
                localScale = scale;
            }
        }
    }
}
