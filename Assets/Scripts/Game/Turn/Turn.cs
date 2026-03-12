using UnityEngine;

public class Turn : MonoBehaviour
{
	protected int moveCount = 1;
	public int MoveCount { get { return moveCount; } }

	public PlayManager playManager;
	public BoardManager boardManager;
	public PlayLogic playLogic;
	public ToyControl toyControl;

	protected bool isActionEnd = false;

	public virtual void StartTurn()
	{
		moveCount = Mathf.FloorToInt(DataTableManger.SettingTable.Get(Settings.moveCount));
	}

	public virtual void EndTurn()
	{
		playManager.IsFinishTurn = true;
	}
}
