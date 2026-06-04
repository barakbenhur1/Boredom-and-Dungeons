using System;
using System.Collections;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBossMagicalBarrier : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Collider blockingCollider;
        [SerializeField] private Renderer[] barrierRenderers;
        [SerializeField] private ParticleSystem dissolveParticles;

        [Header("Animation")]
        [SerializeField] private float dissolveDuration = 1.4f;
        [SerializeField] private float sinkDistance = 1.25f;
        [SerializeField] private bool disableObjectWhenFinished = true;

        private Vector3 startLocalPosition;
        private bool isOpen;
        private bool isAnimating;

        public event Action Opened;

        public bool IsOpen => isOpen;
        public bool IsAnimating => isAnimating;

        private void Awake()
        {
            startLocalPosition = transform.localPosition;

            if (blockingCollider == null)
                blockingCollider = GetComponent<Collider>();

            if (barrierRenderers == null || barrierRenderers.Length == 0)
                barrierRenderers = GetComponentsInChildren<Renderer>(includeInactive: true);
        }

        public void OpenAnimated()
        {
            if (isOpen || isAnimating)
                return;

            StartCoroutine(OpenRoutine());
        }

        public void ResetBarrier()
        {
            StopAllCoroutines();
            isOpen = false;
            isAnimating = false;
            transform.localPosition = startLocalPosition;
            gameObject.SetActive(true);

            if (blockingCollider != null)
                blockingCollider.enabled = true;

            SetRendererAlpha(1f);
        }

        private IEnumerator OpenRoutine()
        {
            isAnimating = true;

            if (dissolveParticles != null)
                dissolveParticles.Play();

            float duration = Mathf.Max(0.05f, dissolveDuration);
            float startedAt = Time.time;
            Vector3 start = transform.localPosition;
            Vector3 end = start + Vector3.down * Mathf.Max(0f, sinkDistance);

            while (Time.time - startedAt < duration)
            {
                float t = Mathf.Clamp01((Time.time - startedAt) / duration);
                float eased = t * t * (3f - 2f * t);
                transform.localPosition = Vector3.Lerp(start, end, eased);
                SetRendererAlpha(1f - eased);
                yield return null;
            }

            transform.localPosition = end;
            SetRendererAlpha(0f);

            if (blockingCollider != null)
                blockingCollider.enabled = false;

            isOpen = true;
            isAnimating = false;
            Opened?.Invoke();

            if (disableObjectWhenFinished)
                gameObject.SetActive(false);
        }

        private void SetRendererAlpha(float alpha)
        {
            if (barrierRenderers == null)
                return;

            for (int i = 0; i < barrierRenderers.Length; i++)
            {
                Renderer renderer = barrierRenderers[i];
                if (renderer == null)
                    continue;

                Material[] materials = renderer.materials;
                for (int m = 0; m < materials.Length; m++)
                {
                    Material material = materials[m];
                    if (material == null)
                        continue;

                    Color color = material.color;
                    color.a = Mathf.Clamp01(alpha);
                    material.color = color;

                    if (material.HasProperty("_BaseColor"))
                        material.SetColor("_BaseColor", color);

                    if (material.HasProperty("_Color"))
                        material.SetColor("_Color", color);
                }
            }
        }
    }
}
