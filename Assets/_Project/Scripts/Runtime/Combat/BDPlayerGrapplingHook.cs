using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDPlayerGrapplingHook : MonoBehaviour
    {
        private static readonly RaycastHit[] HitBuffer = new RaycastHit[48];
        private static Material sharedRopeMaterial;
        private static Material sharedHookMaterial;

        private Transform owner;
        private Transform targetRoot;
        private BDHealth targetHealth;
        private CharacterController ownerController;
        private CharacterController targetController;
        private Rigidbody targetBody;
        private BDGrapplingHookPullState pullState;
        private Collider targetCollider;
        private LineRenderer rope;
        private LineRenderer hookShape;
        private Transform hookRoot;

        private Vector3 direction;
        private Vector3 fallbackHitPoint;
        private Vector3 currentHookPosition;
        private float travelSpeed;
        private float damage;
        private float pullStopDistance;
        private float pullDuration;
        private float maxPullHorizontalSize;
        private float maxPullHeight;
        private float elapsed;
        private bool impacted;
        private bool pulling;
        private bool retracting;
        private bool canPullTarget;

        public static void Launch(
            Transform owner,
            Vector3 direction,
            float maxRange,
            float hitRadius,
            float travelSpeed,
            float damage,
            float pullStopDistance,
            float pullDuration,
            float maxPullHorizontalSize,
            float maxPullHeight)
        {
            if (owner == null)
                return;

            direction.y = 0f;
            if (direction.sqrMagnitude < 0.001f)
                direction = owner.forward;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.001f)
                direction = Vector3.forward;
            direction.Normalize();

            Vector3 origin = ResolveOwnerAnchor(owner, direction);
            ResolveFirstHit(
                owner,
                origin,
                direction,
                Mathf.Max(1f, maxRange),
                Mathf.Max(0.05f, hitRadius),
                out RaycastHit hit,
                out BDHealth health
            );

            GameObject root = new GameObject("BD_Player_Grappling_Hook");
            BDPlayerGrapplingHook hook = root.AddComponent<BDPlayerGrapplingHook>();
            hook.owner = owner;
            hook.ownerController = owner.GetComponent<CharacterController>();
            hook.direction = direction;
            hook.travelSpeed = Mathf.Max(1f, travelSpeed);
            hook.damage = Mathf.Max(0f, damage);
            hook.pullStopDistance = Mathf.Max(1.10f, pullStopDistance);
            hook.pullDuration = Mathf.Max(0.08f, pullDuration);
            hook.maxPullHorizontalSize = Mathf.Max(0.5f, maxPullHorizontalSize);
            hook.maxPullHeight = Mathf.Max(0.75f, maxPullHeight);
            hook.currentHookPosition = origin;
            hook.targetHealth = health;
            hook.targetCollider = hit.collider;
            hook.fallbackHitPoint = hit.collider != null
                ? hit.point
                : origin + direction * Mathf.Max(1f, maxRange);

            if (health != null)
            {
                // BD RELIABLE MOVEMENT-ROOT HOOK PULL V23R12
                // Damage may live on a child while locomotion lives on the
                // CharacterController root. Resolve and move the real actor root.
                hook.targetRoot = ResolveTargetRoot(health);
                hook.targetController = ResolveTargetController(
                    health,
                    hook.targetRoot
                );
                hook.targetBody = ResolveTargetBody(
                    health,
                    hook.targetRoot
                );
                hook.targetCollider = ResolveTargetBodyCollider(
                    hit.collider,
                    health,
                    hook.targetRoot,
                    hook.targetController
                );
                hook.pullStopDistance = ResolveSafePullStopDistance(
                    hook.ownerController,
                    hook.targetController,
                    hook.pullStopDistance
                );
                hook.pullState =
                    hook.targetRoot.GetComponent<BDGrapplingHookPullState>();
                if (hook.pullState == null)
                {
                    hook.pullState =
                        hook.targetRoot.gameObject.AddComponent<
                            BDGrapplingHookPullState>();
                }

                hook.canPullTarget = CanPullSmallEnemy(
                    health,
                    hook.targetController,
                    hook.targetCollider,
                    hook.maxPullHorizontalSize,
                    hook.maxPullHeight
                );
            }

            hook.BuildVisuals();
        }

        private static Vector3 ResolveOwnerAnchor(
            Transform owner,
            Vector3 direction)
        {
            return owner.position +
                   Vector3.up * 1.05f +
                   direction * 0.65f;
        }

        private static Transform ResolveTargetRoot(BDHealth health)
        {
            CharacterController controller =
                ResolveTargetController(health, null);
            if (controller != null)
                return controller.transform;

            Rigidbody body = ResolveTargetBody(health, null);
            if (body != null)
                return body.transform;

            BDCombatantProfile profile =
                BDCombatantProfile.ResolveProfile(health);
            if (profile != null)
                return profile.transform;

            return health.transform;
        }

        private static CharacterController ResolveTargetController(
            BDHealth health,
            Transform resolvedRoot)
        {
            if (resolvedRoot != null)
            {
                CharacterController onRoot =
                    resolvedRoot.GetComponent<CharacterController>();
                if (onRoot != null)
                    return onRoot;
            }

            CharacterController controller =
                health.GetComponent<CharacterController>();
            if (controller == null)
                controller =
                    health.GetComponentInParent<CharacterController>();
            if (controller == null)
                controller =
                    health.GetComponentInChildren<CharacterController>();

            return controller;
        }

        private static Rigidbody ResolveTargetBody(
            BDHealth health,
            Transform resolvedRoot)
        {
            if (resolvedRoot != null)
            {
                Rigidbody onRoot =
                    resolvedRoot.GetComponent<Rigidbody>();
                if (onRoot != null)
                    return onRoot;
            }

            Rigidbody body = health.GetComponent<Rigidbody>();
            if (body == null)
                body = health.GetComponentInParent<Rigidbody>();
            if (body == null)
                body = health.GetComponentInChildren<Rigidbody>();

            return body;
        }

        private static Collider ResolveTargetBodyCollider(
            Collider impactCollider,
            BDHealth health,
            Transform resolvedRoot,
            CharacterController controller)
        {
            if (controller != null)
                return controller;

            if (impactCollider != null &&
                !impactCollider.isTrigger &&
                resolvedRoot != null &&
                (impactCollider.transform == resolvedRoot ||
                 impactCollider.transform.IsChildOf(resolvedRoot)))
            {
                return impactCollider;
            }

            Collider[] colliders =
                resolvedRoot != null
                    ? resolvedRoot.GetComponentsInChildren<Collider>(
                        includeInactive: true)
                    : health.GetComponentsInChildren<Collider>(
                        includeInactive: true);

            for (int index = 0; index < colliders.Length; index++)
            {
                Collider candidate = colliders[index];
                if (candidate == null || candidate.isTrigger)
                    continue;

                string lower = candidate.name.ToLowerInvariant();
                if (lower.Contains("attack") ||
                    lower.Contains("range") ||
                    lower.Contains("vision") ||
                    lower.Contains("detect") ||
                    lower.Contains("telegraph"))
                {
                    continue;
                }

                return candidate;
            }

            return impactCollider;
        }

        private static void ResolveFirstHit(
            Transform owner,
            Vector3 origin,
            Vector3 direction,
            float range,
            float radius,
            out RaycastHit resolvedHit,
            out BDHealth health)
        {
            resolvedHit = default;
            health = null;

            int count = Physics.SphereCastNonAlloc(
                origin,
                radius,
                direction,
                HitBuffer,
                range,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            float bestDistance = float.PositiveInfinity;
            for (int i = 0; i < count; i++)
            {
                RaycastHit candidate = HitBuffer[i];
                Collider collider = candidate.collider;
                HitBuffer[i] = default;

                if (collider == null)
                    continue;

                Transform hitTransform = collider.transform;
                if (hitTransform == owner ||
                    hitTransform.IsChildOf(owner) ||
                    owner.IsChildOf(hitTransform))
                {
                    continue;
                }

                if (collider.GetComponentInParent<BDPlayerMarker>() != null)
                    continue;

                if (collider.GetComponentInParent<BDHorseHealth>() != null)
                    continue;

                if (candidate.distance >= bestDistance)
                    continue;

                bestDistance = candidate.distance;
                resolvedHit = candidate;
            }

            if (resolvedHit.collider == null)
                return;

            BDHealth candidateHealth =
                resolvedHit.collider.GetComponentInParent<BDHealth>();

            if (candidateHealth == null || candidateHealth.IsDead)
                return;

            health = candidateHealth;
        }


        private static float ResolveSafePullStopDistance(
            CharacterController ownerController,
            CharacterController targetController,
            float requestedDistance)
        {
            float ownerRadius = ownerController != null
                ? Mathf.Max(0.20f, ownerController.radius)
                : 0.48f;
            float targetRadius = targetController != null
                ? Mathf.Max(0.20f, targetController.radius)
                : 0.58f;

            // BD SAFE HOOK RELEASE BEFORE CONTACT V23R10
            return Mathf.Max(
                requestedDistance,
                ownerRadius + targetRadius + 0.70f
            );
        }

        private static bool CanPullSmallEnemy(
            BDHealth health,
            CharacterController controller,
            Collider bodyCollider,
            float maxHorizontalSize,
            float maxHeight)
        {
            if (health == null || health.IsDead)
                return false;

            // BD HOOK HIT-COMMITS SMALL-ENEMY PULL V23R19B
            // Use the same authoritative small-regular policy as horse impacts
            // and forced hazard entry. Elite collectible guardians, oversized
            // enemies, mini-bosses and bosses remain damage-only.
            if (!BDEnemyHazardNavigation.IsSmallRegularEnemy(health) ||
                !BDCombatantProfile.CanReceiveForcedMovement(health))
            {
                return false;
            }

            // A valid active CharacterController is the stable body truth.
            // Do not let helper colliders or temporary visual bounds downgrade
            // a confirmed small enemy after the hook has hit it.
            if (controller != null)
                return true;

            if (bodyCollider == null)
                return true;

            Vector3 size = bodyCollider.bounds.size;
            float horizontalSize = Mathf.Max(size.x, size.z);
            float height = size.y;

            return
                horizontalSize <= maxHorizontalSize * 1.08f &&
                height <= maxHeight * 1.08f;
        }

        private void BuildVisuals()
        {
            rope = gameObject.AddComponent<LineRenderer>();
            rope.useWorldSpace = true;
            rope.positionCount = 2;
            rope.startWidth = 0.055f;
            rope.endWidth = 0.040f;
            rope.numCapVertices = 3;
            rope.numCornerVertices = 3;
            rope.sharedMaterial = GetRopeMaterial();
            rope.startColor = new Color(0.28f, 0.14f, 0.055f, 1f);
            rope.endColor = new Color(0.48f, 0.27f, 0.09f, 1f);

            GameObject hookObject = new GameObject("BD_Grappling_Hook_Head");
            hookRoot = hookObject.transform;
            hookRoot.SetParent(transform, worldPositionStays: true);
            hookRoot.position = currentHookPosition;

            hookShape = hookObject.AddComponent<LineRenderer>();
            hookShape.useWorldSpace = false;
            hookShape.positionCount = 6;
            hookShape.startWidth = 0.10f;
            hookShape.endWidth = 0.075f;
            hookShape.numCapVertices = 4;
            hookShape.numCornerVertices = 5;
            hookShape.sharedMaterial = GetHookMaterial();
            hookShape.startColor = new Color(0.70f, 0.74f, 0.78f, 1f);
            hookShape.endColor = new Color(0.35f, 0.40f, 0.46f, 1f);
            hookShape.SetPositions(new[]
            {
                new Vector3(-0.18f, 0.16f, 0f),
                new Vector3(0.02f, 0.10f, 0f),
                new Vector3(0.13f, -0.02f, 0f),
                new Vector3(0.10f, -0.18f, 0f),
                new Vector3(-0.02f, -0.27f, 0f),
                new Vector3(-0.15f, -0.20f, 0f)
            });

            UpdateVisuals();
        }

        private void Update()
        {
            if (owner == null)
            {
                Destroy(gameObject);
                return;
            }

            float delta = Time.deltaTime;
            elapsed += delta;

            if (!impacted)
            {
                Vector3 destination = ResolveDestination();
                currentHookPosition = Vector3.MoveTowards(
                    currentHookPosition,
                    destination,
                    travelSpeed * delta
                );

                if ((currentHookPosition - destination).sqrMagnitude <= 0.0025f)
                    Impact();
            }
            else if (pulling)
            {
                TickPull(delta);
            }
            else if (retracting)
            {
                Vector3 origin = ResolveOwnerAnchor(owner, direction);
                currentHookPosition = Vector3.MoveTowards(
                    currentHookPosition,
                    origin,
                    travelSpeed * 1.45f * delta
                );

                if ((currentHookPosition - origin).sqrMagnitude <= 0.0064f)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            UpdateVisuals();
        }

        private Vector3 ResolveDestination()
        {
            if (targetHealth == null || targetHealth.IsDead)
                return fallbackHitPoint;

            if (targetCollider != null)
            {
                Bounds bounds = targetCollider.bounds;
                return bounds.center;
            }

            Transform root =
                targetRoot != null ? targetRoot : targetHealth.transform;
            return root.position + Vector3.up * 0.9f;
        }

        private void Impact()
        {
            impacted = true;

            if (targetHealth == null || targetHealth.IsDead)
            {
                retracting = true;
                return;
            }

            // Re-evaluate at the actual impact frame. Once a living small
            // regular enemy was hit, pulling is committed for this transaction.
            canPullTarget = CanPullSmallEnemy(
                targetHealth,
                targetController,
                targetCollider,
                maxPullHorizontalSize,
                maxPullHeight
            );

            targetHealth.ApplyDamage(damage);
            BDGameFeelAudio.PlayLightHit();
            BDGameFeelEvents.RequestCameraShake(0.08f, 0.07f);

            BDEnemyHitFlashReceiver flash =
                targetHealth.GetComponent<BDEnemyHitFlashReceiver>();
            if (flash == null &&
                targetHealth.GetComponent<CharacterController>() != null)
            {
                flash = targetHealth.gameObject.AddComponent<BDEnemyHitFlashReceiver>();
            }
            if (flash != null)
                flash.FlashRanged();

            BDRangedAttackVisuals.SpawnImpactBurst(
                ResolveDestination(),
                playerProjectile: true
            );

            if (!canPullTarget || targetHealth.IsDead)
            {
                retracting = true;
                return;
            }

            BDHitStaggerReceiver stagger =
                targetRoot != null
                    ? targetRoot.GetComponent<BDHitStaggerReceiver>()
                    : targetHealth.GetComponent<BDHitStaggerReceiver>();
            if (stagger == null && targetRoot != null)
                stagger = targetRoot.gameObject.AddComponent<BDHitStaggerReceiver>();
            if (stagger == null && targetController != null)
                stagger = targetHealth.gameObject.AddComponent<BDHitStaggerReceiver>();
            if (stagger != null)
                stagger.RequestStagger(pullDuration + 0.55f);

            if (pullState != null)
                pullState.Begin(pullDuration + 0.55f);

            BDKnockbackReceiver knockback =
                targetRoot != null
                    ? targetRoot.GetComponent<BDKnockbackReceiver>()
                    : targetHealth.GetComponent<BDKnockbackReceiver>();
            if (knockback != null)
                knockback.ClearKnockback();

            elapsed = 0f;
            pulling = true;
        }

        private void TickPull(float delta)
        {
            if (targetHealth == null || targetHealth.IsDead)
            {
                pulling = false;
                retracting = true;
                return;
            }

            Vector3 enemyPosition = ResolveTargetRootPosition();
            Vector3 fromOwner = enemyPosition - owner.position;
            fromOwner.y = 0f;

            float distance = fromOwner.magnitude;
            if (distance <= pullStopDistance + 0.08f ||
                elapsed >= pullDuration)
            {
                CompletePullAndRetract();
                return;
            }

            Vector3 radial = distance > 0.001f
                ? fromOwner / distance
                : direction;

            Vector3 destination =
                owner.position + radial * pullStopDistance;
            destination.y = enemyPosition.y;

            float remaining = Mathf.Max(0.03f, pullDuration - elapsed);
            float speed = Mathf.Clamp(
                Vector3.Distance(enemyPosition, destination) / remaining,
                4.5f,
                20f
            );

            Vector3 motion = Vector3.MoveTowards(
                enemyPosition,
                destination,
                speed * delta
            ) - enemyPosition;

            if (targetController != null && targetController.enabled)
            {
                targetController.Move(motion);
            }
            else if (targetBody != null && !targetBody.isKinematic)
            {
                targetBody.MovePosition(targetBody.position + motion);
            }
            else
            {
                MoveTargetRootDirectly(motion);
            }

            currentHookPosition = ResolveDestination();
        }


        private Vector3 ResolveTargetRootPosition()
        {
            if (targetRoot != null)
                return targetRoot.position;

            return targetHealth != null
                ? targetHealth.transform.position
                : currentHookPosition;
        }

        private void MoveTargetRootDirectly(Vector3 motion)
        {
            if (targetRoot != null)
                targetRoot.position += motion;
            else if (targetHealth != null)
                targetHealth.transform.position += motion;
        }

        private void CompletePullAndRetract()
        {
            pulling = false;
            retracting = true;

            if (targetHealth != null && !targetHealth.IsDead)
            {
                Vector3 fromOwner = ResolveTargetRootPosition() - owner.position;
                fromOwner.y = 0f;
                Vector3 radial = fromOwner.sqrMagnitude > 0.001f
                    ? fromOwner.normalized
                    : direction;

                Vector3 safePosition = owner.position + radial * pullStopDistance;
                safePosition.y = ResolveTargetRootPosition().y;
                Vector3 correction = safePosition - ResolveTargetRootPosition();

                if (correction.sqrMagnitude > 0.0004f)
                {
                    if (targetController != null && targetController.enabled)
                        targetController.Move(correction);
                    else if (targetBody != null && !targetBody.isKinematic)
                        targetBody.MovePosition(targetBody.position + correction);
                    else
                        MoveTargetRootDirectly(correction);
                }

                BDEnemyGroundStick groundStick =
                    targetRoot != null
                        ? targetRoot.GetComponent<BDEnemyGroundStick>()
                        : targetHealth.GetComponent<BDEnemyGroundStick>();
                if (groundStick != null)
                    groundStick.ForceSnapNow();

                BDEnemyMotionStabilizer stabilizer =
                    targetRoot != null
                        ? targetRoot.GetComponent<BDEnemyMotionStabilizer>()
                        : targetHealth.GetComponent<BDEnemyMotionStabilizer>();
                if (stabilizer != null)
                    stabilizer.AcceptCurrentPositionAsBaseline();

                if (pullState != null)
                    pullState.Begin(0.55f);
            }

            currentHookPosition = ResolveDestination();
        }

        private void UpdateVisuals()
        {
            if (owner == null)
                return;

            Vector3 origin = ResolveOwnerAnchor(owner, direction);

            if (rope != null)
            {
                rope.SetPosition(0, origin);
                rope.SetPosition(1, currentHookPosition);
            }

            if (hookRoot != null)
            {
                hookRoot.position = currentHookPosition;
                hookRoot.rotation = Quaternion.LookRotation(
                    direction,
                    Vector3.up
                );
            }
        }

        private static Material GetRopeMaterial()
        {
            if (sharedRopeMaterial == null)
                sharedRopeMaterial = CreateMaterial(new Color(0.38f, 0.20f, 0.07f, 1f));
            return sharedRopeMaterial;
        }

        private static Material GetHookMaterial()
        {
            if (sharedHookMaterial == null)
                sharedHookMaterial = CreateMaterial(new Color(0.62f, 0.67f, 0.73f, 1f));
            return sharedHookMaterial;
        }

        private static Material CreateMaterial(Color color)
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader == null)
                shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null)
                shader = Shader.Find("Unlit/Color");
            if (shader == null)
                shader = Shader.Find("Standard");

            Material material = shader != null
                ? new Material(shader)
                : null;

            if (material != null)
            {
                material.hideFlags = HideFlags.HideAndDontSave;
                material.color = color;
                material.renderQueue = 3115;
            }

            return material;
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDGrapplingHookPullState : MonoBehaviour
    {
        private float activeUntil;
        public bool IsActive => Application.isPlaying && Time.time < activeUntil;
        public bool SuppressesImmediateContactAttack => IsActive;

        public static bool IsContactAttackSuppressed(Transform enemy)
        {
            if (enemy == null)
                return false;

            BDGrapplingHookPullState state =
                enemy.GetComponentInParent<BDGrapplingHookPullState>();
            if (state == null)
                state =
                    enemy.GetComponentInChildren<BDGrapplingHookPullState>();

            return state != null &&
                   state.SuppressesImmediateContactAttack;
        }

        public void Begin(float duration)
        {
            activeUntil = Mathf.Max(
                activeUntil,
                Time.time + Mathf.Max(0.05f, duration)
            );
        }
    }
}
