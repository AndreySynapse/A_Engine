using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;
using AEngine.Parser;

namespace AEngine.Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
		private enum AudioState
		{
			Default = 0,
			FadeOffForNewMusic = 1,
			FadeOnForNewMusic = 2
		}
		private AudioState state;
                
        public bool IsMusic
        {
            get { return _generalAudioSettings.UseMusic; }
            set
            {
                if (_generalAudioSettings.UseMusic != value)
                {
                    _generalAudioSettings.UseMusic = value;
                    SaveAudioSettings ();
					if (_generalAudioSettings.UseMusic)
						PlayMusic();
                }				              
            }
        }

        public bool IsSound
        {
            get { return _generalAudioSettings.UseSound; }
            set
            {
                if (_generalAudioSettings.UseSound != value)
                {
                    _generalAudioSettings.UseSound = value;
                    SaveAudioSettings();
                }
            }
        }
        
		public float MusicVolumme
		{
			get { return _generalAudioSettings.MusicVolume; }
			set { _generalAudioSettings.MusicVolume = value; }
		}

        public float SoundVolumme
		{
			get { return _generalAudioSettings.SoundVolume; }
			set { _generalAudioSettings.SoundVolume = value; }
		}

		private int maxSoundSourceCount;
		private float fadeTime;
		private bool fadeOn;

		private AudioBlock audioBlock;
		private float delay;
                
        private AudioSource musicSource = null;
        private List<AudioSource> soundSource = null;

		private float musicTrackVolume;
		private string nextTrackName;

        private GeneralAudioSettings _generalAudioSettings;

        void Awake()
        {
            _generalAudioSettings = new GeneralAudioSettings();

			LoadAudioSettings ();
			LoadAudioConfiguration ();

            musicSource = AddAudioSource ();
            soundSource = new List<AudioSource>();
			soundSource.Add (AddAudioSource ());
			            
			audioBlock = new AudioBlock ();
			delay = 0;
			musicTrackVolume = 0;
			state = AudioState.Default;
        }

		private AudioSource AddAudioSource ()
		{
			AudioSource source = gameObject.AddComponent<AudioSource> ();
			source.playOnAwake = false;
			source.loop = false;

			return source;
		}

        public bool LoadAudioBlock(string blockName)
        {	
			if (audioBlock.name == blockName)
				return false;
            
            XmlDocument xmlDocument = XmlParser.LoadFromResources(AudioConstants.GetReadableRuntimeResourcesPath());
            XmlNode rootNode = XmlParser.GetRootTag(xmlDocument, AudioConstants.XML_ROOT);
			
			if (!XmlDataParser.IsAnyTagInChildExist (rootNode, "AudioBlock"))
				return false;
			
			foreach (XmlNode item in XmlDataParser.FindAllTagsInChild(rootNode, "AudioBlock")) {
				if (blockName == item.Attributes ["Name"].Value) {
					audioBlock.LoadFromXml (item);
					audioBlock.LoadAudioResources ();
					break;
				}
			}

			return true;
        }

		public bool LoadAudioBlock(AudioThemes block)
		{
			return LoadAudioBlock(block.ToString());
		}

		public void PlayMusic (bool fade = false)
		{
			if (!_generalAudioSettings.UseMusic)
				return;

			if (fade && fadeTime > 0) {
				state = AudioState.FadeOffForNewMusic;
				return;
			}

			audioBlock.PlayRandomMusic (musicSource, _generalAudioSettings.MusicVolume);
			musicTrackVolume = musicSource.volume;
			delay = audioBlock.music.delay;
		}

		public void PlayMusic (string trackName)
		{
			if (!_generalAudioSettings.UseMusic)
				return;

			audioBlock.PlayMusic (musicSource, trackName, _generalAudioSettings.MusicVolume);
			musicTrackVolume = musicSource.volume;
			delay = audioBlock.music.delay;
		}

		public void PlayMusic (Musics musicTrack)
		{
			PlayMusic(musicTrack.ToString());
		}

		public bool IsPlayingSound (string soundName)
		{
			for (int i = 0; i < soundSource.Count; i++) {
				if (soundSource [i].isPlaying && soundSource [i].clip.name == soundName)
					return true;
			}

			return false;
		}

		public bool IsPlayingSound (Sounds soundTrack)
		{
			return IsPlayingSound(soundTrack.ToString());
		}

		public void PlayUniqueSound (params string [] soundName)
		{
			for (int i = 0; i < soundName.Length; i++) {
				if (!IsPlayingSound (soundName[i])) {
					PlaySound (soundName [i]);
					break;
				}					
			}
		}

		public void PlayUniqueSound (params Sounds[] soundTracks)
		{
			for (int i = 0; i < soundTracks.Length; i++) {
				if (!IsPlayingSound (soundTracks[i])) {
					PlaySound (soundTracks[i]);
					break;
				}					
			}
		}

		public void PlayRandomSound (params string [] soundNames)
		{
			int index = Random.Range (0, soundNames.Length);
			PlaySound (soundNames[index]);
		}

		public void PlayRandomSound (params Sounds[] soundTracks)
		{
			int index = Random.Range (0, soundTracks.Length);
			PlaySound (soundTracks[index]);
		}

		public void PlaySound(string soundName, bool dontPlayIfSameIsPlaying = false)
		{
			if (!_generalAudioSettings.UseSound)
				return;

			if (dontPlayIfSameIsPlaying && IsPlayingSound (soundName))
				return;
			
			int index = -1;
			for (int i = 0; i < soundSource.Count; i++) {
				if (!soundSource [i].isPlaying) {
					index = i;
					break;
				}
			}
			if (index == -1) {
				if (soundSource.Count < maxSoundSourceCount) {
					soundSource.Add (AddAudioSource ());
					index = soundSource.Count - 1;
				} else
					index = 0;				
			}

			audioBlock.PlaySoundTrack (soundSource [index], soundName, _generalAudioSettings.SoundVolume);
		}

		public void PlaySound (Sounds soundTrack, bool dontPlayIfSameIsPlaying = false)
		{
			PlaySound(soundTrack.ToString(), dontPlayIfSameIsPlaying);
		}

		public void StopSound (string soundName)
		{
			if (!_generalAudioSettings.UseSound)
				return;
			
			for (int i = 0; i < soundSource.Count; i++) {
				if (soundSource [i].clip.name == soundName && soundSource [i].isPlaying) {
					soundSource [i].Stop ();
					return;
				}
			}
		}

		public void StopSound(Sounds soundTrack)
		{
			StopSound(soundTrack.ToString());
		}

		void Update()
		{
			if (musicSource.isPlaying)
            {
				if (!_generalAudioSettings.UseMusic) {
					if (Fade (false))
						musicSource.Stop ();
				}

				if (state == AudioState.Default)
					return;
			}

			if (!_generalAudioSettings.UseMusic)
				return;
			
			if (state == AudioState.FadeOffForNewMusic) {
				if (musicSource.isPlaying) {
					if (Fade (false)) {
						if (fadeOn) {
							nextTrackName = audioBlock.GetRandomMusic ();
							musicTrackVolume = _generalAudioSettings.MusicVolume * audioBlock.music.tracks [nextTrackName].Volume;
							audioBlock.PlayMusic (musicSource, nextTrackName, 0);
							state = AudioState.FadeOnForNewMusic;
							return;
						}
						PlayMusic ();
						state = AudioState.Default;
						return;
					}
				} else {
					if (fadeOn) {
						nextTrackName = audioBlock.GetRandomMusic ();
						musicTrackVolume = _generalAudioSettings.MusicVolume * audioBlock.music.tracks [nextTrackName].Volume;
						audioBlock.PlayMusic (musicSource, nextTrackName, 0);
						state = AudioState.FadeOnForNewMusic;
						return;
					}
					state = AudioState.Default;
					PlayMusic ();
					return;
				}
				return;
			} else if (state == AudioState.FadeOnForNewMusic) {
				if (Fade (true))
					state = AudioState.Default;
				return;
			}
			
			delay -= Time.unscaledDeltaTime;
			if (delay <= 0) {
				PlayMusic ();
			}
		}

		void OnApplicationFocus (bool focus)
		{
			if (focus) {
				if (musicSource.volume == 0)
					PlayMusic ();
			} else {
				musicSource.volume = 0;
			}
		}

		void OnApplicationPause (bool pause)
		{
			if (!pause) {
				if (musicSource.volume == 0)
					PlayMusic ();
			} else {
				musicSource.volume = 0;
			}
		}

		private bool Fade (bool On)
		{
			if (fadeTime == 0) {
				musicSource.volume = (On) ? musicTrackVolume : 0;
				return true;
			}

			float deltaVolume = (Time.unscaledDeltaTime / fadeTime) * musicTrackVolume;
			if (On) {
				musicSource.volume += deltaVolume;
				if (musicSource.volume >= musicTrackVolume) {
					musicSource.volume = musicTrackVolume;
					return true;
				}
			} else {
				musicSource.volume -= deltaVolume;
				if (musicSource.volume <= 0) {
					musicSource.volume = 0;
					return true;
				}
			}

			return false;
		}

		private void LoadAudioSettings ()
		{		
			XmlDocument xmlDocument;
			bool needSave = false;
                        
            if (!File.Exists(AudioConstants.GetCachePath()))
            {
                if (!File.Exists(AudioConstants.GetResourcesPath()))
                {
                    SaveAudioSettings ();
                    xmlDocument = XmlParser.LoadFromFile(AudioConstants.GetCachePath());
                    Debug.LogError("Couldn't find configuration file in resources");
                } else
                {
                    xmlDocument = XmlParser.LoadFromResources(AudioConstants.GetReadableRuntimeResourcesPath());
                    needSave = true;
				}
			} else
            {
                xmlDocument = XmlParser.LoadFromFile(AudioConstants.GetCachePath());
			}

			if (!XmlParser.IsExistRootTag(xmlDocument, AudioConstants.XML_ROOT))
            {
				Debug.Log ("AudioData not founded"); 
				return;
			}
			XmlNode rootNode = XmlParser.GetRootTag(xmlDocument, AudioConstants.XML_ROOT);

			if (!XmlDataParser.IsAnyTagInChildExist (rootNode, "AudioSettings")) {
				Debug.Log ("AudioSettings  not founded"); 
				return;
			}
			XmlNode audioNode = XmlDataParser.FindUniqueTagInChild (rootNode, "AudioSettings");
                        
            _generalAudioSettings.Load(audioNode);
            _generalAudioSettings.SoundVolume = _generalAudioSettings.SoundVolume;
            
            if (needSave)
				SaveAudioSettings ();
		}

		private void SaveAudioSettings ()
		{
			XmlDocument xmlDocument = new XmlDocument ();
            XmlNode rootNode = XmlParser.CreateRootTag(xmlDocument, AudioConstants.XML_ROOT);

			XmlNode audioNode = xmlDocument.CreateElement ("AudioSettings");
            _generalAudioSettings.Save(xmlDocument, audioNode);
			rootNode.AppendChild (audioNode);
                        
            if (!Directory.Exists(Path.GetDirectoryName(AudioConstants.GetCachePath())))
                Directory.CreateDirectory(Path.GetDirectoryName(AudioConstants.GetCachePath()));
            xmlDocument.Save(AudioConstants.GetCachePath());
		}
                
		private void LoadAudioConfiguration ()
		{
			// Default settings
			maxSoundSourceCount = 3;
			fadeTime = 0;
			fadeOn = false;
            
            if (!File.Exists(AudioConstants.GetResourcesPath()))
				return;
            
            XmlDocument xmlDocument = XmlParser.LoadFromResources(AudioConstants.GetReadableRuntimeResourcesPath());
			XmlNode rootNode = XmlDataParser.FindUniqueTag (xmlDocument, "AudioData");

			if (!XmlDataParser.IsAnyTagInChildExist (rootNode, "AudioConfiguration"))
				return;

			XmlNode configNode = XmlDataParser.FindUniqueTagInChild (rootNode, "AudioConfiguration");
			maxSoundSourceCount = int.Parse (configNode.Attributes ["SoundSourceCount"].Value);	
			fadeTime = float.Parse(configNode.Attributes ["fade"].Value);
			fadeOn = bool.Parse (configNode.Attributes ["fadeOn"].Value);
		}

        /*
        public enum FadeMods
        {
           NotFading,
           FadeOut,
           FadeIn,
           FadeInOut,
           CrossMusicAndFullFade
        }
          
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
        */

    }
}
