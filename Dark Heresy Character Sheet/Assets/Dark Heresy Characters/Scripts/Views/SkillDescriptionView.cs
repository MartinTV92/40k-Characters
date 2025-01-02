using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SunJack.DarkHeresy
{
    /// <summary>
    /// Popup window for showing more detailed information on a skill.
    /// </summary>
    public class SkillDescriptionView : MonoBehaviour
    {
		#region----- VARIABLES -----

        public static SkillDescriptionView Instance { get; private set; }
		public TextMeshProUGUI title;
        public TextMeshProUGUI description;
		public ScrollRect scrollRect;

		#endregion


		#region----- MONOBEHAVIOURS -----

		private void Awake()
		{
			Instance = this;
		}


		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		public static void Open(Skill skill)
		{
			if(Instance == null)
				return;

			Instance.Redraw(skill);
			Instance.transform.parent.gameObject.SetActive(true);
		}

		public static void Close()
		{
			Instance.transform.parent.gameObject.SetActive(false);
		}

		public void Redraw(Skill skill)
        {
            title.text = skill.name;
            description.text = skill.description;
			scrollRect.verticalNormalizedPosition = 1;
        }

		#endregion
	}
}
