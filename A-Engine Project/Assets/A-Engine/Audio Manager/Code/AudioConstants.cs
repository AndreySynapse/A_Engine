// A-Engine, Code version: 1

using AEngine.Parser;

namespace AEngine.Audio
{
    public class AudioConstants
	{
        public const string CONFIGURATION_DIRECTORY = "A-Engine/Audio";
        public const string CONFIGURATION_FILE_NAME = "AudioConfiguration";
        public const string EXTENSION = "xml";

        #region Interface
        public static string GetReadableRuntimeConfigurationPath()
        {
            return FilePath.GetPath(LocationKinds.Resources, OperationKinds.Read, CONFIGURATION_DIRECTORY, CONFIGURATION_FILE_NAME, EXTENSION);
        }

        public static string GetFullConfigurationPath()
        {
            return FilePath.GetPath(LocationKinds.Resources, OperationKinds.Write, CONFIGURATION_DIRECTORY, CONFIGURATION_FILE_NAME, EXTENSION);
        }
        #endregion
	}
}