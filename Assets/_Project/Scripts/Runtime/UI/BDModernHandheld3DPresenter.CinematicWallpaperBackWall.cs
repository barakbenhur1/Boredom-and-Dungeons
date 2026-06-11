using UnityEngine;

namespace BoredomAndDungeons
{
    public sealed partial class BDModernHandheld3DPresenter
    {
        private const string CinematicWallpaperResourcePath =
            "ModernHandheld/Textures/BDCinematicKitchenWallpaper";

        private void BuildExactCinematicBackWallWallpaper()
        {
            Material wallpaperMaterial =
                CreateExactCinematicBackWallWallpaperMaterial();

            CreateCinematicCube(
                "Cinematic Exact Fruit Wallpaper Back Wall",
                tableRoot,
                new Vector3(0f, 2.20f, 26f),
                new Vector3(120f, 36f, 0.10f),
                wallpaperMaterial,
                castShadows: false,
                receiveShadows: true
            );

            Material baseboardMaterial = CreateCinematicWoodMaterial(
                "BD Cinematic Warm Wallpaper Baseboard",
                new Color(0.46f, 0.33f, 0.23f, 1f),
                0.12f
            );
            CreateCinematicCube(
                "Cinematic Wallpaper Baseboard",
                tableRoot,
                new Vector3(
                    0f,
                    CinematicFloorSurfaceY + 0.52f,
                    25.84f
                ),
                new Vector3(120f, 1.04f, 0.24f),
                baseboardMaterial,
                castShadows: false,
                receiveShadows: true
            );

            CreateCinematicCube(
                "Cinematic Exact Fruit Wallpaper Right Wall",
                tableRoot,
                new Vector3(22.50f, 2.20f, -6.00f),
                new Vector3(0.12f, 36f, 64f),
                wallpaperMaterial,
                castShadows: false,
                receiveShadows: true
            );
            CreateCinematicCube(
                "Cinematic Right Wall Baseboard",
                tableRoot,
                new Vector3(
                    22.38f,
                    CinematicFloorSurfaceY + 0.52f,
                    -6.00f
                ),
                new Vector3(0.24f, 1.04f, 64f),
                baseboardMaterial,
                castShadows: false,
                receiveShadows: true
            );

            Material ceilingMaterial = CreateCinematicRoomShellMaterial(
                "BD Cinematic Warm Kitchen Ceiling",
                new Color(0.82f, 0.78f, 0.70f, 1f),
                0.035f
            );
            CreateCinematicCube(
                "Cinematic Kitchen Ceiling",
                tableRoot,
                new Vector3(0f, 20.30f, -6.00f),
                new Vector3(45f, 0.20f, 64f),
                ceilingMaterial,
                castShadows: false,
                receiveShadows: true
            );
        }

        private Material CreateCinematicRoomShellMaterial(
            string materialName,
            Color color,
            float smoothness)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
            {
                shader = Shader.Find(
                    "BoredomAndDungeons/ModernHandheldSurface"
                );
            }

            Material material = CreateMaterial(shader);
            material.name = materialName;
            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);
            if (material.HasProperty("_Metallic"))
                material.SetFloat("_Metallic", 0f);
            if (material.HasProperty("_Glossiness"))
                material.SetFloat("_Glossiness", smoothness);
            return material;
        }

        private Material CreateExactCinematicBackWallWallpaperMaterial()
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
            {
                shader = Shader.Find(
                    "BoredomAndDungeons/ModernHandheldSurface"
                );
            }

            Material material = CreateMaterial(shader);
            material.name = "BD Cinematic Exact Fruit Wallpaper";

            Texture2D wallpaper = Resources.Load<Texture2D>(
                CinematicWallpaperResourcePath
            );
            if (wallpaper != null)
            {
                material.mainTexture = wallpaper;
                material.mainTextureScale = new Vector2(8.6f, 4.8f);
                material.mainTextureOffset = Vector2.zero;
            }

            if (material.HasProperty("_Color"))
                material.SetColor("_Color", Color.white);
            if (material.HasProperty("_Metallic"))
                material.SetFloat("_Metallic", 0f);
            if (material.HasProperty("_Glossiness"))
                material.SetFloat("_Glossiness", 0.035f);

            return material;
        }
    }
}
