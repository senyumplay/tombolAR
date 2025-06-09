using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEventSO gameEvent;
    public UnityEvent response;

    private void OnEnable() => gameEvent?.Register(OnEventRaised);

    private void OnDisable() => gameEvent?.Unregister(OnEventRaised);

    private void OnEventRaised() => response?.Invoke();
}
