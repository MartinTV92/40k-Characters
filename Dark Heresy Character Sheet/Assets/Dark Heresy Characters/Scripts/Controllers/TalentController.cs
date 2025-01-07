using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SunJack.DarkHeresy
{ 

    public class TalentController : MonoBehaviour
    {
		#region----- VARIABLES -----

		public TalentView view;
		public Button moreInfoButton;

		#endregion

		#region----- MONOBEHAVIOURS -----

		private void OnEnable() => moreInfoButton.onClick.AddListener(Open);

		private void OnDisable() => moreInfoButton.onClick.RemoveListener(Open);

		#endregion

		#region----- CUSTOM BEHAVIORUS -----

		void Open() => PopupManager.Open(view.talent);

		#endregion
	}
}