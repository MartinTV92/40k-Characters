using Sirenix.OdinInspector;
using JollyRoger.DesignPatterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RegistrumPersonae
{
    [System.Serializable, HideLabel, InlineProperty, FoldoutGroup("Character Sheet State")]
    public class GameStateCharacterSheet : State<GameManager.State>
    {
        
    }
}