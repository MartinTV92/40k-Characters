using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using System;
using JollyRoger.DarkHeresy.Data;

namespace JollyRoger.DarkHeresy
{ 

    public class Character
    {
		#region----- NESTED -----

		public enum Career
        {
            Adept,
            Arbitrator,
            Assassin,
            Cleric,
            Guardsman,
            Psyker,
            Scum,
            TechPriest,
        }

        [System.Serializable] public class CharacteristicDict : UnitySerializedDictionary<Characteristic.Type, Characteristic> { }

		#endregion


		#region----- VARIABLES -----

        /// <summary> The character's name. </summary>
        public string Name { get => name; private set => name = value; }
        private string name = default;

        /// <summary> The character's career path or class. </summary>
        public Career CareerPath { get => careerPath; private set => careerPath = value; }
        private Career careerPath = default;

		int _xp = 0;
		public int xp
		{
			get => _xp; 
			set => _xp = Mathf.Clamp(value, 0, int.MaxValue);
		}

		public int xpSpent
		{
			get
			{
				int sum = 0;
				foreach(var p in advancements)
					sum += p.xp;
				return sum;
			}
		}

        public CharacteristicDict characteristics = new();

		public List<Skill> _basicSkills = new ();
		public List<Skill> basicSkills
		{
			get => _basicSkills;
			set
			{
				_basicSkills = value;
				OnSkillChanged?.Invoke(Skill.Type.Basic);
			}
		}

		public List<Skill> _advancedSkills = new ();
		public List<Skill> advancedSkills
		{
			get => _advancedSkills;
			set
			{
				_advancedSkills = value;
				OnSkillChanged?.Invoke(Skill.Type.Advanced);
			}
		}


		public List<Talent> _talents = new ();
		public List<Talent> talents
		{
			get => _talents;
			set
			{
				_talents = value;
				OnTalentsChanged?.Invoke();
			}
		}


		public List<Advancement> _advancements = new();
		public List<Advancement> advancements
		{
			get => _advancements;
			set
			{
				_advancements = value;
				OnAdvancesChanged?.Invoke();
			}
		}

		// Events
		public event Action OnCharacterChanged;
		public event Action<Skill.Type> OnSkillChanged;
		public event Action OnTalentsChanged;
		public event Action OnAdvancesChanged;

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

        public Character () 
        {
			this.Name = "TestCharacter";
			CareerPath = Career.Arbitrator;

			characteristics = new();
			for (int i = 0; i < Enum.GetValues(typeof(Characteristic.Type)).Length; i++)
			{
				var charType = (Characteristic.Type)i;
				characteristics.Add(charType, new(charType, 0, CareerPath));
			}

			basicSkills = SkillDatabase.GetSkillsByType(Skill.Type.Basic);
			advancedSkills = new();
			talents = new();
		}

        public Character (string name, Career career)
        {
            this.Name = name;
            CareerPath = career;

			characteristics = new();
			for (int i = 0; i < Enum.GetValues(typeof(Characteristic.Type)).Length; i++)
			{
				var charType = (Characteristic.Type)i;
				characteristics.Add(charType, new(charType, 0, CareerPath));
			}

			basicSkills = SkillDatabase.GetSkillsByType(Skill.Type.Basic);
			advancedSkills = new();
			talents = new();
		}

		public Character(CharacterData data)
		{
			Name = data.name;
			CareerPath = data.career;

			characteristics = new();
			for(int i = 0; i < Enum.GetValues(typeof(Characteristic.Type)).Length; i++)
			{
				var charType = (Characteristic.Type) i;
				characteristics.Add(charType, new (charType, data.characteristics[i], CareerPath));
			}

			basicSkills = SkillDatabase.GetSkillsByType(Skill.Type.Basic);
			advancedSkills = new();
			talents = new();
		}

        /// <summary> Change the character's name. </summary>
        /// <param name="newName"> The name to set. </param>
        public void ChangeName(string newName) => Name = newName;

		/// <summary>
		/// General add method for character sheet
		/// </summary>
		/// <param name="value"></param>
		public void Add(object value)
		{
			if(value is Skill skill)
				Add(skill);
			if(value is Skill.Info info)
				Add(info);
			if(value is Talent talent)
				Add(talent);

		}

		public void Add(Skill.Info info) => Add(new Skill(info), info.type);

		public void Add(Skill.Info info, Skill.Type type) => Add(new Skill(info), type);

		public void Add(Skill skill) => Add(skill, skill.type);

		/// <summary>
		/// Adds a skill to the character sheet. If the skill is already present,
		/// Increases the training level of the current one.
		/// </summary>
		/// <param name="skill"> The skill to add. </param>
		/// <param name="type"> The list ot add it too. </param>
		public void Add(Skill skill, Skill.Type type)
		{
			var list = type == Skill.Type.Basic ? basicSkills : advancedSkills;
			var existingSkill = list.Where(x => x.name == skill.name).First();

			if(existingSkill != null)
				list.Add(skill);
			else
				existingSkill.Train(skill);
		}

		public void Add(Talent talent)
		{
			if(talents.Contains(talent) == false)
				talents.Add(talent);
		}

		public void Purchase(Advancement purchase)
		{
			if(purchase.CanBuy(this) == false)
				return;

			advancements.Add(purchase);
			Add(purchase.Value);
		}


		#endregion


		#region----- EDITOR -----

		/*

		[ValueDropdown("SkillDropdown")]
		public Skill testSkill;

		[Button("Add 'Test Skill'")]
		public void Add() => Add(testSkill, testSkill.type);

		IEnumerable SkillDropdown() => SkillDatabase.GetAllSkillInfo().Select(x => new ValueDropdownItem(x.name, x));

		// */

		#endregion

	}

}