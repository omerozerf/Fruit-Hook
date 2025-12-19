using UnityEngine;

namespace ScratchCardAsset.Core.ScratchData
{
    public class MeshRendererData : BaseData
    {
        private readonly MeshRenderer renderer;
        private readonly MeshFilter filter;

        protected override Vector2 Bounds
        {
            get
            {
                // Prefer local-space mesh bounds via MeshFilter
                if (filter && filter.sharedMesh != null)
                    return (Vector2)filter.sharedMesh.bounds.size;

                // Fallback: world-space renderer bounds (should rarely be needed)
                if (renderer)
                    return (Vector2)renderer.bounds.size;

                return Vector2.zero;
            }
        }

        public override Vector2 TextureSize { get; }

        public MeshRendererData(Transform surface, Camera camera) : base(surface, camera)
        {
            if (surface.TryGetComponent(out renderer) && surface.TryGetComponent(out filter))
            {
                InitTriangle();
                var sharedMaterial = renderer.sharedMaterial;
                var offset = sharedMaterial.GetVector(Constants.MaskShader.Offset);
                TextureSize = new Vector2(sharedMaterial.mainTexture.width * offset.z, sharedMaterial.mainTexture.height * offset.w);
            }
        }
    }
}