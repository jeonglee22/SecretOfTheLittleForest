using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayLogic playLogic;
    public ToyControl toyControl;
    public BoardManager boardManager;

    public TMP_Dropdown dropdown;

    public Toy toy;

    public void OnValueChangeMoveType()
    {
        var moveType = (MoveType)dropdown.value;
        playLogic.ClearNodes();
    }

    public void OnClickSetPlayerToy()
    {
        var node = boardManager.GetRandomNodeInPlayer();
        boardManager.ToySettingOnNode(node, toy, false);
    }
    public void OnClickSetEnemyToy()
    {
        var node = boardManager.GetRandomNodeInEnemy();
		boardManager.ToySettingOnNode(node, toy, true);
	}
}
