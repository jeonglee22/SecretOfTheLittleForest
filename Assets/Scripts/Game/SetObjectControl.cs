using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class SetObjectControl : MonoBehaviour
{
    public PlayLogic playLogic;
	public GameObject toy;
    private List<Node> playerStartNodes;
    public BoardManager boardManager;
    private Node beforeNode;

	private float touchStartTime;
	private float touchingTime;
	private float holdingTime = 0.5f;
	private bool isMoving;

	private GameObject dragObject;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        playerStartNodes = boardManager.playerStartNodes;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
		if (dragObject != null && dragObject.GetComponent<DragObject>().IsDrag)
		{
			dragObject.GetComponent<Image>().enabled = false;
		}
		else if (dragObject != null && dragObject.GetComponent<DragObject>().IsFinishDrag)
		{
			var node = dragObject.GetComponent<DragObject>().FinishNode;
			if (node == null)
			{
				beforeNode.GetComponentInChildren<Toy>(true).gameObject.SetActive(true);
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
	}

	private void ChangePos(UnityEngine.Touch touch)
	{
		var touchRay = Camera.main.ScreenPointToRay(touch.position);

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
			boardManager.ToySettingOnNode(node, toy.GetComponent<Toy>(), false);
			node.State = NodeState.Player;
			node.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

			Destroy(dragObject.transform.parent.gameObject);
			Destroy(beforeToy.gameObject);
			beforeNode.Toy = null;
			beforeNode = null;
			playLogic.ClearNodes();
			isMoving = false;
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
