using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ScrollableInputField : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ScrollRect scrollRect;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        scrollRect = GetComponentInParent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!inputField.isFocused)
            scrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!inputField.isFocused)
            scrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!inputField.isFocused)
            scrollRect.OnEndDrag(eventData);
    }
}
