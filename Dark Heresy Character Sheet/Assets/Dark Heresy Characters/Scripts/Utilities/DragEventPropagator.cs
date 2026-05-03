using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using RegistrumPersonae.Attributes;
using System.Collections.Generic;
using System;
using System.Collections;

namespace RegistrumPersonae.UI.Utilities
{ 

	public class DragEventPropagator : EventPropagator, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, IInitializePotentialDragHandler
	{

		#region----- VARIABLES -----

		[SubclassSelector, SerializeReference]
		public PropegateMethod propegateMethod;
		[ShowInInspector]
		public List<object> targets = new();

		[FoldoutGroup("Events")] public UnityEvent<PointerEventData> BeginDrag;
		[FoldoutGroup("Events")] public UnityEvent<PointerEventData> Drag;
		[FoldoutGroup("Events")] public UnityEvent<PointerEventData> EndDrag;
		[FoldoutGroup("Events")] public UnityEvent<PointerEventData> Scroll;
		[FoldoutGroup("Events")] public UnityEvent<PointerEventData> Initialize;

		#endregion


		#region----- MONOBEHAVIOURS -----

		private void OnEnable()
		{
			targets = propegateMethod.Propagate(this);
			foreach (var target in targets)
				Subscribe(target);
		}

		private void OnDisable()
		{
			foreach (var target in targets)
				Unsubscribe(target);
		}

		#endregion


		#region----- CUSTOM BEHAVIORUS -----

		public void OnBeginDrag(PointerEventData eventData) => BeginDrag?.Invoke(eventData);
		public void OnDrag(PointerEventData eventData) => Drag?.Invoke(eventData);
		public void OnEndDrag(PointerEventData eventData) => EndDrag?.Invoke(eventData);
		public void OnScroll(PointerEventData eventData) => Scroll?.Invoke(eventData);
		public void OnInitializePotentialDrag(PointerEventData eventData) => Initialize?.Invoke(eventData);

		public override void Subscribe(object handler)
		{
			SubscribeToEvent(BeginDrag, handler as IBeginDragHandler, (h, data) => h.OnBeginDrag(data));
			SubscribeToEvent(Drag, handler as IDragHandler, (h, data) => h.OnDrag(data));
			SubscribeToEvent(EndDrag, handler as IEndDragHandler, (h, data) => h.OnEndDrag(data));
			SubscribeToEvent(Scroll, handler as IScrollHandler, (h, data) => h.OnScroll(data));
			SubscribeToEvent(Initialize, handler as IInitializePotentialDragHandler, (h, data) => h.OnInitializePotentialDrag(data));
		}

		public override void Unsubscribe(object handler)
		{
			UnsubscribeFromEvent(BeginDrag, handler as IBeginDragHandler, (h, data) => h.OnBeginDrag(data));
			UnsubscribeFromEvent(Drag, handler as IDragHandler, (h, data) => h.OnDrag(data));
			UnsubscribeFromEvent(EndDrag, handler as IEndDragHandler, (h, data) => h.OnEndDrag(data));
			UnsubscribeFromEvent(Scroll, handler as IScrollHandler, (h, data) => h.OnScroll(data));
			UnsubscribeFromEvent(Initialize, handler as IInitializePotentialDragHandler, (h, data) => h.OnInitializePotentialDrag(data));
		}

		private void SubscribeToEvent<T>(UnityEvent<PointerEventData> unityEvent, T handler, System.Action<T, PointerEventData> action) where T : class
		{
			if (handler != null)
				unityEvent.AddListener(eventData => action(handler, eventData));
		}

		private void UnsubscribeFromEvent<T>(UnityEvent<PointerEventData> unityEvent, T handler, System.Action<T, PointerEventData> action) where T : class
		{
			if (handler != null)
				unityEvent.RemoveListener(eventData => action(handler, eventData));
		}

		#endregion
	}
}