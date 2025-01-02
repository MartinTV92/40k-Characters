using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SunJack.Attributes;
using System;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace SunJack.UI.Utilities
{
    [System.Serializable]
    public abstract class PropegateMethod
    {

		/// <summary>
		/// Finds the appropriate objects to propagate events to.
		/// </summary>
		/// <param name="propagator">The object that needs to propagate i.e the one with the EventPropagator script on it. </param>
		/// <returns>A list of UIbehaviours that will need to have events propagated to.</returns>
		public abstract List<object> Propagate(EventPropagator propagator);

    }
}