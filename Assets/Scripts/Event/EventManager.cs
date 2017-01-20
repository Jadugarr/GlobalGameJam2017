using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EventTypes
{
	ObjectHit, 
	DebugObjectHit,
	CheckHit,
	MonsterReachedDestination,
	MonsterKilled,
	KillMonster,
	MonsterMissed
}

/// <summary>
/// Singleton event manager.
/// </summary>
public class EventManager
{
	#region Delegates/Events

	public delegate void EventHandler(IEvent evt);

	#endregion

	#region Öffentliche Felder

	private static EventManager instance = new EventManager();
	private Dictionary<EventTypes, List<EventHandler>> callbacks = new Dictionary<EventTypes, List<EventHandler>>();

	#endregion

	#region Eigenschaften

	/// <summary>
	/// Gets the instance of the EventManager.
	/// </summary>
	/// <value>The instance.</value>
	public static EventManager Instance
	{
		get
		{
			return EventManager.instance;
		}
	}

	#endregion

	#region Private Konstruktoren
	
	/// <summary>
	/// Initializes a new instance of the <see cref="EventManager"/> class.
	/// </summary>
	private EventManager(){}

	#endregion

	#region Öffentliche Methoden

	/// <summary>
	/// Registers and EventHandler to a specific event.
	/// </summary>
	/// <param name="eventType">Event type.</param>
	/// <param name="handler">Handler.</param>
	public void RegisterForEvent(EventTypes eventType, EventHandler method)
	{
		List<EventHandler> eventCallbacks = null;

		if (this.callbacks.TryGetValue(eventType, out eventCallbacks) == true) 
		{
			eventCallbacks.Add(method);
		}
		else 
		{
			eventCallbacks = new List<EventHandler>();
			eventCallbacks.Add(method);
			this.callbacks.Add(eventType, eventCallbacks);
		}
	}

	/// <summary>
	/// Removes the given method from the specified event. If there are no methods for the specified event, this method does nothing.
	/// </summary>
	/// <param name="EventType">Event type.</param>
	/// <param name="method">Method.</param>
	public void RemoveFromEvent(EventTypes eventType, EventHandler method)
	{
		List<EventHandler> eventCallbacks = null;

		if (this.callbacks.TryGetValue(eventType, out eventCallbacks) == true) 
		{
			eventCallbacks.Remove(method);
		}
		else 
		{
			return;
		}
	}

	/// <summary>
	/// Fires the given event. If there are no registered methods for this event, this method will do nothing.
	/// </summary>
	/// <param name="eventType">Event type.</param>
	/// <param name="evt">Evt.</param>
	public void FireEvent(EventTypes eventType, IEvent evt)
	{
		List<EventHandler> eventCallbacks = null;

		if (this.callbacks.TryGetValue(eventType, out eventCallbacks) == true) 
		{
			for (int i = 0; i < eventCallbacks.Count; i++) 
			{
				EventHandler method = eventCallbacks[i];
				method(evt);
			}
		}
		else 
		{
			return;
		}
	}

	#endregion
}
