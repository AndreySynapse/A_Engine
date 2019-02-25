// A-Engine, Code version: 1

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace AEngine.Audio
{
    public enum FadeMods
    {
        NotFading,
        FadeOut,
        FadeIn,
        FadeInOut,
        CrossMusicAndFullFade
    }

    public class AudioManager : MonoSingleton<AudioManager>
    {
        #region Test Block
        public List<AudioClip> _clips;
        public AudioSource _musicSource;
        public float _startMusicDelay;
        public bool _enableMusic;

        public AudioTrack _track;
        
        protected override void Init()
        {
            base.Init();

            StartCoroutine(PlayMusicProcess());

            DontDestroyOnLoad(this);
        }

        private IEnumerator PlayMusicProcess()
        {
            while (true)
            {
                yield return new WaitForSeconds(_startMusicDelay);

                if (_enableMusic)
                {
                    var clip = _clips[Random.Range(0, _clips.Count)];
                    _musicSource.clip = clip;
                    _musicSource.Play();
                }

                yield return null;
            }
        }


        #endregion






        #region Interface
        public string PlayingMusicName { get; set; }

        public string[] PlayingSoundNames { get; set; }

        public void LoadTheme() { }

        public void LoadSubTheme() { }

        public void ClearSubTheme() { }


        public void PlayMusic() { }

        public void PlayMusic(FadeMods fadeMode) { }


        public void PlaySound(string soundName) { }

        public void PlayRandomSound(string soundName) { }

        public void PlayClampSound() { }

        public void PlayRandomClampSound() { }

        public void PlaySoundAsMusic() { }

        public bool IsPlayingSound() { return false; }

        public void StopSound() { }

        public void StopSounds() { }

        public void StopAllSounds() { }


        public void PlayMonoSound() { }

        public void StopMonoSound() { }
        #endregion
    }
}