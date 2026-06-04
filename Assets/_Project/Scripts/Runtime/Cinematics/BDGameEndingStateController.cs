using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDGameEndingStateController : MonoBehaviour
    {
        [Header("Ending Requirements")]
        [SerializeField] private int requiredBatteries = 2;
        [SerializeField] private bool logDecision;

        public int RequiredBatteries => Mathf.Max(0, requiredBatteries);

        public BDGameEndingState Evaluate(BDGameBoyInventory inventory)
        {
            BDGameEndingState state = EvaluateInventory(inventory, RequiredBatteries);

            if (logDecision)
                Debug.Log($"B&D ending decision: {state}");

            return state;
        }

        public BDGameEndingState EvaluatePlayer()
        {
            return Evaluate(BDGameBoyInventory.FindOnPlayer());
        }

        public static BDGameEndingState EvaluateInventory(BDGameBoyInventory inventory, int requiredBatteries = 2)
        {
            requiredBatteries = Mathf.Max(0, requiredBatteries);

            if (inventory == null)
            {
                return new BDGameEndingState(
                    hasGameBoy: false,
                    batteryCount: 0,
                    hasEnoughBatteries: false,
                    hasGameCartridge: false,
                    variant: BDGameEndingVariant.NoGameBoy
                );
            }

            bool hasGameBoy = inventory.HasGameBoy;
            int batteryCount = inventory.BatteryCount;
            bool hasEnoughBatteries = inventory.HasEnoughBatteries(requiredBatteries);
            bool hasGameCartridge = inventory.HasGameCartridge;

            BDGameEndingVariant variant = ResolveVariant(
                hasGameBoy,
                hasEnoughBatteries,
                hasGameCartridge
            );

            return new BDGameEndingState(
                hasGameBoy,
                batteryCount,
                hasEnoughBatteries,
                hasGameCartridge,
                variant
            );
        }

        public static BDGameEndingVariant ResolveVariant(
            bool hasGameBoy,
            bool hasEnoughBatteries,
            bool hasGameCartridge)
        {
            if (!hasGameBoy)
                return BDGameEndingVariant.NoGameBoy;

            if (!hasEnoughBatteries)
                return BDGameEndingVariant.GameBoyNoBatteries;

            if (!hasGameCartridge)
                return BDGameEndingVariant.PoweredNoCartridge;

            return BDGameEndingVariant.PoweredWithCartridge;
        }

        public static string GetDebugName(BDGameEndingVariant variant)
        {
            switch (variant)
            {
                case BDGameEndingVariant.NoGameBoy:
                    return "No Game Boy";

                case BDGameEndingVariant.GameBoyNoBatteries:
                    return "Game Boy, not enough batteries";

                case BDGameEndingVariant.PoweredNoCartridge:
                    return "Powered Game Boy, no cartridge";

                case BDGameEndingVariant.PoweredWithCartridge:
                    return "Powered Game Boy with cartridge";

                default:
                    return "Unknown ending";
            }
        }
    }
}
