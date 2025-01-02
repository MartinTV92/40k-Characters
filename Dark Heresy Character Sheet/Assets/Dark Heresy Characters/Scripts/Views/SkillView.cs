using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace SunJack.DarkHeresy
{
    public class SkillView : MonoBehaviour
    {
		#region----- VARIABLES -----

		public TextMeshProUGUI nameTxt;
        public TextMeshProUGUI characteristicTxt;
        public TextMeshProUGUI bonusTxt;
        public Toggle[] trainingLevels = new Toggle[3];

		public Skill skill;

		#endregion

		#region----- MONOBEHAVIOURS -----

		#endregion

		#region----- CUSTOM BEHAVIOURS -----

		public void SetSkill(Skill skill)
		{
			this.skill = skill;
			Redraw();
		}

		public void Redraw()
		{
			nameTxt.text = skill.name;
			characteristicTxt.text = $"({CharacteristicView.GetShortName(skill.characteristic)})";
			bonusTxt.text = $"(+{skill.bonus})";
			for(int i = 0; i < trainingLevels.Length; i++)
				trainingLevels[i].isOn = skill.mastery > 0 && skill.mastery >= i;
		}

		public void OpenInformationPopup()
		{

		}

		#endregion

	}
}