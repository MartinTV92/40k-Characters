using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RegistrumPersonae.UI.Utilities
{ 
    public abstract class EventPropagator : MonoBehaviour
    {
        public abstract void Subscribe(object handler);
        public abstract void Unsubscribe(object handler);
    }
}
