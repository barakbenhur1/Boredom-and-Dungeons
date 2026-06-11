using UnityEngine;
using UnityEngine.UI;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private const string ChildApproachDialogueLine =
            "honey come here a second";

        private GameObject childApproachDialogueRoot;
        private CanvasGroup childApproachDialogueCanvasGroup;
        private RectTransform childApproachDialogueVisualRect;
        private RectTransform childApproachDialogueRect;
        private RectTransform childApproachDialogueTailRect;
        private Vector2 childApproachDialogueRestPosition;
        private AudioSource childApproachDialogueAudioSource;
        private AudioClip childApproachDialogueVoiceClip;
        private bool childApproachDialogueVoicePlayed;

        private void InitializeChildApproachDialogueState()
        {
            childApproachDialogueRoot = null;
            childApproachDialogueCanvasGroup = null;
            childApproachDialogueVisualRect = null;
            childApproachDialogueRect = null;
            childApproachDialogueTailRect = null;
            childApproachDialogueRestPosition = Vector2.zero;
            childApproachDialogueAudioSource = null;
            childApproachDialogueVoiceClip = null;
            childApproachDialogueVoicePlayed = false;
        }

        private void EnsureChildApproachDialogueOverlay()
        {
            if (childApproachDialogueRoot != null &&
                childApproachDialogueCanvasGroup != null &&
                childApproachDialogueRect != null)
            {
                return;
            }

            childApproachDialogueRoot = new GameObject(
                "B&D Child Approach Dialogue",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(CanvasGroup),
                typeof(AudioSource)
            );
            childApproachDialogueRoot.transform.SetParent(transform, false);

            Canvas canvas = childApproachDialogueRoot.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 32755;

            CanvasScaler scaler =
                childApproachDialogueRoot.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode =
                CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            childApproachDialogueCanvasGroup =
                childApproachDialogueRoot.GetComponent<CanvasGroup>();
            childApproachDialogueCanvasGroup.interactable = false;
            childApproachDialogueCanvasGroup.blocksRaycasts = false;

            childApproachDialogueAudioSource =
                childApproachDialogueRoot.GetComponent<AudioSource>();
            childApproachDialogueAudioSource.playOnAwake = false;
            childApproachDialogueAudioSource.loop = false;
            childApproachDialogueAudioSource.spatialBlend = 0f;
            childApproachDialogueAudioSource.volume = 0.48f;
            childApproachDialogueAudioSource.ignoreListenerPause = true;

            GameObject visualObject = new GameObject(
                "Mother Speech Bubble Visual",
                typeof(RectTransform)
            );
            visualObject.transform.SetParent(
                childApproachDialogueRoot.transform,
                false
            );
            childApproachDialogueVisualRect =
                visualObject.GetComponent<RectTransform>();
            childApproachDialogueVisualRect.anchorMin = new Vector2(0f, 1f);
            childApproachDialogueVisualRect.anchorMax = new Vector2(0f, 1f);
            childApproachDialogueVisualRect.pivot = new Vector2(0f, 1f);
            childApproachDialogueRestPosition = new Vector2(72f, -84f);
            childApproachDialogueVisualRect.anchoredPosition =
                childApproachDialogueRestPosition;
            childApproachDialogueVisualRect.sizeDelta = new Vector2(650f, 164f);

            GameObject panelObject = new GameObject(
                "Mother Speech Bubble",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image),
                typeof(Outline)
            );
            panelObject.transform.SetParent(visualObject.transform, false);

            childApproachDialogueRect =
                panelObject.GetComponent<RectTransform>();
            childApproachDialogueRect.anchorMin = new Vector2(0f, 1f);
            childApproachDialogueRect.anchorMax = new Vector2(0f, 1f);
            childApproachDialogueRect.pivot = new Vector2(0f, 1f);
            childApproachDialogueRect.anchoredPosition = Vector2.zero;
            childApproachDialogueRect.sizeDelta = new Vector2(650f, 132f);

            Image panel = panelObject.GetComponent<Image>();
            panel.color = new Color(0.965f, 0.945f, 0.875f, 0.98f);
            panel.raycastTarget = false;
            if (roundedSprite != null)
            {
                panel.sprite = roundedSprite;
                panel.type = Image.Type.Sliced;
            }

            Outline outline = panelObject.GetComponent<Outline>();
            outline.effectColor = new Color(0.055f, 0.07f, 0.11f, 0.92f);
            outline.effectDistance = new Vector2(4f, -4f);
            outline.useGraphicAlpha = true;

            GameObject tailShadowObject = new GameObject(
                "Mother Speech Bubble Tail Shadow",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );
            tailShadowObject.transform.SetParent(visualObject.transform, false);
            RectTransform tailShadowRect =
                tailShadowObject.GetComponent<RectTransform>();
            tailShadowRect.anchorMin = new Vector2(0f, 1f);
            tailShadowRect.anchorMax = new Vector2(0f, 1f);
            tailShadowRect.pivot = new Vector2(0.5f, 0.5f);
            tailShadowRect.anchoredPosition = new Vector2(62f, -136f);
            tailShadowRect.sizeDelta = new Vector2(34f, 34f);
            tailShadowRect.localRotation = Quaternion.Euler(0f, 0f, 45f);
            Image tailShadow = tailShadowObject.GetComponent<Image>();
            tailShadow.color = outline.effectColor;
            tailShadow.raycastTarget = false;

            GameObject tailObject = new GameObject(
                "Mother Speech Bubble Tail",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );
            tailObject.transform.SetParent(visualObject.transform, false);
            childApproachDialogueTailRect =
                tailObject.GetComponent<RectTransform>();
            childApproachDialogueTailRect.anchorMin = new Vector2(0f, 1f);
            childApproachDialogueTailRect.anchorMax = new Vector2(0f, 1f);
            childApproachDialogueTailRect.pivot = new Vector2(0.5f, 0.5f);
            childApproachDialogueTailRect.anchoredPosition =
                new Vector2(62f, -134f);
            childApproachDialogueTailRect.sizeDelta = new Vector2(28f, 28f);
            childApproachDialogueTailRect.localRotation =
                Quaternion.Euler(0f, 0f, 45f);
            Image tail = tailObject.GetComponent<Image>();
            tail.color = panel.color;
            tail.raycastTarget = false;

            panelObject.transform.SetAsLastSibling();

            GameObject seamObject = new GameObject(
                "Mother Speech Bubble Tail Seam Cover",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );
            seamObject.transform.SetParent(visualObject.transform, false);
            RectTransform seamRect = seamObject.GetComponent<RectTransform>();
            seamRect.anchorMin = new Vector2(0f, 1f);
            seamRect.anchorMax = new Vector2(0f, 1f);
            seamRect.pivot = new Vector2(0.5f, 0.5f);
            seamRect.anchoredPosition = new Vector2(62f, -131f);
            seamRect.sizeDelta = new Vector2(22f, 10f);
            Image seam = seamObject.GetComponent<Image>();
            seam.color = panel.color;
            seam.raycastTarget = false;

            GameObject textObject = new GameObject(
                "Mother Speech Bubble Text",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Text)
            );
            textObject.transform.SetParent(panelObject.transform, false);
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(34f, 20f);
            textRect.offsetMax = new Vector2(-28f, -18f);

            Text text = textObject.GetComponent<Text>();
            text.text = ChildApproachDialogueLine;
            text.font = uiFont != null
                ? uiFont
                : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 38;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleLeft;
            text.color = new Color(0.045f, 0.055f, 0.085f, 1f);
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 28;
            text.resizeTextMaxSize = 38;
            text.raycastTarget = false;

            childApproachDialogueRoot.SetActive(false);
        }

        private void ResetChildApproachDialogueForPlayback()
        {
            EnsureChildApproachDialogueOverlay();
            StopChildApproachDialogueVoice();
            childApproachDialogueVoicePlayed = false;
            SetChildApproachDialogueImmediate(0f);
        }

        private void UpdateChildApproachDialogue(float elapsedSeconds)
        {
            EnsureChildApproachDialogueOverlay();

            float visibility;
            if (elapsedSeconds < ChildApproachDialogueEnterStartsAtSeconds)
            {
                visibility = 0f;
            }
            else if (elapsedSeconds < ChildApproachDialogueEnterEndsAtSeconds)
            {
                visibility = SmoothestStep01(Mathf.InverseLerp(
                    ChildApproachDialogueEnterStartsAtSeconds,
                    ChildApproachDialogueEnterEndsAtSeconds,
                    elapsedSeconds
                ));
            }
            else if (elapsedSeconds < ChildApproachDialogueHoldEndsAtSeconds)
            {
                visibility = 1f;
            }
            else if (elapsedSeconds < ChildApproachDialogueExitEndsAtSeconds)
            {
                visibility = 1f - SmoothestStep01(Mathf.InverseLerp(
                    ChildApproachDialogueHoldEndsAtSeconds,
                    ChildApproachDialogueExitEndsAtSeconds,
                    elapsedSeconds
                ));
            }
            else
            {
                visibility = 0f;
            }

            SetChildApproachDialogueImmediate(visibility);
            if (!childApproachDialogueVoicePlayed &&
                visibility > 0.001f &&
                elapsedSeconds < ChildApproachDialogueExitEndsAtSeconds)
            {
                PlayChildApproachDialogueVoice();
            }
            if (elapsedSeconds >= ChildApproachDialogueExitEndsAtSeconds)
                StopChildApproachDialogueVoice();
        }

        private void SetChildApproachDialogueImmediate(float visibility)
        {
            EnsureChildApproachDialogueOverlay();
            float t = Mathf.Clamp01(visibility);
            bool active = t > 0.001f;
            if (childApproachDialogueRoot.activeSelf != active)
                childApproachDialogueRoot.SetActive(active);
            if (!active)
                return;

            childApproachDialogueCanvasGroup.alpha = t;
            float scale = Mathf.Lerp(0.88f, 1f, t);
            Vector2 entranceOffset = Vector2.Lerp(
                new Vector2(-18f, 12f),
                Vector2.zero,
                t
            );
            if (childApproachDialogueVisualRect != null)
            {
                childApproachDialogueVisualRect.localScale =
                    new Vector3(scale, scale, 1f);
                childApproachDialogueVisualRect.anchoredPosition =
                    childApproachDialogueRestPosition + entranceOffset;
            }
        }

        private void PlayChildApproachDialogueVoice()
        {
            childApproachDialogueVoicePlayed = true;
            EnsureChildApproachDialogueOverlay();
            if (childApproachDialogueAudioSource == null)
                return;

            if (childApproachDialogueVoiceClip == null)
            {
                childApproachDialogueVoiceClip =
                    CreateChildApproachFemaleMurmurClip();
            }

            childApproachDialogueAudioSource.Stop();
            childApproachDialogueAudioSource.clip =
                childApproachDialogueVoiceClip;
            childApproachDialogueAudioSource.Play();
        }

        private static AudioClip CreateChildApproachFemaleMurmurClip()
        {
            const int sampleRate = 44100;
            const float duration = 1.95f;
            int sampleCount = Mathf.CeilToInt(sampleRate * duration);
            float[] samples = new float[sampleCount];
            float[] syllableStarts =
            {
                0.00f, 0.27f, 0.53f, 0.82f, 1.10f, 1.39f, 1.65f
            };
            float[] syllablePitches =
            {
                246f, 278f, 258f, 296f, 272f, 252f, 266f
            };

            for (int syllable = 0;
                 syllable < syllableStarts.Length;
                 syllable++)
            {
                float syllableDuration = syllable == 3 ? 0.24f : 0.20f;
                int start = Mathf.RoundToInt(
                    syllableStarts[syllable] * sampleRate
                );
                int end = Mathf.Min(
                    sampleCount,
                    start + Mathf.RoundToInt(syllableDuration * sampleRate)
                );
                float basePitch = syllablePitches[syllable];

                for (int sample = start; sample < end; sample++)
                {
                    float local = (sample - start) / (float)sampleRate;
                    float normalized = local / syllableDuration;
                    float envelope = Mathf.Sin(Mathf.PI * normalized);
                    envelope *= envelope;
                    float vibrato = 1f +
                        Mathf.Sin(local * Mathf.PI * 2f * 5.2f) * 0.018f;
                    float phase = local * Mathf.PI * 2f *
                        basePitch * vibrato;
                    float voiced =
                        Mathf.Sin(phase) * 0.58f +
                        Mathf.Sin(phase * 2.02f + 0.34f) * 0.21f +
                        Mathf.Sin(phase * 3.01f + 0.77f) * 0.08f;
                    float softBreath =
                        Mathf.Sin(local * 1733f + syllable * 1.71f) *
                        Mathf.Sin(local * 947f + 0.43f) * 0.025f;
                    samples[sample] +=
                        (voiced + softBreath) * envelope * 0.34f;
                }
            }

            AudioClip clip = AudioClip.Create(
                "B&D Female Nonverbal Dialogue",
                sampleCount,
                1,
                sampleRate,
                false
            );
            clip.SetData(samples, 0);
            return clip;
        }

        private void StopChildApproachDialogueVoice()
        {
            if (childApproachDialogueAudioSource != null)
                childApproachDialogueAudioSource.Stop();
        }

        private void DisposeChildApproachDialogue()
        {
            StopChildApproachDialogueVoice();
            if (childApproachDialogueVoiceClip != null)
                Destroy(childApproachDialogueVoiceClip);
            childApproachDialogueVoiceClip = null;
            if (childApproachDialogueRoot != null)
                Destroy(childApproachDialogueRoot);
            childApproachDialogueRoot = null;
            childApproachDialogueCanvasGroup = null;
            childApproachDialogueVisualRect = null;
            childApproachDialogueRect = null;
            childApproachDialogueTailRect = null;
            childApproachDialogueAudioSource = null;
            childApproachDialogueVoicePlayed = false;
        }
    }
}
