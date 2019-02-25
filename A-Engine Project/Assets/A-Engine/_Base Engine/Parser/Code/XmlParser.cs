// A-Engine, Code version: 1

using UnityEngine;
using System.Collections.Generic;
using System.Xml;

namespace AEngine.Parser
{
    public class XmlParser
	{
        #region Interface
        
        /// <summary>
        /// Load xml document from file.
        /// </summary>
        /// <param name="path">Full path to the file. Use FilePath.GetPath for it.</param>
        /// <returns></returns>
        public static XmlDocument LoadFromFile(string path)
        {
            XmlDocument document = new XmlDocument();
            
            document.Load(path);
            
            return document;
        }

        /// <summary>
        /// Load xml document from resources.
        /// </summary>
        /// <param name="path">Path to the file from Resources location. Use FilePath.GetPath for it.</param>
        /// <returns></returns>
        public static XmlDocument LoadFromResources(string path)
        {
            XmlDocument document = new XmlDocument();

            TextAsset textAsset = Resources.Load<TextAsset>(path);
            document.LoadXml(textAsset.text);

            return document;
        }

        public static XmlNode CreateRootTag(XmlDocument document, string tagName)
        {
            XmlNode targetTag = document.CreateElement(tagName);

            targetTag.RemoveAll();
            document.AppendChild(targetTag);
            
            return targetTag;
        }

        public static XmlNode CreateChildTag(XmlDocument document, XmlNode parentTag, string tagName)
        {
            XmlNode targetTag = document.CreateElement(tagName);
            
            parentTag.AppendChild(targetTag);

            return targetTag;
        }

        public static void AddAttribute(XmlDocument document, XmlNode tag, string attributeName, string attributeValue)
        {
            XmlAttribute attribute = document.CreateAttribute(attributeName);
            attribute.Value = attributeValue;

            tag.Attributes.Append(attribute);
        }
        
        public static bool IsExistRootTag(XmlDocument document, string tagName)
        {
            XmlNodeList tagList = document.GetElementsByTagName(tagName);
            
            return !(tagList == null || tagList.Equals(null) || tagList.Count == 0);
        }

        public static XmlNode GetRootTag(XmlDocument document, string tagName)
        {
            XmlNodeList tagList = document.GetElementsByTagName(tagName);

            if (tagList.Count > 1)
            {
                Debug.LogError(string.Format("Not correct xml document. More than one root tag [{0}] found.", tagName));
            }

            const int DEFAULT_ITEM = 0;

            return tagList[DEFAULT_ITEM];
        }

        public static bool IsExistChildTag(XmlNode parentNode, string tagName)
        {
            foreach (XmlNode itemNode in parentNode.ChildNodes)
            {
                if (itemNode.Name.Equals(tagName))
                {
                    return true;
                }
            }

            return false;
        }

        public static XmlNode GetChildTag(XmlNode parentNode, string tagName)
        {
            XmlNode targetNode = null;

            foreach (XmlNode itemNode in parentNode.ChildNodes)
            {
                if (itemNode.Name.Equals(tagName))
                {
                    targetNode = itemNode;
                    break;
                }
            }

            return targetNode;
        }

        public static List<XmlNode> GetChildTags(XmlNode parentNode, string tagName)
        {
            List<XmlNode> xmlTagList = new List<XmlNode>();

            foreach (XmlNode itemNode in parentNode.ChildNodes)
            {
                if (itemNode.Name.Equals(tagName))
                {
                    xmlTagList.Add(itemNode);
                }
            }
            
            return xmlTagList.Count == 0 ? null : xmlTagList;
        }

        public static XmlNode GetChildTagByAttribute(XmlNode parentNode, string tagName, string attributeName, string attributeValue)
        {
            XmlNode targetNode = null;

            foreach (XmlNode item in parentNode.ChildNodes)
            {
                XmlAttribute attribute = item.Attributes[attributeName];
                                
                if (attribute != null && attribute.Value != null && attribute.Value == attributeValue)
                {
                    targetNode = item;
                    break;
                }
            }

            return targetNode;
        }
        
        public static List<XmlNode> GetChildTagsByAttribute(XmlNode parendNode, string tagName, string attributeName, string attributeValue)
        {
            List<XmlNode> tagList = new List<XmlNode>();

            foreach (XmlNode item in parendNode.ChildNodes)
            {
                XmlAttribute attribute = item.Attributes[attributeName];

                if (attribute != null && attribute.Value != null && attribute.Value == attributeValue)
                {
                    tagList.Add(item);
                }
            }

            return tagList;
        }
        #endregion
    }
}