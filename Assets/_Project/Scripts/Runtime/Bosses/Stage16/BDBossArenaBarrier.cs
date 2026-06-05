using System;
using System.Collections;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBossArenaBarrier : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Collider[] blockingColliders;
        [SerializeField] private Renderer[] barrierRenderers;

        [Header("State")]
        [SerializeField] private bool startsLocked;
        [SerializeField] private bool disableRenderersWhenOpen = true;

        [Header("Animation")]
        [SerializeField] private float lockDuration = 0.28f;
        [SerializeField] private float releaseDuration = 0.72f;
        [SerializeField, Range(0.01f, 0.35f)] private float releasedScaleY = 0.04f;

        private Vector3 lockedLocalScale;
        private Coroutine animationRoutine;
        private bool isLocked;
        private bool isAnimating;

        public event Action Locked;
        public event Action Released;

        public bool IsLocked => isLocked;
        public bool IsAnimating => isAnimating;
        public float ReleaseDuration => Mathf.Max(0.01f, releaseDuration);

        private void Awake()
        {
            ResolveReferences();

            if (visualRoot == null)
                visualRoot = transform;

            lockedLocalScale = visualRoot.localScale;
            SetLocked(startsLocked, immediate: true);
        }

        private void OnValidate()
        {
            lockDuration = Mathf.Max(0.01f, lockDuration);
            releaseDuration = Mathf.Max(0.01f, releaseDuration);
            releasedScaleY = Mathf.Clamp(releasedScaleY, 0.01f, 0.35f);
        }

        public void SetLocked(bool locked, bool immediate = false)
        {
            ResolveReferences();

            if (animationRoutine != null)
            {
                StopCoroutine(animationRoutine);
                animationRoutine = null;
            }

            if (immediate || !Application.isPlaying)
            {
                ApplyImmediate(locked);
                return;
            }

            animationRoutine = StartCoroutine(
                AnimateState(
                    locked,
                    locked ? lockDuration : releaseDuration
                )
            );
        }

        public void Lock()
        {
            SetLocked(true);
        }

        public void Release()
        {
            SetLocked(false);
        }

        private void ResolveReferences()
        {
            if (visualRoot == null)
                visualRoot = transform;

            if (blockingColliders == null || blockingColliders.Length == 0)
            {
                blockingColliders =
                    GetComponentsInChildren<Collider>(includeInactive: true);
            }

            if (barrierRenderers == null || barrierRenderers.Length == 0)
            {
                barrierRenderers =
                    GetComponentsInChildren<Renderer>(includeInactive: true);
            }
        }

        private IEnumerator AnimateState(bool lockBarrier, float duration)
        {
            isAnimating = true;

            if (lockBarrier)
            {
                SetRenderersEnabled(true);
                SetCollidersEnabled(false);
            }
            else
            {
                // The exit remains physically blocked until the disappearance
                // animation finishes.
                SetCollidersEnabled(true);
            }

            Vector3 startScale = visualRoot.localScale;
            Vector3 targetScale = lockBarrier
                ? lockedLocalScale
                : new Vector3(
                    lockedLocalScale.x,
                    lockedLocalScale.y * releasedScaleY,
                    lockedLocalScale.z
                );

            float elapsed = 0f;
            float safeDuration = Mathf.Max(0.01f, duration);

            while (elapsed < safeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / safeDuration);
                float eased = lockBarrier
                    ? 1f - Mathf.Pow(1f - t, 3f)
                    : t * t * (3f - 2f * t);

                visualRoot.localScale =
                    Vector3.Lerp(startScale, targetScale, eased);

                yield return null;
            }

            visualRoot.localScale = targetScale;
            isLocked = lockBarrier;

            if (lockBarrier)
            {
                SetCollidersEnabled(true);
                Locked?.Invoke();
            }
            else
            {
                SetCollidersEnabled(false);

                if (disableRenderersWhenOpen)
                    SetRenderersEnabled(false);

                Released?.Invoke();
            }

            isAnimating = false;
            animationRoutine = null;
        }

        private void ApplyImmediate(bool locked)
        {
            isLocked = locked;
            isAnimating = false;

            if (visualRoot != null)
            {
                visualRoot.localScale = locked
                    ? lockedLocalScale
                    : new Vector3(
                        lockedLocalScale.x,
                        lockedLocalScale.y * releasedScaleY,
                        lockedLocalScale.z
                    );
            }

            SetCollidersEnabled(locked);
            SetRenderersEnabled(locked || !disableRenderersWhenOpen);

            if (locked)
                Locked?.Invoke();
            else
                Released?.Invoke();
        }

        private void SetCollidersEnabled(bool enabled)
        {
            if (blockingColliders == null)
                return;

            for (int i = 0; i < blockingColliders.Length; i++)
            {
                Collider collider = blockingColliders[i];

                if (collider != null && !collider.isTrigger)
                    collider.enabled = enabled;
            }
        }

        private void SetRenderersEnabled(bool enabled)
        {
            if (barrierRenderers == null)
                return;

            for (int i = 0; i < barrierRenderers.Length; i++)
            {
                Renderer renderer = barrierRenderers[i];

                if (renderer != null)
                    renderer.enabled = enabled;
            }
        }
    }
}
