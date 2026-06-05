using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [Serializable]
    public sealed class BDMiniBossPlanRules
    {
        [Min(1)] public int maxPlacementAttempts = 128;
        [Min(0)] public int minDistanceFromEntrance = 2;
        [Min(0)] public int minDistanceFromExit = 1;
        [Min(1)] public int minCandidateManhattanDistance = 2;
        [Min(1)] public int minParallelXSeparation = 3;
        [Min(0)] public int maxParallelYDelta = 1;
        [Range(0f, 1f)] public float minSequentialProgressionGap = 0.18f;
        [Range(0f, 1f)] public float secretMinProgression = 0.22f;
        [Range(0f, 1f)] public float secretMaxProgression = 0.84f;
        [Range(0f, 1f)] public float preBossMinProgression = 0.58f;
        [Range(0f, 1f)] public float preBossMaxProgression = 0.92f;
        [Range(0f, 1f)] public float minimumPreBossLead = 0.05f;
        [Range(0f, 1f)] public float parallelPatternChance = 0.30f;
        [Range(0f, 1f)] public float sequentialPatternChance = 0.40f;

        public void Normalize()
        {
            maxPlacementAttempts = Mathf.Max(1, maxPlacementAttempts);
            minDistanceFromEntrance = Mathf.Max(0, minDistanceFromEntrance);
            minDistanceFromExit = Mathf.Max(0, minDistanceFromExit);
            minCandidateManhattanDistance = Mathf.Max(1, minCandidateManhattanDistance);
            minParallelXSeparation = Mathf.Max(1, minParallelXSeparation);
            maxParallelYDelta = Mathf.Max(0, maxParallelYDelta);
            minSequentialProgressionGap = Mathf.Clamp01(minSequentialProgressionGap);
            secretMinProgression = Mathf.Clamp01(secretMinProgression);
            secretMaxProgression = Mathf.Clamp01(secretMaxProgression);
            preBossMinProgression = Mathf.Clamp01(preBossMinProgression);
            preBossMaxProgression = Mathf.Clamp01(preBossMaxProgression);
            minimumPreBossLead = Mathf.Clamp01(minimumPreBossLead);
            parallelPatternChance = Mathf.Clamp01(parallelPatternChance);
            sequentialPatternChance = Mathf.Clamp01(sequentialPatternChance);

            if (secretMaxProgression < secretMinProgression)
            {
                float swap = secretMinProgression;
                secretMinProgression = secretMaxProgression;
                secretMaxProgression = swap;
            }

            if (preBossMaxProgression < preBossMinProgression)
            {
                float swap = preBossMinProgression;
                preBossMinProgression = preBossMaxProgression;
                preBossMaxProgression = swap;
            }

            float patternTotal = parallelPatternChance + sequentialPatternChance;
            if (patternTotal > 1f)
            {
                parallelPatternChance /= patternTotal;
                sequentialPatternChance /= patternTotal;
            }
        }
    }

    public static class BDMiniBossPlanGenerator
    {
        public static bool TryGenerate(
            int seed,
            IReadOnlyList<BDMiniBossPlacementCandidate> candidates,
            BDMiniBossPlanRules rules,
            out BDMiniBossGeneratedPlan plan,
            out string error)
        {
            plan = null;
            error = string.Empty;

            if (candidates == null)
            {
                error = "Mini-boss plan generation failed: candidates collection is null.";
                return false;
            }

            if (candidates.Count < 3)
            {
                error = $"Mini-boss plan generation failed: at least 3 placement candidates are required, received {candidates.Count}.";
                return false;
            }

            if (rules == null)
                rules = new BDMiniBossPlanRules();

            rules.Normalize();

            List<BDMiniBossPlacementCandidate> legalCandidates = FilterLegalCandidates(candidates, rules);
            if (legalCandidates.Count < 3)
            {
                error = $"Mini-boss plan generation failed: only {legalCandidates.Count} legal candidates remain after distance/progression filtering.";
                return false;
            }

            System.Random random = new System.Random(seed);

            List<BDMiniBossArchetype> archetypes = new List<BDMiniBossArchetype>
            {
                BDMiniBossArchetype.SquareJumper,
                BDMiniBossArchetype.Roller,
                BDMiniBossArchetype.Serpent,
                BDMiniBossArchetype.QuadGunners
            };
            Shuffle(archetypes, random);

            BDMiniBossArchetype skippedArchetype = archetypes[3];
            List<BDMiniBossArchetype> selectedArchetypes = new List<BDMiniBossArchetype>
            {
                archetypes[0],
                archetypes[1],
                archetypes[2]
            };

            List<BDMiniBossRole> roles = new List<BDMiniBossRole>
            {
                BDMiniBossRole.GameBoyGuardian,
                BDMiniBossRole.GameCartridgeGuardian,
                BDMiniBossRole.PreBoss
            };
            Shuffle(roles, random);

            BDMiniBossPlacementPattern pattern = ChoosePattern(random, rules);

            for (int attempt = 0; attempt < rules.maxPlacementAttempts; attempt++)
            {
                BDMiniBossPlacementCandidate gameBoyCandidate = legalCandidates[random.Next(legalCandidates.Count)];
                BDMiniBossPlacementCandidate cartridgeCandidate = legalCandidates[random.Next(legalCandidates.Count)];
                BDMiniBossPlacementCandidate preBossCandidate = legalCandidates[random.Next(legalCandidates.Count)];

                if (!AreDistinct(gameBoyCandidate, cartridgeCandidate, preBossCandidate))
                    continue;

                if (!IsValidSecretCandidate(gameBoyCandidate, rules) ||
                    !IsValidSecretCandidate(cartridgeCandidate, rules) ||
                    !IsValidPreBossCandidate(preBossCandidate, rules))
                    continue;

                if (!HasRequiredSeparation(gameBoyCandidate, cartridgeCandidate, rules) ||
                    !HasRequiredSeparation(gameBoyCandidate, preBossCandidate, rules) ||
                    !HasRequiredSeparation(cartridgeCandidate, preBossCandidate, rules))
                    continue;

                if (!MatchesPattern(gameBoyCandidate, cartridgeCandidate, pattern, rules))
                    continue;

                float latestSecretProgression = Mathf.Max(gameBoyCandidate.Progression, cartridgeCandidate.Progression);
                if (preBossCandidate.Progression < latestSecretProgression + rules.minimumPreBossLead)
                    continue;

                Dictionary<BDMiniBossRole, BDMiniBossPlacementCandidate> placements =
                    new Dictionary<BDMiniBossRole, BDMiniBossPlacementCandidate>
                    {
                        { BDMiniBossRole.GameBoyGuardian, gameBoyCandidate },
                        { BDMiniBossRole.GameCartridgeGuardian, cartridgeCandidate },
                        { BDMiniBossRole.PreBoss, preBossCandidate }
                    };

                List<BDMiniBossAssignment> assignments = new List<BDMiniBossAssignment>(3);
                for (int i = 0; i < selectedArchetypes.Count; i++)
                {
                    BDMiniBossRole assignedRole = roles[i];
                    assignments.Add(new BDMiniBossAssignment(
                        selectedArchetypes[i],
                        assignedRole,
                        placements[assignedRole]
                    ));
                }

                plan = new BDMiniBossGeneratedPlan(seed, pattern, skippedArchetype, assignments);

                if (!ValidateGeneratedPlan(plan, out error))
                {
                    plan = null;
                    return false;
                }

                error = string.Empty;
                return true;
            }

            error = $"Mini-boss plan generation failed after {rules.maxPlacementAttempts} bounded placement attempts for seed {seed}.";
            return false;
        }

        public static BDMiniBossGeneratedPlan GenerateOrThrow(
            int seed,
            IReadOnlyList<BDMiniBossPlacementCandidate> candidates,
            BDMiniBossPlanRules rules = null)
        {
            if (TryGenerate(seed, candidates, rules, out BDMiniBossGeneratedPlan plan, out string error))
                return plan;

            throw new InvalidOperationException(error);
        }

        public static bool ValidateGeneratedPlan(BDMiniBossGeneratedPlan plan, out string error)
        {
            error = string.Empty;

            if (plan == null)
            {
                error = "Generated mini-boss plan is null.";
                return false;
            }

            if (plan.Assignments == null || plan.Assignments.Count != 3)
            {
                int count = plan.Assignments == null ? 0 : plan.Assignments.Count;
                error = $"Generated mini-boss plan must contain exactly 3 assignments, received {count}.";
                return false;
            }

            HashSet<BDMiniBossArchetype> archetypes = new HashSet<BDMiniBossArchetype>();
            HashSet<BDMiniBossRole> roles = new HashSet<BDMiniBossRole>();
            HashSet<Vector2Int> cells = new HashSet<Vector2Int>();

            for (int i = 0; i < plan.Assignments.Count; i++)
            {
                BDMiniBossAssignment assignment = plan.Assignments[i];

                if (!archetypes.Add(assignment.Archetype))
                {
                    error = $"Duplicate mini-boss archetype assignment detected: {assignment.Archetype}.";
                    return false;
                }

                if (!roles.Add(assignment.Role))
                {
                    error = $"Duplicate mini-boss role assignment detected: {assignment.Role}.";
                    return false;
                }

                if (!cells.Add(assignment.Cell))
                {
                    error = $"Duplicate mini-boss placement cell detected: {assignment.Cell}.";
                    return false;
                }
            }

            if (!roles.Contains(BDMiniBossRole.GameBoyGuardian) ||
                !roles.Contains(BDMiniBossRole.GameCartridgeGuardian) ||
                !roles.Contains(BDMiniBossRole.PreBoss))
            {
                error = "Generated mini-boss plan does not contain all three required roles.";
                return false;
            }

            if (archetypes.Contains(plan.SkippedArchetype))
            {
                error = $"Skipped archetype {plan.SkippedArchetype} also appears in the selected assignments.";
                return false;
            }

            return true;
        }

        private static List<BDMiniBossPlacementCandidate> FilterLegalCandidates(
            IReadOnlyList<BDMiniBossPlacementCandidate> candidates,
            BDMiniBossPlanRules rules)
        {
            List<BDMiniBossPlacementCandidate> result = new List<BDMiniBossPlacementCandidate>();

            for (int i = 0; i < candidates.Count; i++)
            {
                BDMiniBossPlacementCandidate candidate = candidates[i];

                if (candidate.DistanceFromEntrance < rules.minDistanceFromEntrance)
                    continue;

                if (candidate.DistanceFromExit < rules.minDistanceFromExit)
                    continue;

                result.Add(candidate);
            }

            return result;
        }

        private static bool IsValidSecretCandidate(
            BDMiniBossPlacementCandidate candidate,
            BDMiniBossPlanRules rules)
        {
            return candidate.Progression >= rules.secretMinProgression &&
                   candidate.Progression <= rules.secretMaxProgression;
        }

        private static bool IsValidPreBossCandidate(
            BDMiniBossPlacementCandidate candidate,
            BDMiniBossPlanRules rules)
        {
            return candidate.Progression >= rules.preBossMinProgression &&
                   candidate.Progression <= rules.preBossMaxProgression;
        }

        private static bool AreDistinct(
            BDMiniBossPlacementCandidate a,
            BDMiniBossPlacementCandidate b,
            BDMiniBossPlacementCandidate c)
        {
            return a.Cell != b.Cell && a.Cell != c.Cell && b.Cell != c.Cell;
        }

        private static bool HasRequiredSeparation(
            BDMiniBossPlacementCandidate a,
            BDMiniBossPlacementCandidate b,
            BDMiniBossPlanRules rules)
        {
            int manhattan = Mathf.Abs(a.Cell.x - b.Cell.x) + Mathf.Abs(a.Cell.y - b.Cell.y);
            return manhattan >= rules.minCandidateManhattanDistance;
        }

        private static bool MatchesPattern(
            BDMiniBossPlacementCandidate gameBoy,
            BDMiniBossPlacementCandidate cartridge,
            BDMiniBossPlacementPattern pattern,
            BDMiniBossPlanRules rules)
        {
            switch (pattern)
            {
                case BDMiniBossPlacementPattern.SequentialSecrets:
                    return Mathf.Abs(gameBoy.Progression - cartridge.Progression) >=
                           rules.minSequentialProgressionGap;

                case BDMiniBossPlacementPattern.ParallelBranches:
                    return Mathf.Abs(gameBoy.Cell.y - cartridge.Cell.y) <= rules.maxParallelYDelta &&
                           Mathf.Abs(gameBoy.Cell.x - cartridge.Cell.x) >= rules.minParallelXSeparation;

                default:
                    return true;
            }
        }

        private static BDMiniBossPlacementPattern ChoosePattern(
            System.Random random,
            BDMiniBossPlanRules rules)
        {
            double roll = random.NextDouble();

            if (roll < rules.parallelPatternChance)
                return BDMiniBossPlacementPattern.ParallelBranches;

            if (roll < rules.parallelPatternChance + rules.sequentialPatternChance)
                return BDMiniBossPlacementPattern.SequentialSecrets;

            return BDMiniBossPlacementPattern.MixedBranches;
        }

        private static void Shuffle<T>(IList<T> items, System.Random random)
        {
            for (int i = items.Count - 1; i > 0; i--)
            {
                int swapIndex = random.Next(i + 1);
                T value = items[i];
                items[i] = items[swapIndex];
                items[swapIndex] = value;
            }
        }
    }
}
