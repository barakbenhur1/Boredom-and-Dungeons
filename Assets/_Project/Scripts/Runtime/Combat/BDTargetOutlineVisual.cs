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
        }

        private void EnsureShells()
        {
            if (built)
                return;

            built = true;
            Material material = ResolveMaterial();
            if (material == null)
                return;

            // BD DAMAGEABLE MODEL ONLY TARGET OUTLINE V23R19O
            BDHealth ownerHealth = GetComponent<BDHealth>();
            Collider[] damageableColliders =
                ResolveDamageableColliders(ownerHealth);

            Renderer[] sources =
                GetComponentsInChildren<Renderer>(
                    includeInactive: false
                );

            for (int index = 0; index < sources.Length; index++)
            {
                Renderer source = sources[index];
                if (source == null ||
                    source is LineRenderer ||
                    source is TrailRenderer ||
                    source is ParticleSystemRenderer ||
                    source.gameObject.name.StartsWith(ShellName))
                {
                    continue;
                }

                if (BDAuxiliaryEnemyRingTransparency
                        .IsAuxiliaryRingRenderer(
                            source,
                            transform))
                {
                    BDAuxiliaryEnemyRingTransparency.ApplyTransparency(
                        source,
                        0.62f
                    );
                    continue;
                }

                if (!RendererIntersectsDamageableEnvelope(
                        source,
                        ownerHealth,
                        damageableColliders))
                {
                    continue;
                }

                if (source is SkinnedMeshRenderer skinned)
                    CreateSkinnedShell(skinned, material);
                else if (source is MeshRenderer meshRenderer)
                    CreateMeshShell(meshRenderer, material);
            }
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

                if (BDAuxiliaryEnemyRingTransparency
                        .IsAuxiliaryRingRenderer(
                            candidate.GetComponent<Renderer>(),
                            transform))
                {
                    continue;
                }

                resolved.Add(candidate);
            }

            return resolved.ToArray();
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
                Collider collider =
                    damageableColliders[index];

                if (collider == null)
                    continue;

                Bounds damageableBounds = collider.bounds;
                damageableBounds.Expand(0.18f);

                if (rendererBounds.Intersects(damageableBounds))
                    return true;
            }

            return false;
        }

        private void CreateMeshShell(
            MeshRenderer source,
            Material material)
        {
            MeshFilter filter = source.GetComponent<MeshFilter>();
            if (filter == null || filter.sharedMesh == null)
                return;

            GameObject shell = new GameObject(ShellName + "_Mesh");
            shell.transform.SetParent(source.transform, false);
            shell.layer = source.gameObject.layer;

            MeshFilter shellFilter = shell.AddComponent<MeshFilter>();
            shellFilter.sharedMesh = filter.sharedMesh;

            MeshRenderer shellRenderer = shell.AddComponent<MeshRenderer>();
            ConfigureRenderer(shellRenderer, source, material);
            shells.Add(shell);
        }

        private void CreateSkinnedShell(
            SkinnedMeshRenderer source,
            Material material)
        {
            if (source.sharedMesh == null)
                return;

            GameObject shell = new GameObject(ShellName + "_Skinned");
            shell.transform.SetParent(source.transform, false);
            shell.layer = source.gameObject.layer;

            SkinnedMeshRenderer shellRenderer =
                shell.AddComponent<SkinnedMeshRenderer>();
            shellRenderer.sharedMesh = source.sharedMesh;
            shellRenderer.bones = source.bones;
            shellRenderer.rootBone = source.rootBone;
            shellRenderer.localBounds = source.localBounds;
            shellRenderer.quality = source.quality;
            shellRenderer.updateWhenOffscreen = source.updateWhenOffscreen;
            ConfigureRenderer(shellRenderer, source, material);
            shells.Add(shell);
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
