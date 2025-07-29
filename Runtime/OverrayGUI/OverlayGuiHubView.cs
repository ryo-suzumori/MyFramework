using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;
using Cysharp.Threading.Tasks;

namespace MyFw
{
    public class OverlayGuiFactory : PlaceholderFactory<IOverlayGui>
    {
        /// <summary>
        /// ファクトリ識別用キー.
        /// </summary>
        public string Key { get; }

        [Inject]
        public OverlayGuiFactory(string key) => this.Key = key;
    }

    /// <summary>
    /// OverlayGuiHubView
    /// </summary>
    public class OverlayGuiHubView : IInitializable
    {
        [Inject] private readonly SignalBus signalBus = default;
        [Inject] private readonly IEnumerable<OverlayGuiFactory> factories;
        [Inject(Id = "Filter")] private readonly OverlayGuiFactory filterGuiFactory;

        private readonly LinkedList<IOverlayGui> overlayGuiQueue = new();

        public void Initialize()
        {
            this.signalBus
                .Subscribe<OverlayGuiSignal>(OnSignal);
        }

        private void OnSignal(OverlayGuiSignal signal)
        {
            var factory = this.factories.FirstOrDefault(f => f.Key == signal.key);
            Debug.Assert(factory != null, $"this key is not found. {signal.key}");

            if (!signal.disableFilter && !this.overlayGuiQueue.Any())
            {
                CreateFilter();
            }

            var gui = factory?.Create();
            Debug.Assert(gui != null, $"Failed to create gui. {signal.key}");

            var canvas = GameObject.Find("Canvas").GetComponent<Transform>();
            Debug.Assert(canvas != null, $"Canvas is not found.");

            gui?.Setup(canvas, signal.dto, CloseCallback);
            gui?.PlayOpen().Forget();

            RegisterGui(gui);
        }

        /// <summary>
        /// Overlay Filter 生成
        /// </summary>
        private void CreateFilter()
        {
            if (this.filterGuiFactory == null)
            {
                return;
            }

            var filter = this.filterGuiFactory.Create();
            var canvas = GameObject.Find("Canvas").GetComponent<Transform>();
            Debug.Assert(canvas != null, $"Canvas is not found.");

            filter?.Setup(canvas, null, CloseCallback);
            filter?.PlayOpen().Forget();

            RegisterGui(filter);
        }

        private void RegisterGui(IOverlayGui gui)
        {
            if (gui != null)
            {
                this.overlayGuiQueue.LastOrDefault()?.SetActivate(false).Forget();
                this.overlayGuiQueue.AddLast(gui);
            }
        }

        private void CloseCallback()
        {
            if (this.overlayGuiQueue.Any())
            {
                this.overlayGuiQueue.RemoveLast();
                this.overlayGuiQueue.LastOrDefault()?.SetActivate(true).Forget();
            }
        }

        public static void RegisterPopup(DiContainer diContainer, GameObject prefab)
        {
            diContainer.BindFactory<IOverlayGui, OverlayGuiFactory>()
                .WithFactoryArguments(prefab.name)
                .FromComponentInNewPrefab(prefab)
                .AsCached();
        }

        public static void RegisterFilter(DiContainer diContainer, OverlayFilterGui prefab)
        {
            diContainer.BindFactory<IOverlayGui, OverlayGuiFactory>()
                .WithId("Filter")
                .WithFactoryArguments(prefab.name)
                .FromComponentInNewPrefab(prefab)
                .AsCached();
        }
    }
}
