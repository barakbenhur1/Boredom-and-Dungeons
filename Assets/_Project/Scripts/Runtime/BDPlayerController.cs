using UnityEngine;

#pragma warning disable 0414

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(BDPlayerMarker))]
    [RequireComponent(typeof(BDPlayerHazardRecovery))]
    public sealed class BDPlayerController : MonoBehaviour
    {
        private enum DodgeDirection { None, Forward, Backward, Left, Right }

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float moveAcceleration = 34f;
        [SerializeField] private float moveDeceleration = 42f;
        [SerializeField] private float bodyRotationSmoothing = 34f;

        [Header("Mouse Point Aim")]
        [SerializeField] private float mouseAimMinDistance = 1.85f; // expanded center dead zone radius
        [SerializeField] private float mouseScreenCenterDeadZonePixels = 70f;
        [SerializeField] private float mouseFrontDeadZoneDegrees = 3f;
        [SerializeField] private float mouseModelFrontConeDegrees = 60f;
        [SerializeField] private float mouseAimIdleTurnDegreesPerSecond = 185f;
        [SerializeField] private float mouseAimMovingTurnDegreesPerSecond = 275f;
        [SerializeField] private float mouseAimTargetSmoothing = 26f;
        [SerializeField] private float mousePointAimSmoothing = 60f;
        // BD MOUSE-INTENT DEADZONE FIX
        [SerializeField] private float mouseIntentDeadZonePixels = 9f;

        [Header("Jump")]
        [SerializeField] private float jumpHeight = 1.35f;
        [SerializeField] private float gravity = -24f;

        [Header("Dodge / Double Tap")]
        [SerializeField] private float dashDistance = 3.05f;
        [SerializeField] private float dashDuration = 0.12f;
        [SerializeField] private float dashCooldown = 0.32f;
        [SerializeField] private float doubleTapDashWindow = 0.30f;
        [SerializeField] private float dodgeInvulnerabilityExtraTime = 0.14f;
        [SerializeField] private float dodgeAfterimageInterval = 0.035f;
        [SerializeField] private bool spawnDodgeAfterimages = true;
        // BD FORWARD DODGE REAR VISUAL OFFSET
        [SerializeField] private float forwardDodgeVisualRearOffset = 0.82f;

        [Header("Touch")]
        [SerializeField] private bool enableTouchDragAsFingerTrace = true;
        [SerializeField] private float dragDeadZonePixels = 8f;
        [SerializeField] private float dragMaxPixels = 150f;

        [Header("Debug")]
        [SerializeField] private bool showDebugOverlay = false;

        private CharacterController characterController;

        // BD BOOST API: movement multiplier
        private float boostMoveSpeedMultiplier = 1f;

        private float verticalVelocity;
        private Vector3 dashVelocity;
        private Vector3 smoothedHorizontalVelocity;
        private float dashTimer;
        private float dashCooldownTimer;
        private float dodgeInvulnerableUntil;
        private float lastDodgeStartedAt;
        private float lastJumpStartedAt = -999f;
        private float nextDodgeAfterimageAt;

        private float lastForwardTapTime = -999f;
        private float lastBackwardTapTime = -999f;
        private float lastLeftTapTime = -999f;
        private float lastRightTapTime = -999f;
        private DodgeDirection lastDodgeDirection = DodgeDirection.None;

        private Vector2 lastMoveInput;
        private Vector3 lastLookDirection = Vector3.forward;
        private Vector3 targetLookDirection = Vector3.forward;
        private Vector3 smoothedTargetLookDirection = Vector3.forward;
        private Vector3 movementReferenceForward = Vector3.forward;
        private string lastInputSource = "none";
        private string lastLookSource = "mouse-point";
        private Vector2 lastMouseIntentPosition;
        private bool hasMouseIntentPosition;

        public Vector2 LastMoveInput => lastMoveInput;
        public bool HasMoveInput => lastMoveInput.sqrMagnitude > 0.0001f;
        public bool IsDodging => dashTimer > 0f;
        public bool ShouldLockCameraYawForDodge => dashTimer > 0f;
        public Vector3 LastMoveWorldDirection
        {
            get
            {
                Vector3 direction = smoothedHorizontalVelocity;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.0025f)
                    return direction.normalized;

                return Vector3.zero;
            }
        }
        public Vector3 LastLookDirection => lastLookDirection;
        public bool IsDodgeInvulnerable => Time.time < dodgeInvulnerableUntil;
        public float DodgeInvulnerableRemaining => Mathf.Max(0f, dodgeInvulnerableUntil - Time.time);
        public float DodgeInvulnerableProgress01
        {
            get
            {
                float duration = Mathf.Max(0.01f, dodgeInvulnerableUntil - lastDodgeStartedAt);
                return Mathf.Clamp01(1f - (DodgeInvulnerableRemaining / duration));
            }
        }
        public bool IsDashing => dashTimer > 0f;
        public bool HasRecentIntentionalGapEntry =>
            IsDashing ||
            Time.time - lastDodgeStartedAt <= 0.40f ||
            Time.time - lastJumpStartedAt <= 0.75f;
        public float EffectiveMoveSpeed =>
            Mathf.Max(0.1f, moveSpeed * boostMoveSpeedMultiplier);

        public void SetBoostMoveSpeedMultiplier(float multiplier)
        {
            boostMoveSpeedMultiplier = Mathf.Max(0.1f, multiplier);
        }
        public void ResetMotionAfterExternalTeleport()
        {
            verticalVelocity = -2f;
            dashVelocity = Vector3.zero;
            smoothedHorizontalVelocity = Vector3.zero;
            dashTimer = 0f;
            dashCooldownTimer = 0f;
            dodgeInvulnerableUntil = 0f;
            lastMoveInput = Vector2.zero;
            lastJumpStartedAt = -999f;
        }


        private void Awake()
        {
            characterController = GetComponent<CharacterController>();

            if (GetComponent<BDPlayerHazardRecovery>() == null)
                gameObject.AddComponent<BDPlayerHazardRecovery>();
        }

        private void Start()
        {
            Vector3 forward = transform.forward;
            forward.y = 0f;

            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.forward;

            lastLookDirection = forward.normalized;
            targetLookDirection = lastLookDirection;
            smoothedTargetLookDirection = lastLookDirection;
        }

        private void Update()
        {
            Vector2 moveInput = ReadMoveInput();
            lastMoveInput = moveInput;

            bool wantsMove = moveInput.sqrMagnitude > 0.0001f;

            if (ShouldAcceptMouseAimUpdate())
            {
                Vector3 aimTarget = ResolveMouseAimDirection();

                if (aimTarget.sqrMagnitude > 0.001f)
                    targetLookDirection = SmoothAimTargetDirection(aimTarget.normalized);
            }

            float turnSpeed = wantsMove ? mouseAimMovingTurnDegreesPerSecond : mouseAimIdleTurnDegreesPerSecond;
            lastLookDirection = TurnAimGradually(lastLookDirection, targetLookDirection, turnSpeed);

            RotateToward(lastLookDirection);

            ReadDoubleTapDodge();

            if (ReadJumpPressed())
                TryJump();

            TickGravity();
            TickDash();

            Vector3 worldMove = wantsMove ? ToPlayerRelativeMove(moveInput) : Vector3.zero;
            Vector3 horizontalVelocity = SmoothHorizontalVelocity(worldMove, wantsMove);
            Vector3 velocity = horizontalVelocity + dashVelocity + Vector3.up * verticalVelocity;

            characterController.Move(velocity * Time.deltaTime);
            lastLookSource = wantsMove ? "mouse-clamped-60-front-cone-moving" : "mouse-clamped-60-front-cone-idle";
        }

        private bool ShouldAcceptMouseAimUpdate()
        {
            Vector2 mousePosition = ReadMouseScreenPosition();

            if (!hasMouseIntentPosition)
            {
                lastMouseIntentPosition = mousePosition;
                hasMouseIntentPosition = true;
                return true;
            }

            float minPixels = Mathf.Max(0.5f, mouseIntentDeadZonePixels);
            float sqrDelta =
                (mousePosition - lastMouseIntentPosition).sqrMagnitude;

            if (sqrDelta < minPixels * minPixels)
                return false;

            lastMouseIntentPosition = mousePosition;
            return true;
        }

        private static Vector2 ReadMouseScreenPosition()
        {
#if ENABLE_INPUT_SYSTEM
            Mouse mouse = Mouse.current;

            if (mouse != null)
                return mouse.position.ReadValue();
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.mousePosition;
#else
            return new Vector2(
                Screen.width * 0.5f,
                Screen.height * 0.5f
            );
#endif
        }

        private Vector3 ResolveMouseAimDirection()
        {
            if (BDMouseAimUtility.IsMouseInsideScreenCenterDeadZone(transform, mouseScreenCenterDeadZonePixels))
                return targetLookDirection.sqrMagnitude > 0.001f ? targetLookDirection.normalized : lastLookDirection.normalized;

            if (BDMouseAimUtility.TryGetMouseAimDirection(transform, mouseAimMinDistance, out Vector3 aim))
                return ApplyFrontMouseDeadZone(aim);

            Vector3 fallback = targetLookDirection.sqrMagnitude > 0.001f ? targetLookDirection : lastLookDirection.sqrMagnitude > 0.001f ? lastLookDirection : transform.forward;
            fallback.y = 0f;

            if (fallback.sqrMagnitude < 0.001f)
                fallback = Vector3.forward;

            return fallback.normalized;
        }

        private Vector3 ApplyFrontMouseDeadZone(Vector3 rawAimDirection)
        {
            rawAimDirection.y = 0f;

            if (rawAimDirection.sqrMagnitude < 0.001f)
                return lastLookDirection.sqrMagnitude > 0.001f ? lastLookDirection.normalized : transform.forward;

            rawAimDirection.Normalize();

            Vector3 frontBasis = ResolveModelFrontConeBasis();
            Vector3 clampedAim = ClampDirectionToFrontCone(rawAimDirection, frontBasis, mouseModelFrontConeDegrees);

            Vector3 currentForward = targetLookDirection.sqrMagnitude > 0.001f ? targetLookDirection : lastLookDirection.sqrMagnitude > 0.001f ? lastLookDirection : frontBasis;
            currentForward.y = 0f;

            if (currentForward.sqrMagnitude < 0.001f)
                return clampedAim;

            currentForward.Normalize();

            float angle = Vector3.Angle(currentForward, clampedAim);

            // Tiny mouse movement inside the front arc should not constantly jitter the model.
            if (angle <= mouseFrontDeadZoneDegrees)
                return currentForward;

            return clampedAim;
        }

        private Vector3 ResolveModelFrontConeBasis()
        {
            Vector3 basis = ResolveMovementReferenceForward();
            basis.y = 0f;

            if (basis.sqrMagnitude < 0.001f)
            {
                basis = transform.forward;
                basis.y = 0f;
            }

            if (basis.sqrMagnitude < 0.001f)
                basis = Vector3.forward;

            return basis.normalized;
        }

        private Vector3 ClampDirectionToFrontCone(Vector3 direction, Vector3 frontBasis, float coneDegrees)
        {
            direction.y = 0f;
            frontBasis.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return frontBasis.sqrMagnitude > 0.001f ? frontBasis.normalized : Vector3.forward;

            if (frontBasis.sqrMagnitude < 0.001f)
                frontBasis = Vector3.forward;

            direction.Normalize();
            frontBasis.Normalize();

            float halfCone = Mathf.Clamp(coneDegrees * 0.5f, 1f, 179f);
            float signedAngle = Vector3.SignedAngle(frontBasis, direction, Vector3.up);
            float clampedAngle = Mathf.Clamp(signedAngle, -halfCone, halfCone);

            Vector3 clamped = Quaternion.AngleAxis(clampedAngle, Vector3.up) * frontBasis;
            clamped.y = 0f;

            if (clamped.sqrMagnitude < 0.001f)
                return frontBasis;

            return clamped.normalized;
        }


        private Vector3 SmoothAimDirection(Vector3 rawDirection)
        {
            rawDirection.y = 0f;

            if (rawDirection.sqrMagnitude < 0.001f)
                return lastLookDirection.sqrMagnitude > 0.001f ? lastLookDirection.normalized : transform.forward;

            rawDirection.Normalize();

            Vector3 current = lastLookDirection;
            current.y = 0f;

            if (current.sqrMagnitude < 0.001f)
                return rawDirection;

            current.Normalize();

            float t = 1f - Mathf.Exp(-mousePointAimSmoothing * Time.deltaTime);
            Vector3 smoothed = Vector3.Slerp(current, rawDirection, t);
            smoothed.y = 0f;

            if (smoothed.sqrMagnitude < 0.001f)
                return rawDirection;

            return smoothed.normalized;
        }


        private Vector3 ToPlayerRelativeMove(Vector2 input)
        {
            Vector3 cameraForward = ResolveMovementReferenceForward();
            Vector3 mouseForward = ResolveMovementInputForward();
            Vector3 cameraRight = new Vector3(cameraForward.z, 0f, -cameraForward.x);

            Vector3 move = Vector3.zero;

            // Only forward movement is allowed to follow the mouse/model direction.
            // Backward and side movement stay camera/screen relative.
            if (input.y > 0.001f)
                move += mouseForward * input.y;
            else if (input.y < -0.001f)
                move += cameraForward * input.y;

            move += cameraRight * input.x;
            move.y = 0f;

            if (move.sqrMagnitude > 1f)
                move.Normalize();

            return move;
        }

        private Vector3 ResolveMovementInputForward()
        {
            Vector3 basis = ResolveMovementReferenceForward();
            Vector3 forward = Vector3.zero;

            if (targetLookDirection.sqrMagnitude > 0.001f)
                forward = targetLookDirection;
            else if (lastLookDirection.sqrMagnitude > 0.001f)
                forward = lastLookDirection;

            forward.y = 0f;

            if (forward.sqrMagnitude < 0.001f)
                forward = basis;

            // Movement follows the mouse/model direction, but only inside the same 60-degree front cone.
            forward = ClampDirectionToFrontCone(forward, basis, mouseModelFrontConeDegrees);

            if (forward.sqrMagnitude < 0.001f)
                return basis.sqrMagnitude > 0.001f ? basis.normalized : Vector3.forward;

            return forward.normalized;
        }

        private Vector3 ResolveMovementReferenceForward()
        {
            Camera camera = Camera.main;
            Vector3 forward = Vector3.zero;

            if (camera != null)
            {
                forward = camera.transform.forward;
                forward.y = 0f;
            }

            if (forward.sqrMagnitude < 0.001f)
            {
                forward = transform.forward;
                forward.y = 0f;
            }

            if (forward.sqrMagnitude < 0.001f)
                forward = movementReferenceForward.sqrMagnitude > 0.001f ? movementReferenceForward : Vector3.forward;

            forward.Normalize();

            movementReferenceForward = Vector3.Slerp(
                movementReferenceForward.sqrMagnitude > 0.001f ? movementReferenceForward.normalized : forward,
                forward,
                1f - Mathf.Exp(-18f * Time.deltaTime)
            );

            movementReferenceForward.y = 0f;

            if (movementReferenceForward.sqrMagnitude < 0.001f)
                movementReferenceForward = forward;

            return movementReferenceForward.normalized;
        }

        private Vector3 ToMouseRelativeMove(Vector2 input, Vector3 aimForward)
        {
            if (input.sqrMagnitude < 0.0001f)
                return Vector3.zero;

            Vector3 forward = aimForward;
            forward.y = 0f;

            if (forward.sqrMagnitude < 0.001f)
                forward = transform.forward;

            forward.y = 0f;

            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.forward;

            forward.Normalize();

            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
            Vector3 world = forward * input.y + right * input.x;

            return world.sqrMagnitude > 1f ? world.normalized : world;
        }



        private Vector3 SmoothAimTargetDirection(Vector3 rawTargetDirection)
        {
            rawTargetDirection.y = 0f;

            if (rawTargetDirection.sqrMagnitude < 0.001f)
                return targetLookDirection.sqrMagnitude > 0.001f ? targetLookDirection.normalized : lastLookDirection;

            rawTargetDirection.Normalize();

            if (smoothedTargetLookDirection.sqrMagnitude < 0.001f)
                smoothedTargetLookDirection = rawTargetDirection;

            smoothedTargetLookDirection.y = 0f;
            smoothedTargetLookDirection.Normalize();

            float t = 1f - Mathf.Exp(-mouseAimTargetSmoothing * Time.deltaTime);
            smoothedTargetLookDirection = Vector3.Slerp(smoothedTargetLookDirection, rawTargetDirection, t);
            smoothedTargetLookDirection.y = 0f;

            if (smoothedTargetLookDirection.sqrMagnitude < 0.001f)
                smoothedTargetLookDirection = rawTargetDirection;

            return smoothedTargetLookDirection.normalized;
        }

        private Vector3 TurnAimGradually(Vector3 currentDirection, Vector3 targetDirection, float degreesPerSecond)
        {
            currentDirection.y = 0f;
            targetDirection.y = 0f;

            if (targetDirection.sqrMagnitude < 0.001f)
                return currentDirection.sqrMagnitude > 0.001f ? currentDirection.normalized : transform.forward;

            targetDirection.Normalize();

            if (currentDirection.sqrMagnitude < 0.001f)
                return targetDirection;

            currentDirection.Normalize();

            float maxRadiansDelta = Mathf.Deg2Rad * Mathf.Max(1f, degreesPerSecond) * Time.deltaTime;
            Vector3 turned = Vector3.RotateTowards(currentDirection, targetDirection, maxRadiansDelta, 0f);
            turned.y = 0f;

            if (turned.sqrMagnitude < 0.001f)
                return targetDirection;

            return turned.normalized;
        }

        private Vector2 ReadMoveInput()
        {
            Vector2 keyboard = ReadKeyboardMove();
            Vector2 touch = ReadTouchMove();

            if (touch.sqrMagnitude > keyboard.sqrMagnitude)
            {
                lastInputSource = "touch";
                return Clamp(touch);
            }

            lastInputSource = keyboard.sqrMagnitude > 0.0001f ? "keyboard/gamepad" : "none";
            return Clamp(keyboard);
        }

        private static Vector2 Clamp(Vector2 value)
        {
            return value.sqrMagnitude > 1f ? value.normalized : value;
        }

        private Vector2 ReadKeyboardMove()
        {
            Vector2 move = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) move.x -= 1f;
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) move.x += 1f;
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) move.y -= 1f;
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) move.y += 1f;
            }

            Gamepad gamepad = Gamepad.current;
            if (gamepad != null)
            {
                Vector2 stick = gamepad.leftStick.ReadValue();
                if (stick.sqrMagnitude > move.sqrMagnitude)
                    move = stick;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) move.x -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) move.x += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) move.y -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) move.y += 1f;
#endif

            return move;
        }

        private Vector2 ReadTouchMove()
        {
            Vector2 move = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
            Touchscreen touchscreen = Touchscreen.current;
            if (enableTouchDragAsFingerTrace && touchscreen != null && touchscreen.primaryTouch.press.isPressed)
            {
                Vector2 start = touchscreen.primaryTouch.startPosition.ReadValue();
                Vector2 current = touchscreen.primaryTouch.position.ReadValue();
                Vector2 delta = current - start;

                if (delta.magnitude > dragDeadZonePixels)
                    move = delta / Mathf.Max(1f, dragMaxPixels);
            }
#endif

            return move;
        }

        private bool ReadJumpPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) return true;
            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) return true;
#endif

            return false;
        }

        private void ReadDoubleTapDodge()
        {
            if (dashTimer > 0f || dashCooldownTimer > 0f)
                return;

            Vector3 cameraForward = ResolveMovementReferenceForward();
            cameraForward.y = 0f;

            if (cameraForward.sqrMagnitude < 0.001f)
                cameraForward = transform.forward;

            cameraForward.y = 0f;

            if (cameraForward.sqrMagnitude < 0.001f)
                cameraForward = Vector3.forward;

            cameraForward.Normalize();

            Vector3 mouseForward = ResolveMovementInputForward();
            mouseForward.y = 0f;

            if (mouseForward.sqrMagnitude < 0.001f)
                mouseForward = cameraForward;

            mouseForward.Normalize();

            Vector3 cameraRight = new Vector3(cameraForward.z, 0f, -cameraForward.x);

            // Only forward dodge follows the mouse/front-cone direction.
            // Back/left/right dodges stay key/camera relative.
            if (WasForwardPressedThisFrame())
                HandleDirectionalTap(DodgeDirection.Forward, mouseForward, ref lastForwardTapTime);

            if (WasBackwardPressedThisFrame())
                HandleDirectionalTap(DodgeDirection.Backward, -cameraForward, ref lastBackwardTapTime);

            if (WasLeftPressedThisFrame())
                HandleDirectionalTap(DodgeDirection.Left, -cameraRight, ref lastLeftTapTime);

            if (WasRightPressedThisFrame())
                HandleDirectionalTap(DodgeDirection.Right, cameraRight, ref lastRightTapTime);
        }

        private void HandleDirectionalTap(DodgeDirection direction, Vector3 dodgeDirection, ref float lastTapTime)
        {
            float now = Time.time;

            if (now - lastTapTime <= doubleTapDashWindow)
            {
                TryDash(dodgeDirection, direction);
                lastTapTime = -999f;
                return;
            }

            lastTapTime = now;
        }

        private bool WasForwardPressedThisFrame()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame)) return true;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) return true;
#endif
            return false;
        }

        private bool WasBackwardPressedThisFrame()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)) return true;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) return true;
#endif
            return false;
        }

        private bool WasLeftPressedThisFrame()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && (keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame)) return true;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) return true;
#endif
            return false;
        }

        private bool WasRightPressedThisFrame()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && (keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame)) return true;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) return true;
#endif
            return false;
        }

        private void TickGravity()
        {
            if (characterController.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f;

            verticalVelocity += gravity * Time.deltaTime;
        }

        private void TryJump()
        {
            if (!characterController.isGrounded)
                return;

            lastJumpStartedAt = Time.time;
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        private void TryDash(Vector3 direction, DodgeDirection dodgeDirection)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                direction = lastLookDirection;

            direction.Normalize();

            dashVelocity = direction * (dashDistance / Mathf.Max(0.01f, dashDuration));
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            lastDodgeStartedAt = Time.time;
            dodgeInvulnerableUntil = Time.time + dashDuration + Mathf.Max(0f, dodgeInvulnerabilityExtraTime);
            nextDodgeAfterimageAt = 0f;
            lastDodgeDirection = dodgeDirection;
            SpawnDodgeAfterimageIfNeeded();
            BDGameFeelAudio.PlayDodge();
        }

        private void TickDash()
        {
            if (dashCooldownTimer > 0f)
                dashCooldownTimer -= Time.deltaTime;

            if (dashTimer > 0f)
            {
                SpawnDodgeAfterimageIfNeeded();

                dashTimer -= Time.deltaTime;

                if (dashTimer <= 0f)
                    dashVelocity = Vector3.zero;
            }
        }

        private void SpawnDodgeAfterimageIfNeeded()
        {
            if (!spawnDodgeAfterimages)
                return;

            if (Time.time < nextDodgeAfterimageAt)
                return;

            nextDodgeAfterimageAt = Time.time + Mathf.Max(0.015f, dodgeAfterimageInterval);

            float radius = characterController != null ? characterController.radius : 0.45f;
            float height = characterController != null ? characterController.height : 1.8f;

            Quaternion visualRotation = transform.rotation;
            Vector3 dashDirection = dashVelocity;
            dashDirection.y = 0f;

            Vector3 visualPosition = transform.position;

            if (dashDirection.sqrMagnitude > 0.001f)
            {
                Vector3 normalizedDashDirection =
                    dashDirection.normalized;

                visualRotation =
                    Quaternion.LookRotation(
                        normalizedDashDirection,
                        Vector3.up
                    );

                if (lastDodgeDirection ==
                    DodgeDirection.Forward)
                {
                    visualPosition -=
                        normalizedDashDirection *
                        Mathf.Max(
                            0f,
                            forwardDodgeVisualRearOffset
                        );
                }
            }

            BDPlayerDodgeAfterimage.Spawn(
                visualPosition,
                visualRotation,
                radius,
                height
            );
        }


        private Vector3 SmoothHorizontalVelocity(Vector3 desiredMoveDirection, bool wantsMove)
        {
            desiredMoveDirection.y = 0f;

            Vector3 desiredVelocity = Vector3.zero;

            if (wantsMove && desiredMoveDirection.sqrMagnitude > 0.001f)
                desiredVelocity = desiredMoveDirection.normalized * EffectiveMoveSpeed;

            float rate = desiredVelocity.sqrMagnitude > smoothedHorizontalVelocity.sqrMagnitude
                ? moveAcceleration
                : moveDeceleration;

            smoothedHorizontalVelocity = Vector3.MoveTowards(
                smoothedHorizontalVelocity,
                desiredVelocity,
                Mathf.Max(0.01f, rate) * Time.deltaTime
            );

            if (!wantsMove && smoothedHorizontalVelocity.sqrMagnitude < 0.0004f)
                smoothedHorizontalVelocity = Vector3.zero;

            return smoothedHorizontalVelocity;
        }

        private void RotateToward(Vector3 direction)
        {
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion target = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                target,
                1f - Mathf.Exp(-bodyRotationSmoothing * Time.deltaTime)
            );
        }

        private void OnGUI()
        {
            if (!showDebugOverlay)
                return;

            GUI.Box(new Rect(12, 12, 440, 190), "B&D Clean Core — Player");
            GUI.Label(new Rect(24, 42, 410, 22), $"Move X: {lastMoveInput.x:0.00}");
            GUI.Label(new Rect(24, 64, 410, 22), $"Move Y: {lastMoveInput.y:0.00}");
            GUI.Label(new Rect(24, 86, 410, 22), $"Look: {lastLookSource} | Dir: {lastLookDirection.x:0.00}, {lastLookDirection.z:0.00}");
            GUI.Label(new Rect(24, 108, 410, 22), $"Position X: {transform.position.x:0.00}");
            GUI.Label(new Rect(24, 130, 410, 22), $"Position Z: {transform.position.z:0.00}");
            GUI.Label(new Rect(24, 152, 410, 22), $"Input: {lastInputSource}");
            GUI.Label(new Rect(24, 174, 410, 22), $"Dodge: double-tap direction | Last: {lastDodgeDirection}");
        }
    }
}
