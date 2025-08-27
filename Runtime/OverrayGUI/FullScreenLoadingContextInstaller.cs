using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

namespace MyFw
{
    public class FullScreenLoadingSignal
    {
        public string key;
        public IObservable<Unit> OnDestroy;
    }

    /// <summary>
    /// フルスクリーンローディングインストーラ
    /// </summary>
    [CreateAssetMenu(fileName = "FullScreenLoadingContextInstaller", menuName = "Installers/FullScreenLoadingContextInstaller")]
    public class FullScreenLoadingContextInstaller : ScriptableObjectInstaller
    {
        /// <summary>
        /// 設定配列.
        /// </summary>
        [SerializeField] private List<GameObject> popupPrefabList = new();

        /// <summary>
        /// インストール実行
        /// </summary>
        public override void InstallBindings()
        {
            Container.DeclareSignal<FullScreenLoadingSignal>();
            Container.BindInterfacesTo<FullScreenLoadingHub>().FromNew().AsSingle().NonLazy();

            foreach (var prefab in this.popupPrefabList)
            {
                RegisterView(Container, prefab);
            }
        }

        private static void RegisterView(DiContainer diContainer, GameObject prefab)
        {
            diContainer.BindFactory<FullScreenLoadingView, FullScreenLoadingViewFactory>()
                .WithFactoryArguments(prefab.name)
                .FromComponentInNewPrefab(prefab)
                .AsCached();
        }
    }

    /// <summary>
    /// FullScreenLoadingHub
    /// </summary>
    public class FullScreenLoadingHub : IInitializable
    {
        [Inject] private readonly SignalBus signalBus = default;
        [Inject] private readonly IEnumerable<FullScreenLoadingViewFactory> factories;

        public void Initialize()
        {
            this.signalBus
                .Subscribe<FullScreenLoadingSignal>(Receive);
        }

        public void Receive(FullScreenLoadingSignal signal)
        {
            var factory = this.factories.FirstOrDefault(f => f.Key == signal.key);
            LogUtil.Assert(factory != null, $"this key is not found. {signal.key}");

            var gui = factory?.Create();
            LogUtil.Assert(gui != null, $"Failed to create gui. {signal.key}");

            var canvas = GameObject.Find("Canvas").GetComponent<Transform>();
            LogUtil.Assert(canvas != null, $"Canvas is not found.");

            gui.transform.SetParent(canvas);
            gui.transform.localScale = Vector3.one;

            if (signal.OnDestroy != null)
            {
                signal.OnDestroy
                    .Subscribe(id => gui.DestroySelf())
                    .AddTo(gui);
            }
        }
    }
}
