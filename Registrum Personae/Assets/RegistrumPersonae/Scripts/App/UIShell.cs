using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

		// Asset References
		private StyleSheet styleSheet;
		private UIDocument uIDocument;
		private PanelSettings panelSettings;
		private VisualTreeAsset sourceAsset;

		// UI objects
		private VisualElement backgroundUI;
		private VisualElement safeArea;

		#endregion


		#endregion


		#region----- MONOBEHAVIOUR -----

		private void Awake()
		{
			HandleSignleton();
			FindReferences();
			CreateUI();
		}

		#endregion


		#region----- CUSTOM BEHAVIORUS -----

		public static void Init()
		{
			if (_instance != null)
				return;
			
			_instance = FindObjectOfType<UIShell>(true);
			if (_instance == null)
			{
				var prefab = Resources.Load<UIShell>("Prefabs/UIShell");
				if(prefab)	
					_instance = Instantiate(prefab);
				else
					Debug.LogError($"[UISHELL] - No prefab for UIShell at path 'Prefabs/UIShell'");
			}
		}

		void HandleSignleton()
		{
			if (_instance && _instance != this)
			{
				Destroy(this.gameObject);
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

		void CreateUI()
		{
			if(styleSheet == null)
				styleSheet = Resources.Load<StyleSheet>("MainAppStyleSheet");

			backgroundUI = new VisualElement();
			backgroundUI.styleSheets.Add(styleSheet);
			backgroundUI.AddToClassList("app_background");

			safeArea = new VisualElement();
			safeArea.AddToClassList("app_safe_area");
			
			backgroundUI.Add(safeArea);

			uIDocument.rootVisualElement.Add(backgroundUI);
			uIDocument.rootVisualElement.RegisterCallback<GeometryChangedEvent>(_ => FitToSafeArea());
		}

		void FitToSafeArea()
		{
			Rect safe = Screen.safeArea;
			safeArea.style.left = safe.xMin;
			safeArea.style.top = Screen.height - safe.yMax;
			safeArea.style.width = safe.width;
			safeArea.style.height = safe.height;
		}

		#endregion
	}
}