using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RegistrumPersonae
{ 

    [System.Serializable]
    public abstract class Prerequisite
    {
        public abstract string Name { get; }
        public abstract bool IsMet(Character character);
    }
}