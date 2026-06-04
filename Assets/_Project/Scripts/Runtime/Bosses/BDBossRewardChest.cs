using System;
using System.Collections;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBossRewardChest : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform lid;
        [SerializeField] private GameObject rewardObject;
        [SerializeField] private Collider rewardCollider;

        [Header("Animation")]
        [SerializeField] private float openDuration = 0.75f;
        [SerializeField] private float openAngle = -105f;
        [SerializeField] private float rewardRevealDelay = 0.35f;
        [SerializeField] private float rewardRiseHeight = 0.65f;
        [SerializeField] private float rewardRiseDuration = 0.45f;

        [Header("State")]
        [SerializeField] private bool startsLocked = true;
        [SerializeField] private bool openOnlyOnce = true;

        private Quaternion closedRotation;
        private Vector3 rewardHiddenLocalPosition;
        private bool isLocked;
        private bool isOpen;
        private bool isAnimating;

        public event Action Opened;
        public event Action RewardRevealed;

        public bool IsLocked => isLocked;
        public bool IsOpen => isOpen;
        public bool IsAnimating => isAnimating;

        private void Awake()
        {
            isLocked = startsLocked;

            if (lid != null)
                closedRotation = lid.localRotation;

            if (rewardObject != null)
            {
                rewardHiddenLocalPosition = rewardObject.transform.localPosition;
                rewardObject.SetActive(false);
            }

            if (rewardCollider != null)
                rewardCollider.enabled = false;
        }

        public void Unlock()
        {
            isLocked = false;
        }

        public void UnlockAndOpen()
        {
            Unlock();
            TryOpen();
        }

        public bool TryOpen()
        {
            if (isLocked || isAnimating)
                return false;

            if (isOpen && openOnlyOnce)
                return false;

            StartCoroutine(OpenRoutine());
            return true;
        }

        public void ResetChest(bool lockChest)
        {
            StopAllCoroutines();
            isAnimating = false;
            isOpen = false;
            isLocked = lockChest;

            if (lid != null)
                lid.localRotation = closedRotation;

            if (rewardObject != null)
            {
                rewardObject.transform.localPosition = rewardHiddenLocalPosition;
                rewardObject.SetActive(false);
            }

            if (rewardCollider != null)
                rewardCollider.enabled = false;
        }

        private IEnumerator OpenRoutine()
        {
            isAnimating = true;

            if (lid != null)
            {
                Quaternion start = lid.localRotation;
                Quaternion end = closedRotation * Quaternion.Euler(openAngle, 0f, 0f);
                float duration = Mathf.Max(0.01f, openDuration);
                float startedAt = Time.time;

                while (Time.time - startedAt < duration)
                {
                    float t = Mathf.Clamp01((Time.time - startedAt) / duration);
                    float eased = 1f - Mathf.Pow(1f - t, 3f);
                    lid.localRotation = Quaternion.Slerp(start, end, eased);
                    yield return null;
                }

                lid.localRotation = end;
            }

            isOpen = true;
            Opened?.Invoke();

            float delay = Mathf.Max(0f, rewardRevealDelay);
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            if (rewardObject != null)
            {
                rewardObject.SetActive(true);
                Vector3 start = rewardHiddenLocalPosition;
                Vector3 end = rewardHiddenLocalPosition + Vector3.up * Mathf.Max(0f, rewardRiseHeight);
                float duration = Mathf.Max(0.01f, rewardRiseDuration);
                float startedAt = Time.time;

                while (Time.time - startedAt < duration)
                {
                    float t = Mathf.Clamp01((Time.time - startedAt) / duration);
                    float eased = 1f - Mathf.Pow(1f - t, 3f);
                    rewardObject.transform.localPosition = Vector3.Lerp(start, end, eased);
                    yield return null;
                }

                rewardObject.transform.localPosition = end;
            }

            if (rewardCollider != null)
                rewardCollider.enabled = true;

            RewardRevealed?.Invoke();
            isAnimating = false;
        }
    }
}
