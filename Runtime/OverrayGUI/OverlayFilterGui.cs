using System;
using Cysharp.Threading.Tasks;

namespace MyFw
{
    public class OverlayFilterGui : OverlayGui<OverlayGuiDTO>
    {
        public override async UniTask Activate()
        {
            await PlayClose();
        }

        public override async UniTask Inactivate()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }
}