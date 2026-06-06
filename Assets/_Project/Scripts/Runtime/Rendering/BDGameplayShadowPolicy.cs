using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-35)]
    [DisallowMultipleComponent]
    public sealed class BDGameplayShadowPolicy : MonoBehaviour
    {
        public enum ShadowClass
        {
            None = 0,
            Optional = 1,
            Required = 2
        }

        [Header("Required Gameplay Shadows")]
        [SerializeField] private bool keepRequiredAlwaysOn = true;

        [Header("Optional Performance Budget")]
        [SerializeField] private float optionalShadowDistance = 22f;
        [SerializeField] private int maxOptionalShadowRenderers = 28;
        [SerializeField] private float budgetRefreshInterval = 0.35f;
        [SerializeField] private float dynamicDiscoveryInterval = 2.5f;

        private readonly List<Renderer> requiredRenderers =
            new List<Renderer>();

        private readonly List<Renderer> optionalRenderers =
            new List<Renderer>();

        private Camera targetCamera;
        private float nextBudgetRefreshAt;
        private float nextDiscoveryAt;

        public float OptionalShadowDistance => optionalShadowDistance;
        public int MaxOptionalShadowRenderers => maxOptionalShadowRenderers;

        private static readonly string[] RequiredComponentTokens =
        {
            "Player",
            "Horse",
            "Enemy",
            "Boss",
            "MiniBoss",
            "Collectible",
            "Pickup",
            "Battery",
            "GameBoy",
            "Gameboy",
            "Cartridge",
            "Cassette",
            "Tape",
            "QuestItem",
            "Interactable"
        };

        private static readonly string[] RequiredNameTokens =
        {
            "player",
            "horse",
            "enemy",
            "boss",
            "mini_boss",
            "miniboss",
            "battery",
            "gameboy",
            "game_boy",
            "cartridge",
            "cassette",
            "game_tape",
            "collectible",
            "pickup"
        };

        private static readonly string[] OptionalComponentTokens =
        {
            "Door",
            "Chest",
            "Switch",
            "Lever",
            "Trap",
            "Projectile",
            "Weapon",
            "MovingPlatform",
            "Breakable",
            "Destructible",
            "Interactive",
            "Interactable"
        };

        private static readonly string[] DecorationTokens =
        {
            "ui",
            "canvas",
            "minimap",
            "label",
            "telegraph",
            "vfx",
            "particle",
            "trail",
            "line",
            "ground",
            "floor",
            "wall",
            "ceiling",
            "terrain",
            "decoration",
            "decor",
            "background",
            "lava",
            "hole",
            "chasm",
            "hazard"
        };

        public void Configure(
            float distance,
            int optionalRendererBudget,
            float refreshSeconds,
            float discoverySeconds)
        {
            optionalShadowDistance = Mathf.Max(4f, distance);
            maxOptionalShadowRenderers =
                Mathf.Max(0, optionalRendererBudget);
            budgetRefreshInterval =
                Mathf.Max(0.10f, refreshSeconds);
            dynamicDiscoveryInterval =
                Mathf.Max(0.75f, discoverySeconds);
        }

        public void ApplyNow()
        {
            DiscoverRenderers();
            ApplyRequiredShadows();
            ApplyOptionalBudget();
        }

        private void Awake()
        {
            ApplyNow();
        }

        private void OnEnable()
        {
            ApplyNow();
        }

        private void Update()
        {
            float now = Time.unscaledTime;

            if (now >= nextDiscoveryAt)
            {
                nextDiscoveryAt =
                    now +
                    Mathf.Max(0.75f, dynamicDiscoveryInterval);

                DiscoverRenderers();
                ApplyRequiredShadows();
            }

            if (now < nextBudgetRefreshAt)
                return;

            nextBudgetRefreshAt =
                now +
                Mathf.Max(0.10f, budgetRefreshInterval);

            ApplyOptionalBudget();
        }

        public static ShadowClass ClassifyRenderer(
            Renderer renderer)
        {
            if (!IsEligibleRenderer(renderer))
                return ShadowClass.None;

            Transform current = renderer.transform;
            bool decorationDetected = false;
            bool optionalDetected = false;
            int depth = 0;

            while (current != null && depth < 16)
            {
                GameObject candidate = current.gameObject;

                if (HasRequiredIdentity(candidate))
                    return ShadowClass.Required;

                if (ContainsAny(
                        candidate.name,
                        DecorationTokens))
                {
                    decorationDetected = true;
                }

                if (HasOptionalIdentity(candidate))
                    optionalDetected = true;

                current = current.parent;
                depth++;
            }

            if (optionalDetected && !decorationDetected)
                return ShadowClass.Optional;

            return ShadowClass.None;
        }

        public static bool IsEligibleRenderer(
            Renderer renderer)
        {
            return renderer is MeshRenderer ||
                   renderer is SkinnedMeshRenderer;
        }

        private void DiscoverRenderers()
        {
            requiredRenderers.Clear();
            optionalRenderers.Clear();

            Renderer[] discovered =
                FindObjectsByType<Renderer>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            for (int index = 0;
                 index < discovered.Length;
                 index++)
            {
                Renderer renderer = discovered[index];
                ShadowClass classification =
                    ClassifyRenderer(renderer);

                if (classification == ShadowClass.Required)
                {
                    requiredRenderers.Add(renderer);
                }
                else if (classification == ShadowClass.Optional)
                {
                    optionalRenderers.Add(renderer);
                }
            }
        }

        private void ApplyRequiredShadows()
        {
            if (!keepRequiredAlwaysOn)
                return;

            for (int index = 0;
                 index < requiredRenderers.Count;
                 index++)
            {
                ApplyRendererShadow(
                    requiredRenderers[index],
                    enabled: true
                );
            }
        }

        private void ApplyOptionalBudget()
        {
            if (!Application.isPlaying)
            {
                for (int index = 0;
                     index < optionalRenderers.Count;
                     index++)
                {
                    ApplyRendererShadow(
                        optionalRenderers[index],
                        enabled: true
                    );
                }

                return;
            }

            if (targetCamera == null)
                targetCamera = Camera.main;

            if (targetCamera == null)
            {
                for (int index = 0;
                     index < optionalRenderers.Count;
                     index++)
                {
                    ApplyRendererShadow(
                        optionalRenderers[index],
                        enabled: false
                    );
                }

                return;
            }

            Vector3 cameraPosition =
                targetCamera.transform.position;

            optionalRenderers.Sort(
                (left, right) =>
                    SqrDistance(left, cameraPosition)
                        .CompareTo(
                            SqrDistance(
                                right,
                                cameraPosition
                            )
                        )
            );

            float maximumDistanceSquared =
                optionalShadowDistance *
                optionalShadowDistance;

            int enabledCount = 0;

            for (int index = 0;
                 index < optionalRenderers.Count;
                 index++)
            {
                Renderer renderer =
                    optionalRenderers[index];

                bool insideDistance =
                    SqrDistance(
                        renderer,
                        cameraPosition
                    ) <= maximumDistanceSquared;

                bool insideBudget =
                    enabledCount <
                    maxOptionalShadowRenderers;

                bool enable =
                    insideDistance &&
                    insideBudget;

                ApplyRendererShadow(
                    renderer,
                    enable
                );

                if (enable)
                    enabledCount++;
            }
        }

        private static void ApplyRendererShadow(
            Renderer renderer,
            bool enabled)
        {
            if (!IsEligibleRenderer(renderer))
                return;

            renderer.shadowCastingMode =
                enabled
                    ? ShadowCastingMode.On
                    : ShadowCastingMode.Off;

            renderer.receiveShadows = true;
        }

        private static float SqrDistance(
            Renderer renderer,
            Vector3 position)
        {
            if (renderer == null)
                return float.PositiveInfinity;

            return (
                renderer.bounds.center -
                position
            ).sqrMagnitude;
        }

        private static bool HasRequiredIdentity(
            GameObject candidate)
        {
            if (candidate == null)
                return false;

            if (ContainsAny(
                    candidate.name,
                    RequiredNameTokens))
            {
                return true;
            }

            MonoBehaviour[] behaviours =
                candidate.GetComponents<MonoBehaviour>();

            for (int index = 0;
                 index < behaviours.Length;
                 index++)
            {
                MonoBehaviour behaviour =
                    behaviours[index];

                if (behaviour == null)
                    continue;

                string typeName =
                    behaviour.GetType().Name;

                if (typeName == "BDPlayerMarker" ||
                    typeName == "BDHorseController" ||
                    typeName == "BDHorseHealth" ||
                    typeName == "BDEnemyBootstrap" ||
                    typeName == "BDBossHealthChannel" ||
                    typeName ==
                        "BDCollectibleGuardianSpawner")
                {
                    return true;
                }

                if (ContainsAny(
                        typeName,
                        RequiredComponentTokens))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasOptionalIdentity(
            GameObject candidate)
        {
            if (candidate == null ||
                candidate.isStatic)
            {
                return false;
            }

            if (candidate.GetComponent<Rigidbody>() != null ||
                candidate.GetComponent<CharacterController>() !=
                    null)
            {
                return true;
            }

            Collider collider =
                candidate.GetComponent<Collider>();

            if (collider == null ||
                collider.isTrigger)
            {
                return false;
            }

            MonoBehaviour[] behaviours =
                candidate.GetComponents<MonoBehaviour>();

            for (int index = 0;
                 index < behaviours.Length;
                 index++)
            {
                MonoBehaviour behaviour =
                    behaviours[index];

                if (behaviour == null)
                    continue;

                if (ContainsAny(
                        behaviour.GetType().Name,
                        OptionalComponentTokens))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsAny(
            string value,
            string[] tokens)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            for (int index = 0;
                 index < tokens.Length;
                 index++)
            {
                if (value.IndexOf(
                        tokens[index],
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
