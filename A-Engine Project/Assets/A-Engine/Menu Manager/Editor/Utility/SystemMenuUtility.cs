// A-Engine, Code version: 1

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace AEngine.Menu
{
	public class SystemMenuUtility : Editor
	{
		[MenuItem("A-Engine/Create Menu Manager", false, 1)]
		public static void CreateMenuManager()
		{	
			if (GameObject.FindObjectOfType<MenuManager>() != null) 
			{
				Debug.Log ("Menu Manager already exists in this scene!");
				return;
			}
			
			GameObject manager = AGameObjectTool.CreateGameObject("Menu Manager");
			manager.AddComponent<AEngine.Menu.MenuManager>();
			
			EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		}
	}
}