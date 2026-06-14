using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private const float StartGameTransitionDuration = 1.08f;
        private const float StartGameTransitionNearScreenDistance = 0.34f;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallStartGameTransitionDriver()
        {
            if (UnityEngine.Object.FindFirstObjectByType<
                    StartGameTransitionDriver>() != null)
            {
                return;
            }

            GameObject root = new GameObject(
                "B&D Handheld Start Game Transition Driver"
            );
            DontDestroyOnLoad(root);
            root.AddComponent<StartGameTransitionDriver>();
        }

        [DefaultExecutionOrder(-815)]
        private sealed class StartGameTransitionDriver : MonoBehaviour
        {
            private BDModernHandheld3DPresenter owner;
            private bool transitionActive;
            private bool restoreCameraWhenHidden;
            private float startedAt;
            private Vector3 startCameraPosition;
            private Quaternion startCameraRotation;
            private float startFieldOfView;
            private float startNearClip;
            private Vector3 endCameraPosition;

            private void Update()
            {
                ResolveOwner();
                if (owner == null)
                    return;

                if (restoreCameraWhenHidden && !owner.visible)
                {
                    owner.RestoreRegularMainMenuCameraPose();
                    restoreCameraWhenHidden = false;
                }

                if (transitionActive)
                {
                    TickTransition();
                    return;
                }

                if (!CanStartTransition())
                    return;

                BDModernHandheldControlTarget pressedTarget =
                    ResolvePressedPrimaryTarget();
                bool primaryShortcut = ReadPrimaryPressed();
                bool selectedConfirm =
                    IsPrimaryRowSelected() && ReadConfirmPressed();

                if (!primaryShortcut &&
                    !selectedConfirm &&
                    pressedTarget == null)
                {
                    return;
                }

                BeginTransition(pressedTarget);
            }

            private void OnDisable()
            {
                if (transitionActive)
                    AbortTransition();
            }

            private void ResolveOwner()
            {
                if (owner != null)
                    return;

                owner = UnityEngine.Object.FindFirstObjectByType<
                    BDModernHandheld3DPresenter>();
            }

            private bool CanStartTransition()
            {
                return owner.visible &&
                       owner.presentationReady &&
                       owner.flow != null &&
                       owner.displayedPage == EffectivePage.MainMenu &&
                       owner.IsMenuInputReady() &&
                       !owner.IsIntroToMainMenuTransitionBlockingInput();
            }

            private bool IsPrimaryRowSelected()
            {
                return owner.selectedIndex >= 0 &&
                       owner.selectedIndex < owner.rows.Count &&
                       owner.rows[owner.selectedIndex].action ==
                           RowAction.Primary;
            }

            private BDModernHandheldControlTarget
                ResolvePressedPrimaryTarget()
            {
                if (!ReadPointerPressed() ||
                    owner.deviceCamera == null)
                {
                    return null;
                }

                Vector2 pointerPosition;
                if (!TryReadPointerPosition(out pointerPosition))
                    return null;

                Ray ray = owner.deviceCamera.ScreenPointToRay(
                    pointerPosition
                );
                RaycastHit hit;
                if (!Physics.Raycast(
                        ray,
                        out hit,
                        60f,
                        1 << DeviceLayer,
                        QueryTriggerInteraction.Ignore))
                {
                    return null;
                }

                BDModernHandheldControlTarget target =
                    hit.collider.GetComponent<
                        BDModernHandheldControlTarget>();
                if (target == null)
                    return null;

                switch (target.Action)
                {
                    case BDModernHandheldControlTarget.ControlAction.Primary:
                        return target;

                    case BDModernHandheldControlTarget.ControlAction.ScreenItem:
                        if (target.Index >= 0 &&
                            target.Index < owner.rows.Count &&
                            owner.rows[target.Index].action ==
                                RowAction.Primary)
                        {
                            owner.selectedIndex = target.Index;
                            owner.UpdateSelectionVisuals();
                            return target;
                        }
                        break;

                    case BDModernHandheldControlTarget.ControlAction.Confirm:
                        if (IsPrimaryRowSelected())
                            return target;
                        break;
                }

                return null;
            }

            private void BeginTransition(
                BDModernHandheldControlTarget pressedTarget)
            {
                if (owner.deviceCamera == null ||
                    owner.deviceVisualRoot == null ||
                    owner.flow == null)
                {
                    return;
                }

                transitionActive = true;
                restoreCameraWhenHidden = false;
                startedAt = Time.unscaledTime;

                owner.menuInputUnlockAt = float.PositiveInfinity;
                owner.menuInputNeedsRelease = true;
                owner.screenTransitionActive = false;

                if (pressedTarget != null)
                    pressedTarget.Pulse(0.18f);
                else
                {
                    owner.PulsePersistentControl(
                        BDModernHandheldControlTarget.ControlAction.Primary
                    );
                }

                owner.PlayClick();

                Camera camera = owner.deviceCamera;
                startCameraPosition = camera.transform.position;
                startCameraRotation = camera.transform.rotation;
                startFieldOfView = camera.fieldOfView;
                startNearClip = camera.nearClipPlane;

                Vector3 screenWorld =
                    owner.deviceVisualRoot.TransformPoint(
                        new Vector3(0f, ScreenCenterY, -0.555f)
                    );
                Vector3 forward = camera.transform.forward.normalized;
                float forwardDistance = Mathf.Max(
                    0.1f,
                    Vector3.Dot(
                        screenWorld - startCameraPosition,
                        forward
                    )
                );
                float travel = Mathf.Max(
                    0f,
                    forwardDistance -
                    StartGameTransitionNearScreenDistance
                );
                Vector3 forwardEnd =
                    startCameraPosition + forward * travel;
                Vector3 centeredEnd =
                    screenWorld - forward *
                    StartGameTransitionNearScreenDistance;
                endCameraPosition = Vector3.Lerp(
                    forwardEnd,
                    centeredEnd,
                    0.88f
                );

                PrepareTransitionOverlay();
            }

            private void PrepareTransitionOverlay()
            {
                if (owner.screenTransitionRoot != null)
                {
                    owner.screenTransitionRoot.gameObject.SetActive(true);
                    owner.screenTransitionRoot.SetAsLastSibling();
                }

                if (owner.transitionTopShutter != null)
                {
                    owner.transitionTopShutter.rectTransform.sizeDelta =
                        new Vector2(CanvasSize.x, 0f);
                }

                if (owner.transitionBottomShutter != null)
                {
                    owner.transitionBottomShutter.rectTransform.sizeDelta =
                        new Vector2(CanvasSize.x, 0f);
                }

                SetImageAlpha(owner.transitionFlash, 0f);
                SetImageAlpha(owner.transitionLine, 0f);
            }

            private void TickTransition()
            {
                if (owner == null ||
                    owner.deviceCamera == null ||
                    owner.flow == null ||
                    !owner.visible ||
                    owner.displayedPage != EffectivePage.MainMenu)
                {
                    AbortTransition();
                    return;
                }

                float normalized = Mathf.Clamp01(
                    (Time.unscaledTime - startedAt) /
                    StartGameTransitionDuration
                );
                float cameraPhase = Mathf.InverseLerp(
                    0.10f,
                    0.94f,
                    normalized
                );
                float eased = SmoothestStep01(cameraPhase);

                Camera camera = owner.deviceCamera;
                camera.transform.position = Vector3.Lerp(
                    startCameraPosition,
                    endCameraPosition,
                    eased
                );
                camera.transform.rotation = startCameraRotation;
                camera.fieldOfView = Mathf.Lerp(
                    startFieldOfView,
                    17.5f,
                    eased
                );
                camera.nearClipPlane = Mathf.Lerp(
                    startNearClip,
                    0.018f,
                    eased
                );

                float screenCommit = Mathf.InverseLerp(
                    0.52f,
                    1f,
                    normalized
                );
                float flash = Mathf.Sin(
                    screenCommit * Mathf.PI
                ) * 0.34f;
                float line = Mathf.Sin(
                    Mathf.Clamp01(screenCommit * 1.18f) * Mathf.PI
                );

                SetImageAlpha(owner.transitionFlash, flash);
                SetImageAlpha(owner.transitionLine, line * 0.92f);

                if (owner.transitionLine != null)
                {
                    owner.transitionLine.rectTransform.sizeDelta =
                        new Vector2(
                            CanvasSize.x,
                            Mathf.Lerp(3f, 18f, line)
                        );
                }

                if (owner.pageCanvasGroup != null)
                {
                    owner.pageCanvasGroup.alpha = Mathf.Lerp(
                        1f,
                        0.58f,
                        Mathf.InverseLerp(0.72f, 1f, normalized)
                    );
                }

                if (normalized < 1f)
                    return;

                CompleteTransition();
            }

            private void CompleteTransition()
            {
                transitionActive = false;
                ClearTransitionOverlay();

                BDMainMenuFlow destinationFlow = owner.flow;
                if (destinationFlow == null)
                {
                    AbortTransition();
                    return;
                }

                destinationFlow.HandleModernPrimaryAction();

                if (destinationFlow.IsRunActive)
                    restoreCameraWhenHidden = true;
            }

            private void AbortTransition()
            {
                transitionActive = false;
                ClearTransitionOverlay();

                if (owner == null)
                    return;

                owner.RestoreRegularMainMenuCameraPose();
                owner.menuInputUnlockAt = Time.unscaledTime + 0.14f;
                owner.menuInputNeedsRelease = true;
            }

            private void ClearTransitionOverlay()
            {
                if (owner == null)
                    return;

                SetImageAlpha(owner.transitionFlash, 0f);
                SetImageAlpha(owner.transitionLine, 0f);

                if (owner.pageCanvasGroup != null)
                    owner.pageCanvasGroup.alpha = 1f;

                if (owner.screenTransitionRoot != null)
                    owner.screenTransitionRoot.gameObject.SetActive(false);
            }

            private static void SetImageAlpha(
                UnityEngine.UI.Image image,
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
}
