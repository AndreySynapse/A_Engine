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
        private bool _enableContent;

        public float CompressedMusicVolume { get; set; }
        public float CompressedSoundVolume { get; set; }
        public int MaxSoundSources { get; set; }
        public FadeMods FadeMode { get; set; }
        
        public GeneralAudioSettings()
        {
            this.CompressedMusicVolume = 1f;
            this.CompressedSoundVolume = 1f;
            this.MaxSoundSources = 3;

            _enableContent = true;
        }

        public void Load(XmlNode target)
        {
            this.CompressedMusicVolume = float.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_MUSIC_COMPRESSED_VOLUME].Value);
            this.CompressedSoundVolume = float.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_SOUND_COMPRESSED_VOLUME].Value);
            this.MaxSoundSources = int.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_SOUND_SOURCES_COUNT].Value);
        }

        public void Save(XmlDocument xmlDocument, XmlNode target)
        {
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_MUSIC_COMPRESSED_VOLUME, this.CompressedMusicVolume.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_SOUND_COMPRESSED_VOLUME, this.CompressedSoundVolume.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_SOUND_SOURCES_COUNT, this.MaxSoundSources.ToString()); 
        }

#if UNITY_EDITOR
        public void DrawGUI()
        {
            const float CONTENT_OFFSET = 20f;
            const float FIELDS_CAPTION_WIDTH = 175f;
            const float COMPRESSED_SLIDER_WIDHT = 200f;

            EditorGUILayout.BeginHorizontal();
            _enableContent = EditorGUILayout.Foldout(_enableContent, "Audio configuration");
            EditorGUILayout.EndHorizontal();

            if (_enableContent)
            {
                // Compressed music and sound blocks
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(CONTENT_OFFSET);
                EditorGUILayout.LabelField("Compressed music volume", GUILayout.Width(FIELDS_CAPTION_WIDTH));
                this.CompressedMusicVolume = EditorGUILayout.Slider(this.CompressedMusicVolume, 0f, 1f, GUILayout.Width(COMPRESSED_SLIDER_WIDHT));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(CONTENT_OFFSET);
                EditorGUILayout.LabelField("Compressed sound volume", GUILayout.Width(FIELDS_CAPTION_WIDTH));
                this.CompressedSoundVolume = EditorGUILayout.Slider(this.CompressedSoundVolume, 0f, 1f, GUILayout.Width(COMPRESSED_SLIDER_WIDHT));
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(6f);

                // Sound behaviour block
                // TODO: поведение звуков с одинаковым приоритетом (прерывает, если нет свободных, или пропускает)
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(CONTENT_OFFSET);
                EditorGUILayout.LabelField("Max sound source count", GUILayout.Width(FIELDS_CAPTION_WIDTH));
                //this.MaxSoundSources = EditorGUILayout.IntField(Mathf.Clamp(this.MaxSoundSources, 1, 10), GUILayout.Width(80f));
                this.MaxSoundSources = EditorGUILayout.IntSlider(this.MaxSoundSources, 1, 10, GUILayout.Width(COMPRESSED_SLIDER_WIDHT));
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(6f);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(CONTENT_OFFSET);
                EditorGUILayout.LabelField("Music fade mode", GUILayout.Width(FIELDS_CAPTION_WIDTH));
                this.FadeMode = (FadeMods)EditorGUILayout.EnumPopup(this.FadeMode, GUILayout.Width(COMPRESSED_SLIDER_WIDHT));
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10f);
            }
        }
#endif
    }
}
