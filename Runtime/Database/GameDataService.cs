#if !DISABLE_SAVEDATA

using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using UnityEngine;

namespace MyFw.DS
{
    /// <summary>
    /// セーブ可能データ.
    /// </summary>
    /// <typeparam name="type"></typeparam>
    public interface ISavableData<type>
    {
        void DeepCopy(type data);
    }

    /// <summary>
    /// SaveDataAccessor
    /// </summary>
    public class GameDataService
    {
        public static string DataPath
        {
            get => QuickSaveGlobalSettings.StorageLocation;
            set => QuickSaveGlobalSettings.StorageLocation = value;
        }

        private static readonly QuickSaveSettings DataSettings;

        public static string GetFullPath(string fileName)
            => $"{DataPath}/QuickSave/{fileName}.json";

        /// <summary>
        /// セーブシステム設定
        /// </summary>
        public static void InitialzieSaveSetting()
        {
            var path = Application.persistentDataPath + "/password.txt";
            if (!FileAccess.Exists(path))
            {
                return;
            }

            DataSettings.SecurityMode = SecurityMode.Aes;
            DataSettings.CompressionMode = CompressionMode.Gzip;
            DataSettings.Password = System.IO.File.ReadAllText(path);
        }

        /// <summary>
        ///  ファイルからデータを読み込む
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static bool ReadDataFromJsonFile<T>(ref T data)
            where T : class, ISavableData<T>
        {
            var fileName = typeof(T).Name;
            if (!FileAccess.Exists($"{DataPath}/QuickSave/{fileName}.json"))
            {
                Debug.Log($"Save Data json none : {DataPath}/{fileName}");
                return false;
            }
            else
            {
                Debug.Log($"Load Save Data json : {DataPath}/{fileName}");
            }

            var reader = QuickSaveReader.Create(fileName);
            if (reader.TryRead<string>("jsonRoot", out var jsonData))
            {
                data.DeepCopy(JsonUtility.FromJson<T>(jsonData));
                return true;
            }
            return false;
        }

        /// <summary>
        ///  ファイルにデータを書き込む
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static void WriteDataToJsonFile<T>(in T data)
            where T : class
        {
            var fileName = typeof(T).Name;
            Debug.Log($"Write Save Data json : {DataPath}/{fileName}");

            var writer = QuickSaveWriter.Create(fileName);
            writer.Write("jsonRoot", JsonUtility.ToJson(data));
            writer.Commit();
        }

        /// <summary>
        /// セーブデータ削除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void DeleteData<T>()
            where T : class
        {
            var fileName = typeof(T).Name;
            FileAccess.Delete(GetFullPath(fileName));
        }
    }
}
#endif
