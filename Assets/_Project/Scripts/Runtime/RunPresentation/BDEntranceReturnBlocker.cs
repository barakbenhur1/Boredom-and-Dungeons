using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDEntranceReturnBlocker : MonoBehaviour
    {
        // BD AUTHORED ENTRANCE WORLD-SPACE ONE-WAY BARRIER V13
        private BoxCollider barrier;

        public bool IsBlocking =>
            barrier != null && barrier.enabled;

        public void ConfigureFromAuthoredEntrance(
            Transform authoredEntrance)
        {
            EnsureBarrier();
            if (authoredEntrance == null)
                return;

            Renderer renderer =
                authoredEntrance.GetComponentInChildren<Renderer>();
            Bounds bounds = renderer != null
                ? renderer.bounds
                : new Bounds(
                    authoredEntrance.position,
                    new Vector3(4f, 0.2f, 1f)
                );

            float width = Mathf.Clamp(
                Mathf.Max(bounds.size.x, bounds.size.z) * 0.96f,
                2.5f,
                11f
            );
            float height = 4.4f;

            transform.SetParent(
                authoredEntrance.parent,
                worldPositionStays: true
            );
            transform.position =
                bounds.center + Vector3.up * (height * 0.5f);
            transform.rotation = authoredEntrance.rotation;
            transform.localScale = Vector3.one;

            barrier.center = Vector3.zero;
            barrier.size = new Vector3(width, height, 0.55f);
        }

        public void SetBlocking(bool blocking)
        {
            EnsureBarrier();
            barrier.enabled = blocking;
        }

        private void EnsureBarrier()
        {
            if (barrier != null)
                return;

            barrier = GetComponent<BoxCollider>();
            if (barrier == null)
                barrier = gameObject.AddComponent<BoxCollider>();

            barrier.isTrigger = false;
            barrier.enabled = false;
        }
    }
}
