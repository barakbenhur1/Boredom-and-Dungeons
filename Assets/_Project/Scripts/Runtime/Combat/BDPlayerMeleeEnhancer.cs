using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDPlayerCombat))]
    [RequireComponent(typeof(BDPlayerMarker))]
    public sealed class BDPlayerMeleeEnhancer : MonoBehaviour
    {
        private enum BufferedAttack
        {
            None = 0,
            Light = 1,
            Heavy = 2
        }

        [Header("Attack Buffer")]
        [SerializeField] private float attackBufferDuration = 0.24f;

        [Header("Landing Attack")]
        [SerializeField] private float landingDamageMultiplier = 1.20f;
        [SerializeField] private bool spawnLandingAttackVisual = true;

        private BDPlayerCombat combat;
        private BDPlayerAirStateTracker airState;
        private BDPlayerParryState parryState;

        private FieldInfo lightDamageField;
        private FieldInfo heavyDamageField;
        private FieldInfo lightCooldownField;
        private FieldInfo heavyCooldownField;
        private FieldInfo nextLightAllowedAtField;
        private FieldInfo nextHeavyAllowedAtField;
        private MethodInfo tryMeleeAttackMethod;

        private BufferedAttack bufferedAttack;
        private float bufferedUntilUnscaled;
        private bool restoreLightDamage;
        private bool restoreHeavyDamage;
        private float originalLightDamage;
        private float originalHeavyDamage;
        private bool reflectionReady;

        private void Awake()
        {
            combat = GetComponent<BDPlayerCombat>();
            airState = GetComponent<BDPlayerAirStateTracker>();
            if (airState == null)
                airState = gameObject.AddComponent<BDPlayerAirStateTracker>();

            parryState = GetComponent<BDPlayerParryState>();
            if (parryState == null)
                parryState = gameObject.AddComponent<BDPlayerParryState>();

            CacheCombatReflection();
        }

        private void Update()
        {
            if (!reflectionReady || combat == null || !combat.enabled)
                return;

            if (IsMountedOnHorse())
            {
                ClearBuffer();
                return;
            }

            bool lightPressed = ReadLightAttackPressed();
            bool heavyPressed = ReadHeavyAttackPressed();

            if (lightPressed)
                HandlePressedAttack(BufferedAttack.Light);

            if (heavyPressed)
                HandlePressedAttack(BufferedAttack.Heavy);

            TryExecuteBufferedAttack();
        }

        private void LateUpdate()
        {
            RestoreTemporarilyModifiedDamage();
        }

        private void HandlePressedAttack(BufferedAttack attack)
        {
            float nextAllowedAt = GetNextAllowedAt(attack);

            if (Time.time >= nextAllowedAt)
            {
                PrepareImmediateAttack(attack);
                return;
            }

            // Keep only the most recently requested melee attack.
            // This makes rapid input feel responsive without creating an unlimited queue.
            bufferedAttack = attack;
            bufferedUntilUnscaled = Time.unscaledTime + Mathf.Max(0.05f, attackBufferDuration);
        }

        private void PrepareImmediateAttack(BufferedAttack attack)
        {
            bool heavy = attack == BufferedAttack.Heavy;
            bool landing = airState != null && airState.IsDescendingFromJump;

            parryState.RecordMeleeAttack(heavy);

            if (!landing)
                return;

            ApplyTemporaryLandingDamage(attack);

            if (spawnLandingAttackVisual)
                BDLandingAttackVisual.Spawn(transform.position, ResolveAimDirection(), heavy);
        }

        private void TryExecuteBufferedAttack()
        {
            if (bufferedAttack == BufferedAttack.None)
                return;

            if (Time.unscaledTime > bufferedUntilUnscaled)
            {
                ClearBuffer();
                return;
            }

            if (Time.time < GetNextAllowedAt(bufferedAttack))
                return;

            BufferedAttack attack = bufferedAttack;
            ClearBuffer();
            ExecuteBufferedAttack(attack);
        }

        private void ExecuteBufferedAttack(BufferedAttack attack)
        {
            if (!reflectionReady || tryMeleeAttackMethod == null)
                return;

            bool heavy = attack == BufferedAttack.Heavy;
            bool landing = airState != null && airState.IsDescendingFromJump;
            float baseDamage = GetFloatField(heavy ? heavyDamageField : lightDamageField);
            float cooldown = GetFloatField(heavy ? heavyCooldownField : lightCooldownField);
            FieldInfo nextAllowedField = heavy ? nextHeavyAllowedAtField : nextLightAllowedAtField;
            float nextAllowedAt = GetFloatField(nextAllowedField);
            float finalDamage = landing
                ? baseDamage * Mathf.Max(1f, landingDamageMultiplier)
                : baseDamage;

            parryState.RecordMeleeAttack(heavy);

            if (landing && spawnLandingAttackVisual)
                BDLandingAttackVisual.Spawn(transform.position, ResolveAimDirection(), heavy);

            object[] arguments =
            {
                finalDamage,
                cooldown,
                nextAllowedAt,
                heavy ? "heavy" : "light"
            };

            try
            {
                tryMeleeAttackMethod.Invoke(combat, arguments);

                if (arguments[2] is float updatedNextAllowedAt)
                    nextAllowedField.SetValue(combat, updatedNextAllowedAt);
            }
            catch (TargetInvocationException exception)
            {
                Debug.LogException(exception.InnerException ?? exception, combat);
            }
        }

        private void ApplyTemporaryLandingDamage(BufferedAttack attack)
        {
            float multiplier = Mathf.Max(1f, landingDamageMultiplier);

            if (attack == BufferedAttack.Heavy)
            {
                if (restoreHeavyDamage)
                    return;

                originalHeavyDamage = GetFloatField(heavyDamageField);
                heavyDamageField.SetValue(combat, originalHeavyDamage * multiplier);
                restoreHeavyDamage = true;
            }
            else
            {
                if (restoreLightDamage)
                    return;

                originalLightDamage = GetFloatField(lightDamageField);
                lightDamageField.SetValue(combat, originalLightDamage * multiplier);
                restoreLightDamage = true;
            }
        }

        private void RestoreTemporarilyModifiedDamage()
        {
            if (combat == null)
                return;

            if (restoreLightDamage)
            {
                lightDamageField.SetValue(combat, originalLightDamage);
                restoreLightDamage = false;
            }

            if (restoreHeavyDamage)
            {
                heavyDamageField.SetValue(combat, originalHeavyDamage);
                restoreHeavyDamage = false;
            }
        }

        private void CacheCombatReflection()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = typeof(BDPlayerCombat);

            lightDamageField = type.GetField("lightDamage", flags);
            heavyDamageField = type.GetField("heavyDamage", flags);
            lightCooldownField = type.GetField("lightCooldown", flags);
            heavyCooldownField = type.GetField("heavyCooldown", flags);
            nextLightAllowedAtField = type.GetField("nextLightAllowedAt", flags);
            nextHeavyAllowedAtField = type.GetField("nextHeavyAllowedAt", flags);
            tryMeleeAttackMethod = type.GetMethod("TryMeleeAttack", flags);

            reflectionReady =
                lightDamageField != null &&
                heavyDamageField != null &&
                lightCooldownField != null &&
                heavyCooldownField != null &&
                nextLightAllowedAtField != null &&
                nextHeavyAllowedAtField != null &&
                tryMeleeAttackMethod != null;

            if (!reflectionReady)
                Debug.LogError("BDPlayerMeleeEnhancer could not bind to BDPlayerCombat. Melee enhancement is disabled.", this);
        }

        private float GetNextAllowedAt(BufferedAttack attack)
        {
            return GetFloatField(attack == BufferedAttack.Heavy
                ? nextHeavyAllowedAtField
                : nextLightAllowedAtField);
        }

        private float GetFloatField(FieldInfo field)
        {
            if (field == null || combat == null)
                return 0f;

            object value = field.GetValue(combat);
            return value is float number ? number : 0f;
        }

        private Vector3 ResolveAimDirection()
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                Vector2 pointerPosition;

#if ENABLE_INPUT_SYSTEM
                Mouse mouse = Mouse.current;
                pointerPosition = mouse != null
                    ? mouse.position.ReadValue()
                    : new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
#else
                pointerPosition = Input.mousePosition;
#endif

                Ray ray = camera.ScreenPointToRay(pointerPosition);
                Plane plane = new Plane(Vector3.up, transform.position);

                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 direction = ray.GetPoint(distance) - transform.position;
                    direction.y = 0f;

                    if (direction.sqrMagnitude > 0.001f)
                        return direction.normalized;
                }
            }

            Vector3 forward = transform.forward;
            forward.y = 0f;
            return forward.sqrMagnitude > 0.001f ? forward.normalized : Vector3.forward;
        }

        private bool IsMountedOnHorse()
        {
            BDHorseController horse = FindFirstObjectByType<BDHorseController>();
            if (horse == null || !horse.IsMounted || horse.Rider == null)
                return false;

            Transform rider = horse.Rider;
            return rider == transform || transform.IsChildOf(rider) || rider.IsChildOf(transform);
        }

        private void ClearBuffer()
        {
            bufferedAttack = BufferedAttack.None;
            bufferedUntilUnscaled = 0f;
        }

        private static bool ReadLightAttackPressed()
        {
#if ENABLE_INPUT_SYSTEM
            Mouse mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame)
                return true;

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.jKey.wasPressedThisFrame)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
                return true;
#endif

            return false;
        }

        private static bool ReadHeavyAttackPressed()
        {
#if ENABLE_INPUT_SYSTEM
            Mouse mouse = Mouse.current;
            if (mouse != null && mouse.rightButton.wasPressedThisFrame)
                return true;

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.kKey.wasPressedThisFrame)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K))
                return true;
#endif

            return false;
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDLandingAttackVisual : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.30f;
        [SerializeField] private float startHeight = 2.8f;
        [SerializeField] private float impactRadius = 1.15f;

        private Transform blade;
        private Transform ring;
        private float elapsed;
        private Vector3 direction;
        private bool heavy;

        public static void Spawn(Vector3 position, Vector3 attackDirection, bool heavyAttack)
        {
            GameObject root = new GameObject(heavyAttack
                ? "BD_Heavy_Landing_Attack_Visual"
                : "BD_Light_Landing_Attack_Visual");
            root.transform.position = position + Vector3.up * 0.08f;

            BDLandingAttackVisual visual = root.AddComponent<BDLandingAttackVisual>();
            visual.direction = attackDirection.sqrMagnitude > 0.001f
                ? attackDirection.normalized
                : Vector3.forward;
            visual.heavy = heavyAttack;
            visual.Build();
        }

        private void Build()
        {
            Color color = heavy
                ? new Color(1f, 0.48f, 0.10f, 1f)
                : new Color(0.35f, 0.92f, 1f, 1f);

            GameObject bladeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bladeObject.name = "BD_Landing_Downward_Strike";
            blade = bladeObject.transform;
            blade.SetParent(transform, false);
            blade.localPosition = Vector3.up * startHeight + direction * 0.45f;
            blade.localRotation = Quaternion.LookRotation(Vector3.down + direction * 0.18f, direction);
            blade.localScale = heavy
                ? new Vector3(0.22f, 0.22f, 2.4f)
                : new Vector3(0.15f, 0.15f, 1.8f);
            ConfigureVisualObject(bladeObject, color);

            GameObject ringObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ringObject.name = "BD_Landing_Impact_Ring";
            ring = ringObject.transform;
            ring.SetParent(transform, false);
            ring.localPosition = direction * 0.65f;
            ring.localScale = new Vector3(0.05f, 0.018f, 0.05f);
            ConfigureVisualObject(ringObject, color);
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, lifetime));

            if (blade != null)
            {
                Vector3 end = direction * 0.72f + Vector3.up * 0.12f;
                blade.localPosition = Vector3.Lerp(Vector3.up * startHeight + direction * 0.45f, end, t);
                blade.localScale *= 1f - Time.unscaledDeltaTime * 2.4f;
            }

            if (ring != null)
            {
                float radius = Mathf.Lerp(0.05f, impactRadius * (heavy ? 1.25f : 1f), t);
                ring.localScale = new Vector3(radius, 0.018f, radius);
            }

            if (elapsed >= lifetime)
                Destroy(gameObject);
        }

        private static void ConfigureVisualObject(GameObject visual, Color color)
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
    }

    public static class BDPlayerMeleeEnhancerInstaller
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallAfterSceneLoad()
        {
            Install();
        }

        private static void Install()
        {
            BDPlayerCombat[] players = UnityEngine.Object.FindObjectsByType<BDPlayerCombat>(FindObjectsSortMode.None);
            for (int i = 0; i < players.Length; i++)
            {
                BDPlayerCombat combat = players[i];
                if (combat != null && combat.GetComponent<BDPlayerMeleeEnhancer>() == null)
                    combat.gameObject.AddComponent<BDPlayerMeleeEnhancer>();
            }
        }
    }
}
