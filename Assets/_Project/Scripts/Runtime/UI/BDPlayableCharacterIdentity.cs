using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    public enum BDPlayableCharacterKind
    {
        Boy = 0,
        Girl = 1
    }

    [DisallowMultipleComponent]
    public sealed class BDPlayableCharacterIdentity : MonoBehaviour
    {
        private const string PreferredCharacterKey =
            "BD.Player.Character";

        [SerializeField]
        private BDPlayableCharacterKind character =
            BDPlayableCharacterKind.Boy;

        private static BDPlayableCharacterKind activeCharacter =
            LoadPreferredCharacter();

        public static event Action<BDPlayableCharacterKind>
            ActiveCharacterChanged;

        public BDPlayableCharacterKind Character => character;

        public static BDPlayableCharacterKind ActiveCharacter =>
            activeCharacter;

        private void OnEnable()
        {
            if (GetComponentInParent<BDPlayerMarker>() == null)
                return;

            Publish(character, persist: true);
        }

        public void SetCharacter(BDPlayableCharacterKind value)
        {
            character = value;

            if (isActiveAndEnabled &&
                GetComponentInParent<BDPlayerMarker>() != null)
            {
                Publish(value, persist: true);
            }
        }

        public static void SetPreferredCharacter(
            BDPlayableCharacterKind value)
        {
            Publish(value, persist: true);
        }

        public static void RefreshFromScene()
        {
            BDPlayableCharacterKind previous = activeCharacter;
            BDPlayableCharacterKind resolved = ResolveCurrent();

            if (resolved != previous)
                ActiveCharacterChanged?.Invoke(resolved);
        }

        public static BDPlayableCharacterKind ResolveCurrent()
        {
            BDPlayerMarker[] markers =
                UnityEngine.Object.FindObjectsByType<BDPlayerMarker>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            BDPlayableCharacterKind resolved;
            if (TryResolveFromMarkers(
                    markers,
                    activeOnly: true,
                    out resolved) ||
                TryResolveFromMarkers(
                    markers,
                    activeOnly: false,
                    out resolved))
            {
                activeCharacter = resolved;
                return activeCharacter;
            }

            activeCharacter = LoadPreferredCharacter();
            return activeCharacter;
        }


        private static bool TryResolveFromMarkers(
            BDPlayerMarker[] markers,
            bool activeOnly,
            out BDPlayableCharacterKind value)
        {
            value = BDPlayableCharacterKind.Boy;

            if (markers == null)
                return false;

            for (int index = 0; index < markers.Length; index++)
            {
                BDPlayerMarker marker = markers[index];
                if (marker == null)
                    continue;

                bool active = marker.gameObject.activeInHierarchy;
                if (activeOnly != active)
                    continue;

                BDPlayableCharacterIdentity identity =
                    marker.GetComponentInParent<
                        BDPlayableCharacterIdentity>(
                            includeInactive: true
                        );

                if (identity != null)
                {
                    value = identity.character;
                    return true;
                }

                if (TryInferFromHierarchy(marker.transform, out value))
                    return true;
            }

            return false;
        }

        private static bool TryInferFromHierarchy(
            Transform root,
            out BDPlayableCharacterKind value)
        {
            value = BDPlayableCharacterKind.Boy;

            if (root == null)
                return false;

            Transform current = root;
            while (current != null)
            {
                string lower =
                    current.gameObject.name.ToLowerInvariant();

                if (lower.Contains("girl") ||
                    lower.Contains("daughter") ||
                    lower.Contains("female"))
                {
                    value = BDPlayableCharacterKind.Girl;
                    return true;
                }

                if (lower.Contains("boy") ||
                    lower.Contains("son") ||
                    lower.Contains("male"))
                {
                    value = BDPlayableCharacterKind.Boy;
                    return true;
                }

                current = current.parent;
            }

            return false;
        }

        private static void Publish(
            BDPlayableCharacterKind value,
            bool persist)
        {
            bool changed = activeCharacter != value;
            activeCharacter = value;

            if (persist)
            {
                PlayerPrefs.SetInt(
                    PreferredCharacterKey,
                    (int)value
                );
                PlayerPrefs.Save();
            }

            if (changed)
                ActiveCharacterChanged?.Invoke(value);
        }

        private static BDPlayableCharacterKind
            LoadPreferredCharacter()
        {
            int stored = PlayerPrefs.GetInt(
                PreferredCharacterKey,
                (int)BDPlayableCharacterKind.Boy
            );

            return stored ==
                (int)BDPlayableCharacterKind.Girl
                    ? BDPlayableCharacterKind.Girl
                    : BDPlayableCharacterKind.Boy;
        }
    }
}
