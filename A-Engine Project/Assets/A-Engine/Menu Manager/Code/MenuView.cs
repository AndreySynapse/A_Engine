// A-Engine, Code version: 1

using UnityEngine;
using System.Collections;

namespace AEngine.Menu
{
	public abstract class MenuView : MonoBehaviour 
	{
		public enum EMenuViewState
		{
			HideView = 0,
			ShowView = 1
		}

	#region Events
		/// <summary>
		/// Call when MenuManager open this menu.
		/// </summary>
		public virtual void OnShowMenu () {}

		/// <summary>
		/// Call when MenuManager hide this menu and open new.
		/// </summary>
		public virtual void OnHideMenu () {}

		/// <summary>
		/// Call when menu change screen orientation. First it hide old view with HideView flag, 
		/// then show new view with ShowView flag. Use it to save and load view states.
		/// </summary>
		public virtual void OnChangeScreenOrientation(EMenuViewState currentViewState) {}
	#endregion
	}
}