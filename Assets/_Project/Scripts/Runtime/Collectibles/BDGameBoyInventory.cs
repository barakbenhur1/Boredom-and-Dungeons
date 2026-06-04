using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDGameBoyInventory : MonoBehaviour
    {
        [Header("Secret Collectibles")]
        [SerializeField] private bool hasGameBoy;
        [SerializeField] private int batteryCount;
        [SerializeField] private bool hasGameCartridge;

        public event Action<BDSecretCollectibleKind> SecretCollected;

        public bool HasGameBoy => hasGameBoy;
        public int BatteryCount => batteryCount;
        public bool HasGameCartridge => hasGameCartridge;

        public bool HasAnySecretCollectible => hasGameBoy || batteryCount > 0 || hasGameCartridge;

        public int SecretCollectibleTypeCount
        {
            get
            {
                int count = 0;

                if (hasGameBoy)
                    count++;

                if (batteryCount > 0)
                    count++;

                if (hasGameCartridge)
                    count++;

                return count;
            }
        }

        public bool HasRequiredCinematicItems(int requiredBatteries)
        {
            // Backward-compatible meaning from V108/V111:
            // Game Boy + enough batteries powers the device.
            // Cartridge is a separate optional secret that affects the final ending variant later.
            return HasPoweredGameBoy(requiredBatteries);
        }

        public bool HasEnoughBatteries(int requiredBatteries = 2)
        {
            return batteryCount >= Mathf.Max(0, requiredBatteries);
        }

        public bool HasPoweredGameBoy(int requiredBatteries = 2)
        {
            return hasGameBoy && HasEnoughBatteries(requiredBatteries);
        }

        public bool HasPlayableGameBoy(int requiredBatteries = 2)
        {
            return HasPoweredGameBoy(requiredBatteries) && hasGameCartridge;
        }

        public bool HasCollected(BDSecretCollectibleKind kind)
        {
            switch (kind)
            {
                case BDSecretCollectibleKind.GameBoy:
                    return hasGameBoy;

                case BDSecretCollectibleKind.Battery:
                    return batteryCount > 0;

                case BDSecretCollectibleKind.GameCartridge:
                    return hasGameCartridge;

                default:
                    return false;
            }
        }

        public void CollectGameBoy()
        {
            if (hasGameBoy)
                return;

            hasGameBoy = true;
            NotifySecretCollected(BDSecretCollectibleKind.GameBoy);
        }

        public void CollectBattery(int amount = 1)
        {
            int safeAmount = Mathf.Max(1, amount);
            int previousCount = batteryCount;

            batteryCount = Mathf.Max(0, batteryCount + safeAmount);

            if (batteryCount > previousCount)
                NotifySecretCollected(BDSecretCollectibleKind.Battery);
        }

        public void CollectGameCartridge()
        {
            if (hasGameCartridge)
                return;

            hasGameCartridge = true;
            NotifySecretCollected(BDSecretCollectibleKind.GameCartridge);
        }

        public void Collect(BDSecretCollectibleKind kind, int batteryAmount = 1)
        {
            switch (kind)
            {
                case BDSecretCollectibleKind.GameBoy:
                    CollectGameBoy();
                    break;

                case BDSecretCollectibleKind.Battery:
                    CollectBattery(batteryAmount);
                    break;

                case BDSecretCollectibleKind.GameCartridge:
                    CollectGameCartridge();
                    break;
            }
        }

        public bool ConsumeBatteries(int amount)
        {
            amount = Mathf.Max(0, amount);
            if (amount <= 0)
                return true;

            if (batteryCount < amount)
                return false;

            batteryCount -= amount;
            return true;
        }

        private void NotifySecretCollected(BDSecretCollectibleKind kind)
        {
            SecretCollected?.Invoke(kind);
        }

        public static BDGameBoyInventory FindOnPlayer()
        {
            Transform player = BDTargetFinder.FindPlayer();
            if (player == null)
                return null;

            BDGameBoyInventory inventory = player.GetComponent<BDGameBoyInventory>();
            if (inventory == null)
                inventory = player.gameObject.AddComponent<BDGameBoyInventory>();

            return inventory;
        }
    }
}
