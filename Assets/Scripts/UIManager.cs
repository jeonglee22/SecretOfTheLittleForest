using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayLogic playLogic;
    public ToyControl toyControl;
    public BoardManager boardManager;
    public PlayManager playManager;

    public TMP_Dropdown dropdown;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI endText;

    public Toy toy;

	private void Start()
	{
		turnText.text = string.Empty;
        endText.text = string.Empty;
	}

	public void OnValueChangeMoveType()
    {
        var moveType = (MoveType)dropdown.value;
        playLogic.ClearNodes();
    }

    public void OnClickSetPlayerToy()
    {
        var node = boardManager.GetRandomNodeInPlayer();
        if(node != null )
            boardManager.ToySettingOnNode( node, toy, false);
    }
    public void OnClickSetEnemyToy()
    {
        var node = boardManager.GetRandomNodeInEnemy();
        if(node != null)
		    boardManager.ToySettingOnNode( node, toy, true);

		//var nodeTuples = boardManager.SetEnemyStageData();
		//foreach (var nodeTuple in nodeTuples)
		//{
		//	toy.Data = nodeTuple.data;
		//	boardManager.ToySettingOnNode(nodeTuple.node, toy, true);
		//}
	}

    public void OnClickPlayGame()
    {
		playManager.StartGame();
    }

    public void SetTurnText(bool isEnemy)
    {
        turnText.text = isEnemy ? "Enemy Turn" : "Player Turn";
    }

	public void SetEndText(bool isEnemyWin)
	{
        endText.text = isEnemyWin ? "Enemy Win" : "Player Win";
	}

    public void OnValuePlayerChanged(bool b)
    {
        playManager.ShowPlayerStats(b);
    }

	public void OnValueEnemyChanged(bool b)
	{
		playManager.ShowEnemyStats(b);
	}
}
