using UnityEngine;

namespace BoredomAndDungeons
{
    public readonly struct BDGameEndingState
    {
        public readonly bool HasGameBoy;
        public readonly int BatteryCount;
        public readonly bool HasEnoughBatteries;
        public readonly bool HasGameCartridge;
        public readonly BDGameEndingVariant Variant;

        public BDGameEndingState(
            bool hasGameBoy,
            int batteryCount,
            bool hasEnoughBatteries,
            bool hasGameCartridge,
            BDGameEndingVariant variant)
        {
            HasGameBoy = hasGameBoy;
            BatteryCount = Mathf.Max(0, batteryCount);
            HasEnoughBatteries = hasEnoughBatteries;
            HasGameCartridge = hasGameCartridge;
            Variant = variant;
        }

        public bool IsPowered => HasGameBoy && HasEnoughBatteries;
        public bool IsFullyPlayable => IsPowered && HasGameCartridge;

        public override string ToString()
        {
            return $"EndingState(variant={Variant}, gameBoy={HasGameBoy}, batteries={BatteryCount}, enoughBatteries={HasEnoughBatteries}, cartridge={HasGameCartridge})";
        }
    }
}
