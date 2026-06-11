using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private enum FirstLaunchTutorialActionPresentationType
        {
            None,
            Mount,
            Dismount,
            LightAttack,
            RangedAttack,
            Dodge,
            HeavyAttack,
            SpinAttack,
            Parry,
            Grapple,
            Heal,
            HorseHit
        }

        private FirstLaunchTutorialActionPresentationType
            firstLaunchTutorialActionPresentationType;
        private float firstLaunchTutorialActionPresentationStartedAt;
        private float firstLaunchTutorialActionPresentationDuration;
        private bool firstLaunchTutorialActionAdvancesLesson;
        private Vector2 firstLaunchTutorialActionStartWorld;
        private Vector2 firstLaunchTutorialActionTargetWorld;
        private bool firstLaunchTutorialActionRangedCharged;
        private bool firstLaunchTutorialRangedImpactResolved;
        private Vector2 firstLaunchTutorialActionDirection =
            Vector2.right;

        private RectTransform firstLaunchTutorialSlashEffect;
        private Image firstLaunchTutorialSlashEffectImage;
        private RectTransform firstLaunchTutorialImpactEffect;
        private Image firstLaunchTutorialImpactEffectImage;
        private RectTransform firstLaunchTutorialMuzzleFlashEffect;
        private RectTransform firstLaunchTutorialRangedProjectileEffect;
        private Image firstLaunchTutorialRangedProjectileEffectImage;
        private RectTransform firstLaunchTutorialDodgeTrailEffect;
        private Image firstLaunchTutorialDodgeTrailEffectImage;
        private RectTransform firstLaunchTutorialSpinEffect;
        private RectTransform firstLaunchTutorialParryEffect;
        private RectTransform firstLaunchTutorialGrappleEffect;
        private Image firstLaunchTutorialGrappleEffectImage;
        private RectTransform firstLaunchTutorialHealEffect;
        private RectTransform firstLaunchTutorialHorseHitEffect;

        private void BuildFirstLaunchTutorialActionPresentation(
            RectTransform parent)
        {
            if (parent == null)
                return;

            firstLaunchTutorialSlashEffect =
                CreateFirstLaunchTutorialEffectRoot(
                    parent,
                    "Tutorial Light Slash Effect"
                );
            firstLaunchTutorialSlashEffectImage =
                CreateFirstLaunchTutorialEffectBar(
                    firstLaunchTutorialSlashEffect,
                    "Slash",
                    0f,
                    0f,
                    54f,
                    10f,
                    new Color(1f, 0.75f, 0.28f, 1f)
                );

            firstLaunchTutorialImpactEffect =
                CreateFirstLaunchTutorialEffectRoot(
                    parent,
                    "Tutorial Heavy Impact Effect"
                );
            firstLaunchTutorialImpactEffectImage =
                CreateFirstLaunchTutorialEffectBar(
                    firstLaunchTutorialImpactEffect,
                    "Impact",
                    0f,
                    0f,
                    58f,
                    58f,
                    new Color(1f, 0.36f, 0.18f, 0.92f)
                );

            firstLaunchTutorialMuzzleFlashEffect =
                CreateFirstLaunchTutorialCrossEffect(
                    parent,
                    "Tutorial Muzzle Flash Effect",
                    32f,
                    8f,
                    new Color(0.34f, 0.92f, 1f, 1f)
                );

            firstLaunchTutorialRangedProjectileEffect =
                CreateFirstLaunchTutorialEffectRoot(
                    parent,
                    "Tutorial Player Projectile Effect"
                );
            firstLaunchTutorialRangedProjectileEffectImage =
                CreateFirstLaunchTutorialEffectBar(
                    firstLaunchTutorialRangedProjectileEffect,
                    "Projectile",
                    0f,
                    0f,
                    28f,
                    12f,
                    new Color(0.44f, 0.94f, 1f, 1f)
                );

            firstLaunchTutorialDodgeTrailEffect =
                CreateFirstLaunchTutorialEffectRoot(
                    parent,
                    "Tutorial Dodge Trail Effect"
                );
            firstLaunchTutorialDodgeTrailEffectImage =
                CreateFirstLaunchTutorialEffectBar(
                    firstLaunchTutorialDodgeTrailEffect,
                    "Trail",
                    0f,
                    0f,
                    84f,
                    20f,
                    new Color(0.24f, 0.78f, 1f, 0.62f)
                );

            firstLaunchTutorialSpinEffect =
                CreateFirstLaunchTutorialFrameEffect(
                    parent,
                    "Tutorial Spin Effect",
                    92f,
                    10f,
                    new Color(1f, 0.60f, 0.20f, 0.90f)
                );

            firstLaunchTutorialParryEffect =
                CreateFirstLaunchTutorialFrameEffect(
                    parent,
                    "Tutorial Parry Effect",
                    76f,
                    8f,
                    new Color(0.30f, 0.84f, 1f, 0.96f)
                );

            firstLaunchTutorialGrappleEffect =
                CreateFirstLaunchTutorialEffectRoot(
                    parent,
                    "Tutorial Grapple Line Effect"
                );
            firstLaunchTutorialGrappleEffectImage =
                CreateFirstLaunchTutorialEffectBar(
                    firstLaunchTutorialGrappleEffect,
                    "Line",
                    0f,
                    0f,
                    1f,
                    8f,
                    new Color(0.76f, 0.88f, 1f, 0.95f)
                );

            firstLaunchTutorialHealEffect =
                CreateFirstLaunchTutorialCrossEffect(
                    parent,
                    "Tutorial Healing Effect",
                    46f,
                    10f,
                    new Color(0.34f, 1f, 0.58f, 0.94f)
                );

            firstLaunchTutorialHorseHitEffect =
                CreateFirstLaunchTutorialCrossEffect(
                    parent,
                    "Tutorial Horse Hit Effect",
                    54f,
                    12f,
                    new Color(1f, 0.22f, 0.18f, 0.96f)
                );

            SetFirstLaunchTutorialActionEffectsVisible(false);
        }

        private void DisposeFirstLaunchTutorialActionPresentation()
        {
            firstLaunchTutorialActionPresentationType =
                FirstLaunchTutorialActionPresentationType.None;
            firstLaunchTutorialActionPresentationStartedAt = 0f;
            firstLaunchTutorialActionPresentationDuration = 0f;
            firstLaunchTutorialActionAdvancesLesson = false;

            firstLaunchTutorialSlashEffect = null;
            firstLaunchTutorialSlashEffectImage = null;
            firstLaunchTutorialImpactEffect = null;
            firstLaunchTutorialImpactEffectImage = null;
            firstLaunchTutorialMuzzleFlashEffect = null;
            firstLaunchTutorialRangedProjectileEffect = null;
            firstLaunchTutorialRangedProjectileEffectImage = null;
            firstLaunchTutorialDodgeTrailEffect = null;
            firstLaunchTutorialDodgeTrailEffectImage = null;
            firstLaunchTutorialSpinEffect = null;
            firstLaunchTutorialParryEffect = null;
            firstLaunchTutorialGrappleEffect = null;
            firstLaunchTutorialGrappleEffectImage = null;
            firstLaunchTutorialHealEffect = null;
            firstLaunchTutorialHorseHitEffect = null;
        }

        private RectTransform CreateFirstLaunchTutorialEffectRoot(
            Transform parent,
            string name)
        {
            GameObject value = new GameObject(
                name,
                typeof(RectTransform)
            );
            value.layer =
                firstLaunchTutorialWorldPanel != null
                    ? firstLaunchTutorialWorldPanel.gameObject.layer
                    : gameObject.layer;
            value.transform.SetParent(parent, false);

            RectTransform rect = value.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
            return rect;
        }

        private Image CreateFirstLaunchTutorialEffectBar(
            Transform parent,
            string name,
            float x,
            float y,
            float width,
            float height,
            Color color)
        {
            Image image = CreatePanel(
                parent,
                name,
                x,
                y,
                width,
                height,
                color
            );
            image.sprite = null;
            image.type = Image.Type.Simple;
            image.raycastTarget = false;
            return image;
        }

        private RectTransform CreateFirstLaunchTutorialCrossEffect(
            Transform parent,
            string name,
            float size,
            float thickness,
            Color color)
        {
            RectTransform root =
                CreateFirstLaunchTutorialEffectRoot(parent, name);
            CreateFirstLaunchTutorialEffectBar(
                root,
                name + " Horizontal",
                0f,
                0f,
                size,
                thickness,
                color
            );
            CreateFirstLaunchTutorialEffectBar(
                root,
                name + " Vertical",
                0f,
                0f,
                thickness,
                size,
                color
            );
            return root;
        }

        private RectTransform CreateFirstLaunchTutorialFrameEffect(
            Transform parent,
            string name,
            float size,
            float thickness,
            Color color)
        {
            RectTransform root =
                CreateFirstLaunchTutorialEffectRoot(parent, name);
            float offset = (size - thickness) * 0.5f;

            CreateFirstLaunchTutorialEffectBar(
                root,
                name + " Top",
                0f,
                offset,
                size,
                thickness,
                color
            );
            CreateFirstLaunchTutorialEffectBar(
                root,
                name + " Bottom",
                0f,
                -offset,
                size,
                thickness,
                color
            );
            CreateFirstLaunchTutorialEffectBar(
                root,
                name + " Left",
                -offset,
                0f,
                thickness,
                size,
                color
            );
            CreateFirstLaunchTutorialEffectBar(
                root,
                name + " Right",
                offset,
                0f,
                thickness,
                size,
                color
            );
            return root;
        }

        private bool IsFirstLaunchTutorialActionInputLocked()
        {
            return firstLaunchTutorialPlayerDeathActive ||
                firstLaunchTutorialActionPresentationType !=
                    FirstLaunchTutorialActionPresentationType.None;
        }

        private void BeginFirstLaunchTutorialActionPresentation(
            FirstLaunchTutorialActionPresentationType type,
            float duration,
            bool advancesLesson,
            Vector2 startWorld,
            Vector2 targetWorld)
        {
            firstLaunchTutorialActionPresentationType = type;
            firstLaunchTutorialActionPresentationStartedAt =
                Time.unscaledTime;
            firstLaunchTutorialActionPresentationDuration =
                Mathf.Max(0.08f, duration);
            firstLaunchTutorialActionAdvancesLesson = advancesLesson;
            firstLaunchTutorialActionStartWorld = startWorld;
            firstLaunchTutorialActionTargetWorld = targetWorld;

            SetFirstLaunchTutorialActionEffectsVisible(false);
        }

        private void PlayFirstLaunchTutorialMountAnimation(
            bool advancesLesson)
        {
            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.Mount,
                0.36f,
                advancesLesson,
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialHorseWorldPosition
            );
            PlayClick();
        }

        private void PlayFirstLaunchTutorialDismountAnimation(
            bool advancesLesson)
        {
            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.Dismount,
                0.30f,
                advancesLesson,
                firstLaunchTutorialHorseWorldPosition,
                firstLaunchTutorialHorseWorldPosition +
                    new Vector2(76f, 0f)
            );
            PlayClick();
        }

        private void PlayFirstLaunchTutorialLightAttackAnimation(
            bool advancesLesson)
        {
            Vector2 direction =
                ResolveFirstLaunchTutorialActionDirection();
            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.LightAttack,
                0.24f,
                advancesLesson,
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialPlayerWorldPosition +
                    direction * 70f
            );
            firstLaunchTutorialActionDirection = direction;
            PlayClick();
        }

        private void PlayFirstLaunchTutorialRangedAttackAnimation(
            Vector2 targetWorld,
            bool advancesLesson,
            bool chargedShot = false)
        {
            Vector2 origin =
                firstLaunchTutorialMounted
                    ? firstLaunchTutorialHorseWorldPosition +
                      new Vector2(34f, 24f)
                    : firstLaunchTutorialPlayerWorldPosition +
                      new Vector2(28f, 20f);
            firstLaunchTutorialActionRangedCharged = chargedShot;
            firstLaunchTutorialRangedImpactResolved = false;
            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.RangedAttack,
                chargedShot ? 0.66f : 0.48f,
                advancesLesson,
                origin,
                targetWorld + new Vector2(0f, 18f)
            );
            firstLaunchTutorialActionDirection =
                (targetWorld - origin).normalized;
            PlayClick();
        }

        private void PlayFirstLaunchTutorialDodgeAnimation(
            Vector2 requestedDirection,
            bool advancesLesson)
        {
            Vector2 direction = requestedDirection;
            if (direction.sqrMagnitude < 0.001f)
                direction = Vector2.right;
            direction.Normalize();

            Vector2 target =
                firstLaunchTutorialPlayerWorldPosition +
                direction * 138f;
            target.x = Mathf.Clamp(
                ResolveFirstLaunchTutorialCollisionX(
                    firstLaunchTutorialPlayerWorldPosition.x,
                    target.x,
                    includeStaticObstacles: false
                ),
                TutorialWorldMinX,
                TutorialWorldMaxX
            );
            target.y = Mathf.Clamp(
                target.y,
                TutorialWorldMinY,
                TutorialWorldMaxY
            );

            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.Dodge,
                0.28f,
                advancesLesson,
                firstLaunchTutorialPlayerWorldPosition,
                target
            );
            firstLaunchTutorialActionDirection = direction;
            PlayClick();
        }

        private void PlayFirstLaunchTutorialHeavyAttackAnimation(
            bool advancesLesson)
        {
            Vector2 direction =
                ResolveFirstLaunchTutorialActionDirection();
            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.HeavyAttack,
                0.36f,
                advancesLesson,
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialPlayerWorldPosition +
                    direction * 92f
            );
            firstLaunchTutorialActionDirection = direction;
            PlayClick();
        }

        private void PlayFirstLaunchTutorialSpinAttackAnimation(
            bool advancesLesson)
        {
            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.SpinAttack,
                0.52f,
                advancesLesson,
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialPlayerWorldPosition
            );
            PlayClick();
        }

        private void PlayFirstLaunchTutorialParryAnimation(
            bool advancesLesson)
        {
            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.Parry,
                0.36f,
                advancesLesson,
                firstLaunchTutorialPlayerWorldPosition,
                firstLaunchTutorialProjectileWorldPosition
            );
            PlayClick();
        }

        private void PlayFirstLaunchTutorialGrappleAnimation(
            bool advancesLesson)
        {
            TutorialEnemyActor actor =
                FindClosestLivingTutorialActor(560f);
            Vector2 target = actor != null
                ? actor.Position
                : firstLaunchTutorialPlayerWorldPosition +
                    ResolveFirstLaunchTutorialActionDirection() * 360f;
            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.Grapple,
                0.72f,
                advancesLesson,
                firstLaunchTutorialPlayerWorldPosition,
                target
            );
            PlayClick();
        }

        private void PlayFirstLaunchTutorialHealAnimation(
            bool advancesLesson)
        {
            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.Heal,
                0.54f,
                advancesLesson,
                firstLaunchTutorialHorseWorldPosition,
                firstLaunchTutorialHorseWorldPosition
            );
            PlayClick();
        }

        private void PlayFirstLaunchTutorialHorseHitAnimation()
        {
            BeginFirstLaunchTutorialActionPresentation(
                FirstLaunchTutorialActionPresentationType.HorseHit,
                0.42f,
                advancesLesson: false,
                firstLaunchTutorialHorseWorldPosition,
                firstLaunchTutorialHorseWorldPosition
            );
        }

        private Vector2 ResolveFirstLaunchTutorialActionDirection()
        {
            float facing =
                Mathf.Abs(firstLaunchTutorialLastMoveDirection.x) > 0.01f
                    ? Mathf.Sign(firstLaunchTutorialLastMoveDirection.x)
                    : 1f;
            return new Vector2(facing, 0f);
        }

        private void ApplyFirstLaunchTutorialPlayerActionPose(
            Vector2 worldPosition,
            float horizontalScale,
            float verticalScale,
            float verticalOffset = 0f)
        {
            if (firstLaunchTutorialPlayer == null)
                return;

            float facing =
                Mathf.Abs(firstLaunchTutorialLastMoveDirection.x) > 0.01f
                    ? Mathf.Sign(firstLaunchTutorialLastMoveDirection.x)
                    : 1f;
            float baseScale = firstLaunchTutorialMounted ? 0.78f : 1f;

            firstLaunchTutorialPlayer.rectTransform.anchoredPosition =
                SnapFirstLaunchTutorialWorldPosition(
                    worldPosition + new Vector2(0f, verticalOffset)
                );
            firstLaunchTutorialPlayer.rectTransform.localScale =
                new Vector3(
                    facing * baseScale * horizontalScale,
                    baseScale * verticalScale,
                    1f
                );
        }

        private void ApplyFirstLaunchTutorialHorseActionPose(
            Vector2 worldPosition,
            float horizontalScale,
            float verticalScale,
            float horizontalOffset = 0f,
            float verticalOffset = 0f)
        {
            if (firstLaunchTutorialHorse == null)
                return;

            firstLaunchTutorialHorse.rectTransform.anchoredPosition =
                SnapFirstLaunchTutorialWorldPosition(
                    worldPosition +
                    new Vector2(horizontalOffset, verticalOffset)
                );
            float facing =
                Mathf.Abs(firstLaunchTutorialLastMoveDirection.x) > 0.01f
                    ? Mathf.Sign(firstLaunchTutorialLastMoveDirection.x)
                    : 1f;
            firstLaunchTutorialHorse.rectTransform.localScale =
                new Vector3(
                    facing * horizontalScale,
                    verticalScale,
                    1f
                );
        }

        private void ApplyFirstLaunchTutorialEnemyActionPose(
            Vector2 worldPosition,
            float horizontalScale,
            float verticalScale,
            float horizontalOffset = 0f,
            float verticalOffset = 0f)
        {
            if (firstLaunchTutorialEnemy == null ||
                !firstLaunchTutorialEnemy.gameObject.activeSelf)
            {
                return;
            }

            firstLaunchTutorialEnemy.rectTransform.anchoredPosition =
                SnapFirstLaunchTutorialWorldPosition(
                    worldPosition +
                    new Vector2(horizontalOffset, verticalOffset)
                );
            firstLaunchTutorialEnemy.rectTransform.localScale =
                new Vector3(horizontalScale, verticalScale, 1f);
        }

        private void UpdateFirstLaunchTutorialActionPresentation()
        {
            if (firstLaunchTutorialActionPresentationType ==
                    FirstLaunchTutorialActionPresentationType.None)
            {
                return;
            }

            float progress = Mathf.Clamp01(
                (Time.unscaledTime -
                 firstLaunchTutorialActionPresentationStartedAt) /
                Mathf.Max(
                    0.08f,
                    firstLaunchTutorialActionPresentationDuration
                )
            );
            float stepped =
                Mathf.Floor(progress * 6f) / 6f;

            switch (firstLaunchTutorialActionPresentationType)
            {
                case FirstLaunchTutorialActionPresentationType.Mount:
                    UpdateFirstLaunchTutorialMountPresentation(stepped);
                    break;
                case FirstLaunchTutorialActionPresentationType.Dismount:
                    UpdateFirstLaunchTutorialDismountPresentation(stepped);
                    break;
                case FirstLaunchTutorialActionPresentationType.LightAttack:
                    UpdateFirstLaunchTutorialLightAttackPresentation(
                        stepped
                    );
                    break;
                case FirstLaunchTutorialActionPresentationType.RangedAttack:
                    // Projectile travel must remain continuous so impact and
                    // damage cannot visually precede the projectile endpoint.
                    UpdateFirstLaunchTutorialRangedAttackPresentation(
                        progress
                    );
                    break;
                case FirstLaunchTutorialActionPresentationType.Dodge:
                    UpdateFirstLaunchTutorialDodgePresentation(stepped);
                    break;
                case FirstLaunchTutorialActionPresentationType.HeavyAttack:
                    UpdateFirstLaunchTutorialHeavyAttackPresentation(
                        stepped
                    );
                    break;
                case FirstLaunchTutorialActionPresentationType.SpinAttack:
                    UpdateFirstLaunchTutorialSpinPresentation(stepped);
                    break;
                case FirstLaunchTutorialActionPresentationType.Parry:
                    UpdateFirstLaunchTutorialParryPresentation(stepped);
                    break;
                case FirstLaunchTutorialActionPresentationType.Grapple:
                    UpdateFirstLaunchTutorialGrapplePresentation(stepped);
                    break;
                case FirstLaunchTutorialActionPresentationType.Heal:
                    UpdateFirstLaunchTutorialHealPresentation(stepped);
                    break;
                case FirstLaunchTutorialActionPresentationType.HorseHit:
                    UpdateFirstLaunchTutorialHorseHitPresentation(stepped);
                    break;
            }

            if (progress < 1f)
                return;

            FirstLaunchTutorialActionPresentationType completedType =
                firstLaunchTutorialActionPresentationType;
            bool advances =
                firstLaunchTutorialActionAdvancesLesson;
            firstLaunchTutorialActionPresentationType =
                FirstLaunchTutorialActionPresentationType.None;
            firstLaunchTutorialActionAdvancesLesson = false;
            SetFirstLaunchTutorialActionEffectsVisible(false);

            switch (completedType)
            {
                case FirstLaunchTutorialActionPresentationType.Mount:
                    CompleteFirstLaunchTutorialMountAnimation(advances);
                    break;
                case FirstLaunchTutorialActionPresentationType.Dismount:
                    CompleteFirstLaunchTutorialDismountAnimation(advances);
                    break;
                case FirstLaunchTutorialActionPresentationType.LightAttack:
                    CompleteFirstLaunchTutorialLightAttackAnimation(
                        advances
                    );
                    break;
                case FirstLaunchTutorialActionPresentationType.RangedAttack:
                    CompleteFirstLaunchTutorialRangedAttackAnimation(
                        advances
                    );
                    break;
                case FirstLaunchTutorialActionPresentationType.Dodge:
                    CompleteFirstLaunchTutorialDodgeAnimation(advances);
                    break;
                case FirstLaunchTutorialActionPresentationType.HeavyAttack:
                    CompleteFirstLaunchTutorialHeavyAttackAnimation(
                        advances
                    );
                    break;
                case FirstLaunchTutorialActionPresentationType.SpinAttack:
                    CompleteFirstLaunchTutorialSpinAttackAnimation(
                        advances
                    );
                    break;
                case FirstLaunchTutorialActionPresentationType.Parry:
                    CompleteFirstLaunchTutorialParryAnimation(advances);
                    break;
                case FirstLaunchTutorialActionPresentationType.Grapple:
                    CompleteFirstLaunchTutorialGrappleAnimation(advances);
                    break;
                case FirstLaunchTutorialActionPresentationType.Heal:
                    CompleteFirstLaunchTutorialHealAnimation(advances);
                    break;
            }

            RenderFirstLaunchTutorialFreePlayCourse(force: true);
        }
        private void UpdateFirstLaunchTutorialMountPresentation(
            float progress)
        {
            firstLaunchTutorialPlayerWorldPosition = Vector2.Lerp(
                firstLaunchTutorialActionStartWorld,
                firstLaunchTutorialActionTargetWorld,
                progress
            );
            float arc = Mathf.Sin(progress * Mathf.PI);
            ApplyFirstLaunchTutorialPlayerActionPose(
                firstLaunchTutorialPlayerWorldPosition,
                1f - arc * 0.12f,
                1f + arc * 0.12f,
                arc * 20f
            );
            ApplyFirstLaunchTutorialHorseActionPose(
                firstLaunchTutorialHorseWorldPosition,
                1f + arc * 0.04f,
                1f - arc * 0.04f
            );

            SetEffectVisible(
                firstLaunchTutorialDodgeTrailEffect,
                progress < 0.66f
            );
            if (firstLaunchTutorialDodgeTrailEffect != null)
            {
                SetImageAlpha(
                    firstLaunchTutorialDodgeTrailEffectImage,
                    0.46f * (1f - progress)
                );
                firstLaunchTutorialDodgeTrailEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialPlayerWorldPosition -
                        new Vector2(24f, 0f)
                    );
                firstLaunchTutorialDodgeTrailEffect.localScale =
                    new Vector3(0.55f, 0.55f, 1f);
            }
        }
        private void UpdateFirstLaunchTutorialDismountPresentation(
            float progress)
        {
            firstLaunchTutorialPlayerWorldPosition = Vector2.Lerp(
                firstLaunchTutorialActionStartWorld,
                firstLaunchTutorialActionTargetWorld,
                progress
            );
            float arc = Mathf.Sin(progress * Mathf.PI);
            ApplyFirstLaunchTutorialPlayerActionPose(
                firstLaunchTutorialPlayerWorldPosition,
                1f + arc * 0.10f,
                1f - arc * 0.08f,
                arc * 18f
            );
            ApplyFirstLaunchTutorialHorseActionPose(
                firstLaunchTutorialHorseWorldPosition,
                1f - arc * 0.03f,
                1f + arc * 0.03f
            );

            SetEffectVisible(
                firstLaunchTutorialDodgeTrailEffect,
                progress < 0.66f
            );
            if (firstLaunchTutorialDodgeTrailEffect != null)
            {
                SetImageAlpha(
                    firstLaunchTutorialDodgeTrailEffectImage,
                    0.40f * (1f - progress)
                );
                firstLaunchTutorialDodgeTrailEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialHorseWorldPosition +
                        new Vector2(24f, 0f)
                    );
                firstLaunchTutorialDodgeTrailEffect.localScale =
                    new Vector3(0.42f, 0.42f, 1f);
            }
        }
        private void UpdateFirstLaunchTutorialLightAttackPresentation(
            float progress)
        {
            SetEffectVisible(firstLaunchTutorialSlashEffect, true);

            float windup =
                progress < 0.25f
                    ? progress / 0.25f
                    : 1f;
            float strike =
                progress <= 0.25f
                    ? 0f
                    : Mathf.Clamp01((progress - 0.25f) / 0.40f);
            float recovery =
                progress <= 0.65f
                    ? 0f
                    : Mathf.Clamp01((progress - 0.65f) / 0.35f);
            float lunge =
                Mathf.Lerp(-10f, 32f, strike) *
                (1f - recovery);

            Vector2 playerPose =
                firstLaunchTutorialActionStartWorld +
                firstLaunchTutorialActionDirection * lunge;
            ApplyFirstLaunchTutorialPlayerActionPose(
                playerPose,
                1f + strike * 0.16f - recovery * 0.12f,
                1f - strike * 0.10f + windup * 0.04f
            );

            if (firstLaunchTutorialSlashEffect != null)
            {
                Vector2 position = Vector2.Lerp(
                    firstLaunchTutorialActionStartWorld +
                        firstLaunchTutorialActionDirection * 28f,
                    firstLaunchTutorialActionTargetWorld,
                    progress
                );
                firstLaunchTutorialSlashEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(position);
                firstLaunchTutorialSlashEffect.localRotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        firstLaunchTutorialActionDirection.x >= 0f
                            ? -24f
                            : 204f
                    );
                firstLaunchTutorialSlashEffect.localScale =
                    new Vector3(
                        0.65f + progress * 0.55f,
                        1f,
                        1f
                    );
                SetImageAlpha(
                    firstLaunchTutorialSlashEffectImage,
                    1f - progress * 0.65f
                );
            }

            if (firstLaunchTutorialActionAdvancesLesson &&
                progress >= 0.58f)
            {
                float shake =
                    Mathf.Floor(progress * 12f) % 2f == 0f
                        ? -8f
                        : 8f;
                ApplyFirstLaunchTutorialEnemyActionPose(
                    firstLaunchTutorialEnemyWorldPosition,
                    1.12f,
                    0.86f,
                    shake
                );
            }

            if (progress >= 0.66f)
                ResolveFirstLaunchTutorialMeleeImpact();
        }
        private void UpdateFirstLaunchTutorialRangedAttackPresentation(
            float progress)
        {
            bool flashVisible = progress <= 0.28f;
            SetEffectVisible(
                firstLaunchTutorialMuzzleFlashEffect,
                flashVisible
            );
            SetEffectVisible(
                firstLaunchTutorialRangedProjectileEffect,
                true
            );

            float recoil = Mathf.Sin(progress * Mathf.PI) * 12f;
            Vector2 recoilDirection =
                firstLaunchTutorialActionDirection.sqrMagnitude > 0.001f
                    ? -firstLaunchTutorialActionDirection.normalized
                    : Vector2.left;

            if (firstLaunchTutorialMounted)
            {
                ApplyFirstLaunchTutorialHorseActionPose(
                    firstLaunchTutorialHorseWorldPosition,
                    1f + recoil * 0.002f,
                    1f - recoil * 0.002f,
                    recoilDirection.x * recoil * 0.45f,
                    recoilDirection.y * recoil * 0.30f
                );
                ApplyFirstLaunchTutorialPlayerActionPose(
                    firstLaunchTutorialHorseWorldPosition +
                        recoilDirection * recoil * 0.38f,
                    1f - recoil * 0.004f,
                    1f + recoil * 0.004f,
                    34f
                );
            }
            else
            {
                ApplyFirstLaunchTutorialPlayerActionPose(
                    firstLaunchTutorialPlayerWorldPosition +
                        recoilDirection * recoil,
                    1f - recoil * 0.004f,
                    1f + recoil * 0.004f
                );
            }

            if (firstLaunchTutorialMuzzleFlashEffect != null)
            {
                firstLaunchTutorialMuzzleFlashEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialActionStartWorld
                    );
                float flashScale = progress <= 0.12f ? 1f : 0.62f;
                firstLaunchTutorialMuzzleFlashEffect.localScale =
                    Vector3.one * flashScale;
            }

            if (firstLaunchTutorialRangedProjectileEffect != null)
            {
                firstLaunchTutorialRangedProjectileEffect
                    .anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        Vector2.Lerp(
                            firstLaunchTutorialActionStartWorld,
                            firstLaunchTutorialActionTargetWorld,
                            progress
                        )
                    );
                Vector3 baseScale = firstLaunchTutorialActionRangedCharged
                    ? new Vector3(1.85f, 1.38f, 1f)
                    : Vector3.one;
                Vector3 impactScale = firstLaunchTutorialActionRangedCharged
                    ? new Vector3(2.45f, 1.22f, 1f)
                    : new Vector3(1.28f, 0.86f, 1f);
                firstLaunchTutorialRangedProjectileEffect.localScale =
                    progress < 0.70f
                        ? baseScale
                        : impactScale;
            }

            SetImageAlpha(
                firstLaunchTutorialRangedProjectileEffectImage,
                1f - Mathf.Max(0f, progress - 0.82f) / 0.18f
            );

            if (!firstLaunchTutorialRangedImpactResolved &&
                progress >= 1f)
            {
                firstLaunchTutorialRangedImpactResolved = true;
                ResolveFirstLaunchTutorialRangedProjectileImpact();
            }

            if (firstLaunchTutorialPendingShotHitResolved &&
                progress >= 1f)
            {
                float hitPulse =
                    1f + Mathf.Sin((progress - 0.78f) * 16f) * 0.16f;
                ApplyFirstLaunchTutorialEnemyActionPose(
                    firstLaunchTutorialEnemyWorldPosition,
                    hitPulse,
                    2f - hitPulse
                );
            }
        }
        private void UpdateFirstLaunchTutorialDodgePresentation(
            float progress)
        {
            firstLaunchTutorialPlayerWorldPosition = Vector2.Lerp(
                firstLaunchTutorialActionStartWorld,
                firstLaunchTutorialActionTargetWorld,
                progress
            );

            float compression = Mathf.Sin(progress * Mathf.PI);
            ApplyFirstLaunchTutorialPlayerActionPose(
                firstLaunchTutorialPlayerWorldPosition,
                1f + compression * 0.24f,
                1f - compression * 0.30f,
                compression * 4f
            );

            SetEffectVisible(firstLaunchTutorialDodgeTrailEffect, true);
            if (firstLaunchTutorialDodgeTrailEffect != null)
            {
                firstLaunchTutorialDodgeTrailEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        Vector2.Lerp(
                            firstLaunchTutorialActionStartWorld,
                            firstLaunchTutorialPlayerWorldPosition,
                            0.45f
                        )
                    );
                float rotation = Mathf.Atan2(
                    firstLaunchTutorialActionDirection.y,
                    firstLaunchTutorialActionDirection.x
                ) * Mathf.Rad2Deg;
                firstLaunchTutorialDodgeTrailEffect.localRotation =
                    Quaternion.Euler(0f, 0f, rotation);
            }
            SetImageAlpha(
                firstLaunchTutorialDodgeTrailEffectImage,
                0.72f * (1f - progress)
            );
        }
        private void UpdateFirstLaunchTutorialHeavyAttackPresentation(
            float progress)
        {
            SetEffectVisible(firstLaunchTutorialImpactEffect, true);

            float windup =
                progress < 0.42f
                    ? progress / 0.42f
                    : 1f;
            float strike =
                progress <= 0.42f
                    ? 0f
                    : Mathf.Clamp01((progress - 0.42f) / 0.34f);
            float recovery =
                progress <= 0.76f
                    ? 0f
                    : Mathf.Clamp01((progress - 0.76f) / 0.24f);

            Vector2 direction =
                firstLaunchTutorialActionDirection.sqrMagnitude > 0.001f
                    ? firstLaunchTutorialActionDirection.normalized
                    : Vector2.right;
            Vector2 pose =
                firstLaunchTutorialActionStartWorld -
                direction * (18f * windup) +
                direction * (46f * strike * (1f - recovery));
            ApplyFirstLaunchTutorialPlayerActionPose(
                pose,
                1f + strike * 0.24f,
                1f - windup * 0.16f + recovery * 0.16f,
                windup * 6f
            );

            if (firstLaunchTutorialImpactEffect != null)
            {
                firstLaunchTutorialImpactEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialActionTargetWorld +
                        new Vector2(0f, 18f)
                    );
                float pulse =
                    progress < 0.5f
                        ? Mathf.Lerp(0.35f, 1.25f, progress * 2f)
                        : Mathf.Lerp(
                            1.25f,
                            0.70f,
                            (progress - 0.5f) * 2f
                        );
                firstLaunchTutorialImpactEffect.localScale =
                    Vector3.one * pulse;
                SetImageAlpha(
                    firstLaunchTutorialImpactEffectImage,
                    1f - progress * 0.72f
                );
            }

            if (firstLaunchTutorialActionAdvancesLesson &&
                progress >= 0.58f)
            {
                float squash = Mathf.Clamp01((progress - 0.58f) / 0.22f);
                ApplyFirstLaunchTutorialEnemyActionPose(
                    firstLaunchTutorialEnemyWorldPosition,
                    1f + squash * 0.30f,
                    1f - squash * 0.42f,
                    direction.x * 12f * squash
                );
            }

            if (progress >= 0.78f)
                ResolveFirstLaunchTutorialMeleeImpact();
        }
        private void UpdateFirstLaunchTutorialSpinPresentation(
            float progress)
        {
            SetEffectVisible(firstLaunchTutorialSpinEffect, true);
            int phase = Mathf.FloorToInt(progress * 8f);
            float facingFlip = phase % 2 == 0 ? 1f : -1f;
            float pulse = 1f + Mathf.Sin(progress * Mathf.PI) * 0.14f;

            ApplyFirstLaunchTutorialPlayerActionPose(
                firstLaunchTutorialPlayerWorldPosition,
                facingFlip * pulse,
                2f - pulse,
                Mathf.Sin(progress * Mathf.PI * 2f) * 4f
            );

            if (firstLaunchTutorialSpinEffect != null)
            {
                firstLaunchTutorialSpinEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialPlayerWorldPosition +
                        new Vector2(0f, 12f)
                    );
                float steppedScale = 0.55f + progress * 1.15f;
                firstLaunchTutorialSpinEffect.localScale =
                    Vector3.one * steppedScale;
                firstLaunchTutorialSpinEffect.localRotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        Mathf.Floor(progress * 4f) * 90f
                    );
            }

            if (firstLaunchTutorialActionAdvancesLesson)
            {
                float targetPulse =
                    1f + Mathf.Sin(progress * Mathf.PI * 4f) * 0.12f;
                ApplyFirstLaunchTutorialEnemyActionPose(
                    firstLaunchTutorialEnemyWorldPosition,
                    targetPulse,
                    2f - targetPulse
                );
            }
        }
        private void UpdateFirstLaunchTutorialParryPresentation(
            float progress)
        {
            SetEffectVisible(firstLaunchTutorialParryEffect, true);

            float brace = Mathf.Sin(progress * Mathf.PI);
            ApplyFirstLaunchTutorialPlayerActionPose(
                firstLaunchTutorialPlayerWorldPosition,
                1f + brace * 0.10f,
                1f - brace * 0.16f,
                brace * 3f
            );

            if (firstLaunchTutorialParryEffect != null)
            {
                firstLaunchTutorialParryEffect.anchoredPosition =
                    SnapFirstLaunchTutorialWorldPosition(
                        firstLaunchTutorialPlayerWorldPosition +
                        new Vector2(0f, 12f)
                    );
                float scale =
                    progress < 0.5f
                        ? Mathf.Lerp(0.55f, 1.12f, progress * 2f)
                        : Mathf.Lerp(
                            1.12f,
                            0.80f,
                            (progress - 0.5f) * 2f
                        );
                firstLaunchTutorialParryEffect.localScale =
                    Vector3.one * scale;
            }

            if (firstLaunchTutorialProjectile != null)
            {
                firstLaunchTutorialProjectile.gameObject.SetActive(true);
                Vector2 rebound = Vector2.Lerp(
                    firstLaunchTutorialActionTargetWorld,
                    firstLaunchTutorialEnemyWorldPosition,
                    progress
                );
                firstLaunchTutorialProjectileWorldPosition = rebound;
            }
        }

        private void UpdateFirstLaunchTutorialGrapplePresentation(
            float progress)
        {
            SetEffectVisible(firstLaunchTutorialGrappleEffect, true);
            if (firstLaunchTutorialGrappleEffect == null ||
                firstLaunchTutorialGrappleEffectImage == null)
            {
                return;
            }

            Vector2 anchor = firstLaunchTutorialActionTargetWorld;
            Vector2 start = firstLaunchTutorialActionStartWorld;
            float extension = Mathf.Clamp01(progress / 0.45f);
            Vector2 lineEnd = Vector2.Lerp(start, anchor, extension);
            Vector2 delta = lineEnd - start;
            float length = Mathf.Max(1f, delta.magnitude);

            firstLaunchTutorialGrappleEffect.anchoredPosition =
                SnapFirstLaunchTutorialWorldPosition(
                    start + delta * 0.5f + new Vector2(0f, 18f)
                );
            firstLaunchTutorialGrappleEffectImage.rectTransform
                .sizeDelta = new Vector2(length, 8f);
            firstLaunchTutorialGrappleEffect.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg
                );

            if (progress > 0.45f)
            {
                float pull = (progress - 0.45f) / 0.55f;
                TutorialEnemyActor actor =
                    firstLaunchTutorialPendingHookTarget ??
                    FindClosestLivingTutorialActor(620f);
                if (actor != null && actor.Role != TutorialEnemyRole.MiniBoss)
                {
                    float side = Mathf.Sign(
                        anchor.x - firstLaunchTutorialPlayerWorldPosition.x
                    );
                    Vector2 safeTarget =
                        firstLaunchTutorialPlayerWorldPosition +
                        new Vector2(side * 132f, 0f);
                    actor.Position = Vector2.Lerp(anchor, safeTarget, pull);
                    if (actor.Image == firstLaunchTutorialEnemy)
                        firstLaunchTutorialEnemyWorldPosition = actor.Position;
                }
            }
        }
        private void UpdateFirstLaunchTutorialHealPresentation(
            float progress)
        {
            SetEffectVisible(firstLaunchTutorialHealEffect, true);
            float kneel = Mathf.Sin(progress * Mathf.PI);
            Vector2 direction =
                (firstLaunchTutorialHorseWorldPosition -
                 firstLaunchTutorialPlayerWorldPosition).normalized;

            ApplyFirstLaunchTutorialPlayerActionPose(
                firstLaunchTutorialPlayerWorldPosition +
                    direction * (kneel * 12f),
                1f + kneel * 0.08f,
                1f - kneel * 0.20f,
                -kneel * 6f
            );
            ApplyFirstLaunchTutorialHorseActionPose(
                firstLaunchTutorialHorseWorldPosition,
                1f + kneel * 0.05f,
                1f + kneel * 0.05f,
                0f,
                kneel * 2f
            );

            if (firstLaunchTutorialHealEffect == null)
                return;

            firstLaunchTutorialHealEffect.anchoredPosition =
                SnapFirstLaunchTutorialWorldPosition(
                    firstLaunchTutorialHorseWorldPosition +
                    new Vector2(
                        Mathf.Sin(progress * Mathf.PI * 4f) * 18f,
                        20f + progress * 54f
                    )
                );
            firstLaunchTutorialHealEffect.localScale =
                Vector3.one *
                (0.72f + Mathf.Sin(progress * Mathf.PI) * 0.40f);
        }
        private void UpdateFirstLaunchTutorialHorseHitPresentation(
            float progress)
        {
            SetEffectVisible(firstLaunchTutorialHorseHitEffect, true);

            float shake =
                Mathf.Floor(progress * 10f) % 2f == 0f
                    ? -10f
                    : 10f;
            float collapse = Mathf.Clamp01(progress / 0.70f);
            ApplyFirstLaunchTutorialHorseActionPose(
                firstLaunchTutorialHorseWorldPosition,
                1f + collapse * 0.18f,
                1f - collapse * 0.26f,
                shake * (1f - progress),
                -collapse * 5f
            );

            if (firstLaunchTutorialHorseHitEffect == null)
                return;

            firstLaunchTutorialHorseHitEffect.anchoredPosition =
                SnapFirstLaunchTutorialWorldPosition(
                    firstLaunchTutorialHorseWorldPosition +
                    new Vector2(0f, 24f)
                );
            firstLaunchTutorialHorseHitEffect.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    Mathf.Floor(progress * 4f) * 45f
                );
            firstLaunchTutorialHorseHitEffect.localScale =
                Vector3.one * (0.75f + progress * 0.85f);
        }

        private void SetFirstLaunchTutorialHealingPreview(
            float progress,
            bool visible)
        {
            SetEffectVisible(firstLaunchTutorialHealEffect, visible);
            if (!visible || firstLaunchTutorialHealEffect == null)
                return;

            firstLaunchTutorialHealEffect.anchoredPosition =
                SnapFirstLaunchTutorialWorldPosition(
                    firstLaunchTutorialHorseWorldPosition +
                    new Vector2(
                        Mathf.Sin(Time.unscaledTime * 6f) * 14f,
                        22f + progress * 34f
                    )
                );
            firstLaunchTutorialHealEffect.localScale =
                Vector3.one * (0.68f + progress * 0.42f);
        }

        private void SetFirstLaunchTutorialActionEffectsVisible(
            bool visible)
        {
            SetEffectVisible(firstLaunchTutorialSlashEffect, visible);
            SetEffectVisible(firstLaunchTutorialImpactEffect, visible);
            SetEffectVisible(
                firstLaunchTutorialMuzzleFlashEffect,
                visible
            );
            SetEffectVisible(
                firstLaunchTutorialRangedProjectileEffect,
                visible
            );
            SetEffectVisible(
                firstLaunchTutorialDodgeTrailEffect,
                visible
            );
            SetEffectVisible(firstLaunchTutorialSpinEffect, visible);
            SetEffectVisible(firstLaunchTutorialParryEffect, visible);
            SetEffectVisible(firstLaunchTutorialGrappleEffect, visible);
            SetEffectVisible(firstLaunchTutorialHealEffect, visible);
            SetEffectVisible(firstLaunchTutorialHorseHitEffect, visible);
        }

        private static void SetEffectVisible(
            RectTransform effect,
            bool visible)
        {
            if (effect != null)
                effect.gameObject.SetActive(visible);
        }

        private static void SetImageAlpha(
            Image image,
            float alpha)
        {
            if (image == null)
                return;

            Color color = image.color;
            color.a = Mathf.Clamp01(alpha);
            image.color = color;
        }
    }
}
