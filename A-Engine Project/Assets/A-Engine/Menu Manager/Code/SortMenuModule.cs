// A-Engine, Code version: 1

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AEngine.Menu
{
	public class SortMenuModule
	{
		public enum ESortKind
		{
			None = 0,
			Up = 1,
			Down = 2
		}

		private ESortKind _sortAction;
		private int _sortIndex;

		public void Reset()
		{
			_sortAction = ESortKind.None;
			_sortIndex = 0;
		}

		public void ChangeSortState(ESortKind sortKind, int index)
		{
			_sortAction = sortKind;
			_sortIndex = index;
		}

		public void SortInvoke(List<Menu> list)
		{
			if (_sortAction != ESortKind.None)
			{
				Menu targetMenu = list[_sortIndex];
				list.RemoveAt(_sortIndex);
				list.Insert(_sortIndex + (_sortAction == ESortKind.Up ? -1 : 1), targetMenu);
								
				int siblingIndex = targetMenu.gameObject.transform.GetSiblingIndex();
				targetMenu.gameObject.transform.SetSiblingIndex(siblingIndex + (_sortAction == ESortKind.Up ? -1 : 1));
			}
			
			Reset();
		}
	}
}