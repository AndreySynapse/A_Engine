// A-Engine, Code version: 1

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace AEngine.Menu
{
	public static class TransitionManager 
	{
		private static MenuManager menu;
		public static MenuManager Menu { set { menu = value; } }

		private static List<Transition> transitionHistory;
		private static string transitionMenu;
		private static bool isTransition;
		private static int historyCheckStepCount;

		static TransitionManager ()
		{
			menu = null;

			transitionHistory = new List<Transition> ();
			transitionMenu = "";
			isTransition = false;
			historyCheckStepCount = 20;
		}

		public static void Configure ()
		{

		}

		public static bool ContinueTransition ()
		{
			if (!isTransition)
				return false;

			isTransition = false;

			if (menu == null || menu.Count == 0)				
				return false;
			if (transitionHistory.Count == 0)
				return false;

			string menuName;
			if (transitionMenu == "") {
				// Back transition
				menuName = transitionHistory [transitionHistory.Count - 1].MenuName;
				transitionHistory.RemoveAt (transitionHistory.Count - 1);
			} else {
				// Use rememered menu name
				menuName = transitionMenu;
			}

			if (!menu.Contains (menuName)) {
				Debug.Log ("[Class = BaseGameManager, method = ContinueTransition] : Scene menu has not menu with name: " + menuName);
				return false;
			}
			menu.ShowMenu (menuName);
			if (transitionMenu != "") {
				transitionMenu = "";
				CheckHistory ();
			}

			return true;
		}

		public static void MakeTransition (Transition transition, bool saveHistory = true)
		{
			if (SceneManager.GetActiveScene ().name == transition.SceneName)
				MakeTransitionInActiveScene (transition.MenuName, saveHistory);
			else {
				if (saveHistory)
					transitionHistory.Add (Transition.NewTransition(SceneManager.GetActiveScene().name, menu.ActiveMenu.name));
				transitionMenu = transition.MenuName;
				isTransition = true;
				SceneManager.LoadScene (transition.SceneName);
			}
		}

		public static void MakeTransition (string sceneName, string menuName, bool saveHistory = true)
		{
			MakeTransition (Transition.NewTransition (sceneName, menuName), saveHistory);
		}

		public static void MakeTransitionInActiveScene (string menuName, bool saveHistory = true)
		{
			if (menu == null || menu.Count == 0) {
				Debug.Log ("[Class = BaseGameManager, method = MakeTransitionInActiveScene] : Scene menu equal to null");
				return;
			}
			if (!menu.Contains (menuName)) {
				Debug.Log ("[Class = BaseGameManager, method = MakeTransitionInActiveScene] : Scene menu has not menu with name: " + menuName);
				return;
			}

			if (saveHistory)
				transitionHistory.Add (Transition.NewTransition(SceneManager.GetActiveScene().name, menu.ActiveMenu.name));
			menu.ShowMenu (menuName);
			CheckHistory ();
		}

		public static void MakeBackTransition ()
		{
			if (transitionHistory.Count == 0)
				return;

			Transition lastTransition = transitionHistory [transitionHistory.Count - 1];
			if (SceneManager.GetActiveScene ().name == lastTransition.SceneName) {
				menu.ShowMenu (lastTransition.MenuName);
				transitionHistory.RemoveAt (transitionHistory.Count - 1);
			} else {
				transitionMenu = "";
				isTransition = true;
				SceneManager.LoadScene (lastTransition.SceneName);
			}
		}

		public static void ClearTransitionHistory ()
		{
			transitionHistory.Clear ();
		}

		public static void AddTransitionHistory (params Transition [] transitions)
		{
			if (transitionHistory == null)
				transitionHistory = new List<Transition> ();

			for (int i = 0; i < transitions.Length; i++)
				transitionHistory.Add (transitions [i]);

			CheckHistory ();
		}

		public static void AddTransitionHistory (List<Transition> transitions)
		{
			if (transitionHistory == null)
				transitionHistory = new List<Transition> ();

			for (int i = 0; i < transitions.Count; i++)
				transitionHistory.Add (transitions [i]);

			CheckHistory ();
		}

		/// <summary>
		/// Return count of transitions. If history = null, return -1
		/// </summary>
		public static int HistoryLength ()
		{
			if (transitionHistory == null)
				return -1;

			return transitionHistory.Count;
		}

		public static void SetCheckHistoryStepsLength (int value)
		{
			if (value >= 0)
				historyCheckStepCount = value;
			else
				historyCheckStepCount = 20;
		}

		public static void CheckHistory ()
		{
			if (menu == null || menu.Count == 0)				
				return;
			if (transitionHistory.Count < 3)
				return;

			transitionHistory.Add (Transition.NewTransition (SceneManager.GetActiveScene ().name, menu.ActiveMenu.name)); 

			int steps = 1, N2 = 0, K = 0;
			for (int N1 = transitionHistory.Count-1; N1>= 3 && steps <= historyCheckStepCount; N1--, steps++) {

				if (CurrentTransitionCount (transitionHistory [N1], false) < 2)
					continue;

				K = N1 - 1;
				FindBlock:
				N2 = FindEqualTransitionIndex (transitionHistory [N1], K, true);
				if (N2 < 0 || (N1 - N2 > N2 + 1))
					continue;
				for (int j = 1; j < N1-N2; j++)
					if (transitionHistory [N1 - j] != transitionHistory [N2 - j]) {
						K = N2 - 1;
						goto FindBlock;
					}

				int count = N1 - N2;
				while (count > 0) {
					transitionHistory.RemoveAt (N2);
					N1--;
					N2--;
					count--;
				}
			}
			transitionHistory.RemoveAt (transitionHistory.Count - 1);
		}

		public static void LogHistory ()
		{
			string result = "";
			for (int i = 0; i < transitionHistory.Count; i++)
				result = string.Format("{0}[{1}--{2}]", result, transitionHistory[i].SceneName, transitionHistory [i].MenuName);
			Debug.Log (result);
		}

		private static int CurrentTransitionCount (Transition transition, bool useCurrentMenuAdditionly)
		{
			int count = 0;
			for (int i = 0; i < transitionHistory.Count; i++)
				if (transitionHistory [i] == transition)
					count++;

			if (useCurrentMenuAdditionly && transition == Transition.NewTransition (SceneManager.GetActiveScene ().name, menu.ActiveMenu.name))
				count++;

			return count;
		}

		private static int FindEqualTransitionIndex (Transition baseTransition, int indexFrom, bool checkToBack)
		{
			for (int i = indexFrom; i >= 0 && i < transitionHistory.Count; ) {
				if (transitionHistory [i] == baseTransition)
					return i;

				if (checkToBack)
					i--;
				else
					i++;
			}

			return -1;
		}
	}
}
