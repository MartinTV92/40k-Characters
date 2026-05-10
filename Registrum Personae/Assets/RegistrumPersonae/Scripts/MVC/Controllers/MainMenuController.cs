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
			GameManager.AddStateUpdateListener(UpdateController);
			GameManager.AddStateChangeListener(GameStateChanged);
		}

		private void OnDisable()
		{
			GameManager.RemoveStateUpdateListener(UpdateController);
			GameManager.RemoveStateChangeListener(GameStateChanged);
		}

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		public void GameStateChanged(GameManager.State lastState, GameManager.State newState)
		{
			//MainMenuView.Instance.ShowUI(lastState, false);
			//MainMenuView.Instance.ShowUI(newState, true);
		}

		public void UpdateController(GameManager.State state)
		{
			switch(state)
			{
				case GameManager.State.Loading:
					//MainMenuView.Instance.UpdateLoadingDialogue();
					break;
			}
		}

		#endregion
	}
}

