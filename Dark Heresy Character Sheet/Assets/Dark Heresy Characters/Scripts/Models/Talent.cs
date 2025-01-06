using UnityEngine;
using Sirenix.OdinInspector;

namespace SunJack.DarkHeresy
{ 
    /// <summary>
    /// Defines a trait or action the character has that don't need to be tested (i.e rolled).
    /// These include things like the ability to perform called-shits with no penalty, training in weapon groups, perfect recall
    /// and other miscellaneous things that can't be respresented by a numerical value.
    /// </summary>
    [System.Serializable]
    public struct Talent
    {

		[FoldoutGroup("$name")] public string name;

		/// <summary> Shorter description of the talent for use in smaller UI. </summary>
		[FoldoutGroup("$name")] public string shortDescription;

        /// <summary> Regular, detailed description of the talent that encompasses all rules. </summary>
        [TextArea(3, 15)]
		[FoldoutGroup("$name")] public string description;

		[FoldoutGroup("$name")] public string[] group;
    }
}