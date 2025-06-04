using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using JollyRoger.Utilities;
using System.Threading.Tasks;
using UnityEditor.DeviceSimulation;
using System.Linq;


namespace JollyRoger.Editor.Mobile.Testing
{
    /// <summary>
    /// Helper class to test all devices in simulator mode.
    /// Iterates over each device and flags those that have UI that exceeds the bounds
    /// of the device's screen.
    /// </summary>
    public static class SimulatorDeviceIterator
    {
        private const string SIMULATOR_PACKAGE = "com.unity.device-simulator.devices";
        private const string SIMULATOR_WINDOW_TYPE = "UnityEditor.DeviceSimulation.SimulatorWindow, UnityEditor.DeviceSimulatorModule";
        private const int ITERATE_DELAY_MILISECONDS = 100;
        private const BindingFlags PRIVATE_INSTANCE = BindingFlags.NonPublic | BindingFlags.Instance;
        private const BindingFlags PUBLIC_INSTANCE = BindingFlags.Public | BindingFlags.Instance;

		[MenuItem("Jolly Roger/Iterate Over Simulator Devices")]
        private async static void TestSimulatorDevices()
        {
            try
            {
                bool hasPackage = await PackageUtilities.HasPackage(SIMULATOR_PACKAGE);
				if (!hasPackage)
				    return;

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
				int originalIndex = (int)deviceIndexProp.GetValue(mainInstance);
                var setDevice = simulatorType.GetMethod("SetDevice", PRIVATE_INSTANCE);

				for (int i = 0; i < devices.Length; i++)
				{
					deviceIndexProp.SetValue(mainInstance, i);
					await Task.Delay(ITERATE_DELAY_MILISECONDS);
				}

				deviceIndexProp.SetValue(mainInstance, originalIndex);
			}
			catch (Exception e)
            {
                Debug.LogException(e);
            }
		}
	}
}