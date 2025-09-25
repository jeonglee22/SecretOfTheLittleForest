using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public GameObject drag;
	public SetObjectControl objectControl;
	public List<Node> playerStartNodes;
	public GameObject spawnObj;
	public Action<ToyData> dragSucessFunc;
	public bool IsDrag { get; private set; } = false;
	public bool IsFinishDrag { get; private set; } = false;
	public Node FinishNode { get; private set; }
	private float unitCount;

	private void Start()
	{
		objectControl = transform.root.GetComponentInChildren<SetObjectControl>();
		unitCount = SaveLoadManager.Data.unitCount;
	}

	private void Update()
	{
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (objectControl.IsMoving && eventData.pointerDrag != objectControl.DragObject)
			return;
		int count = 0;
		foreach (var node in playerStartNodes)
		{
			if (node.State != NodeState.Player)
				node.State = NodeState.Choose;
			else
				count++;
		}

		if (count == unitCount)
		{
			ResetDrag();
			return;
		}
		var newobj = new GameObject();
		var image = newobj.AddComponent<Image>();
		var toy = newobj.AddComponent<Toy>();
		toy.Data = transform.parent.GetComponent<Toy>().Data;
		toy.SetData();
		image.sprite = toy.Toy2D;

		var dragObj = Instantiate(newobj, transform.root.GetComponentInChildren<Canvas>().rootCanvas.transform);
		dragObj.GetComponent<Toy>().Data = toy.Data;
		dragObj.GetComponent<Toy>().SetData();
		if (eventData.pointerId == -1)
			Destroy(dragObj);
		else
		{
			drag = dragObj;
			SetDragPosition(eventData);
		}

		Destroy(newobj);
		IsDrag = true;
		IsFinishDrag = false;
	}

	private void SetDragPosition(PointerEventData eventData)
	{
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(eventData.pointerDrag.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out globalMousePos))
		{
			drag.transform.position = globalMousePos;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (drag != null)
		{
			SetDragPosition(eventData);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		IsDrag = false;
		if (drag == null)
			return;
		
		var endPos = eventData.position;
		var ray = Camera.main.ScreenPointToRay(endPos);

		if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerId.node))
		{
			var go = hit.collider.gameObject;
			var droppedNode = go.GetComponent<Node>();
			if (droppedNode.State != NodeState.Choose)
			{
				ResetDrag();
				FinishNode = null;
				IsFinishDrag = true;
				return;
			}

			var boardManager = GameObject.FindWithTag(Tags.BoardManager).GetComponent<BoardManager>();

			Toy toy;
			if (eventData.pointerId == -1)
			{
				toy = drag.transform.parent.GetComponent<Toy>();
			}else
				toy = drag.GetComponent<Toy>();
			var toy3D = spawnObj.GetComponent<Toy>();
			toy3D.Data = toy.Data;

			boardManager.ToySettingOnNode(droppedNode, toy3D, false);
			droppedNode.State = NodeState.Player;

			if (dragSucessFunc != null)
			{
				dragSucessFunc(toy.Data);
			}
			FinishNode = droppedNode;
		}
		else if(Physics.Raycast(ray, out RaycastHit hit2, Mathf.Infinity, LayerId.ui))
		{

		}

		IsFinishDrag = true;
		ResetDrag();
	}

	private void ResetDrag()
	{
		if (drag != null)
			Destroy(drag);
		drag = null;

		foreach (var node in playerStartNodes)
		{
			if (node.State != NodeState.Player)
				node.State = NodeState.None;
		}
	}
}
