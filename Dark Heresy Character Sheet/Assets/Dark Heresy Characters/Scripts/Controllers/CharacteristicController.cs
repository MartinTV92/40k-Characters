using UnityEngine;
using UnityEngine.UI;

namespace SunJack.DarkHeresy
{ 
    public class CharacteristicController : MonoBehaviour
    {
		#region----- VARIABLES ------

		public CharacteristicView view;
		public Characteristic model;
		public Button editButton;

		#endregion


		#region----- MONOBEHAVIOURS -----

		private void OnEnable() => editButton.onClick.AddListener(OpenEditPopup);

		private void OnDisable() => editButton.onClick.RemoveListener(OpenEditPopup);

		#endregion


		#region----- BEHAVIOURS -----

		public void OpenEditPopup()
		{
			PopupManager.Open(view.targetModel);
		}

		#endregion
	}
}