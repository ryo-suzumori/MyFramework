using System;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace MyFw
{
    /// <summary>
    /// SceneManagementModel
    /// </summary>
    public class SceneManagementModel
    {
        public async UniTaskVoid LoadSceneAsync(string sceneName)
        {
            // シーンを非同期で読み込む
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            await asyncOperation.ToUniTask();

            // 読み込みが完了したらシーンをアクティブにする
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        public async UniTask<Scene> LoadSceneAdditiveAsync(string sceneName)
        {
            // シーンを非同期で読み込む
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).ToUniTask();
            return SceneManager.GetSceneByName(sceneName);
        }
    }
}
