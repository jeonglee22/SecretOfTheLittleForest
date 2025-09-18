using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
	private List<Node> enemies;
	public List<Node> CurrentEnemies { get => enemies; }
	private List<Node> players;
	public List<Node> CurrentPlayers { get => players; }

	public PlayerTurn playerTurn;
	public EnemyTurn enemyTurn;
	public UIManager manager;
	public BoardManager boardManager;
	public GameCanvasManager gameCanvasManager;

	private PlayTurn playTurn = PlayTurn.None;
	public PlayTurn PlayTurn { get { return playTurn; } set { playTurn = value; } }

	public bool IsFinishTurn { get; set; } = false;
	public bool IsTurnStart {  get; private set; }
	public bool IsEndGame { get; set; } = false;
	public bool IsEnemyWin { get; set; }

	private int totalTurn;

	private void Awake()
	{
		enemies = new List<Node>();
		players = new List<Node>();
	}

	private void Start()
	{
		totalTurn = (int) DataTableManger.SettingTable.Get(Settings.battleTurnCount);
	}

	private void Update()
	{
		if (IsEndGame || totalTurn == 0)
		{
			EndGame();
			return;
		}

		if(playTurn == PlayTurn.Player)
		{
			UpdatePlayer();
		}
		else if(playTurn == PlayTurn.Enemy)
		{
			UpdateEnemy();
		}
	}

	private void EndGame()
	{
		manager.SetEndText(IsEnemyWin);
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
		if(isPlayerTurn)
			totalTurn--;
		gameCanvasManager.SetTurnText(totalTurn);
	}

	public void AddEnemies(Node enemy)
	{
		enemies.Add(enemy);
	}

	public void AddPlayers(Node player)
	{
		players.Add(player);
	}

	public void ResetToys()
	{
		var allNodes = boardManager.allNodes;
		enemies = new List<Node>();
		players = new List<Node>();
		foreach (var node in allNodes)
		{
			if (node.State == NodeState.Enemy)
				enemies.Add(node);
			else if (node.State == NodeState.Player)
				players.Add(node);
		}
	}

	public int GetAlivePlayerCount() { return players.Count; }
	public int GetAliveEnemyCount() { return enemies.Count; }

	public void ShowPlayerStats(bool b)
	{
		ResetToys();
		foreach (var node in players)
		{
			node.Toy.SetActiveInfoCanvas(b);
		}
	}

	public void ShowEnemyStats(bool b)
	{
		ResetToys();
		foreach (var node in enemies)
		{
			node.Toy.SetActiveInfoCanvas(b);
		}
	}
}
