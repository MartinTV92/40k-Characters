using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SunJack.DarkHeresy
{ 
    public class CharacteristicController : MonoBehaviour
    {
		#region----- VARIABLES ------

		// Statics
		public static CharacteristicController Instance { get; private set; }

		public CharacteristicView targetView;
		public Characteristic targetModel;
		public CharacteristicView editView;

		#endregion


		#region----- MONOBEHAVIOURS -----

		private void Awake()
		{
			if(Instance == null)
				Instance = this;
		}

		private void Start()
		{
			//Setup();
		}

		#endregion


		#region----- BEHAVIOURS -----

		public static void Open(CharacteristicView view)
		{
			if(!Instance)
				return;

			Instance.transform.parent.gameObject.SetActive(true);
			Instance.gameObject.SetActive(true);
			Instance.targetView = view;
			Instance.targetModel = view.targetModel;
			Instance.editView.targetModel = view.targetModel;
			Instance.editView.Redraw();
		}

		public static void Open(int viewIndex)
		{
			if(!Instance)
				return;

			var view = CharacterSheetView.Instance.characteristics[viewIndex];
			Open(view);
		}

		public static void Close()
		{
			if(!Instance)
				return;

			Instance.gameObject.SetActive(false);
		}
		
		public void Apply()
		{
			targetModel.Value = int.Parse(editView.valueTextInputField.text);
			targetView.Redraw();
		}

		void Setup()
		{
			for(int i = 0; i < CharacterSheetView.Instance.characteristics.Length; i++)
			{
				var view = CharacterSheetView.Instance.characteristics[i];
				view.editButton.onClick.AddListener(() => Open(i));
			}
		}

		#endregion
	}
}