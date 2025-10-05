using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject blockCanvas;

    public List<RectTransform> tutorialRects;
    protected int tutorialIndex = 0;
    protected static int textIndex = 0;

    public TextMeshProUGUI explainText;

    public static bool IsTutorial = true;
    protected bool isHoldingEvent = false;
    private bool isShow = false;

    protected int touchId = -1;
    protected float touchStartTime;
    protected Vector2 touchStartPos;

    protected List<Action> behaveFunc = new List<Action>();

    private void OnEnable()
    {
        //IsTutorial = SaveLoadManager.Data.IsTutorial;
        if (IsTutorial)
        {
            blockCanvas.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount != 1)
            return;

        var touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if(touchId == -1)
                {
                    touchId = touch.fingerId;
                    touchStartTime = Time.time;
                    touchStartPos = touch.position;
                }
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (touchId == touch.fingerId && (Time.time - touchStartTime > 0.25f) &&
                    Vector2.Distance(touch.position, touchStartPos) < 5f && isHoldingEvent && !isShow)
                {
                    isShow = true;
                    TutorialHoldingFunc();
                }
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (isHoldingEvent)
                    isHoldingEvent = false;
                else if(touchId == touch.fingerId && !isHoldingEvent)
                {
                    TutorialDoFunc();
                }
                isShow = false;
                break;
        }
    }

    protected void TutorialDoFunc() 
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(tutorialRects[tutorialIndex], Input.GetTouch(0).position))
        {
            behaveFunc[tutorialIndex]();
        }
    }

    protected void TutorialHoldingFunc() 
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(tutorialRects[tutorialIndex], Input.GetTouch(0).position))
        {
            behaveFunc[tutorialIndex]();
        }
    }

    protected void SetTutorialText()
    {
        explainText.text = DataTableManger.StageStringTable.GetTutorialString(textIndex);
    }
}
