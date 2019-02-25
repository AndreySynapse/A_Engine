// A-Engine, Code version: 1

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace AEngine.EditorTools
{
    public static class EditorExtensions
    {
        public static void SaveChanges(this Editor editor)
        {
            if (!Application.isPlaying && GUI.changed)
            {
                EditorUtility.SetDirty(editor.target);

                editor.serializedObject.SetIsDifferentCacheDirty();
                editor.serializedObject.ApplyModifiedProperties();

                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}
#endif