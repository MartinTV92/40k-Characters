using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace SunJack.DarkHeresy
{ 

    public class CharacterSheetView : MonoBehaviour
    {

        #region----- VARIABLES -----

        public static CharacterSheetView Instance { get; private set; }

        [ShowInInspector]
        public static Character target;

        // UI
        public TextMeshProUGUI nameUI;
        public TextMeshProUGUI careerUI;
        public TextMeshProUGUI rankUI;
        public Transform characteristicHolder;
        public CharacteristicView[] characteristics; 

		#endregion


		#region----- MONOBEHAVIOURS -----

		void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

		private void Start()
		{
			SetUp();
		}

        #endregion


        #region----- CUSTOM BEHAVIOUR -----

        [Button("Setup")]
        public void SetUp()
        {
            characteristics = characteristicHolder.GetComponentsInChildren<CharacteristicView>();

            if(target != null)
                for(int i = 0; i < characteristics.Length; i++)
                    characteristics[i].Setup(target.characteristics[(Characteristic.Type) i]);

            Redraw();
        }

        public void SetCharacter(Character character)
        {
            target = character;
            Redraw();
        }

        public void Redraw()
        {
            nameUI.text = $"+{target.Name}+";
            careerUI.text = target.CareerPath.ToString();
            rankUI.text = $"-- (1/8)";

			for (int i = 0; i < characteristics.Length; i++)
				characteristics[i].Setup(target.characteristics[(Characteristic.Type)i]);
		}

		#endregion
	}
}