using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDHorseLocalThreatUtility
    {
        public static bool HasLivingThreatNear(
            Transform horse,
            Transform rider,
            float radius)
        {
            if (horse == null)
                return false;

            float safeRadius = Mathf.Max(1f, radius);
            float radiusSquared = safeRadius * safeRadius;
            BDMinimapRoom activeRoom = ResolveActiveRoom(horse, rider);

            BDHealth[] candidates = UnityEngine.Object.FindObjectsByType<BDHealth>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

            for (int index = 0; index < candidates.Length; index++)
            {
                BDHealth candidate = candidates[index];
                if (!IsLivingCombatant(candidate))
                    continue;

                if (activeRoom != null &&
                    !activeRoom.ContainsWorldPosition(candidate.transform.position, 0.20f))
                {
                    continue;
                }

                if (IsNear(candidate.transform.position, horse.position, radiusSquared))
                    return true;

                if (rider != null &&
                    IsNear(candidate.transform.position, rider.position, radiusSquared))
                {
                    return true;
                }
            }

            return false;
        }

        private static BDMinimapRoom ResolveActiveRoom(Transform horse, Transform rider)
        {
            BDMinimapRoom[] rooms = UnityEngine.Object.FindObjectsByType<BDMinimapRoom>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

            for (int index = 0; index < rooms.Length; index++)
            {
                BDMinimapRoom room = rooms[index];
                if (room == null)
                    continue;

                if (rider != null && room.ContainsWorldPosition(rider.position, 0.20f))
                    return room;

                if (room.ContainsWorldPosition(horse.position, 0.20f))
                    return room;
            }

            return null;
        }

        private static bool IsLivingCombatant(BDHealth candidate)
        {
            if (candidate == null || candidate.IsDead || !candidate.gameObject.activeInHierarchy)
                return false;

            if (candidate.GetComponentInParent<BDPlayerMarker>() != null)
                return false;

            if (candidate.GetComponentInParent<BDHorseHealth>() != null)
                return false;

            bool looksLikeCombatant =
                candidate.GetComponent<CharacterController>() != null ||
                candidate.GetComponent<BDBossHealthChannel>() != null;

            if (!looksLikeCombatant)
                return false;

            BDBossEncounterController bossEncounter =
                candidate.GetComponentInParent<BDBossEncounterController>();

            if (bossEncounter != null && !bossEncounter.IsCombatActive)
                return false;

            return true;
        }

        private static bool IsNear(Vector3 a, Vector3 b, float radiusSquared)
        {
            Vector3 delta = a - b;
            delta.y = 0f;
            return delta.sqrMagnitude <= radiusSquared;
        }
    }
}
