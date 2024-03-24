using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
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
            private readonly IEnumerable<TimeScaleContext> contextList;
            private IDisposable timerDisposable;

            [Inject]
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
    }
}
