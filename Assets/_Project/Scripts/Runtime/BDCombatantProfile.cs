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

        // BD COMBATANT PROFILE POLICY:
        // Final bosses are always immune. Mini-bosses remain explicit:
        // Quad Gunners use true; large mini-bosses use false.
        public bool ReceivesPlayerProjectileKnockback =>
            rank != BDCombatantRank.Boss &&
            receivesPlayerProjectileKnockback;

        public void Configure(
            BDCombatantRank newRank,
            bool receivesKnockback)
        {
            rank = newRank;
            receivesPlayerProjectileKnockback =
                newRank != BDCombatantRank.Boss &&
                receivesKnockback;
        }

        public void ConfigureRegularEnemy()
        {
            Configure(BDCombatantRank.Regular, true);
        }

        public void ConfigureSmallMiniBoss()
        {
            Configure(BDCombatantRank.MiniBoss, true);
        }

        public void ConfigureLargeMiniBoss()
        {
            Configure(BDCombatantRank.MiniBoss, false);
        }

        public void ConfigureFinalBoss()
        {
            Configure(BDCombatantRank.Boss, false);
        }

        public static bool CanReceivePlayerProjectileKnockback(BDHealth health)
        {
            if (health == null)
                return false;

            BDCombatantProfile profile = ResolveProfile(health);

            // Existing regular enemies without a profile keep receiving knockback.
            if (profile == null)
                return true;

            return profile.ReceivesPlayerProjectileKnockback;
        }

        public static BDCombatantRank ResolveRank(BDHealth health)
        {
            if (health == null)
                return BDCombatantRank.Regular;

            BDCombatantProfile profile = ResolveProfile(health);
            return profile != null
                ? profile.rank
                : BDCombatantRank.Regular;
        }

        public static BDCombatantProfile ResolveProfile(BDHealth health)
        {
            if (health == null)
                return null;

            BDCombatantProfile profile =
                health.GetComponent<BDCombatantProfile>();

            if (profile == null)
                profile = health.GetComponentInParent<BDCombatantProfile>();

            return profile;
        }

        private void OnValidate()
        {
            if (rank == BDCombatantRank.Boss)
                receivesPlayerProjectileKnockback = false;
        }
    }
}
