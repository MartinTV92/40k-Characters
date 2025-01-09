using UnityEngine;
using UnityEngine.UI;

namespace JollyRoger.DarkHeresy
{ 
    public class CharacteristicController : MonoBehaviour
    {
		#region----- VARIABLES ------

		public CharacteristicView view;
		public Characteristic model;
		public Button editButton;

		#endregion


		#region----- MONOBEHAVIOURS -----

		private void OnEnable() => editButton.onClick.AddListener(Open);

		private void OnDisable() => editButton.onClick.RemoveListener(Open);

		#endregion


		#region----- BEHAVIOURS -----

		/// <summary>
		/// Opens the characteristic edit window.
		/// </summary>
		public void Open() => PopupManager.Open(view.targetModel);

		#endregion
	}
}