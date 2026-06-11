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
                new Vector3(0f, 1.50f, 23.82f),
                new Vector3(96f, 33f, 0.08f),
                wallpaperMaterial,
                castShadows: false,
                receiveShadows: true
            );
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
                material.mainTextureScale = new Vector2(7.5f, 4.2f);
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
