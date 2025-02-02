using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JollyRoger.DarkHeresy;

namespace JollyRoger.DarkHeresy.Data
{ 

    public class CharacterData
    {
        public string name;
        public Character.Career career;
        public int xp;
        public int[] characteristics = new int[9]; 
        public SkillData[] basicSkills = new SkillData[0];
        public SkillData[] advancedSkillsa = new SkillData[0];
        public TalentData[] talents = new TalentData[0];


        public CharacterData()
        {

        }

        public CharacterData(Character character)
        {
            name = character.Name;
            career = character.CareerPath;
            xp = character.xp;

            for (int i = 0; i < characteristics.Length; i++)
                characteristics[i] = character.characteristics[(Characteristic.Type)i].Base;
        }
    }
}