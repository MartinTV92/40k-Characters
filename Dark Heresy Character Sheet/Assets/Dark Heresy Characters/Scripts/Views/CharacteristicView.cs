using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System.Text.RegularExpressions;

namespace SunJack.DarkHeresy
{ 

	public class CharacteristicView : MonoBehaviour
	{
		#region----- VARIABLES -----

		public Characteristic targetModel;

		// Settings
		public bool _useShortName = false;
		public bool useShortName
		{
			get => _useShortName;
			set
			{
				_useShortName = value;
				Redraw();
			}
		}

		// References
		public TextMeshProUGUI nameText;
		public TextMeshProUGUI valueText;
		public TMP_InputField valueTextInputField;
		public Button editButton;
		public Toggle[] advancements;

		#endregion

		#region----- MONOBEHAVIOURS -----

		#endregion

		#region----- CUSTOM BEHAVIOURS -----

		// Static
		public static string GetShortName(Characteristic.Type charType)
		{
			switch(charType)
			{
				case Characteristic.Type.WeaponSkill: 
					return "WS";
				case Characteristic.Type.BallisticSkill: 
					return "BS";
				case Characteristic.Type.Strength: 
					return "S";
				case Characteristic.Type.Toughness:
					return "T";
				case Characteristic.Type.Agility: 
					return "Ag";
				case Characteristic.Type.Intelligence: 
					return "Int";
				case Characteristic.Type.Perception: 
					return "Per";
				case Characteristic.Type.WillPower: 
					return "WP";
				case Characteristic.Type.Fellowship: 
					return "Fel";
			}

			return default;
		}

		public static string GetLongName(Characteristic.Type charType)
		{
			return Regex.Replace(charType.ToString(), "(\\B[A-Z])", " $1");
		}

		public void Setup(Characteristic characteristic)
		{
			targetModel = characteristic;
			Redraw();
		}

		[Button("Redraw")]
		public void Redraw()
		{
			if(nameText == null)
				Debug.Log($"Name Text in {name} is null");
			if(targetModel == null)
				Debug.Log($"target in {name} is null");

			nameText.text = useShortName ? GetShortName(targetModel.characteristic) : GetLongName(targetModel.characteristic);
			
			if(valueTextInputField)
				valueTextInputField.SetTextWithoutNotify(targetModel.Value.ToString());
			else
				valueText.text = targetModel.Value.ToString();

			for (int i = 0; i < advancements.Length; i++)
				advancements[i].enabled = targetModel.advances[i].purchased;
		}

		#endregion
	}

}