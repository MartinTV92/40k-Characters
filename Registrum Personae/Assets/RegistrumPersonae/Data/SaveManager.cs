using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RegistrumPersonae.Data;
using RegistrumPersonae;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace RegistrumPersonae.Data
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

		/// <summary>
		/// Initializes the SaveManager by loading all save files and printing them for debugging.
		/// </summary>
		public static void Init()
		{
			files = GetAllFiles();
			PrintSaveFiles();
		}

		/// <summary>
		/// Retrieves all save files with the JSON extension from the save path.
		/// </summary>
		/// <returns>A list of file paths for all save files.</returns>
		/// <summary>
		/// Retrieves all save files with the JSON extension from the save path.
		/// </summary>
		/// <returns>A list of file paths for all save files.</returns>
		private static List<string> GetAllFiles()
	    {
	        return Directory.GetFiles(SAVE_PATH, $"*{EXTENSION_JSON}").ToList();
	    }

		/// <summary>
		/// Deletes a save file by its file name.
		/// </summary>
		/// <param name="fileName">The name of the file to delete.</param>
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

		/// <summary>
		/// Deletes a save file by its index in the files list.
		/// </summary>
		/// <param name="fileIndex">The index of the file to delete in the files list.</param>
		/// <summary>
		/// Deletes a save file by its index in the files list.
		/// </summary>
		/// <param name="fileIndex">The index of the file to delete in the files list.</param>
		public static void Delete(int fileIndex)
		{
			if(files.Count <= fileIndex)
				return;

			File.Delete(files[fileIndex]);
			OnFileDeleted?.Invoke(fileIndex);
			files.RemoveAt(fileIndex);
		}

	    /// <summary>
	    /// Saves a character by converting it to CharacterData and saving it.
	    /// </summary>
	    /// <param name="character">The character to save.</param>
	    public static void SaveCharacter(Character character) => SaveCharacter(new CharacterData(character));

	    /// <summary>
	    /// Saves character data to a JSON file.
	    /// </summary>
	    /// <param name="data">The character data to save.</param>
	    public static void SaveCharacter(CharacterData data)
	    {
			CharacterToJSON(data);
	    }

	    /// <summary>
	    /// Loads character data from a JSON file.
	    /// </summary>
	    /// <param name="fileName">The path to the JSON file to load.</param>
	    /// <returns>The loaded character data.</returns>
	    public static CharacterData LoadCharacter(string fileName)
	    {
			return CharacterFromJson(fileName);
		}

		#region JSON

		/// <summary>
		/// Converts character data to JSON and writes it to a file.
		/// </summary>
		/// <param name="data">The character data to convert and save.</param>
		private static void CharacterToJSON(CharacterData data)
		{
			var fileName = FormatName(data.name);
			var path = $"{SAVE_PATH}/{fileName}";
			var json = JsonUtility.ToJson(data, true);
			File.WriteAllText(path, json);
		}

		/// <summary>
		/// Loads character data from a JSON file.
		/// </summary>
		/// <param name="file">The path to the JSON file.</param>
		/// <returns>The deserialized character data.</returns>
		private static CharacterData CharacterFromJson(string file)
		{
			string json = File.ReadAllText(file);
			return JsonUtility.FromJson<CharacterData>(json);
		}

		#endregion

		/// <summary>
		/// Formats the character's name to a standard for save files,
		/// i.e. no spaces or special characters and to lower case.
		/// E.g. "Jason Phile-69" -> "jasonphile69.json"
		/// </summary>
		/// <param name="characterName">The character's name to format.</param>
		/// <returns>The formatted string.</returns>
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

