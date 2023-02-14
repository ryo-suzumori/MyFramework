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
        /// 設定配列.
        /// </summary>
        [SerializeField] private List<EffectPoolContext> contextList = new List<EffectPoolContext>();

        /// <summary>
        /// ルートオブジェクト保存用.
        /// </summary>
        private GameObject rootObject;

        /// <summary>
        /// エフェクトルートのオブジェクト名.
        /// </summary>
        private const string ObjectName = "EffectRoot";

        /// <summary>
        /// インストール実行
        /// </summary>
        public override void InstallBindings()
        {
            var gameObject = GameObject.Find(ObjectName);
            this.rootObject = gameObject != null ? gameObject : new GameObject(ObjectName);
            this.contextList.ForEach(BindPoolStretch);

            Container.Bind<GameObject>()
                .WithId(ObjectName)
                .FromInstance(this.rootObject);

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
        private void BindPoolStretch(EffectPoolContext context)
        {
            // root
            var name = Path.GetFileName(context.prefubPath);
            var poolRoot = new GameObject($"{name}").transform;
            poolRoot.SetParent(this.rootObject.transform);

            Container.BindFactory<EffectAdapter, PoolFactory>()
                .WithFactoryArguments(name) // ファクトリに識別キーを送る.
                .FromMonoPoolableMemoryPool(pool =>
                    pool.WithInitialSize(context.minPoolSize)
                        .WithMaxSize(context.maxPoolSize)
                        .FromComponentInNewPrefabResource(context.prefubPath)
                        .UnderTransform(poolRoot)
                    );
        }
    }
}
