using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDTimedDestroy : MonoBehaviour
    {
        private float destroyAt;
        private bool shrink;

        public void Initialize(float lifetime, bool shrinkOverTime)
        {
            destroyAt = Time.time + Mathf.Max(0.01f, lifetime);
            shrink = shrinkOverTime;
        }

        private void Update()
        {
            if (shrink)
            {
                float remaining = Mathf.Max(0f, destroyAt - Time.time);
                transform.localScale = Vector3.one * Mathf.Clamp01(remaining * 4f);
            }

            if (Time.time >= destroyAt)
                Destroy(gameObject);
        }
    }
}
