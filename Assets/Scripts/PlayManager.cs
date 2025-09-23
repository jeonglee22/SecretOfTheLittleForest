using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
	private List<Node> enemies;
	public List<Node> CurrentEnemies 
	{ 
		get {
			if (boardManager.BattleType == BattleType.Elite)
			{
				if (playTurn == PlayTurn.Enemy)
					return enemies;
				else
					return eliteEnemies;
			}
			else
			{
				return enemies;
			}
		}
	}
	private List<Node> eliteEnemies;
	private List<Node> players;
	public List<Node> CurrentPlayers 
	{
		get
		{
			if(boardManager.BattleType == BattleType.Elite)
			{
				var result = new List<Node>(players);
				if (playTurn == PlayTurn.Enemy)
				{
					result.AddRange(eliteEnemies);
				}
				else if (playTurn == PlayTurn.EliteEnemy)
				{
					result.AddRange(enemies);
				}
				return result;
			}
			else
				return players;
		}
	}

	public PlayerTurn playerTurn;
	public EnemyTurn enemyTurn;
	public EliteEnemyTurn eliteEnemyTurn;
	public UIManager manager;
	public BoardManager boardManager;
	public GameCanvasManager gameCanvasManager;

	public GameObject blockPlane;
	public List<GameObject> resultWindows;

	private PlayTurn playTurn = PlayTurn.None;
	public PlayTurn PlayTurn { get { return playTurn; } set { playTurn = value; } }

	public bool IsFinishTurn { get; set; } = false;
	public bool IsTurnStart {  get; private set; }
	public bool IsEndGame { get; set; } = false;
	public bool IsEnemyWin { get; set; }
	public bool IsTurnShown { get; set; }

	private int totalTurn;

	private void Awake()
	{
		enemies = new List<Node>();
		eliteEnemies = new List<Node>();
		players = new List<Node>();
	}

	private void Start()
	{
		//totalTurn = (int) DataTableManger.SettingTable.Get(Settings.battleTurnCount);
		totalTurn = 4;
		blockPlane.SetActive(false);
		gameCanvasManager.SetTurnText(totalTurn);
	}

	private void Update()
	{
		if (IsEndGame || totalTurn == 0)
		{
			EndGame();
			return;
		}

		if (IsTurnShown)
			return;

		if(playTurn == PlayTurn.Player)
		{
			UpdatePlayer();
		}
		else if(playTurn == PlayTurn.Enemy)
		{
			UpdateEnemy();
		}
		else if(boardManager.BattleType == BattleType.Elite && playTurn == PlayTurn.EliteEnemy)
		{
			UpdateEliteEnemy();
		}
	}

	private void EndGame()
	{
		blockPlane.SetActive(true);
		if(totalTurn == 0)
		{
			var playLogic = boardManager.gameObject.GetComponent<PlayLogic>();
			playLogic.ClearNodes();
			ResetToys();
			int playerCost = GetRemainToyCost(players);
			int enemyCost = GetRemainToyCost(enemies);
			int enemy2Cost = GetRemainToyCost(eliteEnemies);
			int gameResult = playerCost > enemyCost + enemy2Cost ? 1 : (playerCost < enemyCost + enemy2Cost ? -1 : 0);

			OpenResultWindow(gameResult);
		}
		else
		{
			int gameResult = IsEnemyWin ? 1 : -1;
			OpenResultWindow(gameResult);
		}
	}

	public void ForceEndGame(bool end = true)
	{
		IsEnemyWin = end;
		EndGame();
	}

	private void OpenResultWindow(int num)
	{
		List<GameObject> cases = new List<GameObject>()
		{
		resultWindows[(int)boardManager.BattleType],
		resultWindows[3],
		resultWindows[4],
		};

		cases[1 - num].SetActive(true);
	}

	private int GetRemainToyCost(List<Node> toys)
	{
		int totalCost = 0;
		foreach (Node node in toys)
		{
			totalCost += node.Toy.Data.Price;
		}
		return totalCost;
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
			EndEachTurn(PlayTurn.Enemy);
			if(boardManager.BattleType == BattleType.Elite)
			{
				playTurn = PlayTurn.EliteEnemy;
			}
			else
			{
				playTurn = PlayTurn.Player;
			}
			if(totalTurn != 0)
				manager.SetTurnText(playTurn);
		}
	}
	private void UpdateEliteEnemy()
	{
		if(!IsTurnStart)
		{
			eliteEnemyTurn.StartTurn();
			IsTurnStart = true;
		}

		if (IsFinishTurn)
		{
			EndEachTurn(PlayTurn.EliteEnemy);
			playTurn = PlayTurn.Player;
			if (totalTurn != 0)
				manager.SetTurnText(playTurn);
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
			EndEachTurn(PlayTurn.Player);
			playTurn = PlayTurn.Enemy;
			if (totalTurn != 0)
				manager.SetTurnText(playTurn);
		}
	}

	public void StartGame()
	{
		playTurn = PlayTurn.Player;
		manager.SetTurnText(playTurn);
	}

	public void EndEachTurn(PlayTurn turn)
	{
		IsTurnStart = false;
		IsFinishTurn = false;
		if ((boardManager.BattleType != BattleType.Elite && turn == PlayTurn.Enemy ) ||
			(boardManager.BattleType == BattleType.Elite && turn == PlayTurn.EliteEnemy))
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
		eliteEnemies = new List<Node>();
		players = new List<Node>();
		foreach (var node in allNodes)
		{
			if (node.State == NodeState.Enemy)
			{
				if (node.Toy.IsElite)
					eliteEnemies.Add(node);
				else
					enemies.Add(node);
			}
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
		foreach (var node in eliteEnemies)
		{
			node.Toy.SetActiveInfoCanvas(b);
		}
	}
}
