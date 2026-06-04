using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDExitPredictionSensor : MonoBehaviour
    {
        [SerializeField] private BDCombatRoom combatRoom;
        [SerializeField] private Transform exitAnchor;

        [Header("Prediction")]
        [SerializeField] private float fastApproachSpeed = 3.15f;
        [SerializeField] private float slowApproachSpeed = 0.55f;
        [SerializeField] private float nearExitDistance = 7.2f;
        [SerializeField] private float farExitDistance = 16.0f;
        [SerializeField] private float directionDotThreshold = 0.55f;
        [SerializeField] private float notifyCooldown = 0.08f;
        [SerializeField] private float logicTickInterval = 0.10f;

        private Transform player;
        private Vector3 lastPlayerPosition;
        private bool hasLastPosition;
        private float cooldown;
        private string lastReason = "none";
        private float logicTickTimer;

        public void Configure(BDCombatRoom room, Transform anchor)
        {
            combatRoom = room;
            exitAnchor = anchor;
        }

        private void Update()
        {
            if (cooldown > 0f)
                cooldown -= Time.deltaTime;

            if (player == null)
                player = BDTargetFinder.FindPlayer();

            if (player == null || combatRoom == null || exitAnchor == null)
                return;

            if (!combatRoom.CombatActivated && !combatRoom.PlayerInside)
                return;

            logicTickTimer -= Time.deltaTime;
            if (logicTickTimer > 0f)
                return;

            logicTickTimer = logicTickInterval;

            Vector3 current = player.position;

            if (!hasLastPosition)
            {
                lastPlayerPosition = current;
                hasLastPosition = true;
                return;
            }

            Vector3 delta = current - lastPlayerPosition;
            delta.y = 0f;
            lastPlayerPosition = current;

            float dt = Mathf.Max(Time.deltaTime, 0.0001f);
            float speed = delta.magnitude / dt;

            Vector3 toExit = exitAnchor.position - current;
            toExit.y = 0f;
            float distanceToExit = toExit.magnitude;

            if (toExit.sqrMagnitude < 0.001f)
                return;

            Vector3 moveDir = delta.sqrMagnitude > 0.001f ? delta.normalized : Vector3.zero;
            float dot = Vector3.Dot(moveDir, toExit.normalized);

            bool movingTowardExit = dot >= directionDotThreshold;
            bool fastTowardExit = movingTowardExit && speed >= fastApproachSpeed && distanceToExit <= farExitDistance;
            bool nearSlowTowardExit = movingTowardExit && speed >= slowApproachSpeed && distanceToExit <= nearExitDistance;

            if ((fastTowardExit || nearSlowTowardExit) && cooldown <= 0f)
            {
                cooldown = notifyCooldown;
                lastReason = fastTowardExit ? "fast exit approach" : "near slow exit approach";
                combatRoom.NotifyPredictedExitIntent(exitAnchor, lastReason, speed, distanceToExit);
            }
        }
    }
}
