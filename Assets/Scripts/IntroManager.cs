using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntoManager : MonoBehaviour
{
    [Header("Content List")]
    [SerializeField] private List<GameObject> contentList;
    private int currentIdx = 0;
    private CanvasGroup currentCanvasGroup;
    private CanvasGroup nextCanvasGroup;

    [Header("Swipe Gesture Settings")]
    [SerializeField] private float swipeThreshold = 50f;
    private Vector2 touchStartPos;
    private bool isSwiping;

    [Header("Dots Settings")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsContainer;
    private List<Image> dots = new List<Image>();

    [Header("UI Button")]
    [SerializeField] private Button finishButton;

    private void Start()
    {
        finishButton.onClick.AddListener(OnFinishButtonClicked);


        if(!SaveManager.IsFirstLaunch())
            SceneManager.LoadScene("MainMenu");

        InitializePanel();
        InitializeDots(contentList.Count);
    }
    private void Update()
    {
        HandleSwipeInput();
    }

    private void HandleSwipeInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector2 touchEndPos = Input.mousePosition;
            float swipeDistance = touchEndPos.x - touchStartPos.x;

            if (Math.Abs(swipeDistance) > swipeThreshold)
            {

                bool isRightSwipe = swipeDistance > 0;
                HandleSwipe(isRightSwipe);
            }

            isSwiping = false;
        }
    }
    private void HandleSwipe(bool isRightSwipe)
    {
        if (isRightSwipe) PreviousPanel();
        else NextPanel();
    }
    private void NextPanel()
    {
        if (currentIdx < contentList.Count - 1)
            ShowPanel(currentIdx + 1);
    }
    private void PreviousPanel()
    {
        if (currentIdx > 0)
            ShowPanel(currentIdx - 1);
    }
    private void ShowPanel(int index)
    {
        if (index < 0 || index >= contentList.Count) return;

        if (currentCanvasGroup != null)
        {
            currentCanvasGroup.DOFade(0f, 0.5f);
        }

        nextCanvasGroup = contentList[index].GetComponent<CanvasGroup>();
        nextCanvasGroup.alpha = 0f;

        contentList[currentIdx].SetActive(false);
        contentList[index].SetActive(true);

        nextCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
        {
            currentCanvasGroup = nextCanvasGroup;
        });

        currentIdx = index;
        UpdateDots(currentIdx);
    }

    private void InitializePanel()
    {
        if (contentList.Count > 0)
        {
            currentCanvasGroup = contentList[0].GetComponent<CanvasGroup>();
            currentCanvasGroup.alpha = 1f;
        }
    }
    private void InitializeDots(int totalSlides)
    {
        foreach (Transform child in dotsContainer) Destroy(child.gameObject);
        dots.Clear();

        for (int i = 0; i < totalSlides; i++)
        {
            GameObject dot = Instantiate(dotPrefab, dotsContainer);
            Image dotImage = dot.GetComponent<Image>();
            dots.Add(dotImage);
        }

        UpdateDots(0);
    }
    private void UpdateDots(int currentIndex)
    {
        for (int i = 0; i < dots.Count; i++)
        {
            dots[i].color = (i == currentIdx) ? Color.white : Color.gray;
        }
    }

    //Event Button
    private void OnFinishButtonClicked()
    {
        SaveManager.SetFirstLaunchCompleted();
        SceneManager.LoadScene("MainMenu");
    }
}
