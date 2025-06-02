using UnityEngine;

public class ToggleMode : ToggleButton<ToggleMode.Mode>
{
    public enum Mode
    {
        AR_Mode,
        NFC_Mode
    }

    [SerializeField]
    private Mode modeActive = Mode.AR_Mode;

    public override Mode CurrentMode { get; protected set; }

    protected override void Start()
    {
        base.Start();
        CurrentMode = modeActive;
    }

    protected override Mode GetNextMode(Mode currentMode)
    {
        return (currentMode == Mode.AR_Mode) ? Mode.NFC_Mode : Mode.AR_Mode;
    }
}
