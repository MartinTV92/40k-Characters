using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunJack.DarkHeresy
{ 

	public class CharacterSheetController : MonoBehaviour
	{
		#region----- VARIABLES -----

		public static CharacterSheetController Instance { get; private set; }


		#endregion


		#region----- MONOBEHAVIOUR -----

		private void Awake()
		{
			Instance = this;
		}

		#endregion

		#region----- CUSTOM BEHAVIOURS -----

		public void SetCharacter(Character character)
		{
			CharacterSheetView.Instance.SetCharacter(character);
		}

		#endregion
	}
}