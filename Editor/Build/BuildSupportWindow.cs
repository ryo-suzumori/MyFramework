using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;
using System.IO;

namespace MyFw
{
    public class BuildSupportWindow : EditorWindow
    {
        private int versionCode;
        private string version;
        private string productName;
        private KeystoreInfo keystoreInfo;

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
            keystoreInfo = BuildConfigLoader.LoadKeystoreConfigByProjectName();
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

            if (GUILayout.Button("ローカル開発ビルド＆実行"))
            {
                Build($"{productName}_dev_{versionCode}", true, false, true);
            }
            if (GUILayout.Button("ログ有効版リリースビルド"))
            {
                Build($"{productName}_stg_{versionCode}", true, true, true);
            }
            if (GUILayout.Button("リリースビルド"))
            {
                Build($"{productName}_rel_{versionCode}", false, true, false);
            }

            GUILayout.Space(20);
            DrawStatusLine("Build config file", BuildConfigLoader.ConfigExists);
            DrawStatusLine("Keystore info from build config", keystoreInfo != null);

            var keystoreExist = keystoreInfo != null && File.Exists(keystoreInfo.keystorePath);
            DrawStatusLine("Keystore file exist", keystoreExist);
            if (!keystoreExist)
            {
                EditorGUILayout.HelpBox($"こちらのパスを参照しましたが見つかりませんでした。[{keystoreInfo?.keystorePath}]", MessageType.Error);
            }
            if (GUILayout.Button("Reload Build Config"))
            {
                keystoreInfo = BuildConfigLoader.LoadKeystoreConfigByProjectName();
                EditorUtility.DisplayDialog("Keystore情報", "Keystore情報をリロードしました", "OK");
            }
        }

        private int GetVersionCode()
        {
            int androidCode = PlayerSettings.Android.bundleVersionCode;
            int.TryParse(PlayerSettings.iOS.buildNumber, out int iosCode);
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

        private void Build(string fileName, bool isBuildAndRun, bool isStoreBuild, bool useForceLog)
        {
            // 元のバンドルIDを保存
            var originalBundleId = PlayerSettings.applicationIdentifier;
            var currentTarget = NamedBuildTarget.FromBuildTargetGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup
            );

            try
            {
                // バンドルIDを一時的に変更
                if (isBuildAndRun)
                {
                    PlayerSettings.SetApplicationIdentifier(currentTarget, originalBundleId + ".dev");
                }
                
                BuildOptions options = BuildOptions.None;
                if (isBuildAndRun)
                {
                    options |= BuildOptions.Development;
                    options |= BuildOptions.AutoRunPlayer;
                }

                // 強制ログ有効フラグの設定
                var defineEditor = new ScriptingDefineSymbolsEditor(currentTarget);
                defineEditor.Register("USE_FORCE_LOG", useForceLog);
                defineEditor.Apply();

                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                {
                    SetKeystoreFromEnvironment();

                    if (isStoreBuild)
                    {
                        EditorUserBuildSettings.buildAppBundle = true;
                        string aabPath = $"Builds/{fileName}.aab";
                        var aabReport = BuildPipeline.BuildPlayer(GetScenes(), aabPath, BuildTarget.Android, options);
                        if (aabReport.summary.result == BuildResult.Succeeded)
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
                        if (apkReport.summary.result == BuildResult.Succeeded)
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
                    if (report.summary.result == BuildResult.Succeeded)
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
            finally
            {
                // 必ず元のバンドルIDに戻す
                PlayerSettings.SetApplicationIdentifier(currentTarget, originalBundleId);
                AssetDatabase.SaveAssets();
            }
        }

        private static string[] GetScenes()
        {
            return EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();
        }

        private void SetKeystoreFromEnvironment()
        {
            if (keystoreInfo == null)
            {
                Debug.LogWarning("Keystore information is not available. Skipping keystore setup.");
                return;
            }

            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = keystoreInfo.keystorePath;
            PlayerSettings.Android.keystorePass = keystoreInfo.keystorePass;
            PlayerSettings.Android.keyaliasName = keystoreInfo.keyaliasName;
            PlayerSettings.Android.keyaliasPass = keystoreInfo.keyaliasPass;
        }
        private void DrawStatusLine(string label, bool isValid)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(EditorGUIUtility.IconContent(isValid ? "TestPassed" : "TestFailed"), GUILayout.Width(20));
                GUILayout.Label(label);
            }
        }
    }
}