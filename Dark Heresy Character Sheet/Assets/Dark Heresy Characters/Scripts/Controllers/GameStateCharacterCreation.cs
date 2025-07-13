using TMPro;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using JollyRoger.DarkHeresy;
using JollyRoger.FinateStateMachine;

[System.Serializable, HideLabel, InlineProperty, FoldoutGroup("Character Creation State")]
public class GameStateCharacterCreation : State<GameManager.State>
{
	#region ----- VARIABLES -----

	[PropertyOrder(-1), SerializeField] TMP_InputField nameText;
	[PropertyOrder(-1), SerializeField]	TMP_Dropdown careerSelect;

	#endregion

	#region----- METHODS -----

	public override void Setup(StateMachine<GameManager.State> sm)
	{
		base.Setup(sm);
		SetupCareerSelectUI();
	}

	public void SetupCareerSelectUI()
	{
		careerSelect.options.Clear();
		foreach (var val in Enum.GetValues(typeof(Character.Career)))
			careerSelect.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
	}

	public string GetEnteredName() => nameText.text;
	public Character.Career GetCareerSelection() => (Character.Career)careerSelect.value;

	#endregion
}
