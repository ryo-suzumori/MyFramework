using System.IO;
using System.Collections.Generic;

namespace MyFw
{
    /// <summary>
    /// 文字列ユーティリティ
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// CSVフォーマットの文字列を分解する
        /// </summary>
        /// <param name="text">分割する文字列データ</param>
        /// <returns>分割後の文字列</returns>
        public static IList<string[]> SplitFromCSVText(string text)
        {
            var result = new List<string[]>();
            var reader = new StringReader(text);
            while (reader.Peek() != -1) // reader.Peaekが-1になるまで
            {
                string line = reader.ReadLine(); // 一行ずつ読み込み
                result.Add(line.Split(',')); // , 区切りでリストに追加
            }
            return result;
        }
    }
}