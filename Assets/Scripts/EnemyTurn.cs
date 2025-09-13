using UnityEngine;

public class EnemyTurn : Turn
{
	private float turnTimeInterval = 2f;
	private float turnTime;

	private void Start()
	{
		turnTime = 0;
	}

	private void Update()
	{
		if (playManager.PlayTurn != PlayTurn.Enemy || !playManager.IsTurnStart)
			return;

		turnTime += Time.deltaTime;
		if (turnTime < turnTimeInterval)
			return;

		if (Input.touchCount == 1)
			EnemyMove();

		if (moveCount == 0)
		{
			EndTurn();
			return;
		}
	}

	public override void StartTurn()
	{
		base.StartTurn();
		Debug.Log("Enemy Start");
		Debug.Log(moveCount);
	}

	private void EnemyMove()
	{
		moveCount--;
	}

	public override void EndTurn()
	{
		Debug.Log("End Enemy");
		turnTime = 0;
		base.EndTurn();
	}
}
