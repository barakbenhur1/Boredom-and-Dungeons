#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools
{
    public static class BDSecretCollectibleAdvertisingValidator
    {
        private static readonly string[] ForbiddenPhrases =
        {
            "collect the game boy",
            "find the game boy",
            "collect game boy",
            "find game boy",
            "collect batteries",
            "find batteries",
            "collect battery",
            "find battery",
            "collect the cartridge",
            "find the cartridge",
            "collect cartridge",
            "find cartridge",
            "missing game boy",
            "missing battery",
            "missing batteries",
            "missing cartridge",
            "0/4",
            "1/4",
            "2/4",
            "3/4",
            "4/4",
            "objective: game boy",
            "objective: battery",
            "objective: cartridge",
            "secret checklist",
            "collectible checklist"
        };

        private static readonly string[] AllowedPathFragments =
        {
            "/Design/",
            "\\Design\\",
            "/README",
            "\\README",
            "package_manifest.json",
            "BDSecretCollectibleAdvertisingValidator.cs"
        };

        [MenuItem("Boredom & Dungeons/Validation/Check Secret Collectible Advertising")]
        public static void CheckSecretCollectibleAdvertising()
        {
            List<string> offenders = ScanProjectFiles();

            if (offenders.Count == 0)
            {
                Debug.Log("B&D validation passed: no visible secret collectible advertising phrases found in Runtime/Editor gameplay scripts.");
                return;
            }

            Debug.LogError("B&D validation failed: possible secret collectible advertising found:\n" + string.Join("\n", offenders));
        }

        public static List<string> ScanProjectFiles()
        {
            List<string> offenders = new List<string>();
            string root = "Assets/_Project";

            if (!Directory.Exists(root))
                return offenders;

            foreach (string path in Directory.GetFiles(root, "*.*", SearchOption.AllDirectories))
            {
                string normalized = path.Replace('\\', '/');

                if (!ShouldScanPath(normalized))
                    continue;

                string extension = Path.GetExtension(path).ToLowerInvariant();
                if (extension != ".cs" && extension != ".prefab" && extension != ".unity" && extension != ".asset")
                    continue;

                string text = File.ReadAllText(path);
                string lowered = text.ToLowerInvariant();

                for (int i = 0; i < ForbiddenPhrases.Length; i++)
                {
                    string phrase = ForbiddenPhrases[i];
                    if (lowered.Contains(phrase))
                        offenders.Add($"{path} :: {phrase}");
                }
            }

            return offenders;
        }

        private static bool ShouldScanPath(string normalizedPath)
        {
            for (int i = 0; i < AllowedPathFragments.Length; i++)
            {
                if (normalizedPath.Contains(AllowedPathFragments[i]))
                    return false;
            }

            return normalizedPath.Contains("/Scripts/Runtime/") || normalizedPath.Contains("/Scripts/Editor/");
        }
    }
}
#endif
