#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace BoredomAndDungeons.EditorTools.Branding
{
    [InitializeOnLoad]
    internal static class BDApplicationIconInstaller
    {
        private const string IconPath =
            "Assets/_Project/Art/Branding/BoredomAndDungeons_AppIcon.png";
        private const string SessionKey =
            "BoredomAndDungeons.ApplicationIconInstaller.V101117";

        static BDApplicationIconInstaller()
        {
            EditorApplication.delayCall -= ApplyAutomaticallyOncePerSession;
            EditorApplication.delayCall += ApplyAutomaticallyOncePerSession;
        }

        [MenuItem("Boredom And Dungeons/Branding/Apply Application Icon")]
        private static void ApplyFromMenu()
        {
            ApplyIcon(logSuccess: true);
        }

        private static void ApplyAutomaticallyOncePerSession()
        {
            if (SessionState.GetBool(SessionKey, false))
                return;

            SessionState.SetBool(SessionKey, true);
            ApplyIcon(logSuccess: false);
        }

        private static void ApplyIcon(bool logSuccess)
        {
            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
            if (icon == null)
            {
                Debug.LogError("B&D APP ICON: missing texture at " + IconPath);
                return;
            }

            bool assigned = false;
            assigned |= ApplyToTarget(NamedBuildTarget.Standalone, icon);
            assigned |= ApplyToTarget(NamedBuildTarget.Android, icon);
            assigned |= ApplyToTarget(NamedBuildTarget.iOS, icon);

            if (!assigned)
            {
                Debug.LogWarning(
                    "B&D APP ICON: no supported application-icon slots were " +
                    "available in this Editor installation."
                );
                return;
            }

            AssetDatabase.SaveAssets();
            if (logSuccess)
            {
                Debug.Log(
                    "B&D APP ICON: applied the attached artwork to all " +
                    "supported application-icon slots."
                );
            }
        }

        private static bool ApplyToTarget(
            NamedBuildTarget target,
            Texture2D icon)
        {
            int[] sizes = PlayerSettings.GetIconSizes(
                target,
                IconKind.Application
            );
            if (sizes == null || sizes.Length == 0)
                return false;

            Texture2D[] icons = new Texture2D[sizes.Length];
            for (int index = 0; index < icons.Length; index++)
                icons[index] = icon;

            PlayerSettings.SetIcons(
                target,
                icons,
                IconKind.Application
            );
            return true;
        }
    }
}
#endif
