using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/GameEvent (Void)", fileName = "GameEvent")]
public class GameEventSO : ScriptableObject
{
    private event Action listeners;

    public void Raise() => listeners?.Invoke();

    public void Register(Action listener) => listeners += listener;

    public void Unregister(Action listener) => listeners -= listener;
}
