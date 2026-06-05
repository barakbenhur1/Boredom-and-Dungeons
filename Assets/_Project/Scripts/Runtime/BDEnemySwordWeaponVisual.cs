using System.Collections;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDEnemySwordWeaponVisual : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private float windupDuration = 0.12f;
        [SerializeField] private float slashDuration = 0.16f;
        [SerializeField] private float recoveryDuration = 0.22f;
        [SerializeField] private float windupAngle = 72f;
        [SerializeField] private float slashAngle = 28f;
        [SerializeField] private float forwardReach = 0.22f;

        [Header("Sword placement")]
        [SerializeField] private Vector3 leftPivotPosition = new Vector3(-0.48f, 0.30f, 0.05f);
        [SerializeField] private Vector3 rightPivotPosition = new Vector3(0.48f, 0.30f, 0.05f);
        [SerializeField] private float bladeLength = 1.18f;
        [SerializeField] private float bladeWidth = 0.10f;

        private Transform leftPivot;
        private Transform rightPivot;
        private Quaternion leftRestRotation;
        private Quaternion rightRestRotation;
        private Vector3 leftRestPosition;
        private Vector3 rightRestPosition;
        private Coroutine attackRoutine;

        public bool IsAnimating => attackRoutine != null;

        private void Awake()
        {
            EnsureSwordVisuals();
            CacheRestPose();
        }

        public void PlayDoubleSlash()
        {
            EnsureSwordVisuals();

            if (attackRoutine != null)
                StopCoroutine(attackRoutine);

            RestoreRestPose();
            attackRoutine = StartCoroutine(DoubleSlashRoutine());
        }

        private void EnsureSwordVisuals()
        {
            if (leftPivot == null)
                leftPivot = FindOrCreateSword("BD_Sword_Left_Pivot", leftPivotPosition, true);

            if (rightPivot == null)
                rightPivot = FindOrCreateSword("BD_Sword_Right_Pivot", rightPivotPosition, false);
        }

        private Transform FindOrCreateSword(string pivotName, Vector3 localPosition, bool isLeft)
        {
            Transform existing = transform.Find(pivotName);
            if (existing != null)
                return existing;

            GameObject pivotObject = new GameObject(pivotName);
            Transform pivot = pivotObject.transform;
            pivot.SetParent(transform, false);
            pivot.localPosition = localPosition;
            pivot.localRotation = Quaternion.Euler(0f, isLeft ? -42f : 42f, 0f);

            GameObject blade = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blade.name = isLeft ? "BD_Sword_Left_Blade" : "BD_Sword_Right_Blade";
            blade.transform.SetParent(pivot, false);
            blade.transform.localPosition = new Vector3(0f, 0f, bladeLength * 0.5f);
            blade.transform.localScale = new Vector3(bladeWidth, bladeWidth * 0.65f, bladeLength);

            Collider bladeCollider = blade.GetComponent<Collider>();
            if (bladeCollider != null)
                Destroy(bladeCollider);

            Renderer bladeRenderer = blade.GetComponent<Renderer>();
            if (bladeRenderer != null)
                bladeRenderer.sharedMaterial = CreateSwordMaterial();

            GameObject guard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            guard.name = isLeft ? "BD_Sword_Left_Guard" : "BD_Sword_Right_Guard";
            guard.transform.SetParent(pivot, false);
            guard.transform.localPosition = new Vector3(0f, 0f, 0.08f);
            guard.transform.localScale = new Vector3(0.38f, 0.10f, 0.10f);

            Collider guardCollider = guard.GetComponent<Collider>();
            if (guardCollider != null)
                Destroy(guardCollider);

            Renderer guardRenderer = guard.GetComponent<Renderer>();
            if (guardRenderer != null)
                guardRenderer.sharedMaterial = CreateGuardMaterial();

            GameObject handle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            handle.name = isLeft ? "BD_Sword_Left_Handle" : "BD_Sword_Right_Handle";
            handle.transform.SetParent(pivot, false);
            handle.transform.localPosition = new Vector3(0f, 0f, -0.12f);
            handle.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            handle.transform.localScale = new Vector3(0.07f, 0.20f, 0.07f);

            Collider handleCollider = handle.GetComponent<Collider>();
            if (handleCollider != null)
                Destroy(handleCollider);

            Renderer handleRenderer = handle.GetComponent<Renderer>();
            if (handleRenderer != null)
                handleRenderer.sharedMaterial = CreateHandleMaterial();

            return pivot;
        }

        private void CacheRestPose()
        {
            if (leftPivot != null)
            {
                leftRestRotation = leftPivot.localRotation;
                leftRestPosition = leftPivot.localPosition;
            }

            if (rightPivot != null)
            {
                rightRestRotation = rightPivot.localRotation;
                rightRestPosition = rightPivot.localPosition;
            }
        }

        private void RestoreRestPose()
        {
            if (leftPivot != null)
            {
                leftPivot.localRotation = leftRestRotation;
                leftPivot.localPosition = leftRestPosition;
            }

            if (rightPivot != null)
            {
                rightPivot.localRotation = rightRestRotation;
                rightPivot.localPosition = rightRestPosition;
            }
        }

        private IEnumerator DoubleSlashRoutine()
        {
            Quaternion leftWindup = leftRestRotation * Quaternion.Euler(0f, -windupAngle, 0f);
            Quaternion rightWindup = rightRestRotation * Quaternion.Euler(0f, windupAngle, 0f);

            yield return AnimatePose(
                leftRestRotation,
                rightRestRotation,
                leftWindup,
                rightWindup,
                leftRestPosition,
                rightRestPosition,
                windupDuration
            );

            Quaternion leftSlash = leftRestRotation * Quaternion.Euler(0f, slashAngle, 0f);
            Quaternion rightSlash = rightRestRotation * Quaternion.Euler(0f, -slashAngle, 0f);
            Vector3 leftForward = leftRestPosition + Vector3.forward * forwardReach;
            Vector3 rightForward = rightRestPosition + Vector3.forward * forwardReach;

            yield return AnimatePose(
                leftWindup,
                rightWindup,
                leftSlash,
                rightSlash,
                leftForward,
                rightForward,
                slashDuration
            );

            yield return AnimatePose(
                leftSlash,
                rightSlash,
                leftRestRotation,
                rightRestRotation,
                leftRestPosition,
                rightRestPosition,
                recoveryDuration
            );

            RestoreRestPose();
            attackRoutine = null;
        }

        private IEnumerator AnimatePose(
            Quaternion leftFrom,
            Quaternion rightFrom,
            Quaternion leftTo,
            Quaternion rightTo,
            Vector3 leftTargetPosition,
            Vector3 rightTargetPosition,
            float duration)
        {
            if (leftPivot == null || rightPivot == null)
                yield break;

            Vector3 leftStartPosition = leftPivot.localPosition;
            Vector3 rightStartPosition = rightPivot.localPosition;
            float safeDuration = Mathf.Max(0.01f, duration);
            float elapsed = 0f;

            while (elapsed < safeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / safeDuration);
                float eased = t * t * (3f - 2f * t);

                leftPivot.localRotation = Quaternion.Slerp(leftFrom, leftTo, eased);
                rightPivot.localRotation = Quaternion.Slerp(rightFrom, rightTo, eased);
                leftPivot.localPosition = Vector3.Lerp(leftStartPosition, leftTargetPosition, eased);
                rightPivot.localPosition = Vector3.Lerp(rightStartPosition, rightTargetPosition, eased);

                yield return null;
            }

            leftPivot.localRotation = leftTo;
            rightPivot.localRotation = rightTo;
            leftPivot.localPosition = leftTargetPosition;
            rightPivot.localPosition = rightTargetPosition;
        }

        private static Material CreateSwordMaterial()
        {
            Shader shader = FindShader();
            Material material = new Material(shader);
            Color color = new Color(0.78f, 0.84f, 0.90f, 1f);
            ApplyColor(material, color);

            if (material.HasProperty("_Metallic"))
                material.SetFloat("_Metallic", 0.75f);

            if (material.HasProperty("_Smoothness"))
                material.SetFloat("_Smoothness", 0.72f);

            return material;
        }

        private static Material CreateGuardMaterial()
        {
            Shader shader = FindShader();
            Material material = new Material(shader);
            ApplyColor(material, new Color(0.34f, 0.18f, 0.05f, 1f));
            return material;
        }

        private static Material CreateHandleMaterial()
        {
            Shader shader = FindShader();
            Material material = new Material(shader);
            ApplyColor(material, new Color(0.08f, 0.06f, 0.05f, 1f));
            return material;
        }

        private static Shader FindShader()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
                shader = Shader.Find("Standard");
            if (shader == null)
                shader = Shader.Find("Unlit/Color");
            if (shader == null)
                shader = Shader.Find("Sprites/Default");

            return shader;
        }

        private static void ApplyColor(Material material, Color color)
        {
            material.color = color;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);
        }

        private void OnDisable()
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }

            RestoreRestPose();
        }
    }
}
