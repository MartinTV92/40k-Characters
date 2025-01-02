using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SunJack.UI
{
    [RequireComponent(typeof(LayoutElement))]
    public class DynamicLayoutElementMatcher : MonoBehaviour
    {
        private LayoutElement _layout;
        public LayoutElement layout
        {
            get
            {
                if(_layout == null)
                    _layout = GetComponent<LayoutElement>();

                return _layout;
            }
        }

        public void Resize(Vector2 size) => Resize(size.x, size.y);

        public void Resize(float x, float y)
        {
            layout.preferredWidth = x;
            layout.preferredHeight = y;
        }
	}
}