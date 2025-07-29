using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// public class SmapleDataRow : IDataRow
    /// {
    ///     public int Id { get; set; }
    ///     public int Value1 { get; set; }
    ///     public int Value2 { get; set; }
    /// }
    /// 
    /// // SmapleClass、 ISmapleAccessor で公開する.
    /// public class SmapleClass : Datatable<SmapleDataRow>, ISmapleAccessor
    /// {
    ///     // CSVファイルパスを設定 Resources直下からのパス.
    ///     protected override string CsvPath => "Database/m_test";
    ///
    ///     // データ検索関数を定義.
    ///     public SmapleDataRow GetByID(int id)
    ///         => this.dataList.FirstOrDefault(row => row.Id == id);
    /// }
    /// </summary>
    /// <typeparam name="DataStructre">１行分のデータクラス</typeparam>
    public abstract class Datatable<DataStructre> : IDatatable
        where DataStructre : IDataRow, new()
    { 
        /// <summary>
        /// 読み込みcsvパス.
        /// 継承先でオーバーライドすることでファイル名を設定する.
        /// </summary>
        protected abstract string CsvPath { get; }

        /// <summary>
        /// CSVファイル名.
        /// </summary>
        public string CsvName => CsvPath.Substring(this.CsvPath.LastIndexOf("/", StringComparison.Ordinal)+1, this.CsvPath.Length - this.CsvPath.LastIndexOf("/", StringComparison.Ordinal)+1);

        /// <summary>
        /// 読込済みデータ本体.
        /// </summary>
        protected readonly List<DataStructre> dataList = new();

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
            Debug.Log($"load to {this.CsvPath}");
            var textAsset = LoadCSV();
            Assert.IsNotNull(textAsset, $"{this.CsvPath} is not found!!");

            var textData = textAsset.text.Replace("\"", "");
            var splitedDataList = StringUtility.SplitFromCSVText(textData);
            Assert.IsNotNull(splitedDataList, $"{this.CsvPath} is not CSV format!!");

            var datatable = new DataTableContext();
            datatable.SetHeader(splitedDataList[0]);

            var rowIdx = 0;
            foreach(var rowData in splitedDataList.Skip(1))
            {
                var dataStructre = new DataStructre();
                var colmunIdx = 0;
                foreach(var colmun in datatable.colmunContexts)
                {
                    if (string.IsNullOrEmpty(colmun.name))
                    {
                        ++colmunIdx;
                        continue;
                    }

                    var feild = typeof(DataStructre).GetField(colmun.FieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                    switch (colmun.typeName)
                    {
                        case "IPercent":
                            feild.SetValue(dataStructre, new Percent(Int32.TryParse(rowData[colmunIdx], out var retPercent) ? retPercent : 0));
                            break;
                        case "String":
                            feild.SetValue(dataStructre, rowData[colmunIdx]);
                            break;
                        case "Boolean":
                            feild.SetValue(dataStructre, Boolean.TryParse(rowData[colmunIdx], out var retBoolean) ? retBoolean : false);
                            break;
                        case "UInt32":
                            feild.SetValue(dataStructre, UInt32.TryParse(rowData[colmunIdx], out var retUInt32) ? retUInt32 : 0);
                            break;
                        case "Int32":
                        default:
                            feild.SetValue(dataStructre, Int32.TryParse(rowData[colmunIdx], out var retOther) ? retOther : 0);
                            break;
                    }
                    ++colmunIdx;
                }
                this.dataList.Add(dataStructre);
                ++rowIdx;
            }
        }

        /// <summary>
        /// 整合性チェック.
        /// </summary>
        /// <param name="splitedDataList"></param>
        /// <param name="propertieList"></param>
        private void Validation(IList<string[]> splitedDataList, PropertyInfo[] propertieList)
        {
            if (splitedDataList[0].Length != propertieList.Length)
            {
                throw new Exception($"colmun count is not match!! [{this.CsvPath}] csv:{splitedDataList[0].Length} class:{propertieList.Length}");
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

#if DEBUG
        public void ReloadDatabase()
        {
            this.dataList.Clear();
            LoadDatabaseFromCSV();
        }
#endif
    }
}
