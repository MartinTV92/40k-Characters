using UnityEngine;
using Sirenix.OdinInspector;

namespace SunJack.DarkHeresy
{
    /// <summary>
    /// Defines an apptitude for a given area of expertise such as driving, knowledge or tech-use.
    /// Furthermore, defines the mastery and other bonuses applied to skill.
    /// </summary>
    [System.Serializable]
    public struct Skill
    {
        [System.Flags]
        public enum Descriptor
        {
            None = 0,
            Crafting = 1 << 0,
            Interaction = 1 << 1,
            Investigation = 1 << 2,
            Movement = 1 << 3,
            Operator = 1 << 4
        }

        public enum Type
        {
            Advanced,
            Basic
        }

        private const int MAX_MASTERY = 2;
        private const int MAX_BONUS = 60;
        private const int MIN_BONUS = -60;

        [FoldoutGroup("$name")]

        [HorizontalGroup("$name/R0", Width = 0.7f), LabelWidth(40)] 
        public string name;

		[HorizontalGroup("$name/R0", Width = 0.3f), HideLabel, SerializeField]
        public Type type;

		[HorizontalGroup("$name/R1"), HideLabel, SerializeField] 
        private Characteristic.Type _characteristic;

        [HorizontalGroup("$name/R1"), HideLabel, SerializeField]
        private Descriptor _descriptor;

		[FoldoutGroup("$name")]
		public string[] group;

		[FoldoutGroup("$name/Description"), TextArea(3, 15), HideLabel]
        public string description;

		[HideInInspector] 
        public int miscBonus;

		private int _mastery;
        public int mastery { get => _mastery; private set => _mastery = Mathf.Clamp(value, 0, MAX_MASTERY); }
        public int bonus => Mathf.Clamp((mastery * 10) + miscBonus, MIN_BONUS, MAX_BONUS);
        public Characteristic.Type characteristic { get => _characteristic; }
        public Descriptor descriptor { get => _descriptor; }
    }
}