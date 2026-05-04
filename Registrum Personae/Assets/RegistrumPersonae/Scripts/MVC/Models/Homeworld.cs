using Sirenix.OdinInspector;
using JollyRoger.Collections;

namespace RegistrumPersonae
{
	/* TODO
	 * Add starting characteristics, as +/- from standard starting value of 20
	 * Add starting Skills/Talents/Traits
	 * Eventually hookup to database so starting skills etc, can change depending on homeworld (if specifiied)
	 */

	[System.Serializable, HideReferenceObjectPicker, InlineProperty]
	public class Homeworld : NotifyPropertyChangedWrapper
	{
		#region----- NESTED -----

		public enum Type
		{
			FeralWorld,
			HiveWorld,
			ImperialWorld,
			VoidBorn
		}

		#endregion

		#region----- VARIABLES -----

		private string _name = "";
		[ShowInInspector, HorizontalGroup]
		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		private Type _world;
		[ShowInInspector, HorizontalGroup]
		public Type World
		{
			get => _world;
			set => SetProperty(ref _world, value);
		}


		#endregion

		#region----- METHODS -----

		public Advancement[] GetStartingAdvances() => new Advancement[] { };

		#endregion
	}
}

