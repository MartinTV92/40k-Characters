using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace JollyRoger.DarkHeresy
{ 

    public class TalentPopup : MonoBehaviour, IPopup
    {
		#region----- VARIABLES -----

		public TextMeshProUGUI title;
		public TextMeshProUGUI description;
		public ScrollRect scrollRect;

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
			title.text = talent.Name;
			description.text = talent.Description;
			scrollRect.verticalNormalizedPosition = 1;
		}

		public Type GetPopupType() => typeof(Talent);

		#endregion
	}
}