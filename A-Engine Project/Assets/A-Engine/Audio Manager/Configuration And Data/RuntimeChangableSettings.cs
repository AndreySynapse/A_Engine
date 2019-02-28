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
    public class RuntimeChangableSettings
    {
        private bool _enableContent;

        public bool UseMusic { get; set; }
        public bool UseSound { get; set; }
        public float MusicVolume { get; set; }
        public float SoundVolume { get; set; }

        public RuntimeChangableSettings()
        {
            this.UseMusic = true;
            this.UseSound = true;
            this.MusicVolume = 1f;
            this.SoundVolume = 1f;

            _enableContent = true;
        }

        public void Load(XmlNode target)
        {
            this.UseMusic = bool.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_USE_MUSIC].Value);
            this.UseSound = bool.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_USE_SOUND].Value);
            this.MusicVolume = float.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_MUSIC_VOLUME].Value);
            this.SoundVolume = float.Parse(target.Attributes[AudioConstants.XML_ATTRIBUTE_SOUND_VOLUME].Value);
        }

        public void Save(XmlDocument xmlDocument, XmlNode target)
        {
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_USE_MUSIC, this.UseMusic.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_USE_SOUND, this.UseSound.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_MUSIC_VOLUME, this.MusicVolume.ToString());
            XmlParser.AddAttribute(xmlDocument, target, AudioConstants.XML_ATTRIBUTE_SOUND_VOLUME, this.SoundVolume.ToString());
        }

#if UNITY_EDITOR
        public void Draw()
        {
            const float CONTENT_OFFSET = 20f;
            const float TOOGLE_CAPTION_WIDHT = 75f;
            const float TOOGLE_WIDHT = 30f;
            const float SPACE_TO_INPUT_SLIDER = 65f;
            const float INPUT_SLIDER_CAPTION_WIDTH = 90f;
            const float INPUT_SLIDER_WIDHT = 200f;

            EditorGUILayout.BeginHorizontal();
            _enableContent = EditorGUILayout.Foldout(_enableContent, "Runtime changable settings");
            EditorGUILayout.EndHorizontal();

            if (_enableContent)
            {
                // Music block
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(CONTENT_OFFSET);

                EditorGUILayout.LabelField("Use music", GUILayout.Width(TOOGLE_CAPTION_WIDHT));
                this.UseMusic = EditorGUILayout.Toggle(this.UseMusic, GUILayout.Width(TOOGLE_WIDHT));
                GUILayout.Space(SPACE_TO_INPUT_SLIDER);
                EditorGUILayout.LabelField("Music volume", GUILayout.Width(INPUT_SLIDER_CAPTION_WIDTH));
                this.MusicVolume = EditorGUILayout.Slider(this.MusicVolume, 0f, 1f, GUILayout.Width(INPUT_SLIDER_WIDHT));

                EditorGUILayout.EndHorizontal();
                
                // Sound block
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(CONTENT_OFFSET);

                EditorGUILayout.LabelField("Use sound", GUILayout.Width(TOOGLE_CAPTION_WIDHT));
                this.UseSound = EditorGUILayout.Toggle(this.UseSound, GUILayout.Width(TOOGLE_WIDHT));
                GUILayout.Space(SPACE_TO_INPUT_SLIDER);
                EditorGUILayout.LabelField("Sound volume", GUILayout.Width(INPUT_SLIDER_CAPTION_WIDTH));
                this.SoundVolume = EditorGUILayout.Slider(this.SoundVolume, 0f, 1f, GUILayout.Width(INPUT_SLIDER_WIDHT));

                EditorGUILayout.EndHorizontal();
                
                GUILayout.Space(10f);
            }
        }
#endif
    }
}
