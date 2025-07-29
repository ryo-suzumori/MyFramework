using System;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace MyFw
{
    // 画面シェイクのパラメータを保持するDTO
    public class ScreenShakeDTO
    {
        public Vector2 maxShakeSize;
        public float duration;
    }

    [CreateAssetMenu(fileName = "ScreenShakeContextInstaller", menuName = "Installers/ScreenShakeContextInstaller")]
    public class ScreenShakeContextInstaller : ScriptableObjectInstaller<ScreenShakeContextInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<ScreenShakeDTO>();
            Container.BindInterfacesAndSelfTo<ScreenShaker>().FromNew().AsSingle().NonLazy();
        }
    }

    public class ScreenShaker : IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus signalBus;

        private Vector3 originalPosition;
        private CancellationTokenSource cancellationTockenSource;

        public void Initialize()
        {
            // カメラの初期位置を保存
            this.originalPosition = Camera.main.transform.localPosition;

            // SignalBusの購読
            this.signalBus.Subscribe<ScreenShakeDTO>(OnScreenShakeRequested);
        }

        private void OnScreenShakeRequested(ScreenShakeDTO dto)
        {
            this.cancellationTockenSource?.Cancel();
            this.cancellationTockenSource = new CancellationTokenSource();

            ShakeScreenAsync(dto, this.cancellationTockenSource.Token).Forget();
        }

        private async UniTaskVoid ShakeScreenAsync(ScreenShakeDTO parameters, CancellationToken cancellationToken)
        {
            float elapsed = 0f;

            try
            {
                while (elapsed < parameters.duration)
                {
                    // キャンセルが要求された場合、処理を中断
                    cancellationToken.ThrowIfCancellationRequested();

                    // 残り時間に基づいて強度を徐々に減少させる
                    float percentComplete = elapsed / parameters.duration;
                    float damper = 1.0f - Mathf.Clamp01(percentComplete);

                    // ランダムな方向にシェイク
                    float x = UnityEngine.Random.Range(-parameters.maxShakeSize.x, parameters.maxShakeSize.x) * damper;
                    float y = UnityEngine.Random.Range(-parameters.maxShakeSize.y, parameters.maxShakeSize.y) * damper;

                    // カメラ位置を更新
                    Camera.main.transform.localPosition = this.originalPosition + new Vector3(x, y, 0);

                    // 次のフレームまで待機
                    await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
                    elapsed += Time.deltaTime;
                }
            }
            catch (OperationCanceledException)
            {
                // キャンセル処理は特に何もしない
            }
            finally
            {
                // カメラを元の位置に戻す
                Camera.main.transform.localPosition = this.originalPosition;
            }
        }

        public void Dispose()
        {
            this.cancellationTockenSource?.Cancel();
            this.cancellationTockenSource?.Dispose();
        }
    }
}