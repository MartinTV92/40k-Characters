using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunJack.DarkHeresy
{
	[System.Serializable]
	public class CharacteristicPrerequisite : Prerequisite
	{
		public Characteristic.Type stat = default;
		public int requirement = default;

		public override string Name { get => $"{CharacteristicView.GetShortName(stat)} {requirement}"; }

		public override bool IsMet(Character character) => character.characteristics[stat].Value > requirement;
	}
}