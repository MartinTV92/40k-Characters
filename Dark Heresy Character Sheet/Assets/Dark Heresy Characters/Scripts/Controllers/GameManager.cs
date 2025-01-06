using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SunJack.FinateStateMachine;
using SunJack.DarkHeresy;
using System;

public class GameManager : MonoBehaviour
{
	#region----- NESTED -----

	public enum State
	{
		None,
		Boot,
		Loading,
		MainMenu,
		CharacterCreation,
		CharacterSheet,
	}



	#endregion


	#region----- CONSTANT/STATIC -----

	public static GameManager Instance { get; private set; }

	#endregion


	#region----- VARIABLES -----

	public PopupManager popupManager;

	// State Machine
	public StateMachine<State>	stateMachine {get; private set;} = new();
	public GameStateBoot bootState = new();
	public GameStateLoading loadingState = new();
	public GameStateMenu menuState = new();
	public GameStateCharacterCreation characterCreationState = new();
	public GameStateCharacterSheet characterSheetState = new();

	#endregion


	#region----- MONOBEHAVIOURS -----

	
	void Awake()
    {
		if (Instance == null)
			Instance = this;

		popupManager.Setup();
		RegisterStates();
    }

	private void Start()
	{
		stateMachine.ChangeState(State.Boot);
	}

	void Update()
    {
        stateMachine.Run();
    }

	#endregion
	 

	#region----- CUSTOM BEHAVIOURS -----

	private void RegisterStates()
	{
		stateMachine.RegisterState(State.Boot, bootState);
		stateMachine.RegisterState(State.Loading, loadingState);
		stateMachine.RegisterState(State.MainMenu, menuState);
		stateMachine.RegisterState(State.CharacterCreation, characterCreationState);
		stateMachine.RegisterState(State.CharacterSheet, characterSheetState);
	}

	public static State GetState() => Instance ? Instance.stateMachine.currentState : State.None;

	public static void AddStateUpdateListener(Action<State> callBack)
	{
		if (Instance == null)
			return;

		Instance.stateMachine.OnStateUpdate += callBack;
	}

	public static void AddStateChangeListener(Action<State, State> callBack)
	{
		if(Instance == null)
			return;

		Instance.stateMachine.OnStateChanged += callBack;
	}

	public static void RemoveStateUpdateListener(Action<State> callBack)
	{
		if (Instance == null)
			return;

		Instance.stateMachine.OnStateUpdate -= callBack;
	}

	public static void RemoveStateChangeListener(Action<State, State> callBack)
	{
		if(Instance == null) 
			return;

		Instance.stateMachine.OnStateChanged -= callBack;
	}

	public void GoToCharacterCreation() => stateMachine.ChangeState(State.CharacterCreation);
	
	public void GoToMenu() => stateMachine.ChangeState(State.MainMenu);

	public void CreateCharacter()
	{
		var charName = MainMenuView.Instance.GetEnteredName();
		var charClass = MainMenuView.Instance.GetCareerSelection();
		CharacterSheetView.target = new Character(charName, charClass);
		stateMachine.ChangeState(State.CharacterSheet);
	}

	public void Quit()
	{
		Application.Quit();
	}

	#endregion
}