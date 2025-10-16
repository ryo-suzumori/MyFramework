using System.Collections.Generic;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace MyFw
{
    /// <summary>
    /// 翻訳情報
    /// </summary>
    public class LocalizedTextSetting
    {
        public string LocalizeKey;
        public IDictionary<string, string> VariableMap;
        public LocalizedTextSetting(string LocalizeKey, IDictionary<string, string> VariableMap = null)
        {
            this.LocalizeKey = LocalizeKey;
            this.VariableMap = VariableMap;
        }
    }

    /// <summary>
    /// LocalizeStringEventを補助するヘルパークラス
    /// </summary>
    public static class LocalizeViewHelper
    {
        /// <summary>
        /// LocalizedStringに変数を登録します。
        /// </summary>
        /// <param name="localizedString">対象のLocalizedString</param>
        /// <param name="key">変数名（ローカライズテーブルのプレースホルダー名）</param>
        /// <param name="value">値</param>
        public static void RegisterVariable(LocalizeStringEvent localizeStringEvent, string key, string value)
        {
            // 既存変数がなければ追加
            if (!localizeStringEvent.StringReference.Keys.Contains(key))
            {
                localizeStringEvent.StringReference.Add(key, new StringVariable { Value = value ?? string.Empty });
            }
            else
            {
                if (localizeStringEvent.StringReference[key] is StringVariable strVar)
                {
                    strVar.Value = value ?? string.Empty;
                }
                else
                {
                    LogUtil.LogWarning($"RegisterVariable: 変数'{key}'はStringVariable型ではありません。値の更新に失敗しました。");
                }
            }
        }

        /// <summary>
        /// LocalizedStringに一括で設定する
        /// </summary>
        /// <param name="localizeStringEvent"></param>
        /// <param name="localizedTextSetting"></param>
        public static void RegisterVariable(LocalizeStringEvent localizeStringEvent, LocalizedTextSetting localizedTextSetting)
        {
            // 先に変数を紐付けないとエラーになる
            if (localizedTextSetting.VariableMap != null)
            {
                foreach (var variable in localizedTextSetting.VariableMap)
                {
                    RegisterVariable(localizeStringEvent, variable.Key, variable.Value);
                }
            }

            // キーを割り当てる
            SetLocalizeKeyWithRefresh(localizeStringEvent, localizedTextSetting.LocalizeKey);
        }

        /// <summary>
        /// ローカライズキーを設定、更新
        /// </summary>
        /// <param name="localizeStringEvent"></param>
        /// <param name="localizeKey"></param>
        public static void SetLocalizeKeyWithRefresh(LocalizeStringEvent localizeStringEvent, string localizeKey)
        {
            localizeStringEvent.StringReference.TableEntryReference = localizeKey;
            localizeStringEvent.RefreshString();
        }
    }
}