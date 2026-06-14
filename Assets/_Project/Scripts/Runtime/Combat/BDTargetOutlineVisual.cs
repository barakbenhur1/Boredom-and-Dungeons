using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDTargetOutlineVisual : MonoBehaviour
    {
        private const string ShellName = "__BD_TARGET_OUTLINE_SHELL";
        private static Material sharedOutlineMaterial;

        private readonly List<GameObject> shells =
            new List<GameObject>(16);
        private readonly List<Mesh> generatedOutlineMeshes =
            new List<Mesh>(8);
        private bool built;

        public void SetHighlighted(
            bool highlighted,
            Color color,
            float widthPixels)
        {
            if (highlighted)
                EnsureShells();

            Material material = ResolveMaterial();
            if (material != null)
            {
                material.SetColor("_OutlineColor", color);
                material.SetFloat(
                    "_OutlinePixels",
                    Mathf.Clamp(widthPixels, 1f, 8f)
                );
            }

            for (int index = 0; index < shells.Count; index++)
            {
                GameObject shell = shells[index];
                if (shell != null)
                    shell.SetActive(highlighted && material != null);
            }
        }

        private void OnDisable()
        {
            SetHighlighted(false, Color.red, 2f);
        }

        private void OnDestroy()
        {
            for (int index = 0; index < shells.Count; index++)
            {
                if (shells[index] != null)
                    Destroy(shells[index]);
            }
            shells.Clear();

            for (int index = 0;
                 index < generatedOutlineMeshes.Count;
                 index++)
            {
                if (generatedOutlineMeshes[index] != null)
                    Destroy(generatedOutlineMeshes[index]);
            }
            generatedOutlineMeshes.Clear();
        }

        private void EnsureShells()
        {
            if (built)
                return;

            built = true;
            Material material = ResolveMaterial();
            if (material == null)
                return;

            // BD STRICT DAMAGEABLE BODY TARGET OUTLINE V10.11.30.78
            // The outline belongs only to the vulnerable enemy body. Broad
            // rings, floor circles, indicators and mixed-mesh support submeshes
            // remain in their authored color and never receive the red shell.
            BDHealth ownerHealth = GetComponent<BDHealth>();
            Collider[] damageableColliders =
                ResolveDamageableColliders(ownerHealth);

            Bounds? damageableEnvelope =
                ResolveDamageableEnvelope(damageableColliders);

            Renderer[] sources =
                GetComponentsInChildren<Renderer>(
                    includeInactive: false
                );

            for (int index = 0; index < sources.Length; index++)
            {
                Renderer source = sources[index];
                if (!IsEligibleSourceRenderer(
                        source,
                        ownerHealth,
                        damageableColliders,
                        damageableEnvelope))
                {
                    continue;
                }

                if (source is SkinnedMeshRenderer skinned)
                    CreateSkinnedShell(
                        skinned,
                        material,
                        damageableEnvelope
                    );
                else if (source is MeshRenderer meshRenderer)
                    CreateMeshShell(
                        meshRenderer,
                        material,
                        damageableEnvelope
                    );
            }
        }

        private bool IsEligibleSourceRenderer(
            Renderer source,
            BDHealth ownerHealth,
            Collider[] damageableColliders,
            Bounds? damageableEnvelope)
        {
            if (source == null ||
                source is LineRenderer ||
                source is TrailRenderer ||
                source is ParticleSystemRenderer ||
                source.gameObject.name.StartsWith(
                    ShellName,
                    StringComparison.Ordinal))
            {
                return false;
            }

            if (BDAuxiliaryEnemyRingTransparency
                    .IsAuxiliaryRingRenderer(
                        source,
                        transform))
            {
                return false;
            }

            if (damageableEnvelope.HasValue &&
                BDAuxiliaryEnemyRingTransparency.IsAuxiliaryBounds(
                    source.bounds,
                    transform,
                    damageableEnvelope))
            {
                return false;
            }

            return RendererIntersectsDamageableEnvelope(
                source,
                ownerHealth,
                damageableColliders
            );
        }

        private Collider[] ResolveDamageableColliders(
            BDHealth ownerHealth)
        {
            Collider[] candidates =
                GetComponentsInChildren<Collider>(
                    includeInactive: true
                );

            List<Collider> resolved =
                new List<Collider>(candidates.Length);

            for (int index = 0;
                 index < candidates.Length;
                 index++)
            {
                Collider candidate = candidates[index];
                if (candidate == null ||
                    !candidate.enabled ||
                    candidate.isTrigger)
                {
                    continue;
                }

                BDHealth nearestHealth =
                    candidate.GetComponentInParent<BDHealth>();

                if (ownerHealth != null &&
                    nearestHealth != ownerHealth)
                {
                    continue;
                }

                Renderer localRenderer =
                    candidate.GetComponent<Renderer>();
                if (localRenderer != null &&
                    BDAuxiliaryEnemyRingTransparency
                        .IsAuxiliaryRingRenderer(
                            localRenderer,
                            transform))
                {
                    continue;
                }

                resolved.Add(candidate);
            }

            return resolved.ToArray();
        }

        private static Bounds? ResolveDamageableEnvelope(
            Collider[] colliders)
        {
            if (colliders == null || colliders.Length == 0)
                return null;

            bool initialized = false;
            Bounds envelope = default;

            for (int index = 0; index < colliders.Length; index++)
            {
                Collider collider = colliders[index];
                if (collider == null)
                    continue;

                if (!initialized)
                {
                    envelope = collider.bounds;
                    initialized = true;
                }
                else
                {
                    envelope.Encapsulate(collider.bounds);
                }
            }

            return initialized ? envelope : null;
        }

        private static bool RendererIntersectsDamageableEnvelope(
            Renderer source,
            BDHealth ownerHealth,
            Collider[] damageableColliders)
        {
            if (source == null)
                return false;

            BDHealth nearestHealth =
                source.GetComponentInParent<BDHealth>();

            if (ownerHealth != null &&
                nearestHealth != null &&
                nearestHealth != ownerHealth)
            {
                return false;
            }

            if (damageableColliders == null ||
                damageableColliders.Length == 0)
            {
                return true;
            }

            Bounds rendererBounds = source.bounds;

            for (int index = 0;
                 index < damageableColliders.Length;
                 index++)
            {
                Collider collider = damageableColliders[index];
                if (collider == null)
                    continue;

                Bounds damageableBounds = collider.bounds;
                damageableBounds.Expand(0.10f);

                if (rendererBounds.Intersects(damageableBounds))
                    return true;
            }

            return false;
        }

        private void CreateMeshShell(
            MeshRenderer source,
            Material material,
            Bounds? damageableEnvelope)
        {
            MeshFilter filter = source.GetComponent<MeshFilter>();
            if (filter == null || filter.sharedMesh == null)
                return;

            Mesh outlineMesh = ResolveOutlineMesh(
                source,
                filter.sharedMesh,
                damageableEnvelope
            );
            if (outlineMesh == null)
                return;

            GameObject shell = new GameObject(ShellName + "_Mesh");
            shell.transform.SetParent(source.transform, false);
            shell.layer = source.gameObject.layer;

            MeshFilter shellFilter = shell.AddComponent<MeshFilter>();
            shellFilter.sharedMesh = outlineMesh;

            MeshRenderer shellRenderer = shell.AddComponent<MeshRenderer>();
            ConfigureRenderer(shellRenderer, source, material);
            shells.Add(shell);
        }

        private void CreateSkinnedShell(
            SkinnedMeshRenderer source,
            Material material,
            Bounds? damageableEnvelope)
        {
            if (source.sharedMesh == null)
                return;

            Mesh outlineMesh = ResolveOutlineMesh(
                source,
                source.sharedMesh,
                damageableEnvelope
            );
            if (outlineMesh == null)
                return;

            GameObject shell = new GameObject(ShellName + "_Skinned");
            shell.transform.SetParent(source.transform, false);
            shell.layer = source.gameObject.layer;

            SkinnedMeshRenderer shellRenderer =
                shell.AddComponent<SkinnedMeshRenderer>();
            shellRenderer.sharedMesh = outlineMesh;
            shellRenderer.bones = source.bones;
            shellRenderer.rootBone = source.rootBone;
            shellRenderer.localBounds = source.localBounds;
            shellRenderer.quality = source.quality;
            shellRenderer.updateWhenOffscreen = source.updateWhenOffscreen;
            ConfigureRenderer(shellRenderer, source, material);
            shells.Add(shell);
        }

        private Mesh ResolveOutlineMesh(
            Renderer source,
            Mesh sourceMesh,
            Bounds? damageableEnvelope)
        {
            if (sourceMesh == null)
                return null;

            if (!damageableEnvelope.HasValue ||
                sourceMesh.subMeshCount <= 1)
            {
                return sourceMesh;
            }

            bool[] excludeSubMesh =
                new bool[sourceMesh.subMeshCount];
            bool excludedAny = false;
            bool retainedAny = false;

            for (int subMeshIndex = 0;
                 subMeshIndex < sourceMesh.subMeshCount;
                 subMeshIndex++)
            {
                Bounds localBounds =
                    sourceMesh.GetSubMesh(subMeshIndex).bounds;
                Bounds worldBounds = TransformBounds(
                    source.transform.localToWorldMatrix,
                    localBounds
                );

                bool auxiliary =
                    BDAuxiliaryEnemyRingTransparency.IsAuxiliaryBounds(
                        worldBounds,
                        transform,
                        damageableEnvelope
                    );

                excludeSubMesh[subMeshIndex] = auxiliary;
                excludedAny |= auxiliary;
                retainedAny |= !auxiliary;
            }

            if (!retainedAny)
                return null;

            if (!excludedAny)
                return sourceMesh;

            Mesh filteredMesh = Instantiate(sourceMesh);
            filteredMesh.name =
                sourceMesh.name + "_BD_DamageableBodyOutline";
            filteredMesh.hideFlags = HideFlags.HideAndDontSave;

            for (int subMeshIndex = 0;
                 subMeshIndex < filteredMesh.subMeshCount;
                 subMeshIndex++)
            {
                if (!excludeSubMesh[subMeshIndex])
                    continue;

                filteredMesh.SetTriangles(
                    Array.Empty<int>(),
                    subMeshIndex,
                    calculateBounds: false
                );
            }

            filteredMesh.RecalculateBounds();
            generatedOutlineMeshes.Add(filteredMesh);
            return filteredMesh;
        }

        private static Bounds TransformBounds(
            Matrix4x4 matrix,
            Bounds localBounds)
        {
            Vector3 center = matrix.MultiplyPoint3x4(localBounds.center);
            Vector3 extents = localBounds.extents;

            Vector3 axisX = matrix.MultiplyVector(
                new Vector3(extents.x, 0f, 0f)
            );
            Vector3 axisY = matrix.MultiplyVector(
                new Vector3(0f, extents.y, 0f)
            );
            Vector3 axisZ = matrix.MultiplyVector(
                new Vector3(0f, 0f, extents.z)
            );

            Vector3 worldExtents = new Vector3(
                Mathf.Abs(axisX.x) +
                Mathf.Abs(axisY.x) +
                Mathf.Abs(axisZ.x),
                Mathf.Abs(axisX.y) +
                Mathf.Abs(axisY.y) +
                Mathf.Abs(axisZ.y),
                Mathf.Abs(axisX.z) +
                Mathf.Abs(axisY.z) +
                Mathf.Abs(axisZ.z)
            );

            return new Bounds(center, worldExtents * 2f);
        }

        private static void ConfigureRenderer(
            Renderer destination,
            Renderer source,
            Material material)
        {
            int materialCount = Mathf.Max(
                1,
                source.sharedMaterials != null
                    ? source.sharedMaterials.Length
                    : 1
            );

            Material[] materials = new Material[materialCount];
            for (int index = 0; index < materialCount; index++)
                materials[index] = material;

            destination.sharedMaterials = materials;
            destination.shadowCastingMode = ShadowCastingMode.Off;
            destination.receiveShadows = false;
            destination.lightProbeUsage = LightProbeUsage.Off;
            destination.reflectionProbeUsage = ReflectionProbeUsage.Off;
            destination.motionVectorGenerationMode =
                MotionVectorGenerationMode.ForceNoMotion;
        }

        private static Material ResolveMaterial()
        {
            if (sharedOutlineMaterial != null)
                return sharedOutlineMaterial;

            Shader shader = Shader.Find(
                "BoredomAndDungeons/TargetSilhouetteOutline"
            );
            if (shader == null)
                return null;

            sharedOutlineMaterial = new Material(shader)
            {
                name = "BD_TargetSilhouetteOutline_Runtime",
                hideFlags = HideFlags.HideAndDontSave
            };
            return sharedOutlineMaterial;
        }
    }
}
