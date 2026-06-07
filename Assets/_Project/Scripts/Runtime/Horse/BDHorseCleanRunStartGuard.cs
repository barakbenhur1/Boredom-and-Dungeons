using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-12000)]
    [DisallowMultipleComponent]
    public sealed class BDHorseCleanRunStartGuard : MonoBehaviour
    {
        private const float StartupCalmSeconds = 3.50f;
        private const float StartupMaintenanceSeconds = 1.50f;

        private static BDHorseCleanRunStartGuard instance;
        private Coroutine maintenanceRoutine;
        private Scene maintainedScene;
        private float calmEndsAtUnscaled;

        public static bool IsActive { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            instance = null;
            IsActive = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BootstrapBeforeSceneLoad()
        {
            EnsureInstance();
        }

        public static void RegisterHorse(BDHorseHealth horseHealth)
        {
            if (!Application.isPlaying || !IsActive || instance == null || horseHealth == null)
                return;

            ResetHorse(horseHealth, instance.RemainingCalmSeconds);
        }

        private float RemainingCalmSeconds =>
            Mathf.Max(0f, calmEndsAtUnscaled - Time.unscaledTime);

        private static void EnsureInstance()
        {
            if (!Application.isPlaying || instance != null)
                return;

            GameObject root = new GameObject("BD_Horse_Clean_Run_Start_Guard");
            DontDestroyOnLoad(root);
            instance = root.AddComponent<BDHorseCleanRunStartGuard>();
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void Start()
        {
            BeginForScene(SceneManager.GetActiveScene());
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            BeginForScene(scene);
        }

        private void BeginForScene(Scene scene)
        {
            if (!Application.isPlaying || !scene.IsValid() || !scene.isLoaded)
                return;

            if (maintenanceRoutine != null)
                StopCoroutine(maintenanceRoutine);

            maintainedScene = scene;
            calmEndsAtUnscaled = Time.unscaledTime + StartupCalmSeconds;
            maintenanceRoutine = StartCoroutine(MaintainCleanStartWindow());
        }

        private IEnumerator MaintainCleanStartWindow()
        {
            IsActive = true;
            float maintenanceEndsAt = Time.realtimeSinceStartup + StartupMaintenanceSeconds;

            do
            {
                ResetHorsesInMaintainedScene();
                yield return null;
            }
            while (Time.realtimeSinceStartup < maintenanceEndsAt);

            ResetHorsesInMaintainedScene();
            IsActive = false;
            maintenanceRoutine = null;
        }

        private void ResetHorsesInMaintainedScene()
        {
            BDHorseHealth[] horses = FindObjectsByType<BDHorseHealth>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            for (int index = 0; index < horses.Length; index++)
            {
                BDHorseHealth horseHealth = horses[index];
                if (horseHealth == null ||
                    horseHealth.gameObject.scene.handle != maintainedScene.handle)
                {
                    continue;
                }

                ResetHorse(horseHealth, RemainingCalmSeconds);
            }
        }

        private static void ResetHorse(BDHorseHealth horseHealth, float remainingCalmSeconds)
        {
            // Repeat this during startup, not only once. This closes the frame-order
            // gap where another Start/OnEnable callback could damage the horse after
            // the first reset but before gameplay becomes visible.
            horseHealth.ResetForCleanGameStart(remainingCalmSeconds);

            SendReset(horseHealth.GetComponent<BDHorseController>(), remainingCalmSeconds);
            SendReset(horseHealth.GetComponent<BDHorseCombatFleeController>(), remainingCalmSeconds);
            SendReset(horseHealth.GetComponent<BDHorseReliableFleeMotor>(), remainingCalmSeconds);
            SendReset(horseHealth.GetComponent<BDHorseHazardSafety>(), remainingCalmSeconds);
        }

        private static void SendReset(Component component, float remainingCalmSeconds)
        {
            if (component == null)
                return;

            component.SendMessage(
                "ResetForCleanGameStart",
                remainingCalmSeconds,
                SendMessageOptions.DontRequireReceiver
            );
        }
    }
}
