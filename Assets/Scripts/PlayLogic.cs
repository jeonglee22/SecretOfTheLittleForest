using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayLogic : MonoBehaviour
{
	public BoardManager boardManager;
	public ToyControl control;

	private Node choosedNode;
	public Node ChoosedNode {  get { return choosedNode; } set { choosedNode = value; } }
	private List<Node> allNodes;

	public static int allNodeCount = 16 * 6;

	private MoveType currentMoveType;

	//private MoveType moveType;

	//public MoveType MoveType 
	//{
	//	get { return moveType; }
	//	set 
	//	{
	//		if (moveType != value)
	//		{
	//			moveType = value;
	//		}
	//	}
	//}

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
		if (Input.touches.Length == 0)
			return;

		if (control.IsMove)
			return;

		var touchPos = Input.GetTouch(0).position;
		var ray = Camera.main.ScreenPointToRay(touchPos);

		if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerId.node))
		{
			var go = hit.collider.gameObject;
			var beforeNode = choosedNode;
			choosedNode = go.GetComponent<Node>();

			if (beforeNode != null && beforeNode.Toy != null && 
				boardManager.IsChoosed && 
				((choosedNode.State == NodeState.PlayerMove && beforeNode.State == NodeState.Player) || 
				(choosedNode.State == NodeState.EnemyMove && beforeNode.State == NodeState.Enemy) ||
				(choosedNode.State == NodeState.Attack)))
			{
				if (choosedNode.State == NodeState.Attack)
					Destroy(choosedNode.Toy.gameObject);

				ClearNodes();
				
				control.ToyMove(ref beforeNode);
				choosedNode.State = beforeNode.State;
				beforeNode.State = NodeState.None;
				choosedNode = null;
				return;
			}

			ClearNodes();

			if (choosedNode.Toy != null)
			{
				currentMoveType = choosedNode.Toy.MoveType;
				ShowMovable(choosedNode.NodeIndex, 0);
			}
			else
				choosedNode.State = NodeState.Choose;

			boardManager.IsChoosed = true;
			//ShowNeighbor();
		}
		else if(Physics.Raycast(ray, out RaycastHit uihit, Mathf.Infinity, ~LayerId.ui))
		{
			boardManager.IsChoosed = false;
			ClearNodes();
			choosedNode = null;
			currentMoveType = MoveType.None;
		}
	}

	public void ClearNodes()
	{
		foreach (var node in allNodes)
		{
			if( node.Toy == null)
				node.State = NodeState.None;
			else
			{
				node.State = node.Toy.IsEnemy ? NodeState.Enemy : NodeState.Player;
			}
		}
	}

	private void ShowNeighbor()
	{
		var axis1 = choosedNode.Axis1;
		var axis2 = choosedNode.Axis2;

		var cross1 = choosedNode.CrossNode1;
		var cross2 = choosedNode.CrossNode2;

		var centerCross = new List<int>();
		if (choosedNode.IsCenterNode)
			centerCross = choosedNode.CenterCrossNode2;

		for (int i = 0; i < axis1.Count; i++) 
		{
			if (axis1[i] != Node.outSide)
				allNodes[axis1[i]].State = NodeState.PlayerMove;
			if (axis2[i] != Node.outSide)
				allNodes[axis2[i]].State = NodeState.PlayerMove;
			if (cross1[i] != Node.outSide)
				allNodes[cross1[i]].State = NodeState.EnemyMove;
			if (cross2[i] != Node.outSide)
				allNodes[cross2[i]].State = NodeState.EnemyMove;

			if(centerCross.Count > 0)
				allNodes[centerCross[i]].State = NodeState.EnemyMove;
		}
	}

	private void ShowMovable(int choosedNode, int moveCount, bool isFirst = true)
	{
		List<int> nextNodes = new List<int>();

		switch (currentMoveType)
		{
			case MoveType.Pawn:
				nextNodes = MoveAxis(choosedNode, ref moveCount, isFirst);
				break;
			case MoveType.King:
				nextNodes = MoveAxis(choosedNode, ref moveCount, isFirst);
				nextNodes.AddRange(MoveCross(choosedNode, ref moveCount, isFirst));
				if (moveCount == 4)
					return;
				foreach (var node in nextNodes)
					if(node != Node.outSide)
						ShowMovable(node, moveCount, false);
				break;
			case MoveType.Knight when moveCount == 0:
				nextNodes = MoveCross(choosedNode,ref moveCount, isFirst ,false);
				foreach (var node in nextNodes)
					if (node != Node.outSide)
						ShowMovable(node, moveCount, false);
				break;
			case MoveType.Knight when moveCount == 1:
				MoveAxis(choosedNode,ref moveCount, isFirst, false);
				break;
			case MoveType.Bishop:
				nextNodes = MoveCross(choosedNode, ref moveCount, isFirst);
				foreach (var node in nextNodes)
					if (node != Node.outSide)
						ShowMovable(node, moveCount, false);
				break;
			case MoveType.Rook:
				nextNodes = MoveAxis(choosedNode, ref moveCount, isFirst);
				foreach (var node in nextNodes)
					if (node != Node.outSide)
						ShowMovable(node, moveCount, false);
				break;
			case MoveType.Queen:
				currentMoveType = MoveType.Bishop;
				ShowMovable(choosedNode, moveCount);
				currentMoveType = MoveType.Rook;
				ShowMovable(choosedNode, moveCount);
				currentMoveType = MoveType.Queen;
				break;
			default:
				break;
		}
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
			if(node.IsCenterNode && !isAxis)
				nextNodes.AddRange(node.CenterCrossNode2);

			for (int i = 0; i < nextNodes.Count; i++)
			{
				if (nextNodes[i] != -1)
					allNodes[nextNodes[i]].CommingNode = nodeIndex;
			}
		}
		else
		{
			if (currentMoveType == MoveType.Knight)
			{
				nextNodes.AddRange(axis1);
				nextNodes.AddRange(axis2);
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
							allNodes[i].State = ChoosedNode.State == NodeState.Enemy ? NodeState.EnemyMove : NodeState.PlayerMove;
					}
				}
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
			allNodes[index].CommingNode = currIndex;
		return new List<int> { index };
	}

	private List<int> MoveAxis(int nodeIndex, ref int moveCount, bool isFirst, bool isDraw = true)
	{
		if (CheckAttackPos(nodeIndex, isFirst))
			return new List<int>();

		if (isDraw && !isFirst)
			allNodes[nodeIndex].State = ChoosedNode.State == NodeState.Enemy ? NodeState.EnemyMove : NodeState.PlayerMove;

		if(ChoosedNode.Toy.MoveType == MoveType.King && !isFirst)
			return new List<int>();

		moveCount++;

		return Move(nodeIndex, isFirst, true);
	}

	private List<int> MoveCross(int nodeIndex, ref int moveCount, bool isFirst, bool isDraw = true)
	{
		if (CheckAttackPos(nodeIndex, isFirst))
			return new List<int>();

		if (isDraw && !isFirst)
			allNodes[nodeIndex].State = ChoosedNode.State == NodeState.Enemy ? NodeState.EnemyMove : NodeState.PlayerMove;

		if (ChoosedNode.Toy.MoveType == MoveType.King && !isFirst)
			return new List<int>();

		moveCount++;

		return Move(nodeIndex, isFirst, false);
	}

	private bool CheckAttackPos(int index, bool isFirst)
	{
		if (allNodes[index].State != NodeState.None && !isFirst && currentMoveType != MoveType.Knight)
		{
			if ((allNodes[index].State == NodeState.Enemy && ChoosedNode.State == NodeState.Player) ||
				(allNodes[index].State == NodeState.Player && ChoosedNode.State == NodeState.Enemy))
				allNodes[index].State = NodeState.Attack;
			return true;
		}
		return false;
	}
}
