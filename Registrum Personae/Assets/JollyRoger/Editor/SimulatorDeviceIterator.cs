using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using JollyRoger.Utilities;
using System.Threading.Tasks;


namespace JollyRoger.Editor.Mobile.Testing
{
	/// <summary>
	/// Helper class to test all devices in simulator mode.
	/// Iterates over each device and flags those that have UI that exceeds the bounds
	/// of the device's screen.
	/// </summary>
	public static class SimulatorDeviceIterator
	{
		#region----- NESTED -----

		private struct DeviceWindowReflectionProperties
		{
			public PropertyInfo deviceIndex;
			public Array devices;
			public object window;

			public DeviceWindowReflectionProperties(PropertyInfo deviceIndex, Array devices, object window)
			{
				this.deviceIndex = deviceIndex;
				this.devices = devices;
				this.window = window;
			}
		}

		private struct SimulatedDeviceProperties
		{
			public int originalIndex;
			public Type type;
			public PropertyInfo currentDeviceProperty;
			public FieldInfo deviceFieldInfo;
			public FieldInfo friendlyNameFieldInfo;

			public object currentDevice;
			public object deviceInfo;
			public string friendlyName;

			public void UpdateInformationForDevice(int deviceIndex, DeviceWindowReflectionProperties window)
			{
				currentDevice = currentDeviceProperty.GetValue(window.window);
				deviceInfo = deviceFieldInfo.GetValue(currentDevice);
				friendlyName = (string)friendlyNameFieldInfo.GetValue(deviceInfo);
			}
		}

		#endregion

		#region ----- VARIABLES -----

		private const string SIMULATOR_PACKAGE = "com.unity.device-simulator.devices";
	    private const string SIMULATOR_WINDOW_TYPE = "UnityEditor.DeviceSimulation.SimulatorWindow, UnityEditor.DeviceSimulatorModule";
	    private const int ITERATE_DELAY_MILISECONDS = 50;
	    private const BindingFlags PRIVATE_INSTANCE = BindingFlags.NonPublic | BindingFlags.Instance;
	    private const BindingFlags PUBLIC_INSTANCE = BindingFlags.Public | BindingFlags.Instance;

		#endregion



		[MenuItem("Jolly Roger/Iterate Over Simulator Devices")]
		private async static void TestSimulatorDevices() => await TestSimulatorDevices(null, null);

	    public async static Task TestSimulatorDevices(Action<string> onDeviceChangedCallback, Action onEnd)
	    {
	        try
	        {				
				EditorUtility.DisplayProgressBar("Device Iterator", "Checking if Device Simulator package is installed", 0);
	            bool hasPackage = await PackageUtilities.HasPackage(SIMULATOR_PACKAGE);
				if(!hasPackage)
					throw new Exception("Device Simulator not installed");

				var reflectionInfo = GetDeviceWindowProperties();
				await IterateThroughDevices(reflectionInfo, onDeviceChangedCallback);
				onEnd?.Invoke();
			}
			catch (Exception e)
	        {
				Debug.Log($"[Device Iterator] FAILED: Exception");
	            Debug.LogException(e);
	        }
			finally
			{
				EditorUtility.ClearProgressBar();
			}
		}

		/// <summary>
		/// Gets all the relevant information via reflection for performing the iteration.
		/// </summary>
		/// <returns>A thruple containing the information instead (PropertyInfo, Array, object) </returns>
		private static DeviceWindowReflectionProperties GetDeviceWindowProperties()
		{
			var simulatorType = Type.GetType(SIMULATOR_WINDOW_TYPE) ?? AppDomain.CurrentDomain.GetAssemblies()
										.Select(a => a.GetType("UnityEditor.DeviceSimulation.SimulatorWindow"))
										.FirstOrDefault(t => t != null);

			var simulatorWindow = EditorWindow.GetWindow(simulatorType);
			simulatorWindow.Show();
			var mainField = simulatorType.GetField("m_Main", BindingFlags.NonPublic | BindingFlags.Instance);
			var mainInstance = mainField.GetValue(simulatorWindow);
			var mainType = mainInstance.GetType();
			var devicesProperty = mainType.GetProperty("devices", BindingFlags.Public | BindingFlags.Instance);
			var devices = devicesProperty.GetValue(mainInstance) as Array;
			var deviceIndexProp = mainType.GetProperty("deviceIndex", BindingFlags.Public | BindingFlags.Instance);

			return new(deviceIndexProp, devices, mainInstance);
		}

		/// <summary>
		/// Iterates through all the devices and calls a test on each.
		/// Resets the device back to whatever was active before running test.
		/// </summary>
		/// <param name="windowProperties"></param>
		private static async Task IterateThroughDevices(DeviceWindowReflectionProperties windowProperties, Action<string> onDeviceChanged = null)
		{
			var deviceProperties = GetSimulatedDeviceProperties(windowProperties);

			for (int i = 0; i < windowProperties.devices.Length; i++)
			{
				deviceProperties.UpdateInformationForDevice(i, windowProperties);
				
				EditorUtility.DisplayProgressBar("Device Iterator", $"Iteration {i}/{windowProperties.devices.Length} - " +
												$"{deviceProperties.friendlyName}", (float) i / (float) windowProperties.devices.Length);

				windowProperties.deviceIndex.SetValue(windowProperties.window, i);
				await Task.Delay(ITERATE_DELAY_MILISECONDS);
				onDeviceChanged?.Invoke(deviceProperties.friendlyName);
				await Task.Delay(ITERATE_DELAY_MILISECONDS);
			}

			windowProperties.deviceIndex.SetValue(windowProperties.window, deviceProperties.originalIndex);
			EditorUtility.DisplayProgressBar("Device Iterator", "Resetting to starting value", 1);
			await Task.Delay(ITERATE_DELAY_MILISECONDS);
		}

		private static SimulatedDeviceProperties GetSimulatedDeviceProperties(DeviceWindowReflectionProperties reflectionInfo)
		{
			var result = new SimulatedDeviceProperties();
			result.originalIndex = (int)reflectionInfo.deviceIndex.GetValue(reflectionInfo.window);
			result.type = reflectionInfo.window.GetType();
			result.currentDeviceProperty = result.type.GetProperty("currentDevice", BindingFlags.Public | BindingFlags.Instance);
			result.deviceFieldInfo = result.currentDeviceProperty.PropertyType.GetField("deviceInfo", BindingFlags.Public | BindingFlags.Instance);
			result.friendlyNameFieldInfo = result.deviceFieldInfo.FieldType.GetField("friendlyName", BindingFlags.Public | BindingFlags.Instance);

			return result;
		}
	}
}

