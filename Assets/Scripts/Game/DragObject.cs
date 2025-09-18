using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private GameObject drag;
	public List<Node> playerStartNodes;
	public GameObject spawnObj;
	public Action<ToyData> dragSucessFunc;

	public void OnBeginDrag(PointerEventData eventData)
	{
		int count = 0;
		foreach (var node in playerStartNodes)
		{
			if (node.State != NodeState.Player)
				node.State = NodeState.Choose;
			else
				count++;
		}

		if (count == DataTableManger.SettingTable.Get(Settings.unitCount))
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

		drag = Instantiate(newobj, transform.root);
		drag.GetComponent<Toy>().Data = toy.Data;
		drag.GetComponent<Toy>().SetData();

		SetDragPosition(eventData);
		Destroy(newobj);
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
				return;
			}

			var boardManager = GameObject.FindWithTag(Tags.BoardManager).GetComponent<BoardManager>();

			var toy = drag.GetComponent<Toy>();
			var toy3D = spawnObj.GetComponent<Toy>();
			toy3D.Data = toy.Data;

			boardManager.ToySettingOnNode(droppedNode, toy3D, false);
			droppedNode.State = NodeState.Player;

			if (dragSucessFunc != null)
			{
				dragSucessFunc(toy.Data);
			}
		}

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
