using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace MyFw
{
    /// <summary>
    /// SceneManagementModel
    /// </summary>
    public class SceneManagementModel : IDisposable
    {
        public void Setup()
        {
        }

        public void Dispose()
        {
        }

        public async UniTaskVoid LoadSceneAsync(string sceneName)
        {
            // シーンを非同期で読み込む
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            await asyncOperation.ToUniTask();

            // 読み込みが完了したらシーンをアクティブにする
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
    }
}
