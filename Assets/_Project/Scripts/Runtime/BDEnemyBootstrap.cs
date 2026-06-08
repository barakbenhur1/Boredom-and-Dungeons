using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDEnemyBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            if (GetComponent<BDHealth>() == null)
                return;

            if (GetComponent<BDPlayerMarker>() != null)
                return;

            if (GetComponent<BDHorseHealth>() != null)
                return;

            if (GetComponent<CharacterController>() != null && GetComponent<BDKnockbackReceiver>() == null)
                gameObject.AddComponent<BDKnockbackReceiver>();

            if (GetComponent<CharacterController>() != null && GetComponent<BDEnemyCollisionDiscipline>() == null)
                gameObject.AddComponent<BDEnemyCollisionDiscipline>();

            if (GetComponent<BDEnemyGroundStick>() == null)
                gameObject.AddComponent<BDEnemyGroundStick>();

            if (GetComponent<CharacterController>() != null &&
                GetComponent<BDEnemyMotionStabilizer>() == null)
            {
                gameObject.AddComponent<BDEnemyMotionStabilizer>();
            }

            if (GetComponent<CharacterController>() != null &&
                GetComponent<BDEnemyHazardNavigation>() == null)
            {
                gameObject.AddComponent<BDEnemyHazardNavigation>();
            }

            if (GetComponent<CharacterController>() != null &&
                GetComponent<BDEnemyPlacementGuard>() == null)
            {
                gameObject.AddComponent<BDEnemyPlacementGuard>();
            }

            if (GetComponent<BDEnemyDeathFeedback>() == null)
                gameObject.AddComponent<BDEnemyDeathFeedback>();

            if (GetComponent<BDEnemyWorldHealthBar>() == null)
                gameObject.AddComponent<BDEnemyWorldHealthBar>();

            if (GetComponent<BDEnemyAttackTelegraph>() == null)
                gameObject.AddComponent<BDEnemyAttackTelegraph>();

            if (GetComponent<BDEnemyProximityTelegraph>() == null)
                gameObject.AddComponent<BDEnemyProximityTelegraph>();

            if (GetComponent<BDAuxiliaryEnemyRingTransparency>() == null)
            {
                gameObject.AddComponent<
                    BDAuxiliaryEnemyRingTransparency>();
            }

            if (GetComponent<BDHitStaggerReceiver>() == null)
                gameObject.AddComponent<BDHitStaggerReceiver>();

            if (GetComponent<BDEnemyHitFlashReceiver>() == null)
                gameObject.AddComponent<BDEnemyHitFlashReceiver>();
        }
    }
}
