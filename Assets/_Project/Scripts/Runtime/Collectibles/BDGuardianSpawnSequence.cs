using System.Collections;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDGuardianSpawnSequence : MonoBehaviour
    {
        // BD COLLECTIBLE-INDEPENDENT GUARDIAN SPAWN V23R19E
        private GameObject guardian;
        private Vector3 spawnPosition;
        private float vfxBaseY;
        private float delay;

        public static void Schedule(
            GameObject guardian,
            Vector3 spawnPosition,
            float vfxBaseY,
            float delay)
        {
            if (guardian == null)
                return;

            GameObject runner = new GameObject(
                "BD_Collectible_Guardian_SpawnSequence"
            );
            runner.transform.position = spawnPosition;

            BDGuardianSpawnSequence sequence =
                runner.AddComponent<BDGuardianSpawnSequence>();
            sequence.guardian = guardian;
            sequence.spawnPosition = spawnPosition;
            sequence.vfxBaseY = vfxBaseY;
            sequence.delay = Mathf.Max(0.05f, delay);
            sequence.StartCoroutine(sequence.Run());
        }

        private IEnumerator Run()
        {
            float elapsed = 0f;
            while (elapsed < delay)
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            if (guardian != null)
            {
                guardian.transform.position = spawnPosition;
                BDCollectibleGuardianSpawner.ActivateGuardian(guardian);

                BDGuardianSpawnVfx.Create(
                    new Vector3(
                        spawnPosition.x,
                        vfxBaseY + 0.10f,
                        spawnPosition.z
                    ),
                    0.35f
                );
            }

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (guardian != null && !guardian.activeSelf)
                Destroy(guardian);
        }
    }
}
