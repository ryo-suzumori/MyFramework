using System;
using UniRx;
using DG.Tweening;

namespace MyFw
{
    public static class DOTweenExtensions
    {
        /// <summary>
        /// Tween完了通知ストリーム化.
        /// </summary>
        /// <param name="tweener"></param>
        /// <returns></returns>
        public static IObservable<Tween> OnCompleteAsObservable(this Tween tweener)
        {
            return Observable.Create<Tween>(o =>
            {
                tweener.OnComplete(() =>
                {
                    o.OnNext(tweener);
                    o.OnCompleted();
                });
                return Disposable.Create(() => tweener.Kill());
            });
        }

        public static IObservable<Unit> OnCompleteAsUnitObservable(this Tween tweener)
        {
            return Observable.Create<Unit>(o =>
            {
                tweener.OnComplete(() =>
                {
                    o.OnNext(Unit.Default);
                    o.OnCompleted();
                });
                return Disposable.Create(() => tweener.Kill());
            });
        }

        /// <summary>
        /// 再生と完了通知を購読.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IObservable<Sequence> PlayAsObservable(this Sequence sequence)
        {
            return Observable.Create<Sequence>(o =>
            {
                sequence.OnComplete(() =>
                {
                    o.OnNext(sequence);
                    o.OnCompleted();
                });
                sequence.Play();
                return Disposable.Create(() => sequence.Kill() );
            });
        }

        public static IObservable<Unit> PlayAsUnitObservable(this Sequence sequence)
        {
            return Observable.Create<Unit>(o =>
            {
                sequence.OnComplete(() =>
                {
                    o.OnNext(Unit.Default);
                    o.OnCompleted();
                });
                sequence.Play();
                return Disposable.Create(() => sequence.Kill() );
            });
        }
    }
}