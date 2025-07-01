using UnityEngine;
using UnityEngine.UI;

public class EventButtonSO : MonoBehaviour
{
    [SerializeField] private Button targetButton;

    [Header("Use Parameter?")]
    public bool useParameter = false;

    [Header("Events - No Parameter")]
    public GameEventSO gameEvent;

    [Header("Events - With Parameter")]
    public StringGameEventSO stringGameEvent;
    public IntGameEventSO intGameEvent;
    public FloatGameEventSO floatGameEvent;
    public BoolGameEventSO boolGameEvent;

    [Header("Parameter Values")]
    public string stringValue;
    public int intValue;
    public float floatValue;
    public bool boolValue;

    private void Awake()
    {
        if (targetButton == null)
            targetButton = GetComponent<Button>();

        if (targetButton != null)
            targetButton.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        if (!useParameter)
        {
            gameEvent?.Raise();
        }
        else
        {
            stringGameEvent?.Raise(stringValue);
            intGameEvent?.Raise(intValue);
            floatGameEvent?.Raise(floatValue);
            boolGameEvent?.Raise(boolValue);
        }
    }
}
