using UnityEngine;
using UnityEngine.UI;

public class IntroPageController : MonoBehaviour
{
    public Button finishButton;
    //public IntroManager introManager;

    private void OnEnable()
    {
        if (finishButton != null)
            finishButton.onClick.AddListener(OnFinish);
    }
    private void OnDisable()
    {
        if (finishButton != null)
            finishButton.onClick.RemoveListener(OnFinish);
    }

    void OnFinish()
    {
        //introManager.OnFinishIntro();
    }
}
