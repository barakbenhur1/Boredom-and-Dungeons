using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-200)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDPlayerMarker))]
    public sealed class BDPlayerAirStateTracker : MonoBehaviour
    {
        [SerializeField] private float minimumDescendingSpeed = 0.35f;
        [SerializeField] private float recentlyAirborneGrace = 0.10f;

        private CharacterController characterController;
        private float previousY;
        private float verticalSpeed;
        private float lastAirborneAt = -999f;
        private bool initialized;

        public float VerticalSpeed => verticalSpeed;
        public bool IsGrounded => characterController != null && characterController.isGrounded;
        public bool IsDescendingFromJump =>
            !BDNewRunFeedbackReset.IsCombatInputSuppressed &&
            !BDMountedRunIntro.IsGameplayInputLocked &&
            !IsGrounded &&
            verticalSpeed <= -Mathf.Max(0.05f, minimumDescendingSpeed) &&
            Time.unscaledTime - lastAirborneAt <= 2.5f;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            previousY = transform.position.y;
            initialized = true;
        }

        private void Update()
        {
            if (!initialized)
            {
                previousY = transform.position.y;
                initialized = true;
            }

            float dt = Mathf.Max(0.0001f, Time.unscaledDeltaTime);
            verticalSpeed = (transform.position.y - previousY) / dt;
            previousY = transform.position.y;

            if (!IsGrounded)
                lastAirborneAt = Time.unscaledTime;
            else if (Time.unscaledTime - lastAirborneAt > Mathf.Max(0f, recentlyAirborneGrace))
                verticalSpeed = Mathf.Min(0f, verticalSpeed);
        }
    }

    public static class BDPlayerAirStateTrackerInstaller
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallAfterSceneLoad()
        {
            Install();
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Install();
        }

        private static void Install()
        {
            BDPlayerMarker[] players = Object.FindObjectsByType<BDPlayerMarker>(FindObjectsSortMode.None);
            for (int i = 0; i < players.Length; i++)
            {
                BDPlayerMarker player = players[i];
                if (player != null && player.GetComponent<BDPlayerAirStateTracker>() == null)
                    player.gameObject.AddComponent<BDPlayerAirStateTracker>();
            }
        }
    }
}
