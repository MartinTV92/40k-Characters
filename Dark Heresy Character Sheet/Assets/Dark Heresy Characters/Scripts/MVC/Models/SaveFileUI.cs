using System;
using UnityEngine;
using RegistrumPersonae.Data;


namespace RegistrumPersonae
{ 
    public class SaveFileUI : MonoBehaviour
    {

		#region----- VARIABLES -----

        public Action OnFileNameChange;

        private string _fileName;
		public string fileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnFileNameChange?.Invoke();
            }
        }

		#endregion

		#region----- CUSTOM BEHAVIOUR -----

		public void SetSaveFileInfo(string fileName)
        {
            this.fileName = fileName;
        }

        public void Load()
        {
            GameManager.Instance.LoadCharacter(fileName);
        }

        public void Delete()
        {
            SaveManager.Delete(fileName);
        }

		#endregion
	}
}