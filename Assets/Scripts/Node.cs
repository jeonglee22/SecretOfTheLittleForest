using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IDropHandler
{
    private Toy toy;

	public Toy Toy
	{
		get { return toy; }
		set { toy = value; }
	}

	public bool IsCenterNode = false;
	private int nodeIndexAxis;
	private int nodeIndexNumber;
	public int NodeIndex
	{
		get { return nodeIndexAxis * 16 + nodeIndexNumber; }
	}

	public int NodeIndexNumber { get { return nodeIndexNumber; } }
	public int NodeIndexAxis { get { return nodeIndexAxis; } }

	private List<int> axis1;
	private List<int> axis2;

	private List<int> crossNode1;

	private List<int> crossNode2;
	private List<int> centerCrossNode2;

	public List<int> Axis1 { get => axis1; }
	public List<int> Axis2 { get => axis2; }
	public List<int> CrossNode1 { get => crossNode1; }
	public List<int> CrossNode2 { get => crossNode2; }
	public List<int> CenterCrossNode2 { get => centerCrossNode2; }

	public int CommingNode { get; set; }

	public static int outSide = -1;

	private NodeState state = NodeState.None;

	public NodeState State
	{
		get
		{
			return state;
		}
		set
		{
			state = value;
			ChangeColor(state);
		}
	}	

	private Color initColor;

	public void NodeInit()
	{
		if (nodeIndexNumber == 0)
		{
			axis1 = new List<int> { NodeIndex + 1, (NodeIndex + 16 * 5) % PlayLogic.allNodeCount };
			axis2 = new List<int> { NodeIndex + 4, (NodeIndex + 16) % PlayLogic.allNodeCount };

			crossNode1 = new List<int> { (NodeIndex + 16) % PlayLogic.allNodeCount + 4, (NodeIndex + 16 * 5) % PlayLogic.allNodeCount + 1 };
			crossNode2 = new List<int> { NodeIndex + 5, (NodeIndex + 32) % PlayLogic.allNodeCount };
			centerCrossNode2 = new List<int> { NodeIndex + 5, (NodeIndex + 64) % PlayLogic.allNodeCount };
		}
		else if (nodeIndexNumber / 4 == 0)
		{
			axis1 = new List<int> { NodeIndex - 1, nodeIndexNumber % 4 != 3 ? (NodeIndex + 1) : outSide };
			axis2 = new List<int> { NodeIndex + 4, ((nodeIndexAxis + 1) * 16 + nodeIndexNumber * 4) % PlayLogic.allNodeCount };

			crossNode1 = new List<int> { NodeIndex + 3, nodeIndexNumber % 4 != 3 ? ((nodeIndexAxis + 1) * 16 + 4 * nodeIndexNumber) % PlayLogic.allNodeCount + 4 : outSide };
			crossNode2 = new List<int> { nodeIndexNumber % 4 != 3 ? NodeIndex + 5 : outSide, ((nodeIndexAxis + 1) * 16 + 4 * nodeIndexNumber) % PlayLogic.allNodeCount - 4 };
		}
		else if (nodeIndexNumber % 4 == 0)
		{
			axis1 = new List<int> { NodeIndex - 4, nodeIndexNumber / 4 != 3 ? (NodeIndex + 4) : outSide };
			axis2 = new List<int> { NodeIndex + 1, ((nodeIndexAxis + 5) * 16 +  (nodeIndexNumber / 4)) % PlayLogic.allNodeCount };

			crossNode1 = new List<int> { NodeIndex - 3, nodeIndexNumber / 4 != 3 ? ((nodeIndexAxis + 5) * 16 + nodeIndexNumber / 4) % PlayLogic.allNodeCount + 1 : outSide };
			crossNode2 = new List<int> { nodeIndexNumber / 4 != 3 ? NodeIndex + 5 : outSide, ((nodeIndexAxis + 5) * 16 + nodeIndexNumber / 4) % PlayLogic.allNodeCount - 1 };
		}
		else
		{
			axis1 = new List<int> { NodeIndex - 4, nodeIndexNumber / 4 != 3 ? (NodeIndex + 4) : outSide };
			axis2 = new List<int> { NodeIndex - 1, nodeIndexNumber % 4 != 3 ? (NodeIndex + 1) : outSide };

			crossNode1 = new List<int> { NodeIndex - 5, (nodeIndexNumber % 4 != 3 && nodeIndexNumber / 4 != 3) ? NodeIndex + 5 : outSide };
			crossNode2 = new List<int> { nodeIndexNumber % 4 != 3 ? NodeIndex - 3 : outSide, nodeIndexNumber / 4 != 3 ? NodeIndex + 3 : outSide };
		}
	}

	private void Awake()
	{
		nodeIndexAxis = name[0] - 'A';
		nodeIndexNumber = int.Parse(name.Substring(1));

		initColor = gameObject.GetComponent<MeshRenderer>().material.color;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ChangeColor(NodeState state)
	{
		Color color = Color.white;
		color = (state) switch
		{
			NodeState.None => initColor,
			NodeState.Enemy => Color.red,
			NodeState.EnemyMove => new Color(1f, 0.5f, 0f, 1f),
			NodeState.Player => Color.blue,
			NodeState.PlayerMove => Color.cyan,
			NodeState.Attack => Color.yellow,
			NodeState.Choose => Color.yellow,
			NodeState.Moved => Color.gray,
			NodeState.ReadyMove => Color.green,
			_ => initColor,
		};

		gameObject.GetComponent<MeshRenderer>().material.color = color;
	}

	public void OnDrop(PointerEventData eventData)
	{
		Debug.Log("Drop");
		if (State != NodeState.Choose)
			return;
		Debug.Log("Drop");
		var obj = eventData.pointerDrag;
		var toy = obj.GetComponent<Toy>();

		var boardManager = GameObject.FindWithTag(Tags.BoardManager).GetComponent<BoardManager>();
		boardManager.ToySettingOnNode(this, toy, false);
	}
}
