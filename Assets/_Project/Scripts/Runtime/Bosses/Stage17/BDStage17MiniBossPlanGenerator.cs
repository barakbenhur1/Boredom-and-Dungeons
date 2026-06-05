using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDStage17MiniBossPlanGenerator : MonoBehaviour
    {
        [Header("Generation")]
        [SerializeField] private bool generateOnStart = true;
        [SerializeField] private bool useFixedSeed;
        [SerializeField] private int fixedSeed = 17001;
        [SerializeField, Min(1)] private int maximumLayoutAttempts = 120;

        [Header("References")]
        [SerializeField] private BDStage17MiniBossPrefabRegistry prefabRegistry;
        [SerializeField] private BDStage17MiniBossRoomCandidate[] candidates;

        [Header("Progression constraints")]
        [Range(0f, 1f)]
        [SerializeField] private float guardianMinimumProgress = 0.18f;
        [Range(0f, 1f)]
        [SerializeField] private float guardianMaximumProgress = 0.72f;
        [Range(0f, 1f)]
        [SerializeField] private float preBossMinimumProgress = 0.62f;
        [Range(0f, 1f)]
        [SerializeField] private float preBossMaximumProgress = 0.88f;
        [Range(0f, 0.5f)]
        [SerializeField] private float minimumProgressGapBeforePreBoss = 0.10f;

        [Header("Room separation")]
        [Range(0f, 1f)]
        [SerializeField] private float minimumMapDistanceBetweenRooms = 0.18f;

        [Header("Parallel layout")]
        [Range(0f, 1f)]
        [SerializeField] private float parallelMaximumMapYDifference = 0.14f;
        [Range(0f, 1f)]
        [SerializeField] private float parallelMinimumMapXDistance = 0.34f;

        [Header("Runtime result")]
        [SerializeField] private int lastGeneratedSeed;
        [SerializeField] private BDStage17LayoutMode lastLayoutMode;
        [SerializeField] private BDStage17MiniBossId omittedMiniBoss;
        [SerializeField] private List<BDStage17MiniBossPlacement> placements =
            new List<BDStage17MiniBossPlacement>();

        [Header("Debug")]
        [SerializeField] private bool logGeneratedPlan = true;

        public int LastGeneratedSeed => lastGeneratedSeed;
        public BDStage17LayoutMode LastLayoutMode => lastLayoutMode;
        public BDStage17MiniBossId OmittedMiniBoss => omittedMiniBoss;
        public IReadOnlyList<BDStage17MiniBossPlacement> Placements =>
            placements;

        private void Start()
        {
            if (generateOnStart)
                GenerateNewRunPlan();
        }

        private void OnValidate()
        {
            maximumLayoutAttempts =
                Mathf.Max(1, maximumLayoutAttempts);

            guardianMinimumProgress =
                Mathf.Clamp01(guardianMinimumProgress);

            guardianMaximumProgress =
                Mathf.Clamp(
                    guardianMaximumProgress,
                    guardianMinimumProgress,
                    1f
                );

            preBossMinimumProgress =
                Mathf.Clamp01(preBossMinimumProgress);

            preBossMaximumProgress =
                Mathf.Clamp(
                    preBossMaximumProgress,
                    preBossMinimumProgress,
                    1f
                );

            minimumProgressGapBeforePreBoss =
                Mathf.Clamp(minimumProgressGapBeforePreBoss, 0f, 0.5f);
        }

        [ContextMenu("Generate New Run Plan")]
        public void GenerateNewRunPlan()
        {
            int seed = useFixedSeed
                ? fixedSeed
                : CreateRunSeed();

            GeneratePlan(seed);
        }

        public bool GeneratePlan(int seed)
        {
            RefreshCandidates();
            ClearCurrentPlan();

            if (candidates == null || candidates.Length < 3)
            {
                Debug.LogError(
                    "Stage 17 requires at least three mini-boss room candidates.",
                    this
                );
                return false;
            }

            lastGeneratedSeed = seed;
            BDStage17RunSeed.Set(seed);

            System.Random random = new System.Random(seed);

            List<BDStage17MiniBossId> selectedMiniBosses =
                BuildSelectedMiniBosses(random);

            List<BDStage17MiniBossRole> shuffledRoles =
                new List<BDStage17MiniBossRole>
                {
                    BDStage17MiniBossRole.GameBoyGuardian,
                    BDStage17MiniBossRole.CartridgeGuardian,
                    BDStage17MiniBossRole.PreBoss
                };

            Shuffle(shuffledRoles, random);

            lastLayoutMode =
                (BDStage17LayoutMode)random.Next(0, 3);

            CandidateLayout selectedLayout =
                FindBestLayout(lastLayoutMode, random);

            if (selectedLayout == null)
            {
                selectedLayout = BuildFallbackLayout();

                if (selectedLayout == null)
                {
                    Debug.LogError(
                        "Stage 17 could not build a legal three-room layout.",
                        this
                    );
                    return false;
                }

                lastLayoutMode = BDStage17LayoutMode.Mixed;
            }

            Dictionary<BDStage17MiniBossRole, BDStage17MiniBossRoomCandidate>
                roomsByRole =
                    new Dictionary<
                        BDStage17MiniBossRole,
                        BDStage17MiniBossRoomCandidate
                    >
                    {
                        {
                            BDStage17MiniBossRole.GameBoyGuardian,
                            selectedLayout.gameBoyRoom
                        },
                        {
                            BDStage17MiniBossRole.CartridgeGuardian,
                            selectedLayout.cartridgeRoom
                        },
                        {
                            BDStage17MiniBossRole.PreBoss,
                            selectedLayout.preBossRoom
                        }
                    };

            placements.Clear();

            for (int i = 0; i < selectedMiniBosses.Count; i++)
            {
                BDStage17MiniBossId miniBoss =
                    selectedMiniBosses[i];

                BDStage17MiniBossRole role =
                    shuffledRoles[i];

                BDStage17MiniBossRoomCandidate candidate =
                    roomsByRole[role];

                if (candidate == null ||
                    !candidate.TryReserve(role))
                {
                    ClearCurrentPlan();

                    Debug.LogError(
                        $"Stage 17 failed to reserve a room for {role}.",
                        this
                    );
                    return false;
                }

                BDStage17MiniBossPlacement placement =
                    new BDStage17MiniBossPlacement(
                        miniBoss,
                        role,
                        candidate
                    );

                placements.Add(placement);

                GameObject prefab =
                    prefabRegistry != null
                        ? prefabRegistry.Resolve(miniBoss)
                        : null;

                candidate.Slot.Spawn(
                    miniBoss,
                    role,
                    seed,
                    candidate.CandidateId,
                    prefab
                );
            }

            if (logGeneratedPlan)
                LogPlan();

            return true;
        }

        [ContextMenu("Clear Current Run Plan")]
        public void ClearCurrentPlan()
        {
            if (candidates != null)
            {
                for (int i = 0; i < candidates.Length; i++)
                {
                    BDStage17MiniBossRoomCandidate candidate =
                        candidates[i];

                    if (candidate == null)
                        continue;

                    candidate.ReleaseReservation();

                    if (candidate.Slot != null)
                        candidate.Slot.Clear();
                }
            }

            placements.Clear();
        }

        [ContextMenu("Refresh Candidate Rooms")]
        public void RefreshCandidates()
        {
            candidates =
                GetComponentsInChildren<
                    BDStage17MiniBossRoomCandidate
                >(includeInactive: true);

            if (prefabRegistry == null)
            {
                prefabRegistry =
                    GetComponent<
                        BDStage17MiniBossPrefabRegistry
                    >();
            }
        }

        private List<BDStage17MiniBossId> BuildSelectedMiniBosses(
            System.Random random)
        {
            List<BDStage17MiniBossId> all =
                new List<BDStage17MiniBossId>
                {
                    BDStage17MiniBossId.SquareJumper,
                    BDStage17MiniBossId.Roller,
                    BDStage17MiniBossId.Serpent,
                    BDStage17MiniBossId.QuadGunners
                };

            Shuffle(all, random);

            omittedMiniBoss = all[3];

            return new List<BDStage17MiniBossId>
            {
                all[0],
                all[1],
                all[2]
            };
        }

        private CandidateLayout FindBestLayout(
            BDStage17LayoutMode mode,
            System.Random random)
        {
            List<BDStage17MiniBossRoomCandidate> available =
                BuildAvailableCandidateList();

            CandidateLayout best = null;
            float bestScore = float.NegativeInfinity;

            for (int attempt = 0;
                 attempt < maximumLayoutAttempts;
                 attempt++)
            {
                BDStage17MiniBossRoomCandidate gameBoy =
                    PickRandomCandidate(
                        available,
                        BDStage17MiniBossRole.GameBoyGuardian,
                        random
                    );

                BDStage17MiniBossRoomCandidate cartridge =
                    PickRandomCandidate(
                        available,
                        BDStage17MiniBossRole.CartridgeGuardian,
                        random
                    );

                BDStage17MiniBossRoomCandidate preBoss =
                    PickRandomCandidate(
                        available,
                        BDStage17MiniBossRole.PreBoss,
                        random
                    );

                if (!AreDistinct(gameBoy, cartridge, preBoss))
                    continue;

                if (!PassesCommonConstraints(
                        gameBoy,
                        cartridge,
                        preBoss))
                {
                    continue;
                }

                if (!PassesLayoutMode(
                        mode,
                        gameBoy,
                        cartridge,
                        preBoss))
                {
                    continue;
                }

                float score = ScoreLayout(
                    mode,
                    gameBoy,
                    cartridge,
                    preBoss
                );

                // A tiny deterministic jitter prevents the same room trio
                // always winning when scores are identical.
                score += (float)random.NextDouble() * 0.015f;

                if (score <= bestScore)
                    continue;

                bestScore = score;
                best = new CandidateLayout(
                    gameBoy,
                    cartridge,
                    preBoss
                );
            }

            return best;
        }

        private CandidateLayout BuildFallbackLayout()
        {
            List<BDStage17MiniBossRoomCandidate> available =
                BuildAvailableCandidateList();

            BDStage17MiniBossRoomCandidate preBoss = null;
            float bestPreBossProgress = float.NegativeInfinity;

            for (int i = 0; i < available.Count; i++)
            {
                BDStage17MiniBossRoomCandidate candidate =
                    available[i];

                if (!candidate.SupportsRole(
                        BDStage17MiniBossRole.PreBoss))
                {
                    continue;
                }

                if (candidate.ProgressionFromEntrance <
                    preBossMinimumProgress)
                {
                    continue;
                }

                if (candidate.ProgressionFromEntrance >
                    preBossMaximumProgress)
                {
                    continue;
                }

                if (candidate.ProgressionFromEntrance >
                    bestPreBossProgress)
                {
                    bestPreBossProgress =
                        candidate.ProgressionFromEntrance;
                    preBoss = candidate;
                }
            }

            if (preBoss == null)
                return null;

            BDStage17MiniBossRoomCandidate gameBoy =
                FindFarthestGuardian(
                    available,
                    preBoss,
                    null,
                    BDStage17MiniBossRole.GameBoyGuardian
                );

            BDStage17MiniBossRoomCandidate cartridge =
                FindFarthestGuardian(
                    available,
                    preBoss,
                    gameBoy,
                    BDStage17MiniBossRole.CartridgeGuardian
                );

            if (!AreDistinct(gameBoy, cartridge, preBoss))
                return null;

            return new CandidateLayout(
                gameBoy,
                cartridge,
                preBoss
            );
        }

        private BDStage17MiniBossRoomCandidate FindFarthestGuardian(
            List<BDStage17MiniBossRoomCandidate> available,
            BDStage17MiniBossRoomCandidate preBoss,
            BDStage17MiniBossRoomCandidate excluded,
            BDStage17MiniBossRole role)
        {
            BDStage17MiniBossRoomCandidate best = null;
            float bestScore = float.NegativeInfinity;

            for (int i = 0; i < available.Count; i++)
            {
                BDStage17MiniBossRoomCandidate candidate =
                    available[i];

                if (candidate == null ||
                    candidate == preBoss ||
                    candidate == excluded ||
                    !candidate.SupportsRole(role))
                {
                    continue;
                }

                if (!IsGuardianProgressLegal(candidate))
                    continue;

                float score =
                    Vector2.Distance(
                        candidate.MapCoordinates,
                        preBoss.MapCoordinates
                    );

                if (excluded != null)
                {
                    score +=
                        Vector2.Distance(
                            candidate.MapCoordinates,
                            excluded.MapCoordinates
                        );
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }

            return best;
        }

        private bool PassesCommonConstraints(
            BDStage17MiniBossRoomCandidate gameBoy,
            BDStage17MiniBossRoomCandidate cartridge,
            BDStage17MiniBossRoomCandidate preBoss)
        {
            if (!IsGuardianProgressLegal(gameBoy) ||
                !IsGuardianProgressLegal(cartridge))
            {
                return false;
            }

            if (preBoss.ProgressionFromEntrance <
                preBossMinimumProgress ||
                preBoss.ProgressionFromEntrance >
                preBossMaximumProgress)
            {
                return false;
            }

            float latestGuardianProgress =
                Mathf.Max(
                    gameBoy.ProgressionFromEntrance,
                    cartridge.ProgressionFromEntrance
                );

            if (preBoss.ProgressionFromEntrance -
                latestGuardianProgress <
                minimumProgressGapBeforePreBoss)
            {
                return false;
            }

            float gameBoyCartridgeDistance =
                Vector2.Distance(
                    gameBoy.MapCoordinates,
                    cartridge.MapCoordinates
                );

            float gameBoyPreBossDistance =
                Vector2.Distance(
                    gameBoy.MapCoordinates,
                    preBoss.MapCoordinates
                );

            float cartridgePreBossDistance =
                Vector2.Distance(
                    cartridge.MapCoordinates,
                    preBoss.MapCoordinates
                );

            return
                gameBoyCartridgeDistance >=
                    minimumMapDistanceBetweenRooms &&
                gameBoyPreBossDistance >=
                    minimumMapDistanceBetweenRooms &&
                cartridgePreBossDistance >=
                    minimumMapDistanceBetweenRooms;
        }

        private bool PassesLayoutMode(
            BDStage17LayoutMode mode,
            BDStage17MiniBossRoomCandidate gameBoy,
            BDStage17MiniBossRoomCandidate cartridge,
            BDStage17MiniBossRoomCandidate preBoss)
        {
            switch (mode)
            {
                case BDStage17LayoutMode.Sequential:
                    return
                        Mathf.Abs(
                            gameBoy.ProgressionFromEntrance -
                            cartridge.ProgressionFromEntrance
                        ) >= 0.08f;

                case BDStage17LayoutMode.Parallel:
                    return
                        Mathf.Abs(
                            gameBoy.MapCoordinates.y -
                            cartridge.MapCoordinates.y
                        ) <= parallelMaximumMapYDifference &&
                        Mathf.Abs(
                            gameBoy.MapCoordinates.x -
                            cartridge.MapCoordinates.x
                        ) >= parallelMinimumMapXDistance;

                case BDStage17LayoutMode.Mixed:
                    return true;

                default:
                    return false;
            }
        }

        private float ScoreLayout(
            BDStage17LayoutMode mode,
            BDStage17MiniBossRoomCandidate gameBoy,
            BDStage17MiniBossRoomCandidate cartridge,
            BDStage17MiniBossRoomCandidate preBoss)
        {
            float guardianDistance =
                Vector2.Distance(
                    gameBoy.MapCoordinates,
                    cartridge.MapCoordinates
                );

            float preBossDistance =
                Vector2.Distance(
                    preBoss.MapCoordinates,
                    (gameBoy.MapCoordinates +
                     cartridge.MapCoordinates) * 0.5f
                );

            float progressionQuality =
                preBoss.ProgressionFromEntrance -
                Mathf.Max(
                    gameBoy.ProgressionFromEntrance,
                    cartridge.ProgressionFromEntrance
                );

            float score =
                guardianDistance * 1.4f +
                preBossDistance * 0.55f +
                progressionQuality * 1.8f;

            if (mode == BDStage17LayoutMode.Parallel)
            {
                score +=
                    Mathf.Abs(
                        gameBoy.MapCoordinates.x -
                        cartridge.MapCoordinates.x
                    ) * 1.4f;

                score -=
                    Mathf.Abs(
                        gameBoy.MapCoordinates.y -
                        cartridge.MapCoordinates.y
                    ) * 1.6f;
            }

            return score;
        }

        private bool IsGuardianProgressLegal(
            BDStage17MiniBossRoomCandidate candidate)
        {
            return
                candidate != null &&
                candidate.ProgressionFromEntrance >=
                    guardianMinimumProgress &&
                candidate.ProgressionFromEntrance <=
                    guardianMaximumProgress;
        }

        private List<BDStage17MiniBossRoomCandidate>
            BuildAvailableCandidateList()
        {
            List<BDStage17MiniBossRoomCandidate> result =
                new List<BDStage17MiniBossRoomCandidate>();

            for (int i = 0; i < candidates.Length; i++)
            {
                BDStage17MiniBossRoomCandidate candidate =
                    candidates[i];

                if (candidate == null ||
                    candidate.PermanentlyBlocked ||
                    candidate.Occupied)
                {
                    continue;
                }

                result.Add(candidate);
            }

            return result;
        }

        private static BDStage17MiniBossRoomCandidate
            PickRandomCandidate(
                List<BDStage17MiniBossRoomCandidate> available,
                BDStage17MiniBossRole role,
                System.Random random)
        {
            List<BDStage17MiniBossRoomCandidate> legal =
                new List<BDStage17MiniBossRoomCandidate>();

            for (int i = 0; i < available.Count; i++)
            {
                BDStage17MiniBossRoomCandidate candidate =
                    available[i];

                if (candidate != null &&
                    candidate.SupportsRole(role))
                {
                    legal.Add(candidate);
                }
            }

            if (legal.Count == 0)
                return null;

            return legal[random.Next(0, legal.Count)];
        }

        private static bool AreDistinct(
            BDStage17MiniBossRoomCandidate first,
            BDStage17MiniBossRoomCandidate second,
            BDStage17MiniBossRoomCandidate third)
        {
            return
                first != null &&
                second != null &&
                third != null &&
                first != second &&
                first != third &&
                second != third;
        }

        private static void Shuffle<T>(
            IList<T> list,
            System.Random random)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int swapIndex = random.Next(0, i + 1);
                T temporary = list[i];
                list[i] = list[swapIndex];
                list[swapIndex] = temporary;
            }
        }

        private static int CreateRunSeed()
        {
            unchecked
            {
                int ticks = System.Environment.TickCount;
                int timeHash =
                    DateTime.UtcNow.Ticks.GetHashCode();

                return ticks ^ timeHash;
            }
        }

        private void LogPlan()
        {
            string message =
                $"B&D Stage 17 plan | " +
                $"seed={lastGeneratedSeed} | " +
                $"layout={lastLayoutMode} | " +
                $"omitted={omittedMiniBoss}";

            for (int i = 0; i < placements.Count; i++)
            {
                BDStage17MiniBossPlacement placement =
                    placements[i];

                message +=
                    $"\n{i + 1}. " +
                    $"{placement.MiniBoss} → " +
                    $"{placement.Role} → " +
                    $"{placement.Candidate.CandidateId} " +
                    $"progress=" +
                    $"{placement.Candidate.ProgressionFromEntrance:0.00}";
            }

            Debug.Log(message, this);
        }

        private sealed class CandidateLayout
        {
            public readonly BDStage17MiniBossRoomCandidate gameBoyRoom;
            public readonly BDStage17MiniBossRoomCandidate cartridgeRoom;
            public readonly BDStage17MiniBossRoomCandidate preBossRoom;

            public CandidateLayout(
                BDStage17MiniBossRoomCandidate newGameBoyRoom,
                BDStage17MiniBossRoomCandidate newCartridgeRoom,
                BDStage17MiniBossRoomCandidate newPreBossRoom)
            {
                gameBoyRoom = newGameBoyRoom;
                cartridgeRoom = newCartridgeRoom;
                preBossRoom = newPreBossRoom;
            }
        }
    }
}
