using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDGameBoyCollectible : MonoBehaviour
    {
        public enum CollectibleKind
        {
            GameBoy,
            Battery,
            GameCartridge
        }

        [Header("Collectible")]
        [SerializeField] private CollectibleKind kind = CollectibleKind.Battery;
        [SerializeField] private float collectRadius = 1.35f;
        [SerializeField] private float magnetRadius = 3.75f;
        [SerializeField] private float magnetSpeed = 7.5f;

        [Header("Visual")]
        [SerializeField] private float floatAmplitude = 0.12f;
        [SerializeField] private float floatFrequency = 2.4f;
        [SerializeField] private float spinDegreesPerSecond = 80f;

        private bool collected;
        private Transform player;
        private Vector3 basePosition;
        private float phase;

        public CollectibleKind Kind => kind;

        public static BDGameBoyCollectible SpawnGameBoy(Vector3 position)
        {
            return Spawn(position, CollectibleKind.GameBoy);
        }

        public static BDGameBoyCollectible SpawnBattery(Vector3 position)
        {
            return Spawn(position, CollectibleKind.Battery);
        }

        public static BDGameBoyCollectible SpawnGameCartridge(Vector3 position)
        {
            return Spawn(position, CollectibleKind.GameCartridge);
        }

        public static BDGameBoyCollectible Spawn(Vector3 position, CollectibleKind kind)
        {
            GameObject root = new GameObject(GetCollectibleObjectName(kind));
            root.transform.position = position + Vector3.up * 0.65f;

            BDGameBoyCollectible collectible = root.AddComponent<BDGameBoyCollectible>();
            collectible.kind = kind;
            collectible.BuildVisual();

            SphereCollider trigger = root.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 0.75f;

            return collectible;
        }

        private void Awake()
        {
            basePosition = transform.position;
            phase = Random.Range(0f, 10f);
            if (transform.childCount == 0)
                BuildVisual();
        }

        private void Update()
        {
            if (collected)
                return;

            ResolvePlayer();
            AnimateIdle();
            MagnetAndCollect();
        }

        private void ResolvePlayer()
        {
            if (player != null)
                return;

            player = BDTargetFinder.FindPlayer();
        }

        private void AnimateIdle()
        {
            float bob = Mathf.Sin((Time.time + phase) * floatFrequency) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, basePosition.y + bob, transform.position.z);
            transform.Rotate(Vector3.up, spinDegreesPerSecond * Time.deltaTime, Space.World);
        }

        private void MagnetAndCollect()
        {
            if (player == null)
                return;

            Vector3 toPlayer = player.position + Vector3.up * 0.8f - transform.position;
            float distance = toPlayer.magnitude;

            if (distance <= collectRadius)
            {
                Collect(player);
                return;
            }

            if (distance <= magnetRadius && distance > 0.001f)
            {
                transform.position += toPlayer.normalized * (magnetSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (collected)
                return;

            BDPlayerMarker marker = other.GetComponentInParent<BDPlayerMarker>();
            if (marker == null)
                return;

            Collect(marker.transform);
        }

        private void Collect(Transform playerTransform)
        {
            if (collected)
                return;

            BDGameBoyInventory inventory = playerTransform.GetComponent<BDGameBoyInventory>();
            if (inventory == null)
                inventory = playerTransform.gameObject.AddComponent<BDGameBoyInventory>();

            if (kind == CollectibleKind.GameBoy)
                inventory.CollectGameBoy();
            else if (kind == CollectibleKind.GameCartridge)
                inventory.CollectGameCartridge();
            else
                inventory.CollectBattery(1);

            SpawnCollectBurst(transform.position, kind);
            collected = true;
            Destroy(gameObject);
        }

        private void BuildVisual()
        {
            if (kind == CollectibleKind.GameBoy)
                BuildGameBoyVisual();
            else if (kind == CollectibleKind.GameCartridge)
                BuildGameCartridgeVisual();
            else
                BuildBatteryVisual();
        }

        private void BuildGameBoyVisual()
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "BD_GameBoy_Body";
            body.transform.SetParent(transform, worldPositionStays: false);
            body.transform.localPosition = Vector3.zero;
            body.transform.localScale = new Vector3(0.62f, 0.85f, 0.13f);
            body.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.18f, 0.20f, 0.18f, 1f), new Color(0.02f, 0.08f, 0.04f, 1f));
            MakeTriggerless(body);

            GameObject screen = GameObject.CreatePrimitive(PrimitiveType.Cube);
            screen.name = "BD_GameBoy_Green_Screen";
            screen.transform.SetParent(transform, worldPositionStays: false);
            screen.transform.localPosition = new Vector3(0f, 0.15f, -0.071f);
            screen.transform.localScale = new Vector3(0.40f, 0.25f, 0.018f);
            screen.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.30f, 0.85f, 0.38f, 1f), new Color(0.05f, 0.45f, 0.10f, 1f));
            MakeTriggerless(screen);

            GameObject buttonA = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            buttonA.name = "BD_GameBoy_Button_A";
            buttonA.transform.SetParent(transform, worldPositionStays: false);
            buttonA.transform.localPosition = new Vector3(0.18f, -0.19f, -0.075f);
            buttonA.transform.localScale = Vector3.one * 0.10f;
            buttonA.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.08f, 0.08f, 0.09f, 1f), Color.black);
            MakeTriggerless(buttonA);

            GameObject buttonB = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            buttonB.name = "BD_GameBoy_Button_B";
            buttonB.transform.SetParent(transform, worldPositionStays: false);
            buttonB.transform.localPosition = new Vector3(0.31f, -0.10f, -0.075f);
            buttonB.transform.localScale = Vector3.one * 0.085f;
            buttonB.GetComponent<Renderer>().sharedMaterial = buttonA.GetComponent<Renderer>().sharedMaterial;
            MakeTriggerless(buttonB);
        }

        private void BuildGameCartridgeVisual()
        {
            GameObject shell = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shell.name = "BD_GameCartridge_Shell";
            shell.transform.SetParent(transform, worldPositionStays: false);
            shell.transform.localPosition = Vector3.zero;
            shell.transform.localScale = new Vector3(0.55f, 0.72f, 0.10f);
            shell.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.075f, 0.078f, 0.088f, 1f), new Color(0.01f, 0.01f, 0.02f, 1f));
            MakeTriggerless(shell);

            GameObject label = GameObject.CreatePrimitive(PrimitiveType.Cube);
            label.name = "BD_GameCartridge_Label";
            label.transform.SetParent(transform, worldPositionStays: false);
            label.transform.localPosition = new Vector3(0f, 0.10f, -0.057f);
            label.transform.localScale = new Vector3(0.42f, 0.30f, 0.014f);
            label.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.22f, 0.35f, 0.88f, 1f), new Color(0.03f, 0.07f, 0.25f, 1f));
            MakeTriggerless(label);

            GameObject stripeA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stripeA.name = "BD_GameCartridge_Color_Stripe_A";
            stripeA.transform.SetParent(transform, worldPositionStays: false);
            stripeA.transform.localPosition = new Vector3(-0.11f, 0.24f, -0.066f);
            stripeA.transform.localScale = new Vector3(0.07f, 0.16f, 0.012f);
            stripeA.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(1f, 0.18f, 0.20f, 1f), new Color(0.45f, 0.02f, 0.02f, 1f));
            MakeTriggerless(stripeA);

            GameObject stripeB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stripeB.name = "BD_GameCartridge_Color_Stripe_B";
            stripeB.transform.SetParent(transform, worldPositionStays: false);
            stripeB.transform.localPosition = new Vector3(0f, 0.24f, -0.067f);
            stripeB.transform.localScale = new Vector3(0.07f, 0.16f, 0.012f);
            stripeB.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.22f, 1f, 0.35f, 1f), new Color(0.02f, 0.40f, 0.05f, 1f));
            MakeTriggerless(stripeB);

            GameObject stripeC = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stripeC.name = "BD_GameCartridge_Color_Stripe_C";
            stripeC.transform.SetParent(transform, worldPositionStays: false);
            stripeC.transform.localPosition = new Vector3(0.11f, 0.24f, -0.068f);
            stripeC.transform.localScale = new Vector3(0.07f, 0.16f, 0.012f);
            stripeC.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(1f, 0.82f, 0.20f, 1f), new Color(0.40f, 0.22f, 0.02f, 1f));
            MakeTriggerless(stripeC);

            GameObject contacts = GameObject.CreatePrimitive(PrimitiveType.Cube);
            contacts.name = "BD_GameCartridge_Contacts";
            contacts.transform.SetParent(transform, worldPositionStays: false);
            contacts.transform.localPosition = new Vector3(0f, -0.33f, -0.062f);
            contacts.transform.localScale = new Vector3(0.34f, 0.08f, 0.014f);
            contacts.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.88f, 0.68f, 0.22f, 1f), new Color(0.26f, 0.16f, 0.03f, 1f));
            MakeTriggerless(contacts);
        }

        private void BuildBatteryVisual()
        {
            GameObject battery = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            battery.name = "BD_Battery_Body";
            battery.transform.SetParent(transform, worldPositionStays: false);
            battery.transform.localPosition = Vector3.zero;
            battery.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            battery.transform.localScale = new Vector3(0.16f, 0.34f, 0.16f);
            battery.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.95f, 0.78f, 0.18f, 1f), new Color(0.25f, 0.16f, 0.02f, 1f));
            MakeTriggerless(battery);

            GameObject tip = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tip.name = "BD_Battery_Tip";
            tip.transform.SetParent(transform, worldPositionStays: false);
            tip.transform.localPosition = new Vector3(0f, 0f, -0.36f);
            tip.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            tip.transform.localScale = new Vector3(0.12f, 0.035f, 0.12f);
            tip.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.82f, 0.82f, 0.78f, 1f), Color.black);
            MakeTriggerless(tip);
        }

        private static void SpawnCollectBurst(Vector3 position, CollectibleKind kind)
        {
            GameObject burst = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            burst.name = GetCollectBurstObjectName(kind);
            burst.transform.position = position;
            burst.transform.localScale = Vector3.one * 0.28f;

            Collider collider = burst.GetComponent<Collider>();
            if (collider != null)
                DestroyUnityObjectSafely(collider);

            Renderer renderer = burst.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = CreateTransparentMaterial(GetCollectBurstColor(kind));

            burst.AddComponent<BDTimedDestroy>().Initialize(0.28f, true);
        }

        private static string GetCollectibleObjectName(CollectibleKind kind)
        {
            switch (kind)
            {
                case CollectibleKind.GameBoy:
                    return "BD_Collectible_GameBoy";

                case CollectibleKind.GameCartridge:
                    return "BD_Collectible_GameCartridge";

                default:
                    return "BD_Collectible_Battery";
            }
        }

        private static string GetCollectBurstObjectName(CollectibleKind kind)
        {
            switch (kind)
            {
                case CollectibleKind.GameBoy:
                    return "BD_GameBoy_Collect_Burst";

                case CollectibleKind.GameCartridge:
                    return "BD_GameCartridge_Collect_Burst";

                default:
                    return "BD_Battery_Collect_Burst";
            }
        }

        private static Color GetCollectBurstColor(CollectibleKind kind)
        {
            switch (kind)
            {
                case CollectibleKind.GameBoy:
                    return new Color(0.30f, 1f, 0.45f, 0.65f);

                case CollectibleKind.GameCartridge:
                    return new Color(0.55f, 0.40f, 1f, 0.70f);

                default:
                    return new Color(1f, 0.85f, 0.20f, 0.65f);
            }
        }

        private static void MakeTriggerless(GameObject go)
        {
            Collider collider = go.GetComponent<Collider>();
            if (collider != null)
                DestroyUnityObjectSafely(collider);
        }

        private static void DestroyUnityObjectSafely(UnityEngine.Object obj)
        {
            if (obj == null)
                return;

            if (Application.isPlaying)
                Destroy(obj);
            else
                DestroyImmediate(obj);
        }

        private static Material CreateMaterial(Color color, Color emission)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Sprites/Default");

            Material material = new Material(shader);
            material.color = color;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", emission);
            }

            return material;
        }

        private static Material CreateTransparentMaterial(Color color)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Sprites/Default");

            Material material = new Material(shader);
            material.color = color;

            if (material.HasProperty("_Mode"))
                material.SetFloat("_Mode", 3f);

            if (material.HasProperty("_SrcBlend"))
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);

            if (material.HasProperty("_DstBlend"))
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            if (material.HasProperty("_ZWrite"))
                material.SetInt("_ZWrite", 0);

            material.renderQueue = 3000;
            return material;
        }
    }
}
