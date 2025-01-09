using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JollyRoger.DarkHeresy
{
	/// <summary>
	/// Prerequisite for defining minimum characteristic requirement for a Talent advancement.
	/// </summary>
	[System.Serializable]
	public class CharacteristicPrerequisite : Prerequisite
	{
		public Characteristic.Type stat = default;
		public int requirement = default;

		public override string Name { get => $"{CharacteristicView.GetShortName(stat)} {requirement}"; }

		public override bool IsMet(Character character) => character.characteristics[stat].Value > requirement;
	}
}