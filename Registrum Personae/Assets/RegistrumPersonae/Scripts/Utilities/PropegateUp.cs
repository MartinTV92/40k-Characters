using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RegistrumPersonae.Attributes;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TS.PageSlider;

namespace RegistrumPersonae.UI.Utilities
{
	/// <summary>
	/// Searches up the parent hierachy for an appropriate UI component to propegate events to.
	/// For now it will only search for ScrollRect as that is the use case required now, but eventually
	/// it will be updated to search for any UIBehaviour derived class.
	/// </summary>
	[System.Serializable, InstanceType(typeof(PropegateUp))]
	public class PropegateUp : PropegateMethod
	{
		public override List<object> Propagate(EventPropagator propagator)
		{
			if(propagator.transform.parent == null)
				return null;

			var targets = new List<object>();
			var scrollRect = propagator.transform.transform.parent.GetComponentInParent<ScrollRect>();
			var pageScroller = propagator.transform.transform.parent.GetComponentInParent<PageScroller>();
			
			if(scrollRect)
				targets.Add(scrollRect);
			if(pageScroller)
				targets.Add(pageScroller);

			return targets;
		}
	}
}

