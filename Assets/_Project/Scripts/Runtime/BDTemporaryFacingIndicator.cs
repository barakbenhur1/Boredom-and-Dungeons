using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(140)]
    [DisallowMultipleComponent]
    public sealed class BDTemporaryFacingIndicator : MonoBehaviour
    {
        public enum ActorKind
        {
            Player,
            Horse,
            Enemy
        }

        [SerializeField] private ActorKind actorKind;

        private Material frontMaterial;
        private Material rearMaterial;
        private bool built;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void EnsureBootstrap()
        {
            if (Object.FindFirstObjectByType<Bootstrap>() != null)
                return;

            GameObject root =
                new GameObject(
                    "B&D Temporary Facing Marker Bootstrap"
                );

            root.AddComponent<Bootstrap>();
        }

        public void Configure(
            ActorKind kind)
        {
            actorKind = kind;
            BuildIfNeeded();
        }

        private void Start()
        {
            BuildIfNeeded();
        }

        private void OnDestroy()
        {
            if (frontMaterial != null)
                Destroy(frontMaterial);

            if (rearMaterial != null)
                Destroy(rearMaterial);
        }

        private void BuildIfNeeded()
        {
            if (built)
                return;

            built = true;

            CharacterController controller =
                GetComponent<CharacterController>();

            float radius =
                controller != null
                    ? Mathf.Max(
                        0.35f,
                        controller.radius
                    )
                    : 0.55f;

            float height =
                controller != null
                    ? Mathf.Max(
                        1f,
                        controller.height
                    )
                    : 1.8f;

            if (actorKind == ActorKind.Horse)
            {
                radius *= 1.20f;
                height *= 0.88f;
            }

            ResolveColors(
                out Color frontColor,
                out Color rearColor
            );

            frontMaterial =
                CreateMaterial(frontColor);

            rearMaterial =
                CreateMaterial(rearColor);

            CreateMarker(
                "BD_TEMP_FRONT_UNTIL_REAL_MODELS",
                new Vector3(
                    0f,
                    height * 0.20f,
                    radius + 0.48f
                ),
                new Vector3(
                    radius * 0.42f,
                    0.13f,
                    radius * 0.95f
                ),
                frontMaterial
            );

            CreateMarker(
                "BD_TEMP_REAR_UNTIL_REAL_MODELS",
                new Vector3(
                    0f,
                    height * 0.18f,
                    -(radius + 0.32f)
                ),
                new Vector3(
                    radius * 0.62f,
                    0.11f,
                    radius * 0.28f
                ),
                rearMaterial
            );
        }

        private void ResolveColors(
            out Color front,
            out Color rear)
        {
            switch (actorKind)
            {
                case ActorKind.Player:
                    front =
                        new Color(
                            0.28f,
                            0.86f,
                            1f,
                            1f
                        );

                    rear =
                        new Color(
                            0.06f,
                            0.19f,
                            0.32f,
                            1f
                        );
                    break;

                case ActorKind.Horse:
                    front =
                        new Color(
                            1f,
                            0.76f,
                            0.24f,
                            1f
                        );

                    rear =
                        new Color(
                            0.28f,
                            0.16f,
                            0.08f,
                            1f
                        );
                    break;

                default:
                    front =
                        new Color(
                            1f,
                            0.26f,
                            0.22f,
                            1f
                        );

                    rear =
                        new Color(
                            0.18f,
                            0.02f,
                            0.03f,
                            1f
                        );
                    break;
            }
        }

        private void CreateMarker(
            string markerName,
            Vector3 localPosition,
            Vector3 localScale,
            Material material)
        {
            Transform existing =
                transform.Find(markerName);

            if (existing != null)
                return;

            GameObject marker =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube
                );

            marker.name = markerName;
            marker.transform.SetParent(
                transform,
                worldPositionStays: false
            );

            marker.transform.localPosition =
                localPosition;

            marker.transform.localRotation =
                Quaternion.identity;

            marker.transform.localScale =
                localScale;

            Collider markerCollider =
                marker.GetComponent<Collider>();

            if (markerCollider != null)
                Destroy(markerCollider);

            Renderer renderer =
                marker.GetComponent<Renderer>();

            if (renderer != null &&
                material != null)
            {
                renderer.sharedMaterial = material;
            }
        }

        private static Material CreateMaterial(
            Color color)
        {
            Shader shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                ) ??
                Shader.Find("Standard") ??
                Shader.Find("Sprites/Default");

            if (shader == null)
                return null;

            Material material =
                new Material(shader)
                {
                    color = color,
                    hideFlags = HideFlags.DontSave
                };

            return material;
        }

        private sealed class Bootstrap : MonoBehaviour
        {
            private float nextScanAt;

            private void Update()
            {
                if (Time.unscaledTime < nextScanAt)
                    return;

                nextScanAt =
                    Time.unscaledTime + 0.65f;

                AttachPlayers();
                AttachHorses();
                AttachEnemies();
            }

            private static void AttachPlayers()
            {
                BDPlayerMarker[] players =
                    Object.FindObjectsByType<BDPlayerMarker>(
                        FindObjectsInactive.Exclude,
                        FindObjectsSortMode.None
                    );

                for (int index = 0;
                     index < players.Length;
                     index++)
                {
                    Attach(
                        players[index].gameObject,
                        ActorKind.Player
                    );
                }
            }

            private static void AttachHorses()
            {
                BDHorseController[] horses =
                    Object.FindObjectsByType<BDHorseController>(
                        FindObjectsInactive.Exclude,
                        FindObjectsSortMode.None
                    );

                for (int index = 0;
                     index < horses.Length;
                     index++)
                {
                    Attach(
                        horses[index].gameObject,
                        ActorKind.Horse
                    );
                }
            }

            private static void AttachEnemies()
            {
                BDHealth[] candidates =
                    Object.FindObjectsByType<BDHealth>(
                        FindObjectsInactive.Exclude,
                        FindObjectsSortMode.None
                    );

                for (int index = 0;
                     index < candidates.Length;
                     index++)
                {
                    BDHealth candidate =
                        candidates[index];

                    if (candidate == null ||
                        candidate.GetComponent<BDPlayerMarker>() != null ||
                        candidate.GetComponent<BDHorseHealth>() != null)
                    {
                        continue;
                    }

                    if (candidate.GetComponent<CharacterController>() == null)
                        continue;

                    Attach(
                        candidate.gameObject,
                        ActorKind.Enemy
                    );
                }
            }

            private static void Attach(
                GameObject actor,
                ActorKind kind)
            {
                if (actor == null)
                    return;

                BDTemporaryFacingIndicator indicator =
                    actor.GetComponent<
                        BDTemporaryFacingIndicator>();

                if (indicator == null)
                {
                    indicator =
                        actor.AddComponent<
                            BDTemporaryFacingIndicator>();
                }

                indicator.Configure(kind);
            }
        }
    }
}
