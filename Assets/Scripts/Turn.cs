using UnityEngine;

public class Turn : MonoBehaviour
{
	protected int moveCount = 1;

	private int maxTurns = 40;

	public PlayManager playManager;
	public BoardManager boardManager;
	public PlayLogic playLogic;
	public ToyControl toyControl;

	protected bool isActionEnd = false;

	public virtual void StartTurn()
	{
		moveCount = 1;
	}

	public virtual void EndTurn()
	{
		playManager.IsFinishTurn = true;
	}
}
