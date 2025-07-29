using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFw
{
    public static class SpriteRendererExtentions
    {
        /// <summary>
        /// 不透明度を設定する
        /// </summary>
        /// <param name="image">this</param>
        /// <param name="alpha">不透明度。0=透明 1=不透明</param>
        public static void SetOpacity(this SpriteRenderer sprite, float alpha)
        {
            var c = sprite.color;
            sprite.color = new Color(c.r, c.g, c.b, alpha);
        }

        /// <summary>
        /// 不透明度取得
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public static float GetOpacity(this SpriteRenderer sprite)
        {
            return sprite.color.a;
        }
    }
}