// A-Engine, Code version: 2

using UnityEngine;

namespace AEngine.Menu
{
	[ExecuteInEditMode]
	public class Menu : MonoBehaviour 
	{
	#region Unity Editor
		[SerializeField] private DefaultMenuSettings _defaultMenuSettings;
		[SerializeField] private EMenuOrientationStyle _orientationStyle;
		[SerializeField] private ERealOrientation _currentOrientation;

		[SerializeField] private GameObject _horizontalPanel;
		[SerializeField] private GameObject _verticalPanel;
		[SerializeField] private MenuView _horizontalView;
		[SerializeField] private MenuView _verticalView;
	#endregion
		
		public DefaultMenuSettings DefaultSettings { get { return _defaultMenuSettings; } set { _defaultMenuSettings = value; } }

		public EMenuOrientationStyle OrientationStyle { get { return _orientationStyle; }  set { _orientationStyle = value; } }

		public ERealOrientation CurrentOrientation { get { return _currentOrientation; } set { _currentOrientation = value; RefreshViewPanels(); } }

		public GameObject HorizontalPanel { get { return _horizontalPanel; } set { _horizontalPanel = value; } }

		public GameObject VerticalPanel { get { return _verticalPanel; } set { _verticalPanel = value; } }

		public MenuView HorizontalView { get { return _horizontalView; } set { _horizontalView = value; } }

		public MenuView VerticalView { get { return _verticalView; } set { _verticalView = value; } }

		private ERealOrientation ScreenOrientation { get { return (Screen.width >= Screen.height) ? ERealOrientation.Horizontal : ERealOrientation.Vertical; } }

		void Awake()
		{
			RefreshViewPanels();
		}

		void LateUpdate()
		{
			if (_orientationStyle == EMenuOrientationStyle.Both && _currentOrientation != ScreenOrientation)
			{
				MenuView activeView = (_currentOrientation == ERealOrientation.Horizontal) ? _horizontalView : _verticalView;
				MenuView passiveView = (_currentOrientation == ERealOrientation.Horizontal) ? _verticalView : _horizontalView;
				
				if (activeView != null)
				{
					activeView.OnChangeScreenOrientation(MenuView.EMenuViewState.HideView);				
				}
				
				_currentOrientation = ScreenOrientation;
				
				if (passiveView != null)
				{
					passiveView.OnChangeScreenOrientation(MenuView.EMenuViewState.ShowView);
				}
				
				RefreshViewPanels();
			}
		}

		public void ShowMenu()
		{
			this.gameObject.SetActive(true);
			
			if (Application.isPlaying)
			{
				switch (OrientationStyle)
				{
					case EMenuOrientationStyle.Both:
						Screen.orientation = UnityEngine.ScreenOrientation.AutoRotation;
						break;
						
					case EMenuOrientationStyle.Horizontal:
						Screen.orientation = UnityEngine.ScreenOrientation.Landscape;
						break;
						
					case EMenuOrientationStyle.Vertical:
						Screen.orientation = UnityEngine.ScreenOrientation.Portrait;
						break;
				}
			}
			
			RefreshViewPanels();
			
			MenuView currentView = _currentOrientation == ERealOrientation.Horizontal ? _horizontalView : _verticalView;
			if (currentView != null)
			{
				currentView.OnShowMenu();
			}
		}

		public void HideMenu()
		{
			MenuView currentView = _currentOrientation == ERealOrientation.Horizontal ? _horizontalView : _verticalView;
			if (currentView != null)
			{
				currentView.OnHideMenu();
			}
			
			gameObject.SetActive(false);
		}

		public void RefreshViewPanels()
		{
			if (_verticalPanel != null)
			{
				_verticalPanel.SetActive(_currentOrientation == ERealOrientation.Vertical);
			}
            
            if (_horizontalPanel != null)
			{
				_horizontalPanel.SetActive(_currentOrientation == ERealOrientation.Horizontal);
			}
		}
	}
}