using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDAuxiliaryEnemyRingTransparency : MonoBehaviour
    {
        // BD AUXILIARY ENEMY RING TRANSPARENCY V23R19O
        [SerializeField, Range(0.20f, 0.90f)]
        private float alphaMultiplier = 0.62f;

        private bool applying;
        private readonly MaterialPropertyBlock propertyBlock =
            new MaterialPropertyBlock();

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

        private void OnTransformChildrenChanged()
        {
            if (Application.isPlaying)
                Apply();
        }

        public void Apply()
        {
            if (applying)
                return;

            applying = true;
            try
            {
                Renderer[] renderers =
                    GetComponentsInChildren<Renderer>(
                        includeInactive: true
                    );

                for (int index = 0;
                     index < renderers.Length;
                     index++)
                {
                    Renderer candidate = renderers[index];
                    if (!IsAuxiliaryRingRenderer(
                            candidate,
                            transform))
                    {
                        continue;
                    }

                    ApplyTransparency(
                        candidate,
                        alphaMultiplier,
                        propertyBlock
                    );
                }
            }
            finally
            {
                applying = false;
            }
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

            string lower =
                candidate.gameObject.name.ToLowerInvariant();

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
                "enemybase"
            };

            for (int index = 0;
                 index < keywords.Length;
                 index++)
            {
                if (lower.Contains(keywords[index]))
                    return true;
            }

            Bounds bounds = candidate.bounds;
            Vector3 size = bounds.size;

            float horizontalSmall =
                Mathf.Min(
                    Mathf.Abs(size.x),
                    Mathf.Abs(size.z)
                );
            float horizontalLarge =
                Mathf.Max(
                    Mathf.Abs(size.x),
                    Mathf.Abs(size.z)
                );
            float vertical =
                Mathf.Abs(size.y);

            bool broadAndFlat =
                horizontalSmall >= 0.55f &&
                horizontalLarge >= 0.75f &&
                vertical <= Mathf.Max(
                    0.20f,
                    horizontalSmall * 0.20f
                );

            if (!broadAndFlat)
                return false;

            float rootY =
                enemyRoot != null
                    ? enemyRoot.position.y
                    : bounds.center.y;

            bool nearFeet =
                bounds.center.y <=
                rootY + Mathf.Max(0.85f, vertical + 0.35f);

            return nearFeet;
        }

        public static void ApplyTransparency(
            Renderer renderer,
            float multiplier,
            MaterialPropertyBlock reusableBlock = null)
        {
            if (renderer == null)
                return;

            Material[] materials = renderer.sharedMaterials;
            if (materials == null || materials.Length == 0)
                return;

            Color sourceColor = Color.white;
            bool foundColor = false;

            for (int index = 0;
                 index < materials.Length;
                 index++)
            {
                Material material = materials[index];
                if (material == null)
                    continue;

                if (material.HasProperty("_BaseColor"))
                {
                    sourceColor =
                        material.GetColor("_BaseColor");
                    foundColor = true;
                    break;
                }

                if (material.HasProperty("_Color"))
                {
                    sourceColor =
                        material.GetColor("_Color");
                    foundColor = true;
                    break;
                }
            }

            if (!foundColor)
                return;

            sourceColor.a = Mathf.Clamp01(
                sourceColor.a *
                Mathf.Clamp(multiplier, 0.20f, 0.90f)
            );

            MaterialPropertyBlock block =
                reusableBlock ??
                new MaterialPropertyBlock();

            renderer.GetPropertyBlock(block);
            block.SetColor("_BaseColor", sourceColor);
            block.SetColor("_Color", sourceColor);
            renderer.SetPropertyBlock(block);
        }
    }
}
