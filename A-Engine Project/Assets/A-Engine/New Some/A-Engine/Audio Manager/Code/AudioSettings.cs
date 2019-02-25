// A-Engine, Code version: 1

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using AEngine.Parser;
using System.Xml;
using System.IO;

namespace AEngine.Audio
{
	public class AudioSettings
	{
        public bool IsMusic { get; set; }
        public bool IsSound { get; set; }

        public AudioSettings()
        {
            this.IsMusic = true;
            this.IsSound = true;
        }

        #region Interface
        public void ReadData(string path)
        {
            XmlDocument document = XmlParser.LoadFromResources(path);
        }

        public void WriteData(string path)
        {
            string directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            XmlDocument document = new XmlDocument();

            XmlNode root = XmlParser.CreateRootTag(document, "AudioSettings");

            document.Save(path);
        }
        #endregion

#if UNITY_EDITOR
        #region Interface
        public void DrawGui()
        {
            EditorGUILayout.LabelField("Use music");
        }
        #endregion
#endif
    }
}