using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace MyFw.Eff
{
    /// <summary>
    /// エフェクト管理クラス
    /// </summary>
    public class EffectAdapter : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        [SerializeField]
        private List<ParticleSystem> particles = new();
        public IReadOnlyList<ParticleSystem> Particles => this.particles;

        private IMemoryPool ownerPool;
        private readonly List<ICallback_OnPlay> onPlayList = new();
        private readonly List<ICallback_OnEnd> onEndList = new();
        
        public virtual void Reset()
        {
            this.particles = new(GetComponentsInChildren<ParticleSystem>());
        }

        public void Awake()
        {
            this.onPlayList.AddRange(this.GetComponents<ICallback_OnPlay>());
            this.onEndList.AddRange(this.GetComponents<ICallback_OnEnd>());
        }

        public virtual void Dispose()
        {
            this.ownerPool?.Despawn(this);
        }

        /// <summary>
        /// Poolに返却時.
        /// </summary>
        public virtual void OnDespawned()
        {
            this.onEndList.ForEach(callback => callback.OnEnd());
            this.ownerPool = null;
        }

        /// <summary>
        /// Poolから生成時.
        /// </summary>
        public virtual void OnSpawned(IMemoryPool pool)
        {
            this.ownerPool = pool;
        }

        public void OnPlay(IGameData gameData)
        {
            this.onPlayList.ForEach(callback => callback.OnPlay(gameData));
        }
    }

    public interface IGameData { }
    public interface ICallback_OnPlay { void OnPlay(IGameData gameData); }
    public interface ICallback_OnEnd { void OnEnd(); }


    /// <summary>
    /// プール用ファクトリクラス.
    /// </summary>
    public class PoolFactory : PlaceholderFactory<EffectAdapter>
    {
        /// <summary>
        /// ファクトリ識別用キー.
        /// </summary>
        public string Key { get; }

        [Inject]
        public PoolFactory(string key)
        {
            this.Key = key;
        }
    }
}