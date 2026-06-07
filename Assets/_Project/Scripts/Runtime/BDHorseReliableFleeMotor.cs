using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseController))]
    [RequireComponent(typeof(BDHorseHealth))]
    public sealed class BDHorseReliableFleeMotor : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float nearbyEnemyDangerRadius = 13f;
        [SerializeField] private float checkInterval = 0.15f;
        [SerializeField] private float resendInterval = 0.65f;

        [Header("Safe spot selection")]
        [SerializeField] private float minimumEnemyDistanceAtSafeSpot = 5.5f;
        [SerializeField] private float reachedSafeDistance = 0.85f;

        private BDHorseController horseController;
        private BDHorseHealth horseHealth;
        private Transform assignedSafeSpot;
        private float nextCheckAt;
        private float nextResendAt;
        private float suppressedUntil = -999f;

        private void Awake()
        {
            horseController = GetComponent<BDHorseController>();
            horseHealth = GetComponent<BDHorseHealth>();
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

            assignedSafeSpot = null;
            nextCheckAt = 0f;
            nextResendAt = 0f;
        }

        private void Update()
        {
            if (horseController == null || horseHealth == null || horseHealth.IsFainted)
                return;

            if (Time.unscaledTime < suppressedUntil ||
                horseController.IsStartupCalm)
            {
                return;
            }

            if (Time.time < nextCheckAt)
                return;

            nextCheckAt = Time.time + Mathf.Max(0.05f, checkInterval);

            if (!IsDangerActive())
                return;

            Transform bestSafeSpot = FindBestSafeSpot();
            if (bestSafeSpot != null && bestSafeSpot != assignedSafeSpot)
            {
                assignedSafeSpot = bestSafeSpot;
                horseController.SetSafeSpot(assignedSafeSpot);
            }

            if (horseController.IsMounted)
                return;

            if (assignedSafeSpot == null)
                return;

            float distance = HorizontalDistance(transform.position, assignedSafeSpot.position);
            if (distance <= reachedSafeDistance)
                return;

            if (Time.time < nextResendAt)
                return;

            nextResendAt = Time.time + Mathf.Max(0.15f, resendInterval);
            horseController.SendToSafeSpot();
        }

        private bool IsDangerActive()
        {
            if (horseController == null)
                return false;

            return BDHorseLocalThreatUtility.HasLivingThreatNear(
                horseController.transform,
                horseController.Rider,
                nearbyEnemyDangerRadius
            );
        }

        private Transform FindBestSafeSpot()
        {
            BDHorseSafeSpot[] safeSpots = FindObjectsByType<BDHorseSafeSpot>(FindObjectsSortMode.None);
            if (safeSpots == null || safeSpots.Length == 0)
                return null;

            BDHealth[] healthComponents = FindObjectsByType<BDHealth>(FindObjectsSortMode.None);
            Transform bestSafe = null;
            float bestScore = float.MaxValue;

            for (int i = 0; i < safeSpots.Length; i++)
            {
                BDHorseSafeSpot safeSpot = safeSpots[i];
                if (safeSpot == null)
                    continue;

                Vector3 safePosition = safeSpot.transform.position;
                float horseDistance = HorizontalDistance(transform.position, safePosition);
                float nearestEnemyDistance = FindNearestEnemyDistance(safePosition, healthComponents);

                float unsafePenalty = nearestEnemyDistance < minimumEnemyDistanceAtSafeSpot
                    ? (minimumEnemyDistanceAtSafeSpot - nearestEnemyDistance) * 100f
                    : 0f;

                float score = horseDistance + unsafePenalty;
                if (score >= bestScore)
                    continue;

                bestScore = score;
                bestSafe = safeSpot.transform;
            }

            return bestSafe;
        }

        private static float FindNearestEnemyDistance(Vector3 position, BDHealth[] healthComponents)
        {
            float nearest = float.MaxValue;

            for (int i = 0; i < healthComponents.Length; i++)
            {
                BDHealth candidate = healthComponents[i];
                if (!IsLivingEnemy(candidate))
                    continue;

                float distance = HorizontalDistance(position, candidate.transform.position);
                if (distance < nearest)
                    nearest = distance;
            }

            return nearest;
        }

        private static bool IsLivingEnemy(BDHealth candidate)
        {
            if (candidate == null || candidate.IsDead)
                return false;

            if (candidate.GetComponent<BDPlayerMarker>() != null)
                return false;

            if (candidate.GetComponent<BDHorseHealth>() != null)
                return false;

            return true;
        }

        private static float HorizontalDistance(Vector3 a, Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }
    }
}
