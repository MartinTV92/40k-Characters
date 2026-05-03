using UnityEngine;

namespace RegistrumPersonae.UI.Utilities
{
    /// <summary>
    /// Resizes a UI element to the mobile device's safe area.
	/// Place this on the parent most object of the UIs in the application
    /// </summary>
    public class SafeAreaResizer : MonoBehaviour
    {
		#region----- VARIABLES -----

		public RectTransform rectTransform;

		#endregion


		#region----- MONOBEHAVIOURS -----

		void Awake()
        {
			if (!rectTransform)
				rectTransform = GetComponent<RectTransform>();

			OnRectTransformDimensionsChange();
		}

		private void OnRectTransformDimensionsChange()
		{
			FitToSafeArea();
		}

		private void OnValidate()
		{
			if(!rectTransform)
				rectTransform = GetComponent<RectTransform>();
		}

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		public void FitToSafeArea()
		{
			Rect safeArea = Screen.safeArea;
			Vector2 anchorMin = safeArea.position;
			Vector2 anchorMax = safeArea.position + safeArea.size;

			anchorMin.x /= Screen.width;
			anchorMin.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;

			rectTransform.anchorMin = anchorMin;
			rectTransform.anchorMax = anchorMax;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}

		#endregion
	}
}