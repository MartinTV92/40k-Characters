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
        public const string EXTENSION_JSON = ".json";

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
			CharacterToJSON(data);
        }

        public static CharacterData LoadCharacter(string fileName)
        {
			return CharacterFromJson(fileName);
		}

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

		/// <summary>
		/// Format the character's name to a standard for save files, 
		/// i.e. no spaces or special characters and to lower case.
		/// E.g. "Jason Phile-69" -> "jasonphile69.json"
		/// </summary>
		/// <param name="characterName"> The character's name to format.</param>
		/// <returns>The formated string.</returns>
		public static string FormatName(string characterName)
		{
			var fileName = Regex.Replace(characterName, @"[^a-zA-Z0-9]", "").ToLower() + EXTENSION_JSON;
			fileName.Normalize();
			return fileName;
		}

		/// <summary>
		/// Prints all save files. For debug purposes.
		/// </summary>
		private static void PrintSaveFiles()
		{
			foreach (var file in files)
				Debug.Log(file);
		}

		#endregion
	}
}