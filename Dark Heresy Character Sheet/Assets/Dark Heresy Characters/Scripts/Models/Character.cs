using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		#endregion

		#region----- METHODS -----

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
			advancedSkills = new();
		}

        /// <summary> Change the character's name. </summary>
        /// <param name="newName"> The name to set. </param>
        public void ChangeName(string newName) => Name = newName;

		#endregion

	}

}