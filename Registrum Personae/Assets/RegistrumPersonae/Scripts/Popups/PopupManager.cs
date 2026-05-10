using System;
using UnityEngine;
using Sirenix.OdinInspector;
using JollyRoger.Collections;

namespace RegistrumPersonae
{ 
	/// <summary>
	/// Manages all the popups in the application and is the entry point for 
	/// accessing any given popup at runtime.
	/// </summary>
	public class PopupManager : MonoBehaviour
	{
	    #region----- NESTED -----

	    [System.Serializable]
	    public class PopupDictionary : UnitySerializedDictionary<Type, IPopup> { }

	    #endregion


	    #region----- VARIABLES -----

	    // Static/Constants
	    private static PopupManager Instance { get; set; }
		[ShowInInspector]
		private static PopupDictionary popups = new();
	    [ShowInInspector]
	    private static IPopup currentPopup;

		#endregion


		#region----- MONOBEHAVIOURS -----

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

	    public static void Open<T>(T target)
	    {
	        if(Instance == null)
	            return;

	        if(popups.TryGetValue(typeof(T), out var popup))
	        {
	            Close();
	            currentPopup = popup;
				currentPopup.Open(target);
	            Instance.gameObject.SetActive(true);
	        }
	    }

	    public static void Close()
	    {
			Instance.gameObject.SetActive(false);
			if (currentPopup != null)
				currentPopup.Close();
		}

	    public static void CloseAll()
	    {
			Instance.gameObject.SetActive(false);
			foreach (var popup in popups.Values)
	            popup.Close();
	    }

	    public static void Register(IPopup popup)
	    {
	        var key = popup.GetPopupType();
			if (popups.ContainsKey(key) == false)
				popups.Add(key, popup);
	    }

	    public static void Deregister(IPopup popup)
	    {
			var key = popup.GetPopupType();
			if (popups.ContainsKey(key) == true)
				popups.Remove(key);
		}

	    public void Setup()
	    {
			Instance = this;
			var windows = GetComponentsInChildren<IPopup>(true);
	        foreach(var window in windows)
	        { 
	            Register(window);
	            window.Close();
	        }
		}

		#endregion

	}
}

