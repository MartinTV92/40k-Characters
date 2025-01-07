using UnityEngine;
using System;

namespace SunJack.DarkHeresy
{ 
    public class CharacteristicPopup : MonoBehaviour, IPopup
    {
		#region----- VARIABLES -----

		public Characteristic targetModel;
		public CharacteristicView editView;

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		public void Open<T>(T target)
		{
			if (target is Characteristic stat)
			{ 
				Redraw(stat);
				gameObject.SetActive(true);
			}
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}

		public Type GetPopupType() => typeof(Characteristic);

		void Redraw(Characteristic characteristic)
		{
			targetModel = characteristic;
			editView.Setup(characteristic);
		}

		public void Apply()
		{
			targetModel.Value = int.Parse(editView.valueTextInputField.text);
		}

		#endregion
	}
}