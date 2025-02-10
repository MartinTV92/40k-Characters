using UnityEngine;
using JollyRoger.FinateStateMachine;
using JollyRoger.DarkHeresy;
using System;
using Sirenix.OdinInspector;
using JollyRoger.Data;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
	#region----- NESTED -----

	/// <summary>
	/// The general state of the application.
	/// </summary>
	public enum State
	{
		None,
		Boot,
		Loading,
		MainMenu,
		CharacterCreation,
		CharacterSheet,
		LoadCharacter,
	}

	#endregion


	#region----- CONSTANT/STATIC -----

	public static GameManager Instance { get; private set; }
	public static Character Current => CharacterSheetView.target;

	#endregion


	#region----- VARIABLES -----

	public PopupManager popupManager;

	public JollyRoger.DarkHeresy.CharacterController characterController;
	public CharacterSheetPageView characterView;

	// State Machine
	public StateMachine<State>	stateMachine {get; private set;} = new();
	public GameStateBoot bootState = new();
	public GameStateLoading loadingState = new();
	public GameStateMenu menuState = new();
	public GameStateCharacterCreation characterCreationState = new();
	public GameStateCharacterSheet characterSheetState = new();
	public GameStateLoadCharacter loadCharacterState = new();

	#endregion


	#region----- MONOBEHAVIOURS -----
	
	void Awake()
    {
		if (Instance == null)
			Instance = this;

		SaveManager.Init();
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

	private void OnApplicationQuit()
	{
		//SaveManager.SaveCharacter(Current);
	}

	#endregion


	#region----- CUSTOM BEHAVIOURS -----

	/// <summary> 
	/// Registers all states to the state machine. 
	/// </summary>
	private void RegisterStates()
	{
		stateMachine.RegisterState(State.Boot, bootState);
		stateMachine.RegisterState(State.Loading, loadingState);
		stateMachine.RegisterState(State.MainMenu, menuState);
		stateMachine.RegisterState(State.CharacterCreation, characterCreationState);
		stateMachine.RegisterState(State.CharacterSheet, characterSheetState);
		stateMachine.RegisterState(State.LoadCharacter, loadCharacterState);
	}

	/// <summary> 
	/// Gets the currents tate of the application. 
	/// </summary>
	/// <returns> The state of the game (a state greater than 'None') or 'None' if no game manager is available. </returns>
	public static State GetState() => Instance ? Instance.stateMachine.currentState : State.None;

	/// <summary> 
	/// Adds a listener to the update of the state machine. 
	/// </summary>
	/// <param name="callBack">The Listener. </param>
	public static void AddStateUpdateListener(Action<State> callBack)
	{
		if (Instance == null)
			return;

		Instance.stateMachine.OnStateUpdate += callBack;
	}

	/// <summary> 
	/// Adds a listener to the ChangeState event in the state machine. 
	/// </summary>
	/// <param name="callBack">The listener </param>
	public static void AddStateChangeListener(Action<State, State> callBack)
	{
		if(Instance == null)
			return;

		Instance.stateMachine.OnStateChanged += callBack;
	}

	/// <summary> 
	/// Removes a listener to the update of the state machine. 
	/// </summary>
	/// <param name="callBack">The Listener. </param>
	public static void RemoveStateUpdateListener(Action<State> callBack)
	{
		if (Instance == null)
			return;

		Instance.stateMachine.OnStateUpdate -= callBack;
	}

	/// <summary> 
	/// Removes a listener to the ChangeState event in the state machine. 
	/// </summary>
	/// <param name="callBack">The listener </param>
	public static void RemoveStateChangeListener(Action<State, State> callBack)
	{
		if(Instance == null) 
			return;

		Instance.stateMachine.OnStateChanged -= callBack;
	}

	/// <summary>
	/// Helper method to allow UnityEvents (from UI) to go to charater creation state.
	/// </summary>
	public void GoToCharacterCreation() => stateMachine.ChangeState(State.CharacterCreation);

	/// <summary>
	/// Helper method to allow UnityEvents (from UI) to go to Menu state.
	/// </summary>
	public void GoToMenu() => stateMachine.ChangeState(State.MainMenu);

	/// <summary>
	/// Helper method to allow UnityEvents (from UI) to go to charater creation state and creates a new character.
	/// </summary>
	public void CreateCharacter()
	{
		var charName = MainMenuView.Instance.GetEnteredName();
		var charClass = MainMenuView.Instance.GetCareerSelection();

		if(Current != null)
			Current.OnCharacterChanged -= OnCharacterChanged;

		CharacterSheetView.target = new Character(charName, charClass);
		Current.OnCharacterChanged += OnCharacterChanged;
		stateMachine.ChangeState(State.CharacterSheet);
		SaveManager.SaveCharacter(Current);
	}

	public void GoToLoadScreen() => stateMachine.ChangeState(State.LoadCharacter);

	/// <summary>
	/// Loads a character from the given path.
	/// </summary>
	/// <param name="file">Complete filepath including extension. Not just the character's name </param>
	public void LoadCharacter(string file)
	{
		var data = SaveManager.LoadCharacter(file);
		if(data == null)
			return;

		stateMachine.ChangeState(State.CharacterSheet);

		if (Current != null)
			Current.OnCharacterChanged -= Save;

		CharacterSheetView.target = new Character(data);
		Current.OnCharacterChanged += Save;
	}

	/// <summary>
	/// Quits the application enforcing save (not implemented yet)
	/// </summary>
	public void Quit()
	{
		Application.Quit();
	}

	[Button("Save")]
	public void Save()
	{
		if (Application.isPlaying)
			SaveManager.SaveCharacter(Current);

		Debug.Log($"Saving Character: {Current.Name}");
	}

	void OnCharacterChanged() => Save();

	[Button("Delete")]
	public void Delete()
	{
		//SaveManager.Delete(0);
		SaveManager.Delete("cumblastahthemastah​");
	}



	#endregion
}