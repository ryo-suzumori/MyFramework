using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
            LoadDatabase();
        }

        /// <summary>
        /// データベース読み込み.
        /// </summary>
        protected void LoadDatabase()
        {
            Debug.Log($"load to {this.CsvPath}");
            var textAsset = LoadCSV();
            Assert.IsNotNull(textAsset, $"{this.CsvPath} is not found!!");

            var textData = Regex.Replace(textAsset.text, "^#.*\n", "");
            var splitedDataList = StringUtility.SplitFromCSVText(textData);
            Assert.IsNotNull(splitedDataList, $"{this.CsvPath} is not CSV format!!");

            var propertieList= typeof(DataStructre).GetProperties();

            //try
            //{
#if DEBUG
                Validation(splitedDataList, propertieList);
#endif
                foreach (var splitedData in splitedDataList)
                {
                    var dataStructre = new DataStructre();
                    var colmunIndex = 0;
                    foreach (var info in propertieList)
                    {
                        // Debug.Log(info.Name + " code:" + Type.GetTypeCode(info.GetMethod.ReturnType) + " value:" + splitedData[colmunIndex]);
                        switch (Type.GetTypeCode(info.GetMethod.ReturnType))
                        {
                            case TypeCode.Int32:
                                info.SetValue(dataStructre, int.TryParse(splitedData[colmunIndex], out var tempValue) ? tempValue : 0);
                                break;
                            case TypeCode.String:
                                info.SetValue(dataStructre, splitedData[colmunIndex]);
                                break;
                        }
                        ++colmunIndex;
                    }

                    this.dataList.Add(dataStructre);
                }
            //}
            //catch(Exception e)
            //{
            //    Debug.LogError(e.Message);
            //}
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
                throw new Exception($"colmun count is not match!! [{this.CsvName}] csv:{splitedDataList[0].Length} class:{propertieList.Length}");
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
            LoadDatabase();
        }
#endif
    }
}
