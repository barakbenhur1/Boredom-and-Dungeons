using UnityEngine;

namespace BoredomAndDungeons
{
    public enum BDWallTextureFacing
    {
        AlongWorldX,
        AlongWorldZ,
        Freeform
    }

    [DisallowMultipleComponent]
    public sealed class BDWallSurfaceProfile : MonoBehaviour
    {
        // BD ASYMMETRIC WALL TEXTURE READINESS V7
        [SerializeField] private bool preserveAsymmetricTextureOrientation = true;
        [SerializeField] private bool allowTextureMirroring = false;
        [SerializeField] private BDWallTextureFacing textureFacing =
            BDWallTextureFacing.Freeform;
        [SerializeField, Range(0, 3)] private int textureQuarterTurns;
        [SerializeField] private Vector2 uvScale = Vector2.one;
        [SerializeField] private Vector2 uvOffset = Vector2.zero;
        [SerializeField] private float minimumOcclusionHeight = 22f;

        public bool PreserveAsymmetricTextureOrientation =>
            preserveAsymmetricTextureOrientation;
        public bool AllowTextureMirroring => allowTextureMirroring;
        public BDWallTextureFacing TextureFacing => textureFacing;
        public int TextureQuarterTurns => textureQuarterTurns;
        public Vector2 UvScale => uvScale;
        public Vector2 UvOffset => uvOffset;
        public float MinimumOcclusionHeight => minimumOcclusionHeight;

        public void ConfigureFromWorldBounds(
            Bounds worldBounds,
            float requiredHeight)
        {
            preserveAsymmetricTextureOrientation = true;
            allowTextureMirroring = false;
            minimumOcclusionHeight = Mathf.Max(1f, requiredHeight);

            textureFacing = worldBounds.size.x >= worldBounds.size.z
                ? BDWallTextureFacing.AlongWorldX
                : BDWallTextureFacing.AlongWorldZ;

            // Quarter-turn metadata is explicit so a future non-symmetric
            // texture or shader can orient each segment without negative scale.
            textureQuarterTurns =
                textureFacing == BDWallTextureFacing.AlongWorldX ? 0 : 1;

            uvScale = new Vector2(
                Mathf.Max(0.01f, Mathf.Abs(uvScale.x)),
                Mathf.Max(0.01f, Mathf.Abs(uvScale.y))
            );
        }
    }
}
