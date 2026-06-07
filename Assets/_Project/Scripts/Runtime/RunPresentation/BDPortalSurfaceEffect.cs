using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDPortalSurfaceEffect : MonoBehaviour
    {
        // BD AUTHORED OPENING VISIBLE LIGHT COVER V14
        // BD PORTAL MASK NORMALIZED ALPHA FIX V14
        // BD FULL RECTANGULAR DOORWAY OCCLUSION V16R
        // BD EXACT WALL-PLANE FULL-HEIGHT SEAL V18
        // BD MARKER-HIDDEN EXACT DOORWAY SEAL V19
        private const string CoreName =
            "PortalOcclusionCore_FullOpening_NotADoor";
        private const string HaloName =
            "PortalLightHalo_FullOpening_NotADoor";
        private const string LegacySurfaceName =
            "SoftLightSurface_EffectOnly_NotADoor";

        private Material coreMaterial;
        private Material haloMaterial;
        private Transform coreSurface;
        private Transform haloSurface;
        private Texture2D haloMask;
        private Light portalLight;
        private Vector2 openingSize = new Vector2(10.5f, 22f);
        private float phase;

        public void Configure(Transform authoredMarker)
        {
            if (authoredMarker == null)
                return;

            gameObject.SetActive(true);
            enabled = true;

            ResolveExactOpeningGeometry(
                authoredMarker,
                out Vector3 openingCenter,
                out Quaternion openingRotation,
                out Vector2 resolvedSize
            );

            // The authored marker is a horizontal threshold guide. It is not
            // the doorway cover and must never remain visible as a floor slab.
            SetMarkerRenderersVisible(authoredMarker, false);

            Transform parent = authoredMarker.parent;
            transform.SetParent(parent, worldPositionStays: true);
            transform.position = openingCenter;
            transform.rotation = openingRotation;
            transform.localScale = Vector3.one;
            openingSize = resolvedSize;

            EnsureVisuals();
            ApplyVisualState(0f);
        }

        private void Awake()
        {
            EnsureVisuals();
        }

        private void ResolveExactOpeningGeometry(
            Transform authoredMarker,
            out Vector3 center,
            out Quaternion rotation,
            out Vector2 size)
        {
            Renderer markerRenderer =
                authoredMarker.GetComponentInChildren<Renderer>();
            Bounds markerBounds = markerRenderer != null
                ? markerRenderer.bounds
                : new Bounds(
                    authoredMarker.position,
                    new Vector3(10.5f, 0.18f, 1.25f)
                );

            Vector3 inward = ResolveInwardDirection(authoredMarker);
            Vector3 widthAxis =
                Vector3.Cross(Vector3.up, inward).normalized;
            if (widthAxis.sqrMagnitude < 0.001f)
                widthAxis = authoredMarker.right;

            float markerWidth = Mathf.Max(
                2f,
                ProjectBoundsSize(markerBounds, widthAxis)
            );
            float markerDepth = Mathf.Max(
                0.1f,
                ProjectBoundsSize(markerBounds, inward)
            );
            float markerWidthCoordinate =
                Vector3.Dot(markerBounds.center, widthAxis);

            Renderer leftWall = null;
            Renderer rightWall = null;
            float leftInnerEdge = float.NegativeInfinity;
            float rightInnerEdge = float.PositiveInfinity;
            float maximumPlaneDistance =
                Mathf.Max(1.5f, markerDepth * 1.8f);

            Renderer[] renderers =
                Resources.FindObjectsOfTypeAll<Renderer>();
            foreach (Renderer candidate in renderers)
            {
                if (candidate == null ||
                    candidate == markerRenderer ||
                    !candidate.gameObject.scene.IsValid() ||
                    candidate.name.IndexOf(
                        "Wall",
                        StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                Bounds bounds = candidate.bounds;
                if (bounds.size.y < 4f)
                    continue;

                float planeDistance = Mathf.Abs(
                    Vector3.Dot(
                        bounds.center - markerBounds.center,
                        inward
                    )
                );
                if (planeDistance > maximumPlaneDistance)
                    continue;

                float centerCoordinate =
                    Vector3.Dot(bounds.center, widthAxis);
                float halfWidth =
                    ProjectBoundsExtent(bounds, widthAxis);
                float minimum = centerCoordinate - halfWidth;
                float maximum = centerCoordinate + halfWidth;

                if (maximum <= markerWidthCoordinate &&
                    maximum > leftInnerEdge)
                {
                    leftInnerEdge = maximum;
                    leftWall = candidate;
                }

                if (minimum >= markerWidthCoordinate &&
                    minimum < rightInnerEdge)
                {
                    rightInnerEdge = minimum;
                    rightWall = candidate;
                }
            }

            if (leftWall != null && rightWall != null)
            {
                float wallPlaneCoordinate =
                    (
                        Vector3.Dot(leftWall.bounds.center, inward) +
                        Vector3.Dot(rightWall.bounds.center, inward)
                    ) * 0.5f;
                float openingWidthCoordinate =
                    (leftInnerEdge + rightInnerEdge) * 0.5f;

                float bottom = Mathf.Max(
                    leftWall.bounds.min.y,
                    rightWall.bounds.min.y
                );
                float top = Mathf.Min(
                    leftWall.bounds.max.y,
                    rightWall.bounds.max.y
                );
                if (top - bottom < 4f)
                {
                    bottom = Mathf.Min(
                        leftWall.bounds.min.y,
                        rightWall.bounds.min.y
                    );
                    top = Mathf.Max(
                        leftWall.bounds.max.y,
                        rightWall.bounds.max.y
                    );
                }

                Vector3 horizontalCenter = markerBounds.center;
                horizontalCenter += widthAxis * (
                    openingWidthCoordinate -
                    Vector3.Dot(horizontalCenter, widthAxis)
                );
                horizontalCenter += inward * (
                    wallPlaneCoordinate -
                    Vector3.Dot(horizontalCenter, inward)
                );

                center = new Vector3(
                    horizontalCenter.x,
                    (bottom + top) * 0.5f,
                    horizontalCenter.z
                );
                size = new Vector2(
                    Mathf.Max(2f, rightInnerEdge - leftInnerEdge) + 0.12f,
                    Mathf.Max(4f, top - bottom) + 0.12f
                );
            }
            else
            {
                // The marker is a horizontal threshold strip, not the vertical
                // doorway rectangle. Place the fallback on its outer edge and
                // use the production wall height rather than marker height.
                Vector3 wallPlane =
                    markerBounds.center -
                    inward * (markerDepth * 0.5f + 0.05f);
                center = new Vector3(
                    wallPlane.x,
                    markerBounds.min.y + 11f,
                    wallPlane.z
                );
                size = new Vector2(
                    markerWidth + 0.12f,
                    22.12f
                );
            }

            rotation = Quaternion.LookRotation(
                inward,
                Vector3.up
            );
        }

        private static Vector3 ResolveInwardDirection(
            Transform authoredMarker)
        {
            Renderer[] renderers =
                Resources.FindObjectsOfTypeAll<Renderer>();
            Renderer nearestRoom = null;
            float nearestDistance = float.PositiveInfinity;

            foreach (Renderer renderer in renderers)
            {
                if (renderer == null ||
                    !renderer.gameObject.scene.IsValid() ||
                    renderer.name.IndexOf(
                        "BD_MinimapRoom_",
                        StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                Vector3 delta =
                    renderer.bounds.center - authoredMarker.position;
                delta.y = 0f;
                float distance = delta.sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestRoom = renderer;
                }
            }

            if (nearestRoom != null)
            {
                Vector3 direction =
                    nearestRoom.bounds.center - authoredMarker.position;
                direction.y = 0f;
                if (direction.sqrMagnitude > 0.001f)
                    return direction.normalized;
            }

            Vector3 fallback = authoredMarker.forward;
            fallback.y = 0f;
            return fallback.sqrMagnitude > 0.001f
                ? fallback.normalized
                : Vector3.forward;
        }

        private static void SetMarkerRenderersVisible(
            Transform authoredMarker,
            bool visible)
        {
            if (authoredMarker == null)
                return;

            Renderer[] markerRenderers =
                authoredMarker.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in markerRenderers)
            {
                if (renderer != null)
                    renderer.enabled = visible;
            }
        }

        private void EnsureVisuals()
        {
            DisableLegacySurface();

            Shader opaqueShader = ResolveOpaqueShader();
            Shader transparentShader = ResolveTransparentShader();
            if (opaqueShader == null || transparentShader == null)
            {
                enabled = false;
                return;
            }

            coreSurface = EnsureCore(opaqueShader);
            haloSurface = EnsureHalo(transparentShader);

            if (portalLight == null)
            {
                portalLight = GetComponent<Light>();
                if (portalLight == null)
                    portalLight = gameObject.AddComponent<Light>();
            }

            portalLight.type = LightType.Point;
            portalLight.color = new Color(0.34f, 0.72f, 1f);
            portalLight.range = Mathf.Max(5f, openingSize.x * 0.72f);
            portalLight.intensity = 1.35f;
            portalLight.shadows = LightShadows.None;
            portalLight.enabled = true;
        }

        private Transform EnsureCore(Shader shader)
        {
            Transform child = transform.Find(CoreName);
            if (child == null)
            {
                GameObject cube =
                    GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = CoreName;
                cube.transform.SetParent(transform, false);
                child = cube.transform;
            }

            child.gameObject.SetActive(true);
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;

            Collider collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
                Destroy(collider);
            }

            if (coreMaterial == null ||
                coreMaterial.shader != shader)
            {
                if (coreMaterial != null)
                    Destroy(coreMaterial);

                coreMaterial = new Material(shader)
                {
                    name = "BD Opaque Full Doorway Portal Core",
                    hideFlags = HideFlags.DontSave,
                    renderQueue = (int)RenderQueue.Geometry + 20
                };
            }

            ConfigureOpaqueMaterial(coreMaterial);
            Renderer renderer = child.GetComponent<Renderer>();
            renderer.enabled = true;
            renderer.sharedMaterial = coreMaterial;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            return child;
        }

        private Transform EnsureHalo(Shader shader)
        {
            if (haloMask == null)
                haloMask = CreateHaloTexture(256);

            Transform child = transform.Find(HaloName);
            if (child == null)
            {
                GameObject quad =
                    GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.name = HaloName;
                quad.transform.SetParent(transform, false);
                child = quad.transform;
            }

            child.gameObject.SetActive(true);
            child.localPosition = new Vector3(0f, 0f, 0.055f);
            child.localRotation = Quaternion.identity;

            Collider collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
                Destroy(collider);
            }

            if (haloMaterial == null ||
                haloMaterial.shader != shader)
            {
                if (haloMaterial != null)
                    Destroy(haloMaterial);

                haloMaterial = new Material(shader)
                {
                    name = "BD Full Doorway Portal Halo",
                    hideFlags = HideFlags.DontSave,
                    renderQueue = (int)RenderQueue.Transparent + 80
                };
            }

            ConfigureTransparentMaterial(haloMaterial);
            Renderer renderer = child.GetComponent<Renderer>();
            renderer.enabled = true;
            renderer.sharedMaterial = haloMaterial;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.sortingOrder = 220;
            return child;
        }

        private static Shader ResolveOpaqueShader()
        {
            Shader shader = Shader.Find(
                "Universal Render Pipeline/Unlit"
            );
            if (shader == null)
                shader = Shader.Find("Unlit/Color");
            if (shader == null)
                shader = Shader.Find("Standard");
            return shader;
        }

        private static Shader ResolveTransparentShader()
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader == null)
                shader = Shader.Find("Unlit/Transparent");
            if (shader == null)
            {
                shader = Shader.Find(
                    "Universal Render Pipeline/Unlit"
                );
            }
            return shader;
        }

        private static void ConfigureOpaqueMaterial(
            Material target)
        {
            if (target == null)
                return;

            if (target.HasProperty("_Surface"))
                target.SetFloat("_Surface", 0f);
            if (target.HasProperty("_Cull"))
                target.SetFloat("_Cull", 0f);
            if (target.HasProperty("_ZWrite"))
                target.SetFloat("_ZWrite", 1f);

            target.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            target.DisableKeyword("_ALPHABLEND_ON");
        }

        private void ConfigureTransparentMaterial(
            Material target)
        {
            if (target == null)
                return;

            target.mainTexture = haloMask;
            if (target.HasProperty("_BaseMap"))
                target.SetTexture("_BaseMap", haloMask);
            if (target.HasProperty("_MainTex"))
                target.SetTexture("_MainTex", haloMask);
            if (target.HasProperty("_Surface"))
                target.SetFloat("_Surface", 1f);
            if (target.HasProperty("_Cull"))
                target.SetFloat("_Cull", 0f);
            if (target.HasProperty("_ZWrite"))
                target.SetFloat("_ZWrite", 0f);
            if (target.HasProperty("_SrcBlend"))
            {
                target.SetFloat(
                    "_SrcBlend",
                    (float)BlendMode.SrcAlpha
                );
            }
            if (target.HasProperty("_DstBlend"))
            {
                target.SetFloat(
                    "_DstBlend",
                    (float)BlendMode.OneMinusSrcAlpha
                );
            }

            target.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            target.EnableKeyword("_ALPHABLEND_ON");
        }

        private static Texture2D CreateHaloTexture(int size)
        {
            int safeSize = Mathf.Max(64, size);
            Texture2D texture = new Texture2D(
                safeSize,
                safeSize,
                TextureFormat.RGBA32,
                mipChain: false
            );
            texture.name = "BD Full Opening Halo Mask";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            Color[] pixels = new Color[safeSize * safeSize];
            float center = (safeSize - 1) * 0.5f;
            for (int y = 0; y < safeSize; y++)
            {
                for (int x = 0; x < safeSize; x++)
                {
                    float nx = Mathf.Abs((x - center) / center);
                    float ny = Mathf.Abs((y - center) / center);
                    float edge = Mathf.Max(nx, ny);
                    float alpha = 1f - Mathf.SmoothStep(
                        0.72f,
                        1.0f,
                        edge
                    );
                    pixels[y * safeSize + x] = new Color(
                        1f,
                        1f,
                        1f,
                        Mathf.Clamp01(alpha)
                    );
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, true);
            return texture;
        }

        private void Update()
        {
            phase += Time.unscaledDeltaTime;
            ApplyVisualState(
                0.5f + 0.5f * Mathf.Sin(phase * 1.7f)
            );
        }

        private void ApplyVisualState(float pulse01)
        {
            EnsureVisuals();

            // The opaque core never shrinks or feathers. It is a thin vertical
            // slab exactly on the wall plane, filling the complete opening from
            // left to right and floor to ceiling so no viewing angle leaks.
            if (coreSurface != null)
            {
                coreSurface.localScale = new Vector3(
                    openingSize.x,
                    openingSize.y,
                    0.10f
                );
            }

            float haloPulse = 1.02f + pulse01 * 0.015f;
            if (haloSurface != null)
            {
                haloSurface.localScale = new Vector3(
                    openingSize.x * haloPulse,
                    openingSize.y * haloPulse,
                    1f
                );
            }

            Color coreColor = Color.Lerp(
                new Color(0.18f, 0.55f, 0.93f, 1f),
                new Color(0.70f, 0.94f, 1f, 1f),
                pulse01
            );
            Color haloColor = Color.Lerp(
                new Color(0.12f, 0.44f, 0.92f, 0.22f),
                new Color(0.54f, 0.90f, 1f, 0.38f),
                pulse01
            );

            SetMaterialColor(coreMaterial, coreColor);
            SetMaterialColor(haloMaterial, haloColor);

            if (portalLight != null)
            {
                portalLight.range = Mathf.Max(
                    5f,
                    openingSize.x * 0.72f
                );
                portalLight.intensity = Mathf.Lerp(
                    1.20f,
                    1.75f,
                    pulse01
                );
            }
        }

        private static void SetMaterialColor(
            Material target,
            Color color)
        {
            if (target == null)
                return;

            target.color = color;
            if (target.HasProperty("_BaseColor"))
                target.SetColor("_BaseColor", color);
            if (target.HasProperty("_Color"))
                target.SetColor("_Color", color);
        }

        private void DisableLegacySurface()
        {
            string[] legacyNames =
            {
                LegacySurfaceName,
                "PortalLightCore_EffectOnly_NotADoor",
                "PortalLightHalo_EffectOnly_NotADoor"
            };

            foreach (string legacyName in legacyNames)
            {
                Transform legacy = transform.Find(legacyName);
                if (legacy == null)
                    continue;

                Renderer renderer = legacy.GetComponent<Renderer>();
                if (renderer != null)
                    renderer.enabled = false;
                legacy.gameObject.SetActive(false);
            }
        }

        private static float ProjectBoundsSize(
            Bounds bounds,
            Vector3 axis)
        {
            return ProjectBoundsExtent(bounds, axis) * 2f;
        }

        private static float ProjectBoundsExtent(
            Bounds bounds,
            Vector3 axis)
        {
            Vector3 normalized = axis.normalized;
            Vector3 extents = bounds.extents;
            return
                Mathf.Abs(normalized.x) * extents.x +
                Mathf.Abs(normalized.y) * extents.y +
                Mathf.Abs(normalized.z) * extents.z;
        }

        private void OnDestroy()
        {
            if (coreMaterial != null)
                Destroy(coreMaterial);
            if (haloMaterial != null)
                Destroy(haloMaterial);
            if (haloMask != null)
                Destroy(haloMask);
        }
    }
}
