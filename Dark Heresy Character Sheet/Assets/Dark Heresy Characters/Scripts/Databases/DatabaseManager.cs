using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JollyRoger.DarkHeresy
{

    public static class DatabaseManager
    {

		#region Skills

		public static List<Skill.Info> GetAllSkillInfo() => SkillDatabase.GetAllSkillInfo();

		public static List<Skill> GetAllSkills() => SkillDatabase.GetAllSkills();

		public static List<Skill.Info> GetSkillInfoByType(Skill.Type skillType) => SkillDatabase.GetSkillInfoByType(skillType);

		public static List<Skill> GetSkillsByType(Skill.Type skillType) => SkillDatabase.GetSkillsByType(skillType);

		public static Skill.Info GetSkillInfo(string name) => SkillDatabase.GetSkillInfo(name);

		public static Skill GetSkill(string name) => SkillDatabase.GetSkill(name);

		public static IEnumerable SkillDropdown() => SkillDatabase.SkillDropdown();

		#endregion


		#region Talents

		public static Talent GetTalent(string name) => TalentDatabase.Get(name);

		public static List<Talent> GetAllTalents() => TalentDatabase.GetAll();

		public static IEnumerable TalentDropdon() => TalentDatabase.TalentDropdown();

		#endregion
	}
}