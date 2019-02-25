// A-Engine, Code version: 2

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AEngine.Menu
{
	[ExecuteInEditMode]
	public class MenuManager : MonoBehaviour
	{
	#region Unity Editor
		[SerializeField] private DefaultMenuSettings _menuSettings;
		[SerializeField] private List<Menu> _menuList;
		[SerializeField] private Menu _activeMenu;
	#endregion
		
		public DefaultMenuSettings DefaultSettings { get { return _menuSettings; } set { _menuSettings = value; } }
		
		public List<Menu> MenuList { get { return _menuList; } set { _menuList = value; } }

		public Menu ActiveMenu
		{
			get { return _activeMenu; }
			set { 
				_activeMenu = value;
				try 
				{
					UpdateActiveMenuState();
					ShowActiveMenu();
				}
				catch (Exception ex) 
				{
					Debug.Log ("[Class = MenuManager, field = ActiveMenu-set] : Couldn't set acive menu, set it in null");
					Debug.Log(ex.Message);
					_activeMenu = null;
				}
			}
		}

		public Menu this[int index] { get { return _menuList[index]; } }

		public int Count { get { return _menuList.Count; } }

		public bool Contains(string menuName)
		{
			for (int i = 0; i < _menuList.Count; i++) 
			{
				if (_menuList[i].name == menuName)
				{
					return true;
				}
			}
			
			return false;
		}

		void Awake ()
		{
			UpdateActiveMenuState();

			if (Application.isPlaying) 
			{
				TransitionManager.Menu = this;
				if (!TransitionManager.ContinueTransition())
				{
					ShowActiveMenu();
				}
			}
		}

		public void ShowMenu(string name)
		{
			if (_menuList.IsNullOrEmpty())
			{
				return;
			}
			
			for (int i = 0; i < _menuList.Count; i++) 
			{
				if (_menuList[i].name == name) 
				{
					_activeMenu.HideMenu();
					ActiveMenu = _menuList[i];
					break;
				}
			}
		}

		public void UpdateActiveMenuState()
		{
			if (_menuList.IsNullOrEmpty()) 
			{
				_activeMenu = null;
				return;
			}
						
			for (int i = 0; i < _menuList.Count; i++) 
			{
				if (_activeMenu == null) 
				{
					if (_menuList[i].gameObject.activeSelf)
					{
						_activeMenu = _menuList[i];
					}
				}
				else 
				{
					_menuList[i].gameObject.SetActive(_menuList[i] == _activeMenu ? true : false);
				}
			}
			
			if (_activeMenu == null)
			{
				_activeMenu = _menuList[0];
				_menuList[0].gameObject.SetActive (true);
			}
		}

		private void ShowActiveMenu()
		{
			if (!_menuList.IsNullOrEmpty() && _activeMenu != null)
			{
				_activeMenu.ShowMenu();
			}
		}
	}
}