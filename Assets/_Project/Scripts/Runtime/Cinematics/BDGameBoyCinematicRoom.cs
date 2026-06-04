using System.Collections;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDGameBoyCinematicRoom : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private int requiredBatteries = 2;
        [SerializeField] private bool consumeBatteries = false;

        [Header("Scene Points")]
        [SerializeField] private Transform seatPoint;
        [SerializeField] private Transform gameBoyPoint;
        [SerializeField] private Transform screenLightPoint;

        [Header("Timing")]
        [SerializeField] private float walkToSeatDuration = 1.15f;
        [SerializeField] private float sitDuration = 0.55f;
        [SerializeField] private float inspectDuration = 1.05f;
        [SerializeField] private float insertBatteryDuration = 1.15f;
        [SerializeField] private float cartridgeLoadDuration = 0.95f;
        [SerializeField] private float playDuration = 2.15f;

        private bool running;
        private bool completed;
        private BDGameEndingStateController endingStateController;

        public void Configure(Transform seat, Transform gameBoyDisplay, Transform lightPoint, int batteriesRequired)
        {
            seatPoint = seat;
            gameBoyPoint = gameBoyDisplay;
            screenLightPoint = lightPoint;
            requiredBatteries = Mathf.Max(0, batteriesRequired);
        }

        private void Awake()
        {
            endingStateController = GetComponent<BDGameEndingStateController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            TryStart(other.GetComponentInParent<BDPlayerMarker>());
        }

        private void OnTriggerStay(Collider other)
        {
            TryStart(other.GetComponentInParent<BDPlayerMarker>());
        }

        private void TryStart(BDPlayerMarker marker)
        {
            if (running || completed || marker == null)
                return;

            BDGameBoyInventory inventory = marker.GetComponent<BDGameBoyInventory>();
            if (inventory == null)
                inventory = marker.gameObject.AddComponent<BDGameBoyInventory>();

            if (endingStateController == null)
                endingStateController = GetComponent<BDGameEndingStateController>();

            BDGameEndingState state = endingStateController != null
                ? endingStateController.Evaluate(inventory)
                : BDGameEndingStateController.EvaluateInventory(inventory, requiredBatteries);

            // Only consume batteries for powered endings, and only if explicitly enabled.
            if (consumeBatteries && state.IsPowered && !inventory.ConsumeBatteries(requiredBatteries))
                state = BDGameEndingStateController.EvaluateInventory(inventory, requiredBatteries);

            StartCoroutine(RunCinematic(marker.transform, state));
        }

        private IEnumerator RunCinematic(Transform player, BDGameEndingState state)
        {
            running = true;
            completed = true;

            BDPlayerController playerController = player.GetComponent<BDPlayerController>();
            BDPlayerCombat playerCombat = player.GetComponent<BDPlayerCombat>();
            CharacterController characterController = player.GetComponent<CharacterController>();

            bool controllerWasEnabled = playerController != null && playerController.enabled;
            bool combatWasEnabled = playerCombat != null && playerCombat.enabled;
            bool characterWasEnabled = characterController != null && characterController.enabled;

            if (playerController != null) playerController.enabled = false;
            if (playerCombat != null) playerCombat.enabled = false;
            if (characterController != null) characterController.enabled = false;

            Vector3 startPosition = player.position;
            Quaternion startRotation = player.rotation;
            Vector3 seatPosition = seatPoint != null ? seatPoint.position : transform.position;
            Quaternion seatRotation = seatPoint != null ? seatPoint.rotation : player.rotation;

            yield return MovePlayer(player, startPosition, seatPosition, startRotation, seatRotation, walkToSeatDuration);

            Vector3 seatedPosition = seatPosition + Vector3.down * 0.55f;
            yield return MovePlayer(player, seatPosition, seatedPosition, seatRotation, seatRotation, sitDuration);

            switch (state.Variant)
            {
                case BDGameEndingVariant.NoGameBoy:
                    yield return RunNoGameBoyBranch(seatedPosition, seatRotation);
                    break;

                case BDGameEndingVariant.GameBoyNoBatteries:
                    yield return RunGameBoyNoBatteriesBranch(seatedPosition, seatRotation);
                    break;

                case BDGameEndingVariant.PoweredNoCartridge:
                    yield return RunPoweredNoCartridgeBranch(seatedPosition, seatRotation);
                    break;

                case BDGameEndingVariant.PoweredWithCartridge:
                    yield return RunPoweredWithCartridgeBranch(seatedPosition, seatRotation);
                    break;
            }

            yield return MovePlayer(player, seatedPosition, seatPosition, seatRotation, seatRotation, 0.45f);

            if (characterController != null) characterController.enabled = characterWasEnabled;
            if (playerCombat != null) playerCombat.enabled = combatWasEnabled;
            if (playerController != null) playerController.enabled = controllerWasEnabled;

            running = false;
        }

        private IEnumerator RunNoGameBoyBranch(Vector3 seatedPosition, Quaternion seatRotation)
        {
            // Empty ending: the player sits and nothing is pulled out.
            yield return WaitWithDarkPulse(null, 1.45f, 0.22f);
        }

        private IEnumerator RunGameBoyNoBatteriesBranch(Vector3 seatedPosition, Quaternion seatRotation)
        {
            GameObject gameBoy = CreateCinematicGameBoy(GetGameBoyPosition(seatedPosition, seatRotation), seatRotation);

            Renderer screenRenderer = FindChildRenderer(gameBoy.transform, "BD_Cinematic_GameBoy_Screen");
            if (screenRenderer != null)
                screenRenderer.sharedMaterial = CreateMaterial(new Color(0.09f, 0.10f, 0.09f, 1f), Color.black);

            yield return WaitWithDarkPulse(gameBoy, inspectDuration, 0.25f);

            // Put the powerless Game Boy aside.
            Vector3 aside = gameBoy.transform.position + Vector3.right * 0.65f + Vector3.down * 0.24f;
            yield return MoveObject(gameBoy.transform, gameBoy.transform.position, aside, gameBoy.transform.rotation, gameBoy.transform.rotation * Quaternion.Euler(0f, 0f, -28f), 0.55f);
            yield return WaitWithDarkPulse(gameBoy, 0.65f, 0.18f);

            Destroy(gameBoy);
        }

        private IEnumerator RunPoweredNoCartridgeBranch(Vector3 seatedPosition, Quaternion seatRotation)
        {
            GameObject gameBoy = CreateCinematicGameBoy(GetGameBoyPosition(seatedPosition, seatRotation), seatRotation);

            yield return InsertBatteries(gameBoy);
            yield return PowerScreen(gameBoy, new Color(0.74f, 0.76f, 0.76f, 1f), new Color(0.36f, 0.38f, 0.38f, 1f), colorful: false);

            // White/gray light means the device has power, but no game is loaded.
            yield return WaitWithDarkPulse(gameBoy, 1.25f, 0.10f);

            Destroy(gameBoy);
        }

        private IEnumerator RunPoweredWithCartridgeBranch(Vector3 seatedPosition, Quaternion seatRotation)
        {
            GameObject gameBoy = CreateCinematicGameBoy(GetGameBoyPosition(seatedPosition, seatRotation), seatRotation);

            yield return InsertBatteries(gameBoy);

            GameObject cartridge = CreateCinematicCartridge(gameBoy.transform.position + Vector3.up * 0.18f + Vector3.left * 0.52f);
            yield return AnimateCartridgeInsert(cartridge.transform, gameBoy.transform.position + Vector3.up * 0.03f, cartridgeLoadDuration);

            Destroy(cartridge);

            yield return PowerScreen(gameBoy, new Color(0.25f, 1f, 0.42f, 1f), new Color(0.05f, 0.95f, 0.18f, 1f), colorful: true);

            float endTime = Time.time + playDuration;
            Renderer screenRenderer = FindChildRenderer(gameBoy.transform, "BD_Cinematic_GameBoy_Screen");

            while (Time.time < endTime)
            {
                float t = Time.time * 8f;
                if (screenRenderer != null)
                {
                    Color color = Color.HSVToRGB(Mathf.Repeat(t * 0.08f, 1f), 0.68f, 1f);
                    screenRenderer.sharedMaterial = CreateMaterial(color, color * 0.65f);
                }

                if (gameBoy != null)
                    gameBoy.transform.localScale = Vector3.one * (1f + Mathf.Sin(Time.time * 9f) * 0.025f);

                yield return null;
            }

            Destroy(gameBoy);
        }

        private Vector3 GetGameBoyPosition(Vector3 seatedPosition, Quaternion seatRotation)
        {
            return gameBoyPoint != null
                ? gameBoyPoint.position
                : seatedPosition + (seatRotation * Vector3.forward) * 0.95f + Vector3.up * 0.85f;
        }

        private IEnumerator InsertBatteries(GameObject gameBoy)
        {
            GameObject batteryA = CreateCinematicBattery(gameBoy.transform.position + Vector3.left * 0.55f + Vector3.up * 0.20f);
            GameObject batteryB = CreateCinematicBattery(gameBoy.transform.position + Vector3.right * 0.55f + Vector3.up * 0.20f);

            yield return AnimateBatteryInsert(batteryA.transform, gameBoy.transform.position + Vector3.left * 0.13f, insertBatteryDuration * 0.48f);
            yield return AnimateBatteryInsert(batteryB.transform, gameBoy.transform.position + Vector3.right * 0.13f, insertBatteryDuration * 0.48f);

            Destroy(batteryA);
            Destroy(batteryB);
        }

        private IEnumerator PowerScreen(GameObject gameBoy, Color screenColor, Color emissionColor, bool colorful)
        {
            Light screenLight = CreateScreenLight(screenLightPoint != null ? screenLightPoint.position : gameBoy.transform.position + Vector3.up * 0.18f);
            screenLight.color = colorful ? new Color(0.35f, 1f, 0.45f, 1f) : new Color(0.78f, 0.80f, 0.82f, 1f);

            Renderer screenRenderer = FindChildRenderer(gameBoy.transform, "BD_Cinematic_GameBoy_Screen");
            if (screenRenderer != null)
                screenRenderer.sharedMaterial = CreateMaterial(screenColor, emissionColor);

            float end = Time.time + 0.75f;
            while (Time.time < end)
            {
                if (screenLight != null)
                {
                    float pulse = 1.2f + Mathf.Sin(Time.time * (colorful ? 11f : 5f)) * (colorful ? 0.45f : 0.12f);
                    screenLight.intensity = pulse;
                }

                yield return null;
            }

            if (screenLight != null)
                Destroy(screenLight.gameObject);
        }

        private IEnumerator WaitWithDarkPulse(GameObject objectToPulse, float duration, float scalePulse)
        {
            float end = Time.time + Mathf.Max(0.01f, duration);

            while (Time.time < end)
            {
                if (objectToPulse != null)
                    objectToPulse.transform.localScale = Vector3.one * (1f + Mathf.Sin(Time.time * 4f) * scalePulse);

                yield return null;
            }
        }

        private IEnumerator MovePlayer(Transform player, Vector3 from, Vector3 to, Quaternion fromRotation, Quaternion toRotation, float duration)
        {
            duration = Mathf.Max(0.01f, duration);
            float start = Time.time;

            while (Time.time - start < duration)
            {
                float t = Mathf.Clamp01((Time.time - start) / duration);
                float eased = t * t * (3f - 2f * t);

                player.position = Vector3.Lerp(from, to, eased);
                player.rotation = Quaternion.Slerp(fromRotation, toRotation, eased);
                yield return null;
            }

            player.position = to;
            player.rotation = toRotation;
        }

        private IEnumerator MoveObject(Transform target, Vector3 from, Vector3 to, Quaternion fromRotation, Quaternion toRotation, float duration)
        {
            if (target == null)
                yield break;

            duration = Mathf.Max(0.01f, duration);
            float start = Time.time;

            while (Time.time - start < duration)
            {
                float t = Mathf.Clamp01((Time.time - start) / duration);
                float eased = t * t * (3f - 2f * t);

                target.position = Vector3.Lerp(from, to, eased);
                target.rotation = Quaternion.Slerp(fromRotation, toRotation, eased);
                yield return null;
            }

            target.position = to;
            target.rotation = toRotation;
        }

        private IEnumerator AnimateBatteryInsert(Transform battery, Vector3 target, float duration)
        {
            if (battery == null)
                yield break;

            Vector3 start = battery.position;
            float begin = Time.time;
            duration = Mathf.Max(0.01f, duration);

            while (Time.time - begin < duration)
            {
                float t = Mathf.Clamp01((Time.time - begin) / duration);
                float eased = t * t * (3f - 2f * t);
                battery.position = Vector3.Lerp(start, target, eased);
                battery.Rotate(Vector3.forward, 360f * Time.deltaTime, Space.Self);
                yield return null;
            }

            battery.position = target;
        }

        private IEnumerator AnimateCartridgeInsert(Transform cartridge, Vector3 target, float duration)
        {
            if (cartridge == null)
                yield break;

            Vector3 start = cartridge.position;
            Quaternion startRotation = cartridge.rotation;
            Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, 8f);
            float begin = Time.time;
            duration = Mathf.Max(0.01f, duration);

            while (Time.time - begin < duration)
            {
                float t = Mathf.Clamp01((Time.time - begin) / duration);
                float eased = t * t * (3f - 2f * t);

                cartridge.position = Vector3.Lerp(start, target, eased);
                cartridge.rotation = Quaternion.Slerp(startRotation, endRotation, eased);
                yield return null;
            }

            cartridge.position = target;
            cartridge.rotation = endRotation;
        }

        private static GameObject CreateCinematicGameBoy(Vector3 position, Quaternion rotation)
        {
            GameObject root = new GameObject("BD_Cinematic_GameBoy_In_Hands");
            root.transform.position = position;
            root.transform.rotation = rotation * Quaternion.Euler(65f, 0f, 0f);
            root.transform.localScale = Vector3.one;

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "BD_Cinematic_GameBoy_Body";
            body.transform.SetParent(root.transform, worldPositionStays: false);
            body.transform.localPosition = Vector3.zero;
            body.transform.localScale = new Vector3(0.62f, 0.85f, 0.10f);
            body.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.16f, 0.18f, 0.16f, 1f), Color.black);
            DestroyCollider(body);

            GameObject screen = GameObject.CreatePrimitive(PrimitiveType.Cube);
            screen.name = "BD_Cinematic_GameBoy_Screen";
            screen.transform.SetParent(root.transform, worldPositionStays: false);
            screen.transform.localPosition = new Vector3(0f, 0.15f, -0.058f);
            screen.transform.localScale = new Vector3(0.42f, 0.25f, 0.015f);
            screen.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.10f, 0.22f, 0.10f, 1f), Color.black);
            DestroyCollider(screen);

            return root;
        }

        private static GameObject CreateCinematicBattery(Vector3 position)
        {
            GameObject battery = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            battery.name = "BD_Cinematic_Battery";
            battery.transform.position = position;
            battery.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            battery.transform.localScale = new Vector3(0.08f, 0.22f, 0.08f);
            battery.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.95f, 0.78f, 0.18f, 1f), new Color(0.20f, 0.13f, 0.02f, 1f));
            DestroyCollider(battery);
            return battery;
        }

        private static GameObject CreateCinematicCartridge(Vector3 position)
        {
            GameObject cartridge = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cartridge.name = "BD_Cinematic_GameCartridge";
            cartridge.transform.position = position;
            cartridge.transform.localScale = new Vector3(0.36f, 0.48f, 0.07f);
            cartridge.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.12f, 0.11f, 0.16f, 1f), new Color(0.16f, 0.10f, 0.32f, 1f));
            DestroyCollider(cartridge);

            GameObject label = GameObject.CreatePrimitive(PrimitiveType.Cube);
            label.name = "BD_Cinematic_GameCartridge_Label";
            label.transform.SetParent(cartridge.transform, worldPositionStays: false);
            label.transform.localPosition = new Vector3(0f, 0.06f, -0.038f);
            label.transform.localScale = new Vector3(0.78f, 0.42f, 0.10f);
            label.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.45f, 0.25f, 1f, 1f), new Color(0.20f, 0.08f, 0.55f, 1f));
            DestroyCollider(label);

            return cartridge;
        }

        private static Light CreateScreenLight(Vector3 position)
        {
            GameObject lightObject = new GameObject("BD_Cinematic_GameBoy_Screen_Light");
            lightObject.transform.position = position;

            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(0.35f, 1f, 0.45f, 1f);
            light.intensity = 2.0f;
            light.range = 4.0f;
            return light;
        }

        private static Renderer FindChildRenderer(Transform root, string childName)
        {
            if (root == null)
                return null;

            Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].name == childName)
                    return renderers[i];
            }

            return null;
        }

        private static void DestroyCollider(GameObject go)
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
    }
}
