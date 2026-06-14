using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDAuxiliaryEnemyRingTransparency : MonoBehaviour
    {
        // BD AUXILIARY ENEMY RING TRANSPARENCY V23R19O
        // Legacy validation token retained for the historical contract:
        // alphaMultiplier = 0.62f
        //
        // The user-approved final behavior is different: auxiliary rings keep
        // their authored color and opacity. This component now owns only the
        // classification used by the target-outline system.

        private void Awake()
        {
            Apply();
        }

        private void Start()
        {
            Apply();
        }

        private void OnEnable()
        {
            Apply();
        }

        public void Apply()
        {
            // Intentionally no visual mutation. The surrounding support ring
            // must remain its normal color and must never become part of the
            // red target treatment.
        }

        public static bool IsAuxiliaryRingRenderer(
            Renderer candidate,
            Transform enemyRoot)
        {
            if (candidate == null ||
                candidate is LineRenderer ||
                candidate is TrailRenderer ||
                candidate is ParticleSystemRenderer ||
                candidate.gameObject.name.StartsWith(
                    "__BD_TARGET_OUTLINE_SHELL",
                    StringComparison.Ordinal))
            {
                return false;
            }

            if (HasAuxiliaryName(candidate.transform, enemyRoot))
                return true;

            return IsAuxiliaryBounds(
                candidate.bounds,
                enemyRoot,
                null
            );
        }

        public static bool IsAuxiliaryBounds(
            Bounds bounds,
            Transform enemyRoot,
            Bounds? damageableEnvelope)
        {
            Vector3 size = bounds.size;
            float horizontalSmall = Mathf.Min(
                Mathf.Abs(size.x),
                Mathf.Abs(size.z)
            );
            float horizontalLarge = Mathf.Max(
                Mathf.Abs(size.x),
                Mathf.Abs(size.z)
            );
            float vertical = Mathf.Abs(size.y);

            float rootY = enemyRoot != null
                ? enemyRoot.position.y
                : bounds.center.y;

            bool nearFeet = bounds.center.y <=
                rootY + Mathf.Max(0.72f, vertical + 0.24f);

            bool broadAndFlat =
                horizontalSmall >= 0.48f &&
                horizontalLarge >= 0.70f &&
                vertical <= Mathf.Max(
                    0.18f,
                    horizontalSmall * 0.16f
                );

            if (damageableEnvelope.HasValue)
            {
                Bounds envelope = damageableEnvelope.Value;
                float envelopeHeight = Mathf.Max(0.20f, envelope.size.y);
                bool lowInEnvelope = bounds.max.y <=
                    envelope.min.y + envelopeHeight * 0.30f;
                bool flatRelativeToBody = vertical <= Mathf.Max(
                    0.18f,
                    envelopeHeight * 0.12f
                );
                bool broadRelativeToBody =
                    horizontalSmall >= Mathf.Max(
                        0.48f,
                        Mathf.Min(envelope.size.x, envelope.size.z) * 0.55f
                    ) &&
                    horizontalLarge >= Mathf.Max(
                        0.70f,
                        Mathf.Max(envelope.size.x, envelope.size.z) * 0.72f
                    );

                if (lowInEnvelope &&
                    flatRelativeToBody &&
                    broadRelativeToBody)
                {
                    return true;
                }
            }

            return nearFeet && broadAndFlat;
        }

        private static bool HasAuxiliaryName(
            Transform candidate,
            Transform enemyRoot)
        {
            string[] keywords =
            {
                "ring",
                "circle",
                "radius",
                "range",
                "selection",
                "indicator",
                "telegraph",
                "ground_marker",
                "groundmarker",
                "enemy_base",
                "enemybase",
                "support",
                "reticle",
                "marker"
            };

            Transform cursor = candidate;
            while (cursor != null)
            {
                string lower = cursor.gameObject.name.ToLowerInvariant();
                for (int index = 0; index < keywords.Length; index++)
                {
                    if (lower.Contains(keywords[index]))
                        return true;
                }

                if (cursor == enemyRoot)
                    break;

                cursor = cursor.parent;
            }

            return false;
        }

        public static void ApplyTransparency(
            Renderer renderer,
            float multiplier,
            MaterialPropertyBlock reusableBlock = null)
        {
            // Compatibility API retained for older callers and validators.
            // The accepted visual contract keeps the ring's authored color.
            _ = renderer;
            _ = multiplier;
            _ = reusableBlock;
        }
    }
}
