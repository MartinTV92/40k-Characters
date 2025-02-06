using UnityEngine;
using TMPro;
using JollyRoger.Data;

namespace JollyRoger.DarkHeresy
{ 
    public class SaveFileUIView : MonoBehaviour
    {
		#region----- VARIABLES -----

		public SaveFileUI model;
        public TextMeshProUGUI fileNameText;

		#endregion

		#region----- MONOBEHAVIOURS -----

		private void Awake()
		{
			model = GetComponent<SaveFileUI>();
			model.OnFileNameChange += Redraw;
		}

		private void OnDestroy()
		{
			model.OnFileNameChange -= Redraw;
		}

		#endregion

		#region----- CUSTOM BEHAVIOUR -----

		public void Redraw()
		{
			fileNameText.text = SaveManager.LoadCharacter(model.fileName).name;
		}

		#endregion
	}
}