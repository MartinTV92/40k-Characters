using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JollyRoger.DarkHeresy;

namespace JollyRoger.DarkHeresy.Data
{
    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public Character.Career career;
        public int xp;
        public int[] characteristics = new int[9];
        public List<AdvancementData> advancements = new ();

        public CharacterData()
        {

        }

        public CharacterData(Character character)
        {
            name = character.Name;
            career = character.CareerPath;
            xp = character.XP;

            for (int i = 0; i < characteristics.Length; i++)
                characteristics[i] = character.characteristics[(Characteristic.Type)i].Base;

            foreach(var a in character.advancements)
                advancements.Add(new AdvancementData(a));
        }
    }
}