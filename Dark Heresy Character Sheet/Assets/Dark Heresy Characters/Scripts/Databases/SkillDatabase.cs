using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace SunJack.DarkHeresy
{
    [CreateAssetMenu(menuName = "Database Object / Skills")]
    public class SkillDatabase : ScriptableObject
    {
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
        public List<Skill> skills;


        [Button("Alphabetize")]
        private void Alphabetize() => skills.Sort((A,B) => string.Compare(A.name, B.name) );


        public static List<Skill> GetSkillsByType(Skill.Type skillType) => Instance.skills.Where(x => x.type == skillType).ToList();

        public static Skill GetSkill(string name) => Instance.skills.Where(x => x.name == name).FirstOrDefault();
    }
}