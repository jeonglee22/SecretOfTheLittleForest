using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : Turn
{
	private float turnTimeInterval = 2f;
	private float turnTime;

	private List<Node> enemies;
	private List<Node> players;

	private void Start()
	{
		turnTime = 0;
	}

	private void Update()
	{
		if (playManager.PlayTurn != PlayTurn.Enemy || !playManager.IsTurnStart || toyControl.IsMove)
			return;

		turnTime += Time.deltaTime;
		if (turnTime < turnTimeInterval)
			return;

		if (moveCount == 0)
		{
			EndTurn();
			return;
		}

		if (Input.touchCount == 1)
			EnemyMove();
	}

	public override void StartTurn()
	{
		base.StartTurn();
		players = new List<Node>();
		enemies = new List<Node>();
		SetToys();
	}

	private void EnemyMove()
	{
		var enemyCase = new List<List<int>>();
		var attackedNodes = FindAttackedNode();
		if(attackedNodes.Count != 0)
		{
			MoveOptimizeToy(attackedNodes);
			moveCount--;
			return;
		}

		moveCount--;
	}

	public override void EndTurn()
	{
		turnTime = 0;
		base.EndTurn();
	}

	public void SetToys()
	{
		var allNodes = boardManager.allNodes;
		foreach (var node in allNodes)
		{
			if (node.State == NodeState.Enemy)
				enemies.Add(node);
			else if (node.State == NodeState.Player)
				players.Add(node);
		}
	}

	private List<int> FindAttackedNode()
	{
		var result = new List<int>();

		foreach (var node in players)
		{
			playLogic.ChoosedNode = node;
			var movables = playLogic.ShowMovable(node.NodeIndex, 0);
			foreach (var movable in movables)
			{
				if (boardManager.allNodes[movable].State == NodeState.Attack)
				{
					result.Add(movable);
				}
			}
			playLogic.ClearNodes();
		}

		return result;
	}

	private void MoveOptimizeToy(List<int> toys)
	{
		var canMoves = new List<(int,int)>();

		foreach (var node in toys)
		{
			playLogic.ChoosedNode = boardManager.allNodes[node];
			var movables = playLogic.ShowMovable(node, 0);
			foreach (var movePos in movables)
			{
				if(CheckAttacked(movePos) || boardManager.allNodes[movePos].State == NodeState.Player)
				{
					continue;
				}
				canMoves.Add((movePos,node));
			}
		}

		var maxCostIndex = -1;
		var maxCost = 0;
		for (int i = 0; i < canMoves.Count; i++)
		{
			if (boardManager.allNodes[canMoves[i].Item2].Toy.Data.Price > maxCost)
			{
				maxCost = boardManager.allNodes[canMoves[i].Item2].Toy.Data.Price;
				maxCostIndex = i;
			}
		}

		playLogic.ChoosedNode = boardManager.allNodes[canMoves[maxCostIndex].Item1];
		var beforeNode = boardManager.allNodes[canMoves[maxCostIndex].Item2];
		toyControl.ToyMove(ref beforeNode);
	}

	private bool CheckAttacked(int node)
	{
		foreach (var player in players)
		{
			playLogic.ChoosedNode = player;
			var movables = playLogic.ShowMovable(player.NodeIndex, 0);
			foreach (var movable in movables)
			{
				if (node == movable)
				{
					return true;
				}
			}
			playLogic.ClearNodes();
		}

		return false;
	}
}
