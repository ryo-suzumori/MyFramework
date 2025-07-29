using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace MyFw
{
    public enum SoundInjectionKey : int
    {
        None = 0,
        BGM,
        SE,
    }

    public class PlaySEAudioDTO
    {
        public string key;
    }

    public class PlayBGMAudioDTO
    {
        public string key;
        public float crossSeconds = 0.1f;
        public bool isNotLoop;
    }

    public class AudioVolumeDTO
    {
        public float bgm;
        public float se;
    }

    public class AudioDataGetDTO
    {
        public Action<float> getBGMVolume;
        public Action<float> getSEVolume;
    }

    public class StopBGMDTO
    {
        public float fadeSeconds = 0.1f;
    }

    [Serializable]
    public class AudioClipInfo
    {
        public string key;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [CreateAssetMenu(fileName = "SoundContexstInstaller", menuName = "Installers/MyFwSoundContexstInstaller")]
    public class SoundContexstInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private SoundPlayer soundPlayerPrefab;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private List<AudioClipInfo> oneShotList = new();
        [SerializeField] private List<AudioClipInfo> streamingList = new();

        public override void InstallBindings()
        {
            Container.DeclareSignal<PlaySEAudioDTO>();
            Container.DeclareSignal<PlayBGMAudioDTO>();
            Container.DeclareSignal<AudioVolumeDTO>();
            Container.DeclareSignal<AudioDataGetDTO>();
            Container.DeclareSignal<StopBGMDTO>();

            Container.BindInterfacesTo<SoundService>().AsSingle().NonLazy();
            Container.BindInitializableExecutionOrder<SoundService>(-110);
            Container.Bind<AudioMixer>().FromInstance(this.audioMixer);
            Container.Bind<IEnumerable<AudioClipInfo>>().WithId(SoundInjectionKey.BGM).FromInstance(this.streamingList);
            Container.Bind<IEnumerable<AudioClipInfo>>().WithId(SoundInjectionKey.SE).FromInstance(this.oneShotList);
            Container.Bind<SoundPlayer>().FromComponentInNewPrefab(this.soundPlayerPrefab).AsSingle();
        }
    }
}