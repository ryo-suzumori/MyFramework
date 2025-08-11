using UnityEngine;
using UnityEngine.UI;

namespace MyFw
{
    public interface IMaterialColorApplicable
    {
        void ApplyColor(Color color);
        Color GetCurrentColor();
        string GetComponentName();
    }

    public class ColorSettableAdapter : IMaterialColorApplicable
    {
        private readonly IColorSettable settable;

        public ColorSettableAdapter(IColorSettable settable)
        {
            this.settable = settable;
        }

        public void ApplyColor(Color color)
        {
            if (settable != null)
            {
                settable.Color = new Color(color.r, color.g, color.b, settable.Color.a);
            }
        }

        public Color GetCurrentColor() => settable != null ? settable.Color : Color.white;
        public string GetComponentName() => "ColorSettable";
    }

    public class GraphicColorAdapter : IMaterialColorApplicable
    {
        private readonly Graphic graphic;

        public GraphicColorAdapter(Graphic graphic)
        {
            this.graphic = graphic;
        }

        public void ApplyColor(Color color)
        {
            if (graphic != null)
            {
                graphic.color = new Color(color.r, color.g, color.b, graphic.color.a);
            }
        }

        public Color GetCurrentColor() => graphic != null ? graphic.color : Color.white;
        public string GetComponentName() => "Graphic";
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
            {
                spriteRenderer.color = color;
            }
        }

        public Color GetCurrentColor() => spriteRenderer != null ? spriteRenderer.color : Color.white;
        public string GetComponentName() => "SpriteRenderer";
    }
}
