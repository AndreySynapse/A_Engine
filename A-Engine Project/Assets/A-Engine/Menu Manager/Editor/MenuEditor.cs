// A-Engine, Code version: 2

using UnityEngine;
using UnityEditor;

namespace AEngine.Menu
{
	[CustomEditor(typeof(Menu))]
	[System.Serializable]
	public class MenuEditor : Editor
	{
		private const float SMALL_VERTICAL_OFFSET = 5f;
		private const float LARGE_VERTICAL_OFFSET = 9f;
		private const float SMALL_WIDTH = 80f;
		private const float LARGE_WIDTH = 150f;

		private Menu _current;

		void OnEnable()
		{
			_current = (Menu)target;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GUILayout.Space(SMALL_VERTICAL_OFFSET);			
			DrawMenuView(_current);
			
			this.SaveGuiChanges();
		}

		public static void DrawMenuView(Menu menu)
		{
			menu.RefreshViewPanels();

			if (menu.DefaultSettings.AttachUI)
			{
				AUITool.AttachRectTransform(menu.gameObject);
				if (menu.HorizontalPanel != null)
				{
					AUITool.AttachRectTransform(menu.HorizontalPanel);
				}
				if (menu.VerticalPanel != null)
				{
					AUITool.AttachRectTransform(menu.VerticalPanel);
				}
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Orientation:", GUILayout.Width(SMALL_WIDTH));
			menu.OrientationStyle = (EMenuOrientationStyle)EditorGUILayout.EnumPopup(menu.OrientationStyle, GUILayout.Width(LARGE_WIDTH));
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(SMALL_VERTICAL_OFFSET);

			switch (menu.OrientationStyle)
			{
				case EMenuOrientationStyle.Both:
					DrawOrientationBlock(menu, menu.DefaultSettings.RealOrientation);
					DrawOrientationBlock(menu, menu.DefaultSettings.RealOrientation == ERealOrientation.Horizontal ? ERealOrientation.Vertical : ERealOrientation.Horizontal);
					break;
					
				case EMenuOrientationStyle.Horizontal:					
					DrawOrientationBlock(menu, ERealOrientation.Horizontal);
					menu.CurrentOrientation = ERealOrientation.Horizontal;
					break;
					
				case EMenuOrientationStyle.Vertical:					
					DrawOrientationBlock(menu, ERealOrientation.Vertical);
					menu.CurrentOrientation = ERealOrientation.Vertical;
					break;
			}

			if (menu.HorizontalPanel != null)
			{
				if (menu.HorizontalPanel.layer != menu.gameObject.layer)
				{
					menu.HorizontalPanel.layer = menu.gameObject.layer;
				}
			}
			if (menu.VerticalPanel != null)
			{
				if (menu.VerticalPanel.layer != menu.gameObject.layer)
				{
					menu.VerticalPanel.layer = menu.gameObject.layer;
				}
			}
		}

		private static void DrawOrientationBlock(Menu menu, ERealOrientation orientation)
		{
			EditorGUILayout.BeginHorizontal();
			
			GameObject panel = orientation == ERealOrientation.Horizontal ? menu.HorizontalPanel : menu.VerticalPanel;
			MenuView view = orientation == ERealOrientation.Horizontal ? menu.HorizontalView : menu.VerticalView;
			
			if (panel == null)
			{
				panel = AGameObjectTool.CreateGameObject(orientation == ERealOrientation.Horizontal ? "Horizontal Panel" : "Vertical Panel", menu.gameObject);
			}
			
			EditorGUILayout.LabelField (orientation == ERealOrientation.Horizontal ? "Horizontal:" : "Vertical:", GUILayout.Width(SMALL_WIDTH));
			
			if (view == null)
			{
				view = panel.GetComponent<MenuView>();
			}
			
			if (view == null)
			{
				panel = (GameObject)EditorGUILayout.ObjectField(panel, typeof(GameObject), true);
			}
			else
			{
				view = (MenuView)EditorGUILayout.ObjectField(view, typeof(MenuView), true);
			}

			if (orientation == ERealOrientation.Horizontal)
			{
				menu.HorizontalPanel = panel;
				menu.HorizontalView = view;
			}
			else
			{
				menu.VerticalPanel = panel;
				menu.VerticalView = view;
			}
			
			if (menu.OrientationStyle != EMenuOrientationStyle.Both)
			{
				GameObject unusedPanel = orientation == ERealOrientation.Horizontal ? menu.VerticalPanel : menu.HorizontalPanel;
				if (unusedPanel != null)
				{
					DestroyImmediate(unusedPanel);
				}
			}
			
			EditorGUILayout.EndHorizontal();
		}
	}
}