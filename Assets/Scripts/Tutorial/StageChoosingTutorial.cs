using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChoosingTutorial : TutorialManager
{
    public GameObject holdingArrow;
    public GameObject explainArrow;

    public TextMeshProUGUI stageExplainText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        if (!IsTutorial)
            return;

        behaveFunc.Add(() => Holding());
        behaveFunc.Add(() => AnyTouch());
        behaveFunc.Add(() => ChooseNormal());

        holdingArrow.SetActive(true);
        explainArrow.SetActive(false);

        SetTutorialText();
        isHoldingEvent = true;
    }

    private void Holding()
    {
        holdingArrow.SetActive(false);
        explainArrow.SetActive(true);
        stageExplainText.text = DataTableManger.StageStringTable.GetExplainString(1);

        tutorialIndex++;
        textIndex++;
        SetTutorialText();
    }

    private void AnyTouch()
    {
        holdingArrow.SetActive(true);
        explainArrow.SetActive(false);
        stageExplainText.text = DataTableManger.StageStringTable.GetExplainString(0);

        tutorialIndex++;
        textIndex++;
        SetTutorialText();
    }

    private void ChooseNormal()
    {
        holdingArrow.SetActive(false);
        explainArrow.SetActive(false);

        tutorialIndex++;
        SceneManager.LoadScene((int)Scenes.Game);
    }
}
