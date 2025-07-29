using UnityEngine;
using Zenject;

namespace MyFw
{
    public class FullScreenLoadingViewFactory : PlaceholderFactory<FullScreenLoadingView>
    {
        /// <summary>
        /// ファクトリ識別用キー.
        /// </summary>
        public string Key { get; }

        [Inject]
        public FullScreenLoadingViewFactory(string key) => this.Key = key;
    }

    public class FullScreenLoadingView : MonoBehaviour
    {
        public void DestroySelf()
        {
            Destroy(this.gameObject);
        }
    }
}