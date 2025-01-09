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
		private void Alphabetize() => talents.Sort((A, B) => string.Compare(A.name, B.name));

		public static Talent GetTalent(string name) => Instance.talents.Where(x => x.name == name).FirstOrDefault();

		public static List<Talent> GetAllTalents() => Instance.talents;

		public static IEnumerable TalentDropdown() => Instance.talents.Select(x => x.name);

		#endregion
	}
}