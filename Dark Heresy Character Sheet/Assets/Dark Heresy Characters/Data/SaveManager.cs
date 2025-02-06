using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JollyRoger.DarkHeresy.Data;
using JollyRoger.DarkHeresy;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace JollyRoger.Data
{ 
    /// <summary>
    /// Class for handling reading and writing to file in a variety of methods such JSON file or serialization.
    /// </summary>
    public static class SaveManager
    {
		#region----- CONSTANT/STATIC -----

		public static string SAVE_PATH => Application.persistentDataPath;
        public const string EXTENSION_CUSTOM = ".DH";
        public const string EXTENSION_JSON = ".json";

        public static ReadWriteType readWriteType;

		#endregion


		#region----- VARIABLES -----

        public static List<string> files { get; private set; }

		public static Action<int> OnFileDeleted;

		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		public static void Init()
		{
			files = GetAllFiles();
			PrintSaveFiles();
		}

		private static List<string> GetAllFiles()
        {
            return Directory.GetFiles(SAVE_PATH, $"*{EXTENSION_JSON}").ToList();
        }

		public static void Delete(string fileName)
		{
			for(int i = 0; i < files.Count; i++)
			{
				if (files[i] == fileName)
				{
					Delete(i);
					break;
				}
			}
		}

		public static void Delete(int fileIndex)
		{
			if(files.Count <= fileIndex)
				return;

			File.Delete(files[fileIndex]);
			OnFileDeleted?.Invoke(fileIndex);
			files.RemoveAt(fileIndex);
		}

        public static void SaveCharacter(Character character) => SaveCharacter(new CharacterData(character));

        public static void SaveCharacter(CharacterData data)
        {
            if(readWriteType == ReadWriteType.BasicSerialization)
                SerializeCharacter(data);
			else if(readWriteType == ReadWriteType.Json)
				CharacterToJSON(data);
        }

        public static CharacterData LoadCharacter(string fileName)
        {
			if (readWriteType == ReadWriteType.BasicSerialization)
				return DeserializeCharacter(fileName);
			else if(readWriteType == ReadWriteType.Json)
				return CharacterFromJson(fileName);

			return null;
		}

		#region Basic Serialization

		private static void SerializeCharacter(CharacterData data)
        {
			var binForm = new BinaryFormatter();
			var fileName = FormatName(data.name);
			var path = $"{SAVE_PATH}/{fileName}";
			var stream = new FileStream(path, FileMode.Create);
			binForm.Serialize(stream, data);
			stream.Close();
		}

        private static CharacterData DeserializeCharacter(string fileName)
        {
			//var path = $"{SAVE_PATH}/{fileName}{EXTENSION_CUSTOM}";

			if (File.Exists(fileName))
			{
				var binForm = new BinaryFormatter();
				var stream = new FileStream(fileName, FileMode.Create);
				var data = binForm.Deserialize(stream) as CharacterData;
				stream.Close();
				return data;
			}
			else
			{
				Debug.LogError($"No file found for '{fileName}'");
				return null;
			}
		}

		#endregion

		#region JSON

		private static void CharacterToJSON(CharacterData data)
		{
			var fileName = FormatName(data.name);
			var path = $"{SAVE_PATH}/{fileName}";
			var json = JsonUtility.ToJson(data, true);
			File.WriteAllText(path, json);
		}

		private static CharacterData CharacterFromJson(string file)
		{
			string json = File.ReadAllText(file);
			return JsonUtility.FromJson<CharacterData>(json);
		}

		#endregion

		public static string FormatName(string characterName)
		{
			var fileName = Regex.Replace(characterName, @"\s+", "").ToLower();
			fileName += readWriteType == ReadWriteType.Json ? EXTENSION_JSON : EXTENSION_CUSTOM;
			return fileName;
		}

		private static void PrintSaveFiles()
		{
			foreach (var file in files)
				Debug.Log(file);
		}

		#endregion
	}
}