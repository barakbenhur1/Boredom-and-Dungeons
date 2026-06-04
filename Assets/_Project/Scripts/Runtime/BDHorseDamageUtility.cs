using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDHorseDamageUtility
    {
        public static bool TryDamageHorseNear(Vector3 center, float radius, float damage, Transform sourceToIgnore = null)
        {
            Collider[] hits = Physics.OverlapSphere(center, radius, ~0, QueryTriggerInteraction.Ignore);
            bool damaged = false;

            foreach (Collider hit in hits)
            {
                if (hit == null)
                    continue;

                if (sourceToIgnore != null && (hit.transform == sourceToIgnore || hit.transform.IsChildOf(sourceToIgnore)))
                    continue;

                BDHorseHealth horse = hit.GetComponentInParent<BDHorseHealth>();
                if (horse == null || horse.IsFainted)
                    continue;

                horse.ApplyDamage(damage);
                damaged = true;
                break;
            }

            return damaged;
        }
    }
}
