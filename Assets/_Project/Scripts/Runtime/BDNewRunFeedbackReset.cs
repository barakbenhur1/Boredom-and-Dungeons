using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-10000)]
    [DisallowMultipleComponent]
    public sealed class BDNewRunFeedbackReset : MonoBehaviour
    {
        private const float SuppressionDuration = 1.50f;
        private const float CombatInputQuarantineMinimumDuration = 0.20f;
        private const float CombatInputQuarantineMaximumDuration = 1.50f;

        private static readonly string[] FeedbackKeywords =
        {
            "damageflash",
            "damage_flash",
            "hitflash",
            "hit_flash",
            "hurtflash",
            "hurt_flash",
            "impactflash",
            "impact_flash",
            "screenflash",
            "screen_flash",
            "damageoverlay",
            "damage_overlay",
            "hurtoverlay",
            "hurt_overlay",
            "hitimpact",
            "hit_impact",
            "damageimpact",
            "damage_impact"
        };

        private static readonly string[] TransientAudioKeywords =
        {
            "hit",
            "impact",
            "damage",
            "hurt",
            "death",
            "parry"
        };

        private static BDNewRunFeedbackReset instance;
        private static bool combatInputQuarantineActive;
        private static float combatInputMinimumUntilRealtime;
        private static float combatInputMaximumUntilRealtime;
        private Coroutine resetRoutine;

        public static bool IsFeedbackSuppressed { get; private set; }

        public static bool IsCombatInputSuppressed
        {
            get
            {
                if (!combatInputQuarantineActive)
                    return false;

                float now = Time.realtimeSinceStartup;

                if (now >= combatInputMaximumUntilRealtime)
                {
                    combatInputQuarantineActive = false;
                    return false;
                }

                if (now < combatInputMinimumUntilRealtime)
                    return true;

                if (IsAttackInputHeld())
                    return true;

                combatInputQuarantineActive = false;
                return false;
            }
        }

        public static event Action RunStartFeedbackResetRequested;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            instance = null;
            IsFeedbackSuppressed = false;
            combatInputQuarantineActive = false;
            combatInputMinimumUntilRealtime = 0f;
            combatInputMaximumUntilRealtime = 0f;
            RunStartFeedbackResetRequested = null;

            RestoreGlobalPlaybackState();
            ResetKnownStaticFeedbackSystems();
        }

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BootstrapBeforeSceneLoad()
        {
            EnsureInstance();
            BeginCombatInputQuarantine();
            RestoreGlobalPlaybackState();
            ResetKnownStaticFeedbackSystems();
        }

        private static void EnsureInstance()
        {
            if (!Application.isPlaying || instance != null)
                return;

            GameObject root =
                new GameObject("BD_New_Run_Feedback_Reset");

            DontDestroyOnLoad(root);
            instance = root.AddComponent<BDNewRunFeedbackReset>();
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
            BeginFeedbackResetWindow();
        }

        private void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            BeginFeedbackResetWindow();
        }

        private void BeginFeedbackResetWindow()
        {
            if (!Application.isPlaying)
                return;

            BeginCombatInputQuarantine();

            if (resetRoutine != null)
                StopCoroutine(resetRoutine);

            resetRoutine =
                StartCoroutine(ResetFeedbackWindow());
        }

        private IEnumerator ResetFeedbackWindow()
        {
            IsFeedbackSuppressed = true;

            RestoreGlobalPlaybackState();
            ResetKnownStaticFeedbackSystems();
            NotifyExplicitResetListeners();
            ClearTransientSceneFeedback();

            float endsAt =
                Time.realtimeSinceStartup +
                SuppressionDuration;

            while (Time.realtimeSinceStartup < endsAt)
            {
                RestoreGlobalPlaybackState();
                ClearTransientSceneFeedback();
                yield return null;
            }

            RestoreGlobalPlaybackState();
            ResetKnownStaticFeedbackSystems();
            ClearTransientSceneFeedback();

            IsFeedbackSuppressed = false;
            resetRoutine = null;
        }

        private static void BeginCombatInputQuarantine()
        {
            float now = Time.realtimeSinceStartup;

            combatInputQuarantineActive = true;
            combatInputMinimumUntilRealtime =
                Mathf.Max(
                    combatInputMinimumUntilRealtime,
                    now +
                    CombatInputQuarantineMinimumDuration
                );
            combatInputMaximumUntilRealtime =
                Mathf.Max(
                    combatInputMaximumUntilRealtime,
                    now +
                    CombatInputQuarantineMaximumDuration
                );
        }

        private static bool IsAttackInputHeld()
        {
#if ENABLE_INPUT_SYSTEM
            Mouse mouse = Mouse.current;

            if (mouse != null &&
                (
                    mouse.leftButton.isPressed ||
                    mouse.rightButton.isPressed ||
                    mouse.middleButton.isPressed
                ))
            {
                return true;
            }

            Keyboard keyboard = Keyboard.current;

            if (keyboard != null &&
                (
                    keyboard.jKey.isPressed ||
                    keyboard.kKey.isPressed
                ))
            {
                return true;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButton(0) ||
                Input.GetMouseButton(1) ||
                Input.GetMouseButton(2) ||
                Input.GetKey(KeyCode.J) ||
                Input.GetKey(KeyCode.K))
            {
                return true;
            }
#endif

            return false;
        }

        private static void NotifyExplicitResetListeners()
        {
            try
            {
                RunStartFeedbackResetRequested?.Invoke();
            }
            catch (Exception exception)
            {
                Debug.LogWarning(
                    "[BDNewRunFeedbackReset] A reset listener failed: " +
                    exception.Message
                );
            }
        }

        private static void RestoreGlobalPlaybackState()
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }

        private static void ResetKnownStaticFeedbackSystems()
        {
            InvokeKnownStaticReset(
                "BoredomAndDungeons.BDParrySystem",
                "Reset"
            );

            InvokeFirstAvailableStaticReset(
                "BoredomAndDungeons.BDGameFeelEvents",
                new[]
                {
                    "ResetTransientFeedback",
                    "ClearTransientFeedback",
                    "Reset",
                    "Clear"
                }
            );

            InvokeFirstAvailableStaticReset(
                "BoredomAndDungeons.BDGameFeelAudio",
                new[]
                {
                    "ResetTransientFeedback",
                    "StopTransientAudio",
                    "Reset",
                    "Clear"
                }
            );
        }

        private static void InvokeKnownStaticReset(
            string fullTypeName,
            string methodName)
        {
            Type type = FindType(fullTypeName);

            if (type == null)
                return;

            MethodInfo method =
                type.GetMethod(
                    methodName,
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic,
                    binder: null,
                    types: Type.EmptyTypes,
                    modifiers: null
                );

            if (method == null)
                return;

            try
            {
                method.Invoke(null, null);
            }
            catch (TargetInvocationException exception)
            {
                Debug.LogWarning(
                    "[BDNewRunFeedbackReset] Static feedback reset " +
                    "failed for " +
                    fullTypeName +
                    "." +
                    methodName +
                    ": " +
                    (exception.InnerException != null
                        ? exception.InnerException.Message
                        : exception.Message)
                );
            }
            catch (Exception exception)
            {
                Debug.LogWarning(
                    "[BDNewRunFeedbackReset] Static feedback reset " +
                    "failed for " +
                    fullTypeName +
                    "." +
                    methodName +
                    ": " +
                    exception.Message
                );
            }
        }

        private static void InvokeFirstAvailableStaticReset(
            string fullTypeName,
            string[] methodNames)
        {
            Type type = FindType(fullTypeName);

            if (type == null || methodNames == null)
                return;

            for (int index = 0;
                 index < methodNames.Length;
                 index++)
            {
                string methodName = methodNames[index];

                MethodInfo method =
                    type.GetMethod(
                        methodName,
                        BindingFlags.Static |
                        BindingFlags.Public |
                        BindingFlags.NonPublic,
                        binder: null,
                        types: Type.EmptyTypes,
                        modifiers: null
                    );

                if (method == null)
                    continue;

                InvokeKnownStaticReset(
                    fullTypeName,
                    methodName
                );

                return;
            }
        }

        private static Type FindType(string fullTypeName)
        {
            Assembly[] assemblies =
                AppDomain.CurrentDomain.GetAssemblies();

            for (int index = 0;
                 index < assemblies.Length;
                 index++)
            {
                Type type =
                    assemblies[index].GetType(
                        fullTypeName,
                        throwOnError: false
                    );

                if (type != null)
                    return type;
            }

            return null;
        }

        private static void ClearTransientSceneFeedback()
        {
            StopTransientParticles();
            StopTransientAudio();
            ClearFeedbackCanvasGroups();
            ResetFeedbackAnimators();
            ResetCameraShakeState();
        }

        private static void StopTransientParticles()
        {
            ParticleSystem[] particles =
                FindObjectsByType<ParticleSystem>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            for (int index = 0;
                 index < particles.Length;
                 index++)
            {
                ParticleSystem particle = particles[index];

                if (particle == null ||
                    !MatchesFeedbackObject(
                        particle.gameObject.name))
                {
                    continue;
                }

                particle.Stop(
                    withChildren: true,
                    stopBehavior:
                        ParticleSystemStopBehavior
                            .StopEmittingAndClear
                );
            }
        }

        private static void StopTransientAudio()
        {
            AudioSource[] sources =
                FindObjectsByType<AudioSource>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            for (int index = 0;
                 index < sources.Length;
                 index++)
            {
                AudioSource source = sources[index];

                if (source == null ||
                    source.loop ||
                    !source.isPlaying)
                {
                    continue;
                }

                string clipName =
                    source.clip != null
                        ? source.clip.name
                        : string.Empty;

                if (!MatchesAnyKeyword(
                        source.gameObject.name,
                        TransientAudioKeywords) &&
                    !MatchesAnyKeyword(
                        clipName,
                        TransientAudioKeywords))
                {
                    continue;
                }

                source.Stop();
            }
        }

        private static void ClearFeedbackCanvasGroups()
        {
            CanvasGroup[] groups =
                FindObjectsByType<CanvasGroup>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            for (int index = 0;
                 index < groups.Length;
                 index++)
            {
                CanvasGroup group = groups[index];

                if (group == null ||
                    !MatchesFeedbackObject(
                        group.gameObject.name))
                {
                    continue;
                }

                group.alpha = 0f;
                group.blocksRaycasts = false;
                group.interactable = false;
            }
        }

        private static void ResetFeedbackAnimators()
        {
            Animator[] animators =
                FindObjectsByType<Animator>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            for (int index = 0;
                 index < animators.Length;
                 index++)
            {
                Animator animator = animators[index];

                if (animator == null ||
                    !MatchesFeedbackObject(
                        animator.gameObject.name))
                {
                    continue;
                }

                animator.Rebind();
                animator.Update(0f);
            }
        }

        private static void ResetCameraShakeState()
        {
            MonoBehaviour[] behaviours =
                FindObjectsByType<MonoBehaviour>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            for (int index = 0;
                 index < behaviours.Length;
                 index++)
            {
                MonoBehaviour behaviour = behaviours[index];

                if (behaviour == null)
                    continue;

                Type type = behaviour.GetType();
                string typeName = type.Name.ToLowerInvariant();

                bool possibleCameraFeedback =
                    typeName.Contains("camera") &&
                    (
                        typeName.Contains("shake") ||
                        typeName.Contains("rig") ||
                        typeName.Contains("feel")
                    );

                if (!possibleCameraFeedback)
                    continue;

                ResetTransientFields(
                    behaviour,
                    type
                );
            }
        }

        private static void ResetTransientFields(
            object target,
            Type type)
        {
            FieldInfo[] fields =
                type.GetFields(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic
                );

            for (int index = 0;
                 index < fields.Length;
                 index++)
            {
                FieldInfo field = fields[index];

                if (field.IsInitOnly ||
                    field.IsLiteral ||
                    !IsTransientStateName(field.Name))
                {
                    continue;
                }

                try
                {
                    if (field.FieldType == typeof(float))
                    {
                        field.SetValue(target, 0f);
                    }
                    else if (field.FieldType == typeof(double))
                    {
                        field.SetValue(target, 0d);
                    }
                    else if (field.FieldType == typeof(Vector2))
                    {
                        field.SetValue(target, Vector2.zero);
                    }
                    else if (field.FieldType == typeof(Vector3))
                    {
                        field.SetValue(target, Vector3.zero);
                    }
                    else if (field.FieldType == typeof(Quaternion))
                    {
                        field.SetValue(target, Quaternion.identity);
                    }
                }
                catch
                {
                    // A third-party or generated field may reject reflection.
                    // The reset remains best-effort and must not block a new run.
                }
            }
        }

        private static bool IsTransientStateName(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                return false;

            string normalized =
                fieldName.ToLowerInvariant();

            bool stateWord =
                normalized.Contains("remaining") ||
                normalized.Contains("timer") ||
                normalized.Contains("current") ||
                normalized.Contains("offset") ||
                normalized.Contains("velocity") ||
                normalized.Contains("trauma");

            bool feedbackWord =
                normalized.Contains("shake") ||
                normalized.Contains("impulse") ||
                normalized.Contains("recoil") ||
                normalized.Contains("hitstop") ||
                normalized.Contains("hit_stop");

            return stateWord && feedbackWord;
        }

        private static bool MatchesFeedbackObject(
            string objectName)
        {
            return MatchesAnyKeyword(
                objectName,
                FeedbackKeywords
            );
        }

        private static bool MatchesAnyKeyword(
            string value,
            string[] keywords)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                keywords == null)
            {
                return false;
            }

            string normalized =
                value.ToLowerInvariant()
                    .Replace(" ", string.Empty)
                    .Replace("-", "_");

            for (int index = 0;
                 index < keywords.Length;
                 index++)
            {
                if (normalized.Contains(
                        keywords[index]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
