using UnityEngine;

namespace MyFw
{
    public static class Vector2Extensions
    {
        /// <summary>
		/// 左回り（反時計回り）に90度回転
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
        public static Vector2 RotateLeft90(this Vector2 vector)
            => new (-vector.y, vector.x);

        /// <summary>
		/// 右回り（時計回り）に90度回転
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
        public static Vector2 RotateRight90(this Vector2 vector)
            => new (vector.y, -vector.x);

        /// <summary>
        /// ベクトルを角度（度数法）に変換
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float ToDegree(this Vector2 vector)
            => vector.ToRadian() * Mathf.Rad2Deg;

        /// <summary>
        /// ベクトルを角度（ラジアン）に変換 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float ToRadian(this Vector2 vector)
            => Mathf.Atan2(vector.y, vector.x);
    }
}