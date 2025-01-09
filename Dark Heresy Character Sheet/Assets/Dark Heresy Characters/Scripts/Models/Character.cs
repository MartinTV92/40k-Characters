using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace SunJack.DarkHeresy
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

        public CharacteristicDict characteristics = new();

		public List<Skill> basicSkills = new List<Skill>();
		public List<Skill> advancedSkills = new List<Skill>();
		public List<Talent> talents = new List<Talent>();

		#endregion

		#region----- CUSTOM BEHAVIOURS -----

        public Character () 
        {
			this.Name = "TestCharacter";
			CareerPath = Career.Arbitrator;

			characteristics = new()
			{
				{Characteristic.Type.WeaponSkill, new (Characteristic.Type.WeaponSkill, 0, CareerPath)},
				{Characteristic.Type.BallisticSkill, new (Characteristic.Type.BallisticSkill, 0, CareerPath)},
				{Characteristic.Type.Strength, new (Characteristic.Type.Strength, 0, CareerPath)},
				{Characteristic.Type.Toughness, new (Characteristic.Type.Toughness, 0, CareerPath)},
				{Characteristic.Type.Agility, new (Characteristic.Type.Agility, 0, CareerPath)},
				{Characteristic.Type.Intelligence, new (Characteristic.Type.Intelligence, 0, CareerPath)},
				{Characteristic.Type.Perception, new (Characteristic.Type.Perception, 0, CareerPath)},
				{Characteristic.Type.WillPower, new (Characteristic.Type.WillPower, 0, CareerPath)},
				{Characteristic.Type.Fellowship, new (Characteristic.Type.Fellowship, 0, CareerPath)},
			};
		}

        public Character (string name, Career career)
        {
            this.Name = name;
            CareerPath = career;

			characteristics = new()
		    {
			    {Characteristic.Type.WeaponSkill, new (Characteristic.Type.WeaponSkill, 0, CareerPath)},
			    {Characteristic.Type.BallisticSkill, new (Characteristic.Type.BallisticSkill, 0, CareerPath)},
			    {Characteristic.Type.Strength, new (Characteristic.Type.Strength, 0, CareerPath)},
			    {Characteristic.Type.Toughness, new (Characteristic.Type.Toughness, 0, CareerPath)},
			    {Characteristic.Type.Agility, new (Characteristic.Type.Agility, 0, CareerPath)},
			    {Characteristic.Type.Intelligence, new (Characteristic.Type.Intelligence, 0, CareerPath)},
			    {Characteristic.Type.Perception, new (Characteristic.Type.Perception, 0, CareerPath)},
			    {Characteristic.Type.WillPower, new (Characteristic.Type.WillPower, 0, CareerPath)},
			    {Characteristic.Type.Fellowship, new (Characteristic.Type.Fellowship, 0, CareerPath)},
		    };

			basicSkills = SkillDatabase.GetSkillsByType(Skill.Type.Basic);
			//advancedSkills = new();
			advancedSkills = SkillDatabase.GetSkillsByType(Skill.Type.Advanced);
			talents = TalentDatabase.GetAllTalents();
		}

        /// <summary> Change the character's name. </summary>
        /// <param name="newName"> The name to set. </param>
        public void ChangeName(string newName) => Name = newName;

		/// <summary>
		/// Adds a skill to the character sheet. If the skill is already present,
		/// Increases the training level of the current one.
		/// </summary>
		/// <param name="skill"> The skill to add. </param>
		/// <param name="skillType"> The list ot add it too. </param>
		public void Add(Skill skill, Skill.Type skillType)
		{
			var list = skillType == Skill.Type.Basic ? basicSkills : advancedSkills;
			var existingSkill = list.Where(x => x.name == skill.name).First();

			if(existingSkill.IsNull())
				list.Add(skill);
			else
				existingSkill.Train(skill);
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