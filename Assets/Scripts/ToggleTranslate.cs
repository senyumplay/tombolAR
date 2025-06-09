using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine;

public class ToggleTranslate : ToggleButton<ToggleTranslate.Mode>
{
    public enum Mode
    {
        ID,
        EN
    }

    [SerializeField]
    private Mode modeActive = Mode.ID;

    public override Mode CurrentMode { get; protected set; }

    public static event Action<Mode> OnModeChanged;

    protected override void Start()
    {
        base.Start();
        CurrentMode = modeActive;
        OnModeChanged?.Invoke(CurrentMode); // Invoke saat awal
    }

    protected override Mode GetNextMode(Mode currentMode)
    {
        return (currentMode == Mode.ID) ? Mode.EN : Mode.ID;
    }

    public override void ToggleState()
    {
        base.ToggleState();
        OnModeChanged?.Invoke(CurrentMode); // Invoke saat berubah
    }
}

