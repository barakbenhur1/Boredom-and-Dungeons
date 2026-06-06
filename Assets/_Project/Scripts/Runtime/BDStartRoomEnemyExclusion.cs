using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-10000)]
    [DisallowMultipleComponent]
    public sealed class BDStartRoomEnemyExclusion : MonoBehaviour
    {
        private const string GuardName =
            "__BD_START_ROOM_ENEMY_EXCLUSION";

        [SerializeField] private float roomEdgeInset = 0.75f;
        [SerializeField] private float rescanInterval = 0.12f;

        private BDMinimapRoom startRoom;
        private Transform player;
        private float nextScanAt;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InstallSceneHook()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallNow()
        {
            EnsureInstalled();
        }

        private static void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            EnsureInstalled();
        }

        private static void EnsureInstalled()
        {
            if (FindFirstObjectByType<
                    BDStartRoomEnemyExclusion>() != null)
            {
                return;
            }

            GameObject guard = new GameObject(GuardName);
            DontDestroyOnLoad(guard);
            guard.AddComponent<BDStartRoomEnemyExclusion>();
        }

        private void OnEnable()
        {
            ResolveStartRoom();
            RemoveStartRoomEnemies();
        }

        private void Update()
        {
            if (Time.unscaledTime < nextScanAt)
                return;

            nextScanAt =
                Time.unscaledTime +
                Mathf.Max(0.05f, rescanInterval);

            if (startRoom == null || player == null)
                ResolveStartRoom();

            RemoveStartRoomEnemies();
        }

        private void ResolveStartRoom()
        {
            BDPlayerMarker marker =
                FindFirstObjectByType<BDPlayerMarker>();

            if (marker == null)
                return;

            player = marker.transform;

            BDMinimapRoom[] rooms =
                FindObjectsByType<BDMinimapRoom>(
                    FindObjectsSortMode.None);

            float bestDistance = float.PositiveInfinity;
            BDMinimapRoom nearest = null;

            foreach (BDMinimapRoom room in rooms)
            {
                if (room == null)
                    continue;

                if (room.ContainsWorldPosition(
                        player.position,
                        0.05f))
                {
                    startRoom = room;
                    return;
                }

                float distance =
                    room.SqrDistanceToCenter(player.position);

                if (distance >= bestDistance)
                    continue;

                bestDistance = distance;
                nearest = room;
            }

            startRoom = nearest;
        }

        private void RemoveStartRoomEnemies()
        {
            if (startRoom == null)
                return;

            BDHealth[] healthComponents =
                FindObjectsByType<BDHealth>(
                    FindObjectsSortMode.None);

            foreach (BDHealth health in healthComponents)
            {
                if (health == null ||
                    !health.gameObject.activeInHierarchy ||
                    !IsEnemyActor(health) ||
                    !IsInsideStartRoom(
                        health.transform.position))
                {
                    continue;
                }

                Destroy(health.gameObject);
            }

            MonoBehaviour[] behaviours =
                FindObjectsByType<MonoBehaviour>(
                    FindObjectsSortMode.None);

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour == null ||
                    !behaviour.enabled ||
                    !IsSpawner(behaviour) ||
                    !IsInsideStartRoom(
                        behaviour.transform.position))
                {
                    continue;
                }

                behaviour.enabled = false;
            }
        }

        private static bool IsEnemyActor(BDHealth health)
        {
            if (health.GetComponentInParent<
                    BDPlayerMarker>() != null)
            {
                return false;
            }

            if (health.GetComponentInParent<
                    BDHorseHealth>() != null)
            {
                return false;
            }

            if (health.GetComponent<
                    CharacterController>() != null)
            {
                return true;
            }

            MonoBehaviour[] behaviours =
                health.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour == null)
                    continue;

                if (behaviour.GetType().Name.IndexOf(
                        "Enemy",
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSpawner(
            MonoBehaviour behaviour)
        {
            string typeName = behaviour.GetType().Name;

            return typeName.IndexOf(
                       "Spawner",
                       StringComparison.OrdinalIgnoreCase) >= 0 &&
                   typeName.IndexOf(
                       "Enemy",
                       StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool IsInsideStartRoom(
            Vector3 position)
        {
            float halfSize = Mathf.Max(
                0.5f,
                startRoom.RoomSize * 0.5f -
                Mathf.Max(0f, roomEdgeInset)
            );

            Vector3 delta =
                position - startRoom.WorldCenter;

            return Mathf.Abs(delta.x) <= halfSize &&
                   Mathf.Abs(delta.z) <= halfSize;
        }
    }
}
