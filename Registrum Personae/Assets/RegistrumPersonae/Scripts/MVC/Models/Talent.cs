using System;
using UnityEngine;
using Sirenix.OdinInspector;
using JollyRoger.Collections;

namespace RegistrumPersonae
{ 
	/// <summary>
	/// Defines a trait or action the character has that don't need to be tested (i.e rolled).
	/// These include things like the ability to perform called-shots with no penalty, training in weapon groups, perfect recall
	/// and other miscellaneous things.
	/// </summary>
	[System.Serializable]
	public class Talent : NotifyPropertyChangedWrapper
	{
	    /* TODO
	     * Remove this class entirely.
	     * In truth, this would simply be a list of strings that 
	     * would act as keys in a database.
	     * All the information would be gotten and applied from there
	     * Unless I want the effect to be referenced in this class for some reason
	     */


	    string _name;
		[FoldoutGroup("$Name"), ShowInInspector] 
	    public string Name
	    {
	        get => _name;
	        set => SetProperty(ref _name, value);
	    }

	    string _shortDescription;
		/// <summary> Shorter description of the talent for use in smaller UI. </summary>
		[FoldoutGroup("$Name")] 
	    public string ShortDescription
	    { 
	        get => _shortDescription;
	        set => SetProperty(ref _shortDescription, value);
	    }


	    string _description;
	    /// <summary> Regular, detailed description of the talent that encompasses all rules. </summary>
	    [MultiLineProperty(15), ShowInInspector]
		[FoldoutGroup("$Name")] public string Description
	    {
	        get => _description;
	        set => SetProperty(ref _description, value);
	    }

		public override string ToString() => Name;
	}
}

