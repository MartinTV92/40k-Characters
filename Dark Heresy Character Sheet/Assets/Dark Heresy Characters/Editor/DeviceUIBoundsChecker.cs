using UnityEngine;
using UnityEditor;
using RegistrumPersonae.UI.Utilities;
using RegistrumPersonae.Editor.Mobile.Testing;

namespace RegistrumPersonae.EditorTools
{
    public static class DeviceUIBoundsChecker
    {
		#region ----- VARIABLES -----

		private const int DIMENSION_MARGIN = 1;

		private static SafeAreaResizer _resizer;
        private static SafeAreaResizer Resizer
        {
            get
			{
                if(_resizer == null)
					_resizer = GameObject.FindObjectOfType<SafeAreaResizer>();
                
                return _resizer;
			}
        }

		private static Canvas _activeCanvas;
		private static Canvas ActiveCanvas
		{
			get
			{
				if(_activeCanvas == null)
				{
					_activeCanvas = GameObject.FindObjectOfType<Canvas>();
					 if(_activeCanvas != null )
						rectTransform = _activeCanvas.GetComponent<RectTransform>();
				}
				
				return _activeCanvas;
			}
		}

		private static RectTransform rectTransform;


		#endregion

		[MenuItem("Jolly Roger/Test Device UI Bounds")]
		private static void RunUIBoundsCheck()
        {
			if(!Resizer || !ActiveCanvas)
			{
				Debug.LogError($" 'SaveAreaResizer' = {(Resizer ? "Active":"NULL")} | Canvas = {(ActiveCanvas ? "Active" : "NULL")}");
				return;
			}

			_ = SimulatorDeviceIterator.TestSimulatorDevices(TestUI, ResizeScreen);
        }

		
		private static void TestUI(string deviceName)
        {
			Resizer.FitToSafeArea();
			CanvasContainsAllUI(deviceName, rectTransform);
        }


		[MenuItem("Jolly Roger/Trigger Safe Area Resizer")]
		public static void ResizeScreen()
		{
			if (Resizer)
			    Resizer.FitToSafeArea();
            else
                Debug.LogError("No 'SaveAreaResizer' in scene");
		}


		private static void CanvasContainsAllUI(string deviceName, RectTransform root)
		{
			var totalBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(root);
			var result = totalBounds.size.x <= root.rect.size.x + DIMENSION_MARGIN && totalBounds.size.y <= root.rect.size.y + DIMENSION_MARGIN;
			var report = $"{(result ? "SUCCESS" : "FAILED")}";
			var msg = $"{deviceName} UI [X:{totalBounds.size.x}, Y: {totalBounds.size.y}] / Canvas [X:{root.rect.size.x}, Y: {root.rect.size.y}] = {report}";
			if(result)
				Debug.Log(msg);
			else
				Debug.LogError(msg);
		}
	}
}
