using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MyFw
{
    /// <summary>
    /// オーバーレイGUI作成用インストーラー.
    /// </summary>
    [CreateAssetMenu(fileName = "OverlayGuiContextInstaller", menuName = "Installers/OverlayGuiContextInstaller")]
    public class OverlayGuiContextInstaller : ScriptableObjectInstaller
    {
        /// <summary>
        /// 設定配列.
        /// </summary>
        [SerializeField] private List<GameObject> popupPrefubList = new();

        [SerializeField] private OverlayFilterGui filterObject;

        /// <summary>
        /// インストール実行
        /// </summary>
        public override void InstallBindings()
        {
            Container.DeclareSignal<OverlayGuiSignal>();
            Container.BindInterfacesTo<OverlayGuiHubView>().FromNew().AsSingle().NonLazy();

            OverlayGuiHubView.RegisterFilter(Container, this.filterObject);

            foreach (var prefub in this.popupPrefubList)
            {
                OverlayGuiHubView.RegisterPopup(Container, prefub);
            }
        }
    }
}
