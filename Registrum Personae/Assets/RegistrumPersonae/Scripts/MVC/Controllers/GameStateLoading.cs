using JollyRoger.DesignPatterns;
using UnityEngine;
using Sirenix.OdinInspector;

namespace RegistrumPersonae
{
	/// <summary>
	/// The loading state for the application, shows an inquisitorial logo and displays the app is loading.
	/// For now, the loading screen is waiting a small amount of time before going into the main menu.
	/// </summary>
	[System.Serializable, HideLabel, InlineProperty, FoldoutGroup("Loading State")]
	public class GameStateLoading : State<GameManager.State>
	{
		private float loadingStateTimeBuffer = 3;
		private float loadingStateEndTime;

		public override void Enter()
		{
			base.Enter();
			loadingStateEndTime = Time.time + loadingStateTimeBuffer;
		}

		public override void Update()
		{
			if(IsLoading())
				return;

			stateMachine.ChangeState(GameManager.State.MainMenu);
		}

		public bool IsLoading()
		{
			bool inLoadBuffer = Time.time < loadingStateEndTime;
			return inLoadBuffer;
		}

	}
}