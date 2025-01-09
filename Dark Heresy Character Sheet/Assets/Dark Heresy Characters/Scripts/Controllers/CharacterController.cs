using UnityEngine;

namespace JollyRoger.DarkHeresy
{ 

	public class CharacterController : MonoBehaviour
	{
		#region----- VARIABLES -----

		public static CharacterController Instance { get; private set; }

		#endregion


		#region----- MONOBEHAVIOUR -----

		private void Awake()
		{
			Instance = this;
		}

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		/// <summary>
		/// Sets the character the view will need to display
		/// </summary>
		/// <param name="character"></param>
		public void SetCharacter(Character character)
		{
			CharacterSheetView.Instance.SetCharacter(character);
		}

		#endregion
	}
}