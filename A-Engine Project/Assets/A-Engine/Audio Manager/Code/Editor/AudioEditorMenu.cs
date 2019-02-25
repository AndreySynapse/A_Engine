// A-Engine, Code version: 1

using UnityEditor;

namespace AEngine.Audio
{
    public class AudioEditorMenu : Editor
    {
        [MenuItem("A-Engine/Audio Configuration", false, EditorMenuConstants.AUDIO_EDITOR_MENU_PRIORITY)]
        private static void AudioConfigurationWindow()
        {
            EditorWindow.GetWindow<AudioConfigurationWindow>("Audio Config");
        }
    }
}