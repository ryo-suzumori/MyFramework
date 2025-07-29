using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using UniRx;

namespace MyFw
{
    /// <summary>
    /// timescale変更インターフェース.
    /// </summary>
    public interface ITimeScaler
    {
        float RequestTimeScale(string key);
        void ForceReset();
    }

    public class TimeScaleRequestDTO
    {
        public string key;
    }

    public class TimeScaleResetDTO {}

    /// <summary>
    /// TimeScale設定クラス
    /// </summary>
    [CreateAssetMenu(fileName = "TimeScaleContextInstaller", menuName = "Installers/TimeScaleContextInstaller")]
    public class TimeScaleContextInstaller : ScriptableObjectInstaller
    {
        /// <summary>
        /// 設定配列.
        /// </summary>
        [SerializeField] private List<TimeScaleContext> contextList = new();

        /// <summary>
        /// インストール実行
        /// </summary>
        public override void InstallBindings()
        {
            Container.Bind<IEnumerable<TimeScaleContext>>().FromInstance(this.contextList);
            Container.BindInterfacesTo<TimeScalePresenter>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesTo<TimeScalePresenterV2>().FromNew().AsSingle().NonLazy();

            Container.DeclareSignal<TimeScaleRequestDTO>();
            Container.DeclareSignal<TimeScaleResetDTO>();
        }

        /// <summary>
        /// TimeScale定義データ.
        /// </summary>
        [Serializable]
        private class TimeScaleContext
        {
            [SerializeField] private string key;
            [SerializeField] private float timeScale;
            [SerializeField] private float runningTime;

            public string Key => this.key;
            public float TimeScale => this.timeScale;
            public float RunningTime => this.runningTime;
        }

        /// <summary>
        /// TimeScalePresenter
        /// </summary>
        private class TimeScalePresenter : ITimeScaler, IDisposable
        {
            [Inject] private readonly IEnumerable<TimeScaleContext> contextList;
            private IDisposable timerDisposable;


            public TimeScalePresenter(
                IEnumerable<TimeScaleContext> contextList
                )
            {
                this.contextList = contextList;
            }

            public float RequestTimeScale(string key)
            {
                var data = this.contextList.FirstOrDefault(x => x.Key == key);
                if (data == null)
                {
                    return 0;
                }

                var execTime = data.TimeScale * data.RunningTime;
                Time.timeScale = data.TimeScale;

                if (execTime > 0f)
                {
                    this.timerDisposable?.Dispose();
                    this.timerDisposable = Observable.Timer(TimeSpan.FromSeconds(execTime))
                        .Subscribe(_ => Time.timeScale = 1.0f);
                }
                return execTime;
            }

            public void ForceReset()
            {
                Time.timeScale = 1.0f;
                this.timerDisposable?.Dispose();
            }

            public void Dispose()
            {
                ForceReset();
            }
        }

        /// <summary>
        /// TimeScalePresenterV2
        /// </summary>
        private class TimeScalePresenterV2 : IInitializable, IDisposable
        {
            [Inject] private readonly IEnumerable<TimeScaleContext> contextList;
            [Inject] private readonly SignalBus signalBus;
            private CancellationTokenSource cancellationTokenSource;

            public void Initialize()
            {
                this.signalBus.Subscribe<TimeScaleRequestDTO>(dto => ChangeTimeScale(dto).Forget());
                this.signalBus.Subscribe<TimeScaleResetDTO>(_ => ForceReset());
            }

            public void ForceReset()
            {
                Time.timeScale = 1.0f;
                this.cancellationTokenSource?.Cancel();
            }

            public void Dispose()
            {
                ForceReset();
                this.cancellationTokenSource?.Dispose();
            }

            public async UniTaskVoid ChangeTimeScale(TimeScaleRequestDTO dto)
            {
                var context = this.contextList.FirstOrDefault(c => c.Key == dto.key);
                if (context == null)
                {
                    return;
                }

                ForceReset();

                this.cancellationTokenSource = new();

                // 元のTimeScale値を保存
                float originalTimeScale = Time.timeScale;

                try
                {
                    // TimeScaleを変更
                    Time.timeScale = context.TimeScale;

                    if (context.RunningTime > 0)
                    {
                        // 実時間で待機（TimeScaleに影響されない）
                        await UniTask.WaitForSeconds(context.RunningTime, ignoreTimeScale: true, cancellationToken: this.cancellationTokenSource.Token);
                        // 元のTimeScaleに戻す
                        Time.timeScale = originalTimeScale;
                    }
                }
                catch (OperationCanceledException)
                {
                    // キャンセルされた場合も元のTimeScaleに戻す
                    Time.timeScale = originalTimeScale;
                }
            }
        }
    }
}

