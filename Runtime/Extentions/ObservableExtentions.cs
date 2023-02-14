using System;
using UnityEngine;
using UniRx;

namespace MyFw
{
    public static class ObservableExtention
    {
        /// <summary>
        /// 線形補間ストリームを作成.
        /// </summary>
        /// <param name="durationSeccond"></param>
        public static IObservable<float> CreateLerp(float durationSeccond)
        {
            return UniRx.Observable.Create<float>(observer =>
            {
                float elapsed = 0;
                return UniRx.Observable.IntervalFrame(2)
                    .Subscribe(e =>
                    {
                        elapsed += Time.deltaTime;
                        observer.OnNext(Mathf.Clamp01(elapsed / durationSeccond));
                        if (elapsed > durationSeccond)
                        {
                            observer.OnCompleted();
                        }
                    });
            });
        }

    }
}