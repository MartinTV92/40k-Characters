using UnityEngine;
using System;

namespace JollyRoger.DarkHeresy
{ 
    public class CharacteristicPopup : MonoBehaviour, IPopup
    {
		#region----- VARIABLES -----

		public Characteristic targetModel;

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
		}

		public void Apply()
		{
			//targetModel.Base = int.Parse(editView.valueTextInputField.text);
		}

		#endregion
	}
}