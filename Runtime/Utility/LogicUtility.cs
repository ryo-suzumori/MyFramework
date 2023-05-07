
using System.Collections.Generic;
using System.Linq;

namespace MyFw
{
    /// <summary>
    /// 確率インターフェース.
    /// </summary>
    public interface IProbability
    {
        int Probability { get; }
    }

    /// <summary>
    /// ロジックユーティリティ
    /// </summary>
    public static class LogicUtility
    {
        /// <summary>
        /// 確率を持ったリストデータから抽選で一つピックする.
        /// </summary>
        /// <param name="classList">確率リスト</param>
        /// <returns>ピックされた１データ</returns>
        public static Class CalcLottery<Class>(IEnumerable<Class> classList)
            where Class : IProbability
        {
            if (!classList.Any())
            {
                return default;
            }

            if (classList.Count() == 1)
            {
                return classList.First();
            }

            var total = classList.Sum(c => c.Probability);
            var sorted = classList.OrderBy(c => c.Probability);
            var value = UnityEngine.Random.Range(0, total);

            foreach (var c in sorted)
            {
                if (c.Probability >= value)
                {
                    return c;
                }
                value -= c.Probability;
            }

            return sorted.Last();
        }
    }
}
