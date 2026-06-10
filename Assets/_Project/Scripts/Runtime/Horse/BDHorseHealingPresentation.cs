using UnityEngine;

namespace BoredomAndDungeons
{
    /// <summary>
    /// Presents horse healing as a grounded in-world effect. It never owns
    /// health values, input, healing duration, movement, or interaction state.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BDHorseHealth))]
    public sealed class BDHorseHealingPresentation : MonoBehaviour
    {
        [Header("Visual Profile")]
        [SerializeField] private Color healingColor =
            new Color(0.24f, 1f, 0.46f, 1f);

        [SerializeField, Min(0.25f)]
        private float ringRadius = 0.92f;

        [SerializeField, Min(8)]
        private int ringSegments = 48;

        [SerializeField, Min(0.05f)]
        private float fadeInSeconds = 0.16f;

        [SerializeField, Min(0.05f)]
        private float fadeOutSeconds = 0.42f;

        [SerializeField, Min(0.1f)]
        private float pulseFrequency = 2.8f;

        private Transform effectRoot;
        private LineRenderer healingRing;
        private ParticleSystem healingParticles;
        private Material ownedMaterial;
        private float visibility;
        private float healPulse;
        private bool healing;

        public bool IsHealing => healing;

        private void Awake()
        {
            BuildEffect();
            SetImmediateHidden();
        }

        private void OnDisable()
        {
            healing = false;
            SetImmediateHidden();
        }

        private void OnDestroy()
        {
            if (ownedMaterial != null)
            {
                Destroy(ownedMaterial);
                ownedMaterial = null;
            }
        }

        private void Update()
        {
            float duration = healing
                ? Mathf.Max(0.05f, fadeInSeconds)
                : Mathf.Max(0.05f, fadeOutSeconds);

            visibility = Mathf.MoveTowards(
                visibility,
                healing ? 1f : 0f,
                Time.unscaledDeltaTime / duration
            );

            healPulse = Mathf.MoveTowards(
                healPulse,
                0f,
                Time.unscaledDeltaTime * 2.8f
            );

            UpdateEffectVisuals();
        }

        public void BeginHealing()
        {
            BuildEffect();
            healing = true;

            if (healingParticles != null &&
                !healingParticles.isPlaying)
            {
                healingParticles.Play(withChildren: true);
            }
        }

        public void NotifyHealApplied(
            float healedAmount,
            float healthRatio)
        {
            if (!healing || healedAmount <= 0f)
                return;

            float completion = Mathf.Clamp01(healthRatio);
            healPulse = Mathf.Max(
                healPulse,
                Mathf.Lerp(0.72f, 1f, completion)
            );

            if (healingParticles != null)
            {
                int burst = Mathf.Clamp(
                    Mathf.CeilToInt(healedAmount * 0.45f),
                    1,
                    4
                );
                healingParticles.Emit(burst);
            }
        }

        public void EndHealing(bool completed)
        {
            healing = false;

            if (completed)
                healPulse = Mathf.Max(healPulse, 1f);

            if (healingParticles != null)
            {
                healingParticles.Stop(
                    withChildren: true,
                    ParticleSystemStopBehavior.StopEmitting
                );
            }
        }

        private void BuildEffect()
        {
            if (effectRoot != null)
                return;

            GameObject rootObject = new GameObject(
                "BD Horse Healing Presentation"
            );
            effectRoot = rootObject.transform;
            effectRoot.SetParent(transform, worldPositionStays: false);
            effectRoot.localPosition = Vector3.zero;
            effectRoot.localRotation = Quaternion.identity;
            effectRoot.localScale = Vector3.one;

            ownedMaterial = CreateEffectMaterial();
            BuildRing();
            BuildParticles();
        }

        private void BuildRing()
        {
            GameObject ringObject = new GameObject(
                "BD Horse Healing Ground Ring"
            );
            ringObject.transform.SetParent(
                effectRoot,
                worldPositionStays: false
            );
            ringObject.transform.localPosition =
                new Vector3(0f, 0.08f, 0f);
            ringObject.transform.localRotation = Quaternion.identity;

            healingRing = ringObject.AddComponent<LineRenderer>();
            healingRing.useWorldSpace = false;
            healingRing.loop = true;
            healingRing.positionCount = Mathf.Max(8, ringSegments);
            healingRing.widthMultiplier = 0.055f;
            healingRing.numCornerVertices = 3;
            healingRing.numCapVertices = 3;
            healingRing.shadowCastingMode =
                UnityEngine.Rendering.ShadowCastingMode.Off;
            healingRing.receiveShadows = false;

            if (ownedMaterial != null)
                healingRing.sharedMaterial = ownedMaterial;

            int count = healingRing.positionCount;
            for (int index = 0; index < count; index++)
            {
                float angle =
                    index / (float)count * Mathf.PI * 2f;
                healingRing.SetPosition(
                    index,
                    new Vector3(
                        Mathf.Cos(angle) * ringRadius,
                        0f,
                        Mathf.Sin(angle) * ringRadius
                    )
                );
            }
        }

        private void BuildParticles()
        {
            GameObject particleObject = new GameObject(
                "BD Horse Healing Particles"
            );
            particleObject.transform.SetParent(
                effectRoot,
                worldPositionStays: false
            );
            particleObject.transform.localPosition =
                new Vector3(0f, 0.20f, 0f);
            particleObject.transform.localRotation = Quaternion.identity;

            healingParticles =
                particleObject.AddComponent<ParticleSystem>();

            ParticleSystem.MainModule main = healingParticles.main;
            main.loop = true;
            main.playOnAwake = false;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.65f, 1.05f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.18f, 0.38f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.055f, 0.11f);
            main.startColor = new ParticleSystem.MinMaxGradient(
                new Color(
                    healingColor.r,
                    healingColor.g,
                    healingColor.b,
                    0.82f
                )
            );
            main.maxParticles = 72;

            ParticleSystem.EmissionModule emission =
                healingParticles.emission;
            emission.rateOverTime = 16f;

            ParticleSystem.ShapeModule shape = healingParticles.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = Mathf.Max(0.25f, ringRadius * 0.82f);
            shape.radiusThickness = 0.35f;
            shape.rotation = new Vector3(90f, 0f, 0f);

            ParticleSystem.VelocityOverLifetimeModule velocity =
                healingParticles.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.Local;
            velocity.y = new ParticleSystem.MinMaxCurve(0.42f, 0.82f);

            ParticleSystem.ColorOverLifetimeModule color =
                healingParticles.colorOverLifetime;
            color.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(healingColor, 0f),
                    new GradientColorKey(
                        Color.Lerp(healingColor, Color.white, 0.35f),
                        0.55f
                    ),
                    new GradientColorKey(healingColor, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(0.90f, 0.16f),
                    new GradientAlphaKey(0.65f, 0.70f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            color.color = new ParticleSystem.MinMaxGradient(gradient);

            ParticleSystemRenderer particleRenderer =
                particleObject.GetComponent<ParticleSystemRenderer>();
            particleRenderer.renderMode =
                ParticleSystemRenderMode.Billboard;
            particleRenderer.shadowCastingMode =
                UnityEngine.Rendering.ShadowCastingMode.Off;
            particleRenderer.receiveShadows = false;

            if (ownedMaterial != null)
                particleRenderer.sharedMaterial = ownedMaterial;
        }

        private void UpdateEffectVisuals()
        {
            if (effectRoot == null)
                return;

            bool visible = visibility > 0.001f;
            if (effectRoot.gameObject.activeSelf != visible)
                effectRoot.gameObject.SetActive(visible);

            if (!visible)
                return;

            float wave =
                0.5f +
                0.5f * Mathf.Sin(
                    Time.unscaledTime *
                    Mathf.Max(0.1f, pulseFrequency) *
                    Mathf.PI * 2f
                );

            float pulse = Mathf.Max(wave * 0.32f, healPulse);
            float alpha = visibility * Mathf.Lerp(0.45f, 0.95f, pulse);
            Color ringColor = new Color(
                healingColor.r,
                healingColor.g,
                healingColor.b,
                alpha
            );

            if (healingRing != null)
            {
                healingRing.startColor = ringColor;
                healingRing.endColor = ringColor;
                healingRing.widthMultiplier =
                    Mathf.Lerp(0.045f, 0.075f, pulse);
                healingRing.transform.localScale =
                    Vector3.one * Mathf.Lerp(0.94f, 1.08f, pulse);
            }
        }

        private void SetImmediateHidden()
        {
            visibility = 0f;
            healPulse = 0f;

            if (healingParticles != null)
            {
                healingParticles.Stop(
                    withChildren: true,
                    ParticleSystemStopBehavior.StopEmittingAndClear
                );
            }

            if (effectRoot != null)
                effectRoot.gameObject.SetActive(false);
        }

        private Material CreateEffectMaterial()
        {
            Shader shader = Shader.Find(
                "Universal Render Pipeline/Particles/Unlit"
            );
            if (shader == null)
                shader = Shader.Find("Particles/Standard Unlit");
            if (shader == null)
                shader = Shader.Find("Unlit/Color");
            if (shader == null)
                shader = Shader.Find("Sprites/Default");
            if (shader == null)
                return null;

            Material material = new Material(shader)
            {
                name = "BD Horse Healing Presentation Material",
                color = healingColor,
                renderQueue = 3100
            };

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", healingColor);
            if (material.HasProperty("_Color"))
                material.SetColor("_Color", healingColor);
            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor(
                    "_EmissionColor",
                    healingColor * 1.35f
                );
            }

            return material;
        }
    }
}
