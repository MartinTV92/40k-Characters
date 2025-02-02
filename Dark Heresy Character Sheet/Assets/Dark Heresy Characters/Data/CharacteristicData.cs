using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JollyRoger.DarkHeresy.Data
{
	public class CharacteristicData
    {
        public int value = 0;

        public CharacteristicData()
        {

        }

        public CharacteristicData(Characteristic characteristic)
        {
            value = characteristic.Base;
        }
    }
}
