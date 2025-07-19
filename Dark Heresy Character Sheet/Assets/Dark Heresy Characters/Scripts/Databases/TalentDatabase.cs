using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System.Collections;

namespace JollyRoger.DarkHeresy
{
	[CreateAssetMenu(menuName = "Database Object / Talents")]
	public class TalentDatabase : ScriptableObject
    {
		#region----- VARIABLES -----

		private static TalentDatabase _instance;
		public static TalentDatabase Instance
		{
			get
			{
				if (_instance == null)
					_instance = Resources.Load<TalentDatabase>("TalentDatabase");

				return _instance;
			}
			private set => _instance = value;
		}

		[OnValueChanged("Alphabetize")]
		public List<Talent> talents;

		#endregion

		#region----- METHODS -----

		[Button("Alphabetize")]
		private void Alphabetize() => talents.Sort((System.Comparison<Talent>)((A, B) => string.Compare((string)A.Name, (string)B.Name)));

		public static Talent Get(string name) => Instance.talents.Where(x => x.Name == name).FirstOrDefault();

		public static List<Talent> GetAll() => Instance.talents;

		public static IEnumerable TalentDropdown() => Instance.talents.Select((System.Func<Talent, string>)(x => (string)x.Name));

		#endregion
	}
}