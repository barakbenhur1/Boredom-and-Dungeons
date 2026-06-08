using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDPlayerMarker))]
    public sealed class BDPlayerAirborneAttackAnimation : MonoBehaviour
    {
        // BD DISTINCT AIRBORNE LIGHT/HEAVY BODY ANIMATION V23R17
        private static readonly List<BDPlayerAirborneAttackAnimation> Active =
            new List<BDPlayerAirborneAttackAnimation>();

        [SerializeField] private float lightDuration = 0.28f;
        [SerializeField] private float heavyDuration = 0.48f;
        [SerializeField] private float lightPitchDegrees = 10f;
        [SerializeField] private float heavyPitchDegrees = 16f;

        private Transform visualRoot;
        private Vector3 restPosition;
        private Quaternion restRotation;
        private Coroutine routine;

        private void Awake()
        {
            ResolveVisualRoot();
            CaptureRestPose();
            if (!Active.Contains(this))
                Active.Add(this);
        }

        private void OnDestroy()
        {
            Active.Remove(this);
        }

        private void OnDisable()
        {
            CancelAndRestore();
        }

        public void Play(bool heavy)
        {
            ResolveVisualRoot();
            if (visualRoot == null)
                return;

            CancelAndRestore();
            routine = StartCoroutine(Animate(heavy));
        }

        public static void CancelAllActive()
        {
            for (int index = Active.Count - 1; index >= 0; index--)
            {
                BDPlayerAirborneAttackAnimation animation = Active[index];
                if (animation == null)
                {
                    Active.RemoveAt(index);
                    continue;
                }
                animation.CancelAndRestore();
            }
        }

        private IEnumerator Animate(bool heavy)
        {
            float duration = heavy
                ? Mathf.Max(0.12f, heavyDuration)
                : Mathf.Max(0.10f, lightDuration);
            float pitch = heavy
                ? heavyPitchDegrees
                : lightPitchDegrees;
            float windupEnd = heavy ? 0.30f : 0.22f;
            float strikeEnd = heavy ? 0.68f : 0.60f;
            float elapsed = 0f;

            // BD AIRBORNE BODY SUPPORTS ROTATED ATTACK V23R19E
            // The slash itself is the selected grounded attack rotated exactly
            // 90 degrees. Keep body motion deliberately small so it supports
            // that readable vertical attack instead of replacing it.
            Quaternion windup = restRotation *
                Quaternion.Euler(-pitch * 0.34f, 0f, 0f);
            Quaternion strike = restRotation *
                Quaternion.Euler(pitch, 0f, 0f);
            Vector3 windupPosition =
                restPosition + Vector3.up * (heavy ? 0.05f : 0.03f);
            Vector3 strikePosition =
                restPosition +
                Vector3.forward * (heavy ? 0.08f : 0.05f) +
                Vector3.down * (heavy ? 0.08f : 0.05f);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                if (t < windupEnd)
                {
                    float phase = Smooth(t / windupEnd);
                    visualRoot.localRotation = Quaternion.Slerp(restRotation, windup, phase);
                    visualRoot.localPosition = Vector3.Lerp(restPosition, windupPosition, phase);
                }
                else if (t < strikeEnd)
                {
                    float phase = Smooth((t - windupEnd) / Mathf.Max(0.01f, strikeEnd - windupEnd));
                    visualRoot.localRotation = Quaternion.Slerp(windup, strike, phase);
                    visualRoot.localPosition = Vector3.Lerp(windupPosition, strikePosition, phase);
                }
                else
                {
                    float phase = Smooth((t - strikeEnd) / Mathf.Max(0.01f, 1f - strikeEnd));
                    visualRoot.localRotation = Quaternion.Slerp(strike, restRotation, phase);
                    visualRoot.localPosition = Vector3.Lerp(strikePosition, restPosition, phase);
                }

                yield return null;
            }

            Restore();
            routine = null;
        }

        private static float Smooth(float value)
        {
            value = Mathf.Clamp01(value);
            return value * value * (3f - 2f * value);
        }

        private void ResolveVisualRoot()
        {
            if (visualRoot != null)
                return;

            visualRoot = transform.Find("BD_Player_Visual");
        }

        private void CaptureRestPose()
        {
            if (visualRoot == null)
                return;
            restPosition = visualRoot.localPosition;
            restRotation = visualRoot.localRotation;
        }

        private void CancelAndRestore()
        {
            if (routine != null)
            {
                StopCoroutine(routine);
                routine = null;
            }
            Restore();
        }

        private void Restore()
        {
            if (visualRoot == null)
                return;
            visualRoot.localPosition = restPosition;
            visualRoot.localRotation = restRotation;
        }
    }
}
