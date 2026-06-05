using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    public enum BDPlayerBoostType
    {
        ExtraAmmo = 0,
        FasterReload = 1,
        MovementSpeed = 2,
        WeaponDamage = 3
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDPlayerMarker))]
    public sealed class BDPlayerBoostState : MonoBehaviour
    {
        [Header("Limits")]
        [SerializeField, Min(1)] private int maxStacksPerBoost = 3;

        [Header("Per-stack values")]
        [SerializeField, Min(1)] private int extraAmmoPerStack = 1;
        [SerializeField, Min(0.1f)] private float reloadSecondsReductionPerStack = 1f;
        [SerializeField, Range(0.01f, 0.50f)] private float movementSpeedIncreasePerStack = 0.07f;
        [SerializeField, Range(0.01f, 0.50f)] private float weaponDamageIncreasePerStack = 0.10f;
        [SerializeField, Min(0.25f)] private float minimumReloadDuration = 1f;

        [Header("Run state")]
        [SerializeField] private int extraAmmoStacks;
        [SerializeField] private int fasterReloadStacks;
        [SerializeField] private int movementSpeedStacks;
        [SerializeField] private int weaponDamageStacks;

        [Header("Pickup feedback")]
        [SerializeField] private float feedbackDuration = 1.5f;

        private BDPlayerCombat combat;
        private BDPlayerController controller;
        private string feedbackText = string.Empty;
        private float feedbackEndsAtUnscaled;
        private GUIStyle feedbackStyle;

        public event Action<BDPlayerBoostType, int> BoostCollected;

        public int MaxStacksPerBoost => Mathf.Max(1, maxStacksPerBoost);
        public int ExtraAmmoStacks => extraAmmoStacks;
        public int FasterReloadStacks => fasterReloadStacks;
        public int MovementSpeedStacks => movementSpeedStacks;
        public int WeaponDamageStacks => weaponDamageStacks;

        public bool HasAnyAvailableBoost =>
            extraAmmoStacks < MaxStacksPerBoost ||
            fasterReloadStacks < MaxStacksPerBoost ||
            movementSpeedStacks < MaxStacksPerBoost ||
            weaponDamageStacks < MaxStacksPerBoost;

        private void Awake()
        {
            ResolveReferences();
            ClampStacks();
            ApplyAllBoosts(grantAddedAmmo: false);
        }

        private void Start()
        {
            ResolveReferences();
            ApplyAllBoosts(grantAddedAmmo: false);
        }

        public int GetStacks(BDPlayerBoostType type)
        {
            switch (type)
            {
                case BDPlayerBoostType.ExtraAmmo:
                    return extraAmmoStacks;
                case BDPlayerBoostType.FasterReload:
                    return fasterReloadStacks;
                case BDPlayerBoostType.MovementSpeed:
                    return movementSpeedStacks;
                case BDPlayerBoostType.WeaponDamage:
                    return weaponDamageStacks;
                default:
                    return 0;
            }
        }

        public bool CanCollect(BDPlayerBoostType type)
        {
            return GetStacks(type) < MaxStacksPerBoost;
        }

        public bool TryCollect(BDPlayerBoostType type)
        {
            ResolveReferences();

            if (combat == null || controller == null || !CanCollect(type))
                return false;

            bool grantAddedAmmo = false;

            switch (type)
            {
                case BDPlayerBoostType.ExtraAmmo:
                    extraAmmoStacks++;
                    grantAddedAmmo = true;
                    break;
                case BDPlayerBoostType.FasterReload:
                    fasterReloadStacks++;
                    break;
                case BDPlayerBoostType.MovementSpeed:
                    movementSpeedStacks++;
                    break;
                case BDPlayerBoostType.WeaponDamage:
                    weaponDamageStacks++;
                    break;
                default:
                    return false;
            }

            ApplyAllBoosts(grantAddedAmmo);
            int currentStacks = GetStacks(type);
            ShowFeedback(type, currentStacks);
            BoostCollected?.Invoke(type, currentStacks);
            return true;
        }

        public bool TryChooseRandomAvailable(out BDPlayerBoostType type)
        {
            List<BDPlayerBoostType> available = new List<BDPlayerBoostType>(4);

            if (CanCollect(BDPlayerBoostType.ExtraAmmo))
                available.Add(BDPlayerBoostType.ExtraAmmo);
            if (CanCollect(BDPlayerBoostType.FasterReload))
                available.Add(BDPlayerBoostType.FasterReload);
            if (CanCollect(BDPlayerBoostType.MovementSpeed))
                available.Add(BDPlayerBoostType.MovementSpeed);
            if (CanCollect(BDPlayerBoostType.WeaponDamage))
                available.Add(BDPlayerBoostType.WeaponDamage);

            if (available.Count == 0)
            {
                type = default(BDPlayerBoostType);
                return false;
            }

            type = available[UnityEngine.Random.Range(0, available.Count)];
            return true;
        }

        private void ResolveReferences()
        {
            if (combat == null)
                combat = GetComponent<BDPlayerCombat>();

            if (controller == null)
                controller = GetComponent<BDPlayerController>();
        }

        private void ClampStacks()
        {
            int maximum = MaxStacksPerBoost;
            extraAmmoStacks = Mathf.Clamp(extraAmmoStacks, 0, maximum);
            fasterReloadStacks = Mathf.Clamp(fasterReloadStacks, 0, maximum);
            movementSpeedStacks = Mathf.Clamp(movementSpeedStacks, 0, maximum);
            weaponDamageStacks = Mathf.Clamp(weaponDamageStacks, 0, maximum);
        }

        private void ApplyAllBoosts(bool grantAddedAmmo)
        {
            ResolveReferences();

            if (combat != null)
            {
                int additionalMagazineCapacity =
                    extraAmmoStacks * Mathf.Max(1, extraAmmoPerStack);

                float reloadReduction =
                    fasterReloadStacks * Mathf.Max(0.1f, reloadSecondsReductionPerStack);

                float damageMultiplier =
                    1f + weaponDamageStacks * Mathf.Max(0.01f, weaponDamageIncreasePerStack);

                combat.ApplyBoostModifiers(
                    additionalMagazineCapacity,
                    reloadReduction,
                    damageMultiplier,
                    Mathf.Max(0.25f, minimumReloadDuration),
                    grantAddedAmmo
                );
            }

            if (controller != null)
            {
                float movementMultiplier =
                    1f + movementSpeedStacks * Mathf.Max(0.01f, movementSpeedIncreasePerStack);

                controller.SetBoostMoveSpeedMultiplier(movementMultiplier);
            }
        }

        private void ShowFeedback(BDPlayerBoostType type, int stacks)
        {
            feedbackText = $"{ResolveDisplayName(type)}  {stacks}/{MaxStacksPerBoost}";
            feedbackEndsAtUnscaled =
                Time.unscaledTime + Mathf.Max(0.2f, feedbackDuration);
        }

        private void OnGUI()
        {
            if (Time.unscaledTime >= feedbackEndsAtUnscaled ||
                string.IsNullOrEmpty(feedbackText))
                return;

            EnsureFeedbackStyle();

            float width = Mathf.Min(420f, Screen.width - 32f);
            Rect panel = new Rect(
                (Screen.width - width) * 0.5f,
                Screen.height * 0.16f,
                width,
                54f
            );

            Color previous = GUI.color;
            GUI.color = new Color(0.02f, 0.04f, 0.08f, 0.86f);
            GUI.DrawTexture(panel, Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(panel, feedbackText, feedbackStyle);
            GUI.color = previous;
        }

        private void EnsureFeedbackStyle()
        {
            if (feedbackStyle != null)
                return;

            feedbackStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18,
                fontStyle = FontStyle.Bold
            };

            feedbackStyle.normal.textColor =
                new Color(0.78f, 0.97f, 1f, 1f);
        }

        public static string ResolveDisplayName(BDPlayerBoostType type)
        {
            switch (type)
            {
                case BDPlayerBoostType.ExtraAmmo:
                    return "+1 AMMO";
                case BDPlayerBoostType.FasterReload:
                    return "-1s RELOAD";
                case BDPlayerBoostType.MovementSpeed:
                    return "MOVE SPEED +";
                case BDPlayerBoostType.WeaponDamage:
                    return "WEAPON DAMAGE +";
                default:
                    return "BOOST";
            }
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDPlayerBoostPickup : MonoBehaviour
    {
        [SerializeField] private BDPlayerBoostType boostType;
        [SerializeField] private float pickupRadius = 0.65f;
        [SerializeField] private float hoverAmplitude = 0.12f;
        [SerializeField] private float hoverSpeed = 2.8f;
        [SerializeField] private float rotationSpeed = 90f;

        private Vector3 basePosition;
        private bool collected;
        private Transform visualRoot;

        public BDPlayerBoostType BoostType => boostType;

        public static BDPlayerBoostPickup Spawn(Vector3 position, BDPlayerBoostType type)
        {
            GameObject root = new GameObject($"BD_Boost_{type}");
            root.transform.position = position;

            BDPlayerBoostPickup pickup = root.AddComponent<BDPlayerBoostPickup>();
            pickup.boostType = type;
            pickup.basePosition = position;
            pickup.BuildVisual();
            return pickup;
        }

        private void Awake()
        {
            basePosition = transform.position;
            EnsurePhysics();
        }

        private void Start()
        {
            if (visualRoot == null)
                BuildVisual();
        }

        private void Update()
        {
            if (collected)
                return;

            float bob = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
            transform.position = basePosition + Vector3.up * bob;
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (collected || other == null)
                return;

            BDPlayerMarker player = other.GetComponentInParent<BDPlayerMarker>();
            if (player == null)
                return;

            BDPlayerBoostState state = player.GetComponent<BDPlayerBoostState>();
            if (state == null)
                state = player.gameObject.AddComponent<BDPlayerBoostState>();

            BDPlayerBoostType resolvedType = boostType;

            if (!state.CanCollect(resolvedType) &&
                !state.TryChooseRandomAvailable(out resolvedType))
            {
                collected = true;
                Destroy(gameObject);
                return;
            }

            if (!state.TryCollect(resolvedType))
                return;

            collected = true;
            SpawnCollectionBurst(transform.position, ResolveColor(resolvedType));
            Destroy(gameObject);
        }

        private void EnsurePhysics()
        {
            SphereCollider trigger = GetComponent<SphereCollider>();
            if (trigger == null)
                trigger = gameObject.AddComponent<SphereCollider>();

            trigger.isTrigger = true;
            trigger.radius = Mathf.Max(0.1f, pickupRadius);

            Rigidbody body = GetComponent<Rigidbody>();
            if (body == null)
                body = gameObject.AddComponent<Rigidbody>();

            body.isKinematic = true;
            body.useGravity = false;
        }

        private void BuildVisual()
        {
            if (visualRoot != null)
                return;

            GameObject visual = new GameObject("Visual");
            visualRoot = visual.transform;
            visualRoot.SetParent(transform, false);

            Color color = ResolveColor(boostType);

            GameObject core = GameObject.CreatePrimitive(PrimitiveType.Cube);
            core.name = "Boost_Core";
            core.transform.SetParent(visualRoot, false);
            core.transform.localRotation = Quaternion.Euler(35f, 45f, 20f);
            core.transform.localScale = new Vector3(0.42f, 0.42f, 0.42f);
            ConfigureVisual(core, color);

            for (int i = 0; i < 6; i++)
            {
                float angle = i * 60f;
                GameObject mote = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mote.name = $"Boost_Mote_{i}";
                mote.transform.SetParent(visualRoot, false);
                mote.transform.localPosition =
                    Quaternion.Euler(0f, angle, 0f) * Vector3.forward * 0.48f;
                mote.transform.localScale = Vector3.one * 0.09f;
                ConfigureVisual(mote, color);
            }
        }

        private static Color ResolveColor(BDPlayerBoostType type)
        {
            switch (type)
            {
                case BDPlayerBoostType.ExtraAmmo:
                    return new Color(0.30f, 0.86f, 1f, 1f);
                case BDPlayerBoostType.FasterReload:
                    return new Color(1f, 0.72f, 0.16f, 1f);
                case BDPlayerBoostType.MovementSpeed:
                    return new Color(0.30f, 1f, 0.44f, 1f);
                case BDPlayerBoostType.WeaponDamage:
                    return new Color(1f, 0.24f, 0.28f, 1f);
                default:
                    return Color.white;
            }
        }

        private static void ConfigureVisual(GameObject visual, Color color)
        {
            Collider collider = visual.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            Renderer renderer = visual.GetComponent<Renderer>();
            if (renderer == null)
                return;

            Material material = renderer.material;
            material.color = color;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * 2.5f);
            }
        }

        private static void SpawnCollectionBurst(Vector3 position, Color color)
        {
            GameObject root = new GameObject("BD_Boost_Collection_Burst");
            root.transform.position = position;

            for (int i = 0; i < 12; i++)
            {
                float angle = i * 30f;
                GameObject mote = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mote.name = "Boost_Burst_Mote";
                mote.transform.SetParent(root.transform, false);
                mote.transform.localPosition =
                    Quaternion.Euler(0f, angle, 0f) * Vector3.forward * 0.75f;
                mote.transform.localScale = Vector3.one * 0.10f;
                ConfigureVisual(mote, color);
            }

            BDRegularBoostBurstLifetime lifetime =
                root.AddComponent<BDRegularBoostBurstLifetime>();

            lifetime.Configure(0.38f, 1.55f);
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDRegularBoostBurstLifetime : MonoBehaviour
    {
        private float lifetime = 0.38f;
        private float endScale = 1.55f;
        private float elapsed;

        public void Configure(float newLifetime, float newEndScale)
        {
            lifetime = Mathf.Max(0.05f, newLifetime);
            endScale = Mathf.Max(1f, newEndScale);
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / lifetime);
            transform.localScale =
                Vector3.one * Mathf.Lerp(0.65f, endScale, t);

            if (elapsed >= lifetime)
                Destroy(gameObject);
        }
    }

    public static class BDPlayerBoostInstaller
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallAfterSceneLoad()
        {
            Install();
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Install();
        }

        private static void Install()
        {
            BDPlayerMarker[] players =
                UnityEngine.Object.FindObjectsByType<BDPlayerMarker>(
                    FindObjectsSortMode.None
                );

            for (int i = 0; i < players.Length; i++)
            {
                BDPlayerMarker player = players[i];

                if (player != null &&
                    player.GetComponent<BDPlayerBoostState>() == null)
                {
                    player.gameObject.AddComponent<BDPlayerBoostState>();
                }
            }
        }
    }
}
