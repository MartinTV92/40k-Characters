using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JollyRoger.DarkHeresy
{ 

    public class CharacterSheetPageView : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public RectTransform content;
        public RectTransform[] appendages;

        /// <summary>
        /// Ensures that all items that must be set last, such as 'Add' buttons
        /// are set to be the last items in the sibling.
        /// </summary>
        public void SetAppendages()
        {
            foreach (var item in appendages)
                item.SetAsLastSibling();
        }
    }
}