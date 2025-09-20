using System.Collections.Generic;
using UnityEngine;

public class PlayLogic : MonoBehaviour
{
	public BoardManager boardManager;
	public PlayManager playManager;
	public ToyControl control;

	private Node choosedNode;
	public Node ChoosedNode 
	{  
		get { return choosedNode; } 
		set 
		{
			choosedNode = value;
			if (choosedNode != null && choosedNode.Toy != null)
				currentMoveType = choosedNode.Toy.MoveType;
			else
				currentMoveType = MoveType.None;
			ClearNodes();
		} 
	}
	private List<Node> allNodes;

	public static int allNodeCount = 16 * 6;

	private MoveType currentMoveType;

	private void Awake()
	{
		allNodes = new List<Node>();
	}

	private void Start()
	{
		for (int i = 0; i < boardManager.allNodes.Count; i++)
		{
			allNodes.Add(boardManager.allNodes[i]);
		}
		for (int i = 0; i < allNodes.Count; i++)
		{
			allNodes[i].NodeInit();
		}
	}

	private void Update()
	{
	}

	public void ClearNodes()
	{
		foreach (var node in allNodes)
		{
			if (node.Toy == null)
				node.State = NodeState.None;
			else
			{
				node.State = node.Toy.IsEnemy ? NodeState.Enemy : NodeState.Player;
			}
		}
	}

	public List<int> ShowMovable(int choosedNode, int moveCount, bool isFirst = true)
	{
		var movableNodes = new List<int>();
		List<int> nextNodes = new List<int>();

		switch (currentMoveType)
		{
			case MoveType.Pawn when moveCount == 0:
				nextNodes = MoveAxis(choosedNode, ref moveCount, isFirst);
				foreach (var node in nextNodes)
					if (node != Node.outSide)
					{
						movableNodes.AddRange(nextNodes);
						ShowMovable(node, moveCount, false);
					}
				moveCount = 0;
				nextNodes = MoveCross(choosedNode, ref moveCount, isFirst);
				foreach (var node in nextNodes)
					if (node != Node.outSide)
					{
						MoveCross(node,ref moveCount, false);
					}
				break;
			case MoveType.Pawn when moveCount == 1:
				nextNodes = MoveAxis(choosedNode, ref moveCount, isFirst);
				break;
			case MoveType.King:
				nextNodes = MoveAxis(choosedNode, ref moveCount, isFirst);
				nextNodes.AddRange(MoveCross(choosedNode, ref moveCount, isFirst));
				if (moveCount == 4)
					return movableNodes;
				foreach (var node in nextNodes)
					if (node != Node.outSide)
					{
						movableNodes.Add(node);
						movableNodes.AddRange(ShowMovable(node, moveCount, false));
					}
				break;
			case MoveType.Knight when moveCount == 0:
				nextNodes = MoveCross(choosedNode,ref moveCount, isFirst ,false);
				foreach (var node in nextNodes)
					if (node != Node.outSide)
						movableNodes.AddRange(ShowMovable(node, moveCount, false));
				break;
			case MoveType.Knight when moveCount == 1:
				nextNodes = MoveAxis(choosedNode,ref moveCount, isFirst, false);
				foreach (var node in nextNodes)
					if (node != Node.outSide)
					{
						movableNodes.Add(node);
					}
				break;
			case MoveType.Bishop:
				nextNodes = MoveCross(choosedNode, ref moveCount, isFirst);
				foreach (var node in nextNodes)
					if (node != Node.outSide)
					{
						movableNodes.Add(node);
						movableNodes.AddRange(ShowMovable(node, moveCount, false));
					}
				break;
			case MoveType.Rook:
				nextNodes = MoveAxis(choosedNode, ref moveCount, isFirst);
				foreach (var node in nextNodes)
					if (node != Node.outSide)
					{
						movableNodes.Add(node);
						movableNodes.AddRange(ShowMovable(node, moveCount, false));
					}
				break;
			case MoveType.Queen:
				currentMoveType = MoveType.Bishop;
				movableNodes.AddRange(ShowMovable(choosedNode, moveCount));
				currentMoveType = MoveType.Rook;
				movableNodes.AddRange(ShowMovable(choosedNode, moveCount));
				currentMoveType = MoveType.Queen;
				break;
			default:
				break;
		}

		return movableNodes;
	}

	private List<int> Move(int nodeIndex, bool isFirst, bool isAxis)
	{
		var nextNodes = new List<int>();

		var node = allNodes[nodeIndex];
		var axis1 = new List<int>();
		var axis2 = new List<int>();

		if (isAxis)
		{
			axis1 = node.Axis1;
			axis2 = node.Axis2;
		}
		else
		{
			axis1 = node.CrossNode1;
			axis2 = node.CrossNode2;
		}

		var before = node.CommingNode;
		if (isFirst)
		{
			nextNodes.AddRange(axis1);
			nextNodes.AddRange(axis2);

			var resultNodes = new List<int>();
			if(node.IsCenterNode && !isAxis)
				nextNodes.AddRange(node.CenterCrossNode2);

			for (int i = 0; i < nextNodes.Count; i++)
			{
				if (nextNodes[i] != -1 && allNodes[nextNodes[i]].State != choosedNode.State)
				{
					resultNodes.Add(nextNodes[i]);
					allNodes[nextNodes[i]].CommingNode = nodeIndex;
				}
			}

			return resultNodes;
		}
		else
		{
			if (currentMoveType == MoveType.Knight)
			{
				nextNodes.AddRange(axis1);
				nextNodes.AddRange(axis2);

				var resultNodes = new List<int>();
				foreach (var i in nextNodes)
				{
					if (allNodes[before].Axis1.Contains(i) ||
						allNodes[before].Axis2.Contains(i))
						continue;

					if (node.IsCenterNode && allNodes[before].IsCenterNode &&
						(allNodes[i].NodeIndexAxis != node.NodeIndexAxis))
						continue;

					if (i != Node.outSide)
					{
						if ((allNodes[i].State == NodeState.Enemy && ChoosedNode.State == NodeState.Player) ||
							(allNodes[i].State == NodeState.Player && ChoosedNode.State == NodeState.Enemy))
							allNodes[i].State = NodeState.Attack;
						else if (allNodes[i].State == NodeState.Enemy || allNodes[i].State == NodeState.Player)
							continue;
						else
							SetMoveStateAtEmpty(i);
							
						resultNodes.Add(i);
					}
				}

				return resultNodes;
			}
			else if (axis1.Contains(before))
			{
				nextNodes = AddNextNode(axis1, before, nodeIndex);
			}
			else if (axis2.Contains(before))
			{
				nextNodes = AddNextNode(axis2, before, nodeIndex);
				if (node.IsCenterNode && !isAxis)
					nextNodes.AddRange(AddNextNode(node.CenterCrossNode2, before, nodeIndex));
			}
			else if (node.IsCenterNode && !isAxis && node.CenterCrossNode2.Contains(before))
			{
				nextNodes = AddNextNode(node.CenterCrossNode2, before, nodeIndex);
			}
		}

		return nextNodes;
	}

	private List<int> AddNextNode(List<int> axis, int before, int currIndex)
	{
		var index = axis[(axis.IndexOf(before) + 1) % 2];
		if (index != -1)
		{
			if (allNodes[index].State == ChoosedNode.State)
				return new List<int>();

			allNodes[index].CommingNode = currIndex;
		}
		return new List<int> { index };
	}

	private List<int> MoveAxis(int nodeIndex, ref int moveCount, bool isFirst, bool isDraw = true)
	{
		if (CheckAlreadyUsedPos(nodeIndex, isFirst, true))
			return new List<int>();

		if (isDraw && !isFirst)
			SetMoveStateAtEmpty(nodeIndex);

		moveCount++;

		if (currentMoveType == MoveType.Pawn && !isFirst)
			return new List<int>();

		if (ChoosedNode.Toy.MoveType == MoveType.King && !isFirst)
			return new List<int>();

		return Move(nodeIndex, isFirst, true);
	}

	private List<int> MoveCross(int nodeIndex, ref int moveCount, bool isFirst, bool isDraw = true)
	{
		if (CheckAlreadyUsedPos(nodeIndex, isFirst, false))
			return new List<int>();

		if (currentMoveType == MoveType.Pawn && !isFirst)
			return new List<int>();

		if (isDraw && !isFirst)
			SetMoveStateAtEmpty(nodeIndex);

		moveCount++;

		if (ChoosedNode.Toy.MoveType == MoveType.King && !isFirst)
			return new List<int>();

		return Move(nodeIndex, isFirst, false);
	}

	private bool CheckAlreadyUsedPos(int index, bool isFirst, bool isAxis)
	{
		if(currentMoveType == MoveType.Pawn && !isFirst && allNodes[index].State != NodeState.None)
		{
			if (isAxis) 
			{
				return true;
			}
			else
			{
				if ((allNodes[index].State == NodeState.Enemy && ChoosedNode.State == NodeState.Player) ||
				(allNodes[index].State == NodeState.Player && ChoosedNode.State == NodeState.Enemy))
					allNodes[index].State = NodeState.Attack;
				return true;
			}
		}
		else if (allNodes[index].State != NodeState.None && !isFirst && currentMoveType != MoveType.Knight)
		{
			if ((allNodes[index].State == NodeState.Enemy && ChoosedNode.State == NodeState.Player) ||
				(allNodes[index].State == NodeState.Player && ChoosedNode.State == NodeState.Enemy))
				allNodes[index].State = NodeState.Attack;
			return true;
		}

		return false;
	}

	private void SetMoveStateAtEmpty(int index)
	{
		if (ChoosedNode.Toy.IsMove)
		{
			allNodes[index].State = NodeState.Moved;
		}
		else
		{
			allNodes[index].State = ChoosedNode.State == NodeState.Enemy ? NodeState.EnemyMove : NodeState.PlayerMove;
		}
	}
}
