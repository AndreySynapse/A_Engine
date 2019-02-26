// A-Engine, Code version: 1

using AEngine.Parser;

namespace AEngine.Audio
{
    public class AudioConstants
	{
        public const string DIRECTORY = "A-Engine/Audio";
        public const string FILE_NAME = "AudioConfiguration";
        public const string EXTENSION = "xml";

        public const string XML_ROOT = "AudioData";

        #region Interface
        public static string GetCachePath()
        {
            return FilePath.GetPath(LocationKinds.Cache, OperationKinds.Write, DIRECTORY, FILE_NAME, EXTENSION);
        }

        public static string GetResourcesPath()
        {
            return FilePath.GetPath(LocationKinds.Resources, OperationKinds.Write, DIRECTORY, FILE_NAME, EXTENSION);
        }

        public static string GetReadableRuntimeResourcesPath()
        {
            return FilePath.GetPath(LocationKinds.Resources, OperationKinds.Read, DIRECTORY, FILE_NAME, EXTENSION);
        }
        #endregion
	}
}