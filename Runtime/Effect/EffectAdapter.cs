using System;
using UnityEngine;
using System.Collections.Generic;
using UniRx;
using Zenject;

namespace MyFw.Eff
{
    /// <summary>
    /// エフェクト管理クラス
    /// </summary>
    public class EffectAdapter : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        [SerializeField] private List<ParticleSystem> particles = new();
        [SerializeField] private List<TrailRenderer> trailRenderers = new();
        public IReadOnlyList<ParticleSystem> Particles => this.particles;
        public IReadOnlyList<TrailRenderer> TrailRenderers => this.trailRenderers;
        private ParticleSystem rootPariticle;
        private IMemoryPool ownerPool;
        private readonly List<ICallback_OnPlay> onPlayList = new();
        private readonly List<ICallback_OnEnd> onEndList = new();
        
        public virtual void Reset()
        {
            this.particles = new(GetComponentsInChildren<ParticleSystem>());
            this.trailRenderers = new(GetComponentsInChildren<TrailRenderer>());
        }

        public void Awake()
        {
            this.rootPariticle = GetComponent<ParticleSystem>();
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

        /// <summary>
        /// パーティクル再生(手動)
        /// </summary>
        /// <param name="disableDelaySec"></param>
        public void PlayParticle()
        {
            this.rootPariticle.Play(true);
        }

        /// <summary>
        /// パーティクル停止
        /// </summary>
        /// <param name="disableDelaySec"></param>
        public void StopParticle(float disableDelaySec = 1f)
        {
            this.rootPariticle.Stop(true);

            Observable.Timer(TimeSpan.FromSeconds(disableDelaySec))
               .Subscribe(_ => Dispose());
        }

        /// <summary>
        /// 簡易色変更
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            var temp = new ParticleSystem.MinMaxGradient(color);
            foreach (var particle in this.particles)
            {
                var main = particle.main;
                main.startColor = temp;
            }

            foreach (var trail in this.trailRenderers)
            {
                trail.startColor = color;
            }
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