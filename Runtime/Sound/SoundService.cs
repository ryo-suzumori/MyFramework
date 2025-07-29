using System.Linq;
using System.Collections.Generic;
using UnityEngine.Audio;
using Zenject;

namespace MyFw
{
    /// <summary>
    /// SoundService
    /// </summary>
    public class SoundService : IInitializable
    {
        [Inject] private readonly SoundPlayer soundPlayer;
        [Inject(Id = SoundInjectionKey.SE)]
        private readonly IEnumerable<AudioClipInfo> oneShotList;
        [Inject(Id = SoundInjectionKey.BGM)]
        private readonly IEnumerable<AudioClipInfo> streamingList;
        [Inject] private readonly AudioMixer audioMixer;
        [Inject] private readonly SignalBus signalBus;

        public void Initialize()
        {
            this.signalBus.Subscribe<PlaySEAudioDTO>(PlaySE);
            this.signalBus.Subscribe<PlayBGMAudioDTO>(PlayBGM);
            this.signalBus.Subscribe<AudioVolumeDTO>(SetVolume);
            this.signalBus.Subscribe<AudioDataGetDTO>(GetData);
            this.signalBus.Subscribe<StopBGMDTO>(dto => StopBGM(dto.fadeSeconds));
        }

        public void PlaySE(PlaySEAudioDTO dto)
        {
            var info = this.oneShotList.FirstOrDefault(i => i.key == dto.key);
            if (info != null)
            {
                this.soundPlayer.PlayOneShot(info);
            }
            else
            {
                UnityEngine.Debug.LogError($"this name is not found in SE list. [{dto.key}]");
            }
        }

        public void PlayBGM(PlayBGMAudioDTO dto)
        {
            var info = this.streamingList.FirstOrDefault(i => i.key == dto.key);
            if (info != null)
            {
                this.soundPlayer.PlayStreaming(info, dto.crossSeconds, dto.isNotLoop);
            }
            else
            {
                UnityEngine.Debug.LogError($"this name is not found in BGM list. [{dto.key}]");
            }
        }

        public void StopBGM(float fadeSeconds = 0.1f)
        {
            this.soundPlayer.StopStreaming(fadeSeconds);
        }

        private void SetVolume(AudioVolumeDTO dto)
        {
            this.audioMixer.SetFloat("BGM_Group", ConvertLevelToDB(dto.bgm));
            this.audioMixer.SetFloat("SE_Group", ConvertLevelToDB(dto.se));
        }

        private float ConvertLevelToDB(float level)
            => UnityEngine.Mathf.Lerp(-80f, 0f, UnityEngine.Mathf.Clamp01(level));

        private float ConvertDBToLevel(float dB)
            => UnityEngine.Mathf.Pow(10f, dB / 20f);

        private void GetData(AudioDataGetDTO dto)
        {
            if (dto.getBGMVolume != null)
            {
                this.audioMixer.GetFloat("BGM_Group", out float volume);
                dto.getBGMVolume(ConvertDBToLevel(volume));
            }

            if (dto.getSEVolume != null)
            {
                this.audioMixer.GetFloat("SE_Group", out float volume);
                dto.getSEVolume(ConvertDBToLevel(volume));
            }
        }
    }
}
