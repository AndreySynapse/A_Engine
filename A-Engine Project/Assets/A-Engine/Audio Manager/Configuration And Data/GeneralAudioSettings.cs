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
        
        public GeneralAudioSettings()
        {
            this.CompressedMusicVolume = 1f;
            this.CompressedSoundVolume = 1f;

            _enableContent = true;
        }

        public void Load(XmlNode target)
        {
            this.CompressedMusicVolume = float.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_MUSIC_COMPRESSED_VOLUME].Value);
            this.CompressedSoundVolume = float.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_SOUND_COMPRESSED_VOLUME].Value);
        }

        public void Save(XmlDocument xmlDocument, XmlNode target)
        {
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_MUSIC_COMPRESSED_VOLUME, this.CompressedMusicVolume.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_SOUND_COMPRESSED_VOLUME, this.CompressedSoundVolume.ToString());
        }

#if UNITY_EDITOR
        public void DrawGUI()
        {
            const float CONTENT_OFFSET = 20f;
            const float COMPRESSED_CAPTION_WIDTH = 175f;
            const float COMPRESSED_SLIDER_WIDHT = 200f;

            EditorGUILayout.BeginHorizontal();
            _enableContent = EditorGUILayout.Foldout(_enableContent, "Audio configuration");
            EditorGUILayout.EndHorizontal();

            if (_enableContent)
            {
                // Compressed music and sound blocks
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(CONTENT_OFFSET);
                EditorGUILayout.LabelField("Compressed music volume", GUILayout.Width(COMPRESSED_CAPTION_WIDTH));
                this.CompressedMusicVolume = EditorGUILayout.Slider(this.CompressedMusicVolume, 0f, 1f, GUILayout.Width(COMPRESSED_SLIDER_WIDHT));
                EditorGUILayout.EndHorizontal();
                
                GUILayout.Space(3f);
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(CONTENT_OFFSET);
                EditorGUILayout.LabelField("Compressed sound volume", GUILayout.Width(COMPRESSED_CAPTION_WIDTH));
                this.CompressedSoundVolume = EditorGUILayout.Slider(this.CompressedSoundVolume, 0f, 1f, GUILayout.Width(COMPRESSED_SLIDER_WIDHT));
                EditorGUILayout.EndHorizontal();
            }
        }
#endif
    }
}
