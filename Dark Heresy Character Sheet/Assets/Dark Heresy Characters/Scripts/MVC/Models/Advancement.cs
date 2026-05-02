using System.Collections;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using JollyRoger.Collections;

namespace JollyRoger.DarkHeresy
{
    [InlineProperty, System.Serializable]
    public class Advancement : NotifyPropertyChangedWrapper
    {
		#region----- NESTED -----

        public enum Type
        {
            Skill,
            Talent,
            Characteristic
        }

        #endregion

        [HorizontalGroup("R0", Width = 0.4f), HideLabel, PropertyOrder(-1)]
        [OnValueChanged("SetObjectReference"), ValueDropdown("GetAdvancementName")]
		public string name;

        private object _value;
        public object Value
        {
            get => _value;
            private set => _value = value;
        }

        int _mastery = 0;
		[HorizontalGroup("R0", Width = 0.1f), ShowInInspector, PropertyOrder(-1)]
        [LabelText("+"), HideLabel, LabelWidth(10), ShowIf("type", Type.Skill)]
		public int mastery
        {
            get => _mastery;
            set => _mastery = Mathf.Clamp(value, 0, Skill.MAX_MASTERY);
        }

		[HorizontalGroup("R0"), LabelWidth(20)]
		public int xp = 100;

		[HorizontalGroup("R0"), HideLabel]
		public Type type;
        public Prerequisite[] prerequisites;

        public int XP { get => xp; }

        /// <summary>
        /// Tests if the character can purchase the advancement
        /// </summary>
        /// <param name="character"> The character attempting purchase the advancement </param>
        /// <returns>True if the character has XP and prerequisites. </returns>
        public bool CanBuy(Character character)
        {
            if(character.XP < xp)
                return false;

            foreach(Prerequisite prereq in prerequisites)
                if(prereq.IsMet(character) == false)
                    return false;

            return true;
        }

        IEnumerable GetAdvancementName()
        {
            if(type == Type.Skill)
                return SkillDatabase.SkillDropdown();
            else
				return TalentDatabase.TalentDropdown();
		}

        void SetObjectReference()
        {
			if (type == Type.Skill)
				Value = SkillDatabase.GetSkillInfo(name);
			else
				Value = TalentDatabase.Get(name);
		}

        public string ReadFriendlyName()
        {
            if(type == Type.Talent)
                return name;

            var suffix = mastery > 0 ? $" (+{mastery * Skill.MASTERY_BONUS})" : "";
            return name + suffix;
        }

		public override string ToString()
		{
			return $"{ReadFriendlyName()} ({xp})";
		}
	}
}