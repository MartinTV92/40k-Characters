using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace JollyRoger.DarkHeresy
{ 
    public class TalentView : MonoBehaviour
    {
        public TextMeshProUGUI talentName;
        public TextMeshProUGUI shortDescription;
        public Talent talent;

        public void SetTalent(Talent talent)
        {
            this.talent = talent;
            Redraw();
        }

        void Redraw()
        {
            talentName.text = talent.Name;
            shortDescription.text = talent.ShortDescription;
        }
    }
}