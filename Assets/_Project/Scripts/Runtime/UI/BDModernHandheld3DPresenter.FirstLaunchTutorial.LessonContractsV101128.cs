using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        // BD ATOMIC SPIN + ENVIRONMENTAL COMPLETION + PLAYER MODEL V10.11.28
        private const float TutorialSpinPairRadiusV101128 = 210f;
        // BD ATOMIC SPIN IMPACT ALIGNMENT V10.11.30.37
        private const float TutorialSpinPairOffsetV101128 = 82f;
        private const float TutorialDodgeObstacleHalfWidthV101128 = 48f;
        private const float TutorialDodgeExitClearanceV101128 = 58f;

        private TutorialEnemyActor firstLaunchTutorialSpinFrontTargetV101128;
        private TutorialEnemyActor firstLaunchTutorialSpinRearTargetV101128;
        private bool firstLaunchTutorialSpinPairArmedV101128;
        private bool firstLaunchTutorialSpinPairResolvedV101128;

        private bool firstLaunchTutorialDodgeTraversalPendingV101128;
        private float firstLaunchTutorialDodgeStartXV101128;
        private float firstLaunchTutorialDodgeTargetXV101128;
        private float firstLaunchTutorialDodgeObstacleXV101128;
        private int firstLaunchTutorialDodgeDirectionV101128;

        private RectTransform firstLaunchTutorialPlayerModelRootV101128;
        private CanvasGroup firstLaunchTutorialPlayerModelCanvasV101128;
        private RectTransform firstLaunchTutorialPlayerLeftUpperArmV101128;
        private RectTransform firstLaunchTutorialPlayerRightUpperArmV101128;
        private RectTransform firstLaunchTutorialPlayerLeftUpperLegV101128;
        private RectTransform firstLaunchTutorialPlayerRightUpperLegV101128;

        private void ResetFirstLaunchTutorialAtomicLessonStateV101128(
            FirstLaunchTutorialStep step)
        {
            firstLaunchTutorialDodgeTraversalPendingV101128 = false;
            firstLaunchTutorialDodgeStartXV101128 = 0f;
            firstLaunchTutorialDodgeTargetXV101128 = 0f;
            firstLaunchTutorialDodgeObstacleXV101128 = 0f;
            firstLaunchTutorialDodgeDirectionV101128 = 0;

            firstLaunchTutorialSpinPairArmedV101128 = false;
            firstLaunchTutorialSpinPairResolvedV101128 = false;
            firstLaunchTutorialSpinFrontTargetV101128 = null;
            firstLaunchTutorialSpinRearTargetV101128 = null;

            if (step != FirstLaunchTutorialStep.SpinAttack)
                return;

            TutorialEnemyActor secondary =
                FindFirstLaunchTutorialActorByImageV101128(
                    firstLaunchTutorialEnemySecondary
                );
            if (secondary != null)
            {
                secondary.Active = false;
                secondary.Dead = true;
                secondary.Health = 0f;
                secondary.AttackCommitted = false;
                secondary.DamageApplied = false;
                if (secondary.Image != null)
                    secondary.Image.gameObject.SetActive(false);
            }
        }

        private void UpdateFirstLaunchTutorialAtomicLessonContractsV101128()
        {
            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.SpinAttack)
                UpdateFirstLaunchTutorialSpinPairV101128();
                    // BD FOCUSED LESSON ACTOR FRAME SYNC V10.11.30
            UpdateFirstLaunchTutorialFocusedLessonTargetV101130();
}

        private TutorialEnemyActor FindFirstLaunchTutorialActorByImageV101128(
            Image image)
        {
            if (image == null)
                return null;
            for (int index = 0;
                 index < firstLaunchTutorialActors.Count;
                 index++)
            {
                TutorialEnemyActor actor = firstLaunchTutorialActors[index];
                if (actor != null && actor.Image == image)
                    return actor;
            }
            return null;
        }

        private void UpdateFirstLaunchTutorialSpinPairV101128()
        {
            if (firstLaunchTutorialSpinPairResolvedV101128)
                return;

            TutorialEnemyActor front =
                FindFirstLaunchTutorialActorByImageV101128(
                    firstLaunchTutorialEnemy
                );
            if (front == null || !front.Active || front.Dead)
            {
                SpawnTutorialActor(
                    firstLaunchTutorialEnemy,
                    TutorialEnemyRole.Small,
                    firstLaunchTutorialPlayerWorldPosition.x +
                        TutorialSpinPairOffsetV101128,
                    1f
                );
                front = FindFirstLaunchTutorialActorByImageV101128(
                    firstLaunchTutorialEnemy
                );
            }
            if (front == null)
                return;

            float distance = Mathf.Abs(
                front.Position.x -
                firstLaunchTutorialPlayerWorldPosition.x
            );
            if (!firstLaunchTutorialSpinPairArmedV101128 &&
                distance > TutorialInstructionTriggerRange + 48f)
            {
                return;
            }

            if (!firstLaunchTutorialSpinPairArmedV101128)
            {
                float facing =
                    Mathf.Abs(firstLaunchTutorialLastMoveDirection.x) > 0.01f
                        ? Mathf.Sign(firstLaunchTutorialLastMoveDirection.x)
                        : 1f;
                float frontX =
                    firstLaunchTutorialPlayerWorldPosition.x +
                    facing * TutorialSpinPairOffsetV101128;
                float rearX =
                    firstLaunchTutorialPlayerWorldPosition.x -
                    facing * TutorialSpinPairOffsetV101128;

                SpawnTutorialActor(
                    firstLaunchTutorialEnemy,
                    TutorialEnemyRole.Small,
                    frontX,
                    1f
                );
                SpawnTutorialActor(
                    firstLaunchTutorialEnemySecondary,
                    TutorialEnemyRole.Small,
                    rearX,
                    1f
                );
                front = FindFirstLaunchTutorialActorByImageV101128(
                    firstLaunchTutorialEnemy
                );
                TutorialEnemyActor rear =
                    FindFirstLaunchTutorialActorByImageV101128(
                        firstLaunchTutorialEnemySecondary
                    );
                firstLaunchTutorialSpinFrontTargetV101128 = front;
                firstLaunchTutorialSpinRearTargetV101128 = rear;
                firstLaunchTutorialSpinPairArmedV101128 =
                    front != null && rear != null;

                PrepareFirstLaunchTutorialSpinActorV101128(front);
                PrepareFirstLaunchTutorialSpinActorV101128(rear);
                SetFirstLaunchTutorialInstructionRequested(true);
                UpdateFirstLaunchTutorialPrompt();
                ShowFirstLaunchTutorialSuccess(
                    "ENEMY BEHIND YOU — HIT BOTH WITH ONE SPIN"
                );
                return;
            }

            firstLaunchTutorialSpinFrontTargetV101128 = front;
            firstLaunchTutorialSpinRearTargetV101128 =
                FindFirstLaunchTutorialActorByImageV101128(
                    firstLaunchTutorialEnemySecondary
                );
            EnsureFirstLaunchTutorialSpinActorAliveV101128(
                firstLaunchTutorialSpinFrontTargetV101128
            );
            EnsureFirstLaunchTutorialSpinActorAliveV101128(
                firstLaunchTutorialSpinRearTargetV101128
            );
        }

        private static void PrepareFirstLaunchTutorialSpinActorV101128(
            TutorialEnemyActor actor)
        {
            if (actor == null)
                return;
            actor.MaximumHealth = 1f;
            actor.Health = 1f;
            actor.Active = true;
            actor.Dead = false;
            actor.AttackCommitted = false;
            actor.DamageApplied = false;
            actor.NextActionAt = float.PositiveInfinity;
            if (actor.Image != null)
                actor.Image.gameObject.SetActive(true);
        }

        private static void EnsureFirstLaunchTutorialSpinActorAliveV101128(
            TutorialEnemyActor actor)
        {
            if (actor == null)
                return;
            if (!actor.Active || actor.Dead || actor.Health <= 0f)
            {
                actor.Health = Mathf.Max(1f, actor.MaximumHealth);
                actor.Active = true;
                actor.Dead = false;
                actor.AttackCommitted = false;
                actor.DamageApplied = false;
                if (actor.Image != null)
                    actor.Image.gameObject.SetActive(true);
            }
            actor.NextActionAt = float.PositiveInfinity;
        }

        private bool TryCompleteFirstLaunchTutorialAtomicSpinV101128()
        {
            if (firstLaunchTutorialStep != FirstLaunchTutorialStep.SpinAttack)
                return false;

            UpdateFirstLaunchTutorialSpinPairV101128();
            TutorialEnemyActor front =
                firstLaunchTutorialSpinFrontTargetV101128;
            TutorialEnemyActor rear =
                firstLaunchTutorialSpinRearTargetV101128;
            bool validPair =
                firstLaunchTutorialSpinPairArmedV101128 &&
                front != null && rear != null && front != rear &&
                front.Active && rear.Active &&
                !front.Dead && !rear.Dead;
            bool frontInRange = validPair && Vector2.Distance(
                front.Position,
                firstLaunchTutorialPlayerWorldPosition
            ) <= TutorialSpinPairRadiusV101128;
            bool rearInRange = validPair && Vector2.Distance(
                rear.Position,
                firstLaunchTutorialPlayerWorldPosition
            ) <= TutorialSpinPairRadiusV101128;

            // BD ATOMIC SPIN ALL-OR-NOTHING V10.11.28
            // A spin that reaches only one target is a failed lesson attempt.
            // Neither target loses health unless the same spin reaches both.
            if (!frontInRange || !rearInRange)
            {
                EnsureFirstLaunchTutorialSpinActorAliveV101128(front);
                EnsureFirstLaunchTutorialSpinActorAliveV101128(rear);
                firstLaunchTutorialLessonHitConfirmedV101125 = false;
                ShowFirstLaunchTutorialSuccess(
                    "ONE SPIN MUST HIT BOTH — NEITHER WAS HURT"
                );
                return false;
            }

            ForceFirstLaunchTutorialLessonActorDeathV101128(front);
            ForceFirstLaunchTutorialLessonActorDeathV101128(rear);
            bool bothKilled = front.Dead && rear.Dead &&
                              !front.Active && !rear.Active;
            if (!bothKilled)
            {
                RestoreFirstLaunchTutorialSpinActorV101128(front, 1f);
                RestoreFirstLaunchTutorialSpinActorV101128(rear, 1f);
                firstLaunchTutorialLessonHitConfirmedV101125 = false;
                ShowFirstLaunchTutorialSuccess(
                    "BOTH MUST FALL TO THE SAME SPIN"
                );
                return false;
            }

            firstLaunchTutorialLessonHitConfirmedV101125 = true;
            firstLaunchTutorialSpinPairResolvedV101128 = true;
            SetFirstLaunchTutorialLearningState(
                "Spin",
                TutorialLearningState.Demonstrated
            );
            ShowFirstLaunchTutorialSuccess("BOTH ENEMIES CLEARED TOGETHER");
            SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.Grapple);
            return true;
        }


        private static void RestoreFirstLaunchTutorialSpinActorV101128(
            TutorialEnemyActor actor,
            float health)
        {
            if (actor == null)
                return;
            actor.Health = Mathf.Max(1f, health);
            actor.MaximumHealth = Mathf.Max(actor.MaximumHealth, actor.Health);
            actor.Active = true;
            actor.Dead = false;
            actor.AttackCommitted = false;
            actor.DamageApplied = false;
            actor.NextActionAt = float.PositiveInfinity;
            if (actor.Image != null)
                actor.Image.gameObject.SetActive(true);
        }

        private void BeginFirstLaunchTutorialDodgeTraversalV101128()
        {
            float obstacleX = firstLaunchTutorialHazardWorldPosition.x;
            float delta = obstacleX - firstLaunchTutorialPlayerWorldPosition.x;
            int direction = delta >= 0f ? 1 : -1;
            firstLaunchTutorialDodgeTraversalPendingV101128 = true;
            firstLaunchTutorialDodgeStartXV101128 =
                firstLaunchTutorialPlayerWorldPosition.x;
            firstLaunchTutorialDodgeObstacleXV101128 = obstacleX;
            firstLaunchTutorialDodgeDirectionV101128 = direction;
            firstLaunchTutorialDodgeTargetXV101128 =
                obstacleX + direction *
                (TutorialDodgeObstacleHalfWidthV101128 +
                 TutorialDodgeExitClearanceV101128);
        }

        private Vector2 ResolveFirstLaunchTutorialDodgeTargetV101128(
            Vector2 defaultTarget)
        {
            if (firstLaunchTutorialStep != FirstLaunchTutorialStep.Dodge ||
                !firstLaunchTutorialDodgeTraversalPendingV101128)
            {
                return defaultTarget;
            }
            return new Vector2(
                firstLaunchTutorialDodgeTargetXV101128,
                firstLaunchTutorialPlayerWorldPosition.y
            );
        }

        private bool CompleteFirstLaunchTutorialDodgeTraversalV101128()
        {
            if (!firstLaunchTutorialDodgeTraversalPendingV101128 ||
                firstLaunchTutorialStep != FirstLaunchTutorialStep.Dodge)
            {
                return false;
            }

            float endX = firstLaunchTutorialPlayerWorldPosition.x;
            bool crossedCenter =
                Mathf.Min(firstLaunchTutorialDodgeStartXV101128, endX) <=
                    firstLaunchTutorialDodgeObstacleXV101128 &&
                Mathf.Max(firstLaunchTutorialDodgeStartXV101128, endX) >=
                    firstLaunchTutorialDodgeObstacleXV101128;
            bool clearedFarSide = firstLaunchTutorialDodgeDirectionV101128 > 0
                ? endX >= firstLaunchTutorialDodgeTargetXV101128 - 2f
                : endX <= firstLaunchTutorialDodgeTargetXV101128 + 2f;

            firstLaunchTutorialDodgeTraversalPendingV101128 = false;
            if (!crossedCenter || !clearedFarSide)
            {
                ShowFirstLaunchTutorialSuccess(
                    "DODGE THROUGH TO THE OTHER SIDE"
                );
                return false;
            }

            SetFirstLaunchTutorialLearningState(
                "Dodge",
                TutorialLearningState.Demonstrated
            );
            ShowFirstLaunchTutorialSuccess("OBSTACLE CLEARED BY DODGE");
            SetFirstLaunchTutorialStep(FirstLaunchTutorialStep.Parry);
            return true;
        }

        private void EnsureFirstLaunchTutorialPlayerModelV101128()
        {
            if (firstLaunchTutorialPlayer == null)
                return;

            // BD SIMPLE DIRECTIONAL PLAYER MODEL V10.11.30
            string[] oldNames =
            {
                "BD Player Model V10.11.28",
                "BD Player Model V10.11.29",
                "BD Player Model V10.11.30"
            };
            for (int index = 0; index < oldNames.Length; index++)
            {
                Transform oldModel =
                    firstLaunchTutorialPlayer.rectTransform.Find(oldNames[index]);
                if (oldModel == null)
                    continue;
                if (oldNames[index] == "BD Player Model V10.11.30")
                {
                    firstLaunchTutorialPlayerModelRootV101128 =
                        oldModel as RectTransform;
                    firstLaunchTutorialPlayerLeftUpperArmV101128 =
                        oldModel.Find("Back Arm") as RectTransform;
                    firstLaunchTutorialPlayerRightUpperArmV101128 =
                        oldModel.Find("Front Arm") as RectTransform;
                    firstLaunchTutorialPlayerLeftUpperLegV101128 =
                        oldModel.Find("Back Leg") as RectTransform;
                    firstLaunchTutorialPlayerRightUpperLegV101128 =
                        oldModel.Find("Front Leg") as RectTransform;
                    firstLaunchTutorialPlayer.enabled = false;
                    return;
                }
                oldModel.gameObject.SetActive(false);
                Object.Destroy(oldModel.gameObject);
            }

            firstLaunchTutorialPlayer.enabled = false;
            firstLaunchTutorialPlayerModelCanvasV101128 =
                firstLaunchTutorialPlayer.GetComponent<CanvasGroup>();
            if (firstLaunchTutorialPlayerModelCanvasV101128 == null)
            {
                firstLaunchTutorialPlayerModelCanvasV101128 =
                    firstLaunchTutorialPlayer.gameObject.AddComponent<CanvasGroup>();
            }

            GameObject rootObject = new GameObject(
                "BD Player Model V10.11.30",
                typeof(RectTransform)
            );
            rootObject.layer = firstLaunchTutorialPlayer.gameObject.layer;
            rootObject.transform.SetParent(
                firstLaunchTutorialPlayer.rectTransform,
                false
            );
            firstLaunchTutorialPlayerModelRootV101128 =
                rootObject.GetComponent<RectTransform>();
            ConfigureFirstLaunchTutorialPlayerPartRectV101128(
                firstLaunchTutorialPlayerModelRootV101128,
                Vector2.zero,
                new Vector2(40f, 70f),
                new Vector2(0.5f, 0.5f)
            );

            Color outline = new Color(0.035f, 0.045f, 0.080f, 1f);
            Color skin = new Color(0.94f, 0.70f, 0.48f, 1f);
            Color hair = new Color(0.96f, 0.68f, 0.10f, 1f);
            Color shirt = new Color(0.80f, 0.10f, 0.15f, 1f);
            Color shirtBack = new Color(0.58f, 0.08f, 0.12f, 1f);
            Color trousers = new Color(0.10f, 0.32f, 0.78f, 1f);
            Color trousersBack = new Color(0.07f, 0.22f, 0.58f, 1f);

            firstLaunchTutorialPlayerLeftUpperArmV101128 =
                CreateFirstLaunchTutorialPlayerJointV101128(
                    firstLaunchTutorialPlayerModelRootV101128,
                    "Back Arm",
                    new Vector2(-11f, 6f),
                    new Vector2(5f, 24f),
                    new Vector2(0.5f, 0.82f),
                    shirtBack,
                    Color.clear
                );
            firstLaunchTutorialPlayerLeftUpperLegV101128 =
                CreateFirstLaunchTutorialPlayerJointV101128(
                    firstLaunchTutorialPlayerModelRootV101128,
                    "Back Leg",
                    new Vector2(-5f, -15f),
                    new Vector2(7f, 27f),
                    new Vector2(0.5f, 0.84f),
                    trousersBack,
                    Color.clear
                );
            CreateFirstLaunchTutorialPlayerPartV101128(
                firstLaunchTutorialPlayerModelRootV101128,
                "Body",
                new Vector2(0f, 1f),
                new Vector2(21f, 29f),
                shirt,
                outline
            );
            CreateFirstLaunchTutorialPlayerPartV101128(
                firstLaunchTutorialPlayerModelRootV101128,
                "Head",
                new Vector2(0f, 26f),
                new Vector2(19f, 19f),
                skin,
                outline
            );
            CreateFirstLaunchTutorialPlayerPartV101128(
                firstLaunchTutorialPlayerModelRootV101128,
                "Hair",
                new Vector2(-1f, 34f),
                new Vector2(20f, 6f),
                hair,
                Color.clear
            );
            CreateFirstLaunchTutorialPlayerPartV101128(
                firstLaunchTutorialPlayerModelRootV101128,
                "Eye",
                new Vector2(6f, 27f),
                new Vector2(3f, 3f),
                outline,
                Color.clear
            );
            firstLaunchTutorialPlayerRightUpperLegV101128 =
                CreateFirstLaunchTutorialPlayerJointV101128(
                    firstLaunchTutorialPlayerModelRootV101128,
                    "Front Leg",
                    new Vector2(5f, -15f),
                    new Vector2(7f, 27f),
                    new Vector2(0.5f, 0.84f),
                    trousers,
                    Color.clear
                );
            firstLaunchTutorialPlayerRightUpperArmV101128 =
                CreateFirstLaunchTutorialPlayerJointV101128(
                    firstLaunchTutorialPlayerModelRootV101128,
                    "Front Arm",
                    new Vector2(11f, 6f),
                    new Vector2(5f, 24f),
                    new Vector2(0.5f, 0.82f),
                    skin,
                    Color.clear
                );

            firstLaunchTutorialPlayerModelRootV101128.SetAsLastSibling();
        }

        private RectTransform CreateFirstLaunchTutorialPlayerJointV101128(
            Transform parent,
            string name,
            Vector2 position,
            Vector2 size,
            Vector2 pivot,
            Color color,
            Color outline)
        {
            RectTransform part = CreateFirstLaunchTutorialPlayerPartV101128(
                parent,
                name,
                position,
                size,
                color,
                outline
            );
            part.pivot = pivot;
            return part;
        }

        private RectTransform CreateFirstLaunchTutorialPlayerPartV101128(
            Transform parent,
            string name,
            Vector2 position,
            Vector2 size,
            Color color,
            Color outline)
        {
            GameObject partObject = new GameObject(
                name,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );
            partObject.layer = firstLaunchTutorialPlayer.gameObject.layer;
            partObject.transform.SetParent(parent, false);
            RectTransform rect = partObject.GetComponent<RectTransform>();
            ConfigureFirstLaunchTutorialPlayerPartRectV101128(
                rect,
                position,
                size,
                new Vector2(0.5f, 0.5f)
            );
            Image image = partObject.GetComponent<Image>();
            image.sprite = null;
            image.type = Image.Type.Simple;
            image.color = color;
            image.raycastTarget = false;
            if (outline.a > 0.001f)
                AddOutline(partObject, outline, 2f);
            return rect;
        }

        private static void ConfigureFirstLaunchTutorialPlayerPartRectV101128(
            RectTransform rect,
            Vector2 position,
            Vector2 size,
            Vector2 pivot)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = pivot;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
        }

        private void UpdateFirstLaunchTutorialPlayerModelV101128()
        {
            EnsureFirstLaunchTutorialPlayerModelV101128();
            if (firstLaunchTutorialPlayerModelRootV101128 == null)
                return;

            if (firstLaunchTutorialPlayerModelCanvasV101128 != null &&
                firstLaunchTutorialPlayer != null)
            {
                firstLaunchTutorialPlayerModelCanvasV101128.alpha =
                    firstLaunchTutorialPlayer.color.a;
            }

            float leftArm = 0f;
            float rightArm = 0f;
            float leftLeg = 0f;
            float rightLeg = 0f;
            if (firstLaunchTutorialMounted)
            {
                leftArm = -6f;
                rightArm = 8f;
                leftLeg = 10f;
                rightLeg = -10f;
            }
            else if (firstLaunchTutorialActionPresentationType ==
                     FirstLaunchTutorialActionPresentationType.None &&
                     firstLaunchTutorialMovementActive)
            {
                float swing = Mathf.Sin(Time.unscaledTime * 9f) * 8f;
                leftArm = swing;
                rightArm = -swing;
                leftLeg = -swing;
                rightLeg = swing;
            }

            SetFirstLaunchTutorialPlayerLimbRotationV101128(
                firstLaunchTutorialPlayerLeftUpperArmV101128,
                leftArm
            );
            SetFirstLaunchTutorialPlayerLimbRotationV101128(
                firstLaunchTutorialPlayerRightUpperArmV101128,
                rightArm
            );
            SetFirstLaunchTutorialPlayerLimbRotationV101128(
                firstLaunchTutorialPlayerLeftUpperLegV101128,
                leftLeg
            );
            SetFirstLaunchTutorialPlayerLimbRotationV101128(
                firstLaunchTutorialPlayerRightUpperLegV101128,
                rightLeg
            );
        }

        private static void SetFirstLaunchTutorialPlayerLimbRotationV101128(
            RectTransform limb,
            float angle)
        {
            if (limb != null)
                limb.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void ForceFirstLaunchTutorialLessonActorDeathV101128(
            TutorialEnemyActor actor)
        {
            if (actor == null)
                return;

            actor.Health = 0f;
            actor.Dead = true;
            actor.Active = false;
            actor.AttackCommitted = false;
            actor.DamageApplied = false;
            if (actor.Image != null)
                actor.Image.gameObject.SetActive(false);

            // BD PRIMARY LESSON VISUAL DEATH SYNC V10.11.28
            // Some legacy lesson rooms render firstLaunchTutorialEnemy directly
            // while combat resolves through the registered actor. Keep both
            // representations in the same dead state at the impact frame.
            bool primaryFocusedLesson =
                firstLaunchTutorialStep == FirstLaunchTutorialStep.AttackEnemy ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.JumpAttack ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.HeavyAttack ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.RangedAttack ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.ChargedShot ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.MountedImpact ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.HazardKnockback;
            if (firstLaunchTutorialEnemy != null &&
                (actor.Image == firstLaunchTutorialEnemy ||
                 primaryFocusedLesson))
            {
                firstLaunchTutorialEnemy.gameObject.SetActive(false);
            }
        }

        private void ApplyFirstLaunchTutorialTextPaletteV101128()
        {
            // BD PROFESSIONAL COLORED TUTORIAL TEXT V10.11.28
            Color accent;
            switch (firstLaunchTutorialStep)
            {
                case FirstLaunchTutorialStep.AttackEnemy:
                case FirstLaunchTutorialStep.JumpAttack:
                case FirstLaunchTutorialStep.HeavyAttack:
                case FirstLaunchTutorialStep.Parry:
                case FirstLaunchTutorialStep.SpinAttack:
                case FirstLaunchTutorialStep.Grapple:
                    accent = new Color(1f, 0.64f, 0.18f, 1f);
                    break;
                case FirstLaunchTutorialStep.RangedAttack:
                case FirstLaunchTutorialStep.Reload:
                case FirstLaunchTutorialStep.ChargedShot:
                case FirstLaunchTutorialStep.MountedImpact:
                case FirstLaunchTutorialStep.HealHorse:
                    accent = new Color(0.72f, 0.50f, 1f, 1f);
                    break;
                case FirstLaunchTutorialStep.Dodge:
                case FirstLaunchTutorialStep.HazardKnockback:
                case FirstLaunchTutorialStep.Jump:
                case FirstLaunchTutorialStep.WallJump:
                    accent = new Color(0.28f, 0.92f, 0.72f, 1f);
                    break;
                default:
                    accent = new Color(0.24f, 0.78f, 1f, 1f);
                    break;
            }

            if (firstLaunchTutorialPrompt != null)
            {
                firstLaunchTutorialPrompt.color = accent;
                firstLaunchTutorialPrompt.fontStyle = FontStyle.Bold;
                firstLaunchTutorialPrompt.supportRichText = true;
            }
            if (firstLaunchTutorialDetail != null)
            {
                firstLaunchTutorialDetail.color =
                    new Color(0.78f, 0.88f, 1f, 1f);
                firstLaunchTutorialDetail.supportRichText = true;
            }
            if (firstLaunchTutorialProgress != null)
                firstLaunchTutorialProgress.color =
                    new Color(0.32f, 0.82f, 1f, 1f);
            if (firstLaunchTutorialFeedback != null)
                firstLaunchTutorialFeedback.color =
                    new Color(0.48f, 1f, 0.70f, 1f);
            if (firstLaunchTutorialKeyboardBindingTitle != null)
                firstLaunchTutorialKeyboardBindingTitle.color =
                    new Color(0.34f, 0.86f, 1f, 1f);
            if (firstLaunchTutorialHandheldBindingTitle != null)
                firstLaunchTutorialHandheldBindingTitle.color =
                    new Color(0.82f, 0.58f, 1f, 1f);
            if (firstLaunchTutorialKeyboardBinding != null)
                firstLaunchTutorialKeyboardBinding.color = Color.white;
            if (firstLaunchTutorialHandheldBinding != null)
                firstLaunchTutorialHandheldBinding.color = Color.white;
            if (firstLaunchTutorialBindingDivider != null)
                firstLaunchTutorialBindingDivider.color =
                    new Color(1f, 0.78f, 0.30f, 1f);
            if (firstLaunchTutorialInstructionAccent != null)
                firstLaunchTutorialInstructionAccent.color = accent;
            if (firstLaunchTutorialInstructionPanel != null)
                firstLaunchTutorialInstructionPanel.color =
                    new Color(0.025f, 0.038f, 0.070f, 0.992f);
        }

        private void DisposeFirstLaunchTutorialPlayerModelV101128()
        {
            firstLaunchTutorialPlayerModelRootV101128 = null;
            firstLaunchTutorialPlayerModelCanvasV101128 = null;
            firstLaunchTutorialPlayerLeftUpperArmV101128 = null;
            firstLaunchTutorialPlayerRightUpperArmV101128 = null;
            firstLaunchTutorialPlayerLeftUpperLegV101128 = null;
            firstLaunchTutorialPlayerRightUpperLegV101128 = null;
        }
    
        // BD FOCUSED LESSON TARGET / FACING CONTRACT V10.11.30
        private FirstLaunchTutorialStep firstLaunchTutorialFocusedTargetStepV101130;
        private bool firstLaunchTutorialFocusedTargetArmedV101130;

        private void UpdateFirstLaunchTutorialFocusedLessonTargetV101130()
        {
            if (firstLaunchTutorialFocusedTargetStepV101130 != firstLaunchTutorialStep)
            {
                firstLaunchTutorialFocusedTargetStepV101130 = firstLaunchTutorialStep;
                firstLaunchTutorialFocusedTargetArmedV101130 = false;
            }

            if (firstLaunchTutorialStep != FirstLaunchTutorialStep.AttackEnemy &&
                firstLaunchTutorialStep != FirstLaunchTutorialStep.HeavyAttack)
            {
                return;
            }

            TutorialEnemyActor current =
                FindFirstLaunchTutorialActorByImageV101128(firstLaunchTutorialEnemy);
            if (firstLaunchTutorialFocusedTargetArmedV101130 &&
                current != null &&
                (!current.Active || current.Dead || current.Health <= 0f))
            {
                return;
            }

            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.AttackEnemy)
            {
                // BD EXACT-CENTER VISIBLE ONE-HIT TARGET V10.11.30.26
                EnsureFirstLaunchTutorialFocusedLessonActorV101130(
                    firstLaunchTutorialLessonScreenCenterX,
                    TutorialEnemyRole.Small
                );
                PrepareFirstLaunchTutorialPrimaryEnemyVisualV1011326(
                    new Color(0.92f, 0.16f, 0.24f, 1f)
                );
            }
            else if (firstLaunchTutorialStep == FirstLaunchTutorialStep.HeavyAttack)
            {
                EnsureFirstLaunchTutorialFocusedLessonActorV101130(
                    firstLaunchTutorialLessonScreenCenterX,
                    TutorialEnemyRole.Sword
                );
            }
        }

        private void EnsureFirstLaunchTutorialFocusedLessonActorV101130(float targetX, TutorialEnemyRole role)
        {
            TutorialEnemyActor actor = FindFirstLaunchTutorialActorByImageV101128(firstLaunchTutorialEnemy);
            bool needsSpawn = actor == null || !actor.Active || actor.Dead || actor.Health <= 0f ||
                Mathf.Abs(actor.Position.x - targetX) > 8f;
            if (needsSpawn)
            {
                SpawnTutorialActor(firstLaunchTutorialEnemy, role, targetX, 1f);
                actor = FindFirstLaunchTutorialActorByImageV101128(firstLaunchTutorialEnemy);
            }
            if (actor == null)
                return;

            actor.Position = new Vector2(targetX, TutorialGroundY);
            actor.SpawnPosition = actor.Position;
            actor.MaximumHealth = 1f;
            actor.Health = Mathf.Max(1f, actor.Health);
            actor.Active = true;
            actor.Dead = false;
            actor.AttackCommitted = false;
            actor.DamageApplied = false;
            actor.NextActionAt = float.PositiveInfinity;
            firstLaunchTutorialEnemyWorldPosition = actor.Position;
            if (actor.Image != null)
                actor.Image.gameObject.SetActive(true);
            firstLaunchTutorialFocusedTargetArmedV101130 = true;
        }

        private Vector2 ResolveFirstLaunchTutorialFocusedAttackDirectionV101130(
            TutorialEnemyActor target,
            Vector2 fallback)
        {
            bool focused = firstLaunchTutorialStep == FirstLaunchTutorialStep.AttackEnemy ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.JumpAttack ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.HeavyAttack;
            if (focused && target != null && target.Active && !target.Dead)
            {
                Vector2 delta = target.Position - firstLaunchTutorialPlayerWorldPosition;
                if (delta.sqrMagnitude > 0.001f)
                    return delta.normalized;
            }
            if (fallback.sqrMagnitude < 0.001f)
                return Vector2.right;
            return fallback.normalized;
        }

        private float ResolveFirstLaunchTutorialPresentationFacingV101130(float fallback)
        {
            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.Jump)
            {
                // BD OPENING FACING RELEASE V10.11.30.28
                // Face the upcoming obstacle while approaching it, but never
                // turn back toward an obstacle that the player already cleared.
                // After landing, ordinary movement owns facing immediately.
                float obstacleDelta = TutorialJumpObstacleX -
                    firstLaunchTutorialPlayerWorldPosition.x;
                if (obstacleDelta > 1f)
                    return 1f;
            }

            if (firstLaunchTutorialStep == FirstLaunchTutorialStep.AttackEnemy ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.HeavyAttack ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.JumpAttack ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.SpinAttack ||
                firstLaunchTutorialStep == FirstLaunchTutorialStep.Grapple)
            {
                TutorialEnemyActor actor = FindFirstLaunchTutorialActorByImageV101128(firstLaunchTutorialEnemy);
                if (actor != null && actor.Active && !actor.Dead)
                {
                    float delta = actor.Position.x - firstLaunchTutorialPlayerWorldPosition.x;
                    if (Mathf.Abs(delta) > 1f)
                        return Mathf.Sign(delta);
                }
            }
            return Mathf.Abs(fallback) > 0.01f ? Mathf.Sign(fallback) : 1f;
        }
}
}
