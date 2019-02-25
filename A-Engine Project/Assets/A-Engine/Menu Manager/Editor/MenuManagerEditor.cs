// A-Engine, Code version: 2

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using AEngine;

namespace AEngine.Menu
{
	[CustomEditor(typeof(MenuManager))]
	[Serializable]
	public class MenuManagerEditor : Editor 
	{
		private MenuManager _current;
		private List<string> _namesList;

		private Texture2D _btnUpTex;
		private Texture2D _btnDownTex;

		private SortMenuModule _sortModule;

		void OnEnable ()
		{
			_current = (MenuManager)target;

			if (_current.DefaultSettings == null)
			{
				_current.DefaultSettings = Editor.CreateInstance<DefaultMenuSettings>();
			}

			if (_current.MenuList == null)
			{
				_current.MenuList = new List<Menu>();
			}

			if (_namesList == null)
			{
				_namesList = new List<string>();
			}

			UpdateNames();

			if (_btnUpTex == null)
			{
				_btnUpTex = AssetDatabase.LoadAssetAtPath<Texture2D>(ACodeTool.GetEngineRootDirectory(true) + "Menu Manager/Resources/Textures/up.png") as Texture2D;
			}
			if (_btnDownTex == null) 
			{
				_btnDownTex = AssetDatabase.LoadAssetAtPath<Texture2D>(ACodeTool.GetEngineRootDirectory(true) + "Menu Manager/Resources/Textures/down.png") as Texture2D;
			}

			if (_sortModule == null)
			{
				_sortModule = new SortMenuModule();
			}

			_sortModule.Reset();
		}

		override public void OnInspectorGUI ()
		{
			serializedObject.Update();

			GUILayout.Space(AEditorValues.LARGE_OFFSET);

			_current.DefaultSettings.DrawInspector();
			SortAllMenuOrientation();

			if (_current.DefaultSettings.AttachUI)
			{
				AUITool.AttachRectTransform(_current.gameObject);
			}

			GUILayout.Space(AEditorValues.MIN_OFFSET);

			AEditorTool.DrawSeparator(AEditorTool.SeparationStyle.Default);

			GUILayout.Space (AEditorValues.LARGE_OFFSET);
			AEditorTool.DrawListButtons(_current.MenuList, "Add new menu", AddMenu, "Clear menu list", ClearMenuList);
			GUILayout.Space (AEditorValues.LARGE_OFFSET);

			_current.MenuList = _current.MenuList.Where(x => x != null).ToList();

			UpdateNames();

			for (int i = 0; i < _current.Count; i++)
			{
				DrawMenuView(_current[i], i);
			}

			_sortModule.SortInvoke(_current.MenuList);
			_current.UpdateActiveMenuState ();

			AEditorTool.DrawSeparator(AEditorTool.SeparationStyle.Default);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Save menu names in enum " + BaseEngineConstants.MenuDataEnumForMenu);
			EditorGUILayout.EndHorizontal();

			AEditorTool.DrawListButtons(_current.MenuList, "Clear all names", ClearMenuData, "Apply menu names", SaveMenuData);

			this.SaveGuiChanges();
		}

		private void AddMenu()
		{
			Menu menu = (new GameObject(GenerateUniqueMenuName("Menu"))).AddComponent<Menu>();
			SortMenuOrientation(menu, true);

			_current.gameObject.AttachChild(menu.gameObject);
			_current.MenuList.Add(menu);

			UpdateNames();
		}

		private void ClearMenuList()
		{
			foreach (Menu menu in _current.MenuList)
			{
				DestroyImmediate(menu.gameObject);
			}

			_current.MenuList.Clear();
		}

		private void DrawMenuView(Menu menu, int index)
		{
			AEditorTool.DrawSeparator(index == 0 ? AEditorTool.SeparationStyle.Default : AEditorTool.SeparationStyle.SmallLight);

			if (menu.DefaultSettings == null)
			{
				menu.DefaultSettings = _current.DefaultSettings;
			}

			float size = Screen.width / 2f - 32f;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Menu:",  GUILayout.Width(size));
			EditorGUILayout.LabelField("Name:", GUILayout.Width(size));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal ();
			menu = (Menu)EditorGUILayout.ObjectField(menu, typeof(Menu), true, GUILayout.Width(size));
			menu.name = EditorGUILayout.TextField(menu.name, GUILayout.Width(size));
			menu.name = new string(menu.name.Where(c => char.IsLetterOrDigit(c)).ToArray()); 
			_namesList[index] = menu.name;

			if (GUILayout.Button ("-", GUILayout.Width(AEditorValues.MIN_SIZE))) 
			{
				_current.MenuList.RemoveAt(index);
				DestroyImmediate(menu.gameObject);
				UpdateNames();
				return;
			}
			EditorGUILayout.EndHorizontal ();

			if (GetNamesCount(menu.name) > 1)
			{
				this.DrawColorLabel(new Color (1f, 0.35f, 0.35f), string.Format("Already used menu with name {0}", menu.name));
			}

			GUILayout.Space(AEditorValues.OFFSET);

			const float ACTIVE_BUTTON_WIDTH = 35f;
			const float TOOGLE_WIDTH = 40f;
			EditorGUILayout.BeginHorizontal ();
			if (menu.gameObject.activeSelf)
			{
				this.DrawColorLabel(Color.green, "ON", GUILayout.Width(ACTIVE_BUTTON_WIDTH));
			}
			else
			{
				EditorGUILayout.LabelField("OFF", GUILayout.Width(ACTIVE_BUTTON_WIDTH));
			}

			if (EditorGUILayout.Toggle(menu.gameObject.activeSelf, GUILayout.Width(TOOGLE_WIDTH)))
			{
				_current.ActiveMenu = menu;
			}

			EditorGUILayout.LabelField(" ", GUILayout.Width(size - ACTIVE_BUTTON_WIDTH - TOOGLE_WIDTH));
			if (index != 0) 
			{
				if (GUILayout.Button (_btnUpTex, GUILayout.Width (30))) 
				{
					_sortModule.ChangeSortState(SortMenuModule.ESortKind.Up, index);
				}
			}
			if (index < _current.Count - 1) 
			{
				if (GUILayout.Button (_btnDownTex, GUILayout.Width (30))) 
				{
					_sortModule.ChangeSortState(SortMenuModule.ESortKind.Down, index);
				}
			}
			EditorGUILayout.EndHorizontal ();

			if (menu.gameObject.layer != _current.gameObject.layer)
				menu.gameObject.layer = _current.gameObject.layer;
			
			GUILayout.Space (AEditorValues.LARGE_OFFSET);
			MenuEditor.DrawMenuView(menu);
			GUILayout.Space (AEditorValues.LARGE_OFFSET);
		}

		private void SortAllMenuOrientation()
		{
			foreach (Menu menu in _current.MenuList)
			{
				SortMenuOrientation(menu);
			}
		}

		private void SortMenuOrientation(Menu currentMenu, bool refreshAsDefault = false)
		{
			if (currentMenu.OrientationStyle != EMenuOrientationStyle.Both)
			{
				return;
			}

			GameObject targetObject = (_current.DefaultSettings.RealOrientation == ERealOrientation.Horizontal) ? currentMenu.HorizontalPanel : currentMenu.VerticalPanel;

			if (targetObject != null)
			{
				targetObject.transform.SetAsFirstSibling();
			}

			if (refreshAsDefault)
			{
				currentMenu.CurrentOrientation = _current.DefaultSettings.RealOrientation;
				currentMenu.OrientationStyle = _current.DefaultSettings.MenuOrientationStyle;
			}
		}

	#region Menu Names
		private string GenerateUniqueMenuName (string baseName)
		{
			string result = baseName;						
			int index = 1;
			
			while (_namesList.Contains(result)) 
			{
				result = baseName + index++;
			}
			
			return result;
		}
		
		private void UpdateNames()
		{
			_namesList = _current.MenuList.Where(x => x != null && x.gameObject != null).Select(x => x.name).ToList();
		}
		
		private int GetNamesCount(string baseName)
		{
			return _namesList.Aggregate(0, (acc, x) => acc += x == baseName ? 1 : 0);
		}
	#endregion

		private void SaveMenuData()
		{
			List<string> content = new List<string>();
			for (int i = 0; i < _current.Count; i++)
			{
				content.Add(_current[i].name);
			}

			CodeManager.AddItemsToBlock(BaseEngineConstants.MenuDataFileName, "", BaseEngineConstants.MenuDataEnumForMenu, content.ToArray(), BaseEngineConstants.MenuDataEnumsOffset);
		}

		private void ClearMenuData()
		{
			CodeManager.ClearBlock(BaseEngineConstants.MenuDataFileName, "", BaseEngineConstants.MenuDataEnumForMenu, BaseEngineConstants.MenuDataEnumsOffset);
		}
	}
}