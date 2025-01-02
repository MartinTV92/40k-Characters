using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEventPropagator : MonoBehaviour, IPointerClickHandler
{
	public UnityEvent<PointerEventData> OnClick;

	// Called when the button is clicked
	public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke(eventData);

	// Subscribe to the event
	public void Subscribe(object handler)
	{
		SubscribeToEvent(OnClick, handler as IPointerClickHandler, (h, data) => h.OnPointerClick(data));
	}

	// Unsubscribe from the event
	public void Unsubscribe(object handler)
	{
		UnsubscribeFromEvent(OnClick, handler as IPointerClickHandler, (h, data) => h.OnPointerClick(data));
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
}