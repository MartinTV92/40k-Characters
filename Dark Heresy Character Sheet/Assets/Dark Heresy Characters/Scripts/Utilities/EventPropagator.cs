using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JollyRoger.UI.Utilities
{ 
    public abstract class EventPropagator : MonoBehaviour
    {
        public abstract void Subscribe(object handler);
        public abstract void Unsubscribe(object handler);
    }
}
