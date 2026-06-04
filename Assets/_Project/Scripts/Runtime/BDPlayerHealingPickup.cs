using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDPlayerHealingPickup : MonoBehaviour
    {
        [Header("Healing")]
        [SerializeField] private float healFractionOfMax = 0.40f;

        [Header("Pickup Feel")]
        [SerializeField] private float magnetRadius = 4.25f;
        [SerializeField] private float collectRadius = 1.15f;
        [SerializeField] private float magnetAcceleration = 18f;
        [SerializeField] private float maxMagnetSpeed = 9f;
        [SerializeField] private float floatAmplitude = 0.12f;
        [SerializeField] private float floatFrequency = 2.8f;
        [SerializeField] private float pulseScale = 0.14f;
        [SerializeField] private bool allowMountedPickup = true;

        [Header("Debug")]
        [SerializeField] private bool logPickup = false;

        private bool consumed;
        private Vector3 baseScale;
        private float baseY;
        private float phase;
        private Vector3 magnetVelocity;
        private Transform cachedPlayer;
        private BDHorseController cachedHorse;


        public static BDPlayerHealingPickup Spawn(Vector3 position)
        {
            return Spawn(position, 0.40f);
        }

        public static BDPlayerHealingPickup Spawn(Vector3 position, float healFraction)
        {
            GameObject pickupObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pickupObject.name = "BD_Player_Healing_Pickup";
            pickupObject.transform.position = position + Vector3.up * 0.35f;
            pickupObject.transform.localScale = Vector3.one * 0.55f;

            Renderer renderer = pickupObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = CreatePickupMaterial();
                if (material != null)
                    renderer.sharedMaterial = material;
            }

            Collider collider = pickupObject.GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true;

            BDPlayerHealingPickup pickup = pickupObject.AddComponent<BDPlayerHealingPickup>();
            pickup.healFractionOfMax = Mathf.Clamp01(healFraction);
            return pickup;
        }

        public static BDPlayerHealingPickup Spawn(Vector3 position, float healFraction, float scale)
        {
            BDPlayerHealingPickup pickup = Spawn(position, healFraction);

            if (pickup != null)
                pickup.transform.localScale = Vector3.one * Mathf.Max(0.1f, scale);

            return pickup;
        }

        private static Material CreatePickupMaterial()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            if (shader == null) shader = Shader.Find("Hidden/InternalErrorShader");

            if (shader != null)
            {
                Material material = new Material(shader);
                material.color = new Color(0.20f, 1f, 0.45f, 1f);

                if (material.HasProperty("_EmissionColor"))
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor("_EmissionColor", new Color(0.08f, 1f, 0.35f, 1f));
                }

                return material;
            }

            Material builtIn = Resources.GetBuiltinResource<Material>("Default-Material.mat");
            if (builtIn != null)
                return new Material(builtIn);

            return null;
        }

        private void Awake()
        {
            baseScale = transform.localScale;
            baseY = transform.position.y;
            phase = Random.Range(0f, 100f);

            EnsureTriggerCollider();
        }

        private void Update()
        {
            if (consumed)
                return;

            AnimatePickup();
            TickMagnet();
        }

        private void EnsureTriggerCollider()
        {
            Collider collider = GetComponent<Collider>();

            if (collider == null)
            {
                SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
                sphere.radius = Mathf.Max(0.5f, collectRadius * 0.55f);
                sphere.isTrigger = true;
                return;
            }

            collider.isTrigger = true;
        }

        private void AnimatePickup()
        {
            float wave = Mathf.Sin((Time.time + phase) * floatFrequency);
            Vector3 position = transform.position;
            float effectiveFloatAmplitude = BDPerformanceGuard.ReducedEffects ? floatAmplitude * 0.45f : floatAmplitude;
            position.y = baseY + wave * effectiveFloatAmplitude;
            transform.position = position;

            float effectivePulseScale = BDPerformanceGuard.ReducedEffects ? pulseScale * 0.45f : pulseScale;
            float scale = 1f + Mathf.Abs(wave) * effectivePulseScale;
            transform.localScale = baseScale * scale;

            transform.Rotate(Vector3.up, 80f * Time.deltaTime, Space.World);
        }

        private void TickMagnet()
        {
            Transform target = ResolveMagnetTarget();

            if (target == null)
                return;

            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;

            float distance = toTarget.magnitude;

            if (distance <= collectRadius)
            {
                TryConsumeFromTransform(target);
                return;
            }

            if (distance > magnetRadius || distance <= 0.001f)
                return;

            Vector3 desired = toTarget.normalized * maxMagnetSpeed;
            float t = 1f - Mathf.Exp(-magnetAcceleration * Time.deltaTime);
            magnetVelocity = Vector3.Lerp(magnetVelocity, desired, t);

            transform.position += magnetVelocity * Time.deltaTime;
            baseY = transform.position.y;
        }

        private Transform ResolveMagnetTarget()
        {
            if (cachedPlayer == null)
                cachedPlayer = BDTargetFinder.FindPlayer();

            Transform best = cachedPlayer;
            float bestSqr = cachedPlayer != null
                ? (cachedPlayer.position - transform.position).sqrMagnitude
                : float.MaxValue;

            if (allowMountedPickup)
            {
                if (cachedHorse == null)
                    cachedHorse = FindFirstObjectByType<BDHorseController>();

                if (cachedHorse != null && cachedHorse.IsMounted)
                {
                    float horseSqr = (cachedHorse.transform.position - transform.position).sqrMagnitude;
                    if (horseSqr < bestSqr)
                    {
                        bestSqr = horseSqr;
                        best = cachedHorse.transform;
                    }
                }
            }

            if (best == null)
                return null;

            if (bestSqr > magnetRadius * magnetRadius)
                return null;

            return best;
        }

        private void OnTriggerEnter(Collider other)
        {
            TryConsumeFromCollider(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TryConsumeFromCollider(other);
        }

        private bool TryConsumeFromTransform(Transform target)
        {
            if (target == null || consumed)
                return false;

            Collider collider = target.GetComponentInChildren<Collider>();
            if (collider != null)
                return TryConsumeFromCollider(collider);

            BDHealth directHealth = ResolvePlayerHealthFromTransform(target);
            return TryConsumeHealth(directHealth);
        }

        private bool TryConsumeFromCollider(Collider other)
        {
            if (other == null || consumed)
                return false;

            BDHealth playerHealth = ResolvePlayerHealthFromCollider(other);
            return TryConsumeHealth(playerHealth);
        }

        private bool TryConsumeHealth(BDHealth playerHealth)
        {
            if (playerHealth == null || playerHealth.IsDead || consumed)
                return false;

            float amount = playerHealth.MaxHealth * healFractionOfMax;
            playerHealth.Heal(amount);

            consumed = true;
            BDHealingPickupCollectBurst.Spawn(transform.position);

            if (logPickup)
                Debug.Log($"Player healing pickup consumed. Healed {amount:0.0} HP ({healFractionOfMax * 100f:0}% max).");

            Destroy(gameObject);
            return true;
        }

        private BDHealth ResolvePlayerHealthFromCollider(Collider other)
        {
            BDPlayerMarker player = other.GetComponentInParent<BDPlayerMarker>();
            if (player != null)
                return player.GetComponent<BDHealth>();

            if (allowMountedPickup)
            {
                BDHorseController horse = other.GetComponentInParent<BDHorseController>();
                if (horse != null && horse.IsMounted && horse.Rider != null)
                {
                    BDPlayerMarker mountedPlayer = horse.Rider.GetComponent<BDPlayerMarker>();
                    if (mountedPlayer != null)
                        return mountedPlayer.GetComponent<BDHealth>();
                }
            }

            return null;
        }

        private BDHealth ResolvePlayerHealthFromTransform(Transform target)
        {
            if (target == null)
                return null;

            BDPlayerMarker player = target.GetComponentInParent<BDPlayerMarker>();
            if (player != null)
                return player.GetComponent<BDHealth>();

            if (allowMountedPickup)
            {
                BDHorseController horse = target.GetComponentInParent<BDHorseController>();
                if (horse != null && horse.IsMounted && horse.Rider != null)
                {
                    BDPlayerMarker mountedPlayer = horse.Rider.GetComponent<BDPlayerMarker>();
                    if (mountedPlayer != null)
                        return mountedPlayer.GetComponent<BDHealth>();
                }
            }

            return null;
        }
    }
}
