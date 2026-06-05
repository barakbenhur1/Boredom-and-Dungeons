using System;
using UnityEngine;

namespace BoredomAndDungeons
{
    public enum BDStage17MiniBossId
    {
        SquareJumper = 0,
        Roller = 1,
        Serpent = 2,
        QuadGunners = 3
    }

    public enum BDStage17MiniBossRole
    {
        GameBoyGuardian = 0,
        CartridgeGuardian = 1,
        PreBoss = 2
    }

    public enum BDStage17LayoutMode
    {
        Sequential = 0,
        Parallel = 1,
        Mixed = 2
    }

    [Serializable]
    public sealed class BDStage17MiniBossPlacement
    {
        [SerializeField] private BDStage17MiniBossId miniBoss;
        [SerializeField] private BDStage17MiniBossRole role;
        [SerializeField] private BDStage17MiniBossRoomCandidate candidate;

        public BDStage17MiniBossId MiniBoss => miniBoss;
        public BDStage17MiniBossRole Role => role;
        public BDStage17MiniBossRoomCandidate Candidate => candidate;

        public BDStage17MiniBossPlacement(
            BDStage17MiniBossId newMiniBoss,
            BDStage17MiniBossRole newRole,
            BDStage17MiniBossRoomCandidate newCandidate)
        {
            miniBoss = newMiniBoss;
            role = newRole;
            candidate = newCandidate;
        }
    }

    [DisallowMultipleComponent]
    public sealed class BDStage17MiniBossIdentity : MonoBehaviour
    {
        [SerializeField] private BDStage17MiniBossId miniBoss;
        [SerializeField] private BDStage17MiniBossRole role;
        [SerializeField] private int runSeed;
        [SerializeField] private string candidateId;

        public BDStage17MiniBossId MiniBoss => miniBoss;
        public BDStage17MiniBossRole Role => role;
        public int RunSeed => runSeed;
        public string CandidateId => candidateId;

        public void Configure(
            BDStage17MiniBossId newMiniBoss,
            BDStage17MiniBossRole newRole,
            int newRunSeed,
            string newCandidateId)
        {
            miniBoss = newMiniBoss;
            role = newRole;
            runSeed = newRunSeed;
            candidateId = newCandidateId ?? string.Empty;
        }
    }

    public static class BDStage17RunSeed
    {
        public static int CurrentSeed { get; private set; }
        public static bool HasSeed { get; private set; }

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            CurrentSeed = 0;
            HasSeed = false;
        }

        public static void Set(int seed)
        {
            CurrentSeed = seed;
            HasSeed = true;
        }
    }
}
