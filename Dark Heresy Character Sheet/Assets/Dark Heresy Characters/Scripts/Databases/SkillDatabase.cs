using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace SunJack.DarkHeresy
{
    [CreateAssetMenu(menuName = "Database Object / Skills")]
    public class SkillDatabase : ScriptableObject
    {
		#region----- VARIABLES -----

		private static SkillDatabase _instance;
        public static SkillDatabase Instance
        {
            get
            {
                if(_instance == null)
                    _instance = Resources.Load<SkillDatabase>("SkillDatabase");

                return _instance;
            }
            private set => _instance = value;
        }

		[OnValueChanged("Alphabetize")]
		public List<Skill.Info> skillInfo;

		#endregion


		#region----- METHODS -----

        // SEARCH FUNCTIONS

		public static List<Skill.Info> GetAllSkillInfo() => Instance.skillInfo;

        public static List<Skill> GetAllSkills() => GetSkillsFromInfo(Instance.skillInfo);

        public static List<Skill.Info> GetSkillInfoByType(Skill.Type skillType) => Instance.skillInfo.Where(x => x.type == skillType).ToList();

        public static List<Skill> GetSkillsByType(Skill.Type skillType) => GetSkillsFromInfo(GetSkillInfoByType(skillType));

        public static Skill.Info GetSkillInfo(string name) => Instance.skillInfo.Where(x => x.name == name).FirstOrDefault();

        public static Skill GetSkill(string name) => new Skill(Instance.skillInfo.Where(x => x.name == name).FirstOrDefault());

        private static List<Skill> GetSkillsFromInfo(List<Skill.Info> info) => GetSkillsFromInfo(info.ToArray());

        private static List<Skill> GetSkillsFromInfo(params Skill.Info[] info)
        {
			var result = new List<Skill>();
			foreach (var i in info)
				result.Add(new Skill(i));

			return result;
		}

        // SORT FUNCTIONS

		[Button("Alphabetize")]
		public void Alphabetize() => skillInfo.Sort((A, B) => string.Compare(A.name, B.name));

		#endregion
	}
}