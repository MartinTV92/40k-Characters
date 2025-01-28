using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace JollyRoger.DarkHeresy
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
			UnSubscribe();
			this.skill = skill;
			Subscribe();
			Redraw();
		}

		public void Redraw()
		{
			nameTxt.text = skill.name;
			characteristicTxt.text = $"({CharacteristicView.GetShortName(skill.characteristicType)})";
			bonusTxt.text = $"{skill.GetBonus(GameManager.Current.characteristics[skill.characteristicType]):+#;-#;+0}";
			for(int i = 0; i < trainingLevels.Length; i++)
				trainingLevels[i].isOn = skill.mastery >= i;
		}

		void Subscribe()
		{
			skill.OnValueChanged += Redraw;
			GameManager.Current.characteristics[skill.characteristicType].OnValueChanged += Redraw;
		}

		void UnSubscribe()
		{
			skill.OnValueChanged -= Redraw;
			GameManager.Current.characteristics[skill.characteristicType].OnValueChanged -= Redraw;
		}

		#endregion

	}
}