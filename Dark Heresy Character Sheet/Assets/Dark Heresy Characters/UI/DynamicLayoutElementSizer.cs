using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace JollyRoger.UI
{ 
	/// <summary>
	/// For use with PageSliders, ensures that the pages match the size of the window they'll be in.
	/// The size fitting requires the use of particular components, LayoutElements in particular.
	/// </summary>
	public class DynamicLayoutElementSizer : Selectable
	{
		/// <summary> The RectTransform to match the size of. Typically the object with this component. </summary>
		public RectTransform rect2Match;
		/// <summary> The parent that holds all the pages i.e LayoutElements </summary>
		public RectTransform elementHolder;
		/// <summary> The LayoutElements to resize. </summary>
		public DynamicLayoutElementMatcher[] elements;

		/// <summary> Calls the resize function whenever the UI element's size is changed so UI can be made in editor. </summary>
		protected override void OnRectTransformDimensionsChange()
		{
			Resize();
		}

		#if UNITY_EDITOR

		protected override void OnValidate()
		{
			Resize();
		}

		#endif

		[Button("Resize")]
		/// <summary> Resizes all the LayoutElements' prefered widths and heights. </summary>
		private void Resize()
		{
			if(!rect2Match || !elementHolder)
				return;

			elements = GetComponentsInChildren<DynamicLayoutElementMatcher>();
			var size = new Vector2(rect2Match.rect.width, rect2Match.rect.height);
			for (int i = 0; i < elements.Length; i++)
				elements[i].Resize(size);
		}
	}
}