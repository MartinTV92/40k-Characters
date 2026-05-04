using System.Collections.Generic;
using UnityEngine;
using JollyRoger.Utilities;
using RegistrumPersonae.Data;

namespace RegistrumPersonae
{

	public class SaveFileUIController : MonoBehaviour
	{
		#region----- VARIABLES -----

		public SaveFileUI prefab;
		public RectTransform uiContainer;
		public List<SaveFileUI> saveFileUIs;
		private ObjectPoolHelper<SaveFileUI> uiPool;

		#endregion


		#region----- MONOBEHAVIOURS -----

		private void Awake()
		{
			uiPool = new(prefab);
			SaveManager.OnFileDeleted += OnFileDeleted;
		}

		private void OnDestroy()
		{
			SaveManager.OnFileDeleted -= OnFileDeleted;
		}

		#endregion


		#region----- CUSTOM BEHAVIOUR -----

		public void Open()
		{
			gameObject.SetActive(true);
			foreach (var file in SaveManager.files)
			{
				var ui = uiPool.Get();
				ui.fileName = file;
				saveFileUIs.Add(ui);
				ui.transform.SetParent(uiContainer);
				ui.transform.localScale = Vector3.one;
			}
		}

		void OnFileDeleted(int index)
		{
			uiPool.Release(saveFileUIs[index]);
		}

		public void Close()
		{
			foreach(var ui in saveFileUIs)
				uiPool.Release(ui);

			saveFileUIs.Clear();
			gameObject.SetActive(false);
		}


		#endregion
	}
}

