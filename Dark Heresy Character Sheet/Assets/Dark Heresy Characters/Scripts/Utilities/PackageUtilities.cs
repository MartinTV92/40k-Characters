using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;

namespace JollyRoger.Utilities
{
    
    public static class PackageUtilities
    {
		/// <summary>
		/// Checks if the given package is in the project.
		/// </summary>
		/// <param name="packageName">The name of the package to look in a format like "com.unity.device-simulator.devices" </param>
		/// <returns>Task<bool> with the bool as true or false depending on if the package is installed or not. </returns>
		public static Task<bool> HasPackage(string packageName)
        {
			Debug.Log("Start");
			var request = Client.List();
			var tcs = new TaskCompletionSource<bool>();
			EditorApplication.CallbackFunction callback = null;
			callback = () =>
			{
				Debug.Log($"Request.IsCompleted = {request.IsCompleted}");
				if (!request.IsCompleted)
					return;

				Debug.Log($"Status = {request.Status}");
				EditorApplication.update -= callback;
				if (request.Status == StatusCode.Success)
				{
					bool result = false;
					foreach (var package in request.Result)
					{
						Debug.Log($"Has package '{package.displayName}'");
						if (package.name == packageName)
						{
							Debug.Log($"MATCH");
							result = true;
							tcs.SetResult(true);
							return;
						}
					}

					Debug.Log($"[PackageUtilities] Package '{packageName}' installed = {result}");
					tcs.SetResult(result);
				}
				else
				{
					Debug.LogError($"[PackageUtilities] Error checking package '{packageName}': {request.Error.message}");
					tcs.SetException(new System.Exception(request.Error.message));
				}
			};

			EditorApplication.update += callback;
			Debug.Log("Done?");
			return tcs.Task;
		}
    }
}