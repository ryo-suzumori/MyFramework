using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;
using Cysharp.Threading.Tasks;

namespace MyFw
{
    public class LoadingSignSignal
    {
        public string key;
        public IObservable<Unit> OnDestroy;
        public bool isSkipOpen;

        public LoadingSignSignal(string key, bool isSkipOpen = false, IObservable<Unit> onDestroy = null)
        {
            this.key = key;
            this.OnDestroy = onDestroy;
            this.isSkipOpen = isSkipOpen;
        }
    }

    /// <summary>
    /// ローディングインストーラ
    /// </summary>
    [CreateAssetMenu(fileName = "LoadingSignContextInstaller", menuName = "Installers/LoadingSignContextInstaller")]
    public class LoadingSignContextInstaller : ScriptableObjectInstaller
    {
        /// <summary>
        /// 設定配列.
        /// </summary>
        [SerializeField] private List<LoadingSignViewBase> prefabList = new();

        /// <summary>
        /// インストール実行
        /// </summary>
        public override void InstallBindings()
        {
            Container.DeclareSignal<LoadingSignSignal>();
            Container.BindInterfacesTo<LoadingSignHub>().FromNew().AsSingle().NonLazy();

            foreach (var prefab in this.prefabList)
            {
                RegisterView(Container, prefab);
            }
        }

        private static void RegisterView(DiContainer diContainer, LoadingSignViewBase prefab)
        {
            diContainer.BindFactory<LoadingSignViewBase, LoadingSignViewFactory>()
                .WithFactoryArguments(prefab.name)
                .FromComponentInNewPrefab(prefab)
                .AsCached();
        }
    }

    /// <summary>
    /// FullScreenLoadingHub
    /// </summary>
    public class LoadingSignHub : IInitializable
    {
        [Inject] private readonly SignalBus signalBus;
        [Inject] private readonly IEnumerable<LoadingSignViewFactory> factories;

        public void Initialize()
        {
            this.signalBus
                .Subscribe<LoadingSignSignal>(signal => Receive(signal).Forget());
        }

        public async UniTaskVoid Receive(LoadingSignSignal signal)
        {
            var factory = this.factories.FirstOrDefault(f => f.Key == signal.key);
            LogUtil.Assert(factory != null, $"this key is not found. {signal.key}");

            var gui = factory?.Create();
            LogUtil.Assert(gui != null, $"Failed to create gui. {signal.key}");

            var canvas = GameObject.Find("Canvas").GetComponent<Transform>();
            LogUtil.Assert(canvas != null, $"Canvas is not found.");

            gui.transform.SetParent(canvas);
            gui.transform.localScale = Vector3.one;

            if (signal.isSkipOpen)
            {
                gui.RequestSkipOpen();
            }

            signal.OnDestroy
                ?.Subscribe(id => gui.RequestClose())
                .AddTo(gui);

            await gui.UpdateTask();
        }
    }
}
