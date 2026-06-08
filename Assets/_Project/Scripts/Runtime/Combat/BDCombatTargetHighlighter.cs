using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(300)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDPlayerCombat))]
    public sealed class BDCombatTargetHighlighter : MonoBehaviour
    {
        // BD CONSTANT-SIZE SILHOUETTE TARGET OUTLINE V23R13
        [Header("Target Resolution")]
        [SerializeField] private float resolveInterval = 0.025f;
        [SerializeField] private LayerMask targetingMask = ~0;

        [Header("Outline")]
        [SerializeField] private Color outlineColor =
            new Color(1f, 0.08f, 0.055f, 0.90f);
        [SerializeField, Range(1f, 8f)]
        private float outlineWidthPixels = 2.4f;

        private static readonly RaycastHit[] HitBuffer =
            new RaycastHit[64];

        private BDPlayerCombat combat;
        private BDHealth target;
        private BDTargetOutlineVisual targetOutline;
        private float nextResolveAt;
        private string targetingMode = "none";

        public BDHealth CurrentTarget => target;
        public string TargetingMode => targetingMode;

        private void Awake()
        {
            combat = GetComponent<BDPlayerCombat>();
        }

        private void OnDisable()
        {
            SetTarget(null, "none");
        }

        private void OnDestroy()
        {
            SetTarget(null, "none");
        }

        private void LateUpdate()
        {
            if (!BDGameplayUiVisibility.IsTargetingVisible ||
                BDMountedRunIntro.IsGameplayInputLocked ||
                BDNewRunFeedbackReset.IsCombatInputSuppressed)
            {
                SetTarget(null, "none");
                return;
            }

            if (Time.unscaledTime < nextResolveAt)
                return;

            nextResolveAt =
                Time.unscaledTime +
                Mathf.Max(0.01f, resolveInterval);

            ResolveTarget();
        }

        private void ResolveTarget()
        {
            if (combat == null)
                combat = GetComponent<BDPlayerCombat>();

            if (combat == null ||
                !combat.TryResolveTargetHighlightEnvelope(
                    out float range,
                    out float radius,
                    out string mode))
            {
                SetTarget(null, "none");
                return;
            }

            Vector3 direction = combat.TargetHighlightAimDirection;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
            {
                SetTarget(null, "none");
                return;
            }

            direction.Normalize();
            Vector3 origin = combat.TargetHighlightOrigin;

            int hitCount = Physics.SphereCastNonAlloc(
                origin,
                Mathf.Max(0.02f, radius),
                direction,
                HitBuffer,
                Mathf.Max(0.5f, range),
                targetingMask,
                QueryTriggerInteraction.Ignore
            );

            float nearestBlocker = float.PositiveInfinity;
            float nearestTarget = float.PositiveInfinity;
            BDHealth resolved = null;

            for (int index = 0; index < hitCount; index++)
            {
                RaycastHit hit = HitBuffer[index];
                HitBuffer[index] = default;

                Collider collider = hit.collider;
                if (collider == null)
                    continue;

                Transform hitTransform = collider.transform;

                if (hitTransform == transform ||
                    hitTransform.IsChildOf(transform) ||
                    transform.IsChildOf(hitTransform))
                {
                    continue;
                }

                if (collider.GetComponentInParent<BDPlayerMarker>() != null ||
                    collider.GetComponentInParent<BDHorseHealth>() != null)
                {
                    continue;
                }

                BDHealth health =
                    collider.GetComponentInParent<BDHealth>();

                if (IsValidEnemy(health))
                {
                    if (hit.distance < nearestTarget)
                    {
                        nearestTarget = hit.distance;
                        resolved = health;
                    }
                    continue;
                }

                if (IsTargetingBlocker(collider, hit.normal) &&
                    hit.distance < nearestBlocker)
                {
                    nearestBlocker = hit.distance;
                }
            }

            if (resolved == null ||
                nearestTarget > nearestBlocker + 0.01f)
            {
                SetTarget(null, "none");
                return;
            }

            SetTarget(resolved, mode);
        }

        private void SetTarget(BDHealth nextTarget, string mode)
        {
            if (target == nextTarget)
            {
                targetingMode = nextTarget != null ? mode : "none";
                if (targetOutline != null)
                {
                    targetOutline.SetHighlighted(
                        true,
                        outlineColor,
                        outlineWidthPixels
                    );
                }
                return;
            }

            if (targetOutline != null)
                targetOutline.SetHighlighted(false, outlineColor, outlineWidthPixels);

            target = nextTarget;
            targetOutline = null;
            targetingMode = target != null ? mode : "none";

            if (target == null)
                return;

            targetOutline = target.GetComponent<BDTargetOutlineVisual>();
            if (targetOutline == null)
                targetOutline = target.gameObject.AddComponent<BDTargetOutlineVisual>();

            targetOutline.SetHighlighted(
                true,
                outlineColor,
                outlineWidthPixels
            );
        }

        private static bool IsValidEnemy(BDHealth health)
        {
            if (health == null || health.IsDead)
                return false;
            if (health.GetComponent<BDPlayerMarker>() != null)
                return false;
            if (health.GetComponent<BDHorseHealth>() != null)
                return false;
            return true;
        }

        private static bool IsTargetingBlocker(
            Collider collider,
            Vector3 hitNormal)
        {
            if (collider == null || collider.isTrigger)
                return false;

            if (collider.GetComponentInParent<BDWallSurfaceProfile>() != null)
                return true;

            string lower = collider.name.ToLowerInvariant();

            if (lower.Contains("floor") ||
                lower.Contains("ground") ||
                lower.Contains("terrain"))
            {
                return false;
            }

            if (lower.Contains("wall") ||
                lower.Contains("boundary") ||
                lower.Contains("cliff") ||
                lower.Contains("rock") ||
                lower.Contains("column") ||
                lower.Contains("pillar") ||
                lower.Contains("obstacle"))
            {
                return true;
            }

            return hitNormal.y < 0.45f;
        }
    }

    public static class BDCombatTargetHighlighterInstaller
    {
        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallAfterSceneLoad()
        {
            Install();
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private static void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            Install();
        }

        private static void Install()
        {
            BDPlayerCombat[] players =
                Object.FindObjectsByType<BDPlayerCombat>(
                    FindObjectsSortMode.None
                );

            for (int index = 0; index < players.Length; index++)
            {
                BDPlayerCombat player = players[index];
                if (player != null &&
                    player.GetComponent<BDCombatTargetHighlighter>() == null)
                {
                    player.gameObject.AddComponent<BDCombatTargetHighlighter>();
                }
            }
        }
    }
}
