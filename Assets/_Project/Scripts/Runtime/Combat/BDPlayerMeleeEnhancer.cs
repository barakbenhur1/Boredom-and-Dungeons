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
        private BDPlayerAirborneAttackAnimation airborneAnimation;
        private BDPlayerController playerController;

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
        private int lastCommittedPresentationFrame = -1;
        private bool lastCommittedPresentationWasHeavy;
        private bool lastCommittedPresentationWasAirborne;

        private void Awake()
        {
            combat = GetComponent<BDPlayerCombat>();
            airState = GetComponent<BDPlayerAirStateTracker>();
            if (airState == null)
                airState = gameObject.AddComponent<BDPlayerAirStateTracker>();

            playerController = GetComponent<BDPlayerController>();

            parryState = GetComponent<BDPlayerParryState>();
            if (parryState == null)
                parryState = gameObject.AddComponent<BDPlayerParryState>();

            airborneAnimation =
                GetComponent<BDPlayerAirborneAttackAnimation>();
            if (airborneAnimation == null)
                airborneAnimation =
                    gameObject.AddComponent<BDPlayerAirborneAttackAnimation>();

            CacheCombatReflection();
        }

        private void Update()
        {
            if (BDMountedRunIntro.IsGameplayInputLocked)
            {
                ClearBuffer();
                return;
            }

            if (BDNewRunFeedbackReset.IsCombatInputSuppressed)
            {
                ClearBuffer();
                return;
            }

            if (!reflectionReady || combat == null || !combat.enabled)
                return;

            if (IsMountedOnHorse())
            {
                ClearBuffer();
                return;
            }

            bool lightPressed = ReadLightAttackPressed();
            bool heavyPressed = ReadHeavyAttackPressed();

            if (lightPressed && !combat.UsesLightHoldInput)
                HandlePressedAttack(BufferedAttack.Light);

            if (heavyPressed && !combat.UsesHeavyHoldInput)
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

        public void PrepareCommittedAttack(bool heavy)
        {
            float damage = GetFloatField(
                heavy ? heavyDamageField : lightDamageField
            );
            PrepareCommittedAttackDamage(heavy, damage);
        }

        // BD COMMITTED AIRBORNE ATTACK PRESENTATION V23R11
        // Runtime names intentionally remain stable for QA and capture:
        // BD_Heavy_Vertical_Airborne_Slash / BD_Light_Vertical_Airborne_Slash.
        public float PrepareCommittedAttackDamage(
            bool heavy,
            float requestedDamage)
        {
            return PrepareCommittedAttackDamage(
                heavy,
                requestedDamage,
                out _
            );
        }

        // BD EXPLICIT AIRBORNE ATTACK PRESENTATION BRANCH V23R19
        // The real committed attack returns one explicit presentation identity.
        // BDPlayerCombat then spawns either the vertical airborne slash or the
        // grounded slash, never both and never via a timing-based suppression race.
        public float PrepareCommittedAttackDamage(
            bool heavy,
            float requestedDamage,
            out bool airbornePresentation)
        {
            airbornePresentation = false;

            if (!reflectionReady || combat == null || !combat.enabled)
                return requestedDamage;

            airbornePresentation =
                (airState != null && airState.IsAirborneFromJump) ||
                (playerController != null &&
                 playerController.IsAirborneFromControlledJump);
            bool landingDamage =
                airbornePresentation &&
                airState != null &&
                airState.IsDescendingFromJump;

            float resolvedDamage = landingDamage
                ? requestedDamage * Mathf.Max(1f, landingDamageMultiplier)
                : requestedDamage;

            bool alreadyPreparedThisFrame =
                lastCommittedPresentationFrame == Time.frameCount &&
                lastCommittedPresentationWasHeavy == heavy;

            if (alreadyPreparedThisFrame)
            {
                airbornePresentation =
                    lastCommittedPresentationWasAirborne;
                return resolvedDamage;
            }

            lastCommittedPresentationFrame = Time.frameCount;
            lastCommittedPresentationWasHeavy = heavy;
            lastCommittedPresentationWasAirborne = airbornePresentation;
            parryState.RecordMeleeAttack(heavy);

            if (airbornePresentation)
                airborneAnimation?.Play(heavy);

            return resolvedDamage;
        }

        public bool ShouldSpawnAirborneSlashVisual =>
            spawnLandingAttackVisual;

        private void PrepareImmediateAttack(BufferedAttack attack)
        {
            bool heavy = attack == BufferedAttack.Heavy;
            float damage = GetFloatField(
                heavy ? heavyDamageField : lightDamageField
            );

            // BD VERTICAL AIRBORNE MELEE VISUAL V23R8
            // Non-hold inputs commit on the press frame. Hold-based light/heavy
            // inputs are intentionally deferred to BDPlayerCombat's real commit.
            PrepareCommittedAttackDamage(heavy, damage);
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
            float baseDamage = GetFloatField(
                heavy ? heavyDamageField : lightDamageField
            );
            float cooldown = GetFloatField(
                heavy ? heavyCooldownField : lightCooldownField
            );
            FieldInfo nextAllowedField =
                heavy ? nextHeavyAllowedAtField : nextLightAllowedAtField;
            float nextAllowedAt = GetFloatField(nextAllowedField);

            object[] arguments =
            {
                baseDamage,
                cooldown,
                nextAllowedAt,
                heavy ? "heavy" : "light"
            };

            try
            {
                // TryMeleeAttack owns the final committed presentation and
                // landing-damage resolution for buffered and direct attacks.
                tryMeleeAttackMethod.Invoke(combat, arguments);

                if (arguments[2] is float updatedNextAllowedAt)
                    nextAllowedField.SetValue(
                        combat,
                        updatedNextAllowedAt
                    );
            }
            catch (TargetInvocationException exception)
            {
                Debug.LogException(
                    exception.InnerException ?? exception,
                    combat
                );
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
        [SerializeField] private int arcSegments = 12;

        private LineRenderer primaryArc;
        private LineRenderer secondaryArc;
        private Material runtimeMaterial;
        private float elapsed;
        private Vector3 direction;
        private Vector3 startPosition;
        private bool heavy;
        private float initialVerticalSpeed;
        private Color baseColor;

        public static void Spawn(
            Vector3 position,
            Vector3 attackDirection,
            bool heavyAttack,
            float verticalSpeed)
        {
            GameObject root = new GameObject(
                heavyAttack
                    ? "BD_Heavy_Vertical_Airborne_Slash"
                    : "BD_Light_Vertical_Airborne_Slash"
            );

            BDLandingAttackVisual visual =
                root.AddComponent<BDLandingAttackVisual>();

            visual.direction =
                attackDirection.sqrMagnitude > 0.001f
                    ? attackDirection.normalized
                    : Vector3.forward;
            visual.direction.y = 0f;

            if (visual.direction.sqrMagnitude < 0.001f)
                visual.direction = Vector3.forward;

            visual.direction.Normalize();
            visual.heavy = heavyAttack;
            visual.initialVerticalSpeed = verticalSpeed;
            visual.startPosition =
                position +
                Vector3.up * 0.28f +
                visual.direction * 0.18f;

            root.transform.position = visual.startPosition;
            root.transform.rotation = Quaternion.LookRotation(
                visual.direction,
                Vector3.up
            );

            visual.Build();
        }

        private void Build()
        {
            // Light keeps the regular cyan identity; heavy keeps the approved
            // orange identity. Only the slash plane changes from horizontal
            // to vertical while the player is airborne.
            baseColor = heavy
                ? new Color(1f, 0.48f, 0.10f, 0.94f)
                : new Color(0.35f, 0.92f, 1f, 0.92f);

            runtimeMaterial = CreateRuntimeMaterial(baseColor);

            primaryArc = CreateArc(
                "BD_Airborne_Vertical_Slash_Primary",
                heavy ? 0.18f : 0.115f,
                heavy ? 0.42f : 0.31f,
                0f,
                mirror: false
            );

            if (heavy)
            {
                secondaryArc = CreateArc(
                    "BD_Airborne_Vertical_Slash_Secondary",
                    0.09f,
                    0.31f,
                    -0.16f,
                    mirror: true
                );
            }
        }

        private LineRenderer CreateArc(
            string objectName,
            float width,
            float curve,
            float lateralOffset,
            bool mirror)
        {
            GameObject arcObject = new GameObject(objectName);
            arcObject.transform.SetParent(
                transform,
                worldPositionStays: false
            );

            LineRenderer line =
                arcObject.AddComponent<LineRenderer>();

            line.useWorldSpace = false;
            line.loop = false;
            line.positionCount = Mathf.Max(6, arcSegments);
            line.startWidth = width;
            line.endWidth = width * 0.48f;
            line.numCapVertices = 4;
            line.numCornerVertices = 4;
            line.textureMode = LineTextureMode.Stretch;
            line.alignment = LineAlignment.View;

            if (runtimeMaterial != null)
                line.sharedMaterial = runtimeMaterial;

            float verticalStart =
                initialVerticalSpeed > 0.20f ? 2.15f : 1.92f;
            int count = line.positionCount;

            for (int index = 0; index < count; index++)
            {
                float t = index / (float)(count - 1);
                float signedCurve =
                    Mathf.Sin(t * Mathf.PI) *
                    curve *
                    (mirror ? -1f : 1f);

                Vector3 point = new Vector3(
                    lateralOffset + signedCurve,
                    Mathf.Lerp(verticalStart, -0.68f, t),
                    Mathf.Lerp(-0.10f, 1.14f, t)
                );

                line.SetPosition(index, point);
            }

            line.startColor = baseColor;
            line.endColor = new Color(
                baseColor.r,
                baseColor.g,
                baseColor.b,
                baseColor.a * 0.40f
            );

            return line;
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(
                elapsed / Mathf.Max(0.01f, lifetime)
            );

            float eased = 1f - (1f - t) * (1f - t);
            transform.position =
                startPosition +
                direction * Mathf.Lerp(0f, 0.34f, eased) +
                Vector3.down * Mathf.Lerp(0f, 0.20f, eased);

            float scale = Mathf.Lerp(
                heavy ? 0.88f : 0.82f,
                heavy ? 1.12f : 1.04f,
                eased
            );
            transform.localScale = Vector3.one * scale;

            float alpha = 1f - Mathf.SmoothStep(
                0.42f,
                1f,
                t
            );

            UpdateArcStyle(
                primaryArc,
                heavy ? 0.18f : 0.115f,
                alpha
            );
            UpdateArcStyle(
                secondaryArc,
                0.09f,
                alpha * 0.72f
            );

            if (elapsed >= lifetime)
                Destroy(gameObject);
        }

        private void UpdateArcStyle(
            LineRenderer line,
            float baseWidth,
            float alpha)
        {
            if (line == null)
                return;

            line.startWidth =
                baseWidth * Mathf.Lerp(1f, 0.58f, elapsed / lifetime);
            line.endWidth = line.startWidth * 0.48f;

            Color start = baseColor;
            start.a *= Mathf.Clamp01(alpha);

            Color end = baseColor;
            end.a *= Mathf.Clamp01(alpha * 0.38f);

            line.startColor = start;
            line.endColor = end;
        }

        private static Material CreateRuntimeMaterial(Color color)
        {
            Shader shader = Shader.Find("Sprites/Default");

            if (shader == null)
                shader = Shader.Find(
                    "Universal Render Pipeline/Unlit"
                );

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                return null;

            Material material = new Material(shader);
            material.hideFlags = HideFlags.HideAndDontSave;
            material.color = color;
            material.renderQueue = 3120;
            return material;
        }

        private void OnDestroy()
        {
            if (runtimeMaterial != null)
                Destroy(runtimeMaterial);
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
