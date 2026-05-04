using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RegistrumPersonae
{

	public class UIShell : MonoBehaviour
	{
		#region----- VARIABLES -----

		#region Static

		private static UIShell _instance;
		public static UIShell Instance 
		{ 
			get
			{
				Init();
				return _instance;
			}

			set => _instance = value;
			
		}

		#endregion


		#region Instance

		public UIDocument uIDocument;
		public PanelSettings panelSettings;
		public VisualTreeAsset sourceAsset;

		#endregion

		#endregion


		#region----- MONOBEHAVIOUR -----

		private void Awake()
		{
			HandleSignleton();
			FindReferences();
		}

		#if UNITY_EDITOR

		private void OnValidate()
		{
			FindReferences();
		}

		#endif

		#endregion


		#region----- CUSTOM BEHAVIORUS -----

		public static void Init()
		{
			Debug.Log("Initializing UIShell");
			if (_instance == null)
			{
				Debug.Log("Finding UIShell in scene");
				_instance = FindObjectOfType<UIShell>();
				if(_instance == null)
				{
					Debug.Log("Instantiating UIShell from Resources");
					_instance = Instantiate(Resources.Load<UIShell>("Prefabs/UIShell"), Vector3.zero, Quaternion.identity);
					Debug.Log($"Instantiated UIShell as '{_instance.name}'");
				}
			}

			//Instance.HandleSignleton();
		}

		void HandleSignleton()
		{
			if (Instance && Instance != this)
			{
				Destroy(Instance.gameObject);
				return;	
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		void FindReferences()
		{
			if(TryGetComponent<UIDocument>(out uIDocument))
			{
				panelSettings = uIDocument.panelSettings;
				sourceAsset = uIDocument.visualTreeAsset;
				return;
			}

			uIDocument = gameObject.AddComponent<UIDocument>();
		}

		#endregion
	}
}