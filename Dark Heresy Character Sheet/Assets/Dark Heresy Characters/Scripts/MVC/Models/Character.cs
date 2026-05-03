using System;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Collections;
using Sirenix.OdinInspector;
using RegistrumPersonae.Collections;
using RegistrumPersonae.Data;
using System.Collections.Generic;
using System.ComponentModel;

namespace RegistrumPersonae
{
	//[Serializable]
    public class Character : NotifyPropertyChangedWrapper
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

        private string _name = default;
		[ShowInInspector, PropertyOrder(-1)]
        public string Name 
		{ 
			get => _name; 
			private set => SetProperty(ref _name, value); 
		}

		[ShowInInspector, PropertyOrder(-1)]
		public Homeworld HomeWorld { get; private set;} = new();

        private Career _careerPath = default;
        public Career CareerPath { get => _careerPath; private set => _careerPath = value; }

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
		[ShowInInspector, HorizontalGroup("XP"), SuffixLabel("/")]
		public int XP
		{
			get => _xp; 
			set => SetProperty(ref _xp, Mathf.Clamp(value, 0, int.MaxValue));
		}

		[ShowInInspector, HorizontalGroup("XP")]
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

		int _currentFatepoints = 0;
		[ShowInInspector, HorizontalGroup("FatePoints"), SuffixLabel("/")]
		public int CurrentFatePoints
		{
			get => _currentFatepoints;
			set => SetProperty(ref _currentFatepoints, Mathf.Clamp(value,0,TotalFatePoints));
		}

		int _totalFatePoints = 0;
		[ShowInInspector, HorizontalGroup("FatePoints")]
		public int TotalFatePoints
		{
			get => _totalFatePoints;
			set 
			{
				SetProperty(ref _totalFatePoints, Mathf.Clamp(value, 0, int.MaxValue));
				CurrentFatePoints = CurrentFatePoints;
			}
		}

        public CharacteristicDict characteristics = new();
		public DeepObservableList<Skill> basicSkills = new ();
		public DeepObservableList<Skill> advancedSkills = new ();
		public DeepObservableList<Talent> talents = new ();


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
				talents.Add(talent);
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
			SubscribeToUpdates(this);
			SubscribeToUpdates(HomeWorld);
			SubscribeToUpdates(characteristics.Values.ToArray());
			SubscribeToUpdates(basicSkills);
			SubscribeToUpdates(advancedSkills);
			SubscribeToUpdates(talents);
		}

		private void ClearUpdateEvents()
		{
			UnsubscribeFromUpdate(this);
			UnsubscribeFromUpdate(HomeWorld);
			UnsubscribeFromUpdate(characteristics.Values.ToArray());
			UnsubscribeFromUpdate(basicSkills);
			UnsubscribeFromUpdate(advancedSkills);
			UnsubscribeFromUpdate(talents);
		}

		private void SubscribeToUpdates(params INotifyPropertyChanged[] updateables)
		{
			foreach(var u in updateables)
				u.PropertyChanged += UpdateCharacter;
		}

		private void SubscribeToUpdates<T>(DeepObservableList<T> list) where T : INotifyPropertyChanged
		{
			list.ListChanged += UpdateCharacter;
			list.PropertyChanged += UpdateCharacter;
		}

		private void UnsubscribeFromUpdate(params INotifyPropertyChanged[] updateables)
		{
			foreach (var u in updateables)
				u.PropertyChanged -= UpdateCharacter;
		}

		private void UnsubscribeFromUpdate<T>(DeepObservableList<T> list) where T: INotifyPropertyChanged
		{
			list.ListChanged -= UpdateCharacter;
			list.PropertyChanged -= UpdateCharacter;
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
			OnCharacterChanged?.Invoke();
		}

		public override string ToString()
		{
			var s = new StringBuilder();
			var sectionBreak = "____________________________________________________________________________________________";

			#region Basic Info (name, career, stats, etc.)

			s.AppendLine("\t\t\t[Basic Info]");
			s.AppendLine($"Name: {Name}");
			s.AppendLine($"Homeworld: {HomeWorld.Name} - {HomeWorld.World}");
			s.AppendLine($"Career Path: {CareerPath} - {Rank} ({rankInfo.name} | {rankInfo.xp}xp)");
			s.AppendLine($"XP (current | spent): {XP}xp  | {xpSpent}xp");
			s.AppendLine($"Fate Points: {CurrentFatePoints} / {TotalFatePoints}");

			s.AppendLine(sectionBreak);

			var statBlock = "";
			for(int i = 0; i < characteristics.Count; i++)
				statBlock += characteristics[(Characteristic.Type) i].ToString() + (i < characteristics.Count - 1 ? " | ":"");
			s.AppendLine(statBlock);
			s.AppendLine(sectionBreak);

			#endregion


			#region Skills (It is a great, big ol' mess-o-shit

			var LongestBasicLength = basicSkills.OrderByDescending(o => o.ToString().Length).FirstOrDefault().GetNameWithSkill().Length;
			var widestAdvancedLength = 54;
			s.AppendLine($"\t[Basic Skills] \t\t\t[Advanced Skills]");
			for (int i = 0; i < Math.Max(basicSkills.Count, advancedSkills.Count); i++)
			{
				var rightColumn = i < basicSkills.Count ? basicSkills[i].GetNameWithSkill().PadRight(LongestBasicLength + 9) + "\t" + basicSkills[i].GetBonusString() : 
					"".PadRight(LongestBasicLength + 9); 

				if(string.IsNullOrWhiteSpace(rightColumn))
					rightColumn = "\t\t\t";

				var diff = (advancedSkills.Count > 0 && i < advancedSkills.Count) ? widestAdvancedLength - (advancedSkills[i].GetNameWithSkill().Length) : 0;
				int tabs = Mathf.Clamp(Mathf.FloorToInt(diff / TabMod), 0, 10);
				if(i < advancedSkills.Count && NameRequiresExtraTab(advancedSkills[i].name))
					tabs++;
				var tabString = (tabs > 0 ? new string('\t', tabs) : "");
				var leftColumn = (i < advancedSkills.Count) ? advancedSkills[i].GetNameWithSkill() + $"{tabString}" + advancedSkills[i].GetBonusString() : "";
				s.AppendLine($"{rightColumn}\t| {leftColumn}");
			}
			s.AppendLine(sectionBreak);

			#endregion


			s.AppendLine("\t\t\t[Talents & Traits]");
			foreach(var talent in talents)
				s.AppendLine(talent.ToString());
			s.AppendLine(sectionBreak);


			s.AppendLine("\t\t\t[Advancements]");
			foreach (var adv in advancements)
				s.AppendLine(advancements.ToString());
			s.AppendLine(sectionBreak);



			return s.ToString();
		}


		string[] forceTabSkillNames = new string[]
		{
			"Blather",
			"Ciphers",
			"Forebidden Lore: Heresy",
			"Forebidden Lore: Mutants",
			"Forebidden Lore: Psykers",
			"Forebidden Lore: The Black Library",
			"Medicae",
			"Literacy",
			"Pilot: Civilian Craft",
			"Pilot: Military Craft",
			"Pilot: Space Craft",
			"Scholastic Lore: Archaic",
			"Scholastic Lore: Beasts",
			"Scholastic Lore: Chymistry",
			"Scholastic Lore: Heraldry",
			"Scholastic Lore: Imperial Creed",
			"Scholastic Lore: Legend",
			"Scholastic Lore: Occult",
			"Scholastic Lore: Tactica Imperialis",
			"Secret Tongue: Military",
			"Security",
			"Survival",
			"Tech-Use",
			"Tracking",
			"Tracking",
			"Trade: Agri",
			"Trade: Apothecary",
			"Trade: Embalmer",
			"Trade: Merchant",
			"Trade: Merchant",
			"Trade: Prospector",
			"Trade: Technomat",
			"Wrangling"
		};

		bool NameRequiresExtraTab(string skill)
		{
			foreach(var skillName in forceTabSkillNames)
				if(skillName == skill)
					return true;

			return false;
		}

		#endregion


		#region----- EDITOR -----

		//*

		int _tabMod = 8;
		int _spaceMod = 8;
		bool _addSpaces = false;

		[ShowInInspector, PropertyOrder(101)]
		public int TabMod { get => _tabMod; set => SetProperty(ref _tabMod, value); }
		[ShowInInspector, PropertyOrder(101)]
		public int SpaceMod { get => _spaceMod; set => SetProperty(ref _spaceMod, value); }
		[ShowInInspector, PropertyOrder(101)]
		public bool AddSpaces { get => _addSpaces; set => SetProperty(ref _addSpaces, value); }

		[ValueDropdown("SkillDropdown"), PropertyOrder(100)]
		public Skill.Info testSkill;

		[Button("Add 'Test Skill'"), PropertyOrder(100)]
		public void Add() => Add(testSkill, testSkill.type);

		IEnumerable SkillDropdown() => SkillDatabase.GetAllSkillInfo().Select(x => new ValueDropdownItem(x.name, x));

		[Button("Add All Adv. Skills"), PropertyOrder(100)]
		void AddAllAdvancedSKills()
		{
			foreach(var skill in SkillDatabase.GetSkillsByType(Skill.Type.Advanced))
				Add(skill, skill.type);
		}

		// */

		#endregion

	}

}