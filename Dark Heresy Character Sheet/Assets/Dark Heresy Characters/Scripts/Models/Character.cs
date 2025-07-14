using System;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Collections;
using Sirenix.OdinInspector;
using JollyRoger.Collections;
using JollyRoger.DarkHeresy.Data;
using System.Collections.Generic;
using System.ComponentModel;

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

		// REMOVE THIS AT A LATER DATE
		public struct RankInfo
		{
			public string name;
			public int xp;

			public RankInfo(string name, int xp)
			{
				this.name = name;
				this.xp = xp;
			}
		}

		#endregion


		#region----- VARIABLES -----

        /// <summary> The character's name. </summary>
        public string Name { get => name; private set => name = value; }
        private string name = default;

        /// <summary> The character's career path or class. </summary>
        public Career CareerPath { get => careerPath; private set => careerPath = value; }
        private Career careerPath = default;

		public int Rank
		{
			get
			{
				/* Should be calculated something like this.
				var ranks = DatabaseManager.GetRanks(CareerPath);
				for(int i = 0; i < DatabaseManager.GetRanks(CareerPath); i++)
					if(i < ranks.legnth)
						return Mathf.Clamp(i - 1, ranks.length);
				*/
				return 0;
			}
		}

		public RankInfo rankInfo = new RankInfo("Some Rank", 499);

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

		public DeepObservableList<Skill> basicSkills = new ();
/*		public DeepObservableList<Skill> basicSkills
		{
			get => _basicSkills;
			set
			{
				_basicSkills = value;
				OnSkillChanged?.Invoke(Skill.Type.Basic);
			}
		}*/

		public DeepObservableList<Skill> advancedSkills = new ();
/*		public DeepObservableList<Skill> advancedSkills
		{
			get => _advancedSkills;
			set
			{
				_advancedSkills = value;
				OnSkillChanged?.Invoke(Skill.Type.Advanced);
			}
		}*/


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
		public event Action OnCharacterChanged = delegate {};
		public event Action<Skill.Type> OnSkillChanged;
		public event Action OnTalentsChanged;
		public event Action OnAdvancesChanged;

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		/// <summary>
		/// Creates a simple character with test settings
		/// </summary>
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

			basicSkills = new(SkillDatabase.GetSkillsByType(Skill.Type.Basic));
			advancedSkills = new();
			talents = new();
			SetupUpdateEvents();
		}

		/// <summary>
		/// Creates a charcter from creator details
		/// </summary>
		/// <param name="name">The name of the character</param>
		/// <param name="career">The career of the character</param>
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

			basicSkills = new(SkillDatabase.GetSkillsByType(Skill.Type.Basic));
			advancedSkills = new();
			talents = new();
			SetupUpdateEvents();
		}

		/// <summary>
		/// Creates a character from save data.
		/// </summary>
		/// <param name="data"></param>
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

			basicSkills = new(SkillDatabase.GetSkillsByType(Skill.Type.Basic));
			advancedSkills = new();
			talents = new();
			SetupUpdateEvents();
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
			var existingSkill = list.Where(x => x.name == skill.name).FirstOrDefault();

			if(existingSkill != null)
				existingSkill.Train(skill);
			else
				list.Add(skill);
		}

		public void Add(Talent talent)
		{
			if(talents.Contains(talent) == false)
			{
				talents.Add(talent);
				SubscribeToUpdates(talent);
			}
		}

		public void Purchase(Advancement purchase)
		{
			if(purchase.CanBuy(this) == false)
				return;

			advancements.Add(purchase);
			Add(purchase.Value);
		}

		/// <summary>
		/// Subscribes character sheet to all update events from within to allow other objects to know, in an easy way
		/// when the character sheet has been modified in some way.
		/// </summary>
		private void SetupUpdateEvents()
		{
			// New
			Debug.Log("Character is listening to basic and advanced skills");
			basicSkills.ListChanged += UpdateCharacter;
			basicSkills.PropertyChanged += UpdateCharacter;

			advancedSkills.ListChanged += UpdateCharacter;
			advancedSkills.PropertyChanged += UpdateCharacter;


			// Phase Out
			SubscribeToUpdates(characteristics.Values.ToArray());
			SubscribeToUpdates(basicSkills.ToArray());
			SubscribeToUpdates(advancedSkills.ToArray());
			SubscribeToUpdates(talents.ToArray());
		}

		private void ClearUpdateEvents()
		{
			UnsubscribeFromUpdate(characteristics.Values.ToArray());
			UnsubscribeFromUpdate(basicSkills.ToArray());
			UnsubscribeFromUpdate(advancedSkills.ToArray());
			UnsubscribeFromUpdate(talents.ToArray());
		}

		private void SubscribeToUpdates(params IUpdateable[] updateables)
		{
			foreach(var u in updateables)
				u.OnUpdate += InvokeUpdate;
		}

		private void UnsubscribeFromUpdate(params IUpdateable[] updateables)
		{
			foreach (var u in updateables)
				u.OnUpdate -= InvokeUpdate;
		}

		private void InvokeUpdate()
		{
			OnCharacterChanged?.Invoke();
		}

		private void UpdateCharacter()
		{
			Debug.Log($"Character Update");
			OnCharacterChanged?.Invoke();
		}

		private void UpdateCharacter(object sender, PropertyChangedEventArgs e)
		{
			Debug.Log($"Character Update: {e.PropertyName}");
			OnCharacterChanged?.Invoke();
		}

		public override string ToString()
		{
			var s = new StringBuilder();
			var sectionBreak = "_______________________";

			s.AppendLine($"Name: {Name}");
			s.AppendLine($"Career Path: {CareerPath} - {Rank} ({rankInfo.name}) | {xpSpent}xp/{rankInfo.xp}xp");
			s.AppendLine(sectionBreak);

			s.AppendLine("Characteristics");
			var statBlock = "";
			for(int i = 0; i < characteristics.Count; i++)
				statBlock += characteristics[(Characteristic.Type) i].ToString() + (i < characteristics.Count - 1 ? " | ":"");
			s.AppendLine(statBlock);
			s.AppendLine(sectionBreak);


			s.AppendLine("\t[Basic Skills] \t\t[Advanced Skills]");
			var longestEntry = basicSkills.OrderByDescending(o => o.ToString().Length).FirstOrDefault().GetNameWithSkill().Length;
			var columnWidth = longestEntry + 6; 
			for(int i = 0; i < Math.Max(basicSkills.Count, advancedSkills.Count); i++)
			{
				var rightColumn = i < basicSkills.Count ? basicSkills[i].GetNameWithSkill().PadRight(columnWidth) + "\t" + basicSkills[i].GetBonusString() : 
					"".PadRight(columnWidth + 3);

				var leftColumn = ((i < advancedSkills.Count) ? advancedSkills[i].GetNameWithSkill().PadRight(columnWidth) + "\t" + advancedSkills[i].GetBonusString() : "");
				s.AppendLine($"{rightColumn}| {leftColumn}");
			}
			s.AppendLine(sectionBreak);


			s.AppendLine("Talents & Traits");
			foreach(var talent in talents)
				s.AppendLine(talent.ToString());
			s.AppendLine(sectionBreak);


			s.AppendLine("Advancements");
			foreach (var adv in advancements)
				s.AppendLine(advancements.ToString());
			s.AppendLine(sectionBreak);



			return s.ToString();
		}

		#endregion


		#region----- EDITOR -----

		//*

		[ValueDropdown("SkillDropdown")]
		public Skill.Info testSkill;

		[Button("Add 'Test Skill'")]
		public void Add() => Add(testSkill, testSkill.type);

		IEnumerable SkillDropdown() => SkillDatabase.GetAllSkillInfo().Select(x => new ValueDropdownItem(x.name, x));

		// */

		#endregion

	}

}