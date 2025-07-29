using System;
using System.Diagnostics;
using UnityEngine;

namespace MyFw
{
    public static class LogUtil
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var enable = UnityEngine.Debug.unityLogger.logEnabled = UnityEngine.Debug.isDebugBuild;

            UnityEngine.Debug.Log($"===== Debug Log Mode ({enable})=====");
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Log(object obj) => UnityEngine.Debug.Log("[LOG]" + obj);

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Log(object obj, UnityEngine.Object obj2) => UnityEngine.Debug.Log("[LOG]" + obj, obj2);

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void LogWarning(object obj) => UnityEngine.Debug.LogWarning("[WARNING]" + obj);

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void LogWarning(object obj, UnityEngine.Object obj2) => UnityEngine.Debug.LogWarning("[WARNING]" + obj, obj2);
        public static void Error(object obj) => UnityEngine.Debug.LogError("[ERROR]" + obj);
        public static void Error(object obj, UnityEngine.Object obj2) => UnityEngine.Debug.LogError("[ERROR]" + obj, obj2);
        public static void Assert(bool condition) => UnityEngine.Debug.Assert(condition);   
        public static void Assert(bool condition, string message) => UnityEngine.Debug.Assert(condition, message);
        public static void Assert(bool condition, string message, UnityEngine.Object obj) => UnityEngine.Debug.Assert(condition, message, obj);
    }
}