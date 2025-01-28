using UnityEngine;
using Sirenix.OdinInspector;

namespace JollyRoger.DarkHeresy
{
    /// <summary>
    /// Handles defining when the user is presented with a choice of some sort in any combination. 
    /// I.e skill OR talent, or Skill or another skill etc.
    /// </summary>
    [System.Serializable]
    public class Choice
    {
        #region----- NESTED -----

        public enum Type
        {
            Skill,
            Talent,
            Item
        }

        [System.Serializable]
        public struct Entry
        {
            public UODUniversalDropdown[] option;
        }

		#endregion

        public Entry[] choice;
    }
}