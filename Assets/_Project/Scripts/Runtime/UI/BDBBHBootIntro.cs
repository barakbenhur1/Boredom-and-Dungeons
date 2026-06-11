using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-10000)]
    [DisallowMultipleComponent]
    public sealed class BDBBHBootIntro : MonoBehaviour
    {
        private const int LetterCount = 3;
        private const string IntroText = "BBH";
        private const float VerticalScreenPositionFromTop = 0.45f;
        private const float CircleDiameterScale = 1.16f;
        private const float CircleMaximumScreenWidth = 0.78f;
        private const float CircleMaximumScreenHeight = 0.72f;
        private const float CircleSafeEdgeInset = 18f;
        private const float CircleActivationGatherFraction = 0.14f;

        // BD BBH SINGLE-LETTER SEQUENCE + VISIBLE GROWING FILLED CIRCLE V14
        // BD BBH GRAPHITE STEEL CIRCLE STYLE V16R
        // BD FIRST RENDER FRAME BLACK CLOCK V20
        // BD CINEMATIC CHARACTER MOTION + LARGER RESPONSIVE CIRCLE SIDE TASK V1
        private const float InitialBlackHold = 0.52f;
        private const float LetterDuration = 0.72f;
        private const float GapAfterLetter = 0.12f;
        private const float CircleGrowthDuration = 0.58f;
        private const float CircleFullHoldDuration = 0.50f;
        private const float FadeOutDuration = 0.18f;

        [Header("Composition")]
        [SerializeField, Range(0.50f, 1.20f)]
        private float letterSpacingMultiplier = 0.78f;

        [Header("Compact Depth")]
        [SerializeField, Range(0.01f, 0.08f)]
        private float extrusionDistanceMultiplier = 0.035f;
        [SerializeField, Range(3, 8)]
        private int pseudoDepthLayers = 5;
        [SerializeField]
        private Vector2 extrusionDirection = new Vector2(0.72f, 1f);
        [SerializeField, Range(0.01f, 0.10f)]
        private float shadowDistanceMultiplier = 0.045f;

        private static bool playedThisApplicationSession;
        private static bool currentlyPlaying;
        private static bool introToMainMenuTransitionRequested;
        private static bool skipToFinalStateRequested;
        private static BDBBHBootIntro activeInstance;

        private float startedAt;
        private bool renderClockStarted;
        private bool active;
        private GUIStyle letterStyle;
        private Texture2D solidTexture;
        private Texture2D filledCircleTexture;
        private Texture2D circleRimTexture;
        private Texture2D bootGradientTexture;
        private Texture2D bootGlowTexture;
        private Texture2D bootScanlineTexture;
        private Texture2D bootVignetteTexture;

        public static bool IsPlaying => currentlyPlaying;
        public static bool HasPlayedThisSession => playedThisApplicationSession;
        public static bool HasPendingIntroToMainMenuTransition =>
            introToMainMenuTransitionRequested;

        public static bool TryConsumeSkipToFinalStateRequest()
        {
            if (!skipToFinalStateRequested)
                return false;

            skipToFinalStateRequested = false;
            return true;
        }

        public static bool TryConsumeIntroToMainMenuTransition()
        {
            if (!introToMainMenuTransitionRequested)
                return false;

            introToMainMenuTransitionRequested = false;
            return true;
        }

        public static void CancelPendingIntroToMainMenuTransition()
        {
            introToMainMenuTransitionRequested = false;
        }

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetSessionState()
        {
            playedThisApplicationSession = false;
            currentlyPlaying = false;
            introToMainMenuTransitionRequested = false;
            skipToFinalStateRequested = false;
            activeInstance = null;
        }

        private void Awake()
        {
            if (playedThisApplicationSession ||
                (activeInstance != null && activeInstance != this))
            {
                enabled = false;
                return;
            }

            activeInstance = this;
            active = true;
            currentlyPlaying = true;
            startedAt = 0f;
            renderClockStarted = false;
            // Presentation timing is realtime/unscaled. The BBH intro must
            // never take ownership of the global simulation clock.

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnDisable()
        {
            if (activeInstance == this)
                activeInstance = null;

            if (!active)
                return;

            active = false;
            currentlyPlaying = false;
        }

        private void OnDestroy()
        {
            if (activeInstance == this)
                activeInstance = null;

            DestroyTexture(ref solidTexture);
            DestroyTexture(ref filledCircleTexture);
            DestroyTexture(ref circleRimTexture);
            DestroyTexture(ref bootGradientTexture);
            DestroyTexture(ref bootGlowTexture);
            DestroyTexture(ref bootScanlineTexture);
            DestroyTexture(ref bootVignetteTexture);
            letterStyle = null;
        }

        private void Update()
        {
            if (active && ReadEscapePressed())
            {
                skipToFinalStateRequested = true;
                CompleteIntro();
                return;
            }

            if (!active || !renderClockStarted || Elapsed < TotalDuration)
                return;

            CompleteIntro();
        }

        private static bool ReadEscapePressed()
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null &&
                   Keyboard.current.escapeKey.wasPressedThisFrame;
#else
            return Input.GetKeyDown(KeyCode.Escape);
#endif
        }

        private float Elapsed =>
            renderClockStarted
                ? Time.realtimeSinceStartup - startedAt
                : 0f;

        private static float PerLetterWindow =>
            LetterDuration + GapAfterLetter;

        private static float LetterStartTime(int index) =>
            InitialBlackHold + index * PerLetterWindow;

        private static float LettersEndTime =>
            LetterStartTime(LetterCount - 1) + LetterDuration;

        private static float CircleGrowthEndTime =>
            LettersEndTime + CircleGrowthDuration;

        private static float CircleHoldEndTime =>
            CircleGrowthEndTime + CircleFullHoldDuration;

        private static float FadeStartTime =>
            CircleHoldEndTime;

        private static float TotalDuration =>
            FadeStartTime + FadeOutDuration;

        private void CompleteIntro()
        {
            active = false;
            currentlyPlaying = false;
            playedThisApplicationSession = true;
            introToMainMenuTransitionRequested = true;
            enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnGUI()
        {
            if (!active)
                return;

            EnsureResources();

            // Start the visible timeline on the first actual repaint, not in
            // Awake. Scene loading can take longer than InitialBlackHold and
            // must never consume the first B animation before it is rendered.
            if (!renderClockStarted &&
                Event.current != null &&
                Event.current.type == EventType.Repaint)
            {
                startedAt = Time.realtimeSinceStartup;
                renderClockStarted = true;
            }

            int previousDepth = GUI.depth;
            Matrix4x4 previousMatrix = GUI.matrix;
            Color previousColor = GUI.color;

            GUI.depth = -10000;
            GUI.matrix = Matrix4x4.identity;

            float globalAlpha = ResolveGlobalAlpha();
            GUI.color = new Color(0f, 0f, 0f, globalAlpha);
            GUI.DrawTexture(
                new Rect(0f, 0f, Screen.width, Screen.height),
                solidTexture
            );

            DrawProfessionalBootSurface(globalAlpha);
            DrawComposition(globalAlpha);
            DrawProfessionalBootGlass(globalAlpha);

            if (Event.current != null &&
                (Event.current.isKey ||
                 Event.current.isMouse ||
                 Event.current.type == EventType.ScrollWheel))
            {
                Event.current.Use();
            }

            GUI.color = previousColor;
            GUI.matrix = previousMatrix;
            GUI.depth = previousDepth;
        }

        // BD PROFESSIONAL BBH BOOT SURFACE V23R19Q
        private void DrawProfessionalBootSurface(float globalAlpha)
        {
            if (Elapsed < InitialBlackHold)
                return;

            Color previousColor = GUI.color;
            Rect full = new Rect(0f, 0f, Screen.width, Screen.height);
            float surfaceReveal = SmoothStep01(
                Mathf.Clamp01(
                    (Elapsed - InitialBlackHold) / 0.22f
                )
            );

            GUI.color = new Color(
                1f,
                1f,
                1f,
                globalAlpha * surfaceReveal
            );
            GUI.DrawTexture(
                full,
                bootGradientTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );

            float reveal = surfaceReveal;

            float glowSize =
                Mathf.Min(Screen.width, Screen.height) * 0.74f;
            Vector2 glowCenter = new Vector2(
                Screen.width * 0.50f,
                Screen.height * VerticalScreenPositionFromTop
            );

            GUI.color = new Color(
                0.28f,
                0.50f,
                0.94f,
                globalAlpha * reveal * 0.15f
            );
            GUI.DrawTexture(
                new Rect(
                    glowCenter.x - glowSize * 0.5f,
                    glowCenter.y - glowSize * 0.5f,
                    glowSize,
                    glowSize
                ),
                bootGlowTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );

            GUI.color = previousColor;
        }

        private void DrawProfessionalBootGlass(float globalAlpha)
        {
            if (Elapsed < InitialBlackHold)
                return;

            Color previousColor = GUI.color;
            Rect full = new Rect(0f, 0f, Screen.width, Screen.height);
            float surfaceReveal = SmoothStep01(
                Mathf.Clamp01(
                    (Elapsed - InitialBlackHold) / 0.22f
                )
            );

            GUI.color = new Color(
                1f,
                1f,
                1f,
                globalAlpha * surfaceReveal * 0.34f
            );
            GUI.DrawTextureWithTexCoords(
                full,
                bootScanlineTexture,
                new Rect(
                    0f,
                    0f,
                    Mathf.Max(1f, Screen.width / 8f),
                    Mathf.Max(1f, Screen.height / 8f)
                ),
                alphaBlend: true
            );

            GUI.color = new Color(
                1f,
                1f,
                1f,
                globalAlpha * surfaceReveal
            );
            GUI.DrawTexture(
                full,
                bootVignetteTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );

            float inset = Mathf.Clamp(
                Mathf.Min(Screen.width, Screen.height) * 0.028f,
                12f,
                28f
            );
            Color frame = new Color(
                0.46f,
                0.66f,
                0.94f,
                globalAlpha * surfaceReveal * 0.10f
            );

            DrawBootLine(
                new Rect(inset, inset, Screen.width - inset * 2f, 1f),
                frame
            );
            DrawBootLine(
                new Rect(inset, Screen.height - inset - 1f, Screen.width - inset * 2f, 1f),
                frame
            );
            DrawBootLine(
                new Rect(inset, inset, 1f, Screen.height - inset * 2f),
                frame
            );
            DrawBootLine(
                new Rect(Screen.width - inset - 1f, inset, 1f, Screen.height - inset * 2f),
                frame
            );

            GUI.color = previousColor;
        }

        private void DrawBootLine(Rect rect, Color color)
        {
            Color previous = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, solidTexture);
            GUI.color = previous;
        }

        private void DrawComposition(float globalAlpha)
        {
            float screenHeight = Mathf.Max(360f, Screen.height);
            float finalFontSize =
                Mathf.Clamp(screenHeight * 0.145f, 72f, 188f);

            float spacing =
                finalFontSize *
                Mathf.Clamp(letterSpacingMultiplier, 0.50f, 1.20f);

            Vector2 compositionCenter = new Vector2(
                Screen.width * 0.50f,
                Screen.height * VerticalScreenPositionFromTop
            );

            float secondBProgress = Mathf.Clamp01(
                (Elapsed - LetterStartTime(1)) /
                LetterDuration
            );
            float hProgress = Mathf.Clamp01(
                (Elapsed - LetterStartTime(2)) /
                LetterDuration
            );
            float circleProgress = Mathf.Clamp01(
                (Elapsed - LettersEndTime) /
                Mathf.Max(0.01f, CircleGrowthDuration)
            );

            float secondBBump = ResolvePulse(
                secondBProgress,
                0.56f,
                0.96f
            );
            float hImpact = ResolvePulse(
                hProgress,
                0.68f,
                1.00f
            );
            float circleLift = ResolvePulse(
                circleProgress,
                0.12f,
                0.78f
            );

            // The circle is deliberately drawn before the letters so it always
            // grows behind the complete BBH mark, never over it.
            DrawGrowingFilledCircleBehindText(
                compositionCenter,
                finalFontSize,
                spacing,
                globalAlpha
            );

            float shimmerProgress = Mathf.Clamp01(
                (Elapsed - CircleGrowthEndTime) /
                Mathf.Max(0.01f, CircleFullHoldDuration)
            );

            for (int index = 0; index < LetterCount; index++)
            {
                float localProgress =
                    (Elapsed - LetterStartTime(index)) /
                    LetterDuration;

                // Preserve the strict B -> B -> H entrance contract. A later
                // letter is never drawn before its own start time.
                if (localProgress <= 0f)
                    continue;

                float progress = Mathf.Clamp01(localProgress);
                float finalX =
                    compositionCenter.x +
                    (index - 1) * spacing;

                ResolveLetterMotion(
                    index,
                    progress,
                    finalFontSize,
                    out Vector2 authoredOffset,
                    out float authoredScale,
                    out float authoredRotation
                );

                Vector2 center =
                    new Vector2(finalX, compositionCenter.y) +
                    authoredOffset +
                    ResolveInteractionOffset(
                        index,
                        finalFontSize,
                        secondBBump,
                        hImpact,
                        circleLift
                    );

                float scale =
                    authoredScale *
                    ResolveInteractionScale(
                        index,
                        secondBBump,
                        hImpact,
                        circleLift
                    );

                float rotation =
                    authoredRotation +
                    ResolveInteractionRotation(
                        index,
                        secondBBump,
                        hImpact
                    );

                DrawCompactDepth(
                    IntroText[index].ToString(),
                    center,
                    finalFontSize,
                    progress,
                    globalAlpha,
                    scale,
                    rotation
                );

                DrawFrontLetter(
                    IntroText[index].ToString(),
                    center,
                    finalFontSize,
                    progress,
                    globalAlpha,
                    shimmerProgress,
                    index,
                    scale,
                    rotation
                );
            }

            if (Elapsed >= CircleGrowthEndTime)
            {
                DrawCompletionLightSweep(
                    compositionCenter,
                    finalFontSize,
                    spacing,
                    globalAlpha,
                    shimmerProgress
                );
            }
        }

        // BD CINEMATIC CHARACTER MOTION + LARGER RESPONSIVE CIRCLE SIDE TASK V1
        private static void ResolveLetterMotion(
            int index,
            float progress,
            float finalFontSize,
            out Vector2 offset,
            out float scale,
            out float rotation)
        {
            float smooth = SmoothStep01(progress);
            float settleWindow = Mathf.Clamp01(
                (progress - 0.46f) / 0.54f
            );
            float settleWave = Mathf.Sin(settleWindow * Mathf.PI);

            switch (index)
            {
                case 0:
                    // First B: careful, curious and slightly searching.
                    scale = Mathf.LerpUnclamped(
                        0.035f,
                        1f,
                        EaseOutBackTuned(progress, 1.18f)
                    );
                    offset = new Vector2(
                        -finalFontSize * 0.080f * (1f - smooth) +
                        finalFontSize * 0.018f * settleWave,
                        finalFontSize * 0.045f * (1f - smooth) -
                        finalFontSize * 0.012f * settleWave
                    );
                    rotation =
                        Mathf.Lerp(-7.5f, 0f, smooth) +
                        settleWave * 1.35f;
                    break;

                case 1:
                    // Second B: faster and more confident, with a small rebound.
                    scale = Mathf.LerpUnclamped(
                        0.040f,
                        1f,
                        EaseOutBackTuned(progress, 1.62f)
                    );
                    offset = new Vector2(
                        finalFontSize * 0.160f * (1f - smooth) -
                        finalFontSize * 0.018f * settleWave,
                        -finalFontSize * 0.020f * (1f - smooth)
                    );
                    rotation =
                        Mathf.Lerp(6.5f, 0f, smooth) -
                        settleWave * 0.75f;
                    break;

                default:
                    // H: heavier, straighter and stabilizing.
                    float landingWeight = ResolvePulse(
                        progress,
                        0.62f,
                        0.98f
                    );
                    scale = Mathf.LerpUnclamped(
                        0.030f,
                        1f,
                        EaseOutBackTuned(progress, 0.82f)
                    );
                    offset = new Vector2(
                        0f,
                        -finalFontSize * 0.105f * (1f - smooth) +
                        finalFontSize * 0.028f * landingWeight
                    );
                    rotation = Mathf.Lerp(-2.4f, 0f, smooth);
                    break;
            }
        }

        private static Vector2 ResolveInteractionOffset(
            int index,
            float finalFontSize,
            float secondBBump,
            float hImpact,
            float circleLift)
        {
            Vector2 offset = Vector2.zero;

            if (index == 0)
                offset.x -= finalFontSize * 0.055f * secondBBump;
            else if (index == 1)
                offset.x += finalFontSize * 0.020f * secondBBump;

            offset.y += finalFontSize * 0.025f * hImpact;
            if (index < 2)
            {
                offset.x +=
                    (index == 0 ? -1f : 1f) *
                    finalFontSize * 0.012f * hImpact;
            }

            // The circle expansion gently pushes the completed letters forward.
            // In this screen-space presenter, that depth cue is represented by a
            // tiny lift plus scale response rather than a second camera owner.
            offset.y -= finalFontSize * 0.014f * circleLift;
            return offset;
        }

        private static float ResolveInteractionScale(
            int index,
            float secondBBump,
            float hImpact,
            float circleLift)
        {
            float scale = 1f;

            if (index == 0)
                scale *= 1f - secondBBump * 0.018f;
            else if (index == 1)
                scale *= 1f + secondBBump * 0.024f;

            scale *= index == 2
                ? 1f + hImpact * 0.032f
                : 1f - hImpact * 0.024f;

            scale *= 1f + circleLift * 0.018f;
            return scale;
        }

        private static float ResolveInteractionRotation(
            int index,
            float secondBBump,
            float hImpact)
        {
            float rotation = 0f;

            if (index == 0)
                rotation -= secondBBump * 2.8f;
            else if (index == 1)
                rotation += secondBBump * 1.2f;

            if (index == 0)
                rotation -= hImpact * 1.4f;
            else if (index == 1)
                rotation += hImpact * 1.4f;

            return rotation;
        }

        private static float ResolvePulse(
            float value,
            float start,
            float end)
        {
            if (end <= start || value <= start || value >= end)
                return 0f;

            float local = Mathf.Clamp01(
                (value - start) / (end - start)
            );
            return Mathf.Sin(local * Mathf.PI);
        }

        private void DrawCompactDepth(
            string letter,
            Vector2 center,
            float finalFontSize,
            float progress,
            float globalAlpha,
            float scale,
            float rotationDegrees)
        {
            float appear = SmoothStep01(
                Mathf.Clamp01(progress / 0.58f)
            );

            Vector2 direction =
                extrusionDirection.sqrMagnitude > 0.0001f
                    ? extrusionDirection.normalized
                    : new Vector2(0.58f, 0.82f);

            float distance =
                finalFontSize *
                Mathf.Clamp(
                    extrusionDistanceMultiplier,
                    0.01f,
                    0.08f
                ) *
                scale;

            int layers = Mathf.Clamp(pseudoDepthLayers, 3, 8);
            for (int layer = layers; layer >= 1; layer--)
            {
                float layer01 = layer / (float)layers;
                Vector2 offset = direction * distance * layer01;
                float alpha =
                    globalAlpha *
                    appear *
                    Mathf.Lerp(0.16f, 0.50f, layer01);

                DrawCenteredLabel(
                    letter,
                    center + offset,
                    finalFontSize,
                    scale,
                    new Color(0.10f, 0.16f, 0.27f, alpha),
                    rotationDegrees * 0.60f
                );
            }
        }

        private void DrawFrontLetter(
            string letter,
            Vector2 center,
            float finalFontSize,
            float progress,
            float globalAlpha,
            float shimmerProgress,
            int letterIndex,
            float scale,
            float rotation)
        {
            float alpha =
                globalAlpha *
                SmoothStep01(
                    Mathf.Clamp01(progress / 0.52f)
                );

            DrawCompactShadow(
                letter,
                center,
                finalFontSize,
                scale,
                alpha,
                rotation
            );

            // One authored completion breath replaces the previous perpetual
            // real-time pulse, keeping the finished logo composed and deterministic.
            float pulse =
                progress >= 1f
                    ? Mathf.Sin(
                        Mathf.Clamp01(shimmerProgress) *
                        Mathf.PI
                    )
                    : 0f;

            DrawCenteredLabel(
                letter,
                center,
                finalFontSize,
                scale * 1.055f,
                new Color(
                    0.27f,
                    0.48f,
                    0.86f,
                    alpha * (0.08f + pulse * 0.035f)
                ),
                rotation
            );

            DrawCenteredLabel(
                letter,
                center + new Vector2(1.2f, 1.2f),
                finalFontSize,
                scale,
                new Color(0.05f, 0.08f, 0.14f, alpha * 0.72f),
                rotation
            );

            float brightness = Mathf.Lerp(0.64f, 1f, progress);
            float metallicLift =
                Mathf.Clamp01(shimmerProgress * 1.4f) * 0.08f;

            DrawCenteredLabel(
                letter,
                center,
                finalFontSize,
                scale,
                new Color(
                    Mathf.Clamp01(brightness + metallicLift),
                    Mathf.Clamp01(brightness + metallicLift),
                    Mathf.Clamp01(
                        brightness + metallicLift * 1.3f
                    ),
                    alpha
                ),
                rotation
            );

            DrawCenteredLabel(
                letter,
                center + new Vector2(-1.4f, -1.6f),
                finalFontSize,
                scale * 0.987f,
                new Color(
                    0.86f,
                    0.93f,
                    1f,
                    alpha * Mathf.Lerp(0.42f, 0.12f, progress)
                ),
                rotation
            );
        }

        private void DrawCompactShadow(
            string letter,
            Vector2 center,
            float finalFontSize,
            float scale,
            float alpha,
            float rotationDegrees)
        {
            Vector2 direction = new Vector2(0.45f, 0.89f);
            float distance =
                finalFontSize *
                Mathf.Clamp(
                    shadowDistanceMultiplier,
                    0.01f,
                    0.10f
                );

            DrawCenteredLabel(
                letter,
                center + direction * distance,
                finalFontSize,
                scale * 1.015f,
                new Color(0f, 0f, 0f, alpha * 0.20f),
                rotationDegrees * 0.55f
            );
        }

        private void DrawGrowingFilledCircleBehindText(
            Vector2 center,
            float finalFontSize,
            float spacing,
            float globalAlpha)
        {
            if (Elapsed < LettersEndTime ||
                filledCircleTexture == null ||
                circleRimTexture == null)
            {
                return;
            }

            float progress = Mathf.Clamp01(
                (Elapsed - LettersEndTime) /
                CircleGrowthDuration
            );

            float desiredDiameter = Mathf.Max(
                finalFontSize * 3.15f,
                spacing * 4.15f
            ) * CircleDiameterScale;

            float screenFractionLimit = Mathf.Min(
                Screen.width * CircleMaximumScreenWidth,
                Screen.height * CircleMaximumScreenHeight
            );

            float horizontalBoundsLimit =
                2f * Mathf.Max(
                    0f,
                    Mathf.Min(
                        center.x - CircleSafeEdgeInset,
                        Screen.width - center.x - CircleSafeEdgeInset
                    )
                );
            float verticalBoundsLimit =
                2f * Mathf.Max(
                    0f,
                    Mathf.Min(
                        center.y - CircleSafeEdgeInset,
                        Screen.height - center.y - CircleSafeEdgeInset
                    )
                );

            float maximumDiameter = Mathf.Max(
                1f,
                Mathf.Min(
                    screenFractionLimit,
                    Mathf.Min(
                        horizontalBoundsLimit,
                        verticalBoundsLimit
                    )
                )
            );

            // Leave a small physical margin so the authored overshoot remains
            // visible without ever clipping against the responsive clamp.
            float settledDiameter = Mathf.Min(
                desiredDiameter,
                maximumDiameter / 1.055f
            );
            float pointDiameter = Mathf.Max(
                2f,
                finalFontSize * 0.028f
            );

            float diameter;
            if (progress < CircleActivationGatherFraction)
            {
                float gather = Mathf.Clamp01(
                    progress /
                    Mathf.Max(0.001f, CircleActivationGatherFraction)
                );
                float gatherPulse = Mathf.Sin(gather * Mathf.PI);
                diameter = pointDiameter *
                    (0.74f + gatherPulse * 0.38f);
            }
            else
            {
                float growth = Mathf.Clamp01(
                    (progress - CircleActivationGatherFraction) /
                    Mathf.Max(
                        0.001f,
                        1f - CircleActivationGatherFraction
                    )
                );
                float eased = EaseOutBackTuned(growth, 1.22f);
                diameter = Mathf.Min(
                    maximumDiameter,
                    Mathf.LerpUnclamped(
                        pointDiameter,
                        settledDiameter,
                        eased
                    )
                );
            }

            if (Elapsed >= CircleGrowthEndTime)
            {
                float holdProgress = Mathf.Clamp01(
                    (Elapsed - CircleGrowthEndTime) /
                    Mathf.Max(0.01f, CircleFullHoldDuration)
                );
                float composedPulse =
                    Mathf.Sin(holdProgress * Mathf.PI) * 0.012f;
                diameter = Mathf.Min(
                    maximumDiameter,
                    diameter * (1f + composedPulse)
                );
            }

            if (diameter <= 0.5f)
                return;

            Rect rect = new Rect(
                center.x - diameter * 0.5f,
                center.y - diameter * 0.5f,
                diameter,
                diameter
            );

            Color previous = GUI.color;
            float appear =
                globalAlpha *
                Mathf.Lerp(0.22f, 1f, progress);

            // Graphite outer plate: same dark metallic family as the BBH depth.
            GUI.color = new Color(
                0.055f,
                0.070f,
                0.100f,
                appear * 0.99f
            );
            GUI.DrawTexture(
                rect,
                filledCircleTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );

            // Steel inner plate provides the layered, polished badge treatment.
            Rect inner = new Rect(
                rect.x + rect.width * 0.085f,
                rect.y + rect.height * 0.085f,
                rect.width * 0.83f,
                rect.height * 0.83f
            );
            GUI.color = new Color(
                0.19f,
                0.235f,
                0.315f,
                appear * 0.94f
            );
            GUI.DrawTexture(
                inner,
                filledCircleTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );

            // Restrained cool highlight, matching the letter bevels rather than
            // introducing a separate bright-blue visual language.
            Rect highlight = new Rect(
                inner.x + inner.width * 0.10f,
                inner.y + inner.height * 0.08f,
                inner.width * 0.44f,
                inner.height * 0.34f
            );
            GUI.color = new Color(
                0.76f,
                0.84f,
                0.96f,
                appear * 0.16f
            );
            GUI.DrawTexture(
                highlight,
                filledCircleTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );

            GUI.color = new Color(
                0.72f,
                0.82f,
                0.96f,
                appear * 0.90f
            );
            GUI.DrawTexture(
                rect,
                circleRimTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );

            GUI.color = previous;
        }

        private static Texture2D CreateCircleTexture(
            int size,
            bool rimOnly)
        {
            int safeSize = Mathf.Max(64, size);
            Texture2D texture = new Texture2D(
                safeSize,
                safeSize,
                TextureFormat.RGBA32,
                mipChain: false
            );

            texture.name = rimOnly
                ? "BBH Circle Rim"
                : "BBH Filled Circle";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            Color[] pixels = new Color[safeSize * safeSize];
            float center = (safeSize - 1) * 0.5f;
            float radius = safeSize * 0.49f;

            for (int y = 0; y < safeSize; y++)
            {
                for (int x = 0; x < safeSize; x++)
                {
                    float dx = x - center;
                    float dy = y - center;
                    float distance = Mathf.Sqrt(dx * dx + dy * dy);
                    float normalizedDistance =
                        distance / Mathf.Max(0.001f, radius);

                    // BD BBH CIRCLE ALPHA SDF FIX V14
                    // Mathf.SmoothStep(from, to, t) does not normalize a raw
                    // pixel distance. The previous implementation passed the
                    // distance as t, which made the generated circle fully
                    // transparent. Normalize explicitly before smoothing.
                    float outerFeather = SmoothStep01(
                        Mathf.InverseLerp(
                            0.94f,
                            1.02f,
                            normalizedDistance
                        )
                    );
                    float outerAlpha = 1f - outerFeather;

                    float alpha = outerAlpha;
                    if (rimOnly)
                    {
                        float innerRise = SmoothStep01(
                            Mathf.InverseLerp(
                                0.80f,
                                0.89f,
                                normalizedDistance
                            )
                        );
                        alpha = outerAlpha * innerRise;
                    }

                    pixels[y * safeSize + x] =
                        new Color(
                            1f,
                            1f,
                            1f,
                            Mathf.Clamp01(alpha)
                        );
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(
                updateMipmaps: false,
                makeNoLongerReadable: true
            );
            return texture;
        }

        private void DrawCompletionLightSweep(
            Vector2 center,
            float finalFontSize,
            float spacing,
            float globalAlpha,
            float shimmerProgress)
        {
            float width = spacing * 3.25f;
            float height = finalFontSize * 1.16f;
            Rect clip = new Rect(
                center.x - width * 0.5f,
                center.y - height * 0.5f,
                width,
                height
            );

            GUI.BeginGroup(clip);

            float sweep = Mathf.SmoothStep(
                0f,
                1f,
                shimmerProgress
            );
            float sweepX = Mathf.Lerp(
                -width * 0.24f,
                width * 1.05f,
                sweep
            );

            Color previous = GUI.color;
            GUI.color = new Color(
                0.72f,
                0.86f,
                1f,
                globalAlpha *
                Mathf.Sin(shimmerProgress * Mathf.PI) *
                0.12f
            );

            GUI.DrawTexture(
                new Rect(
                    sweepX,
                    0f,
                    width * 0.10f,
                    height
                ),
                solidTexture
            );

            GUI.color = previous;
            GUI.EndGroup();
        }

        private void DrawCenteredLabel(
            string text,
            Vector2 center,
            float finalFontSize,
            float scale,
            Color color,
            float rotationDegrees)
        {
            Matrix4x4 previousMatrix = GUI.matrix;
            Color previousColor = GUI.color;

            float safeScale = Mathf.Max(0.01f, scale);
            float boxSize = finalFontSize * 1.65f;
            letterStyle.fontSize = Mathf.RoundToInt(finalFontSize);

            GUIUtility.ScaleAroundPivot(
                new Vector2(safeScale, safeScale),
                center
            );

            if (Mathf.Abs(rotationDegrees) > 0.01f)
            {
                GUIUtility.RotateAroundPivot(
                    rotationDegrees,
                    center
                );
            }

            GUI.color = color;
            GUI.Label(
                new Rect(
                    center.x - boxSize * 0.5f,
                    center.y - boxSize * 0.5f,
                    boxSize,
                    boxSize
                ),
                text,
                letterStyle
            );

            GUI.color = previousColor;
            GUI.matrix = previousMatrix;
        }

        private float ResolveGlobalAlpha()
        {
            if (Elapsed <= FadeStartTime)
                return 1f;

            float fadeProgress =
                (Elapsed - FadeStartTime) /
                FadeOutDuration;

            return 1f -
                   SmoothStep01(
                       Mathf.Clamp01(fadeProgress)
                   );
        }

        private void EnsureResources()
        {
            if (solidTexture == null)
            {
                solidTexture = new Texture2D(
                    1,
                    1,
                    TextureFormat.RGBA32,
                    mipChain: false
                );
                solidTexture.name = "BBH Intro Solid";
                solidTexture.SetPixel(0, 0, Color.white);
                solidTexture.Apply(
                    updateMipmaps: false,
                    makeNoLongerReadable: true
                );
            }

            if (filledCircleTexture == null)
            {
                filledCircleTexture =
                    CreateCircleTexture(256, rimOnly: false);
            }

            if (circleRimTexture == null)
            {
                circleRimTexture =
                    CreateCircleTexture(256, rimOnly: true);
            }

            if (bootGradientTexture == null)
            {
                bootGradientTexture =
                    CreateBootGradientTexture(128);
            }

            if (bootGlowTexture == null)
            {
                bootGlowTexture =
                    CreateBootRadialGlowTexture(128);
            }

            if (bootScanlineTexture == null)
            {
                bootScanlineTexture =
                    CreateBootScanlineTexture(8);
            }

            if (bootVignetteTexture == null)
            {
                bootVignetteTexture =
                    CreateBootVignetteTexture(128);
            }

            if (letterStyle != null)
                return;

            letterStyle = new GUIStyle(GUI.skin.label)
            {
                font = GUI.skin.label.font,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                clipping = TextClipping.Overflow,
                wordWrap = false
            };
        }

        private static Texture2D CreateBootGradientTexture(
            int height)
        {
            int safeHeight = Mathf.Max(32, height);
            Texture2D texture = new Texture2D(
                1,
                safeHeight,
                TextureFormat.RGBA32,
                mipChain: false
            );
            texture.name = "BBH Professional Boot Gradient";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            Color top = new Color(0.035f, 0.055f, 0.105f, 1f);
            Color bottom = new Color(0.002f, 0.005f, 0.014f, 1f);

            for (int y = 0; y < safeHeight; y++)
            {
                float t = y / (float)(safeHeight - 1);
                texture.SetPixel(0, y, Color.Lerp(bottom, top, t));
            }

            texture.Apply(false, true);
            return texture;
        }

        private static Texture2D CreateBootRadialGlowTexture(
            int size)
        {
            int safeSize = Mathf.Max(32, size);
            Texture2D texture = new Texture2D(
                safeSize,
                safeSize,
                TextureFormat.RGBA32,
                mipChain: false
            );
            texture.name = "BBH Professional Boot Glow";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            Vector2 center = new Vector2(
                (safeSize - 1) * 0.5f,
                (safeSize - 1) * 0.5f
            );
            float radius = Mathf.Max(1f, safeSize * 0.5f);

            for (int y = 0; y < safeSize; y++)
            {
                for (int x = 0; x < safeSize; x++)
                {
                    float distance =
                        Vector2.Distance(new Vector2(x, y), center) /
                        radius;
                    float alpha = Mathf.Pow(
                        Mathf.Clamp01(1f - distance),
                        2.6f
                    );
                    texture.SetPixel(
                        x,
                        y,
                        new Color(1f, 1f, 1f, alpha)
                    );
                }
            }

            texture.Apply(false, true);
            return texture;
        }

        private static Texture2D CreateBootScanlineTexture(
            int size)
        {
            int safeSize = Mathf.Max(4, size);
            Texture2D texture = new Texture2D(
                safeSize,
                safeSize,
                TextureFormat.RGBA32,
                mipChain: false
            );
            texture.name = "BBH Professional Boot Scanlines";
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Point;

            for (int y = 0; y < safeSize; y++)
            {
                float alpha = y == safeSize - 1 ? 0.13f : 0f;
                for (int x = 0; x < safeSize; x++)
                {
                    texture.SetPixel(
                        x,
                        y,
                        new Color(0f, 0f, 0f, alpha)
                    );
                }
            }

            texture.Apply(false, true);
            return texture;
        }

        private static Texture2D CreateBootVignetteTexture(
            int size)
        {
            int safeSize = Mathf.Max(32, size);
            Texture2D texture = new Texture2D(
                safeSize,
                safeSize,
                TextureFormat.RGBA32,
                mipChain: false
            );
            texture.name = "BBH Professional Boot Vignette";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            Vector2 center = new Vector2(
                (safeSize - 1) * 0.5f,
                (safeSize - 1) * 0.5f
            );
            float radius = Mathf.Max(1f, safeSize * 0.71f);

            for (int y = 0; y < safeSize; y++)
            {
                for (int x = 0; x < safeSize; x++)
                {
                    float distance =
                        Vector2.Distance(new Vector2(x, y), center) /
                        radius;
                    float alpha = SmoothStep01(
                        Mathf.InverseLerp(0.46f, 1f, distance)
                    ) * 0.72f;
                    texture.SetPixel(
                        x,
                        y,
                        new Color(0f, 0f, 0f, alpha)
                    );
                }
            }

            texture.Apply(false, true);
            return texture;
        }

        private static void DestroyTexture(ref Texture2D texture)
        {
            if (texture == null)
                return;

            Destroy(texture);
            texture = null;
        }

        private static float EaseOutBack(float value)
        {
            return EaseOutBackTuned(value, 1.85f);
        }

        private static float EaseOutBackTuned(
            float value,
            float overshoot)
        {
            float t = Mathf.Clamp01(value);
            float c1 = Mathf.Max(0f, overshoot);
            float c3 = c1 + 1f;
            float shifted = t - 1f;

            return 1f +
                   c3 * shifted * shifted * shifted +
                   c1 * shifted * shifted;
        }

        private static float EaseOutCubic(float value)
        {
            float t = Mathf.Clamp01(value);
            float inverse = 1f - t;
            return 1f - inverse * inverse * inverse;
        }

        private static float SmoothStep01(float value)
        {
            float t = Mathf.Clamp01(value);
            return t * t * (3f - 2f * t);
        }
    }
}
