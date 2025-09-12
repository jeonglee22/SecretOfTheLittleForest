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
		playLogic.MoveType = moveType;
        playLogic.ClearNodes();
    }

    public void OnClickSetPlayerToy()
    {
        boardManager.ToySettingOnNode(playLogic.ChoosedNode, toy, false);
    }
    public void OnClickSetEnemyToy()
    {
		boardManager.ToySettingOnNode(playLogic.ChoosedNode, toy, true);
	}
}
