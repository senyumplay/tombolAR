using System.Collections;
using System.Collections.Generic;
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

    protected override void Start()
    {
        base.Start();
        CurrentMode = modeActive;
    }

    protected override Mode GetNextMode(Mode currentMode)
    {
        return (currentMode == Mode.ID) ? Mode.EN : Mode.ID;
    }
}
