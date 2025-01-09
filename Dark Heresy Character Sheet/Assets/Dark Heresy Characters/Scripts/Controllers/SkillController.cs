using UnityEngine;
using UnityEngine.UI;

namespace JollyRoger.DarkHeresy
{ 

    public class SkillController : MonoBehaviour
    {
		#region----- VARIABLES -----

		public SkillView view;
		public Button moreInfoButton;

		#endregion

		#region----- MONOBEHAVIOURS -----

		private void OnEnable() => moreInfoButton.onClick.AddListener(Open);

		private void OnDisable() => moreInfoButton.onClick.RemoveListener(Open);

		#endregion

		#region----- CUSTOM BEHAVIORUS -----

		void Open() =>	PopupManager.Open(view.skill);

		#endregion
	}
}