using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class BDEnemyGroundStick : MonoBehaviour
    {
        [SerializeField] private bool enabledStick = true;
        [SerializeField] private float rayStartHeight = 3.5f;
        [SerializeField] private float rayDistance = 8f;
        [SerializeField] private float snapSpeed = 18f;
        [SerializeField] private float maxSnapDistancePerFrame = 0.65f;
        [SerializeField] private float desiredBottomOffset = 0.02f;
        [SerializeField] private float maxAllowedAboveGround = 0.25f;
        [SerializeField] private float wallAvoidRadius = 0.72f;
        [SerializeField] private float stickCheckInterval = 0.12f;

        private CharacterController controller;
        private BDHitStaggerReceiver hitStaggerReceiver;
        private float disabledUntil;
        private float nextStickCheckTime;
        private static readonly RaycastHit[] GroundHits = new RaycastHit[12];
        private static readonly Collider[] WallHits = new Collider[12];

        public bool IsTemporarilyDisabled => Time.time < disabledUntil;

        private void Awake()
        {
            hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();
            controller = GetComponent<CharacterController>();
        }

        private void LateUpdate()
        {
            if (!enabledStick || IsTemporarilyDisabled)
                return;

            if (Time.time < nextStickCheckTime)
                return;

            nextStickCheckTime = Time.time + stickCheckInterval;

            StickToGroundIfNeeded();
            PushAwayFromWallsIfEmbedded();
        }

        public void DisableFor(float seconds)
        {
            disabledUntil = Mathf.Max(disabledUntil, Time.time + Mathf.Max(0f, seconds));
        }

        public void ForceSnapNow()
        {
            disabledUntil = 0f;
            nextStickCheckTime = 0f;
            StickToGroundIfNeeded(force: true);
            PushAwayFromWallsIfEmbedded();
        }

        private void StickToGroundIfNeeded(bool force = false)
        {
            if (controller == null)
                return;

            if (!TryFindGround(out RaycastHit groundHit))
                return;

            Vector3 desiredRoot =
                BDSafeEnemyPlacement.ResolveRootPositionForGround(
                    groundHit.point,
                    transform,
                    controller,
                    desiredBottomOffset
                );
            float deltaY = desiredRoot.y - transform.position.y;

            if (!force && Mathf.Abs(deltaY) <= maxAllowedAboveGround)
                return;

            float maxStep = force
                ? Mathf.Abs(deltaY)
                : Mathf.Min(Mathf.Abs(deltaY), Mathf.Min(maxSnapDistancePerFrame, snapSpeed * Time.deltaTime));

            Vector3 move =
                Vector3.up *
                Mathf.Sign(deltaY) *
                maxStep;

            // A forced grounding repair is structural, not authored enemy
            // locomotion, and must not be damped by hit stagger.
            controller.Move(
                force
                    ? move
                    : FilterMoveByHitStagger(move)
            );
        }

        public bool TryFindGround(out RaycastHit groundHit)
        {
            Vector3 origin = transform.position + Vector3.up * rayStartHeight;
            int hitCount = Physics.RaycastNonAlloc(origin, Vector3.down, GroundHits, rayDistance, ~0, QueryTriggerInteraction.Ignore);
            SortHitsByDistance(GroundHits, hitCount);

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = GroundHits[i];

                if (hit.collider == null)
                    continue;

                if (hit.collider.GetComponentInParent<BDOccludingWall>() != null)
                    continue;

                if (hit.collider.GetComponentInParent<BDHealth>() != null)
                    continue;

                if (hit.collider.GetComponentInParent<BDHorseHealth>() != null)
                    continue;

                // Ground cubes and floor tint markers are valid ground.
                if (hit.normal.y >= 0.65f)
                {
                    groundHit = hit;
                    return true;
                }
            }

            groundHit = default;
            return false;
        }


        private static void SortHitsByDistance(RaycastHit[] hits, int count)
        {
            for (int i = 1; i < count; i++)
            {
                RaycastHit key = hits[i];
                float distance = key.distance;
                int j = i - 1;

                while (j >= 0 && hits[j].distance > distance)
                {
                    hits[j + 1] = hits[j];
                    j--;
                }

                hits[j + 1] = key;
            }
        }

        private void PushAwayFromWallsIfEmbedded()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, wallAvoidRadius, WallHits, ~0, QueryTriggerInteraction.Ignore);

            Vector3 push = Vector3.zero;

            for (int i = 0; i < hitCount; i++)
            {
                Collider hit = WallHits[i];
                if (hit == null)
                    continue;

                if (hit.GetComponentInParent<BDOccludingWall>() == null && !hit.name.Contains("Wall"))
                    continue;

                Vector3 away = transform.position - hit.ClosestPoint(transform.position);
                away.y = 0f;

                if (away.sqrMagnitude < 0.001f)
                    away = transform.position - hit.transform.position;

                away.y = 0f;

                if (away.sqrMagnitude > 0.001f)
                    push += away.normalized;
            }

            if (push.sqrMagnitude > 0.001f)
                controller.Move(BDEnemyHazardNavigation.FilterBrainMotion(this, FilterMoveByHitStagger(push.normalized * 1.25f * Time.deltaTime)));
        }
        private Vector3 FilterMoveByHitStagger(Vector3 move)
        {
            if (hitStaggerReceiver == null)
                hitStaggerReceiver = GetComponent<BDHitStaggerReceiver>();

            return hitStaggerReceiver != null ? hitStaggerReceiver.FilterMove(move) : move;
        }


    }
}
