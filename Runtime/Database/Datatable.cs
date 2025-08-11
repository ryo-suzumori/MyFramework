using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions;


namespace MyFw.DS
{
    /// <summary>
    /// データテーブルインターフェース.
    /// </summary>
    public interface IDatatable
    {
#if DEBUG
        public void ReloadDatabase();
#endif
    }

    /// <summary>
    /// 行データインターフェース.
    /// </summary>
    public interface IDataRow
    {
    }

    /// <summary>
    /// データテーブルクラス.
    ///
    /// // １行分データを定義 プロパティーでカラムを登録する.
    /// public class SampleDataRow : IDataRow
    /// {
    ///     public int Id { get; set; }
    ///     public int Value1 { get; set; }
    ///     public int Value2 { get; set; }
    /// }
    /// 
    /// // SampleClass、 ISmapleAccessor で公開する.
    /// public class SampleClass : Datatable<SampleDataRow>, ISmapleAccessor
    /// {
    ///     // CSVファイルパスを設定 Resources直下からのパス.
    ///     protected override string CsvPath => "Database/m_test";
    ///
    ///     // データ検索関数を定義.
    ///     public SampleDataRow GetByID(int id)
    ///         => this.dataList.FirstOrDefault(row => row.Id == id);
    /// }
    /// </summary>
    /// <typeparam name="DataStructure">１行分のデータクラス</typeparam>
    public abstract class Datatable<DataStructure> : IDatatable
        where DataStructure : IDataRow, new()
    { 
        /// <summary>
        /// 読み込みcsvパス.
        /// 継承先でオーバーライドすることでファイル名を設定する.
        /// </summary>
        protected abstract string CsvPath { get; }

        /// <summary>
        /// CSVファイル名.
        /// </summary>
        public string CsvName => CsvPath.Substring(this.CsvPath.LastIndexOf("/", StringComparison.Ordinal)+1);

        /// <summary>
        /// 読込済みデータ本体.
        /// </summary>
        protected readonly List<DataStructure> dataList = new();

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        protected Datatable()
        {
            LoadDatabaseFromCSV();
        }

        /// <summary>
        /// データベース読み込み.
        /// </summary>
        protected void LoadDatabaseFromCSV()
        {
            LogUtil.Log($"load from {this.CsvName}");
            var textAsset = LoadCSV();
            Assert.IsNotNull(textAsset, $"{this.CsvPath} is not found!!");

            var textData = textAsset.text.Replace("\"", "");
            var splitDataList = StringUtility.SplitFromCSVText(textData);
            Assert.IsNotNull(splitDataList, $"{this.CsvPath} is not CSV format!!");

            var datatable = new DataTableContext();
            datatable.SetHeader(splitDataList[0]);

            var rowIdx = 0;
            foreach(var rowData in splitDataList.Skip(1))
            {
                var dataStructure = new DataStructure();
                var columnIdx = 0;
                foreach(var column in datatable.columnContexts)
                {
                    if (string.IsNullOrEmpty(column.name))
                    {
                        ++columnIdx;
                        continue;
                    }

                    var field = typeof(DataStructure).GetField(column.FieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                    switch (column.typeName)
                    {
                        case "IPercent":
                            field.SetValue(dataStructure, new Percent(Int32.TryParse(rowData[columnIdx], out var retPercent) ? retPercent : 0));
                            break;
                        case "String":
                            field.SetValue(dataStructure, rowData[columnIdx]);
                            break;
                        case "Boolean":
                            field.SetValue(dataStructure, Boolean.TryParse(rowData[columnIdx], out var retBoolean) ? retBoolean : false);
                            break;
                        case "Color":
                            field.SetValue(dataStructure, ColorUtility.TryParseHtmlString("#"+rowData[columnIdx], out var retColor) ? retColor : Color.white);
                            break;
                        case "UInt32":
                            field.SetValue(dataStructure, UInt32.TryParse(rowData[columnIdx], out var retUInt32) ? retUInt32 : 0);
                            break;
                        case "Int32":
                        default:
                            field.SetValue(dataStructure, Int32.TryParse(rowData[columnIdx], out var retOther) ? retOther : 0);
                            break;
                    }
                    ++columnIdx;
                }
                this.dataList.Add(dataStructure);
                ++rowIdx;
            }
        }

        /// <summary>
        /// csvファイル読み込み.
        /// </summary>
        /// <returns>文字列データ</returns>
        private TextAsset LoadCSV()
        {
            return Resources.Load(this.CsvPath) as TextAsset;
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public void DumpTable()
        {
            LogUtil.Log($"Dump {this.CsvName} Table");
            foreach (var data in this.dataList)
            {
                var properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var values = properties.Select(p => p.GetValue(data, null)).ToArray();
                LogUtil.Log(string.Join(", ", values));
            }
        }

#if DEBUG
        public void ReloadDatabase()
        {
            this.dataList.Clear();
            LoadDatabaseFromCSV();
        }
#endif
    }
}
