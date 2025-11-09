using UnityEditor;
using UnityEditor.Build;
using System.Collections.Generic;
using System.Linq;

namespace MyFw
{
    public class ScriptingDefineSymbolsEditor
    {
        private readonly NamedBuildTarget buildTarget;
        private readonly HashSet<string> currentSymbols;
        private readonly Dictionary<string, bool> registeredSymbols = new();

        public ScriptingDefineSymbolsEditor(NamedBuildTarget buildTarget)
        {
            this.buildTarget = buildTarget;
            this.currentSymbols = PlayerSettings.GetScriptingDefineSymbols(buildTarget)
                .Split(';')
                .Where(s => !string.IsNullOrEmpty(s))
                .ToHashSet();
        }

        public void Register(string symbolName, bool enables)
        {
            registeredSymbols[symbolName] = enables;
        }

        public void Apply()
        {
            // 登録されたシンボルの処理
            foreach (var kvp in registeredSymbols)
            {
                if (kvp.Value)
                {
                    // 有効化：追加
                    currentSymbols.Add(kvp.Key);
                }
                else
                {
                    // 無効化：削除
                    currentSymbols.Remove(kvp.Key);
                }
            }

            // 設定を適用
            var newDefines = string.Join(";", currentSymbols);
            PlayerSettings.SetScriptingDefineSymbols(buildTarget, newDefines);

            LogUtil.Log($"Applied Scripting Define Symbols for {buildTarget}: {newDefines}");
        }
    }
}