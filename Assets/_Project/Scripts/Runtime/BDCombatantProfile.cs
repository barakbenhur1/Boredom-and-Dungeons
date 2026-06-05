using UnityEngine;

namespace BoredomAndDungeons
{
    public enum BDCombatantRank
    {
        Regular = 0,
        MiniBoss = 1,
        Boss = 2
    }

    [DisallowMultipleComponent]
    public sealed class BDCombatantProfile : MonoBehaviour
    {
        [SerializeField] private BDCombatantRank rank = BDCombatantRank.Regular;
        [SerializeField] private bool receivesPlayerProjectileKnockback = true;

        public BDCombatantRank Rank => rank;
        public bool ReceivesPlayerProjectileKnockback => receivesPlayerProjectileKnockback;

        public void Configure(BDCombatantRank newRank, bool receivesKnockback)
        {
            rank = newRank;
            receivesPlayerProjectileKnockback = receivesKnockback;
        }

        public static bool CanReceivePlayerProjectileKnockback(BDHealth health)
        {
            if (health == null)
                return false;

            BDCombatantProfile profile = health.GetComponent<BDCombatantProfile>();
            if (profile == null)
                return true;

            return profile.receivesPlayerProjectileKnockback;
        }

        public static BDCombatantRank ResolveRank(BDHealth health)
        {
            if (health == null)
                return BDCombatantRank.Regular;

            BDCombatantProfile profile = health.GetComponent<BDCombatantProfile>();
            return profile != null ? profile.rank : BDCombatantRank.Regular;
        }
    }
}
