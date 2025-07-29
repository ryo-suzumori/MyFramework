using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Collections;

namespace MyFw
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource oneShotSource;
        [SerializeField] private List<AudioSource> streamingSourceList = new();

        private AudioSource currentSouce;
        private AudioSource subSouce;
        private Coroutine fadeInCoroutine;
        private Coroutine fadeOutCoroutine;

        public void PlayOneShot(AudioClipInfo info)
        {
            if (this.oneShotSource)
            {
                this.oneShotSource.PlayOneShot(info.clip, info.volume);
            }
        }

        public void PlayStreaming(AudioClipInfo info, float crossSeconds, bool isNotLoop)
        {
            //Debug.Log($"Request PlayStreaming");

            var source = this.streamingSourceList.FirstOrDefault(s => !s.isPlaying);
            if (!source)
            {
                if (this.subSouce)
                {
                    source = this.subSouce;
                    this.subSouce.Stop();
                    this.subSouce = null;
                }
                else
                {
                    //Debug.Log($"=== Skip PlayStreaming : source is not found.");
                    return;
                }
            }

            if (this.currentSouce && this.currentSouce?.clip == info.clip)
            {
                //Debug.Log($"=== Skip PlayStreaming : clip is playing");
                return;
            }

            source.clip = info.clip;
            source.loop = !isNotLoop;

            CheckAndStopCoroutine(ref this.fadeInCoroutine);
            CheckAndStopCoroutine(ref this.fadeOutCoroutine);

            if (crossSeconds > 0f)
            {
                source.volume = 0;
                this.fadeInCoroutine = StartCoroutine(FadeInSource(info, crossSeconds));
                this.fadeOutCoroutine = StartCoroutine(FadeOutSource(crossSeconds));
            }
            else
            {
                source.volume = info.volume;
            }
            source.Play();
        }

        public void StopStreaming(float fadeSeconds = 0.1f)
        {
            CheckAndStopCoroutine(ref this.fadeInCoroutine);
            CheckAndStopCoroutine(ref this.fadeOutCoroutine);

            this.fadeOutCoroutine = StartCoroutine(FadeOutSource(fadeSeconds));
        }

        private void CheckAndStopCoroutine(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = null;
        }

        private IEnumerator FadeInSource(AudioClipInfo info, float fadeSeconds)
        {
            var fadeTime = 0f;
            var audioSource = this.streamingSourceList.FirstOrDefault(s => !s.isPlaying);
            
            this.currentSouce = audioSource;
            while (true)
            {
                fadeTime += Time.deltaTime;
                var rate = Mathf.Clamp01((float)(fadeTime / fadeSeconds));
                audioSource.volume = rate * info.volume;

                if (1f <= rate)
                {
                    break;
                }
                yield return null;
            }

            this.currentSouce = null;
        }

        private IEnumerator FadeOutSource(float fadeSeconds)
        {
            var audioSource = this.streamingSourceList.FirstOrDefault(s => s.isPlaying);
            if (audioSource != null)
            {
                this.subSouce = audioSource;

                var fadeTime = 0f;
                var baseVolume = audioSource.volume;

                while (true)
                {
                    fadeTime += Time.deltaTime;
                    var rate = Mathf.Clamp01((float)(fadeTime / fadeSeconds));
                    audioSource.volume = (1 - rate) * baseVolume;

                    if (1f <= rate)
                    {
                        break;
                    }
                    yield return null;
                }

                audioSource.Stop();
                this.subSouce = null;
            }       
        }
    }
}
