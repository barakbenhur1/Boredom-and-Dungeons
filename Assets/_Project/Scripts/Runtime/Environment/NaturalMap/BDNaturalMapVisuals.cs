using UnityEngine;

namespace BoredomAndDungeons.Environment.NaturalMap
{
    public static class BDNaturalMapVisuals
    {
        private static Material stoneMaterial;
        private static Material earthMaterial;
        private static Material grassMaterial;
        private static Material dryMaterial;

        public static GameObject CreateRoundedCornerRock(Vector3 position, Quaternion rotation, float radius, float height, bool mossy)
        {
            GameObject rock = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rock.name = mossy ? "BD_Natural_Mossy_Rounded_Corner" : "BD_Natural_Rounded_Corner";
            rock.transform.position = position;
            rock.transform.rotation = rotation;
            rock.transform.localScale = new Vector3(radius, height, radius);

            Renderer renderer = rock.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = mossy ? GetGrassMaterial() : GetStoneMaterial();

            rock.AddComponent<BDNaturalMapVisualMarker>();
            return rock;
        }

        public static GameObject CreateSoftEdgeBoulder(Vector3 position, Vector3 scale, Quaternion rotation, bool mossy)
        {
            GameObject boulder = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            boulder.name = mossy ? "BD_Natural_Mossy_Edge_Boulder" : "BD_Natural_Edge_Boulder";
            boulder.transform.position = position;
            boulder.transform.rotation = rotation;
            boulder.transform.localScale = scale;

            Renderer renderer = boulder.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = mossy ? GetGrassMaterial() : GetStoneMaterial();

            boulder.AddComponent<BDNaturalMapVisualMarker>();
            return boulder;
        }

        public static GameObject CreateGroundPatch(Vector3 position, Vector3 scale, Quaternion rotation, int biomeHint)
        {
            GameObject patch = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            patch.name = ResolvePatchName(biomeHint);
            patch.transform.position = position + Vector3.up * 0.012f;
            patch.transform.rotation = rotation;
            patch.transform.localScale = new Vector3(scale.x, 0.018f, scale.z);

            Collider collider = patch.GetComponent<Collider>();
            if (collider != null)
                DestroyColliderSafely(collider);

            Renderer renderer = patch.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sharedMaterial = ResolvePatchMaterial(biomeHint);

            patch.AddComponent<BDNaturalMapVisualMarker>();
            return patch;
        }

        private static void DestroyColliderSafely(Collider collider)
        {
            if (Application.isPlaying)
                UnityEngine.Object.Destroy(collider);
            else
                UnityEngine.Object.DestroyImmediate(collider);
        }

        private static Material ResolvePatchMaterial(int biomeHint)
        {
            switch (biomeHint)
            {
                case 0: return GetGrassMaterial();
                case 1: return GetEarthMaterial();
                case 2: return GetDryMaterial();
                default: return GetEarthMaterial();
            }
        }

        private static string ResolvePatchName(int biomeHint)
        {
            switch (biomeHint)
            {
                case 0: return "BD_Natural_GroundPatch_Grass";
                case 1: return "BD_Natural_GroundPatch_Earth";
                case 2: return "BD_Natural_GroundPatch_Dry";
                default: return "BD_Natural_GroundPatch_Mixed";
            }
        }

        private static Material GetStoneMaterial()
        {
            if (stoneMaterial != null)
                return stoneMaterial;

            stoneMaterial = CreateMaterial(new Color(0.34f, 0.33f, 0.30f, 1f), new Color(0.10f, 0.095f, 0.085f, 1f));
            stoneMaterial.name = "BD_Mat_Procedural_Stone";
            return stoneMaterial;
        }

        private static Material GetEarthMaterial()
        {
            if (earthMaterial != null)
                return earthMaterial;

            earthMaterial = CreateMaterial(new Color(0.37f, 0.25f, 0.16f, 1f), new Color(0.10f, 0.065f, 0.035f, 1f));
            earthMaterial.name = "BD_Mat_Procedural_Earth";
            return earthMaterial;
        }

        private static Material GetGrassMaterial()
        {
            if (grassMaterial != null)
                return grassMaterial;

            grassMaterial = CreateMaterial(new Color(0.16f, 0.34f, 0.16f, 1f), new Color(0.035f, 0.10f, 0.035f, 1f));
            grassMaterial.name = "BD_Mat_Procedural_Grass";
            return grassMaterial;
        }

        private static Material GetDryMaterial()
        {
            if (dryMaterial != null)
                return dryMaterial;

            dryMaterial = CreateMaterial(new Color(0.47f, 0.38f, 0.22f, 1f), new Color(0.12f, 0.085f, 0.04f, 1f));
            dryMaterial.name = "BD_Mat_Procedural_DryEarth";
            return dryMaterial;
        }

        private static Material CreateMaterial(Color baseColor, Color emission)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            if (shader == null) shader = Shader.Find("Hidden/InternalErrorShader");

            if (shader == null)
                return null;

            Material material = new Material(shader);
            material.color = baseColor;

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", baseColor);

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", baseColor);

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", emission);
            }

            return material;
        }
    }
}
