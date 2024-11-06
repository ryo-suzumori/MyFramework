using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;


namespace MyFw
{
    public class EditorMasterSyncWindow : EditorWindow
    {
        private EditorMasterSyncConfig config;

        [MenuItem("MFTools/MasterSync", false, 1)]
        private static void ShowWindow()
        {
            var window = GetWindow<EditorMasterSyncWindow>();
            window.titleContent = new GUIContent("MasterSync Window");

            var path = "Assets/Editor/EditorMasterSyncConfig.asset";
            window.config = AssetDatabase.LoadAssetAtPath<EditorMasterSyncConfig>(path);

            if (window.config == null)
            {
                window.config = CreateInstance<EditorMasterSyncConfig>();
                AssetDatabase.CreateAsset(window.config, path);
            }
        }

        private void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            this.config.sheetId = EditorGUILayout.DelayedTextField("SpreadSheetId", this.config.sheetId);
            if (GUILayout.Button("Add Sheet"))
            {
                this.config.sheetNameList.Add("");
            }

            for (var index = 0; index < this.config.sheetNameList.Count; ++index)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("-"))
                    {
                        this.config.sheetNameList.RemoveAt(index);
                        continue;
                    }
                    GUILayout.Label($"┗━ {index} : ");
                    this.config.sheetNameList[index] = EditorGUILayout.DelayedTextField(this.config.sheetNameList[index]);
                    GUILayout.FlexibleSpace();
                }
            }
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));

            this.config.outputDir = EditorGUILayout.DelayedTextField("OutputDirectory", this.config.outputDir);

            if (GUILayout.Button("Run Download Csv"))
            {
                RunSyncAll().Forget();
            }

            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));

            this.config.nameSpace = EditorGUILayout.DelayedTextField("Namespace", this.config.nameSpace);
            this.config.scriptDir = EditorGUILayout.DelayedTextField("ScriptDirectory", this.config.scriptDir);

            if (GUILayout.Button("Run Master Script Build"))
            {
                var builder = new MasterClassBuilder();
                builder.Build(this.config);
            }

            // 変更を検知した場合、設定ファイルに戻す
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this.config, "Edit MasterSync Window");
                EditorUtility.SetDirty(this.config);
            }
        }

        private async UniTaskVoid RunSyncAll()
        {
            Debug.Log("===== Start Sync =====");

            if (!Directory.Exists(this.config.outputDir))
            {
                Directory.CreateDirectory(this.config.outputDir);
            }

            for (var i = 0; i < this.config.sheetNameList.Count; ++i)
            {
                await RunSync(this.config.sheetId, this.config.outputDir, this.config.sheetNameList[i]);
            }
            AssetDatabase.Refresh();

            Debug.Log("===== End Sync =====");
        }

        private async UniTask RunSync(string sheetId, string outputDir, string sheetName)
        {
            var url = $"https://docs.google.com/spreadsheets/d/{sheetId}/gviz/tq?tqx=out:csv&sheet={sheetName}";
            var request = UnityWebRequest.Get(url);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"{sheetName} : {request.error}");
            }
            else
            {
                Debug.Log($"{sheetName} : sync");
                using var sw = new StreamWriter($"{outputDir}/{sheetName}.csv", false, Encoding.UTF8);
                sw.Write(request.downloadHandler.text);
            }
        }
    }
}
