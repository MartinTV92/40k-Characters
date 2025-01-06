using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace SunJack.DarkHeresy
{
	[CreateAssetMenu(menuName = "Database Object / Talents")]
	public class TalentDatabase : ScriptableObject
    {
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

		[Button("Alphabetize")]
		private void Alphabetize() => talents.Sort((A, B) => string.Compare(A.name, B.name));

		public static Talent Talent(string name) => Instance.talents.Where(x => x.name == name).FirstOrDefault();
	}
}