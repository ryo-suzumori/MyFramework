using UnityEngine;
using UnityEngine.UI;

namespace MyFw
{
    // =====================================================
    // 2. 色適用インターフェース
    // =====================================================
    public interface IMaterialColorApplicable
    {
        void ApplyColor(Color color);
        Color GetCurrentColor();
        string GetComponentName();
    }

    // =====================================================
    // 3. コンポーネントアダプター
    // =====================================================
    public class ImageColorAdapter : IMaterialColorApplicable
    {
        private readonly Image image;

        public ImageColorAdapter(Image image)
        {
            this.image = image;
        }

        public void ApplyColor(Color color)
        {
            if (image != null)
                image.color = color;
        }

        public Color GetCurrentColor()
        {
            return image != null ? image.color : Color.white;
        }

        public string GetComponentName()
        {
            return "Image";
        }
    }

    public class SpriteRendererColorAdapter : IMaterialColorApplicable
    {
        private readonly SpriteRenderer spriteRenderer;

        public SpriteRendererColorAdapter(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
        }

        public void ApplyColor(Color color)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = color;
        }

        public Color GetCurrentColor()
        {
            return spriteRenderer != null ? spriteRenderer.color : Color.white;
        }

        public string GetComponentName()
        {
            return "SpriteRenderer";
        }
    }
}
