using Sirenix.OdinInspector;
using JollyRoger.DesignPatterns;


namespace RegistrumPersonae
{
	/// <summary>
	/// Boot state for applicaiton, it just setups the UIs so none are active when uneeded.
	/// </summary>
	[System.Serializable, HideLabel, InlineProperty, FoldoutGroup("Boot State")]
	public class AppStateBoot : State<App.State>
	{
		public override void Update()
		{
			stateMachine.ChangeState(App.State.Loading);
		}
	}

}