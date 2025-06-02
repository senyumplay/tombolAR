using System;
using UnityEngine;

public abstract class ToggleButton<T> : MonoBehaviour
{
    public static Action<T> OnSetModeActive;

    public abstract T CurrentMode { get; protected set; }

    protected virtual void Start()
    {
        // Bisa diisi default value di child
    }

    public virtual void ToggleState()
    {
        CurrentMode = GetNextMode(CurrentMode);
        Debug.Log("ParentToggle - Current Mode: " + CurrentMode);
        OnSetModeActive?.Invoke(CurrentMode);
    }

    // Child wajib implementasikan logika mode switching
    protected abstract T GetNextMode(T currentMode);
}
