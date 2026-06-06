using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDCollectibleGuardianSpawner : MonoBehaviour
    {
        [Header("Trigger")]
        [SerializeField] private float triggerRadius = 8.25f;
        [SerializeField] private bool spawnOnlyOnce = true;

        [Header("Guardians")]
        [SerializeField] private int swordGuardians = 3;
        [SerializeField] private int chargerGuardians = 1;
        [SerializeField] private float spawnDistance = 6.6f;
        [SerializeField] private float spawnArcDegrees = 150f;
        [SerializeField] private float swordHealth = 190f;
        [SerializeField] private float chargerHealth = 260f;

        [Header("Guardian Fairness")]
        [SerializeField] private float minimumDistanceFromPlayer = 6.25f;
        [SerializeField] private float minimumDistanceFromCollectible = 3.75f;
        [SerializeField] private float minimumGuardianSpacing = 1.85f;
        [SerializeField] private float maxSpawnDistance = 9.25f;
        [SerializeField] private float spawnDistanceStep = 0.75f;
        [SerializeField] private float spawnAngleStep = 18f;
        [SerializeField] private int spawnResolveAttempts = 14;
        [SerializeField] private float roomEdgeInset = 1.75f;

        [Header("Guardian Spawn VFX")]
        [SerializeField] private bool useSpawnVfx = true;
        [SerializeField] private float spawnVfxDelay = 1.05f;
        [SerializeField] private float spawnVerticalOffset = 1.05f;
        [SerializeField] private float inactiveSinkDepth = 1.65f;

        [Header("Debug")]
        [SerializeField] private bool logSpawn = true;
        [SerializeField] private bool logFallbackSpawnPositions = false;

        private bool spawned;
        private Transform player;
        private BDMinimapRoom spawnRoom;
        private bool missingSpawnRoomLogged;

        public void Configure(float radius, int swords, int chargers, float distance)
        {
            triggerRadius = Mathf.Max(1.5f, radius);
            swordGuardians = Mathf.Max(0, swords);
            chargerGuardians = Mathf.Max(0, chargers);
            spawnDistance = Mathf.Max(2.5f, distance);
            maxSpawnDistance = Mathf.Max(spawnDistance, maxSpawnDistance);
        }

        private void Update()
        {
            if (spawned && spawnOnlyOnce)
                return;

            if (player == null)
                player = BDTargetFinder.FindPlayer();

            if (player == null)
                return;

            // BD SAME-ROOM GUARDIAN SPAWN SAFETY V1
            if (!TryResolveSpawnRoom(out spawnRoom))
            {
                if (!missingSpawnRoomLogged)
                {
                    missingSpawnRoomLogged = true;
                    Debug.LogWarning(
                        $"B&D guardian encounter at {name} has no containing BDMinimapRoom; spawn is blocked."
                    );
                }

                return;
            }

            missingSpawnRoomLogged = false;

            // A player in the adjacent room cannot trigger this encounter through a wall.
            if (!spawnRoom.ContainsWorldPosition(player.position, 0f))
                return;

            Vector3 delta = player.position - transform.position;
            delta.y = 0f;

            if (delta.sqrMagnitude > triggerRadius * triggerRadius)
                return;

            SpawnGuardians(player);
        }

        private void SpawnGuardians(Transform playerTransform)
        {
            spawned = true;

            Vector3 awayFromPlayer = transform.position - playerTransform.position;
            awayFromPlayer.y = 0f;

            if (awayFromPlayer.sqrMagnitude < 0.001f)
                awayFromPlayer = -playerTransform.forward;

            awayFromPlayer.y = 0f;

            if (awayFromPlayer.sqrMagnitude < 0.001f)
                awayFromPlayer = Vector3.forward;

            awayFromPlayer.Normalize();

            int total = swordGuardians + chargerGuardians;
            int index = 0;
            List<Vector3> claimedPositions = new List<Vector3>(total);

            for (int i = 0; i < swordGuardians; i++)
            {
                Vector3 position = ResolveFairSpawnPosition(awayFromPlayer, index, total, playerTransform, claimedPositions);
                claimedPositions.Add(position);
                SpawnGuardianWithVfx(position, GuardianType.Sword);
                index++;
            }

            for (int i = 0; i < chargerGuardians; i++)
            {
                Vector3 position = ResolveFairSpawnPosition(awayFromPlayer, index, total, playerTransform, claimedPositions);
                claimedPositions.Add(position);
                SpawnGuardianWithVfx(position, GuardianType.Charger);
                index++;
            }

            if (logSpawn)
                Debug.Log($"B&D Collectible guardian spawn sequence started near {name}: swords={swordGuardians}, chargers={chargerGuardians}, fairPositions={claimedPositions.Count}");
        }

        private void SpawnGuardianWithVfx(Vector3 position, GuardianType type)
        {
            if (!useSpawnVfx || !Application.isPlaying)
            {
                CreateGuardian(position, type);
                return;
            }

            StartCoroutine(SpawnGuardianAfterVfx(position, type));
        }

        private IEnumerator SpawnGuardianAfterVfx(Vector3 position, GuardianType type)
        {
            BDGuardianSpawnVfx.Create(new Vector3(position.x, transform.position.y + 0.08f, position.z), spawnVfxDelay);

            Vector3 hiddenPosition = position + Vector3.down * Mathf.Max(0f, inactiveSinkDepth);
            GameObject guardian = CreateGuardian(hiddenPosition, type);
            SetGuardianActiveState(guardian, false);

            float start = Time.time;
            float delay = Mathf.Max(0.05f, spawnVfxDelay);

            while (Time.time - start < delay)
                yield return null;

            if (guardian == null)
                yield break;

            guardian.transform.position = position;
            SetGuardianActiveState(guardian, true);

            BDGuardianSpawnVfx.Create(new Vector3(position.x, transform.position.y + 0.10f, position.z), 0.35f);
        }

        private static void SetGuardianActiveState(GameObject guardian, bool active)
        {
            if (guardian == null)
                return;

            CharacterController controller = guardian.GetComponent<CharacterController>();
            if (controller != null)
                controller.enabled = active;

            BDSwordEnemy sword = guardian.GetComponent<BDSwordEnemy>();
            if (sword != null)
                sword.enabled = active;

            BDChargerEnemy charger = guardian.GetComponent<BDChargerEnemy>();
            if (charger != null)
                charger.enabled = active;

            BDEnemyGroundStick groundStick = guardian.GetComponent<BDEnemyGroundStick>();
            if (groundStick != null)
                groundStick.enabled = active;

            BDEnemyCollisionDiscipline collisionDiscipline = guardian.GetComponent<BDEnemyCollisionDiscipline>();
            if (collisionDiscipline != null)
                collisionDiscipline.enabled = active;

            BDEnemyBootstrap bootstrap = guardian.GetComponent<BDEnemyBootstrap>();
            if (bootstrap != null)
                bootstrap.enabled = active;
        }
        private Vector3 ResolveFairSpawnPosition(
            Vector3 baseDirection,
            int index,
            int total,
            Transform playerTransform,
            List<Vector3> claimedPositions)
        {
            total = Mathf.Max(1, total);
            int attempts = Mathf.Max(1, spawnResolveAttempts);
            float baseT = total == 1
                ? 0.5f
                : index / Mathf.Max(1f, total - 1f);
            float baseAngle = Mathf.Lerp(
                -spawnArcDegrees * 0.5f,
                spawnArcDegrees * 0.5f,
                baseT
            );

            bool hasHardSafeCandidate = false;
            Vector3 bestCandidate = default;
            float bestScore = float.NegativeInfinity;

            for (int attempt = 0; attempt < attempts; attempt++)
            {
                float angleOffset =
                    ResolveAttemptAngleOffset(attempt);
                float distanceOffset =
                    ResolveAttemptDistanceOffset(attempt);
                float distance = Mathf.Min(
                    Mathf.Max(spawnDistance, maxSpawnDistance),
                    spawnDistance + distanceOffset
                );

                Vector3 candidate = ResolveCandidate(
                    baseDirection,
                    baseAngle + angleOffset,
                    distance
                );

                if (spawnRoom == null ||
                    !IsInsideRoomInterior(candidate, spawnRoom) ||
                    !HasClearPathFromCollectible(candidate))
                {
                    continue;
                }

                float score = ScoreSpawnCandidate(
                    candidate,
                    playerTransform,
                    claimedPositions
                );

                if (!hasHardSafeCandidate || score > bestScore)
                {
                    hasHardSafeCandidate = true;
                    bestScore = score;
                    bestCandidate = candidate;
                }

                if (IsSpawnPositionFair(
                        candidate,
                        playerTransform,
                        claimedPositions))
                {
                    return candidate;
                }
            }

            if (hasHardSafeCandidate)
            {
                if (logFallbackSpawnPositions)
                {
                    Debug.LogWarning(
                        $"B&D guardian spawn used a same-room fallback near {name}. " +
                        $"score={bestScore:0.00}"
                    );
                }

                return bestCandidate;
            }

            if (logFallbackSpawnPositions)
            {
                Debug.LogWarning(
                    $"B&D guardian spawn had no clear candidate near {name}; " +
                    "using the room interior center rather than crossing a wall."
                );
            }

            Vector3 fallback = spawnRoom != null
                ? spawnRoom.WorldCenter
                : transform.position;
            fallback.y =
                transform.position.y +
                Mathf.Max(0.1f, spawnVerticalOffset);
            return spawnRoom != null
                ? ClampToRoomInterior(fallback, spawnRoom)
                : fallback;
        }

        private Vector3 ResolveCandidate(Vector3 baseDirection, float angle, float distance)
        {
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * baseDirection;
            Vector3 position = transform.position + direction.normalized * Mathf.Max(2.5f, distance);

            if (spawnRoom != null)
                position = ClampToRoomInterior(position, spawnRoom);

            position.y = transform.position.y + Mathf.Max(0.1f, spawnVerticalOffset);
            return position;
        }

        private float ResolveAttemptAngleOffset(int attempt)
        {
            if (attempt <= 0)
                return 0f;

            int pair = (attempt + 1) / 2;
            float sign = attempt % 2 == 0 ? -1f : 1f;
            return sign * pair * Mathf.Max(1f, spawnAngleStep);
        }

        private float ResolveAttemptDistanceOffset(int attempt)
        {
            int distanceStep = Mathf.Max(0, attempt / 4);
            return distanceStep * Mathf.Max(0f, spawnDistanceStep);
        }

        private bool IsSpawnPositionFair( Vector3 position, Transform playerTransform, List<Vector3> claimedPositions)
        {
            if (spawnRoom == null ||
                !IsInsideRoomInterior(position, spawnRoom) ||
                !HasClearPathFromCollectible(position))
            {
                return false;
            }

            if (playerTransform != null)
            {
                Vector3 playerDelta = position - playerTransform.position;
                playerDelta.y = 0f;

                if (playerDelta.sqrMagnitude < minimumDistanceFromPlayer * minimumDistanceFromPlayer)
                    return false;
            }

            Vector3 collectibleDelta = position - transform.position;
            collectibleDelta.y = 0f;

            if (collectibleDelta.sqrMagnitude < minimumDistanceFromCollectible * minimumDistanceFromCollectible)
                return false;

            for (int i = 0; i < claimedPositions.Count; i++)
            {
                Vector3 claimedDelta = position - claimedPositions[i];
                claimedDelta.y = 0f;

                if (claimedDelta.sqrMagnitude < minimumGuardianSpacing * minimumGuardianSpacing)
                    return false;
            }

            return true;
        }

        private float ScoreSpawnCandidate(Vector3 position, Transform playerTransform, List<Vector3> claimedPositions)
        {
            float score = 0f;

            if (playerTransform != null)
            {
                Vector3 playerDelta = position - playerTransform.position;
                playerDelta.y = 0f;
                score += Mathf.Min(20f, playerDelta.magnitude);
            }

            Vector3 collectibleDelta = position - transform.position;
            collectibleDelta.y = 0f;
            score += Mathf.Min(12f, collectibleDelta.magnitude) * 0.35f;

            for (int i = 0; i < claimedPositions.Count; i++)
            {
                Vector3 claimedDelta = position - claimedPositions[i];
                claimedDelta.y = 0f;
                score += Mathf.Min(6f, claimedDelta.magnitude) * 0.5f;
            }

            return score;
        }

        private bool TryResolveSpawnRoom(
            out BDMinimapRoom resolvedRoom)
        {
            if (spawnRoom != null &&
                spawnRoom.ContainsWorldPosition(
                    transform.position,
                    0.05f))
            {
                resolvedRoom = spawnRoom;
                return true;
            }

            BDMinimapRoom[] rooms =
                FindObjectsByType<BDMinimapRoom>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            resolvedRoom = null;
            float nearestDistance = float.PositiveInfinity;

            for (int i = 0; i < rooms.Length; i++)
            {
                BDMinimapRoom room = rooms[i];

                if (room == null ||
                    !room.ContainsWorldPosition(
                        transform.position,
                        0.05f))
                {
                    continue;
                }

                float distance =
                    room.SqrDistanceToCenter(
                        transform.position);

                if (distance >= nearestDistance)
                    continue;

                nearestDistance = distance;
                resolvedRoom = room;
            }

            spawnRoom = resolvedRoom;
            return resolvedRoom != null;
        }

        private Vector3 ClampToRoomInterior(
            Vector3 position,
            BDMinimapRoom room)
        {
            float halfSize = Mathf.Max(
                0.5f,
                room.RoomSize * 0.5f -
                Mathf.Max(0.25f, roomEdgeInset)
            );

            Vector3 center = room.WorldCenter;
            position.x = Mathf.Clamp(
                position.x,
                center.x - halfSize,
                center.x + halfSize
            );
            position.z = Mathf.Clamp(
                position.z,
                center.z - halfSize,
                center.z + halfSize
            );
            return position;
        }

        private bool IsInsideRoomInterior(
            Vector3 position,
            BDMinimapRoom room)
        {
            if (room == null)
                return false;

            float halfSize = Mathf.Max(
                0.5f,
                room.RoomSize * 0.5f -
                Mathf.Max(0.25f, roomEdgeInset)
            );

            Vector3 delta = position - room.WorldCenter;
            return Mathf.Abs(delta.x) <= halfSize &&
                   Mathf.Abs(delta.z) <= halfSize;
        }

        private bool HasClearPathFromCollectible(
            Vector3 position)
        {
            Vector3 origin =
                transform.position + Vector3.up * 0.85f;
            Vector3 destination = position;
            destination.y = origin.y;

            Vector3 delta = destination - origin;
            float distance = delta.magnitude;

            if (distance <= 0.50f)
                return true;

            Vector3 direction = delta / distance;
            RaycastHit[] hits = Physics.RaycastAll(
                origin + direction * 0.30f,
                direction,
                Mathf.Max(0f, distance - 0.45f),
                ~0,
                QueryTriggerInteraction.Ignore
            );

            System.Array.Sort(
                hits,
                (left, right) =>
                    left.distance.CompareTo(right.distance)
            );

            for (int i = 0; i < hits.Length; i++)
            {
                Collider hit = hits[i].collider;

                if (hit == null || hit.isTrigger)
                    continue;

                if (hit.transform == transform ||
                    hit.transform.IsChildOf(transform))
                {
                    continue;
                }

                if (hit.GetComponentInParent<BDPlayerMarker>() != null ||
                    hit.GetComponentInParent<BDHorseController>() != null)
                {
                    continue;
                }

                if (hit.bounds.max.y <= origin.y - 0.40f)
                    continue;

                return false;
            }

            return true;
        }

        private enum GuardianType
        {
            Sword,
            Charger
        }

        private GameObject CreateGuardian(Vector3 position, GuardianType type)
        {
            GameObject guardian = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            guardian.name = type == GuardianType.Sword ? "BD_Collectible_Guardian_Sword" : "BD_Collectible_Guardian_Charger";
            guardian.transform.position = position;

            CapsuleCollider capsule = guardian.GetComponent<CapsuleCollider>();
            if (capsule != null)
                Destroy(capsule);

            CharacterController controller = guardian.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.38f;
            controller.center = Vector3.zero;
            controller.stepOffset = 0.25f;

            Renderer renderer = guardian.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color color = type == GuardianType.Sword
                    ? new Color(0.19f, 0.04f, 0.05f, 1f)
                    : new Color(0.26f, 0.05f, 0.03f, 1f);

                renderer.sharedMaterial = CreateMaterial(color, color * 0.25f);
            }

            BDHealth health = guardian.AddComponent<BDHealth>();
            health.SetMaxHealth(type == GuardianType.Sword ? swordHealth : chargerHealth, true);

            guardian.AddComponent<BDEnemyBootstrap>();
            guardian.AddComponent<BDKnockbackReceiver>();
            guardian.AddComponent<BDEnemyGroundStick>();
            guardian.AddComponent<BDEnemyCollisionDiscipline>();

            if (type == GuardianType.Sword)
                guardian.AddComponent<BDSwordEnemy>();
            else
                guardian.AddComponent<BDChargerEnemy>();

            CreateDarkAura(guardian.transform, type);
            return guardian;
        }

        private static void CreateDarkAura(Transform parent, GuardianType type)
        {
            GameObject aura = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            aura.name = type == GuardianType.Sword ? "BD_Guardian_Dark_Aura" : "BD_Guardian_Heavy_Dark_Aura";
            aura.transform.SetParent(parent, worldPositionStays: false);
            aura.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            aura.transform.localScale = type == GuardianType.Sword ? Vector3.one * 2.9f : Vector3.one * 3.35f;

            Collider collider = aura.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            Renderer renderer = aura.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = CreateTransparentMaterial(new Color(0.02f, 0.0f, 0.0f, type == GuardianType.Sword ? 0.24f : 0.30f));
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
