// A-Engine, Code version: 1

using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;
using AEngine.Parser;

namespace AEngine.Audio
{
	[Serializable]
	public class AudioConfigurationWindow : EditorWindow
	{
        private RuntimeChangableSettings _runtimeAudioSettings;
        private GeneralAudioSettings _generalAudioSettings;

		private Dictionary<string, AudioBlock> audioData;
		private float fadeTime;
		private bool useFadeOn;
		
		private Vector2 scrollPosition;

        private Texture2D _gearIcon;

        private List<string> namesList = new List<string> ();

		void OnEnable()
		{
            if (_gearIcon == null)
            {
                _gearIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(ACodeTool.GetEngineRootDirectory(true) + "Audio Manager/Textures/GearIcon.png") as Texture2D;
            }

            if (_runtimeAudioSettings == null)
                _runtimeAudioSettings = new RuntimeChangableSettings();
            if (_generalAudioSettings == null)
                _generalAudioSettings = new GeneralAudioSettings();

            XmlDocument doc = AudioDataParser.Load();
            AssetDatabase.Refresh();

            InitConfiguration(doc);
                        
            scrollPosition = Vector2.zero;
		}
					
		void OnGUI()
		{
			scrollPosition = GUILayout.BeginScrollView (scrollPosition);
            
            Color defaultColor = GUI.color;
			GUILayout.Space (9);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(38f);
            EditorGUILayout.LabelField("General audio settings", EditorStyles.boldLabel, GUILayout.Width(150f));
            DrawImage(_gearIcon, 20f, 20f, 10f, 0f);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(12);


            _runtimeAudioSettings.Draw();
			_generalAudioSettings.DrawGUI();

            GUILayout.Space(12);
                        
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Fade time", GUILayout.Width (180));
			fadeTime = EditorGUILayout.FloatField (fadeTime, GUILayout.Width (120));
			if (fadeTime < 0)
				fadeTime = 0;
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Use fade ON", GUILayout.Width (180));
			useFadeOn = EditorGUILayout.Toggle (useFadeOn, GUILayout.Width (120));
			EditorGUILayout.EndHorizontal ();

			GUILayout.Space (12);

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("New Audio Block", GUILayout.Width (250))) {
				AudioBlock newBlock = new AudioBlock ();
				newBlock.name = GetUniqueBlockName ();
				audioData.Add (newBlock.name, newBlock);
			}
			EditorGUILayout.EndHorizontal ();

			string removeKey = "";
            if (audioData != null)
            {
                foreach (var item in audioData)
                {
                    GUILayout.Space(12);
                    AEditorTool.DrawSeparator(AEditorTool.SeparationStyle.BigDark);

                    var block = item.Value;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Audio Block Name:");
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        removeKey = item.Key;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    string name = block.name;
                    name = EditorGUILayout.TextField(name, GUILayout.MinWidth(100), GUILayout.MaxWidth(250));
                    if (audioData.ContainsKey(name) && audioData[name] != block)
                        name = GetUniqueBlockName(name);
                    block.name = name;
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(12);

                    EditorGUILayout.BeginHorizontal();
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("Music:");
                    GUI.color = defaultColor;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Delay between musics", GUILayout.Width(150));
                    block.music.delay = EditorGUILayout.FloatField(block.music.delay, GUILayout.Width(40));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Not reply length", GUILayout.Width(150));
                    block.music.musicNotReplyCount = EditorGUILayout.IntField(block.music.musicNotReplyCount, GUILayout.Width(40));
                    if (block.music.musicNotReplyCount < 0)
                        block.music.musicNotReplyCount = 0;
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(6);

                    DrawTrackList(block.music, true);
                    GUILayout.Space(6);

                    EditorGUILayout.BeginHorizontal();
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("Sound:");
                    GUI.color = defaultColor;
                    EditorGUILayout.EndHorizontal();

                    DrawTrackList(block.sound, false);
                }
            }

			if (removeKey != "") {
				audioData.Remove (removeKey);
			}

			GUILayout.Space (12);
			if (GUILayout.Button ("Save")) {
				SaveConfiguration (true);
				SaveNamesInCode();
			}
			GUILayout.Space (20);
			EditorGUILayout.EndScrollView ();
		}

        private void DrawImage(Texture2D image, float width, float height, float x, float yOffset)
        {
            Rect rect = GUILayoutUtility.GetRect(width, height);
            rect.width = width;
            rect.height = height;
            rect.x = x;
            rect.y += yOffset;
            
            GUI.DrawTexture(rect, _gearIcon);
        }

		private void DrawTrackList (TrackList trackList, bool withSpecial)
		{
			if (trackList.IsFilled) {
				string removeKey = "";
				foreach (var item in trackList.tracks) {
					Track track = item.Value;

					EditorGUILayout.BeginHorizontal ();
					if (GUILayout.Button ("-", GUILayout.Width (20))) {
						removeKey = item.Key;
					}
					EditorGUILayout.LabelField (track.name, GUILayout.Width (120));
					EditorGUILayout.LabelField ("Volume:", GUILayout.Width (60));
					track.Volume = EditorGUILayout.Slider (track.Volume, 0, 1, GUILayout.Width (200));
					if (withSpecial) {
						MusicTrackList _trackList = trackList as MusicTrackList;
						GUILayout.Space (30);
						EditorGUILayout.LabelField ("Not play in background", GUILayout.Width(150));
						bool special = !_trackList.IsBackgroundMusic (track);
						special = EditorGUILayout.Toggle (special, GUILayout.Width(20));
						if (special != !_trackList.IsBackgroundMusic(track)) {
							if (special)
								_trackList.RemoveFromBackgroundMusic (track);
							else
								_trackList.AddTrackToBackgroundMusic (track);
						}
					}
					EditorGUILayout.EndHorizontal ();
				}

				if (removeKey != "") {
					if (withSpecial) {
						MusicTrackList _trackList = trackList as MusicTrackList;
						_trackList.RemoveFromBackgroundMusic (_trackList.tracks[removeKey]);
						_trackList.tracks.Remove (removeKey);
					} else
						trackList.tracks.Remove (removeKey);
					if (namesList.Contains (removeKey))
						namesList.Remove (removeKey);
				}

				GUILayout.Space (8);
			}

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button("Add selected tracks", GUILayout.Width(200))) {
				foreach (var obj in Selection.objects) {
					string path = AssetDatabase.GetAssetPath (obj);
					if (obj == null)
						continue;

					if (!Directory.Exists (path)) {
						AddTrack (trackList, path, withSpecial);
						continue;
					}

					string[] files = Directory.GetFiles (path);
					if (files != null) {
						for (int i = 0; i < files.Length; i++)
							AddTrack (trackList, files [i], withSpecial);
					}
				}
			}
			EditorGUILayout.EndHorizontal ();
		}

		private void InitConfiguration (XmlDocument xmlDocument)
		{
			if (audioData == null)
				audioData = new Dictionary<string, AudioBlock> ();
			else
				audioData.Clear ();

            XmlNode rootNode = XmlParser.GetRootTag(xmlDocument, AudioConstants.XML_ROOT);
			if (XmlDataParser.IsAnyTagInChildExist (rootNode, AudioConstants.XML_RUNTIME_TAG)) {
				XmlNode defaultNode = XmlDataParser.FindUniqueTagInChild (rootNode, AudioConstants.XML_RUNTIME_TAG);
                _runtimeAudioSettings.Load(defaultNode);
			}

			if (XmlDataParser.IsAnyTagInChildExist (rootNode, "AudioConfiguration")) {
				XmlNode configNode = XmlDataParser.FindUniqueTagInChild (rootNode, "AudioConfiguration");
                _generalAudioSettings.Load(configNode);
                //soundSourceCount = int.Parse (configNode.Attributes ["SoundSourceCount"].Value);	
				fadeTime = float.Parse(configNode.Attributes ["fade"].Value);
				useFadeOn = bool.Parse (configNode.Attributes ["fadeOn"].Value);
			}

			if (!XmlDataParser.IsAnyTagInChildExist (rootNode, "AudioBlock"))
				return;

			foreach (XmlNode item in XmlDataParser.FindAllTagsInChild(rootNode, "AudioBlock")) {
				string key = item.Attributes ["Name"].Value;
								
				if (!audioData.ContainsKey (key)) {
					audioData.Add (key, new AudioBlock ());
					audioData [key].LoadFromXml (item);
				} else {
					Debug.LogError ("Some equals audio blocks name");
				}
			}
		}

		private void SaveConfiguration (bool saveAdditionalToResources = false)
		{
			XmlDocument xmlDocument = new XmlDocument ();
            XmlNode root = XmlParser.CreateRootTag(xmlDocument, AudioConstants.XML_ROOT);

			XmlNode defaultNode = xmlDocument.CreateElement(AudioConstants.XML_RUNTIME_TAG);
            _runtimeAudioSettings.Save(xmlDocument, defaultNode);

            root.AppendChild (defaultNode);

			defaultNode = xmlDocument.CreateElement ("AudioConfiguration");
            _generalAudioSettings.Save(xmlDocument, defaultNode);
			//XmlDataParser.AddAttributeToNode (xmlDocument, defaultNode, "SoundSourceCount", soundSourceCount.ToString());	
			XmlDataParser.AddAttributeToNode (xmlDocument, defaultNode, "fade", fadeTime.ToString ());
			XmlDataParser.AddAttributeToNode (xmlDocument, defaultNode, "fadeOn", useFadeOn.ToString ());
			root.AppendChild (defaultNode);

			if (audioData != null) {
				foreach (var item in audioData) {
					XmlNode blockNode = xmlDocument.CreateElement ("AudioBlock");
					item.Value.SaveToXml (xmlDocument, blockNode);
					root.AppendChild (blockNode);
				}
			} else
				return;
                        
            xmlDocument.Save(AudioConstants.GetResourcesPath());

			AssetDatabase.Refresh ();
		}
        		
		private string GetUniqueBlockName (string startName = "NewBlock")
		{
			if (audioData == null || audioData.Count == 0)
				return startName;
			
			string name = startName;
			int index = 1;
			while (audioData.ContainsKey (name)) {
				name = startName + index;
				index++;
			}
			
			return name;				
		}

		private void AddTrack (TrackList tracks, string path, bool isMusic)
		{				
			string subPath = path.Substring (path.IndexOf ("Resources/"));
			subPath = subPath.Remove (0, 10);

			string extansion = Path.GetExtension (subPath);
			if (extansion == ".meta")				
				return;
			
			string shortPath = subPath.Replace (extansion, "");
			string audioName = Path.GetFileNameWithoutExtension (subPath);

			if (!tracks.tracks.ContainsKey (audioName)) {
				Track track = new Track ();
				track.name = audioName;
				track.path = shortPath;
				tracks.tracks.Add (audioName, track);
				if (isMusic) {
					MusicTrackList _tracks = tracks as MusicTrackList;
					_tracks.AddTrackToBackgroundMusic (track);
				}
			}

			return;
		}

		private void SaveNamesInCode()
		{
			CodeManager.ClearBlock(AudioConstants.CODE_PARSER_FILE_NAME, "", AudioConstants.CODE_PARSER_THEMES_ENUM, 1);
			CodeManager.ClearBlock(AudioConstants.CODE_PARSER_FILE_NAME, "", AudioConstants.CODE_PARSER_SOUNDS_ENUM, 1);
			CodeManager.ClearBlock(AudioConstants.CODE_PARSER_FILE_NAME, "", AudioConstants.CODE_PARSER_MUSICS_ENUM, 1);

			List<string> blocks = new List<string>();
			List<string> sounds = new List<string>();
			List<string> musics = new List<string>();
			foreach (var data in audioData)
			{
				blocks.Add(data.Value.name);
				JoinItemsToList(sounds, data.Value.sound.GetTracksNames());
				JoinItemsToList(musics, data.Value.music.GetTracksNames());
			}

			CodeManager.AddItemsToBlock(AudioConstants.CODE_PARSER_FILE_NAME, "", AudioConstants.CODE_PARSER_THEMES_ENUM, blocks.ToArray(), 1);
			CodeManager.AddItemsToBlock(AudioConstants.CODE_PARSER_FILE_NAME, "", AudioConstants.CODE_PARSER_SOUNDS_ENUM, sounds.ToArray(), 1);
			CodeManager.AddItemsToBlock(AudioConstants.CODE_PARSER_FILE_NAME, "", AudioConstants.CODE_PARSER_MUSICS_ENUM, musics.ToArray(), 1);
		}

		private void JoinItemsToList(List<string> baseList, List<string> sourceList)
		{
			foreach (string item in sourceList)
			{
				if (!baseList.Contains(item))
				{
					baseList.Add(item);
				}
			}
		}
	}
}
