using UnityEngine;
using Sirenix.OdinInspector;

namespace JollyRoger.DarkHeresy
{ 
    /// <summary>
    /// Defines a trait or action the character has that don't need to be tested (i.e rolled).
    /// These include things like the ability to perform called-shots with no penalty, training in weapon groups, perfect recall
    /// and other miscellaneous things.
    /// </summary>
    [System.Serializable]
    public class Talent
    {

		[FoldoutGroup("$name")] public string name;

		/// <summary> Shorter description of the talent for use in smaller UI. </summary>
		[FoldoutGroup("$name")] public string shortDescription;

        /// <summary> Regular, detailed description of the talent that encompasses all rules. </summary>
        [TextArea(3, 15)]
		[FoldoutGroup("$name")] public string description;
    }
}