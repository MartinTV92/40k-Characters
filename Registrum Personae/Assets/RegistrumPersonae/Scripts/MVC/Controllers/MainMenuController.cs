using JollyRoger.DesignPatterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RegistrumPersonae
{ 

	public class MainMenuController : MonoBehaviour
	{
		#region----- VARIABLES -----

		public static MainMenuController Instance { get; private set; }

		#endregion


		#region----- MONOBEHAVIOURS -----

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			App.AddStateUpdateListener(UpdateController);
			App.AddStateChangeListener(GameStateChanged);
		}

		private void OnDisable()
		{
			App.RemoveStateUpdateListener(UpdateController);
			App.RemoveStateChangeListener(GameStateChanged);
		}

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		public void GameStateChanged(App.State lastState, App.State newState)
		{
			//MainMenuView.Instance.ShowUI(lastState, false);
			//MainMenuView.Instance.ShowUI(newState, true);
		}

		public void UpdateController(App.State state)
		{
			switch(state)
			{
				case App.State.Loading:
					//MainMenuView.Instance.UpdateLoadingDialogue();
					break;
			}
		}

		#endregion
	}
}

