using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayLogic : MonoBehaviour
{
	private Node choosedNode;
	public List<Node> allNodes;

	public static int allNodeCount = 16 * 6;

	private MoveType moveType;

	public MoveType MoveType 
	{
		get { return moveType; }
		set 
		{
			if (moveType != value)
			{
				moveType = value;
				Debug.Log(value);
			}
		}
	}

	private void Awake()
	{
		
	}

	private void Start()
	{
		for (int i = 0; i < allNodes.Count; i++)
		{
			allNodes[i].NodeInit();
		}
	}

	private void Update()
	{
		if (Input.touches.Length == 0)
			return;

		ClearNodes();

		var touchPos = Input.GetTouch(0).position;
		var ray = Camera.main.ScreenPointToRay(touchPos);

		if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerId.node))
		{
			var go = hit.collider.gameObject;
			choosedNode = go.GetComponent<Node>();
			
			ShowMovable(choosedNode.NodeIndex, 0);
			choosedNode.State = NodeState.Player;
			//ShowNeighbor();
		}
	}

	private void ClearNodes()
	{
		foreach (var node in allNodes)
		{
			node.State = NodeState.None;
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
		switch (moveType)
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
				moveType = MoveType.Bishop;
				ShowMovable(choosedNode, moveCount);
				moveType = MoveType.Rook;
				ShowMovable(choosedNode, moveCount);
				moveType = MoveType.Queen;
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
			if (moveType == MoveType.Knight)
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
						allNodes[i].State = NodeState.PlayerMove;
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
		if (isDraw)
			allNodes[nodeIndex].State = NodeState.PlayerMove;

		moveCount++;

		return Move(nodeIndex, isFirst, true);
	}

	private List<int> MoveCross(int nodeIndex, ref int moveCount, bool isFirst, bool isDraw = true)
	{
		if (isDraw)
			allNodes[nodeIndex].State = NodeState.PlayerMove;

		moveCount++;

		return Move(nodeIndex, isFirst, false);
	}
}
