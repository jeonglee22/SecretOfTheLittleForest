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

    public Toy toy;

	private void Start()
	{
		turnText.text = string.Empty;
	}

	public void OnValueChangeMoveType()
    {
        var moveType = (MoveType)dropdown.value;
        playLogic.ClearNodes();
    }

    public void OnClickSetPlayerToy()
    {
        var node = boardManager.GetRandomNodeInPlayer();
        boardManager.ToySettingOnNode( node, toy, false);
    }
    public void OnClickSetEnemyToy()
    {
        var node = boardManager.GetRandomNodeInEnemy();
		boardManager.ToySettingOnNode( node, toy, true);
	}

    public void OnClickPlayGame()
    {
		playManager.StartGame();
    }

    public void SetTurnText(bool isEnemy)
    {
        turnText.text = isEnemy ? "Enemy Turn" : "Player Turn";
    }
}
