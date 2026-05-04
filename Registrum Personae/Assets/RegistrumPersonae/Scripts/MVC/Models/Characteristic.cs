using System;
using UnityEngine;
using Sirenix.OdinInspector;
using JollyRoger.Collections;

namespace RegistrumPersonae
{ 
	/// <summary>
	/// A number beteen 0-100 that represents the character's natural capacity in some regard.
	/// Strength, Toughness, Willpower etc.
	/// </summary>
	[System.Serializable, HideReferenceObjectPicker]
	public class Characteristic : NotifyPropertyChangedWrapper
	{
		#region----- NESTED -----

	    public enum Type
	    {
	        WeaponSkill,
	        BallisticSkill,
	        Strength,
	        Toughness,
	        Agility,
	        Intelligence,
	        Perception,
	        WillPower,
	        Fellowship
	    }

		#endregion


		#region----- VARIABLES -----

		// Constants/Statics
		private const int MAX_ADVANCEMENTS = 4;

		private readonly Type characteristic = Type.WeaponSkill;

		private int _unnatural = 0;
		[SerializeField, PropertyOrder(0), HorizontalGroup("R1")]
		public int Unnatural
		{
			get => _unnatural;
			set => SetProperty(ref _unnatural, Mathf.Clamp(value, 0, int.MaxValue));
		}

		private int _base = 0;
		[SerializeField, PropertyOrder(0), SuffixLabel("+"), HorizontalGroup("R1"), LabelText("Value")] 
		public int Base
		{ 
			get => _base;
			set => SetProperty(ref _base, Mathf.Clamp(value, 0, 100));
		}

		private int _advancedTaken = 0;
		[ShowInInspector, PropertyOrder(0), SuffixLabel("/4 ->"), HorizontalGroup("R1"), LabelText("Adv.")]
		public int AdvancesTaken
		{
			get => _advancedTaken;
			set => SetProperty(ref _advancedTaken, Mathf.Clamp(value, 0, 4));
		}

		[ShowInInspector, PropertyOrder(0), HideLabel, ReadOnly, HorizontalGroup("R1")] 
	    public int FinalValue  => _base + (AdvancesTaken * 5); 

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		#region Statics/Helpers

		public static string GetShortName(Type charType)
		{
			switch (charType)
			{
				case Type.WeaponSkill:
					return "WS";
				case Type.BallisticSkill:
					return "BS";
				case Type.Strength:
					return "S";
				case Type.Toughness:
					return "T";
				case Type.Agility:
					return "Ag";
				case Type.Intelligence:
					return "Int";
				case Type.Perception:
					return "Per";
				case Type.WillPower:
					return "WP";
				case Type.Fellowship:
					return "Fel";
			}

			return default;
		}

		#endregion

		public Characteristic(Type charType, int value, Character.Career career)
	    {
	        characteristic = charType;
			Base = value;
		}

		public override string ToString()
		{
			var advFrac = new string[] { "�/4", "�", "�/4", "�", "4/4"}; 
			var advString = advFrac[AdvancesTaken];
			var unat = Unnatural > 0 ? $"({Unnatural})":"";
			return $"{GetShortName(characteristic)}: {unat}{FinalValue}({advString})";
		}

		#endregion
	}
}

