using UnityEngine;
using System;

namespace RegistrumPersonae
{ 

	public class TalentPopup : MonoBehaviour, IPopup
	{
		#region----- VARIABLES -----

		//public TextMeshProUGUI title;
		//public TextMeshProUGUI description;
		//public ScrollRect scrollRect;

		#endregion

		#region----- CUSTOM BEHAVIOURS -----

		public void Open<T>(T target)
		{
			if (target is Talent talent)
			{
				Redraw(talent);
				gameObject.SetActive(true);
			}
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}

		public void Redraw(Talent talent)
		{
			//title.text = talent.Name;
			//description.text = talent.Description;
			//scrollRect.verticalNormalizedPosition = 1;
		}

		public Type GetPopupType() => typeof(Talent);

		#endregion
	}
}

