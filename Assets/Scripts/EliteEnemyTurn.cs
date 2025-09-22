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
		if (playManager.CurrentEnemies.Count == 0)
		{
			playManager.IsEndGame = true;
			playManager.IsEnemyWin = false;
			return;
		}
		else if (playManager.CurrentPlayers.Count == 0)
		{
			playManager.IsEndGame = true;
			playManager.IsEnemyWin = true;
			return;
		}

		if (moveCount == 0)
		{
			EndTurn();
			return;
		}

		EnemyMove();
	}
}
