using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System;

namespace SunJack.DarkHeresy
{ 

    public class MainMenuView : MonoBehaviour
    {
		#region----- NESTED -----

		[Serializable] public class StateUIDict : UnitySerializedDictionary<GameManager.State, GameObject> { }

		#endregion


		#region----- VARIABLES -----

		public static MainMenuView Instance { get; private set; }
		public StateUIDict stateUI;

		[FoldoutGroup("MISC UI")] public GameObject inquisitorLogo;

		[FoldoutGroup("Loading UI")] public GameObject loadingUI;
		[FoldoutGroup("Loading UI")] public TextMeshProUGUI loadingSubtitleUI;
		[FoldoutGroup("Loading UI")] public float subtitleDelay = 1;
		private float nextStep;
		private int _curSubtitle = 0;
		private int curSubtitle
		{
			get => _curSubtitle;
			set => _curSubtitle = (int)Mathf.Repeat(value, loadingSubtitlesLoop.Length);
		}
		private string[] loadingSubtitlesLoop = new string[]
		{
			"",
			".",
			"..",
			"..."
		};

		[FoldoutGroup("Menu UI")] public GameObject menuUI;

		[FoldoutGroup("Create Character UI")] public GameObject createCharacterUI;
		[FoldoutGroup("Create Character UI")] public TextMeshProUGUI nameText;
		[FoldoutGroup("Create Character UI")] public TMP_Dropdown careerSelect;

		[FoldoutGroup("Character Sheet UI")] public GameObject characterSheetUI;

		[FoldoutGroup("Edit Windows")] public GameObject editWindows;

		#endregion



		#region----- MONOBEHAVIOURS -----

		private void Awake()
		{
			Instance = this;

			stateUI = new StateUIDict()
			{
				{GameManager.State.Loading, loadingUI},
				{GameManager.State.MainMenu, menuUI},
				{GameManager.State.CharacterCreation, createCharacterUI},
				{GameManager.State.CharacterSheet, characterSheetUI},
			};

			SetupCareerSelectUI();
			HideAll();
		}


		#endregion


		#region----- CUSTOM BEHAVIOURS -----

		public void HideAll()
		{
			editWindows.SetActive(false);
			foreach (var ui in stateUI.Values)
				ui.SetActive(false);
		}

		public void ShowUI(GameManager.State state, bool show)
		{
			if(stateUI.ContainsKey(state))
				stateUI[state].SetActive(show);
		}

		public void UpdateLoadingDialogue()
		{
			if (Time.time < nextStep)
				return;

			nextStep = Time.time + subtitleDelay;
			curSubtitle++;
			loadingSubtitleUI.text = loadingSubtitlesLoop[curSubtitle];
		}

		public void SetupCareerSelectUI()
		{
			careerSelect.options.Clear();
			foreach (var val in Enum.GetValues(typeof(Character.Career)))
				careerSelect.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
		}

		public string GetEnteredName() => nameText.text;
		public Character.Career GetCareerSelection() => (Character.Career)careerSelect.value;

		#endregion

	}
}