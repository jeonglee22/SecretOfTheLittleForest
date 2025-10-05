using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSettingTutorial : TutorialManager
{
    public GameObject determinePanel;
    public GameObject lockedPanel;
    public GameObject startPanel;
    public GameObject startGamePanel;

    public GameObject originStartPanel;

    public GameObject lockButtonArrow;

    public TextMeshProUGUI crystalText;
    public TextMeshProUGUI havingCrystalText; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        if (!IsTutorial)
            return;

        behaveFunc.Add(() => OpenDeterminePanel());
        behaveFunc.Add(() => OpenPreset());
        behaveFunc.Add(() => OpenStartGamePanel());
        behaveFunc.Add(() => StartGame());
        
        lockedPanel.SetActive(true);
        startPanel.SetActive(false);
        originStartPanel.SetActive(false);

        SetTutorialText();

        crystalText.text = "10";
        havingCrystalText.text = "10";
    }

    private void OpenDeterminePanel()
    {
        determinePanel.SetActive(true);
        determinePanel.transform.SetAsLastSibling();
        lockButtonArrow.SetActive(false);
        tutorialIndex++;
    }

    private void OpenPreset()
    {
        lockedPanel.SetActive(false);
        startPanel.SetActive(true);
        determinePanel.SetActive(false);
        lockButtonArrow.SetActive(true);
        tutorialIndex++;
        textIndex++;
        SetTutorialText();
        havingCrystalText.text = "0";
    }

    private void OpenStartGamePanel()
    {
        startGamePanel.SetActive(true);
        startGamePanel.transform.SetAsLastSibling();
        lockButtonArrow.SetActive(false);
        textIndex++;
        tutorialIndex++;
        SetTutorialText();
    }

    private void StartGame()
    {
        startGamePanel.SetActive(false);
        originStartPanel.SetActive(true);
        SceneManager.LoadScene((int)Scenes.StageChoosing);
        textIndex++;
    }
}
