using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-10000)]
    [DisallowMultipleComponent]
    public sealed class BDBBHBootIntro : MonoBehaviour
    {
        private const int LetterCount = 3;
        private const string IntroText = "BBH";
        private const float VerticalScreenPositionFromTop = 0.45f;

        [Header("Timing")]
        [SerializeField] private float initialBlackHold = 0.24f;
        [SerializeField] private float letterDuration = 0.72f;
        [SerializeField] private float gapAfterLetter = 0.08f;
        [SerializeField] private float completedHold = 0.82f;
        [SerializeField] private float fadeOutDuration = 0.18f;

        [Header("Completed BBH Circle")]
        // BD FILLED CIRCLE BADGE V7
        [SerializeField] private float circleGrowthDuration = 0.52f;
        [SerializeField] private float circleFinalHold = 0.50f;
        [SerializeField] private float circleDiameterMultiplier = 3.55f;

        [Header("Composition")]
[SerializeField, Range(0.50f, 1.20f)]
        private float letterSpacingMultiplier = 0.78f;

[Header("Depth")]
        [SerializeField] private float startingScale = 0.055f;
        [SerializeField] private float overshootStrength = 1.16f;
[SerializeField, Range(4, 16)]
        private int pseudoDepthLayers = 9;

        [SerializeField, Range(0.04f, 0.35f)]
        private float extrusionDistanceMultiplier = 0.16f;

        [SerializeField]
        private Vector2 extrusionDirection = new Vector2(0.72f, 1f);

        [SerializeField, Range(0.04f, 0.35f)]
        private float shadowDistanceMultiplier = 0.18f;

        [SerializeField, Range(2, 8)]
        private int shadowSoftCopies = 4;

        private static bool playedThisApplicationSession;
        private static bool currentlyPlaying;

        private float startedAt;
        private bool active;
        private GUIStyle letterStyle;
        private Texture2D solidTexture;
        private Texture2D filledCircleTexture;
        private Texture2D circleRimTexture;
        public static bool IsPlaying => currentlyPlaying;
        public static bool HasPlayedThisSession =>
            playedThisApplicationSession;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetSessionState()
        {
            playedThisApplicationSession = false;
            currentlyPlaying = false;
        }

        private void Awake()
        {
            if (playedThisApplicationSession)
            {
                enabled = false;
                return;
            }

            active = true;
            currentlyPlaying = true;
            startedAt = Time.realtimeSinceStartup;
            Time.timeScale = 0f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnDisable()
        {
            if (!active)
                return;

            active = false;
            currentlyPlaying = false;
        }

        private void OnDestroy()
        {
            if (solidTexture != null)
            {
                Destroy(solidTexture);
                solidTexture = null;
            }
            // by Unity for the play session. Explicitly destroying the font
            // while GUIStyle still references it can produce
            // "Deleting invalid font reference" during script reload.
            if (filledCircleTexture != null)
            {
                Destroy(filledCircleTexture);
                filledCircleTexture = null;
            }
            if (circleRimTexture != null)
            {
                Destroy(circleRimTexture);
                circleRimTexture = null;
            }
            letterStyle = null;
        }

        private void Update()
        {
            if (!active)
                return;

            if (Elapsed < TotalDuration)
                return;

            CompleteIntro();
        }

        private float Elapsed =>
            Time.realtimeSinceStartup - startedAt;

        private float PerLetterWindow =>
            Mathf.Max(0.05f, letterDuration) +
            Mathf.Max(0f, gapAfterLetter);

        private float LettersEndTime =>
            Mathf.Max(0f, initialBlackHold) +
            PerLetterWindow * LetterCount -
            Mathf.Max(0f, gapAfterLetter);

        private float CircleGrowthEndTime =>
            LettersEndTime + Mathf.Max(0.05f, circleGrowthDuration);

        private float FadeStartTime =>
            CircleGrowthEndTime +
            Mathf.Max(
                0f,
                Mathf.Max(0.05f, circleFinalHold) -
                Mathf.Max(0.05f, fadeOutDuration)
            );

        private float TotalDuration =>
            CircleGrowthEndTime + Mathf.Max(0.05f, circleFinalHold);

        private void CompleteIntro()
        {
            active = false;
            currentlyPlaying = false;
            playedThisApplicationSession = true;
            enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnGUI()
        {
            if (!active)
                return;

            EnsureResources();

            int previousDepth = GUI.depth;
            Matrix4x4 previousMatrix = GUI.matrix;
            Color previousColor = GUI.color;

            GUI.depth = -10000;
            GUI.matrix = Matrix4x4.identity;

            float fadeAlpha = ResolveGlobalAlpha();
            GUI.color = new Color(0f, 0f, 0f, fadeAlpha);

            GUI.DrawTexture(
                new Rect(0f, 0f, Screen.width, Screen.height),
                solidTexture
            );

            DrawLetters(fadeAlpha);

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

        private void DrawLetters(float globalAlpha)
        {
            float screenHeight = Mathf.Max(360f, Screen.height);
            float finalFontSize =
                Mathf.Clamp(screenHeight * 0.145f, 72f, 188f);

            float spacing =
                finalFontSize *
                Mathf.Clamp(letterSpacingMultiplier, 0.50f, 1.20f);

            Vector2 compositionCenter = new Vector2(
                Screen.width * 0.50f,
                Screen.height *
                VerticalScreenPositionFromTop
            );
float shimmerProgress = Mathf.Clamp01(
                (Elapsed - LettersEndTime) /
                Mathf.Max(0.01f, completedHold)
            );

            DrawFilledCircleBadge(
                compositionCenter,
                finalFontSize,
                spacing,
                globalAlpha
            );

            for (int index = 0; index < LetterCount; index++)
            {
                float letterStart =
                    Mathf.Max(0f, initialBlackHold) +
                    index * PerLetterWindow;

                float localProgress =
                    (Elapsed - letterStart) /
                    Mathf.Max(0.05f, letterDuration);

                if (localProgress <= 0f)
                    continue;

                float t = Mathf.Clamp01(localProgress);
                float finalX =
                    compositionCenter.x +
                    (index - 1) * spacing;

                Vector2 center = new Vector2(
                    finalX,
                    compositionCenter.y
                );

                DrawDepthTrail(
                    IntroText[index].ToString(),
                    center,
                    finalFontSize,
                    t,
                    globalAlpha
                );

                DrawFrontLetter(
                    IntroText[index].ToString(),
                    center,
                    finalFontSize,
                    t,
                    globalAlpha,
                    shimmerProgress,
                    index
                );
            }

            if (Elapsed >= LettersEndTime)
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
        private void DrawDepthTrail(
            string letter,
            Vector2 finalCenter,
            float finalFontSize,
            float currentProgress,
            float globalAlpha)
        {
            float eased = EaseOutBack(currentProgress);
            float scale = Mathf.Lerp(
                Mathf.Max(0.01f, startingScale),
                1f,
                eased
            );

            float appear =
                SmoothStep01(
                    Mathf.Clamp01(currentProgress / 0.60f)
                );

            int layers = Mathf.Clamp(pseudoDepthLayers, 4, 16);
            Vector2 direction = extrusionDirection.sqrMagnitude > 0.0001f
                ? extrusionDirection.normalized
                : new Vector2(0.58f, 0.82f);

            float totalExtrusion =
                finalFontSize *
                Mathf.Clamp(extrusionDistanceMultiplier, 0.04f, 0.35f) *
                Mathf.Lerp(0.35f, 1f, appear) *
                scale;

            for (int layer = layers; layer >= 1; layer--)
            {
                float layerT = layer / (float)layers;
                float offsetT = Mathf.Lerp(0.08f, 1f, layerT);
                Vector2 offset =
                    direction * totalExtrusion * offsetT;

                float alpha =
                    globalAlpha *
                    appear *
                    Mathf.Lerp(0.22f, 0.78f, layerT);

                Color sideColor = Color.Lerp(
                    new Color(0.04f, 0.07f, 0.11f, alpha * 0.95f),
                    new Color(0.28f, 0.36f, 0.48f, alpha * 0.88f),
                    Mathf.Pow(layerT, 0.75f)
                );

                DrawCenteredLabel(
                    letter,
                    finalCenter + offset,
                    finalFontSize,
                    Mathf.Lerp(scale * 0.985f, scale * 1.02f, layerT),
                    sideColor,
                    rotationDegrees: (1f - currentProgress) * 4.2f
                );
            }

            float bevelAlpha = globalAlpha * appear * 0.28f;
            DrawCenteredLabel(
                letter,
                finalCenter + direction * (totalExtrusion * 0.16f),
                finalFontSize,
                scale,
                new Color(0.68f, 0.80f, 0.96f, bevelAlpha),
                rotationDegrees: (1f - currentProgress) * 2.8f
            );
        }
        private void DrawGrowingShadow(
            string letter,
            Vector2 center,
            float finalFontSize,
            float progress,
            float globalAlpha)
        {
            float eased = EaseOutBack(progress);
            float scale = Mathf.Lerp(
                Mathf.Max(0.01f, startingScale),
                1f,
                eased
            );

            float appear =
                SmoothStep01(
                    Mathf.Clamp01(progress / 0.55f)
                );

            int copies = Mathf.Clamp(shadowSoftCopies, 2, 8);
            float distance =
                finalFontSize *
                Mathf.Clamp(shadowDistanceMultiplier, 0.04f, 0.35f) *
                Mathf.Lerp(0.35f, 1f, appear) *
                scale;

            Vector2 baseOffset = new Vector2(distance * 0.55f, distance);

            for (int copy = copies; copy >= 1; copy--)
            {
                float softness = copy / (float)copies;
                Vector2 offset = baseOffset * Mathf.Lerp(0.35f, 1f, softness);
                float shadowScale = scale * Mathf.Lerp(1.00f, 1.08f, softness);
                float alpha =
                    globalAlpha *
                    appear *
                    Mathf.Lerp(0.04f, 0.20f, softness);

                DrawCenteredLabel(
                    letter,
                    center + offset,
                    finalFontSize,
                    shadowScale,
                    new Color(0f, 0f, 0f, alpha),
                    rotationDegrees: (1f - progress) * 2.5f
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
            int letterIndex)
        {
            float eased = EaseOutBack(progress);
            float scale = Mathf.Lerp(
                Mathf.Max(0.01f, startingScale),
                1f,
                eased
            );

            float alpha =
                globalAlpha *
                SmoothStep01(
                    Mathf.Clamp01(progress / 0.52f)
                );

            float depth = 1f - progress;
            float rotation = depth * 6.5f;

            DrawGrowingShadow(
                letter,
                center,
                finalFontSize,
                progress,
                globalAlpha
            );

            float pulse =
                progress >= 1f
                    ? 0.5f +
                      0.5f *
                      Mathf.Sin(
                          Time.realtimeSinceStartup * 2.6f +
                          letterIndex * 0.7f
                      )
                    : 0f;

            Color glowColor = new Color(
                0.29f,
                0.48f,
                0.82f,
                alpha * (0.08f + pulse * 0.04f)
            );

            DrawCenteredLabel(
                letter,
                center,
                finalFontSize,
                scale * 1.10f,
                glowColor,
                rotation
            );

            Color rimColor = new Color(
                0.06f,
                0.10f,
                0.16f,
                alpha * 0.74f
            );

            DrawCenteredLabel(
                letter,
                center + new Vector2(1.4f, 1.4f),
                finalFontSize,
                scale,
                rimColor,
                rotation
            );

            float brightness = Mathf.Lerp(0.64f, 1f, progress);
            float metallicLift = Mathf.Clamp01(shimmerProgress * 1.4f) * 0.08f;

            Color frontColor = new Color(
                Mathf.Clamp01(brightness + metallicLift),
                Mathf.Clamp01(brightness + metallicLift),
                Mathf.Clamp01(brightness + metallicLift * 1.3f),
                alpha
            );

            DrawCenteredLabel(
                letter,
                center,
                finalFontSize,
                scale,
                frontColor,
                rotation
            );

            Color bevelHighlight = new Color(
                0.86f,
                0.93f,
                1f,
                alpha * Mathf.Lerp(0.44f, 0.12f, progress)
            );

            DrawCenteredLabel(
                letter,
                center + new Vector2(-1.6f, -1.8f),
                finalFontSize,
                scale * 0.985f,
                bevelHighlight,
                rotation
            );

            Color topFaceHighlight = new Color(
                0.72f,
                0.84f,
                1f,
                alpha * 0.15f
            );

            DrawCenteredLabel(
                letter,
                center + new Vector2(-0.4f, -0.6f),
                finalFontSize,
                scale * 1.015f,
                topFaceHighlight,
                rotation
            );
        }
        private void DrawFilledCircleBadge(
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
                Mathf.Max(0.05f, circleGrowthDuration)
            );
            float eased = EaseOutBack(progress);
            float diameter =
                Mathf.Max(
                    finalFontSize * 2.7f,
                    spacing * Mathf.Max(3f, circleDiameterMultiplier)
                ) * Mathf.Max(0f, eased);

            if (diameter <= 0.5f)
                return;

            Rect rect = new Rect(
                center.x - diameter * 0.5f,
                center.y - diameter * 0.5f,
                diameter,
                diameter
            );

            Color previous = GUI.color;
            GUI.color = new Color(
                0.035f,
                0.075f,
                0.17f,
                globalAlpha * 0.96f
            );
            GUI.DrawTexture(rect, filledCircleTexture);

            GUI.color = new Color(
                0.58f,
                0.76f,
                1f,
                globalAlpha * 0.82f
            );
            GUI.DrawTexture(rect, circleRimTexture);
            GUI.color = previous;
        }

        private static Texture2D CreateCircleTexture(
            int size,
            bool rimOnly)
        {
            int safeSize = Mathf.Max(32, size);
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
            float edge = Mathf.Max(1f, safeSize * 0.018f);
            float rimWidth = Mathf.Max(2f, safeSize * 0.035f);

            for (int y = 0; y < safeSize; y++)
            {
                for (int x = 0; x < safeSize; x++)
                {
                    float dx = x - center;
                    float dy = y - center;
                    float distance = Mathf.Sqrt(dx * dx + dy * dy);
                    float outer = 1f - Mathf.SmoothStep(
                        radius - edge,
                        radius + edge,
                        distance
                    );

                    float alpha = outer;
                    if (rimOnly)
                    {
                        float inner = Mathf.SmoothStep(
                            radius - rimWidth - edge,
                            radius - rimWidth + edge,
                            distance
                        );
                        alpha *= inner;
                    }

                    pixels[y * safeSize + x] =
                        new Color(1f, 1f, 1f, Mathf.Clamp01(alpha));
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

            float sweep =
                Mathf.SmoothStep(0f, 1f, shimmerProgress);

            float sweepX =
                Mathf.Lerp(
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

            letterStyle.fontSize =
                Mathf.RoundToInt(finalFontSize);

            Vector2 pivot = center;

            GUIUtility.ScaleAroundPivot(
                new Vector2(safeScale, safeScale),
                pivot
            );

            if (Mathf.Abs(rotationDegrees) > 0.01f)
            {
                GUIUtility.RotateAroundPivot(
                    rotationDegrees,
                    pivot
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
                Mathf.Max(0.05f, fadeOutDuration);

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

                solidTexture.SetPixel(0, 0, Color.white);
                solidTexture.Apply(
                    updateMipmaps: false,
                    makeNoLongerReadable: true
                );
            }

            if (filledCircleTexture == null)
                filledCircleTexture = CreateCircleTexture(256, rimOnly: false);
            if (circleRimTexture == null)
                circleRimTexture = CreateCircleTexture(256, rimOnly: true);

            if (letterStyle != null)
                return;
            letterStyle = new GUIStyle(GUI.skin.label)
            {
                font = GUI.skin.label.font,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                clipping = TextClipping.Overflow,
                wordWrap = false,
            };
        }
        private float EaseOutBack(float value)
        {
            float t = Mathf.Clamp01(value);
            float overshoot =
                Mathf.Clamp(overshootStrength, 1.02f, 1.40f);

            float c1 = 1.70158f * overshoot;
            float c3 = c1 + 1f;
            float shifted = t - 1f;

            return 1f +
                   c3 * shifted * shifted * shifted +
                   c1 * shifted * shifted;
        }

        private static float SmoothStep01(float value)
        {
            float t = Mathf.Clamp01(value);
            return t * t * (3f - 2f * t);
        }
    }
}
