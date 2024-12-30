using SunJack.FinateStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// The main menu state for the program. Shows load and create options for character sheets
/// and options for the app (which may not do much at this point).
/// </summary>
[System.Serializable, HideLabel, InlineProperty, FoldoutGroup("Menu State")]
public class GameStateMenu : State<GameManager.State>
{
    
}
