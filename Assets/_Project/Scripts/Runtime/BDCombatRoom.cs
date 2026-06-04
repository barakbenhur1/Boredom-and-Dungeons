using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDCombatRoom : MonoBehaviour
    {
        [SerializeField] private List<BDHealth> enemies = new List<BDHealth>();
        [SerializeField] private List<BDPatrolExitBlockerBrain> patrolExitBlockers = new List<BDPatrolExitBlockerBrain>();
        [SerializeField] private List<BDEnemyExitInterference> genericInterferenceEnemies = new List<BDEnemyExitInterference>();
        [SerializeField] private List<BDEnemyTacticalCommand> tacticalAgents = new List<BDEnemyTacticalCommand>();
        [SerializeField] private List<Transform> exitAnchors = new List<Transform>();

        [Header("AI Director")]
        [SerializeField] private bool activateOnlyAfterPlayerEnters = true;
        [SerializeField] private float genericInterferenceDuration = 2.0f;
        [SerializeField] private float tacticalCommandDuration = 0.85f;
        [SerializeField] private float directorInterval = 0.62f;
        [SerializeField] private float exitPressureCooldown = 0.12f;
        [SerializeField] private int maxGenericInterferersPerExitAttempt = 1;
        [SerializeField] private int maxTacticalAgents = 1;
        [SerializeField] private bool showDebugOverlay = false;

        private bool playerInside;
        private bool combatActivated;
        private bool complete;
        private int liveEnemies;
        private float pressureCooldownTimer;
        private float directorTimer;
        private int exitPressureCount;
        private int nextGenericIndex;
        private int nextTacticalIndex;
        private string lastExitName = "none";
        private bool clearAnnounced;

        public bool PlayerInside => playerInside;
        public bool CombatActivated => combatActivated;
        public bool Complete => complete;
        public int LiveEnemies => liveEnemies;

        private void Start()
        {
            RefreshEnemyCount();
        }

        private void Update()
        {
            if (pressureCooldownTimer > 0f)
                pressureCooldownTimer -= Time.deltaTime;

            if (directorTimer > 0f)
                directorTimer -= Time.deltaTime;

            RefreshEnemyCount();

            if (liveEnemies <= 0)
            {
                combatActivated = false;

                if (!complete)
                {
                    complete = true;
                    AnnounceRoomClearOnce();
                }

                return;
            }

            if (combatActivated && playerInside && directorTimer <= 0f)
            {
                directorTimer = directorInterval;
                TickTacticalDirector();
            }
        }


        private void AnnounceRoomClearOnce()
        {
            if (clearAnnounced)
                return;

            clearAnnounced = true;
            BDCombatEventBus.NotifyRoomClear("Exit is open"); // no HUD text in V70
        }

        public void RegisterEnemy(BDHealth enemy)
        {
            if (enemy == null || enemies.Contains(enemy))
                return;

            enemies.Add(enemy);

            BDEnemyTacticalRole role = enemy.GetComponent<BDEnemyTacticalRole>();
            BDEnemyTacticalRoleType roleType = role != null ? role.Role : BDEnemyTacticalRoleType.Pressure;

            if (roleType == BDEnemyTacticalRoleType.ExitBlocker)
            {
                BDPatrolExitBlockerBrain blocker = enemy.GetComponent<BDPatrolExitBlockerBrain>();
                if (blocker == null)
                    blocker = enemy.gameObject.AddComponent<BDPatrolExitBlockerBrain>();

                RegisterPatrolExitBlocker(blocker);
            }
            else
            {
                BDEnemyExitInterference interference = enemy.GetComponent<BDEnemyExitInterference>();
                if (interference == null)
                    interference = enemy.gameObject.AddComponent<BDEnemyExitInterference>();

                RegisterGenericInterferenceEnemy(interference);

                if (CanUseTacticalDirector(roleType))
                {
                    BDEnemyTacticalCommand command = enemy.GetComponent<BDEnemyTacticalCommand>();
                    if (command == null)
                        command = enemy.gameObject.AddComponent<BDEnemyTacticalCommand>();

                    RegisterTacticalAgent(command);
                }
            }

            RefreshEnemyCount();
        }

        public void RegisterExitAnchor(Transform anchor)
        {
            if (anchor == null || exitAnchors.Contains(anchor))
                return;

            exitAnchors.Add(anchor);
        }

        public void RegisterPatrolExitBlocker(BDPatrolExitBlockerBrain blocker)
        {
            if (blocker == null || patrolExitBlockers.Contains(blocker))
                return;

            patrolExitBlockers.Add(blocker);
        }

        public void RegisterGenericInterferenceEnemy(BDEnemyExitInterference enemy)
        {
            if (enemy == null || genericInterferenceEnemies.Contains(enemy))
                return;

            genericInterferenceEnemies.Add(enemy);
        }

        public void RegisterTacticalAgent(BDEnemyTacticalCommand agent)
        {
            if (agent == null || tacticalAgents.Contains(agent))
                return;

            tacticalAgents.Add(agent);
        }

        public void NotifyPlayerNearExit(BDCombatRoomExitPressureZone zone, Transform exitAnchor)
        {
            HandleExitIntent(zone != null ? zone.name : "exit", exitAnchor);
        }

        public void NotifyExitIntent(Transform exitAnchor)
        {
            HandleExitIntent("exit", exitAnchor);
        }

        public void NotifyPredictedExitIntent(Transform exitAnchor, string reason, float speed, float distance)
        {
            if (exitAnchor == null)
                return;

            HandleExitIntent($"predicted {reason} speed={speed:0.0} distance={distance:0.0}", exitAnchor);
        }

        private bool CanUseTacticalDirector(BDEnemyTacticalRoleType role)
        {
            // V24: the director may guide simple pressure enemies, but it should not
            // overwrite the identity of jumpers, chargers, ranged enemies, or bomb layers.
            return role == BDEnemyTacticalRoleType.Pressure;
        }

        private void TickTacticalDirector()
        {
            Transform player = BDTargetFinder.FindPlayer();
            if (player == null)
                return;

            // Professional pass: normal director pressure should not constantly command
            // exit blocking. Exit blocking is handled by ExitIntent / ExitPrediction.
            AssignFlankingCommands();
        }

        private Transform FindNearestExitAnchor(Vector3 playerPosition)
        {
            Transform best = null;
            float bestDistance = float.MaxValue;

            foreach (Transform anchor in exitAnchors)
            {
                if (anchor == null)
                    continue;

                float distance = Vector3.Distance(playerPosition, anchor.position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = anchor;
                }
            }

            return best;
        }

        private void AssignFlankingCommands()
        {
            int assigned = 0;
            int attempts = 0;

            while (assigned < maxTacticalAgents && attempts < tacticalAgents.Count)
            {
                if (tacticalAgents.Count == 0)
                    break;

                int index = nextTacticalIndex % tacticalAgents.Count;
                nextTacticalIndex++;
                attempts++;

                BDEnemyTacticalCommand agent = tacticalAgents[index];
                if (agent == null)
                    continue;

                if (assigned == 0)
                    agent.CommandFlankLeft(tacticalCommandDuration);
                else if (assigned == 1)
                    agent.CommandFlankRight(tacticalCommandDuration);
                else
                    agent.CommandPressure(tacticalCommandDuration);

                assigned++;
            }
        }

        private void HandleExitIntent(string exitName, Transform exitAnchor)
        {
            if (complete || exitAnchor == null)
                return;

            RefreshEnemyCount();

            if (liveEnemies <= 0)
                return;

            combatActivated = true;

            // Exit intent should immediately refresh the patrol blocker target.
            foreach (BDPatrolExitBlockerBrain blocker in patrolExitBlockers)
            {
                if (blocker != null)
                    blocker.CommandBlockExit(exitAnchor);
            }

            if (pressureCooldownTimer > 0f)
                return;

            pressureCooldownTimer = exitPressureCooldown;
            exitPressureCount++;
            lastExitName = exitName;

            int commanded = 0;
            int attempts = 0;

            while (commanded < maxGenericInterferersPerExitAttempt && attempts < genericInterferenceEnemies.Count)
            {
                if (genericInterferenceEnemies.Count == 0)
                    break;

                int index = nextGenericIndex % genericInterferenceEnemies.Count;
                nextGenericIndex++;
                attempts++;

                BDEnemyExitInterference enemy = genericInterferenceEnemies[index];
                if (enemy == null)
                    continue;

                enemy.CommandInterfereWithExit(exitAnchor, genericInterferenceDuration);
                commanded++;
            }

            Debug.Log($"Combat room exit intent: {lastExitName}. Patrol blockers={patrolExitBlockers.Count}, generic commanded={commanded}.");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<BDPlayerMarker>() == null)
                return;

            playerInside = true;
            RefreshEnemyCount();

            if (liveEnemies > 0)
                combatActivated = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<BDPlayerMarker>() == null)
                return;

            playerInside = false;

            if (!activateOnlyAfterPlayerEnters)
                combatActivated = false;
        }

        private void RefreshEnemyCount()
        {
            int count = 0;

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                BDHealth enemy = enemies[i];

                if (enemy == null)
                {
                    enemies.RemoveAt(i);
                    continue;
                }

                if (!enemy.IsDead)
                    count++;
            }

            liveEnemies = count;
        }

        private void OnGUI()
        {
            if (!showDebugOverlay)
                return;

            GUI.Box(new Rect(Screen.width - 350, 205, 335, 176), "Combat Room — Tactical Director V21");
            GUI.Label(new Rect(Screen.width - 338, 235, 310, 22), $"Inside: {playerInside}");
            GUI.Label(new Rect(Screen.width - 338, 257, 310, 22), $"Activated: {combatActivated}");
            GUI.Label(new Rect(Screen.width - 338, 279, 310, 22), $"Live Enemies: {liveEnemies}");
            GUI.Label(new Rect(Screen.width - 338, 301, 310, 22), $"Patrol Blockers: {patrolExitBlockers.Count}");
            GUI.Label(new Rect(Screen.width - 338, 323, 310, 22), $"Tactical Agents: {tacticalAgents.Count}");
            GUI.Label(new Rect(Screen.width - 338, 345, 310, 22), $"Exit Anchors: {exitAnchors.Count}");
            GUI.Label(new Rect(Screen.width - 338, 367, 310, 22), $"Exit Pressures: {exitPressureCount}");
        }
    }
}
