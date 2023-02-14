using UnityEngine;

namespace MyFw
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// レイヤー名を使用してレイヤーを設定します
        /// </summary>
        /// <param name="self">GmaeObject自身</param>
        /// <param name="layerName">レイヤー名</param>
        public static void SetLayer( this GameObject self, string layerName )
        {
            self.layer = LayerMask.NameToLayer( layerName );
        }

        /// <summary>
        /// レイヤー名を使用してレイヤーを設定します
        /// </summary>
        /// <param name="self">GmaeObject自身</param>
        /// <param name="layerName">レイヤー名</param>
        /// <returns>一致判定</returns>
        public static bool CompareToLayer( this GameObject self, string layerName )
        {
            return self.layer == LayerMask.NameToLayer( layerName );
        }
    }
}