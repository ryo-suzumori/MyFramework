using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;

namespace MyFw
{

    [Serializable]
    public class KeystoreInfo
    {
        public string keystorePath;
        public string keystorePass;
        public string keyaliasName;
        public string keyaliasPass;
    }

    [Serializable]
    public class AndroidConfigRoot
    {
        public Dictionary<string, KeystoreInfo> keystores;
    }

    public class BuildConfigLoader
    {
        private static string ConfigDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".AppDev",
            "android"
        );

        private static string ConfigPath => Path.Combine(ConfigDirectory, "config.json");

        public static bool ConfigExists => File.Exists(ConfigPath);

        public static KeystoreInfo LoadKeystoreConfigByProjectName()
        {
            return LoadKeystoreConfig(UnityEngine.Application.productName);
        }

        public static KeystoreInfo LoadKeystoreConfig(string projectName)
        {
            if (!ConfigExists)
            {
                LogUtil.Error($"Config file not found at: {ConfigPath}");
                LogUtil.Error("Please create the config file using the setup menu.");
                return null;
            }

            try
            {
                var json = File.ReadAllText(ConfigPath);
                var configData = ParseConfig(json);

                if (configData.ContainsKey(projectName))
                {
                    var keystoreInfo = configData[projectName];
                    keystoreInfo.keystorePath = ExpandPath(keystoreInfo.keystorePath);
                    return keystoreInfo;
                }
                else
                {
                    LogUtil.Error($"Project '{projectName}' not found in config. Available projects: {string.Join(", ", configData.Keys)}");
                    return null;
                }
            }
            catch (Exception e)
            {
                LogUtil.Error($"Failed to load config: {e.Message}");
                return null;
            }
        }

        private static Dictionary<string, KeystoreInfo> ParseConfig(string json)
        {
            try
            {
                var configRoot = JsonConvert.DeserializeObject<AndroidConfigRoot>(json);
                return configRoot?.keystores ?? new Dictionary<string, KeystoreInfo>();
            }
            catch (Exception e)
            {
                LogUtil.Error($"JSON parse error: {e.Message}");
                return new Dictionary<string, KeystoreInfo>();
            }
        }

        private static string ExpandPath(string path)
        {
            if (path.StartsWith("~/"))
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    path.Substring(2)
                );
            }
            return path;
        }

        
        [MenuItem(EditorConstants.MFToolsRootMenuItem + "/Setup AppDev Config")]
        public static void SetupConfigDirectory()
        {
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
                Directory.CreateDirectory(Path.Combine(ConfigDirectory, "keystores"));

                // テンプレートファイルを作成
                string templateJson = @"{
  ""keystores"": {
    ""ExampleProject"": {
      ""keystorePath"": ""~/.AppDev/android/keystores/release.keystore"",
      ""keystorePass"": ""YOUR_KEYSTORE_PASSWORD"",
      ""keyaliasName"": ""YOUR_ALIAS_NAME"",
      ""keyaliasPass"": ""YOUR_ALIAS_PASSWORD""
    }
  }
}";
                File.WriteAllText(ConfigPath, templateJson);

                LogUtil.Log($"Created config directory at: {ConfigDirectory}");
                LogUtil.Log($"Please edit the config file: {ConfigPath}");

                // Finderで開く
                EditorUtility.RevealInFinder(ConfigPath);
            }
            else
            {
                LogUtil.Log($"Config directory already exists: {ConfigDirectory}");
                EditorUtility.RevealInFinder(ConfigPath);
            }
        }
    }
}