using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SunJack.DarkHeresy
{ 

    public class SkillController : MonoBehaviour
    {
		#region----- VARIABLES -----

		public SkillView view;
		public Button moreInfoButton;

		#endregion

		#region----- MONOBEHAVIOURS -----

		private void OnEnable() => moreInfoButton.onClick.AddListener(OpenSkillDescription);

		private void OnDisable() => moreInfoButton.onClick.RemoveListener(OpenSkillDescription);

		#endregion

		#region----- CUSTOM BEHAVIORUS -----

		void OpenSkillDescription() =>	SkillDescriptionView.Open(view.skill);

		#endregion
	}
}