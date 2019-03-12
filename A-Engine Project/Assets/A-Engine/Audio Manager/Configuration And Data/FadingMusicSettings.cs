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
    public class FadingMusicSettings
    {
        private bool _enableContent;

        private string[] _fadeInOptions;
        private int _index;
        
        public FadingBlock PlayMusicFading { get; set; }

        public FadeMods PlayFadeMode { get; set; }
        public float PlayFadeDuration { get; set; }
        bool UsePlayClampTrackLength { get; set; }
        public float PlayClampMaxTrackLength { get; set; }

        public FadingMusicSettings()
        {
            _fadeInOptions = new string[] { FadeMods.NotFading.ToString(), FadeMods.FadeIn.ToString() };

            this.PlayMusicFading = new FadingBlock();

            this.PlayFadeMode = FadeMods.NotFading;
            this.PlayFadeDuration = 0.4f;
            this.UsePlayClampTrackLength = false;
            this.PlayClampMaxTrackLength = 0.05f;
            
            _enableContent = true;
        }

        public void Load(XmlNode target)
        {
            XmlNode fadeTag = XmlParser.GetChildTag(target, AudioConstants.XML_FADE_TAG);
            this.PlayFadeMode = (FadeMods)Enum.Parse(typeof(FadeMods), fadeTag.Attributes[AudioConstants.XML_ATTRIBUTE_FADE_MODE].Value);
            this.PlayFadeDuration = float.Parse(fadeTag.Attributes[AudioConstants.XML_ATTRIBUTE_FADE_DURATION].Value);
        }

        public void Save(XmlDocument xmlDocument, XmlNode target)
        {
            XmlNode fadeTag = XmlParser.CreateChildTag(xmlDocument, target, AudioConstants.XML_FADE_TAG);
            XmlParser.AddAttribute(xmlDocument, fadeTag, AudioConstants.XML_ATTRIBUTE_FADE_MODE, this.PlayFadeMode.ToString());
            XmlParser.AddAttribute(xmlDocument, fadeTag, AudioConstants.XML_ATTRIBUTE_FADE_DURATION, this.PlayFadeDuration.ToString());
        }

#if UNITY_EDITOR
        const float CONTENT_OFFSET = 20f;
        
        public void DrawGUI()
        {
            EditorGUILayout.BeginHorizontal();
            _enableContent = EditorGUILayout.Foldout(_enableContent, "Music fading configuration");
            EditorGUILayout.EndHorizontal();
            
            if (_enableContent)
            {
                this.PlayMusicFading.Draw("PlayMusic fading behaviour", CONTENT_OFFSET);
            }
        }
#endif
    }
}