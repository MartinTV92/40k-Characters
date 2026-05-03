using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace RegistrumPersonae
{
	/// <summary>
	/// Utility class for defining a field with a dropdown and relevant options for sh
	/// </summary>
	[System.Serializable, InlineProperty, HideLabel]
    public class UODUniversalDropdown
    {
		#region----- NESTED -----

		public enum Type
		{
			Skill,
			Talent
		}

		#endregion

		#region----- VARIABLES -----

		[HorizontalGroup("R0", Width = 0.6f), HideLabel]
		[PropertyOrder(-1), ValueDropdown("GetAdvancementName")]
		public string name;

		int _mastery = 0;
		[HorizontalGroup("R0", Width = 0.1f), ShowInInspector, PropertyOrder(-1)]
		[LabelText("+"), HideLabel, LabelWidth(10), ShowIf("type", Type.Skill)]
		public int mastery
		{
			get => _mastery;
			set => _mastery = Mathf.Clamp(value, 0, Skill.MAX_MASTERY);
		}

		[HorizontalGroup("R0"), HideLabel]
		public Type type;

		#endregion

		#region----- BEHAVIOURS -----

		IEnumerable GetAdvancementName()
		{
			if (type == Type.Skill)
				return SkillDatabase.SkillDropdown();
			else
				return TalentDatabase.TalentDropdown();
		}

		public string ReadFriendlyName()
		{
			if (type == Type.Talent)
				return name;

			var suffix = mastery > 0 ? $" (+{mastery * Skill.MASTERY_BONUS})" : "";
			return name + suffix;
		}

		#endregion
	}
}