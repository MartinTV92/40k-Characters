using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Windows;
using System.Collections.Generic;

namespace JollyRoger.DarkHeresy
{
    /// <summary>
    /// Defines an apptitude for a given area of expertise such as driving, knowledge or tech-use.
    /// Furthermore, defines the mastery and other bonuses applied to skill.
    /// </summary>
    [System.Serializable, HideReferenceObjectPicker]
    public class Skill
    {
		#region----- NESTED -----

		/// <summary>
		/// Static info for SkillInfo i.e Names, Description, Type and characteristic.
		/// This information is what is stored in the SkillDatabase.
		/// </summary>
		[System.Serializable, HideLabel, InlineProperty]
		public struct Info
		{
			[FoldoutGroup("$name")]

			[HorizontalGroup("$name/R0", Width = 0.7f), LabelWidth(40), PropertyOrder(-1)]
			public string name;

			[HorizontalGroup("$name/R0", Width = 0.3f), HideLabel, SerializeField, PropertyOrder(1)]
			public Type type;

			[HorizontalGroup("$name/R1"), HideLabel, SerializeField]
			public Characteristic.Type characteristic;

			[HorizontalGroup("$name/R1"), HideLabel, SerializeField]
			public Descriptor descriptor;

			[FoldoutGroup("$name/Description"), TextArea(3, 15), HideLabel]
			public string description;
		}

        /// <summary> The basic flag(s) that can be used to describe the uses a skill has </summary>
		[System.Flags]
        public enum Descriptor
        {
            None = 0,
            Crafting = 1 << 0,
            Interaction = 1 << 1,
            Investigation = 1 << 2,
            Movement = 1 << 3,
            Operator = 1 << 4
        }

        /// <summary> 
        /// The category that a skll falls into denoting if it must be trained to be used,
        /// or if the character can 'take a stab at it' with a penalty.
        /// </summary>
        public enum Type
        {
            Advanced,
            Basic
        }

		#endregion


		#region----- VARIABLES -----

        // CONSTANTS
		public const int MAX_MASTERY = 2;
        private const int MAX_BONUS = 60;
        private const int MIN_BONUS = -60;
        public const int MASTERY_BONUS = 10;

        // EVENTS
        public event Action OnValueChanged;

        // VARIABLES
        [InlineProperty]
		public Info info;
		public string name => info.name;
        public Type type => info.type;
        public Characteristic characteristic;
        public Characteristic.Type characteristicType => info.characteristic;
        public Descriptor descriptor => info.descriptor;
        public string description => info.description;

		[HorizontalGroup("R0"), ShowInInspector, LabelWidth(50)]
		private int _mastery = -1;
        private int masteryBonus => mastery < 0 ? 0 : mastery * MASTERY_BONUS;
		public int mastery 
        { 
            get => _mastery; 
            set
            {
                _mastery = Mathf.Clamp(value, -1, MAX_MASTERY);
                OnValueChanged?.Invoke();
            }
        }

		[HideInInspector] 
        private int _miscBonus;
        [HorizontalGroup("R0"), ShowInInspector, LabelWidth(70)]
		public int miscBonus
        {
            get => _miscBonus;
            set
            {
                _miscBonus = value;
                OnValueChanged?.Invoke();
            }
        }

		[HorizontalGroup("R0"), ShowInInspector, LabelWidth(40)]
		public int bonus => Mathf.Clamp(masteryBonus + miscBonus, MIN_BONUS, MAX_BONUS);

		public string[] group;


		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		public Skill() { }

        public Skill(Info info)
        {
            this.info = info;
        }

        public int GetBonus(Characteristic characteristic)
        {
            var trainingBonus = mastery == -1 ? -Mathf.FloorToInt(characteristic.Value / (float)2) : mastery * MASTERY_BONUS;
			return Mathf.Clamp(trainingBonus + miscBonus, MIN_BONUS, MAX_BONUS);
		}


        /// <summary>
        /// Trains the skill, increasing its mastery if possible.
        /// </summary>
        /// <param name="skill">The skill to check for training. </param>
        /// <param name="enforceProgression"> Enforces training through progression in order. </param>
		public void Train(Skill skill)
        {
            if(skill.name != name || skill.mastery <= mastery)
                return;

            mastery++;
        }

		#endregion
	}
}