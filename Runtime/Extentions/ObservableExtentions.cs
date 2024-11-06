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
            var elapsed = 0f;

            return Observable.EveryEndOfFrame()
                .Do(_ => elapsed += Time.deltaTime)
                .Select(t => Mathf.Clamp01(1f - (durationSeccond - elapsed) / durationSeccond))
                .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(durationSeccond)))
                .Concat(Observable.Return(1f))
                .DistinctUntilChanged();
        }
    }
}

