using UnityEngine;
using System.Diagnostics;

namespace MyFw
{
    public static class LogUtil
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD || USE_FORCE_LOG
            UnityEngine.Debug.unityLogger.logEnabled = true;
#else
            UnityEngine.Debug.unityLogger.logEnabled = UnityEngine.Debug.isDebugBuild;
#endif
            UnityEngine.Debug.Log($"===== Debug Log Mode ({UnityEngine.Debug.unityLogger.logEnabled})=====");
        }

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("USE_FORCE_LOG")]
        public static void Log(object obj) => UnityEngine.Debug.Log("[LOG]" + obj);

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("USE_FORCE_LOG")]
        public static void Log(object obj, UnityEngine.Object obj2) => UnityEngine.Debug.Log("[LOG]" + obj, obj2);

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("USE_FORCE_LOG")]
        public static void LogWarning(object obj) => UnityEngine.Debug.LogWarning("[WARNING]" + obj);

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR"), Conditional("USE_FORCE_LOG")]
        public static void LogWarning(object obj, UnityEngine.Object obj2) => UnityEngine.Debug.LogWarning("[WARNING]" + obj, obj2);
        public static void Error(object obj) => UnityEngine.Debug.LogError("[ERROR]" + obj);
        public static void Error(object obj, UnityEngine.Object obj2) => UnityEngine.Debug.LogError("[ERROR]" + obj, obj2);
        public static void Assert(bool condition) => UnityEngine.Debug.Assert(condition);   
        public static void Assert(bool condition, string message) => UnityEngine.Debug.Assert(condition, message);
        public static void Assert(bool condition, string message, UnityEngine.Object obj) => UnityEngine.Debug.Assert(condition, message, obj);
    }
}