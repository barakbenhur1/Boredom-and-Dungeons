using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    public enum BDMiniBossArchetype
    {
        SquareJumper = 0,
        Roller = 1,
        Serpent = 2,
        QuadGunners = 3
    }

    public enum BDMiniBossRole
    {
        GameBoyGuardian = 0,
        GameCartridgeGuardian = 1,
        PreBoss = 2
    }

    public enum BDMiniBossPlacementPattern
    {
        SequentialSecrets = 0,
        ParallelBranches = 1,
        MixedBranches = 2
    }

    [Serializable]
    public struct BDMiniBossPlacementCandidate
    {
        [SerializeField] private string candidateId;
        [SerializeField] private Vector2Int cell;
        [SerializeField] private Vector3 worldPosition;
        [SerializeField, Range(0f, 1f)] private float progression;
        [SerializeField] private int distanceFromEntrance;
        [SerializeField] private int distanceFromExit;

        public string CandidateId => candidateId;
        public Vector2Int Cell => cell;
        public Vector3 WorldPosition => worldPosition;
        public float Progression => progression;
        public int DistanceFromEntrance => distanceFromEntrance;
        public int DistanceFromExit => distanceFromExit;

        public BDMiniBossPlacementCandidate(
            string candidateId,
            Vector2Int cell,
            Vector3 worldPosition,
            float progression,
            int distanceFromEntrance,
            int distanceFromExit)
        {
            this.candidateId = string.IsNullOrWhiteSpace(candidateId)
                ? $"Cell_{cell.x}_{cell.y}"
                : candidateId.Trim();
            this.cell = cell;
            this.worldPosition = worldPosition;
            this.progression = Mathf.Clamp01(progression);
            this.distanceFromEntrance = Mathf.Max(0, distanceFromEntrance);
            this.distanceFromExit = Mathf.Max(0, distanceFromExit);
        }
    }

    [Serializable]
    public struct BDMiniBossAssignment
    {
        [SerializeField] private BDMiniBossArchetype archetype;
        [SerializeField] private BDMiniBossRole role;
        [SerializeField] private string candidateId;
        [SerializeField] private Vector2Int cell;
        [SerializeField] private Vector3 worldPosition;

        public BDMiniBossArchetype Archetype => archetype;
        public BDMiniBossRole Role => role;
        public string CandidateId => candidateId;
        public Vector2Int Cell => cell;
        public Vector3 WorldPosition => worldPosition;

        public BDMiniBossAssignment(
            BDMiniBossArchetype archetype,
            BDMiniBossRole role,
            BDMiniBossPlacementCandidate candidate)
        {
            this.archetype = archetype;
            this.role = role;
            candidateId = candidate.CandidateId;
            cell = candidate.Cell;
            worldPosition = candidate.WorldPosition;
        }
    }

    [Serializable]
    public sealed class BDMiniBossGeneratedPlan
    {
        [SerializeField] private int seed;
        [SerializeField] private BDMiniBossPlacementPattern placementPattern;
        [SerializeField] private BDMiniBossArchetype skippedArchetype;
        [SerializeField] private List<BDMiniBossAssignment> assignments = new List<BDMiniBossAssignment>();

        public int Seed => seed;
        public BDMiniBossPlacementPattern PlacementPattern => placementPattern;
        public BDMiniBossArchetype SkippedArchetype => skippedArchetype;
        public IReadOnlyList<BDMiniBossAssignment> Assignments => assignments;

        public BDMiniBossGeneratedPlan(
            int seed,
            BDMiniBossPlacementPattern placementPattern,
            BDMiniBossArchetype skippedArchetype,
            IEnumerable<BDMiniBossAssignment> assignments)
        {
            this.seed = seed;
            this.placementPattern = placementPattern;
            this.skippedArchetype = skippedArchetype;
            this.assignments = assignments == null
                ? new List<BDMiniBossAssignment>()
                : new List<BDMiniBossAssignment>(assignments);
        }

        public bool TryGetAssignment(BDMiniBossRole role, out BDMiniBossAssignment assignment)
        {
            for (int i = 0; i < assignments.Count; i++)
            {
                if (assignments[i].Role == role)
                {
                    assignment = assignments[i];
                    return true;
                }
            }

            assignment = default(BDMiniBossAssignment);
            return false;
        }
    }
}
