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

        // BD FORCED MOVEMENT COMPATIBILITY API V23R19I
        // Unified alias used by hook and generic knockback call sites.
        // It intentionally preserves the existing serialized projectile-knockback
        // field and the current Regular/MiniBoss/Boss rank semantics.
        public bool ReceivesForcedMovement =>
            ReceivesPlayerProjectileKnockback;

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

        // BD ELITE GUARDIAN IS NOT A SMALL REGULAR ENEMY V23R19G
        public void ConfigureEliteGuardian()
        {
            Configure(BDCombatantRank.MiniBoss, false);
        }

        public void ConfigureFinalBoss()
        {
            Configure(BDCombatantRank.Boss, false);
        }

        public static bool CanReceiveForcedMovement(BDHealth health)
        {
            if (health == null)
                return false;

            BDCombatantProfile profile = ResolveProfile(health);

            // Existing regular enemies without a profile preserve their current
            // forced-movement behavior. Explicit profiles remain authoritative.
            return profile == null || profile.ReceivesForcedMovement;
        }

        public static bool CanReceivePlayerProjectileKnockback(BDHealth health)
        {
            return CanReceiveForcedMovement(health);
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
