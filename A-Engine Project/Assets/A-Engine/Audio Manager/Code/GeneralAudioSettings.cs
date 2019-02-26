// A-Engine, Code version: 1

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Xml;
using System;
using UnityEngine;
using AEngine.Parser;

namespace AEngine.Audio
{
    [Serializable]
    public class GeneralAudioSettings
    {
        public bool UseMusic { get; set; }
        public bool UseSound { get; set; }

        public float MusicVolume { get; set; }
        public float SoundVolume { get; set; }

        public float CompressedMusicVolume { get; set; }
        public float CompressedSoundVolume { get; set; }
                
        public GeneralAudioSettings()
        {
            this.UseMusic = true;
            this.UseSound = true;
            this.MusicVolume = 1f;
            this.SoundVolume = 1f;
            this.CompressedMusicVolume = 1f;
            this.CompressedSoundVolume = 1f;
        }

        public void Load(XmlNode target)
        {
            this.UseMusic = bool.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_USE_MUSIC].Value);
            this.UseSound = bool.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_USE_SOUND].Value);
            this.MusicVolume = float.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_MUSIC_VOLUME].Value);
            this.SoundVolume = float.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_SOUND_VOLUME].Value);
            this.CompressedMusicVolume = float.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_MUSIC_COMPRESSED_VOLUME].Value);
            this.CompressedSoundVolume = float.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_SOUND_COMPRESSED_VOLUME].Value);
        }

        public void Save(XmlDocument xmlDocument, XmlNode target)
        {
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_USE_MUSIC, this.UseMusic.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_USE_SOUND, this.UseSound.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_MUSIC_VOLUME, this.MusicVolume.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_SOUND_VOLUME, this.SoundVolume.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_MUSIC_COMPRESSED_VOLUME, this.CompressedMusicVolume.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_SOUND_COMPRESSED_VOLUME, this.CompressedSoundVolume.ToString());
        }

#if UNITY_EDITOR
        private bool _showWindow;
        private Texture2D _gearIcon;

        public void DrawGUI()
        {
            if (_gearIcon == null)
            {
                _gearIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(ACodeTool.GetEngineRootDirectory(true) + "Audio Manager/Textures/GearIcon.png") as Texture2D;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("General audio settings", EditorStyles.boldLabel, GUILayout.Width(150f));
            EditorGUILayout.EndHorizontal();

            //GUI.DrawTexture(GUILayoutUtility.GetRect(60f, 59f), _gearIcon);

            EditorGUILayout.BeginHorizontal();
            _showWindow = EditorGUILayout.Foldout(_showWindow, "Runtime changable settings");
            EditorGUILayout.EndHorizontal();

            if (_showWindow)
            {
                float OFFSET = 20f;

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(OFFSET);

                EditorGUILayout.LabelField("Use music", GUILayout.Width(75f));
                this.UseMusic = EditorGUILayout.Toggle(this.UseMusic, GUILayout.Width(30));
                GUILayout.Space(65f);
                EditorGUILayout.LabelField("Music volume", GUILayout.Width(90f));
                this.MusicVolume = EditorGUILayout.Slider(this.MusicVolume, 0, 1, GUILayout.Width(200));

                EditorGUILayout.EndHorizontal();


                GUILayout.Space(3);


                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(OFFSET);

                EditorGUILayout.LabelField("Use sound", GUILayout.Width(75f));
                this.UseSound = EditorGUILayout.Toggle(this.UseSound, GUILayout.Width(30f));
                GUILayout.Space(65f);
                EditorGUILayout.LabelField("Sound volume", GUILayout.Width(90f));
                this.SoundVolume = EditorGUILayout.Slider(this.SoundVolume, 0, 1, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();
            }
        }
#endif
    }
}
