﻿using System.Xml;
using System.IO;
using AEngine.Parser;

using Debug = UnityEngine.Debug;

namespace AEngine.Audio
{
    public class AudioDataParser
    {
        public static XmlDocument Load()
        {
            XmlDocument xmlDocument;

            string path = AudioConstants.GetFullConfigurationPath();
            CheckDirectory(Path.GetDirectoryName(path));
            
            if (!File.Exists(path))
            {
                xmlDocument = new XmlDocument();

                XmlNode root = XmlParser.CreateRootTag(xmlDocument, "AudioData");
                xmlDocument.Save(path);
            }
            
            xmlDocument = XmlParser.LoadFromFile(AudioConstants.GetFullConfigurationPath());
                                    
            return xmlDocument;
        }

        public void Save(XmlDocument xmlDocument)
        {
            string path = Path.GetDirectoryName(AudioConstants.GetFullConfigurationPath());

            CheckDirectory(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //xmlDocument.Save();
        }

        private static void CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}