using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

namespace MyFw
{
    public class LoadingSignViewFactory : PlaceholderFactory<LoadingSignViewBase>
    {
        /// <summary>
        /// ファクトリ識別用キー.
        /// </summary>
        public string Key { get; }

        [Inject]
        public LoadingSignViewFactory(string key) => this.Key = key;
    }

    public abstract class LoadingSignViewBase : MonoBehaviour
    {
        /// <summary>
        /// 開いているかどうか
        /// </summary>
        protected bool isOpen = false;

        /// <summary>
        /// オープンアニメーションをスキップをリクエストする
        /// </summary>
        public virtual void RequestSkipOpen() { }

        /// <summary>
        /// クローズのリクエスト
        /// </summary>
        public void RequestClose() => isOpen = false;

        /// <summary>
        /// クローズ処理
        /// </summary>
        /// <returns></returns>
        public abstract UniTask Close();

        /// <summary>
        /// 更新タスク
        /// </summary>
        /// <returns></returns>
        public async UniTask UpdateTask()
        {
            this.isOpen = true;
            await UniTask.WaitUntil(() => this.isOpen == false);
            await Close();
            // 自身を破棄
            Destroy(this.gameObject);
        }
    }
}