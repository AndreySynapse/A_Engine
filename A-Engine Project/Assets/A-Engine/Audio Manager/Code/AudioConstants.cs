// A-Engine, Code version: 1

using AEngine.Parser;

namespace AEngine.Audio
{
    public class AudioConstants
	{
        public const string DIRECTORY = "A-Engine/Audio";
        public const string FILE_NAME = "AudioConfiguration";
        public const string EXTENSION = "xml";

        public const string CODE_PARSER_FILE_NAME = "AudioNames.cs";
        public const string CODE_PARSER_THEMES_ENUM = "AudioThemes";
        public const string CODE_PARSER_SOUNDS_ENUM = "Sounds";
        public const string CODE_PARSER_MUSICS_ENUM = "Musics";

        public const string XML_ROOT = "AudioData";
        public const string XML_RUNTIME_TAG = "RuntimeChangableSettings";
        public const string XML_FADE_TAG = "Fade";
        public const string XML_ATTRIBUTE_USE_MUSIC = "UseMusic";
        public const string XML_ATTRIBUTE_USE_SOUND = "UseSound";
        public const string XML_ATTRIBUTE_MUSIC_VOLUME = "MusicVolume";
        public const string XML_ATTRIBUTE_SOUND_VOLUME = "SoundVolume";
        public const string XML_ATTRIBUTE_MUSIC_COMPRESSED_VOLUME = "MusicCompressedVolume";
        public const string XML_ATTRIBUTE_SOUND_COMPRESSED_VOLUME = "SoundCompressedVolume";
        public const string XML_ATTRIBUTE_SOUND_SOURCES_COUNT = "SoundSourceCount";
        public const string XML_ATTRIBUTE_FADE_MODE = "FadeMode";
        public const string XML_ATTRIBUTE_FADE_DURATION = "FadeDuration";
        
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