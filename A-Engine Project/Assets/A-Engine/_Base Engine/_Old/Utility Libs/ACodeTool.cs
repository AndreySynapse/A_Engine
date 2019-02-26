// A-Engine, Code version: 1

using UnityEngine;
using System.Collections;
using System.IO;

namespace AEngine
{
	public static class ACodeTool
	{
        private const string ENGINE_NAME = "A-Engine";

        public static string GetEngineRootDirectory(bool shortUnityPath)
        {
            string[] directories = Directory.GetDirectories(Application.dataPath, ENGINE_NAME, SearchOption.AllDirectories);

            if (directories == null || directories.Length == 0)
            {
                Debug.Log("Couldn't fine A-Engine root directory");
                return null;
            }

            string result = directories[0];

            if (shortUnityPath && !string.IsNullOrEmpty(result))
            {
                result = result.Substring(result.IndexOf("Assets")) + "/";
            }

            return result;            
        }

		public static string GetEngineMenuRootDirectory(bool useShortVariation)
		{
			string[] directories = Directory.GetDirectories(Application.dataPath, ENGINE_NAME, SearchOption.AllDirectories);

			if (directories.IsNullOrEmpty())
			{
				Debug.Log("Couldn't find root directory: ACodeTool.GetEngineRootDirectory");
				return null;
			}

			string result = null;
			foreach (string dir in directories)
			{
				if (Directory.Exists(dir + "/Menu Manager"))
				{
					result = dir;
					break;
				}
			}

			if (useShortVariation && !string.IsNullOrEmpty(result))
			{
				result = result.Substring(result.IndexOf("Assets")) + "/";
			}

			return result;
		}
	}
}