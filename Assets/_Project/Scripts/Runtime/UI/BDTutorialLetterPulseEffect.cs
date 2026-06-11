using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDTutorialLetterPulseEffect : BaseMeshEffect
    {
        private Color primary = Color.white;
        private Color accent = new Color(0.22f, 0.82f, 1f, 1f);
        private Color secondary = new Color(1f, 0.30f, 0.78f, 1f);
        private float verticalAmplitude = 2.4f;
        private float horizontalAmplitude = 0.7f;
        private float nextRefreshAt;
        private float textChangedAt;
        private string previousText = string.Empty;

        public void SetPalette(Color newPrimary, Color newAccent)
        {
            SetPalette(
                newPrimary,
                newAccent,
                Color.Lerp(newAccent, Color.magenta, 0.30f)
            );
        }

        public void SetPalette(
            Color newPrimary,
            Color newAccent,
            Color newSecondary)
        {
            primary = newPrimary;
            accent = newAccent;
            secondary = newSecondary;
            if (graphic != null)
                graphic.SetVerticesDirty();
        }

        public void SetMotion(float vertical, float horizontal)
        {
            verticalAmplitude = Mathf.Max(0f, vertical);
            horizontalAmplitude = Mathf.Max(0f, horizontal);
            if (graphic != null)
                graphic.SetVerticesDirty();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            textChangedAt = Time.unscaledTime;
            previousText = ResolveCurrentText();
            if (graphic != null)
                graphic.SetVerticesDirty();
        }

        private void Update()
        {
            if (graphic == null)
                return;

            string current = ResolveCurrentText();
            if (current != previousText)
            {
                previousText = current;
                textChangedAt = Time.unscaledTime;
                graphic.SetVerticesDirty();
            }

            if (Time.unscaledTime < nextRefreshAt)
                return;

            nextRefreshAt = Time.unscaledTime + 1f / 24f;
            graphic.SetVerticesDirty();
        }

        private string ResolveCurrentText()
        {
            Text text = graphic as Text;
            return text != null ? text.text : string.Empty;
        }

        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            if (!IsActive() || vertexHelper == null)
                return;

            int count = vertexHelper.currentVertCount;
            float steppedTime =
                Mathf.Floor(Time.unscaledTime * 18f) / 18f;
            float sinceChange = Time.unscaledTime - textChangedAt;
            UIVertex vertex = new UIVertex();

            for (int index = 0; index < count; index++)
            {
                vertexHelper.PopulateUIVertex(ref vertex, index);
                int characterIndex = index / 4;
                float delayedAge =
                    sinceChange - characterIndex * 0.012f;
                float entrance = Mathf.SmoothStep(
                    0f,
                    1f,
                    Mathf.Clamp01(delayedAge / 0.24f)
                );
                float wave = Mathf.Sin(
                    steppedTime * 5.8f + characterIndex * 0.62f
                );
                float sideWave = Mathf.Sin(
                    steppedTime * 3.6f + characterIndex * 0.41f
                );

                vertex.position.y += Mathf.Round(
                    wave * verticalAmplitude -
                    (1f - entrance) * 9f
                );
                vertex.position.x += Mathf.Round(
                    sideWave * horizontalAmplitude
                );

                float colorCycle =
                    0.5f + 0.5f * Mathf.Sin(
                        steppedTime * 2.8f + characterIndex * 0.48f
                    );
                Color color = colorCycle < 0.5f
                    ? Color.Lerp(primary, accent, colorCycle * 2f)
                    : Color.Lerp(
                        accent,
                        secondary,
                        (colorCycle - 0.5f) * 2f
                    );
                color = Color.Lerp(primary, color, 0.52f);
                color.a *= (vertex.color.a / 255f) * entrance;
                vertex.color = color;
                vertexHelper.SetUIVertex(vertex, index);
            }
        }
    }
}
