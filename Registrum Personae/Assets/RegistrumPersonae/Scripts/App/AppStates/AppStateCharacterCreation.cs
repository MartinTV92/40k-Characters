using System;
using UnityEngine;
using Sirenix.OdinInspector;
using RegistrumPersonae;
using JollyRoger.DesignPatterns;

namespace RegistrumPersonae
{		
	[System.Serializable, HideLabel, InlineProperty, FoldoutGroup("Character Creation State")]
	public class AppStateCharacterCreation : State<App.State>
	{
		#region ----- VARIABLES -----

		// TMP_InputField nameText;
		// TMP_Dropdown careerSelect;

		#endregion

		#region----- METHODS -----

		public override void Setup(StateMachine<App.State> sm)
		{
			base.Setup(sm);
			SetupCareerSelectUI();
		}

		public void SetupCareerSelectUI()
		{

		}

		public string GetEnteredName() => "Name from text";
		public Character.Career GetCareerSelection() => Character.Career.Adept;

		#endregion
	}

}