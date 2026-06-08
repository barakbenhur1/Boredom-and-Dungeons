using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-40)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    public sealed class BDEnemyHazardNavigation : MonoBehaviour
    {
        // BD ENEMY HAZARD INTENT/FORCED ENTRY OWNER V23R17
        [SerializeField] private float hazardClearance = 0.18f;
        [SerializeField] private float forcedEntryGraceSeconds = 0.95f;
        [SerializeField] private float smallEnemyMaximumWidth = 2.65f;
        [SerializeField] private float smallEnemyMaximumHeight = 3.65f;

        private CharacterController controller;
        private BDHealth health;
        private BDKnockbackReceiver knockback;
        private float forcedEntryUntil = -999f;

        public bool AllowsForcedHazardEntry =>
            Time.time <= forcedEntryUntil ||
            (knockback != null && knockback.IsBeingKnocked);

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            health = GetComponent<BDHealth>();
            knockback = GetComponent<BDKnockbackReceiver>();
        }

        public void NotifyForcedHazardEntry(float duration = -1f)
        {
            float safeDuration = duration > 0f
                ? duration
                : forcedEntryGraceSeconds;
            forcedEntryUntil = Mathf.Max(
                forcedEntryUntil,
                Time.time + Mathf.Max(0.10f, safeDuration)
            );
        }

        public Vector3 FilterBrainMotion(Vector3 requestedMotion)
        {
            if (controller == null ||
                health == null ||
                health.IsDead ||
                AllowsForcedHazardEntry)
            {
                return requestedMotion;
            }

            return BDHazardVolume.FilterEnemyMotion(
                controller,
                requestedMotion,
                Mathf.Max(0.05f, hazardClearance)
            );
        }

        public static Vector3 FilterBrainMotion(
            MonoBehaviour owner,
            Vector3 requestedMotion)
        {
            if (owner == null)
                return requestedMotion;

            BDEnemyHazardNavigation navigation =
                owner.GetComponent<BDEnemyHazardNavigation>();

            if (navigation == null &&
                Application.isPlaying &&
                owner.GetComponent<CharacterController>() != null &&
                owner.GetComponent<BDHealth>() != null)
            {
                navigation =
                    owner.gameObject.AddComponent<BDEnemyHazardNavigation>();
            }

            return navigation != null
                ? navigation.FilterBrainMotion(requestedMotion)
                : requestedMotion;
        }

        public static bool IsSmallRegularEnemy(BDHealth candidate)
        {
            if (candidate == null || candidate.IsDead)
                return false;

            if (candidate.GetComponent<BDPlayerMarker>() != null ||
                candidate.GetComponent<BDHorseHealth>() != null)
            {
                return false;
            }

            if (BDCombatantProfile.ResolveRank(candidate) !=
                BDCombatantRank.Regular)
            {
                return false;
            }

            CharacterController body =
                candidate.GetComponent<CharacterController>();
            if (body == null)
                body = candidate.GetComponentInParent<CharacterController>();

            if (body == null)
                return true;

            Bounds bounds = body.bounds;
            float width = Mathf.Max(bounds.size.x, bounds.size.z);
            float height = bounds.size.y;

            BDEnemyHazardNavigation navigation =
                candidate.GetComponent<BDEnemyHazardNavigation>();

            float maxWidth = navigation != null
                ? navigation.smallEnemyMaximumWidth
                : 2.65f;
            float maxHeight = navigation != null
                ? navigation.smallEnemyMaximumHeight
                : 3.65f;

            return width <= maxWidth && height <= maxHeight;
        }
    }
}
