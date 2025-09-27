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
    private Node beforeNode;
	private ToggleGroup imageToggles;

	private PointerEventData eventData;

	private float touchStartTime;
	private float touchingTime;
	private float holdingTime = 0.5f;
	private bool isMoving;
	private bool isAddingFromDeck;

	public bool IsMoving { get { return isMoving; } }

	private GameObject dragObject;
	public GameObject DragObject { get { return dragObject; } }

	void Start()
    {
        playerStartNodes = boardManager.playerStartNodes;
		imageToggles = scrollRect.gameObject.GetComponent<ScrollRect>().content.gameObject.GetComponent<ToggleGroup>();
	}

    void Update()
    {
        if (Input.touchCount == 0)
            return;

		var touch = Input.GetTouch(0);
		DragMoving(touch);

		if (touch.phase == TouchPhase.Stationary && beforeNode != null && !isMoving)
		{
			touchingTime = Time.time - touchStartTime;
			if(touchingTime >= holdingTime)
			{
				isMoving = true;
				playLogic.ClearNodes();
				ShownMovableNodes();
				beforeNode.State = NodeState.ReadyMove;
				MakeDragImage();
				if(beforeNode.GetComponentInChildren<Toy>() !=null)
					beforeNode.GetComponentInChildren<Toy>().gameObject.SetActive(false);
			}
		}
		else if (touch.phase == TouchPhase.Began)
		{
			if(isMoving)
			{
				ChangePos(touch);
				return;
			}
			beforeNode = null;
			isMoving = false;
			touchStartTime = Time.time;
			TouchBegin(touch);
		}
		else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && eventData != null)
		{
			if(CheckSameNode(touch))
			{
				return;
			}
			eventData.position = Input.mousePosition;
			eventData.pointerId = -1;
			ExecuteEvents.Execute(dragObject, eventData, ExecuteEvents.endDragHandler);
			DragMoving(touch);
		}
	}

	private bool CheckSameNode(Touch touch)
	{
		if (beforeNode == null)
			return false;

		var ray = Camera.main.ScreenPointToRay(touch.position);
		if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, LayerId.node))
		{
			var hitNode = hitInfo.collider.gameObject.GetComponent<Node>();

			if (hitNode.NodeIndex == beforeNode.NodeIndex)
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
		if (dragObject != null && beforeNode != null && dragObject.GetComponent<DragObject>().IsFinishDrag)
		{
			var node = dragObject.GetComponent<DragObject>().FinishNode;
			if (node == null)
			{
				if(RectTransformUtility.RectangleContainsScreenPoint(scrollRect, touch.position, null))
				{
					unitSetting.AddData(beforeNode.Toy.Data);
					var beforeToy = beforeNode.GetComponentInChildren<Toy>(true);
					beforeNode.Toy = null;
					Destroy(beforeToy.gameObject);
				}
				else
				{
					beforeNode.GetComponentInChildren<Toy>(true).gameObject.SetActive(true);
				}
				playLogic.ClearNodes();
				beforeNode = null;
			}
			else
			{
				node.GetComponentInChildren<Toy>(true).gameObject.SetActive(true);
				if(beforeNode.GetComponentInChildren<Toy>(true)  != null)
					Destroy(beforeNode.GetComponentInChildren<Toy>(true).gameObject);
			}
			Destroy(dragObject.transform.parent.gameObject);
			dragObject = null;
			isMoving = false;
			touchStartTime = Time.time;
		}
	}

	private void ChangePos(UnityEngine.Touch touch)
	{
		if (beforeNode == null)
			return;

		var touchRay = Camera.main.ScreenPointToRay(touch.position);
		var rectSize = scrollRect.rect.size;
		if (Physics.Raycast(touchRay, out var hitInfo, float.MaxValue, LayerId.node))
		{
			var go = hitInfo.collider.gameObject;
			var node = go.GetComponent<Node>();

			if (node.State == NodeState.Player)
			{
				return;
			}

			var boardManager = GameObject.FindWithTag(Tags.BoardManager).GetComponent<BoardManager>();

			var beforeToy = beforeNode.GetComponentInChildren<Toy>(true);

			beforeNode.Toy = null;
			beforeNode = null;
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
			unitSetting.AddData(beforeNode.Toy.Data);

			var beforeToy = beforeNode.GetComponentInChildren<Toy>(true);
			beforeNode.Toy = null;
			beforeNode = null;

			if (dragObject != null)
				Destroy(dragObject.transform.parent.gameObject);
			dragObject = null;
			Destroy(beforeToy.gameObject);
			playLogic.ClearNodes();
			isMoving = false;
			touchStartTime = Time.time;
		}
	}

	private void MakeDragImage()
	{
		var go = new GameObject();
		go.AddComponent<Toy>();
		var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, beforeNode.gameObject.transform.position);

		var baseGo = Instantiate(go, transform.root.GetComponentInChildren<Canvas>().rootCanvas.transform);
		baseGo.transform.position = screenPos;
		var toy = baseGo.GetComponent<Toy>();
		toy.Data = beforeNode.Toy.Data;
		toy.SetData();

		Destroy(go);

		var child = new GameObject();
		var drag = child.AddComponent<DragObject>();
		drag.playerStartNodes = playerStartNodes;
		drag.spawnObj = this.toy;
		drag.dragSucessFunc = (data) => Destroy(beforeNode.Toy.gameObject);
		drag.objectControl = this;
		var image = child.AddComponent<Image>();
		image.sprite = beforeNode.Toy.Toy2D;

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

			if (node.Toy == null)
			{
				playLogic.ClearNodes();
				beforeNode = null;
				return;
			}

			if(beforeNode != null && beforeNode.NodeIndex == node.NodeIndex)
			{
				return;
			}

			playLogic.ClearNodes();
			playLogic.ChoosedNode = node;
			playLogic.ShowMovable(node.NodeIndex, 0);

			ShownMovableNodes();
			beforeNode = node;
		}
		else if (Physics.Raycast(touchRay, Mathf.Infinity, LayerId.ground))
		{
			playLogic.ChoosedNode = null;
			beforeNode = null;
		}
	}
}
