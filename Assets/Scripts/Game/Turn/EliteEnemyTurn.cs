using UnityEngine;

public class EliteEnemyTurn : EnemyTurn
{

    // Update is called once per frame
    protected override void Update()
    {
		if (playManager.PlayTurn != PlayTurn.EliteEnemy || !playManager.IsTurnStart || toyControl.IsMove)
			return;

		turnTime += Time.deltaTime;
		if (turnTime < turnTimeInterval)
			return;

		playManager.ResetToys();
		if (playManager.CheckPlayerCaptainDie())
		{
			playManager.IsEndGame = true;
			playManager.IsEnemyWin = true;
			playManager.LoseType = LoseType.KilledKing;
			return;
		}
		else if (playManager.CheckPlayerExceptCaptainDie())
		{
			playManager.IsEndGame = true;
			playManager.IsEnemyWin = true;
			playManager.LoseType = LoseType.KilledExceptKing;
			return;
		}

		if (moveCount == 0 || playManager.CheckEliteEnemyKingDead())
		{
			EndTurn();
			return;
		}

		movableAttackedPair.Clear();
		movabledefencePair.Clear();
		movableEmptyPair.Clear();

		EnemyMove();
	}
}
