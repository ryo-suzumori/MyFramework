using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

namespace MyFw
{
    public class OverlayGuiSignal
    {
        public string key;
        public OverlayGuiDTO dto;
        public bool disableFilter;
    }
    public class OverlayGuiDTO { }

    public interface IOverlayGui
    {
        GameObject gameObject { get; }
        void Setup(Transform root, OverlayGuiDTO desc, Action closeCallback);
        UniTask PlayOpen();
        UniTask PlayClose();
        UniTask SetActivate(bool flag);
    }

    public abstract class OverlayGui<DescriptionClass> : MonoBehaviour, IOverlayGui
        where DescriptionClass : OverlayGuiDTO
    {
        protected DescriptionClass description;
        protected Action closeCallback;
        protected readonly CompositeDisposable disposables = new();

        public void Setup(Transform root, OverlayGuiDTO desc, Action closeCallback)
        {
            this.closeCallback = closeCallback;
            this.description = desc as DescriptionClass;

            var trans = this.transform;
            trans.SetParent(root);
            trans.localPosition = Vector3.zero;
            trans.localScale = Vector3.one;

            SetupInner();
        }

        public async UniTask PlayOpen()
        {
            await Show();
        }

        public async UniTask PlayClose()
        {
            this.closeCallback();
            await Hide();

            this.disposables.Dispose();
            Destroy(this.gameObject);
        }

        public async UniTask SetActivate(bool flag)
        {
            if (flag)
            {
                await Activate();
            }
            else
            {
                await Inactivate();
            }
        }
        public virtual void SetupInner() { }
        public virtual async UniTask Show() => await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        public virtual async UniTask Hide() => await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        public virtual async UniTask Activate() => await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        public virtual async UniTask Inactivate() => await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
    }
}
