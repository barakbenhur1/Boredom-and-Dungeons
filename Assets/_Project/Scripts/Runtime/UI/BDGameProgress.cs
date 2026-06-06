using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDGameProgress
    {
        private const string MotherDefeatedKey =
            "BD.Progress.MotherDefeated";

        public static bool MotherDefeated
        {
            get
            {
                return PlayerPrefs.GetInt(
                    MotherDefeatedKey,
                    0
                ) != 0;
            }
        }

        public static void MarkMotherDefeated()
        {
            if (MotherDefeated)
                return;

            PlayerPrefs.SetInt(
                MotherDefeatedKey,
                1
            );

            PlayerPrefs.Save();
        }
    }
}
