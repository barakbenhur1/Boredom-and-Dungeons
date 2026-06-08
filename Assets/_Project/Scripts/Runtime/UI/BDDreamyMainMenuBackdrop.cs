using UnityEngine;

namespace BoredomAndDungeons
{
    [DefaultExecutionOrder(-850)]
    [DisallowMultipleComponent]
    public sealed class BDDreamyMainMenuBackdrop :
        MonoBehaviour
    {
        private Texture2D solidTexture;
        private Texture2D skyTexture;
        private Texture2D glowTexture;

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void EnsureCreated()
        {
            if (FindFirstObjectByType<
                    BDDreamyMainMenuBackdrop>() != null)
            {
                return;
            }

            GameObject root =
                new GameObject(
                    "B&D Dreamy Main Menu Backdrop"
                );

            root.AddComponent<
                BDDreamyMainMenuBackdrop>();
        }

        private void OnDestroy()
        {
            if (solidTexture != null)
                Destroy(solidTexture);

            if (skyTexture != null)
                Destroy(skyTexture);

            if (glowTexture != null)
                Destroy(glowTexture);
        }

        private void OnGUI()
        {
            BDMainMenuFlow flow =
                BDMainMenuFlow.Instance;

            if (flow == null ||
                flow.IsRunActive ||
                BDBBHBootIntro.IsPlaying)
            {
                return;
            }

            EnsureTextures();

            int previousDepth = GUI.depth;
            Color previousColor = GUI.color;
            Matrix4x4 previousMatrix = GUI.matrix;

            GUI.depth = 1000;
            GUI.matrix = Matrix4x4.identity;

            DrawSky();
            DrawMoon();
            DrawStars();
            DrawCloudHaze();
            DrawDeviceFocusHalo();
            DrawStorybookHorizon();
            DrawGoldenPath();

            GUI.color = previousColor;
            GUI.matrix = previousMatrix;
            GUI.depth = previousDepth;
        }

        private void DrawSky()
        {
            GUI.color = Color.white;

            GUI.DrawTexture(
                new Rect(
                    0f,
                    0f,
                    Screen.width,
                    Screen.height
                ),
                skyTexture
            );
        }

        private void DrawMoon()
        {
            float size =
                Mathf.Min(
                    Screen.width,
                    Screen.height
                ) * 0.28f;

            Rect glow = new Rect(
                Screen.width * 0.09f,
                Screen.height * 0.045f,
                size,
                size
            );

            GUI.color = new Color(
                1f,
                0.84f,
                0.48f,
                0.22f
            );

            GUI.DrawTexture(
                glow,
                glowTexture
            );

            float moonSize =
                size * 0.29f;

            GUI.color = new Color(
                1f,
                0.94f,
                0.72f,
                0.86f
            );

            GUI.DrawTexture(
                new Rect(
                    glow.center.x -
                    moonSize * 0.5f,
                    glow.center.y -
                    moonSize * 0.5f,
                    moonSize,
                    moonSize
                ),
                glowTexture
            );
        }

        private void DrawStars()
        {
            float time = Time.unscaledTime;

            for (int index = 0;
                 index < 54;
                 index++)
            {
                float seed =
                    index * 19.173f + 2.9f;

                float x =
                    Mathf.Repeat(
                        Mathf.Sin(seed) *
                        43758.5453f,
                        1f
                    );

                float y =
                    Mathf.Repeat(
                        Mathf.Sin(
                            seed * 1.719f
                        ) *
                        24634.6345f,
                        1f
                    );

                float twinkle =
                    0.5f +
                    0.5f *
                    Mathf.Sin(
                        time *
                        (0.65f +
                         index % 5 * 0.16f) +
                        seed
                    );

                float size =
                    1.4f +
                    index % 4 * 0.7f;

                GUI.color = new Color(
                    0.76f,
                    0.87f,
                    1f,
                    0.22f +
                    twinkle * 0.46f
                );

                GUI.DrawTexture(
                    new Rect(
                        Screen.width *
                        (0.035f + x * 0.93f),
                        Screen.height *
                        (0.025f + y * 0.60f),
                        size,
                        size
                    ),
                    solidTexture
                );
            }
        }

        private void DrawCloudHaze()
        {
            float time = Time.unscaledTime;

            for (int cloud = 0;
                 cloud < 5;
                 cloud++)
            {
                float width =
                    Screen.width *
                    (0.18f + cloud * 0.018f);

                float drift =
                    Mathf.Repeat(
                        time *
                        (5f + cloud * 0.8f) +
                        cloud * Screen.width *
                        0.24f,
                        Screen.width + width
                    ) - width;

                GUI.color = new Color(
                    0.48f,
                    0.43f,
                    0.73f,
                    0.045f +
                    cloud * 0.008f
                );

                GUI.DrawTexture(
                    new Rect(
                        drift,
                        Screen.height *
                        (0.18f +
                         cloud * 0.082f),
                        width,
                        Screen.height * 0.13f
                    ),
                    glowTexture
                );
            }
        }

        // BD PROFESSIONAL DEVICE FOCUS HALO V23R19Q
        private void DrawDeviceFocusHalo()
        {
            Color previousColor = GUI.color;
            float width = Screen.width * 0.62f;
            float height = Screen.height * 0.86f;

            GUI.color = new Color(
                0.18f,
                0.46f,
                0.58f,
                0.085f
            );

            GUI.DrawTexture(
                new Rect(
                    Screen.width * 0.50f - width * 0.50f,
                    Screen.height * 0.50f - height * 0.48f,
                    width,
                    height
                ),
                glowTexture,
                ScaleMode.StretchToFill,
                alphaBlend: true
            );

            GUI.color = previousColor;
        }

        private void DrawStorybookHorizon()
        {
            GUI.color = new Color(
                0.035f,
                0.055f,
                0.12f,
                0.96f
            );

            GUI.DrawTexture(
                new Rect(
                    0f,
                    Screen.height * 0.73f,
                    Screen.width,
                    Screen.height * 0.27f
                ),
                solidTexture
            );

            GUI.color = new Color(
                0.058f,
                0.086f,
                0.17f,
                0.98f
            );

            GUI.DrawTexture(
                new Rect(
                    0f,
                    Screen.height * 0.81f,
                    Screen.width,
                    Screen.height * 0.19f
                ),
                solidTexture
            );

            GUI.color = new Color(
                0.12f,
                0.16f,
                0.28f,
                0.34f
            );

            GUI.DrawTexture(
                new Rect(
                    Screen.width * 0.42f,
                    Screen.height * 0.69f,
                    Screen.width * 0.16f,
                    Screen.height * 0.20f
                ),
                glowTexture
            );
        }

        private void DrawGoldenPath()
        {
            float time = Time.unscaledTime;

            for (int segment = 0;
                 segment < 18;
                 segment++)
            {
                float t =
                    segment / 17f;

                float y =
                    Mathf.Lerp(
                        Screen.height * 0.98f,
                        Screen.height * 0.68f,
                        t
                    );

                float x =
                    Screen.width * 0.50f +
                    Mathf.Sin(
                        t * 8.4f +
                        time * 0.11f
                    ) *
                    Screen.width *
                    (0.052f * (1f - t));

                float size =
                    Mathf.Lerp(
                        9f,
                        2f,
                        t
                    );

                GUI.color = new Color(
                    0.95f,
                    0.73f,
                    0.30f,
                    Mathf.Lerp(
                        0.22f,
                        0.68f,
                        t
                    )
                );

                GUI.DrawTexture(
                    new Rect(
                        x - size * 0.5f,
                        y - size * 0.5f,
                        size,
                        size
                    ),
                    glowTexture
                );
            }
        }

        private void EnsureTextures()
        {
            if (solidTexture != null)
                return;

            solidTexture =
                CreateSolidTexture(Color.white);

            skyTexture =
                CreateVerticalGradientTexture(
                    new Color(
                        0.060f,
                        0.075f,
                        0.20f,
                        1f
                    ),
                    new Color(
                        0.010f,
                        0.016f,
                        0.052f,
                        1f
                    )
                );

            glowTexture =
                CreateRadialGlowTexture(64);
        }

        private static Texture2D CreateSolidTexture(
            Color color)
        {
            Texture2D texture =
                new Texture2D(
                    1,
                    1,
                    TextureFormat.RGBA32,
                    mipChain: false
                );

            texture.SetPixel(
                0,
                0,
                color
            );

            texture.Apply(
                updateMipmaps: false,
                makeNoLongerReadable: true
            );

            return texture;
        }

        private static Texture2D
            CreateVerticalGradientTexture(
                Color top,
                Color bottom)
        {
            const int height = 128;

            Texture2D texture =
                new Texture2D(
                    1,
                    height,
                    TextureFormat.RGBA32,
                    mipChain: false
                );

            for (int y = 0;
                 y < height;
                 y++)
            {
                float t =
                    y / (float)(height - 1);

                texture.SetPixel(
                    0,
                    y,
                    Color.Lerp(
                        bottom,
                        top,
                        t
                    )
                );
            }

            texture.wrapMode =
                TextureWrapMode.Clamp;

            texture.Apply(
                updateMipmaps: false,
                makeNoLongerReadable: true
            );

            return texture;
        }

        private static Texture2D
            CreateRadialGlowTexture(
                int size)
        {
            Texture2D texture =
                new Texture2D(
                    size,
                    size,
                    TextureFormat.RGBA32,
                    mipChain: false
                );

            Vector2 center =
                new Vector2(
                    (size - 1) * 0.5f,
                    (size - 1) * 0.5f
                );

            float radius =
                Mathf.Max(
                    1f,
                    size * 0.5f
                );

            for (int y = 0;
                 y < size;
                 y++)
            {
                for (int x = 0;
                     x < size;
                     x++)
                {
                    float distance =
                        Vector2.Distance(
                            new Vector2(x, y),
                            center
                        ) / radius;

                    float alpha =
                        Mathf.Pow(
                            Mathf.Clamp01(
                                1f - distance
                            ),
                            2.2f
                        );

                    texture.SetPixel(
                        x,
                        y,
                        new Color(
                            1f,
                            1f,
                            1f,
                            alpha
                        )
                    );
                }
            }

            texture.wrapMode =
                TextureWrapMode.Clamp;

            texture.Apply(
                updateMipmaps: false,
                makeNoLongerReadable: true
            );

            return texture;
        }
    }
}
