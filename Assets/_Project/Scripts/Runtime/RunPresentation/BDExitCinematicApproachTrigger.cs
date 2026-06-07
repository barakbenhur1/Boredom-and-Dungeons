using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDExitCinematicApproachTrigger : MonoBehaviour
    {
        // BD AUTHORED EXIT CINEMATIC APPROACH V6
        private Transform authoredExit;
        private bool consumed;

        public void Configure(Transform exitTransform)
        {
            authoredExit = exitTransform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (consumed || other == null)
                return;

            Transform actor = ResolveActor(other.transform);
            if (actor == null)
                return;

            consumed = true;
            BDRunPresentationCoordinator.BeginExitTransition(
                actor,
                authoredExit != null ? authoredExit : transform.parent);
        }

        private static Transform ResolveActor(Transform candidate)
        {
            Transform playerCandidate = null;
            Transform current = candidate;
            while (current != null)
            {
                if (current.GetComponent<BDHorseController>() != null)
                    return current;

                if (current.GetComponent<BDPlayerController>() != null)
                    playerCandidate = current;

                current = current.parent;
            }

            if (playerCandidate != null)
            {
                BDHorseController mountedHorse =
                    Object.FindFirstObjectByType<BDHorseController>();
                if (mountedHorse != null && mountedHorse.IsMounted)
                    return mountedHorse.transform;
            }

            return playerCandidate;
        }
    }
}
