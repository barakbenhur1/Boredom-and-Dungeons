using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-32000)]
    [DisallowMultipleComponent]
    public sealed class BDRunPresentationCoordinator : MonoBehaviour
    {
        // BD RUN PRESENTATION COORDINATOR V7
        private const string RootName = "BD_RunPresentationCoordinator";
        private const string EntranceEffectName = "BD_AuthoredEntrance_PortalEffectOnly";
        private const string ExitEffectName = "BD_AuthoredExit_PortalEffectOnly";
        private const string ExitApproachName = "BD_Exit_Cinematic_ApproachTrigger";

        private static BDRunPresentationCoordinator instance;
        private static bool introPlayedThisSession;
        private static bool forceNextIntro;
        private static bool cinematicSeenSinceLastMenu;

        private float coverAlpha = 1f;
        private float coverTarget = 1f;
        private float coverSpeed = 8f;
        private bool inputLocked = true;
        private bool awaitingRunStart = true;
        private bool mainMenuWasVisible;
        private Coroutine activeSequence;

        public static bool InputLocked => instance != null && instance.inputLocked;
        public static bool HoldGameplayControlOnRunStart =>
            instance != null && (instance.awaitingRunStart || instance.inputLocked);
        public static bool IsTransitionActive => instance != null && instance.activeSequence != null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            instance = null;
            introPlayedThisSession = false;
            forceNextIntro = false;
            cinematicSeenSinceLastMenu = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (instance != null)
                return;

            GameObject root = new GameObject(RootName);
            DontDestroyOnLoad(root);
            instance = root.AddComponent<BDRunPresentationCoordinator>();
        }

        public static void MarkNextRunAsFreshOrVictoryIntro()
        {
            forceNextIntro = true;
            if (instance != null)
                instance.awaitingRunStart = true;
        }

        public static void MarkCinematicSeen()
        {
            cinematicSeenSinceLastMenu = true;
            MarkNextRunAsFreshOrVictoryIntro();
        }

        public static void MarkDeathRestartWithoutIntro()
        {
            forceNextIntro = false;
            introPlayedThisSession = true;
            if (instance != null)
                instance.awaitingRunStart = true;
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
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDestroy()
        {
            if (instance != this)
                return;

            SceneManager.sceneLoaded -= HandleSceneLoaded;
            instance = null;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            coverAlpha = 1f;
            coverTarget = 1f;
            inputLocked = true;
            awaitingRunStart = true;
            mainMenuWasVisible = false;

            if (activeSequence != null)
                StopCoroutine(activeSequence);

            activeSequence = StartCoroutine(PrepareLoadedScene());
        }

        private IEnumerator PrepareLoadedScene()
        {
            yield return null;

            DestroyObsoleteGeneratedPortals();
            Transform entrance = FindAuthoredDoor(true);
            Transform exit = FindAuthoredDoor(false);
            AttachEffectOnly(entrance, EntranceEffectName);
            AttachEffectOnly(exit, ExitEffectName);
            AttachExitApproachTrigger(exit);

            float deadline = Time.realtimeSinceStartup + 3f;
            while (Time.realtimeSinceStartup < deadline &&
                   !IsMainMenuVisible() &&
                   !IsGameplayRunActive())
            {
                yield return null;
            }

            if (IsMainMenuVisible())
            {
                mainMenuWasVisible = true;
                inputLocked = false;
                yield return FadeCoverTo(0f, 7f);

                while (IsMainMenuVisible())
                    yield return null;

                inputLocked = true;
                yield return FadeCoverTo(1f, 12f);
            }

            bool shouldPlayIntro = !introPlayedThisSession || forceNextIntro;
            if (shouldPlayIntro)
            {
                forceNextIntro = false;
                yield return PlayMountedEntrance(entrance);
                introPlayedThisSession = true;
            }
            else
            {
                // Ordinary death -> New Game: no mounted doorway replay.
                yield return FadeCoverTo(0f, 9f);
                awaitingRunStart = false;
                inputLocked = false;
                ReleaseOnFootGameplayControl();
            }

            activeSequence = null;
        }

        private void Update()
        {
            coverAlpha = Mathf.MoveTowards(
                coverAlpha,
                coverTarget,
                Mathf.Max(0.01f, coverSpeed) * Time.unscaledDeltaTime);

            bool menuVisible = IsMainMenuVisible();
            if (menuVisible && !mainMenuWasVisible)
            {
                mainMenuWasVisible = true;
                awaitingRunStart = true;
                inputLocked = false;

                if (cinematicSeenSinceLastMenu)
                {
                    forceNextIntro = true;
                    cinematicSeenSinceLastMenu = false;
                }
            }
            else if (!menuVisible)
            {
                mainMenuWasVisible = false;
            }
        }

        private void OnGUI()
        {
            if (coverAlpha <= 0.001f)
                return;

            int oldDepth = GUI.depth;
            Color oldColor = GUI.color;
            // The BBH boot intro uses -10000 and remains visible above this cover.
            // The menu and gameplay remain below it until the cover is released.
            GUI.depth = -9000;
            GUI.color = new Color(0.012f, 0.014f, 0.020f, Mathf.Clamp01(coverAlpha));
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = oldColor;
            GUI.depth = oldDepth;
        }

        private IEnumerator PlayMountedEntrance(Transform entrance)
        {
            Transform player = FindGameplayTransform("BDPlayerController", "Player");
            Transform horse = FindGameplayTransform("BDHorseController", "Horse");
            Transform spawn = FindBestSpawnPoint();
            BDHorseController horseController =
                horse != null ? horse.GetComponent<BDHorseController>() : null;

            if (player == null || horse == null || entrance == null || horseController == null ||
                !horseController.ForceMountForCinematic(player))
            {
                yield return FadeCoverTo(0f, 8f);
                awaitingRunStart = false;
                inputLocked = false;
                ReleaseOnFootGameplayControl();
                yield break;
            }

            // Capture after mounting: the rider controller/CharacterController are
            // already disabled and remain disabled when the horse regains control.
            List<ComponentState> states = CaptureAndDisableControls(player, horse);
            Camera camera = Camera.main;
            float originalFov = camera != null ? camera.fieldOfView : 60f;
            float originalOrtho = camera != null ? camera.orthographicSize : 5f;

            Vector3 end = spawn != null
                ? spawn.position
                : entrance.position + entrance.forward * 6f;
            Vector3 direction = end - entrance.position;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.01f)
                direction = entrance.forward;
            direction.Normalize();

            Vector3 start = entrance.position - direction * 4.25f;
            start.y = horse.position.y;
            end.y = horse.position.y;

            horse.position = start;
            horse.rotation = Quaternion.LookRotation(direction, Vector3.up);
            horseController.SnapCinematicRiderToMountPoint();
            Physics.SyncTransforms();

            yield return FadeCoverTo(0f, 7f);

            const float duration = 2.45f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float eased = t * t * (3f - 2f * t);
                horse.position = Vector3.Lerp(start, end, eased);
                horse.rotation = Quaternion.LookRotation(direction, Vector3.up);
                horseController.SnapCinematicRiderToMountPoint();

                if (camera != null)
                {
                    if (camera.orthographic)
                        camera.orthographicSize = Mathf.Lerp(originalOrtho * 0.84f, originalOrtho, eased);
                    else
                        camera.fieldOfView = Mathf.Lerp(originalFov * 0.86f, originalFov, eased);
                }

                Physics.SyncTransforms();
                yield return null;
            }

            horse.position = end;
            horseController.SnapCinematicRiderToMountPoint();
            Physics.SyncTransforms();

            if (camera != null)
            {
                camera.fieldOfView = originalFov;
                camera.orthographicSize = originalOrtho;
            }

            RestoreControls(states);
            horseController.SnapCinematicRiderToMountPoint();
            awaitingRunStart = false;
            inputLocked = false;
        }

        public static void BeginExitTransition(Transform actor, Transform authoredExit)
        {
            if (instance == null || instance.activeSequence != null)
                return;

            instance.activeSequence = instance.StartCoroutine(
                instance.PlayExitTransition(actor, authoredExit));
        }

        private IEnumerator PlayExitTransition(Transform actor, Transform authoredExit)
        {
            if (actor == null || authoredExit == null)
            {
                activeSequence = null;
                yield break;
            }

            inputLocked = true;
            awaitingRunStart = false;
            Transform player = FindGameplayTransform("BDPlayerController", "Player");
            Transform horse = FindGameplayTransform("BDHorseController", "Horse");
            BDHorseController horseController =
                horse != null ? horse.GetComponent<BDHorseController>() : null;
            List<ComponentState> states = CaptureAndDisableControls(player, horse);

            Vector3 direction = authoredExit.position - actor.position;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.01f)
                direction = authoredExit.forward;
            direction.Normalize();

            Vector3 start = actor.position;
            Vector3 end = authoredExit.position + direction * 2.5f;
            end.y = start.y;

            float elapsed = 0f;
            const float moveDuration = 1.15f;
            while (elapsed < moveDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / moveDuration);
                actor.position = Vector3.Lerp(start, end, t * t * (3f - 2f * t));
                if (horseController != null && horseController.IsMounted)
                    horseController.SnapCinematicRiderToMountPoint();
                Physics.SyncTransforms();
                yield return null;
            }

            MarkCinematicSeen();
            yield return FadeCoverTo(1f, 9f);
            InvokeExistingExitFlow(authoredExit);

            float sequenceDeadline = Time.realtimeSinceStartup + 1f;
            while (Time.realtimeSinceStartup < sequenceDeadline)
                yield return null;

            bool sequenceOwnsControl =
                BDMainMenuFlow.Instance != null &&
                BDMainMenuFlow.Instance.IsResultSequenceActive;

            if (!sequenceOwnsControl)
            {
                RestoreControls(states);
                inputLocked = false;
            }

            yield return FadeCoverTo(0f, 7f);
            activeSequence = null;
        }

        private IEnumerator FadeCoverTo(float target, float speed)
        {
            coverTarget = Mathf.Clamp01(target);
            coverSpeed = Mathf.Max(0.1f, speed);
            while (Mathf.Abs(coverAlpha - coverTarget) > 0.01f)
                yield return null;
            coverAlpha = coverTarget;
        }

        private static bool IsMainMenuVisible()
        {
            if (BDMainMenuFlow.Instance != null &&
                !BDMainMenuFlow.Instance.IsRunActive)
            {
                return true;
            }

            GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject root in roots)
            {
                string name = root.name.ToLowerInvariant();
                if (root.activeInHierarchy &&
                    (name.Contains("mainmenu") || name.Contains("main_menu") ||
                     name.Contains("mainmenusettings")))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsGameplayRunActive()
        {
            return BDMainMenuFlow.Instance != null &&
                   BDMainMenuFlow.Instance.IsRunActive;
        }

        private static void ReleaseOnFootGameplayControl()
        {
            if (BDMainMenuFlow.Instance != null)
                BDMainMenuFlow.Instance.ReleaseControlAfterRunPresentation(false);
        }

        private static Transform FindGameplayTransform(string componentTypeName, string nameHint)
        {
            MonoBehaviour[] behaviours = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour == null || !behaviour.gameObject.scene.IsValid())
                    continue;
                if (behaviour.GetType().Name == componentTypeName)
                    return behaviour.transform;
            }

            GameObject tagged = null;
            try { tagged = GameObject.FindGameObjectWithTag(nameHint); }
            catch (UnityException) { }
            if (tagged != null)
                return tagged.transform;

            Transform[] transforms = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (Transform candidate in transforms)
            {
                if (candidate != null && candidate.gameObject.scene.IsValid() &&
                    candidate.name.IndexOf(nameHint, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return candidate;
                }
            }

            return null;
        }

        private static Transform FindBestSpawnPoint()
        {
            string[] hints =
            {
                "PlayerSpawn", "SpawnPoint", "StartSpawn", "Player_Start", "StartPoint"
            };
            Transform[] all = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (string hint in hints)
            {
                foreach (Transform item in all)
                {
                    if (item != null && item.gameObject.scene.IsValid() &&
                        item.name.IndexOf(hint, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        private static Transform FindAuthoredDoor(bool entrance)
        {
            string[] preferred = entrance
                ? new[]
                {
                    "Entrance", "Entry", "StartGate", "StartDoor", "MapEntrance",
                    "MazeEntrance", "SideEntrance"
                }
                : new[]
                {
                    "Exit", "Finish", "EndGate", "EndDoor", "MapExit",
                    "MazeExit", "BossExit"
                };

            Transform[] all = Resources.FindObjectsOfTypeAll<Transform>();
            Transform best = null;
            int bestScore = int.MinValue;
            foreach (Transform item in all)
            {
                if (item == null || !item.gameObject.scene.IsValid())
                    continue;

                string lower = item.name.ToLowerInvariant();
                if (lower.Contains("bd_entrance_portal") ||
                    lower.Contains("bd_exit_portal") ||
                    lower.Contains("cinematic_approachtrigger") ||
                    lower.Contains("portaleffectonly"))
                {
                    continue;
                }

                int score = 0;
                foreach (string hint in preferred)
                {
                    if (item.name.IndexOf(hint, StringComparison.OrdinalIgnoreCase) >= 0)
                        score += 10;
                }

                if (item.GetComponent<Collider>() != null)
                    score += 3;
                if (item.GetComponent<Renderer>() != null)
                    score += 1;
                if (item.parent != null)
                    score += 1;

                if (score > bestScore && score >= 10)
                {
                    best = item;
                    bestScore = score;
                }
            }
            return best;
        }

        private static void DestroyObsoleteGeneratedPortals()
        {
            string[] names = { "BD_Entrance_Portal", "BD_Exit_Portal" };
            foreach (string name in names)
            {
                GameObject obsolete = GameObject.Find(name);
                if (obsolete != null)
                    Destroy(obsolete);
            }
        }

        private static void AttachEffectOnly(Transform authoredDoor, string effectName)
        {
            if (authoredDoor == null || authoredDoor.Find(effectName) != null)
                return;

            GameObject effect = new GameObject(effectName);
            effect.transform.SetParent(authoredDoor, false);
            effect.AddComponent<BDPortalSurfaceEffect>();
        }

        private static void AttachExitApproachTrigger(Transform authoredExit)
        {
            if (authoredExit == null || authoredExit.Find(ExitApproachName) != null)
                return;

            GameObject trigger = new GameObject(ExitApproachName);
            trigger.transform.SetParent(authoredExit, false);
            trigger.transform.localPosition = new Vector3(0f, 1f, -2.25f);
            BoxCollider collider = trigger.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = new Vector3(4.5f, 3f, 1.25f);
            BDExitCinematicApproachTrigger component =
                trigger.AddComponent<BDExitCinematicApproachTrigger>();
            component.Configure(authoredExit);
        }

        private static List<ComponentState> CaptureAndDisableControls(
            Transform player,
            Transform horse)
        {
            List<ComponentState> states = new List<ComponentState>();
            CaptureTransformControls(player, states);
            CaptureTransformControls(horse, states);
            return states;
        }

        private static void CaptureTransformControls(
            Transform root,
            List<ComponentState> states)
        {
            if (root == null)
                return;

            MonoBehaviour[] behaviours = root.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour == null)
                    continue;

                string name = behaviour.GetType().Name.ToLowerInvariant();
                if (!(name.Contains("controller") || name.Contains("combat") ||
                      name.Contains("melee") || name.Contains("ranged") ||
                      name.Contains("interaction") || name.Contains("pet")))
                {
                    continue;
                }

                states.Add(new ComponentState(behaviour, behaviour.enabled));
                behaviour.enabled = false;
            }

            CharacterController[] controllers =
                root.GetComponentsInChildren<CharacterController>(true);
            foreach (CharacterController controller in controllers)
            {
                states.Add(new ComponentState(controller, controller.enabled));
                controller.enabled = false;
            }
        }

        private static void RestoreControls(List<ComponentState> states)
        {
            foreach (ComponentState state in states)
            {
                if (state.component == null)
                    continue;

                if (state.component is Behaviour behaviour)
                    behaviour.enabled = state.enabled;
                else if (state.component is Collider collider)
                    collider.enabled = state.enabled;
            }
        }

        private static void InvokeExistingExitFlow(Transform authoredExit)
        {
            string[] names =
            {
                "BeginExit", "EnterExit", "TriggerExit", "StartEnding",
                "PlayEnding", "CompleteRun"
            };

            foreach (MonoBehaviour component in
                     authoredExit.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (component == null || component is BDExitCinematicApproachTrigger)
                    continue;

                foreach (string name in names)
                {
                    MethodInfo method = component.GetType().GetMethod(
                        name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                        null,
                        Type.EmptyTypes,
                        null);
                    if (method == null)
                        continue;

                    try
                    {
                        method.Invoke(component, null);
                        return;
                    }
                    catch (Exception) { }
                }
            }
        }

        private readonly struct ComponentState
        {
            public readonly Component component;
            public readonly bool enabled;

            public ComponentState(Component component, bool enabled)
            {
                this.component = component;
                this.enabled = enabled;
            }
        }
    }
}
