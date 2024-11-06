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
        private readonly List<PoolFactory> poolFactory;
        
        [Inject]
        public EffectService(
            List<PoolFactory> poolFactory
        )
        {
            this.poolFactory = poolFactory;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
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
            if (pool == null)
            {
                Debug.LogError($"this key is not found!! [{key}]");
                return null;
            }

            var ef = pool?.Create();
            if (ef != null) {
                ef.transform.position = pos;
            }
            return ef;
        }
    }
}