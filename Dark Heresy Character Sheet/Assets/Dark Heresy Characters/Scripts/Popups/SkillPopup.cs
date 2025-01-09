using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace JollyRoger.DarkHeresy
{
    /// <summary>
    /// Popup window for showing more detailed information on a skill.
    /// </summary>
    public class SkillPopup : MonoBehaviour, IPopup
    {
		#region----- VARIABLES -----

		public TextMeshProUGUI title;
        public TextMeshProUGUI description;
		public ScrollRect scrollRect;

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		public void Open<T>(T target)
		{
			if(target is Skill skill)
			{ 
				Redraw(skill);
				gameObject.SetActive(true);
			}
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}

		public void Redraw(Skill skill)
        {
			title.text = skill.name;
            description.text = skill.description;
			scrollRect.verticalNormalizedPosition = 1;
        }

		public Type GetPopupType() => typeof(Skill);

		#endregion
	}
}