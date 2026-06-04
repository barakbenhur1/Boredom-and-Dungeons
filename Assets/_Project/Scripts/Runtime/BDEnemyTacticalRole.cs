using UnityEngine;

namespace BoredomAndDungeons
{
    public enum BDEnemyTacticalRoleType
    {
        Pressure,
        ExitBlocker,
        RangedControl,
        AreaControl,
        BurstPunish,
        AmbushPunish
    }

    [DisallowMultipleComponent]
    public sealed class BDEnemyTacticalRole : MonoBehaviour
    {
        [SerializeField] private BDEnemyTacticalRoleType role = BDEnemyTacticalRoleType.Pressure;

        public BDEnemyTacticalRoleType Role => role;

        public void SetRole(BDEnemyTacticalRoleType newRole)
        {
            role = newRole;
        }
    }
}
