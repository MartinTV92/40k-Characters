using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SunJack.DarkHeresy
{
	[CreateAssetMenu(menuName = "Database Object / Career")]
    public class Career : ScriptableObject
    {
		#region----- NESTED -----

		[System.Serializable]
		public class CharacteristicAdvanceDict : UnitySerializedDictionary<Characteristic.Type, CharacteristicAdvance> { }

		[System.Serializable]
		public class CharacteristicAdvance
		{
			[HorizontalGroup("R0", Width = 0.2f), ShowInInspector, LabelWidth(50)] public int Simple { get => advances[0]; set => advances[0] = value; }
			[HorizontalGroup("R0", Width = 0.30f), ShowInInspector, LabelWidth(80)] public int Intermediate { get => advances[1]; set => advances[1] = value; }
			[HorizontalGroup("R0", Width = 0.25f), ShowInInspector, LabelWidth(50)] public int Trained { get => advances[2]; set => advances[2] = value; }
			[HorizontalGroup("R0", Width = 0.2f), ShowInInspector, LabelWidth(40)] public int Expert { get => advances[3]; set => advances[3] = value; }

			[HideInInspector] public int[] advances = new int[4];

			public int GetAdvance(int lvl) => advances[lvl];
			public int GetAdvance(Characteristic.Advancement advancement) => GetAdvance((int) advancement.level);
		}

		[System.Serializable]
		public class Rank
		{

			public int xp = 0;

			public Path[] path = new Path[0];

			public Rank(int xp)
			{
				this.xp = xp;
			}
		}


		#endregion


		#region----- VARIABLES -----

		[ShowInInspector, PropertyOrder(-1)]
		public string CareerName => this.name;

		public CharacteristicAdvanceDict characteristicAdvances = new CharacteristicAdvanceDict()
		{
			{Characteristic.Type.WeaponSkill, new()},
			{Characteristic.Type.BallisticSkill, new()},
			{Characteristic.Type.Strength, new()},
			{Characteristic.Type.Toughness, new()},
			{Characteristic.Type.Agility, new()},
			{Characteristic.Type.Intelligence, new()},
			{Characteristic.Type.Perception, new()},
			{Characteristic.Type.WillPower, new()},
			{Characteristic.Type.Fellowship, new()},
		};

		public Rank[] ranks = new Rank[8]
		{
			new Rank(499),
			new Rank(999),
			new Rank(1999),
			new Rank(2999),
			new Rank(5999),
			new Rank(7999),
			new Rank(9999),
			new Rank(14999),
		};

		#endregion


		#region----- EDITOR ONLY -----
		#if UNITY_EDITOR

		public enum AdvancePreference
		{
			Favoured,
			Preferable,
			Neutral,
			Unfavourable
		}
	
		public static readonly Dictionary<AdvancePreference, int[]> xpAdvancePresets = new()
		{
			{AdvancePreference.Favoured,	 new int[] {100, 250, 500, 500}},
			{AdvancePreference.Preferable,	 new int[] {100, 250, 500, 750}},
			{AdvancePreference.Neutral,		 new int[] {250, 500, 750, 1000}},
			{AdvancePreference.Unfavourable, new int[] {500, 750, 1000, 2000}},
		};

		[PropertySpace(spaceBefore: 50), Button("Characteristic Advance Preset")]
		public void ApplyCharacteristicPreset(Characteristic.Type characteristic, AdvancePreference preference)
		{
			characteristicAdvances[characteristic].advances = xpAdvancePresets[preference];
		}

		#endif
		#endregion
	}
}