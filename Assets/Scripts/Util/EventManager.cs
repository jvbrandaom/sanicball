using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    #region Class and delegate declaration

    [System.Serializable]
    public class MetadataEvent : UnityEvent<object[]> { }

    #endregion

    private Dictionary<string, MetadataEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, MetadataEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener) {
        MetadataEvent thisEvent = null;
        UnityAction<object[]> parametrizedCall = delegate (object[] metadata) { listener(); };

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(parametrizedCall);
        }
        else {
            thisEvent = new MetadataEvent();
            thisEvent.AddListener(parametrizedCall);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(string eventName, UnityAction<object[]> listener)
    {
        MetadataEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new MetadataEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<object[]> listener)
    {
        if (eventManager == null) return;
        MetadataEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, params object[] metadata)
    {
        MetadataEvent thisEvent = null;
        if (!eventManager)
            return;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(metadata);
        }
    }
}