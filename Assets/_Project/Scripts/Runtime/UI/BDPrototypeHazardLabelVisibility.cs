using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    // BD OCCLUSION-SAFE PROTOTYPE HAZARD LABELS V23R14
    public sealed class BDPrototypeHazardLabelVisibility : MonoBehaviour
    {
        [SerializeField] private float maxVisibleDistance = 9.0f;
        [SerializeField] private float visibilityRefreshSeconds = 0.06f;
        [SerializeField] private float endpointPadding = 0.32f;

        private static readonly RaycastHit[] Hits = new RaycastHit[24];

        private Renderer labelRenderer;
        private Transform hazardRoot;
        private Transform player;
        private Camera cachedCamera;
        private float nextRefreshAt;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void UpgradeExistingPrototypeLabels()
        {
            if (!Application.isPlaying)
                return;

            TextMesh[] labels = Resources.FindObjectsOfTypeAll<TextMesh>();

            for (int i = 0; i < labels.Length; i++)
            {
                TextMesh label = labels[i];
                if (label == null ||
                    !label.gameObject.scene.IsValid() ||
                    !IsPrototypeHazardLabel(label.transform))
                {
                    continue;
                }

                if (label.GetComponent<BDPrototypeHazardLabelVisibility>() == null)
                {
                    label.gameObject.AddComponent<
                        BDPrototypeHazardLabelVisibility>();
                }
            }
        }

        public void Configure(float distance)
        {
            maxVisibleDistance = Mathf.Max(1f, distance);
        }

        private void Awake()
        {
            labelRenderer = GetComponent<Renderer>();
            hazardRoot = transform.parent;
            SetVisible(false);
        }

        private void LateUpdate()
        {
            if (Time.unscaledTime < nextRefreshAt)
                return;

            nextRefreshAt =
                Time.unscaledTime +
                Mathf.Max(0.02f, visibilityRefreshSeconds);

            RefreshVisibility();
        }

        private void RefreshVisibility()
        {
            if (labelRenderer == null ||
                !BDGameplayUiVisibility.IsGameplayHudVisible)
            {
                SetVisible(false);
                return;
            }

            if (cachedCamera == null)
                cachedCamera = Camera.main;
            if (player == null)
                player = BDTargetFinder.FindPlayer();

            if (cachedCamera == null || player == null)
            {
                SetVisible(false);
                return;
            }

            Vector3 target = labelRenderer.bounds.center +
                Vector3.up * 0.05f;
            float playerDistance = Vector3.Distance(
                player.position,
                target
            );

            if (playerDistance > Mathf.Max(1f, maxVisibleDistance))
            {
                SetVisible(false);
                return;
            }

            Vector3 origin = cachedCamera.transform.position;
            Vector3 delta = target - origin;
            float distance = delta.magnitude;
            if (distance <= 0.05f)
            {
                SetVisible(true);
                return;
            }

            float castDistance = Mathf.Max(
                0f,
                distance - Mathf.Max(0.05f, endpointPadding)
            );

            int hitCount = Physics.RaycastNonAlloc(
                origin,
                delta / distance,
                Hits,
                castDistance,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore
            );

            for (int i = 0; i < hitCount; i++)
            {
                Transform hit = Hits[i].transform;
                if (hit == null)
                    continue;

                if (hazardRoot != null &&
                    (hit == hazardRoot || hit.IsChildOf(hazardRoot)))
                {
                    continue;
                }

                SetVisible(false);
                return;
            }

            SetVisible(true);
        }

        private void SetVisible(bool visible)
        {
            if (labelRenderer != null &&
                labelRenderer.enabled != visible)
            {
                labelRenderer.enabled = visible;
            }
        }

        private static bool IsPrototypeHazardLabel(Transform candidate)
        {
            Transform current = candidate;
            while (current != null)
            {
                if (current.name == "__BD_HAZARD_TEST_AREA")
                    return true;
                current = current.parent;
            }

            return false;
        }
    }
}
