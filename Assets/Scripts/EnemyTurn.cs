using System;
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
		if (attackedNodes.Count != 0 && MoveOptimizeAttackedToy(attackedNodes))
		{
			moveCount--;
			return;
		}
		var canAttackNodes = FindCanAttackNode();
		if (canAttackNodes.Count != 0)
		{
			if (MoveOptimizeForAttackingToy(canAttackNodes))
			{
				moveCount--;
				return;
			}

		}

		moveCount--;
	}

	public override void EndTurn()
	{
		turnTime = 0;
		playLogic.ChoosedNode = null;
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

	private bool MoveOptimizeForAttackingToy(List<int> toys)
	{
		var canMoves = new List<(int, int)>();
		var attackPlayers = FindAttackPlayerToys();

		foreach (var node in toys)
		{
			playLogic.ChoosedNode = boardManager.allNodes[node];
			var movables = playLogic.ShowMovable(node, 0);
			foreach (var movePos in movables)
			{
				if(attackPlayers.Contains(movePos))
				{
					canMoves.Add((movePos, node));
				}
			}
		}

		playLogic.ClearNodes();
		if (canMoves.Count == 0)
			return false;

		GetMaxPriceTupleIndex(out int maxCostIndex, canMoves, true);

		playLogic.ChoosedNode = boardManager.allNodes[canMoves[maxCostIndex].Item1];
		var beforeNode = boardManager.allNodes[canMoves[maxCostIndex].Item2];
		Destroy(playLogic.ChoosedNode.Toy.gameObject);
		toyControl.ToyMove(ref beforeNode);
		playLogic.ClearNodes();

		return true;
	}

	private List<int> FindAttackPlayerToys()
	{
		var result = new List<int>();

		foreach (var player in players)
		{
			playLogic.ChoosedNode = player;
			var nodes = playLogic.ShowMovable(player.NodeIndex, 0);
			foreach (var node in nodes)
			{
				if (boardManager.allNodes[node].State == NodeState.Attack)
				{
					result.Add(player.NodeIndex);
					break;
				}
			}
		}
		return result;
	}

	private List<int> FindCanAttackNode()
	{
		var result = new List<int>();

		foreach (var node in enemies)
		{
			playLogic.ChoosedNode = node;
			var movables = playLogic.ShowMovable(node.NodeIndex, 0);
			foreach (var movable in movables)
			{
				if (boardManager.allNodes[movable].State == NodeState.Attack)
				{
					result.Add(node.NodeIndex);
					break;
				}
			}
		}

		return result;
	}

	private bool MoveOptimizeAttackedToy(List<int> toys)
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

		playLogic.ClearNodes();
		if (canMoves.Count == 0)
			return false;

		GetMaxPriceTupleIndex(out int maxCostIndex, canMoves, false);

		playLogic.ChoosedNode = boardManager.allNodes[canMoves[maxCostIndex].Item1];
		var beforeNode = boardManager.allNodes[canMoves[maxCostIndex].Item2];
		toyControl.ToyMove(ref beforeNode);
		playLogic.ClearNodes();

		return true;
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

	private void GetMaxPriceTupleIndex(out int maxCostIndex, List<(int, int)> moveList, bool baseFirst)
	{
		maxCostIndex = -1;
		int maxCost = 0;

		for (int i = 0; i < moveList.Count; i++)
		{
			if (!baseFirst && boardManager.allNodes[moveList[i].Item2].Toy.Data.Price > maxCost)
			{
				maxCost = boardManager.allNodes[moveList[i].Item2].Toy.Data.Price;
				maxCostIndex = i;
			}
			else if(baseFirst && boardManager.allNodes[moveList[i].Item1].Toy.Data.Price > maxCost)
			{
				maxCost = boardManager.allNodes[moveList[i].Item1].Toy.Data.Price;
				maxCostIndex = i;
			}
		}
	}
}
