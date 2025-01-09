using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JollyRoger.DarkHeresy
{

	[CreateAssetMenu(menuName = "Database Object / Career Path")]
	public class Path : ScriptableObject
	{
		public string pathName;
		public Advancement[] advances;
	}
}