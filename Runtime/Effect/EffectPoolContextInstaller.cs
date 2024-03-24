using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MyFw.Eff
{
    /// <summary>
    /// エフェクトプール作成用インストーラー.
    /// </summary>
    [CreateAssetMenu(fileName = "EffectPoolContextInstaller", menuName = "Installers/EffectPoolContextInstaller")]
    public class EffectPoolContextInstaller : ScriptableObjectInstaller
    {
        /// <summary>
        /// エフェクトプール設定.
        /// </summary>
        [Serializable]
        private struct EffectPoolContext
        {
            public string prefubPath;
            public int minPoolSize;
            public int maxPoolSize;
        }

        /// <summary>
        /// プレハブ読み込み先ルートパス.
        /// </summary>
        [SerializeField] private string prefabRootPath;

        /// <summary>
        /// ワールド座標系のエフェクトルートクラス.
        /// </summary>
        [SerializeField] private string worldRootObjectName;

        /// <summary>
        /// 設定配列.
        /// </summary>
        [SerializeField] private List<EffectPoolContext> worldContextList = new();

        /// <summary>
        /// ワールド座標系のエフェクトルートクラス.
        /// </summary>
        [SerializeField] private string canvasRootObjectName;

        /// <summary>
        /// 設定配列.
        /// </summary>
        [SerializeField] private List<EffectPoolContext> canvasContextList = new();

        private Transform worldRootTransform;
        private Transform canvasRootTransform;

        /// <summary>
        /// インストール実行
        /// </summary>
        public override void InstallBindings()
        {
            this.worldRootTransform = GameObject.Find(this.worldRootObjectName).transform;
            foreach(var context in this.worldContextList)
            {
                BindPoolStretch(this.worldRootTransform, context);
            }

            var temp = GameObject.Find(this.canvasRootObjectName);
            this.canvasRootTransform = temp.transform;
            foreach (var context in this.canvasContextList)
            {
                BindCanavsPoolStretch(this.canvasRootTransform, context);
            }
            
            Container.BindInterfacesAndSelfTo<EffectService>()
                .FromNew()
                .AsSingle();
        }

        /// <summary>
        /// プールのバインド.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prefubPath"></param>
        /// <param name="minPoolSize"></param行.>
        private void BindPoolStretch(Transform trans, EffectPoolContext context)
        {
            // root
            var name = Path.GetFileName(context.prefubPath);
            var poolRoot = new GameObject($"{name}").transform;
            poolRoot.SetParent(trans);

            Container.BindFactory<EffectAdapter, PoolFactory>()
                .WithFactoryArguments(name) // ファクトリに識別キーを送る.
                .FromMonoPoolableMemoryPool(pool =>
                    pool.WithInitialSize(context.minPoolSize)
                        .WithMaxSize(context.maxPoolSize)
                        .FromComponentInNewPrefabResource($"{this.prefabRootPath}/{context.prefubPath}")
                        .UnderTransform(poolRoot)
                    );
        }

        /// <summary>
        /// プールのバインド.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prefubPath"></param>
        /// <param name="minPoolSize"></param行.>
        private void BindCanavsPoolStretch(Transform trans, EffectPoolContext context)
        {
            // root
            var name = Path.GetFileName(context.prefubPath);
            var poolRoot = new GameObject($"{name}")
                .AddComponent<RectTransform>()
                .transform;
            poolRoot.SetParent(trans);
            poolRoot.localPosition = Vector3.zero;
            poolRoot.localScale = Vector3.one;

            Container.BindFactory<EffectAdapter, PoolFactory>()
                .WithFactoryArguments(name) // ファクトリに識別キーを送る.
                .FromMonoPoolableMemoryPool(pool =>
                    pool.WithInitialSize(context.minPoolSize)
                        .WithMaxSize(context.maxPoolSize)
                        .FromComponentInNewPrefabResource($"{this.prefabRootPath}/{context.prefubPath}")
                        .UnderTransform(poolRoot)
                    );
        }
    }
}
