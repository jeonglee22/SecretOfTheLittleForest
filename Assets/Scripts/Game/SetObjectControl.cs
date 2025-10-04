using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetObjectControl : MonoBehaviour
{
    public PlayLogic playLogic;
	public UnitSetting unitSetting;
	public GameObject toy;
	public RectTransform scrollRect;
    private List<Node> playerStartNodes;
    public BoardManager boardManager;
    private Node choosingNode;
    private Node beforeNode;
	private ToggleGroup imageToggles;

	private PointerEventData eventData;

	private float touchStartTime;
	private float touchingTime;
	private float holdingTime = 0.5f;
	private bool isMoving;

	public bool IsMoving { get { return isMoving; } }

	private GameObject dragObject;
	public GameObject DragObject { get { return dragObject; } }

	void Start()
    {
        playerStartNodes = boardManager.playerStartNodes;
		imageToggles = scrollRect.gameObject.GetComponent<ScrollRect>().content.gameObject.GetComponent<ToggleGroup>();

		boardManager.SetBoardColor(false);
	}

    void Update()
    {
		if (Input.touchCount == 0)
            return;

		var touch = Input.GetTouch(0);
		DragMoving(touch);

		if (touch.phase == TouchPhase.Stationary && choosingNode != null && !isMoving)
		{
			touchingTime = Time.time - touchStartTime;
			if(touchingTime >= holdingTime)
			{
				isMoving = true;
				playLogic.ClearNodes();
				ShownMovableNodes();
				choosingNode.State = NodeState.ReadyMove;
				MakeDragImage();
				if(choosingNode.GetComponentInChildren<Toy>() !=null)
					choosingNode.GetComponentInChildren<Toy>().gameObject.SetActive(false);
			}
		}
		else if (touch.phase == TouchPhase.Began)
		{
			if(isMoving)
			{
				ChangePos(touch);
				boardManager.SetBoardColor(boardManager.IsEliteBoard);
				return;
			}
			choosingNode = null;
			isMoving = false;
			touchStartTime = Time.time;
			TouchBegin(touch);
		}
		else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
		{
			beforeNode = choosingNode;

			if (CheckSameNode(touch))
			{
				boardManager.SetBoardColor(boardManager.IsEliteBoard);
				return;
			}

			if(eventData != null)
			{ 
				eventData.position = Input.mousePosition;
				eventData.pointerId = -1;
				ExecuteEvents.Execute(dragObject, eventData, ExecuteEvents.endDragHandler);
				DragMoving(touch);
			}
		}
		boardManager.SetBoardColor(boardManager.IsEliteBoard);
	}

	private bool CheckSameNode(Touch touch)
	{
		if (choosingNode == null)
			return false;

		var ray = Camera.main.ScreenPointToRay(touch.position);
		if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, LayerId.node))
		{
			var hitNode = hitInfo.collider.gameObject.GetComponent<Node>();

			if (hitNode.NodeIndex == choosingNode.NodeIndex)
			{
				return true;
			}
		}

		return false;
	}

	private void DragMoving(Touch touch)
	{
		if(dragObject != null && dragObject.GetComponent<DragObject>().IsDrag)
		{
			eventData.position = Input.mousePosition;
			ExecuteEvents.Execute(dragObject, eventData, ExecuteEvents.dragHandler);
		}
		if (dragObject != null && choosingNode != null && dragObject.GetComponent<DragObject>().IsFinishDrag)
		{
			var node = dragObject.GetComponent<DragObject>().FinishNode;
			if (node == null)
			{
				if(RectTransformUtility.RectangleContainsScreenPoint(scrollRect, touch.position, null))
				{
					unitSetting.AddData(choosingNode.Toy.Data);
					var beforeToy = choosingNode.GetComponentInChildren<Toy>(true);
					if (choosingNode.Toy.IsKing)
					{
						boardManager.PlayerDeck.KingPos = -1;
					}
					choosingNode.Toy = null;
					
					Destroy(beforeToy.gameObject);
				}
				else
				{
					choosingNode.GetComponentInChildren<Toy>(true).gameObject.SetActive(true);
					if(boardManager.PlayerDeck.KingPos != -1 && choosingNode.NodeIndex == boardManager.playerStartNodes[boardManager.PlayerDeck.KingPos].NodeIndex)
					{
						choosingNode.Toy.kingCanvas.SetActive(true);
					}
				}
				playLogic.ClearNodes();
				
			}
			else
			{
				node.GetComponentInChildren<Toy>(true).gameObject.SetActive(true);
				if(choosingNode.GetComponentInChildren<Toy>(true) != null)
					Destroy(choosingNode.GetComponentInChildren<Toy>(true).gameObject);
			}
			beforeNode = null;
			choosingNode = null;
			Destroy(dragObject.transform.parent.gameObject);
			dragObject = null;
			isMoving = false;
			touchStartTime = Time.time;
		}
	}

	private void ChangePos(UnityEngine.Touch touch)
	{
		if (choosingNode == null)
			return;

		var touchRay = Camera.main.ScreenPointToRay(touch.position);
		var rectSize = scrollRect.rect.size;
		if (Physics.Raycast(touchRay, out var hitInfo, float.MaxValue, LayerId.node))
		{
			var go = hitInfo.collider.gameObject;
			var node = go.GetComponent<Node>();

			if (node.State == NodeState.Player || !playerStartNodes.Contains(node))
			{
				playLogic.ChoosedNode = null;
				if (beforeNode != null && (boardManager.PlayerDeck.KingPos == -1 || beforeNode.NodeIndex != boardManager.playerStartNodes[boardManager.PlayerDeck.KingPos].NodeIndex))
				{
					beforeNode.Toy.kingCanvas.SetActive(false);
				}
				else if (beforeNode != null && beforeNode.NodeIndex == boardManager.playerStartNodes[boardManager.PlayerDeck.KingPos].NodeIndex)
				{
					beforeNode.Toy.kingCanvas.SetActive(true);
				}
				
				choosingNode.GetComponentInChildren<Toy>(true).gameObject.SetActive(true);
				choosingNode = null;
				beforeNode = null;
				if (dragObject != null)
					Destroy(dragObject.transform.parent.gameObject);
				dragObject = null;
				playLogic.ClearNodes();
				isMoving = false;
				return;
			}

			var beforeToy = choosingNode.GetComponentInChildren<Toy>(true);

			choosingNode.Toy = null;
			choosingNode = null;
			beforeNode = null;
			var toyComp = toy.GetComponent<Toy>();
			toyComp.Data = beforeToy.Data;
			toyComp.IsKing = beforeToy.IsKing;
			if(toyComp.IsKing)
			{
				boardManager.PlayerDeck.KingId = toyComp.Data.UnitID;
				boardManager.PlayerDeck.KingPos = boardManager.playerStartNodes.IndexOf(node);
			}
			toy.GetComponent<Toy>().Data = beforeToy.Data;

			var newToy = boardManager.ToySettingOnNode(node, toy.GetComponent<Toy>(), false);

			if(dragObject != null)
				Destroy(dragObject.transform.parent.gameObject);
			dragObject = null;
			Destroy(beforeToy.gameObject);
			playLogic.ClearNodes();
			isMoving = false;
		}
		else if(RectTransformUtility.RectangleContainsScreenPoint(scrollRect, touch.position, null))
		{
			unitSetting.AddData(choosingNode.Toy.Data);

			var beforeToy = choosingNode.GetComponentInChildren<Toy>(true);
			boardManager.PlayerDeck.KingPos = -1;
			choosingNode.Toy = null;
			beforeNode = null;
			choosingNode = null;

			if (dragObject != null)
				Destroy(dragObject.transform.parent.gameObject);
			dragObject = null;
			Destroy(beforeToy.gameObject);
			playLogic.ClearNodes();
			isMoving = false;
			touchStartTime = Time.time;
		}
	}

	private void ChangeKing(Node node)
	{
		node.Toy.IsKing = true;
		choosingNode.Toy.IsKing = false;

		var nodeIndex = boardManager.playerStartNodes.IndexOf(node);
		boardManager.PlayerDeck.KingPos = nodeIndex;
		boardManager.PlayerDeck.KingId = node.Toy.Data.UnitID;
	}

	private void MakeDragImage()
	{
		var go = new GameObject();
		go.AddComponent<Toy>();
		var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, choosingNode.gameObject.transform.position);

		var baseGo = Instantiate(go, transform.root.GetComponentInChildren<Canvas>().rootCanvas.transform);
		baseGo.transform.position = screenPos;
		var toy = baseGo.GetComponent<Toy>();
		choosingNode.Toy.kingCanvas.SetActive(false);
		toy.Data = choosingNode.Toy.Data;
		toy.IsKing = choosingNode.Toy.IsKing;
		toy.SetData();

		Destroy(go);

		var child = new GameObject();
		var drag = child.AddComponent<DragObject>();
		drag.playerStartNodes = playerStartNodes;
		drag.spawnObj = this.toy;
		drag.dragSucessFunc = (data) => Destroy(choosingNode.Toy.gameObject);
		drag.objectControl = this;
		var image = child.AddComponent<Image>();
		image.sprite = choosingNode.Toy.Toy2D;

		dragObject = Instantiate(child, baseGo.transform);
		eventData = new PointerEventData(EventSystem.current)
		{
			pointerEnter = dragObject,
			pointerDrag = dragObject,
			position = Input.mousePosition,
			pointerId = -1,
		};
		dragObject.GetComponent<DragObject>().drag = dragObject;
		ExecuteEvents.Execute(dragObject, eventData, ExecuteEvents.beginDragHandler);
		
		Destroy(child);
	}

	public void ShownMovableNodes()
    {
        foreach (var node in playerStartNodes)
        {
            if (node.State != NodeState.None)
                continue;

            node.State = NodeState.Choose;
        }
    }

    private void TouchBegin(Touch touch)
    {
		var touchRay = Camera.main.ScreenPointToRay(touch.position);

		if (Physics.Raycast(touchRay, out var hitInfo, float.MaxValue, LayerId.node))
		{
			var go = hitInfo.collider.gameObject;
			var node = go.GetComponent<Node>();

			if (node.Toy == null || node.Toy.IsEnemy)
			{
				playLogic.ChoosedNode = null;
				if (beforeNode != null && (boardManager.PlayerDeck.KingPos == -1 || beforeNode.NodeIndex != boardManager.playerStartNodes[boardManager.PlayerDeck.KingPos].NodeIndex))
				{
					beforeNode.Toy.kingCanvas.SetActive(false);
				}
				choosingNode = null;
				beforeNode = null;
				return;
			}

			if(beforeNode == null && !node.Toy.IsKing)
			{
				node.Toy.kingCanvas.SetActive(true);
			}
			else if(beforeNode != null && beforeNode.NodeIndex == node.NodeIndex && !node.Toy.IsKing)
			{
				node.Toy.IsKing = true;
				if(boardManager.PlayerDeck.KingPos != -1 && playerStartNodes[boardManager.PlayerDeck.KingPos].Toy != null)
				{
					playerStartNodes[boardManager.PlayerDeck.KingPos].Toy.IsKing = false;
					playerStartNodes[boardManager.PlayerDeck.KingPos].Toy.kingCanvas.SetActive(false);
				}
				boardManager.PlayerDeck.KingId = node.Toy.Data.UnitID;
				boardManager.PlayerDeck.KingPos = playerStartNodes.IndexOf(node);
				beforeNode = null;
				return;
			}
			else if(beforeNode != null && beforeNode.NodeIndex == node.NodeIndex)
			{
				return;
			}
			else if(beforeNode != null && beforeNode.NodeIndex != node.NodeIndex)
			{
				if ((boardManager.PlayerDeck.KingPos == -1 || beforeNode.NodeIndex != boardManager.playerStartNodes[boardManager.PlayerDeck.KingPos].NodeIndex) &&
					beforeNode.Toy != null)
				{
					beforeNode.Toy.kingCanvas.SetActive(false);
				}
				playLogic.ClearNodes();
				choosingNode = null;
				beforeNode = null;
				return;
			}

			playLogic.ClearNodes();
			playLogic.ChoosedNode = node;
			playLogic.ShowMovable(node.NodeIndex, 0);

			ShownMovableNodes();
			choosingNode = node;
		}
		else if (Physics.Raycast(touchRay, Mathf.Infinity, LayerId.ground))
		{
			playLogic.ChoosedNode = null;
			if (beforeNode != null && (boardManager.PlayerDeck.KingPos == -1 || beforeNode.NodeIndex != boardManager.playerStartNodes[boardManager.PlayerDeck.KingPos].NodeIndex))
			{
				beforeNode.Toy.kingCanvas.SetActive(false);
			}
			choosingNode = null;
			beforeNode = null;
		}
	}
}
