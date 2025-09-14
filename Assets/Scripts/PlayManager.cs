using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
	private List<Node> enemies = new List<Node>();
	private List<Node> players = new List<Node>();

	public PlayerTurn playerTurn;
	public EnemyTurn enemyTurn;
	public UIManager manager;

	private PlayTurn playTurn = PlayTurn.None;
	public PlayTurn PlayTurn { get { return playTurn; } set { playTurn = value; } }

	public bool IsFinishTurn { get; set; } = false;
	public bool IsTurnStart {  get; private set; }

	private void Update()
	{
		if(playTurn == PlayTurn.Player)
		{
			UpdatePlayer();
		}
		else if(playTurn == PlayTurn.Enemy)
		{
			UpdateEnemy();
		}
	}

	private void UpdateEnemy()
	{
		if(!IsTurnStart)
		{
			enemyTurn.StartTurn();
			IsTurnStart = true;
		}

		if (IsFinishTurn)
		{
			EndEachTurn(false);
			playTurn = PlayTurn.Player;
			manager.SetTurnText(false);
		}
	}

	private void UpdatePlayer()
	{
		if (!IsTurnStart)
		{ 
			playerTurn.StartTurn();
			IsTurnStart = true;
		}

		if (IsFinishTurn)
		{
			EndEachTurn(true);
			playTurn = PlayTurn.Enemy;
			manager.SetTurnText(true);
		}
	}

	public void StartGame()
	{
		playTurn = PlayTurn.Player;
		manager.SetTurnText(false);
	}

	public void EndEachTurn(bool isPlayerTurn)
	{
		IsTurnStart = false;
		IsFinishTurn = false;
	}

	public void AddEnemies(Node enemy)
	{
		enemies.Add(enemy);
	}

	public void AddPlayers(Node player)
	{
		players.Add(player);
	}

	public int GetAlivePlayerCount() { return players.Count; }
	public int GetAliveEnemyCount() { return enemies.Count; }
}
