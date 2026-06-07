using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDDoorwayPortalVisual : MonoBehaviour
    {
        private static Material sharedPortalMaterial;
        private static Material sharedFrameMaterial;

        private readonly List<Renderer> surfaceRenderers =
            new List<Renderer>();
        private readonly List<Transform> lightBands =
            new List<Transform>();

        private MaterialPropertyBlock propertyBlock;
        private Color colorA;
        private Color colorB;
        private float portalHeight;
        private bool exitPortal;

        private static readonly int BaseColorId =
            Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId =
            Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId =
            Shader.PropertyToID("_EmissionColor");

        public static BDDoorwayPortalVisual EnsurePortal(
            string objectName,
            Vector3 groundPosition,
            Vector3 outwardDirection,
            float width,
            float height,
            bool isExit)
        {
            GameObject existing =
                GameObject.Find(objectName);

            if (existing != null)
            {
                BDDoorwayPortalVisual existingVisual =
                    existing.GetComponent<
                        BDDoorwayPortalVisual>();

                if (existingVisual != null)
                    return existingVisual;

                Destroy(existing);
            }

            outwardDirection.y = 0f;

            if (outwardDirection.sqrMagnitude < 0.001f)
                outwardDirection = Vector3.forward;

            outwardDirection.Normalize();

            GameObject root =
                new GameObject(objectName);

            root.transform.position =
                groundPosition +
                Vector3.up *
                Mathf.Max(1f, height * 0.5f);

            root.transform.rotation =
                Quaternion.LookRotation(
                    -outwardDirection,
                    Vector3.up
                );

            BDDoorwayPortalVisual visual =
                root.AddComponent<
                    BDDoorwayPortalVisual>();

            visual.Build(
                Mathf.Max(1.5f, width),
                Mathf.Max(2f, height),
                isExit
            );

            return visual;
        }

        private void Build(
            float width,
            float height,
            bool isExit)
        {
            exitPortal = isExit;
            portalHeight = height;
            propertyBlock =
                new MaterialPropertyBlock();

            colorA =
                isExit
                    ? new Color(
                        0.20f,
                        0.56f,
                        1.00f,
                        1f
                    )
                    : new Color(
                        0.18f,
                        0.94f,
                        0.80f,
                        1f
                    );

            colorB =
                isExit
                    ? new Color(
                        0.65f,
                        0.30f,
                        1.00f,
                        1f
                    )
                    : new Color(
                        1.00f,
                        0.70f,
                        0.22f,
                        1f
                    );

            CreateSurface(
                "Portal_Surface_Front",
                width,
                height,
                false
            );

            CreateSurface(
                "Portal_Surface_Back",
                width,
                height,
                true
            );

            float frameThickness =
                Mathf.Max(
                    0.08f,
                    width * 0.035f
                );

            CreateFramePiece(
                "Frame_Left",
                new Vector3(
                    -width * 0.5f,
                    0f,
                    0.035f
                ),
                new Vector3(
                    frameThickness,
                    height + frameThickness * 2f,
                    frameThickness
                )
            );

            CreateFramePiece(
                "Frame_Right",
                new Vector3(
                    width * 0.5f,
                    0f,
                    0.035f
                ),
                new Vector3(
                    frameThickness,
                    height + frameThickness * 2f,
                    frameThickness
                )
            );

            CreateFramePiece(
                "Frame_Top",
                new Vector3(
                    0f,
                    height * 0.5f,
                    0.035f
                ),
                new Vector3(
                    width,
                    frameThickness,
                    frameThickness
                )
            );

            CreateFramePiece(
                "Frame_Bottom",
                new Vector3(
                    0f,
                    -height * 0.5f,
                    0.035f
                ),
                new Vector3(
                    width,
                    frameThickness,
                    frameThickness
                )
            );

            for (int index = 0;
                 index < 4;
                 index++)
            {
                CreateLightBand(
                    index,
                    width,
                    height
                );
            }
        }

        private void CreateSurface(
            string objectName,
            float width,
            float height,
            bool reverse)
        {
            GameObject surface =
                GameObject.CreatePrimitive(
                    PrimitiveType.Quad
                );

            surface.name = objectName;
            surface.transform.SetParent(
                transform,
                false
            );
            surface.transform.localPosition =
                new Vector3(
                    0f,
                    0f,
                    reverse ? -0.01f : 0f
                );
            surface.transform.localRotation =
                reverse
                    ? Quaternion.Euler(
                        0f,
                        180f,
                        0f
                    )
                    : Quaternion.identity;
            surface.transform.localScale =
                new Vector3(
                    width,
                    height,
                    1f
                );

            Collider collider =
                surface.GetComponent<Collider>();

            if (collider != null)
                Destroy(collider);

            Renderer renderer =
                surface.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    GetPortalMaterial();

                surfaceRenderers.Add(renderer);
            }
        }

        private void CreateFramePiece(
            string objectName,
            Vector3 localPosition,
            Vector3 localScale)
        {
            GameObject piece =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube
                );

            piece.name = objectName;
            piece.transform.SetParent(
                transform,
                false
            );
            piece.transform.localPosition =
                localPosition;
            piece.transform.localScale =
                localScale;

            Collider collider =
                piece.GetComponent<Collider>();

            if (collider != null)
                Destroy(collider);

            Renderer renderer =
                piece.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    GetFrameMaterial();
            }
        }

        private void CreateLightBand(
            int index,
            float width,
            float height)
        {
            GameObject band =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cube
                );

            band.name =
                "Portal_Light_Band_" +
                index;

            band.transform.SetParent(
                transform,
                false
            );

            float x =
                Mathf.Lerp(
                    -width * 0.35f,
                    width * 0.35f,
                    index / 3f
                );

            band.transform.localPosition =
                new Vector3(
                    x,
                    -height * 0.5f +
                    index * 0.42f,
                    -0.035f
                );

            band.transform.localScale =
                new Vector3(
                    Mathf.Max(
                        0.03f,
                        width * 0.025f
                    ),
                    Mathf.Max(
                        0.35f,
                        height * 0.20f
                    ),
                    0.025f
                );

            Collider collider =
                band.GetComponent<Collider>();

            if (collider != null)
                Destroy(collider);

            Renderer renderer =
                band.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial =
                    GetFrameMaterial();
            }

            lightBands.Add(
                band.transform
            );
        }

        private void Update()
        {
            float time =
                Time.unscaledTime;

            float blend =
                0.5f +
                Mathf.Sin(
                    time *
                    (exitPortal ? 1.55f : 1.25f)
                ) *
                0.5f;

            Color baseColor =
                Color.Lerp(
                    colorA,
                    colorB,
                    blend
                );

            Color darkBase =
                Color.Lerp(
                    new Color(
                        0.015f,
                        0.025f,
                        0.055f,
                        1f
                    ),
                    baseColor * 0.34f,
                    0.55f
                );

            for (int index = 0;
                 index < surfaceRenderers.Count;
                 index++)
            {
                Renderer renderer =
                    surfaceRenderers[index];

                if (renderer == null)
                    continue;

                renderer.GetPropertyBlock(
                    propertyBlock
                );

                propertyBlock.SetColor(
                    BaseColorId,
                    darkBase
                );
                propertyBlock.SetColor(
                    ColorId,
                    darkBase
                );
                propertyBlock.SetColor(
                    EmissionColorId,
                    baseColor *
                    (
                        1.65f +
                        Mathf.Sin(
                            time * 2.15f +
                            index
                        ) *
                        0.18f
                    )
                );

                renderer.SetPropertyBlock(
                    propertyBlock
                );
            }

            for (int index = 0;
                 index < lightBands.Count;
                 index++)
            {
                Transform band =
                    lightBands[index];

                if (band == null)
                    continue;

                Vector3 local =
                    band.localPosition;

                float normalized =
                    Mathf.Repeat(
                        time *
                        (
                            0.18f +
                            index * 0.025f
                        ) +
                        index * 0.23f,
                        1f
                    );

                local.y =
                    Mathf.Lerp(
                        -portalHeight * 0.55f,
                        portalHeight * 0.55f,
                        normalized
                    );

                band.localPosition = local;
            }
        }

        private static Material GetPortalMaterial()
        {
            if (sharedPortalMaterial != null)
                return sharedPortalMaterial;

            Shader shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            sharedPortalMaterial =
                new Material(shader);

            sharedPortalMaterial.name =
                "BD_Portal_Surface_Runtime";

            sharedPortalMaterial.hideFlags =
                HideFlags.DontSave;

            if (sharedPortalMaterial.HasProperty(
                    "_Smoothness"))
            {
                sharedPortalMaterial.SetFloat(
                    "_Smoothness",
                    0.72f
                );
            }

            if (sharedPortalMaterial.HasProperty(
                    "_Metallic"))
            {
                sharedPortalMaterial.SetFloat(
                    "_Metallic",
                    0.08f
                );
            }

            if (sharedPortalMaterial.HasProperty(
                    "_Cull"))
            {
                sharedPortalMaterial.SetFloat(
                    "_Cull",
                    0f
                );
            }

            if (sharedPortalMaterial.HasProperty(
                    "_EmissionColor"))
            {
                sharedPortalMaterial.EnableKeyword(
                    "_EMISSION"
                );
            }

            return sharedPortalMaterial;
        }

        private static Material GetFrameMaterial()
        {
            if (sharedFrameMaterial != null)
                return sharedFrameMaterial;

            Shader shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            sharedFrameMaterial =
                new Material(shader);

            sharedFrameMaterial.name =
                "BD_Portal_Frame_Runtime";

            sharedFrameMaterial.hideFlags =
                HideFlags.DontSave;

            Color frameColor =
                new Color(
                    0.95f,
                    0.66f,
                    0.20f,
                    1f
                );

            if (sharedFrameMaterial.HasProperty(
                    "_BaseColor"))
            {
                sharedFrameMaterial.SetColor(
                    "_BaseColor",
                    frameColor
                );
            }

            if (sharedFrameMaterial.HasProperty(
                    "_Color"))
            {
                sharedFrameMaterial.SetColor(
                    "_Color",
                    frameColor
                );
            }

            if (sharedFrameMaterial.HasProperty(
                    "_EmissionColor"))
            {
                sharedFrameMaterial.EnableKeyword(
                    "_EMISSION"
                );
                sharedFrameMaterial.SetColor(
                    "_EmissionColor",
                    frameColor * 1.8f
                );
            }

            return sharedFrameMaterial;
        }
    }
}
