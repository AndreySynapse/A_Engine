// A-Engine, Code version: 1

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace AEngine.Menu
{
	public class DefaultMenuSettings : ScriptableObject
	{
		private const float TEXT_WIDTH = 120f;
		private const float VERTICAL_OFFSET = 5f;
		private const float WIDTH_OFFSET = 36f;
		
	#region Unity Editor
		[SerializeField] private EMenuOrientationStyle _menuOrientation;
		[SerializeField] private ERealOrientation _realOrientation;
		[SerializeField] private bool _attachUI;
	#endregion

		public EMenuOrientationStyle MenuOrientationStyle { get { return _menuOrientation; } set { _menuOrientation = value; } }

		public ERealOrientation RealOrientation { get { return _realOrientation; } set { _realOrientation = value; } }

		public bool AttachUI { get { return _attachUI; } set { _attachUI = value; } }
						
#if UNITY_EDITOR
		public void DrawInspector()
		{
			EditorGUILayout.BeginVertical();

			EditorGUILayout.LabelField("Default menu settings", EditorStyles.boldLabel);

			GUILayout.Space(VERTICAL_OFFSET);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Orientation style", GUILayout.Width(TEXT_WIDTH));
			_menuOrientation = (EMenuOrientationStyle)EditorGUILayout.EnumPopup(_menuOrientation);
			EditorGUILayout.EndHorizontal();
			
			if (_menuOrientation == EMenuOrientationStyle.Both)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Real orientation", GUILayout.Width(TEXT_WIDTH));
				_realOrientation = (ERealOrientation)EditorGUILayout.EnumPopup(_realOrientation);
				EditorGUILayout.EndHorizontal();
			}
			else
			{
				_realOrientation = _menuOrientation == EMenuOrientationStyle.Horizontal ? ERealOrientation.Horizontal : ERealOrientation.Vertical;
			}
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Attach uGUI", GUILayout.Width(TEXT_WIDTH));
			_attachUI = EditorGUILayout.Toggle(_attachUI);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();
		}
#endif
	}
}