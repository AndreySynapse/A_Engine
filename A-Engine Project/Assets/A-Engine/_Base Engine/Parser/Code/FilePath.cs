// A-Engine, Code version: 1

using UnityEngine;

namespace AEngine.Parser
{
    public enum LocationKinds
    {
        FreeLocation,
        Cache,
        Resources,
        Assets     
    }

    public enum OperationKinds
    {
        Read,
        Write
    }

    public static class FilePath
	{
        #region Interface

        /// <summary>
        /// Get file location path.
        /// </summary>
        /// <param name="location">File location. FreeLocation - is the full system path or path from project root to the file.</param>
        /// <param name="operation">Read or write file mode.</param>
        /// <param name="directory">Short path to the file from location.</param>
        /// <param name="shortFileName">File name without extension.</param>
        /// <param name="extension">File extension.</param>
        /// <returns></returns>
        public static string GetPath(LocationKinds location, OperationKinds operation, string directory, string shortFileName, string extension)
        {
            directory = string.IsNullOrEmpty(directory) ? "" : (directory.EndsWith("/") || directory.EndsWith("\\") ? directory : directory + "/");

            shortFileName = string.IsNullOrEmpty(shortFileName) ? "" : shortFileName;

            extension = string.IsNullOrEmpty(extension) ? "" : (extension.StartsWith(".") ? extension : "." + extension);

            string path = directory + shortFileName + extension;

            switch (location)
            {
                case LocationKinds.Cache:
                    path = Application.persistentDataPath + "/" + path;
                    break;

                case LocationKinds.Resources:
                    path = operation == OperationKinds.Read ? directory + shortFileName : Application.dataPath + "/Resources/" + path;
                    break;

                case LocationKinds.Assets:
                    path = Application.dataPath + "/" + path;
                    break;
            }

            return path;
        }
        #endregion
    }
}