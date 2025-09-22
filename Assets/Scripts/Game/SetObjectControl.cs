using System;
using System.Collections.Generic;
using UnityEngine;
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
			isMoving = false;
			touchStartTime = Time.time;
			TouchBegin(touch);
		}

		//if (!isMoving)
		//	ActiveMovingAtToggleOn();
	}

	//private void ActiveMovingAtToggleOn()
	//{
	//	var toggleElements = imageToggles.ActiveToggles();
	//	foreach (var element in toggleElements)
	//	{
	//		if (!element.isOn)
	//			continue;

	//		isMoving = true;
	//		isAddingFromDeck = true;
	//	}
	//}

	private void DragMoving(Touch touch)
	{
		if (dragObject != null && dragObject.GetComponent<DragObject>().IsDrag)
		{
			dragObject.GetComponent<Image>().enabled = false;
		}
		else if (dragObject != null && dragObject.GetComponent<DragObject>().IsFinishDrag)
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
		var touchRay = Camera.main.ScreenPointToRay(touch.position);
		var rectSize = scrollRect.rect.size;
		if (Physics.Raycast(touchRay, out var hitInfo, float.MaxValue, LayerId.node))
		{
			var go = hitInfo.collider.gameObject;
			var node = go.GetComponent<Node>();

			if (node.State == NodeState.Player || node.NodeIndex == beforeNode.NodeIndex)
			{
				return;
			}

			var boardManager = GameObject.FindWithTag(Tags.BoardManager).GetComponent<BoardManager>();

			var beforeToy = beforeNode.GetComponentInChildren<Toy>(true);
			beforeNode.Toy = null;
			beforeNode = null;
			boardManager.ToySettingOnNode(node, toy.GetComponent<Toy>(), false);

			if(dragObject != null)
				Destroy(dragObject.transform.parent.gameObject);
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
		var image = child.AddComponent<Image>();
		image.sprite = beforeNode.Toy.Toy2D;

		dragObject = Instantiate(child, baseGo.transform);
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

    private void TouchBegin(UnityEngine.Touch touch)
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
