using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : Turn
{
	private float turnTimeInterval = 2f;
	private float turnTime;

	private Dictionary<int, Func<bool>> aiFuncs;

	private void Awake()
	{
		aiFuncs = new Dictionary<int, Func<bool>>();
		var aiNames = AINames.AINameList;
		var aiDatas = DataTableManger.AITable;

		foreach (var name in aiNames)
		{
			Func<bool> func = name switch
			{
				AINames.runAI => RunAI,
				AINames.atkSaveAI => AtkSaveAI,
				AINames.moveSaveAI => MoveSaveAI,
				AINames.atkAI => AtkAI,
				AINames.moveAI => MoveAI,
				AINames.defMove => (() => RemainMove(0)),
				AINames.randomMove => (() => RemainMove(1)),
				AINames.hateMove => (() => RemainMove(2)),
				_ => null,
			};
			if (func != null)
				aiFuncs.Add(aiDatas.Get(name).Value, func);
			else
				throw new Exception("Wrong Function Name");
		}
	}

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

	public override void StartTurn()
	{
		base.StartTurn();
	}

	private void EnemyMove()
	{
		for(int i = DataTableManger.AITable.Count - 1; i >= 0; i--)
		{
			if (aiFuncs[i]())
				break;
		}
		turnTime = 0f;
	}

	private bool RunAI()
	{
		var attackedNodes = FindAttackedNode();
		if (attackedNodes.Count != 0 && MoveOptimizeAttackedToy(attackedNodes))
		{
			moveCount--;
			return true;
		}
		return false;
	}

	private bool AtkSaveAI()
	{
		var canAttackNodes = FindCanAttackNode();
		if (canAttackNodes.Count != 0 && MoveOptimizeForAttackingToy(canAttackNodes, true))
		{
			moveCount--;
			return true;
		}
		return false;
	}
	private bool MoveSaveAI()
	{
		if (FindMoveAndAttackNode(true))
		{
			moveCount--;
			return true;
		}
		return false;
	}
	private bool AtkAI()
	{
		var canAttackNodes = FindCanAttackNode();
		if (canAttackNodes.Count != 0 && MoveOptimizeForAttackingToy(canAttackNodes, false))
		{
			moveCount--;
			return true;
		}
		return false;
	}
	private bool MoveAI()
	{
		if (FindMoveAndAttackNode(false))
		{
			moveCount--;
			return true;
		}
		return false;
	}
	private bool RemainMove(int i)
	{
		int moveType = FindRemainAndMoveNode();
		if (moveType == i)
		{
			moveCount--;

			return true;
		}
		return false;
	}

	private int FindRemainAndMoveNode()
	{
		var movabledefencePair = new PriorityQueue<(int,int), int>();
		var movableEmptyPair = new List<(int, int)>();
		var movableAttackedPair = new PriorityQueue<(int, int), int>();

		var playersAttackNodes = FindAllAttackNodes(playManager.CurrentPlayers);

		foreach (var enemy in playManager.CurrentEnemies)
		{
			playLogic.ChoosedNode = enemy;
			var movables = playLogic.ShowMovable(enemy.NodeIndex, 0);
			var enemiesAttackNodes = FindAllAttackNodes(playManager.CurrentEnemies, enemy);
			foreach (var movable in movables)
			{
				if (enemiesAttackNodes.ConvertAll(x => x.pos).Contains(movable))
				{
					var defenceNodeIndex = enemiesAttackNodes.ConvertAll(x => x.pos).IndexOf(movable);
					var defenceNode = enemiesAttackNodes[defenceNodeIndex].start;

					movabledefencePair.Enqueue((movable, enemy.NodeIndex), -boardManager.allNodes[defenceNode].Toy.Data.Attack);
				}
				else if (playersAttackNodes.ConvertAll(x => x.pos).Contains(movable))
				{
					movableAttackedPair.Enqueue((movable, enemy.NodeIndex), boardManager.allNodes[enemy.NodeIndex].Toy.Data.Price);
				}
				else
					movableEmptyPair.Add((movable, enemy.NodeIndex));
			}
		}

		(int, int) movePair = new();
		int moveInt = -1;

		if (movabledefencePair.Count > 0)
		{
			var minPriority = movabledefencePair.Priority;
			var pairs = new List<(int, int)> { movabledefencePair.Dequeue() };

			while(movabledefencePair.TryDequeue(out (int, int) pair, out int priority))
			{
				if(minPriority == priority)
					pairs.Add(pair);
			}
			movePair = pairs[UnityEngine.Random.Range(0, pairs.Count)];
			moveInt = 0;
		}
		else if (movableEmptyPair.Count > 0)
		{
			movePair = movableEmptyPair[UnityEngine.Random.Range(0, movableEmptyPair.Count)];
			moveInt = 1;
		}
		else
		{
			var minPriority = movableAttackedPair.Priority;
			var pairs = new List<(int, int)> { movableAttackedPair.Dequeue() };

			while (movableAttackedPair.TryDequeue(out (int, int) pair, out int priority))
			{
				if (minPriority == priority)
					pairs.Add(pair);
			}
			movePair = pairs[UnityEngine.Random.Range(0, pairs.Count)];
			moveInt = 2;
		}

		playLogic.ChoosedNode = boardManager.allNodes[movePair.Item1];
		var beforeNode = boardManager.allNodes[movePair.Item2];
		
		toyControl.ToyMove(ref beforeNode);
		playLogic.ClearNodes();

		return moveInt;
	}

	private List<(int pos, int start)> FindAllAttackNodes(List<Node> nodes, Node except = null)
	{
		var result = new List<(int,int)>();

		foreach (var node in nodes)
		{
			if (except != null && node.NodeIndex == except.NodeIndex)
				continue;

			playLogic.ChoosedNode = node;
			var moves = playLogic.ShowMovable(node.NodeIndex, 0);
			foreach (var move in moves)
			{
				if (boardManager.allNodes[move].Toy == null)
				{
					result.Add((move,node.NodeIndex));
				}
			}
		}

		return result;
	}

	public override void EndTurn()
	{
		turnTime = 0;
		playLogic.ChoosedNode = null;
		base.EndTurn();
	}

	private bool FindMoveAndAttackNode(bool isPlayerAttack)
	{
		var movablePair = new List<(int,int)>();
		var moveCostPair = new List<(int,int)>();
		var playerIndex = playManager.CurrentPlayers.ConvertAll(x => x.NodeIndex);
		var attackingPlayers = FindAttackPlayerToys();
		var attackPlayers = isPlayerAttack ? attackingPlayers : playerIndex.FindAll(x => !attackingPlayers.Contains(x));

		foreach (var node in playManager.CurrentEnemies)
		{
			playLogic.ChoosedNode = node;
			var movables = playLogic.ShowMovable(node.NodeIndex, 0);
			foreach (var movable in movables)
			{
				if (boardManager.allNodes[movable].State == NodeState.Player ||
					boardManager.allNodes[movable].State == NodeState.Attack)
					continue;

				boardManager.allNodes[movable].State = NodeState.Enemy;
				boardManager.allNodes[movable].Toy = node.Toy;
				playLogic.ChoosedNode = boardManager.allNodes[movable];

				var movableAtmovePos = playLogic.ShowMovable(movable, 0);
				foreach(var nextPos in  movableAtmovePos)
				{
					if (attackPlayers.Contains(nextPos))
					{
						movablePair.Add((movable, node.NodeIndex));
						moveCostPair.Add((movable, nextPos));
					}
				}

				boardManager.allNodes[movable].State = NodeState.None;
				boardManager.allNodes[movable].Toy = null;
			}
		}

		playLogic.ClearNodes();
		if (movablePair.Count == 0)
			return false;

		GetMaxPriceTupleIndex(out int maxCostIndex, moveCostPair, false);

		playLogic.ChoosedNode = boardManager.allNodes[movablePair[maxCostIndex].Item1];
		var beforeNode = boardManager.allNodes[movablePair[maxCostIndex].Item2];
		
		toyControl.ToyMove(ref beforeNode);
		playLogic.ClearNodes();

		return true;
	}

	private List<int> FindAttackedNode()
	{
		var result = new List<int>();

		foreach (var node in playManager.CurrentPlayers)
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

	private bool MoveOptimizeForAttackingToy(List<int> toys, bool isPlayerAttack)
	{
		var canMoves = new List<(int, int)>();
		var playerIndex = playManager.CurrentPlayers.ConvertAll(x => x.NodeIndex);
		var attackingPlayers = FindAttackPlayerToys();
		var attackPlayers = isPlayerAttack ? attackingPlayers : playerIndex.FindAll(x => !attackingPlayers.Contains(x));

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

		foreach (var player in playManager.CurrentPlayers)
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

		foreach (var node in playManager.CurrentEnemies)
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
		foreach (var player in playManager.CurrentPlayers)
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
