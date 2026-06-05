using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    public static class BDMiniBossRunProgress
    {
        private static readonly HashSet<int> DefeatedMiniBosses = new HashSet<int>();
        private static bool thirdVictoryRewardClaimed;

        public static int DefeatedCount => DefeatedMiniBosses.Count;
        public static bool ThirdVictoryRewardClaimed => thirdVictoryRewardClaimed;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRunState()
        {
            DefeatedMiniBosses.Clear();
            thirdVictoryRewardClaimed = false;
        }

        public static int RegisterVictory(int encounterId)
        {
            if (encounterId == 0)
                return DefeatedMiniBosses.Count;

            DefeatedMiniBosses.Add(encounterId);
            return DefeatedMiniBosses.Count;
        }

        public static bool TryClaimThirdVictoryReward()
        {
            if (thirdVictoryRewardClaimed || DefeatedMiniBosses.Count < 3)
                return false;

            thirdVictoryRewardClaimed = true;
            return true;
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDMiniBossThirdVictoryParryReward : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BDBossEncounterController encounter;
        [SerializeField] private BDBossRewardChest rewardChest;

        [Header("Bonus placement")]
        [SerializeField] private Vector3 bonusLocalOffset = new Vector3(0.75f, 0.35f, 0f);
        [SerializeField] private bool unlockChestOnVictory = true;

        [Header("Debug")]
        [SerializeField] private bool logProgress = true;

        private BDParryFreezeBoostPickup bonusPickup;
        private bool victoryHandled;

        private void Awake()
        {
            ResolveReferences();
        }

        private void OnEnable()
        {
            ResolveReferences();

            if (encounter != null)
            {
                encounter.StateChanged -= HandleEncounterStateChanged;
                encounter.StateChanged += HandleEncounterStateChanged;

                if (encounter.State == BDBossEncounterState.Victory)
                    HandleVictory();
            }

            if (rewardChest != null)
            {
                rewardChest.RewardRevealed -= HandleChestRewardRevealed;
                rewardChest.RewardRevealed += HandleChestRewardRevealed;
            }
        }

        private void OnDisable()
        {
            if (encounter != null)
                encounter.StateChanged -= HandleEncounterStateChanged;

            if (rewardChest != null)
                rewardChest.RewardRevealed -= HandleChestRewardRevealed;
        }

        private void ResolveReferences()
        {
            if (encounter == null)
                encounter = GetComponent<BDBossEncounterController>();

            if (encounter == null)
                encounter = GetComponentInParent<BDBossEncounterController>();

            if (rewardChest == null)
                rewardChest = GetComponentInChildren<BDBossRewardChest>(includeInactive: true);

            if (rewardChest == null && transform.parent != null)
                rewardChest = transform.parent.GetComponentInChildren<BDBossRewardChest>(includeInactive: true);
        }

        private void HandleEncounterStateChanged(BDBossEncounterState state)
        {
            if (state == BDBossEncounterState.Victory)
                HandleVictory();
            else if (state == BDBossEncounterState.Dormant)
                victoryHandled = false;
        }

        private void HandleVictory()
        {
            if (victoryHandled || encounter == null)
                return;

            victoryHandled = true;

            int defeatedCount = BDMiniBossRunProgress.RegisterVictory(encounter.GetInstanceID());

            if (logProgress)
                Debug.Log($"Mini-boss defeated: {defeatedCount}/3 for the special Parry reward.", this);

            if (unlockChestOnVictory && rewardChest != null)
                rewardChest.Unlock();

            if (defeatedCount == 3 && BDMiniBossRunProgress.TryClaimThirdVictoryReward())
                PrepareThirdVictoryBonus();
        }

        private void PrepareThirdVictoryBonus()
        {
            if (rewardChest == null)
            {
                Debug.LogError("The third mini-boss was defeated, but no reward chest is assigned for the Parry boost.", this);
                return;
            }

            if (bonusPickup == null)
                bonusPickup = CreateBonusPickup(rewardChest.transform);

            if (rewardChest.IsOpen && !rewardChest.IsAnimating)
                RevealBonusPickup();
        }

        private BDParryFreezeBoostPickup CreateBonusPickup(Transform chestTransform)
        {
            GameObject pickupObject = new GameObject("BD_Parry_Freeze_2s_Boost_Pickup");
            pickupObject.transform.SetParent(chestTransform, false);
            pickupObject.transform.localPosition = bonusLocalOffset;
            pickupObject.transform.localRotation = Quaternion.identity;

            BDParryFreezeBoostPickup pickup = pickupObject.AddComponent<BDParryFreezeBoostPickup>();
            pickup.ConfigureAsHiddenChestReward(bonusLocalOffset);
            pickupObject.SetActive(false);
            return pickup;
        }

        private void HandleChestRewardRevealed()
        {
            RevealBonusPickup();
        }

        private void RevealBonusPickup()
        {
            if (bonusPickup == null)
                return;

            GameObject pickupObject = bonusPickup.gameObject;
            if (!pickupObject.activeSelf)
                pickupObject.SetActive(true);

            bonusPickup.BeginChestReveal();
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDParryFreezeBoostPickup : MonoBehaviour
    {
        [Header("Pickup")]
        [SerializeField] private float pickupRadius = 0.72f;
        [SerializeField] private float hoverHeight = 0.55f;
        [SerializeField] private float hoverAmplitude = 0.10f;
        [SerializeField] private float hoverSpeed = 2.8f;
        [SerializeField] private float rotationSpeed = 95f;

        [Header("Reveal")]
        [SerializeField] private float revealRiseHeight = 0.70f;
        [SerializeField] private float revealDuration = 0.48f;

        private Vector3 hiddenLocalPosition;
        private Vector3 hoverLocalPosition;
        private Transform visualRoot;
        private bool revealed;
        private bool collected;
        private Coroutine revealRoutine;

        public bool IsCollected => collected;

        private void Awake()
        {
            BuildVisual();
            EnsureTrigger();
        }

        public void ConfigureAsHiddenChestReward(Vector3 localPosition)
        {
            hiddenLocalPosition = localPosition;
            hoverLocalPosition = localPosition + Vector3.up * hoverHeight;
            transform.localPosition = hiddenLocalPosition;
        }

        public void BeginChestReveal()
        {
            if (revealed || collected)
                return;

            revealed = true;

            if (revealRoutine != null)
                StopCoroutine(revealRoutine);

            revealRoutine = StartCoroutine(RevealRoutine());
        }

        private void Update()
        {
            if (!revealed || collected || revealRoutine != null)
                return;

            float bob = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
            transform.localPosition = hoverLocalPosition + Vector3.up * bob;
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!revealed || collected || other == null)
                return;

            BDPlayerMarker player = other.GetComponentInParent<BDPlayerMarker>();
            if (player == null)
                return;

            BDPlayerParryState parryState = player.GetComponent<BDPlayerParryState>();
            if (parryState == null)
                parryState = player.gameObject.AddComponent<BDPlayerParryState>();

            if (!parryState.CollectExtendedFreezeUpgrade())
            {
                collected = true;
                Destroy(gameObject);
                return;
            }

            collected = true;
            SpawnCollectionBurst(transform.position);
            Debug.Log("Special boost collected: Parry time freeze is now 2.0 seconds.", player);
            Destroy(gameObject);
        }

        private IEnumerator RevealRoutine()
        {
            Vector3 start = hiddenLocalPosition;
            Vector3 end = hiddenLocalPosition + Vector3.up * Mathf.Max(0f, revealRiseHeight + hoverHeight);
            float duration = Mathf.Max(0.01f, revealDuration);
            float elapsed = 0f;

            transform.localPosition = start;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float eased = 1f - Mathf.Pow(1f - t, 3f);
                transform.localPosition = Vector3.Lerp(start, end, eased);
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
                yield return null;
            }

            hoverLocalPosition = end;
            transform.localPosition = end;
            revealRoutine = null;
        }

        private void EnsureTrigger()
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

            GameObject root = new GameObject("Visual");
            visualRoot = root.transform;
            visualRoot.SetParent(transform, false);

            Color coreColor = new Color(0.28f, 0.92f, 1f, 1f);
            Color ringColor = new Color(0.66f, 0.34f, 1f, 1f);

            GameObject core = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            core.name = "Parry_Time_Core";
            core.transform.SetParent(visualRoot, false);
            core.transform.localScale = Vector3.one * 0.42f;
            ConfigureVisual(core, coreColor);

            GameObject vertical = GameObject.CreatePrimitive(PrimitiveType.Cube);
            vertical.name = "Parry_Time_II_Vertical";
            vertical.transform.SetParent(visualRoot, false);
            vertical.transform.localPosition = new Vector3(-0.12f, 0f, -0.24f);
            vertical.transform.localScale = new Vector3(0.07f, 0.46f, 0.07f);
            ConfigureVisual(vertical, Color.white);

            GameObject verticalTwo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            verticalTwo.name = "Parry_Time_II_Vertical_2";
            verticalTwo.transform.SetParent(visualRoot, false);
            verticalTwo.transform.localPosition = new Vector3(0.12f, 0f, -0.24f);
            verticalTwo.transform.localScale = new Vector3(0.07f, 0.46f, 0.07f);
            ConfigureVisual(verticalTwo, Color.white);

            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f;
                GameObject mote = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mote.name = $"Parry_Orbit_Mote_{i}";
                mote.transform.SetParent(visualRoot, false);
                mote.transform.localPosition = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * 0.52f;
                mote.transform.localScale = Vector3.one * 0.09f;
                ConfigureVisual(mote, ringColor);
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
                material.SetColor("_EmissionColor", color * 2.8f);
            }
        }

        private static void SpawnCollectionBurst(Vector3 position)
        {
            GameObject root = new GameObject("BD_Parry_2s_Boost_Collected_Burst");
            root.transform.position = position;

            for (int i = 0; i < 14; i++)
            {
                float angle = i * (360f / 14f);
                Vector3 direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                GameObject mote = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mote.name = "Boost_Collected_Mote";
                mote.transform.SetParent(root.transform, false);
                mote.transform.localPosition = direction * 0.85f;
                mote.transform.localScale = Vector3.one * 0.12f;
                ConfigureVisual(mote, new Color(0.36f, 0.94f, 1f, 1f));
            }

            BDBoostCollectionBurstLifetime lifetime = root.AddComponent<BDBoostCollectionBurstLifetime>();
            lifetime.Configure(0.42f, 1.65f);
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDBoostCollectionBurstLifetime : MonoBehaviour
    {
        private float lifetime = 0.42f;
        private float endScale = 1.65f;
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
            transform.localScale = Vector3.one * Mathf.Lerp(0.65f, endScale, t);

            if (elapsed >= lifetime)
                Destroy(gameObject);
        }
    }
}
