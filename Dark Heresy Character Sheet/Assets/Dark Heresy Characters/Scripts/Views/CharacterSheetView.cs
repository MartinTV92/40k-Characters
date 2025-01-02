using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace SunJack.DarkHeresy
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

		public PageDictionary pageDict;

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

            Redraw();
        }

        public void SetCharacter(Character character)
        {
            target = character;
            Redraw();
        }

        public void Redraw()
        {
            nameUI.text = $"+{target.Name}+";
            careerUI.text = target.CareerPath.ToString();
            rankUI.text = $"-- (1/8)";

			for (int i = 0; i < characteristics.Length; i++)
				characteristics[i].Setup(target.characteristics[(Characteristic.Type)i]);

            RedrawSkills(Skill.Type.Basic);
		}

        void RedrawSkills(Skill.Type skillType)
        {
			var skillList = skillType == Skill.Type.Basic ? target.basicSkills : target.advancedSkills;
			var uiList = skillType == Skill.Type.Basic ? basicSkills : advancedSkills;

			for (int i = 0; i < skillList.Count; i++)
            {
                var need2Add = uiList.Count < skillList.Count;
                if(need2Add)
                    AddSkill(skillList[i], skillType);
                else
                    break;
            }

            if(uiList.Count > target.basicSkills.Count)
                for(int i = uiList.Count - 1; i >= 0; i--)
                    RemoveSkill(skillList[i], skillType);
        }

        void AddSkill(Skill skill, Skill.Type skillType)
        {
            var page = skillType == Skill.Type.Basic ? Page.BasicSkills : Page.AdvancedSkills;
            var list = skillType == Skill.Type.Basic ? basicSkills : advancedSkills;
			var skillUI = Instantiate(skillPrefab, pageDict[page].content);
            skillUI.gameObject.name = $"Skill_{skill.name}";
            skillUI.SetSkill(skill);
            list.Add(skillUI);
        }

        void RemoveSkill(Skill skill, Skill.Type skillType)
        {
			var page = skillType == Skill.Type.Basic ? Page.BasicSkills : Page.AdvancedSkills;
			var list = skillType == Skill.Type.Basic ? basicSkills : advancedSkills;
            var index = list.FindIndex(x => x.name == skill.name);
			Destroy(list[index].gameObject);
            list.RemoveAt(index);
		}

		#endregion
	}
}