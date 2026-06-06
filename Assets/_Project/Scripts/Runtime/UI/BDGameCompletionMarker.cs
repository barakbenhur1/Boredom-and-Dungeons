using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDGameCompletionMarker :
        MonoBehaviour
    {
        [SerializeField] private bool motherVictorySequence;

        public bool MotherVictorySequence =>
            motherVictorySequence;

        // Timeline Signal / Animation Event entry point.
        public void BeginResultSequence()
        {
            BDGameFlowSignals.BeginResultSequence();
        }

        // Use after the ordinary death cutscene, the ending-door
        // sequence without the required items, or Mother defeat.
        public void FinishSequenceToUnchangedMainMenu()
        {
            BDGameFlowSignals
                .ReturnToMainMenuAfterSequence();
        }

        // Use only after every Mother-victory cinematic has finished.
        public void FinishMotherVictorySequence()
        {
            BDGameFlowSignals
                .CompleteMotherVictorySequence();
        }

        public void FinishConfiguredSequence()
        {
            if (motherVictorySequence)
            {
                FinishMotherVictorySequence();
            }
            else
            {
                FinishSequenceToUnchangedMainMenu();
            }
        }
    }
}
