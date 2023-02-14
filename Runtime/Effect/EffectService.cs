using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Zenject;

namespace MyFw.Eff
{
    /// <summary>
    /// エフェクト生成インターフェース.
    /// </summary>
    public interface ISpawner
    {
        EffectAdapter Spwan(string key, Vector3 pos);
    }

    /// <summary>
    /// エフェクトサービス.
    /// </summary>
    public class EffectService : IDisposable, IInitializable, ISpawner
    {
        private readonly List<PoolFactory> poolFactory = null;
        private readonly GameObject effectRoot = null;

        public EffectService(
            [Inject] List<PoolFactory> poolFactory,
            [Inject(Id = "EffectRoot")] GameObject effRoot
        )
        {
            this.poolFactory = poolFactory;
            this.effectRoot = effRoot;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// 有効なエフェクトを検索.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public EffectAdapter[] FindEffects()
        {
            return this.effectRoot.GetComponentsInChildren<EffectAdapter>();
        }

        /// <summary>
        /// エフェクト生成.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public EffectAdapter Spwan(string key, Vector3 pos)
        {
            var pool = this.poolFactory.FirstOrDefault(p => p.Key == key);
            var ef = pool?.Create();
            if (ef != null) {
                ef.transform.position = pos;
            }
            return ef;
        }
    }
}