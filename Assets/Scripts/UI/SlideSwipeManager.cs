using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlideSwipeManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public RectTransform[] slides;
    public float transitionSpeed = 10f;

    private int currentIndex = 0;
    private Vector2 startDragPos;
    private Vector2 targetPos;
    private bool isTransitioning = false;

    void Start()
    {
        UpdateSlidePositions();
        SetTargetPosition(currentIndex);
    }

    void Update()
    {
        if (isTransitioning)
        {
            Vector2 current = ((RectTransform)transform).anchoredPosition;
            ((RectTransform)transform).anchoredPosition = Vector2.Lerp(current, targetPos, transitionSpeed * Time.deltaTime);

            if (Vector2.Distance(current, targetPos) < 0.1f)
            {
                ((RectTransform)transform).anchoredPosition = targetPos;
                isTransitioning = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float dragDelta = eventData.position.x - startDragPos.x;

        if (Mathf.Abs(dragDelta) > Screen.width * 0.1f) // minimal drag threshold
        {
            if (dragDelta < 0 && currentIndex < slides.Length - 1)
                currentIndex++;
            else if (dragDelta > 0 && currentIndex > 0)
                currentIndex--;
        }

        SetTargetPosition(currentIndex);
    }

    private void SetTargetPosition(int index)
    {
        float screenWidth = ((RectTransform)transform).rect.width;
        targetPos = new Vector2(-index * screenWidth, 0);
        isTransitioning = true;
    }

    private void UpdateSlidePositions()
    {
        float screenWidth = ((RectTransform)transform).rect.width;

        for (int i = 0; i < slides.Length; i++)
        {
            slides[i].anchoredPosition = new Vector2(i * screenWidth, 0);
        }
    }
}
