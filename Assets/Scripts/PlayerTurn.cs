using UnityEngine;

public class PlayerTurn : Turn
{
	private Node touchedNode;
	private Node beforeNode;

	protected void Update()
	{
		if (toyControl.IsMove  || playManager.PlayTurn != PlayTurn.Player || !playManager.IsTurnStart)
			return;

		if (moveCount == 0)
		{
			EndTurn();
			return;
		}

		if (Input.touches.Length == 0)
			return;

		var touchPos = Input.GetTouch(0).position;
		var ray = Camera.main.ScreenPointToRay(touchPos);

		if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerId.node))
		{
			var go = hit.collider.gameObject;
			beforeNode = touchedNode;
			touchedNode = go.GetComponent<Node>();
			if (touchedNode.State == NodeState.None)
				playLogic.ClearNodes();
			else if (touchedNode.State == NodeState.Enemy || touchedNode.State == NodeState.Player)
			{
				playLogic.ChoosedNode = touchedNode;
				playLogic.ShowMovable(touchedNode.NodeIndex, 0);
			}
			else if (beforeNode != null && beforeNode.State == NodeState.Player &&
				(touchedNode.State == NodeState.PlayerMove || touchedNode.State == NodeState.Attack))
			{
				playLogic.ChoosedNode = touchedNode;
				PlayAction();
			}
		}
		else if (Physics.Raycast(ray, Mathf.Infinity, LayerId.ground))
		{
			boardManager.IsChoosed = false;
			playLogic.ChoosedNode = null;
		}
	}

	public override void StartTurn()
	{
		base.StartTurn();
	}

	private void PlayAction()
	{
		var isAlive = false;
		if (touchedNode.State == NodeState.Enemy)
			isAlive = touchedNode.Toy.GetDamageAndAlive(beforeNode.Toy.Attack);

		playLogic.ClearNodes();

		toyControl.ToyMove(ref beforeNode, isAlive);
		if(!isAlive)
		{
			touchedNode.State = beforeNode.State;
			beforeNode.State = NodeState.None;
			touchedNode = null;
		}

		moveCount--;
	}

	public override void EndTurn()
	{
		boardManager.IsChoosed = false;
		playLogic.ChoosedNode = null;

		base.EndTurn();
	}
}
