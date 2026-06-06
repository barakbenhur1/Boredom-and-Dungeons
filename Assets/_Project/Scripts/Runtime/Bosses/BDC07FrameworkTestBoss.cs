using System.Collections;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDHealth))]
    [RequireComponent(typeof(BDBossHealthChannel))]
    [RequireComponent(typeof(BDBossHealthDamageBridge))]
    public sealed class BDC07FrameworkTestBoss : MonoBehaviour
    {
        [Header("Framework")]
        [SerializeField] private BDBossEncounterController encounter;

        [Header("Arena")]
        [SerializeField] private Vector3 arenaCenter;
        [SerializeField] private float arenaRadius = 5.5f;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2.15f;
        [SerializeField] private float stopDistance = 2.15f;
        [SerializeField] private float rotationSpeed = 540f;
        [SerializeField] private float gravity = -24f;

        [Header("Telegraphed Test Attack")]
        [SerializeField] private float attackRange = 2.65f;
        [SerializeField] private float attackDamage = 12f;
        [SerializeField] private float attackWindup = 0.80f;
        [SerializeField] private float attackRecovery = 1.15f;

        [Header("Visuals")]
        [SerializeField] private Transform attackTelegraph;

        private CharacterController controller;
        private BDHealth ownHealth;
        private Transform target;
        private BDHealth targetHealth;
        private Renderer telegraphRenderer;
        private Renderer[] bodyRenderers;
        private Coroutine attackRoutine;
        private float verticalVelocity;
        private float nextAttackAt;
        private MaterialPropertyBlock propertyBlock;

        public bool IsAttacking => attackRoutine != null;
        public Vector3 ArenaCenter => arenaCenter;
        public float ArenaRadius => arenaRadius;

        public void Configure(
            BDBossEncounterController encounterController,
            Vector3 center,
            float radius)
        {
            encounter = encounterController;
            arenaCenter = center;
            arenaRadius = Mathf.Max(2.5f, radius);
        }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            ownHealth = GetComponent<BDHealth>();
            bodyRenderers = GetComponentsInChildren<Renderer>(true);
            propertyBlock = new MaterialPropertyBlock();

            if (attackTelegraph == null)
            {
                Transform found = transform.Find("AttackTelegraph");

                if (found != null)
                    attackTelegraph = found;
            }

            if (attackTelegraph != null)
            {
                telegraphRenderer =
                    attackTelegraph.GetComponent<Renderer>();

                attackTelegraph.gameObject.SetActive(false);
            }

            ResolveEncounter();
            ResolveTarget();
            ApplyBossColor(new Color(0.18f, 0.34f, 0.82f, 1f));
        }

        private void OnEnable()
        {
            ResolveEncounter();

            if (encounter != null)
            {
                encounter.StateChanged -= HandleEncounterStateChanged;
                encounter.StateChanged += HandleEncounterStateChanged;
            }
        }

        private void OnDisable()
        {
            if (encounter != null)
            {
                encounter.StateChanged -= HandleEncounterStateChanged;
            }

            CancelAttack();
        }

        private void Update()
        {
            ResolveEncounter();
            ResolveTarget();

            if (ownHealth == null || ownHealth.IsDead)
            {
                CancelAttack();
                return;
            }

            if (targetHealth != null && targetHealth.IsDead)
            {
                if (encounter != null && !encounter.IsFinished)
                    encounter.FailEncounter();

                CancelAttack();
                return;
            }

            if (encounter == null || !encounter.IsCombatActive)
            {
                ApplyGravityOnly();
                return;
            }

            if (target == null)
            {
                ApplyGravityOnly();
                return;
            }

            Vector3 planarDelta =
                target.position - transform.position;

            planarDelta.y = 0f;
            float distance = planarDelta.magnitude;

            if (attackRoutine != null)
            {
                ApplyGravityOnly();
                return;
            }

            FaceDirection(planarDelta);

            if (distance > stopDistance)
                MoveToward(planarDelta.normalized);
            else
                ApplyGravityOnly();

            if (distance <= attackRange &&
                Time.time >= nextAttackAt)
            {
                attackRoutine = StartCoroutine(
                    AttackRoutine()
                );
            }
        }

        private IEnumerator AttackRoutine()
        {
            SetTelegraphVisible(true);

            float duration = Mathf.Max(0.08f, attackWindup);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (encounter == null ||
                    !encounter.IsCombatActive ||
                    ownHealth == null ||
                    ownHealth.IsDead)
                {
                    SetTelegraphVisible(false);
                    attackRoutine = null;
                    yield break;
                }

                elapsed += Time.deltaTime;
                float progress =
                    Mathf.Clamp01(elapsed / duration);

                UpdateTelegraph(progress);
                yield return null;
            }

            ResolveTarget();

            if (target != null &&
                targetHealth != null &&
                !targetHealth.IsDead)
            {
                Vector3 delta =
                    target.position - transform.position;

                delta.y = 0f;

                if (delta.magnitude <= attackRange + 0.45f)
                {
                    targetHealth.ApplyDamage(
                        Mathf.Max(1f, attackDamage)
                    );
                }
            }

            SetTelegraphVisible(false);

            nextAttackAt =
                Time.time +
                Mathf.Max(0.10f, attackRecovery);

            attackRoutine = null;
        }

        private void MoveToward(Vector3 direction)
        {
            if (controller == null || !controller.enabled)
                return;

            Vector3 next =
                transform.position +
                direction *
                Mathf.Max(0f, moveSpeed) *
                Time.deltaTime;

            Vector3 fromCenter = next - arenaCenter;
            fromCenter.y = 0f;

            float safeRadius =
                Mathf.Max(2.5f, arenaRadius);

            if (fromCenter.sqrMagnitude >
                safeRadius * safeRadius)
            {
                Vector3 clamped =
                    fromCenter.normalized * safeRadius;

                next.x = arenaCenter.x + clamped.x;
                next.z = arenaCenter.z + clamped.z;
            }

            if (controller.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f;
            else
                verticalVelocity += gravity * Time.deltaTime;

            Vector3 motion = next - transform.position;
            motion.y = verticalVelocity * Time.deltaTime;
            controller.Move(motion);
        }

        private void ApplyGravityOnly()
        {
            if (controller == null || !controller.enabled)
                return;

            if (controller.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f;
            else
                verticalVelocity += gravity * Time.deltaTime;

            controller.Move(
                Vector3.up *
                verticalVelocity *
                Time.deltaTime
            );
        }

        private void FaceDirection(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation =
                Quaternion.LookRotation(
                    direction.normalized,
                    Vector3.up
                );

            transform.rotation =
                Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    Mathf.Max(1f, rotationSpeed) *
                    Time.deltaTime
                );
        }

        private void ResolveEncounter()
        {
            if (encounter == null)
            {
                encounter =
                    GetComponentInParent<
                        BDBossEncounterController>();
            }
        }

        private void ResolveTarget()
        {
            if (target != null && targetHealth != null)
                return;

            BDPlayerMarker marker =
                Object.FindFirstObjectByType<
                    BDPlayerMarker>();

            target = marker != null
                ? marker.transform
                : BDTargetFinder.FindPlayer();

            targetHealth = target != null
                ? target.GetComponent<BDHealth>()
                : null;
        }

        private void HandleEncounterStateChanged(
            BDBossEncounterState state)
        {
            if (state != BDBossEncounterState.Active)
                CancelAttack();

            if (state == BDBossEncounterState.Victory ||
                state == BDBossEncounterState.Completed)
            {
                ApplyBossColor(
                    new Color(0.18f, 0.72f, 0.34f, 1f)
                );
            }
            else if (state == BDBossEncounterState.Failed)
            {
                ApplyBossColor(
                    new Color(0.45f, 0.45f, 0.45f, 1f)
                );
            }
        }

        private void CancelAttack()
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }

            SetTelegraphVisible(false);
        }

        private void SetTelegraphVisible(bool visible)
        {
            if (attackTelegraph == null)
                return;

            attackTelegraph.gameObject.SetActive(visible);

            if (visible)
                UpdateTelegraph(0f);
        }

        private void UpdateTelegraph(float progress)
        {
            if (attackTelegraph == null)
                return;

            float scale = Mathf.Lerp(
                0.35f,
                1f,
                progress
            );

            attackTelegraph.localScale =
                new Vector3(
                    attackRange * 0.82f * scale,
                    0.035f,
                    attackRange * 0.82f * scale
                );

            if (telegraphRenderer == null)
                return;

            Color color = Color.Lerp(
                new Color(1f, 0.78f, 0.10f, 0.72f),
                new Color(1f, 0.08f, 0.04f, 0.90f),
                progress
            );

            telegraphRenderer.GetPropertyBlock(
                propertyBlock
            );

            propertyBlock.SetColor(
                "_BaseColor",
                color
            );

            propertyBlock.SetColor(
                "_Color",
                color
            );

            telegraphRenderer.SetPropertyBlock(
                propertyBlock
            );
        }

        private void ApplyBossColor(Color color)
        {
            if (bodyRenderers == null)
                return;

            for (int index = 0;
                 index < bodyRenderers.Length;
                 index++)
            {
                Renderer current = bodyRenderers[index];

                if (current == null ||
                    current == telegraphRenderer)
                {
                    continue;
                }

                current.GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor("_BaseColor", color);
                propertyBlock.SetColor("_Color", color);
                current.SetPropertyBlock(propertyBlock);
            }
        }
    }
}
