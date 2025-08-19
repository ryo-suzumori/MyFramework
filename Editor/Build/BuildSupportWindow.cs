using UnityEditor;
using UnityEngine;
using System.Linq;

namespace MyFw
{
    public class BuildSupportWindow : EditorWindow
    {
        private int versionCode;
        private string version;
        private string productName;

        [MenuItem(EditorConstants.MFToolsRootMenuItem + "/Build Support")]
        public static void ShowWindow()
        {
            GetWindow<BuildSupportWindow>("Build Support");
        }

        private void OnEnable()
        {
            versionCode = GetVersionCode();
            version = PlayerSettings.bundleVersion;
            productName = PlayerSettings.productName;
        }

        private void OnGUI()
        {
            using (var h = new GUILayout.HorizontalScope())
            {
                version = EditorGUILayout.TextField("Version 更新", version);
                if (GUILayout.Button("更新", GUILayout.Width(60)))
                {
                    SetVersion(version);
                    EditorUtility.DisplayDialog("Version", $"Version を {version} に更新しました", "OK");
                }
            }
            using (var h = new GUILayout.HorizontalScope())
            {
                GUILayout.Label($"VersionCode: {versionCode}");
                if (GUILayout.Button("インクリメント"))
                {
                    versionCode++;
                    SetVersionCode(versionCode);
                    EditorUtility.DisplayDialog("VersionCode", $"VersionCode を {versionCode} に更新しました", "OK");
                }
            }

            GUILayout.Space(10);
            GUILayout.Label("プラットフォーム切替", EditorStyles.boldLabel);
            GUILayout.Label($"現在のプラットフォーム: {EditorUserBuildSettings.activeBuildTarget}", EditorStyles.boldLabel);
            string nextPlatformLabel = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ? "iOSに切り替え" : "Androidに切り替え";
            if (GUILayout.Button(nextPlatformLabel))
            {
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                {
                    if (EditorUtility.DisplayDialog("プラットフォーム切替", "iOSに切り替えますか？", "OK", "キャンセル"))
                    {
                        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
                    }
                }
                else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
                {
                    if (EditorUtility.DisplayDialog("プラットフォーム切替", "Androidに切り替えますか？", "OK", "キャンセル"))
                    {
                        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                    }
                }
            }
            GUILayout.Space(10);
            if (GUILayout.Button("PlayerSettingsを開く"))
            {
                SettingsService.OpenProjectSettings("Project/Player");
            }


            GUILayout.Space(20);
            GUILayout.Label("ビルドショートカット", EditorStyles.boldLabel);

            if (GUILayout.Button("ローカルテストビルド"))
            {
                Build($"{productName}_dev_{versionCode}", true, false);
            }
            if (GUILayout.Button("ストアテストビルド"))
            {
                Build($"{productName}_stg_{versionCode}", true, true);
            }
            if (GUILayout.Button("リリースビルド"))
            {
                Build($"{productName}_rel_{versionCode}", false, true);
            }
        }

        private int GetVersionCode()
        {
            int androidCode = PlayerSettings.Android.bundleVersionCode;
            int iosCode = 0;
            int.TryParse(PlayerSettings.iOS.buildNumber, out iosCode);
            return Mathf.Max(androidCode, iosCode);
        }

        private void SetVersionCode(int code)
        {
            PlayerSettings.Android.bundleVersionCode = code;
            PlayerSettings.iOS.buildNumber = code.ToString();
            AssetDatabase.SaveAssets();
        }

        private void SetVersion(string ver)
        {
            PlayerSettings.bundleVersion = ver;
            AssetDatabase.SaveAssets();
        }

        private void Build(string fileName, bool isDevelopBuild, bool isStoreBuild)
        {
            
            BuildOptions options = BuildOptions.None;
            if (isDevelopBuild)
            {
                options |= BuildOptions.Development;
            }

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                if (isStoreBuild)
                {
                    EditorUserBuildSettings.buildAppBundle = true;
                    string aabPath = $"Builds/{fileName}.aab";
                    var aabReport = BuildPipeline.BuildPlayer(GetScenes(), aabPath, BuildTarget.Android, options);
                    if (aabReport.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                    {
                        EditorUtility.DisplayDialog("ビルド完了", $"Android AABビルド完了: {aabPath}", "OK");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("エラー", $"Android AABビルド失敗: {aabReport.summary.result}", "OK");
                    }
                }
                else
                {
                    EditorUserBuildSettings.buildAppBundle = false;
                    string apkPath = $"Builds/{fileName}.apk";
                    var apkReport = BuildPipeline.BuildPlayer(GetScenes(), apkPath, BuildTarget.Android, options);
                    if (apkReport.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                    {
                        EditorUtility.DisplayDialog("ビルド完了", $"Android APKビルド完了: {apkPath}", "OK");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("エラー", $"Android APKビルド失敗: {apkReport.summary.result}", "OK");
                    }
                }
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                string iosPath = $"Builds/{fileName}_iOS";
                var report = BuildPipeline.BuildPlayer(GetScenes(), iosPath, BuildTarget.iOS, options);
                if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                {
                    EditorUtility.DisplayDialog("ビルド完了", $"iOSビルド完了: {iosPath}", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("エラー", $"iOSビルド失敗: {report.summary.result}", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("エラー", "AndroidまたはiOSプラットフォームを選択してください", "OK");
            }
        }

        private static string[] GetScenes()
        {
            return EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();
        }
    }
}