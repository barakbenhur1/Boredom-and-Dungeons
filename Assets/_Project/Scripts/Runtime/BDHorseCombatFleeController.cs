using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseController))]
    public sealed class BDHorseCombatFleeController : MonoBehaviour
    {
        [Header("Combat flee")]
        [SerializeField] private bool fleeWhenCombatIsActive = true;
        [SerializeField] private bool dismountRiderWhenCombatStarts = false;
        [SerializeField] private float checkInterval = 0.20f;
        [SerializeField] private float resendInterval = 1.25f;
        [SerializeField] private float safeSpotSwitchHysteresis = 2.0f;
        [SerializeField] private float localThreatRadius = 12.0f;

        private BDHorseController horse;
        private float nextCheckAt;
        private float nextSendAt;
        private Transform currentSafeSpot;
        private float suppressedUntil = -999f;

        private void Awake()
        {
            horse = GetComponent<BDHorseController>();
            ResetForCleanGameStart(2.50f);
        }

        public void ResetForCleanGameStart(
            float calmSeconds)
        {
            suppressedUntil =
                Mathf.Max(
                    suppressedUntil,
                    Time.unscaledTime +
                    Mathf.Max(0f, calmSeconds)
                );

            nextCheckAt = 0f;
            nextSendAt = 0f;
            currentSafeSpot = null;
        }

        private void Update()
        {
            if (!fleeWhenCombatIsActive || horse == null)
                return;

            if (Time.unscaledTime < suppressedUntil ||
                horse.IsStartupCalm)
            {
                return;
            }

            if (Time.time < nextCheckAt)
                return;

            nextCheckAt = Time.time + Mathf.Max(0.05f, checkInterval);

            if (!IsCombatActive())
            {
                currentSafeSpot = null;
                return;
            }

            Transform nearest = FindNearestSafeSpot();
            if (nearest != null && ShouldSwitchSafeSpot(nearest))
            {
                currentSafeSpot = nearest;
                horse.SetSafeSpot(nearest);
            }

            if (Time.time < nextSendAt)
                return;

            nextSendAt = Time.time + Mathf.Max(0.15f, resendInterval);

            if (horse.IsMounted)
            {
                if (dismountRiderWhenCombatStarts)
                    horse.ForceDismountForCombat();

                return;
            }

            horse.SendToSafeSpot();
        }

        private bool ShouldSwitchSafeSpot(Transform candidate)
        {
            if (candidate == null)
                return false;

            if (currentSafeSpot == null)
                return true;

            float currentDistance = HorizontalDistance(transform.position, currentSafeSpot.position);
            float candidateDistance = HorizontalDistance(transform.position, candidate.position);
            return candidateDistance + Mathf.Max(0f, safeSpotSwitchHysteresis) < currentDistance;
        }

        private Transform FindNearestSafeSpot()
        {
            BDHorseSafeSpot[] spots = FindObjectsByType<BDHorseSafeSpot>(FindObjectsSortMode.None);
            Transform best = null;
            float bestDistance = float.MaxValue;

            for (int i = 0; i < spots.Length; i++)
            {
                BDHorseSafeSpot spot = spots[i];
                if (spot == null)
                    continue;

                float distance = HorizontalDistance(transform.position, spot.transform.position);
                if (distance >= bestDistance)
                    continue;

                bestDistance = distance;
                best = spot.transform;
            }

            return best;
        }

        private static float HorizontalDistance(Vector3 a, Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }

        private bool IsCombatActive()
        {
            if (horse == null)
                return false;

            return BDHorseLocalThreatUtility.HasLivingThreatNear(
                horse.transform,
                horse.Rider,
                localThreatRadius
            );
        }
    }
}
