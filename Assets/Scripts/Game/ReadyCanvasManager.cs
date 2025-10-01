using System;
using System.Text;
using TMPro;
using UnityEngine;

public class ReadyCanvasManager : MonoBehaviour
{
    public TextMeshProUGUI unitCountText;
    public TextMeshProUGUI cameraText;

    public GameObject NonCaptain;
    public GameObject finishButton;

    public BoardManager boardManager;
    public SettingButtonFunctions buttonFunctions;

    private bool beforeCaptain;

    private void OnEnable()
    {

    }

    private void Start()
    {
        SetUnitCountText(GetCountInBoard());
        SetCameraText(false);

        if (boardManager.PlayerDeck.KingPos == -1)
        {
			NonCaptain.SetActive(true);
			finishButton.SetActive(false);
		}
    }

	private void Update()
	{
        var captain = CheckHaveCaptain();
        if (captain == beforeCaptain)
            return;

		if (!captain)
        {
            if (buttonFunctions.IsVibrate) Handheld.Vibrate();
            NonCaptain.SetActive(true);
            finishButton.SetActive(false);
        }
        else
        {
            NonCaptain.SetActive(false);
            finishButton.SetActive(true);
        }
        beforeCaptain = captain;
	}

	private bool CheckHaveCaptain()
	{
        var playerNodes = boardManager.playerStartNodes;

        foreach (var node in playerNodes)
        {
            if (node.Toy == null)
                continue;

            var toy = node.Toy;
            var isKing = toy.IsKing;
            if (isKing)
                return true;
        }

        return false;
	}

	private int GetCountInBoard()
    {
        var count = 0;
        var fieldToys = SaveLoadManager.Data.Deck.Pos;
        foreach (var item in fieldToys)
        {
            if (item != 0)
                count++;
        }

        return count;
    }

    public void SetUnitCountText(int count)
    {
        var sb = new StringBuilder();
        var maxCount = SaveLoadManager.Data.unitCount;

        sb.Append("배치 가능\n").Append($"({count}/{maxCount})");
        unitCountText.text = sb.ToString();

        if (count == maxCount)
        {
            unitCountText.color = Color.red;
        }
        else
        {
            unitCountText.color = Color.white;
        }
    }

    public void SetCameraText(bool isGameView)
    {
        if (isGameView)
        {
            cameraText.text = "전체 화면";
        }
        else
        {
            cameraText.text = "배치 화면";
        }
    }
}
