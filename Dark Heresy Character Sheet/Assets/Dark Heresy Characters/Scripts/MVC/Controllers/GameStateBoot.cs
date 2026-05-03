using Sirenix.OdinInspector;
using RegistrumPersonae.FinateStateMachine;

/// <summary>
/// Boot state for applicaiton, it just setups the UIs so none are active when uneeded.
/// </summary>
[System.Serializable, HideLabel, InlineProperty, FoldoutGroup("Boot State")]
public class GameStateBoot : State<GameManager.State>
{
	public override void Update()
	{
		stateMachine.ChangeState(GameManager.State.Loading);
	}
}
