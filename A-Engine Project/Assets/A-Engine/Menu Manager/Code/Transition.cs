// A-Engine, Code version: 1

using UnityEngine;
using System.Collections;

namespace AEngine.Menu
{
	public class Transition
	{
		public string SceneName;
		public string MenuName;

		public Transition ()
		{
			SceneName = "";
			MenuName = "";
		}

		public static Transition NewTransition (string SceneName, string MenuName)
		{
			Transition transition = new Transition ();
			transition.SceneName = SceneName;
			transition.MenuName = MenuName;

			return transition;
		}

		public static bool operator == (Transition transition1, Transition transition2)
		{
			if (transition1.SceneName == transition2.SceneName && transition1.MenuName == transition2.MenuName)
				return true;
			else
				return false;
		}

		public static bool operator != (Transition transition1, Transition transition2)
		{
			if (transition1.SceneName == transition2.SceneName && transition1.MenuName == transition2.MenuName)
				return false;
			else
				return true;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}