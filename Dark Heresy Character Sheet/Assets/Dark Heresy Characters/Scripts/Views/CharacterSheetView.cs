using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using JollyRoger.Utilities;
using System.Linq;
using TS.PageSlider;

namespace JollyRoger.DarkHeresy
{ 

    public class CharacterSheetView : MonoBehaviour
    {
        #region----- NESTED -----

        public enum Page
        {
            BasicSkills,
            AdvancedSkills,
            TalentsTraits
        }

        [System.Serializable] public class PageDictionary : UnitySerializedDictionary<Page, CharacterSheetPageView> { }

		#endregion


		#region----- VARIABLES -----

		public static CharacterSheetView Instance { get; private set; }

		#region UI

		[ShowInInspector] public static Character target;

		[FoldoutGroup("Basic Info")] public TextMeshProUGUI nameUI;
		[FoldoutGroup("Basic Info")] public TextMeshProUGUI careerUI;
		[FoldoutGroup("Basic Info")] public TextMeshProUGUI rankUI;
		[FoldoutGroup("Basic Info")] public Transform characteristicHolder;
		[FoldoutGroup("Basic Info")] public CharacteristicView[] characteristics;

		[FoldoutGroup("Skills")] public SkillView skillPrefab;
		[FoldoutGroup("Skills")] public List<SkillView> basicSkills;
		[FoldoutGroup("Skills")] public List<SkillView> advancedSkills;

		[FoldoutGroup("Talents")] public TalentView talentPrefab;
		[FoldoutGroup("Talents")] public List<TalentView> talents;


        public PageScroller scroller;
		public PageDictionary pageDict;

        // Object Pool
        ObjectPoolHelper<SkillView> skillPool;
        ObjectPoolHelper<TalentView> talentPool;

		#endregion

		#endregion


		#region----- MONOBEHAVIOURS -----

		void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

		private void Start()
		{
			SetUp();
		}

        #endregion


        #region----- CUSTOM BEHAVIOUR -----

        [Button("Setup")]
        public void SetUp()
        {
            characteristics = characteristicHolder.GetComponentsInChildren<CharacteristicView>();

            if(target != null)
                for(int i = 0; i < characteristics.Length; i++)
                    characteristics[i].Setup(target.characteristics[(Characteristic.Type) i]);

            skillPool = new ObjectPoolHelper<SkillView>(skillPrefab);
            talentPool = new ObjectPoolHelper<TalentView>(talentPrefab);

            scroller = GetComponentInChildren<PageScroller>();
            scroller.SetPage(0);

            Redraw();
        }

        public void SetCharacter(Character character)
        {
            target = character;
            Redraw();
        }

        public void Redraw()
        {
            nameUI.text = $"{target.Name}";
            careerUI.text = target.CareerPath.ToString();
            rankUI.text = $"-- (1/8)";

			for (int i = 0; i < characteristics.Length; i++)
				characteristics[i].Setup(target.characteristics[(Characteristic.Type)i]);

            RedrawSkills(Skill.Type.Basic);
			RedrawSkills(Skill.Type.Advanced);
            RedrawTalents();
		}

        // SKILLS
        void RedrawSkills(Skill.Type skillType)
        {
			var skillList = skillType == Skill.Type.Basic ? target.basicSkills : target.advancedSkills;
			var uiList = skillType == Skill.Type.Basic ? basicSkills : advancedSkills;
			var page = skillType == Skill.Type.Basic ? Page.BasicSkills : Page.AdvancedSkills;

			foreach (var ui in uiList)
                skillPool.Release(ui);

			foreach(var skill in skillList)
                Add(skill, skillType);
        }

        public void Add(Skill skill, Skill.Type skillType)
        {
            var page = skillType == Skill.Type.Basic ? Page.BasicSkills : Page.AdvancedSkills;
            var skillList = skillType == Skill.Type.Basic ? basicSkills : advancedSkills;

			var view = skillPool.Get();
			view.transform.SetParent(pageDict[page].content);
            view.gameObject.name = $"Skill_{skill.name}";
			view.transform.localScale = Vector3.one;
			view.SetSkill(skill);
			skillList.Add(view);
			pageDict[page].SetAppendages();
		}

        public void Remove(Skill skill)
        {
            var list2Search = skill.type == Skill.Type.Basic ? basicSkills : advancedSkills;
            var skillUI = list2Search.Where(x => x.name == skill.name).FirstOrDefault();
            skillPool.Release(skillUI);
        }


        // TALENTS & TRAITS
        void RedrawTalents()
        {
            foreach(var talentUI in talents)
                talentPool.Release(talentUI);

            foreach(var talent in target.talents)
                Add(talent);
        }

        public void Add(Talent talent)
        {
            var view = talentPool.Get();
            view.transform.SetParent(pageDict[Page.TalentsTraits].content);
            view.gameObject.name = $"Talent_{talent.name}";
            view.transform.localScale = Vector3.one;
            view.SetTalent(talent);
			pageDict[Page.TalentsTraits].SetAppendages();

		}

		public void Remove(Talent skill)
		{
			var view = talents.Where(x => x.name == skill.name).FirstOrDefault();
			talentPool.Release(view);
		}


		#endregion

	}
}