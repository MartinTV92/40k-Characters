using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunJack.DarkHeresy
{ 

    [System.Serializable]
    public abstract class Prerequisite
    {
        public abstract string Name { get; }
        public abstract bool IsMet(Character character);
    }
}