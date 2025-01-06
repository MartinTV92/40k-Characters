using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SunJack.DarkHeresy
{ 
    /// <summary>
    /// A number beteen 0-100 that represents the character's natural capacity in some regard.
    /// Strength, Toughness, Willpower etc.
    /// </summary>
	[System.Serializable]
    public class Characteristic
    {
		/* TODO
         
        - Make method 'GetCost(int)' to get the cost of the advancment
        X Turn advancment into its own class? 
        x Implement MVC for this and app in general going forward

         */

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

        [System.Serializable]
        public struct Advancement
        {
            public enum Level
            {
                Simple = 1,
                Intermidiate,
                Trained,
                Expert
            }

            [ReadOnly, HorizontalGroup("R1", Width = 0.5f), HideLabel] public Level level;
            [ReadOnly, HorizontalGroup("R1", Width = 0.3f), HideLabel, SuffixLabel("XP")] public int cost;
			[HorizontalGroup("R1", Width = 0.2f), HideLabel] public bool purchased;

			public Advancement(Character.Career career, Level lvl)
			{
				level = lvl;
				purchased = false;
				cost = 0; // <-- Get cost from database
			}

		}

		#endregion


		#region----- VARIABLES -----

		// Constants/Statics
		private const int MAX_ADVANCEMENTS = 4;

		// Properties/PBFs
		[SerializeField, PropertyOrder(0), SuffixLabel("->"), HorizontalGroup("R1", Width = 0.5f), LabelText("Value")] 
        private int _base = 0;

		[ShowInInspector, PropertyOrder(0), ReadOnly, HorizontalGroup("R1", Width = 0.2f), HideLabel] 
        public int Value 
		{ 
			get => _base + (advancesTaken * 5); 
			set
			{ 
				_base = Mathf.Clamp(value, 0, 100);
				OnValueChanged?.Invoke();
			}
		}
        
		public int advancesTaken
		{
			get
			{
				var result = 0;
				foreach (var advance in advances)
					if (advance.purchased)
						result++;

				return result;
			}
		}

		// Fields
		[ShowInInspector, PropertyOrder(-1), ReadOnly, HorizontalGroup("R1", Width = 0.3f), HideLabel]
		public Type characteristic = Type.WeaponSkill;
        public Advancement[] advances;

		// Events
		public event Action OnValueChanged;

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

        public Characteristic(Type charType, int value, Character.Career career)
        {
            // Set Characteristic
            characteristic = charType;

			// Set advancements with cost
			advances = new[]
		    {
			    new Advancement(career, Advancement.Level.Simple),
			    new Advancement(career, Advancement.Level.Intermidiate),
			    new Advancement(career, Advancement.Level.Trained),
			    new Advancement(career, Advancement.Level.Expert)
		    };
		}

		public override string ToString() => $"{characteristic}: {Value}({advancesTaken}/4)";

		#endregion
	}
}