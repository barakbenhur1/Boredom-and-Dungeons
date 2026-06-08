using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    public static class BDSafeEnemyPlacement
    {
        private const int DirectionCount = 16;

        // BD CONTROLLER-CENTER-AWARE GROUND ROOT V23R12
        public static Vector3 ResolveRootPositionForGround(
            Vector3 groundPoint,
            Transform owner,
            CharacterController controller,
            float bottomOffset = 0.02f)
        {
            if (owner == null || controller == null)
                return groundPoint;

            float scaleY = Mathf.Max(
                0.0001f,
                Mathf.Abs(owner.lossyScale.y)
            );
            float centerOffsetY =
                owner.TransformPoint(controller.center).y -
                owner.position.y;
            float halfHeight =
                Mathf.Max(
                    controller.height * 0.5f,
                    controller.radius
                ) * scaleY;

            Vector3 root = groundPoint;
            root.y =
                groundPoint.y +
                Mathf.Max(0f, bottomOffset) +
                halfHeight -
                centerOffsetY;
            return root;
        }

        public static bool TryFindSafeGroundPosition(
            Vector3 preferred,
            Transform owner,
            Transform player,
            float bodyRadius,
            float bodyHeight,
            float minimumPlayerDistance,
            float maximumSearchRadius,
            out Vector3 safePosition)
        {
            bodyRadius = Mathf.Max(0.2f, bodyRadius);
            bodyHeight = Mathf.Max(bodyRadius * 2f, bodyHeight);
            minimumPlayerDistance = Mathf.Max(
                bodyRadius + 0.35f,
                minimumPlayerDistance
            );
            maximumSearchRadius = Mathf.Max(
                minimumPlayerDistance,
                maximumSearchRadius
            );

            Vector3 away = Vector3.forward;

            if (player != null)
            {
                away = preferred - player.position;
                away.y = 0f;

                if (away.sqrMagnitude < 0.001f)
                {
                    away = -player.forward;
                    away.y = 0f;
                }

                if (away.sqrMagnitude < 0.001f)
                    away = Vector3.forward;

                away.Normalize();

                Vector3 playerDelta = preferred - player.position;
                playerDelta.y = 0f;

                if (playerDelta.magnitude < minimumPlayerDistance)
                {
                    preferred =
                        player.position +
                        away * minimumPlayerDistance;
                }
            }

            if (TryValidate(
                    preferred,
                    owner,
                    player,
                    bodyRadius,
                    bodyHeight,
                    minimumPlayerDistance,
                    out safePosition))
            {
                return true;
            }

            float step = Mathf.Max(0.65f, bodyRadius * 1.25f);

            for (float radius = step;
                 radius <= maximumSearchRadius + 0.001f;
                 radius += step)
            {
                for (int i = 0; i < DirectionCount; i++)
                {
                    float angle = i * (360f / DirectionCount);
                    Vector3 direction =
                        Quaternion.AngleAxis(angle, Vector3.up) * away;
                    Vector3 candidate = preferred + direction * radius;

                    if (TryValidate(
                            candidate,
                            owner,
                            player,
                            bodyRadius,
                            bodyHeight,
                            minimumPlayerDistance,
                            out safePosition))
                    {
                        return true;
                    }
                }
            }

            safePosition = preferred;
            return false;
        }

        public static bool IsPositionSafe(
            Vector3 position,
            Transform owner,
            Transform player,
            float bodyRadius,
            float bodyHeight,
            float minimumPlayerDistance)
        {
            return TryValidate(
                position,
                owner,
                player,
                Mathf.Max(0.2f, bodyRadius),
                Mathf.Max(bodyRadius * 2f, bodyHeight),
                Mathf.Max(bodyRadius + 0.35f, minimumPlayerDistance),
                out _
            );
        }

        private static bool TryValidate(
            Vector3 candidate,
            Transform owner,
            Transform player,
            float bodyRadius,
            float bodyHeight,
            float minimumPlayerDistance,
            out Vector3 grounded)
        {
            grounded = candidate;

            if (!TryResolveGround(candidate, owner, out Vector3 ground))
                return false;

            grounded = ground;

            if (player != null)
            {
                Vector3 delta = ground - player.position;
                delta.y = 0f;

                if (delta.sqrMagnitude <
                    minimumPlayerDistance * minimumPlayerDistance)
                {
                    return false;
                }
            }

            Vector3 bottom =
                ground + Vector3.up * (bodyRadius + 0.08f);

            Vector3 top =
                ground + Vector3.up *
                Mathf.Max(bodyRadius + 0.1f, bodyHeight - bodyRadius);

            Collider[] overlaps = Physics.OverlapCapsule(
                bottom,
                top,
                bodyRadius,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < overlaps.Length; i++)
            {
                Collider overlap = overlaps[i];

                if (overlap == null || overlap.isTrigger)
                    continue;

                Transform candidateTransform = overlap.transform;

                if (owner != null &&
                    (candidateTransform == owner ||
                     candidateTransform.IsChildOf(owner)))
                {
                    continue;
                }

                if (overlap.bounds.max.y <= ground.y + 0.14f)
                    continue;

                return false;
            }

            return true;
        }

        private static bool TryResolveGround(
            Vector3 candidate,
            Transform owner,
            out Vector3 groundPoint)
        {
            RaycastHit[] hits = Physics.RaycastAll(
                candidate + Vector3.up * 4.5f,
                Vector3.down,
                11f,
                ~0,
                QueryTriggerInteraction.Ignore
            );

            Array.Sort(
                hits,
                (left, right) =>
                    left.distance.CompareTo(right.distance)
            );

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                Collider collider = hit.collider;

                if (collider == null || collider.isTrigger)
                    continue;

                Transform hitTransform = collider.transform;

                if (owner != null &&
                    (hitTransform == owner ||
                     hitTransform.IsChildOf(owner)))
                {
                    continue;
                }

                if (hit.normal.y < 0.58f)
                    continue;

                if (hitTransform.GetComponentInParent<BDPlayerMarker>() != null ||
                    hitTransform.GetComponentInParent<BDHorseHealth>() != null ||
                    hitTransform.GetComponentInParent<BDHealth>() != null)
                {
                    continue;
                }

                groundPoint = hit.point;
                return true;
            }

            groundPoint = candidate;
            return false;
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDEnemyPlacementGuard : MonoBehaviour
    {
        [SerializeField] private float spawnSearchRadius = 5.5f;
        [SerializeField] private float playerClearanceMargin = 0.7f;

        private CharacterController controller;
        private BDHealth health;
        private Transform player;
        private Vector3 previousPosition;
        private bool previousGrounded;
        private bool initialized;

        private IEnumerator Start()
        {
            ResolveReferences();

            // Validate before the first visible gameplay frame, then repeat
            // after one frame in case scene construction settled late.
            ValidateSpawn();
            previousPosition = transform.position;
            previousGrounded = IsGrounded();

            yield return null;

            ValidateSpawn();
            previousPosition = transform.position;
            previousGrounded = IsGrounded();
            initialized = true;
        }

        private void OnEnable()
        {
            initialized = false;
        }

        private void LateUpdate()
        {
            ResolveReferences();

            if (!initialized ||
                health == null ||
                health.IsDead)
            {
                previousPosition = transform.position;
                previousGrounded = IsGrounded();
                return;
            }

            // BD SPAWN-ONLY HORIZONTAL RELOCATION V23R12
            // Runtime landing recovery must never teleport an enemy across the
            // player. Ground-stick and the motion stabilizer own vertical repair.
            previousPosition = transform.position;
            previousGrounded = IsGrounded();
        }

        private void ResolveReferences()
        {
            if (controller == null)
                controller = GetComponent<CharacterController>();

            if (health == null)
                health = GetComponent<BDHealth>();

            if (health == null)
                health = GetComponentInParent<BDHealth>();

            if (player == null)
                player = BDTargetFinder.FindPlayer();
        }

        private void ValidateSpawn()
        {
            if (player == null)
                return;

            ResolveDimensions(out float radius, out float height);
            float minimumDistance = ResolveMinimumDistance(radius);

            if (!BDSafeEnemyPlacement.TryFindSafeGroundPosition(
                    transform.position,
                    transform,
                    player,
                    radius,
                    height,
                    minimumDistance,
                    spawnSearchRadius,
                    out Vector3 groundPoint))
            {
                return;
            }

            ApplyGroundPoint(groundPoint);
        }

        private bool IsTooCloseToPlayer()
        {
            ResolveDimensions(out float radius, out _);
            float minimumDistance = ResolveMinimumDistance(radius);

            Vector3 delta = transform.position - player.position;
            delta.y = 0f;

            return delta.sqrMagnitude <
                   minimumDistance * minimumDistance;
        }

        private void Relocate(float searchRadius)
        {
            ResolveDimensions(out float radius, out float height);
            float minimumDistance = ResolveMinimumDistance(radius);

            Vector3 away = transform.position - player.position;
            away.y = 0f;

            if (away.sqrMagnitude < 0.001f)
            {
                away = -player.forward;
                away.y = 0f;
            }

            if (away.sqrMagnitude < 0.001f)
                away = Vector3.forward;

            Vector3 preferred =
                player.position +
                away.normalized * minimumDistance;

            if (!BDSafeEnemyPlacement.TryFindSafeGroundPosition(
                    preferred,
                    transform,
                    player,
                    radius,
                    height,
                    minimumDistance,
                    searchRadius,
                    out Vector3 safePosition))
            {
                return;
            }

            ApplyGroundPoint(safePosition);
        }

        private void ApplyGroundPoint(Vector3 groundPoint)
        {
            Vector3 rootPosition =
                BDSafeEnemyPlacement.ResolveRootPositionForGround(
                    groundPoint,
                    transform,
                    controller
                );

            bool wasEnabled =
                controller != null && controller.enabled;

            if (wasEnabled)
                controller.enabled = false;

            transform.position = rootPosition;

            if (wasEnabled)
                controller.enabled = true;

            BDEnemyGroundStick groundStick =
                GetComponent<BDEnemyGroundStick>();
            if (groundStick != null)
                groundStick.ForceSnapNow();

            BDEnemyMotionStabilizer stabilizer =
                GetComponent<BDEnemyMotionStabilizer>();
            if (stabilizer != null)
                stabilizer.AcceptCurrentPositionAsBaseline();
        }

        private float ResolveMinimumDistance(float enemyRadius)
        {
            float playerRadius = 0.45f;

            CharacterController playerController =
                player != null
                    ? player.GetComponent<CharacterController>()
                    : null;

            if (playerController != null)
                playerRadius = Mathf.Max(0.2f, playerController.radius);

            return enemyRadius +
                   playerRadius +
                   Mathf.Max(0.2f, playerClearanceMargin);
        }

        private void ResolveDimensions(
            out float radius,
            out float height)
        {
            if (controller != null)
            {
                radius = Mathf.Max(0.2f, controller.radius);
                height = Mathf.Max(radius * 2f, controller.height);
                return;
            }

            Collider collider = GetComponent<Collider>();

            if (collider == null)
                collider = GetComponentInChildren<Collider>();

            if (collider != null)
            {
                Vector3 size = collider.bounds.size;
                radius = Mathf.Max(
                    0.2f,
                    Mathf.Max(size.x, size.z) * 0.5f
                );
                height = Mathf.Max(radius * 2f, size.y);
                return;
            }

            radius = 0.45f;
            height = 1.8f;
        }

        private bool IsGrounded()
        {
            return controller != null &&
                   controller.enabled &&
                   controller.isGrounded;
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDEnemyPlacementSafetyManager : MonoBehaviour
    {
        [SerializeField] private float scanInterval = 0.25f;
        private float nextScanAt;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Time.unscaledTime < nextScanAt)
                return;

            nextScanAt =
                Time.unscaledTime + Mathf.Max(0.1f, scanInterval);

            BDHealth[] allHealth =
                FindObjectsByType<BDHealth>(
                    FindObjectsSortMode.None
                );

            for (int i = 0; i < allHealth.Length; i++)
            {
                BDHealth health = allHealth[i];

                if (health == null ||
                    health.IsDead ||
                    !LooksLikeEnemy(health.gameObject))
                {
                    continue;
                }

                if (health.GetComponent<BDEnemyPlacementGuard>() == null)
                    health.gameObject.AddComponent<BDEnemyPlacementGuard>();
            }
        }

        private static bool LooksLikeEnemy(GameObject owner)
        {
            if (owner == null ||
                owner.GetComponentInParent<BDPlayerMarker>() != null ||
                owner.GetComponentInParent<BDHorseHealth>() != null)
            {
                return false;
            }

            if (owner.GetComponentInParent<BDCombatantProfile>() != null)
                return true;

            MonoBehaviour[] behaviours =
                owner.GetComponentsInParent<MonoBehaviour>(
                    includeInactive: true
                );

            for (int i = 0; i < behaviours.Length; i++)
            {
                MonoBehaviour behaviour = behaviours[i];

                if (behaviour == null)
                    continue;

                string typeName = behaviour.GetType().Name;

                if (typeName.IndexOf(
                        "Enemy",
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    typeName.IndexOf(
                        "Boss",
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static class BDEnemyPlacementSafetyInstaller
    {
        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
            EnsureManager();
        }

        private static void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            EnsureManager();
        }

        private static void EnsureManager()
        {
            if (UnityEngine.Object.FindFirstObjectByType<
                    BDEnemyPlacementSafetyManager>() != null)
            {
                return;
            }

            GameObject manager = new GameObject(
                "BD_Enemy_Placement_Safety_Manager"
            );

            manager.AddComponent<BDEnemyPlacementSafetyManager>();
        }
    }
}
