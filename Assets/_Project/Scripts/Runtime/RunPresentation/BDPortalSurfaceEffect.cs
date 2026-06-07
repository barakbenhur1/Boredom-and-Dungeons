using UnityEngine;
using UnityEngine.Rendering;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDPortalSurfaceEffect : MonoBehaviour
    {
        // BD AUTHORED DOOR EFFECT ONLY V6
        private Material material;
        private Transform surface;
        private float phase;

        private void Awake()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null)
                shader = Shader.Find("Unlit/Color");
            if (shader == null)
                shader = Shader.Find("Sprites/Default");
            if (shader == null)
            {
                enabled = false;
                return;
            }

            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = "LightSurface";
            quad.transform.SetParent(transform, false);
            quad.transform.localPosition = Vector3.zero;
            quad.transform.localRotation = Quaternion.identity;
            quad.transform.localScale = new Vector3(3.2f, 3.4f, 1f);

            Collider collider = quad.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            material = new Material(shader)
            {
                name = "BD Portal Effect Runtime Material",
                hideFlags = HideFlags.DontSave,
                renderQueue = (int)RenderQueue.Transparent
            };
            if (material.HasProperty("_Surface")) material.SetFloat("_Surface", 1f);
            if (material.HasProperty("_Cull")) material.SetFloat("_Cull", 0f);
            if (material.HasProperty("_ZWrite")) material.SetFloat("_ZWrite", 0f);

            Renderer renderer = quad.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
            surface = quad.transform;
        }

        private void Update()
        {
            phase += Time.unscaledDeltaTime;
            float pulse = 0.5f + 0.5f * Mathf.Sin(phase * 1.6f);
            Color color = Color.Lerp(
                new Color(0.18f, 0.54f, 0.88f, 0.90f),
                new Color(0.58f, 0.88f, 1f, 0.96f),
                pulse);

            if (material != null)
            {
                if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
                if (material.HasProperty("_Color")) material.SetColor("_Color", color);
            }

            if (surface != null)
            {
                float scale = 1f + Mathf.Sin(phase * 2.1f) * 0.012f;
                surface.localScale = new Vector3(3.2f * scale, 3.4f, 1f);
            }
        }

        private void OnDestroy()
        {
            if (material != null)
                Destroy(material);
        }
    }
}
