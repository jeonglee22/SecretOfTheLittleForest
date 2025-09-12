using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public List<Node> allNodes;
	public PlayLogic playLogic;

	public static int allNodeCount = 16 * 6;

	public List<Node> playerStartNodes;
	public List<Node> enemyStartNodes;

	public bool IsChoosed { get; set; }

	public void ReloadBoard()
	{

	}

	public Node GetRandomNodeInPlayer()
	{
		var index = 0;
		do
		{
			index = Random.Range(0, playerStartNodes.Count);
		} while (allNodes[index].State == NodeState.Player);
		
		return playerStartNodes[index];
	}
	public Node GetRandomNodeInEnemy()
	{
		var index = 0;
		do
		{
			index = Random.Range(0, enemyStartNodes.Count);
		} while (allNodes[index].State == NodeState.Enemy);
		return enemyStartNodes[index];
	}

	public Toy ToySettingOnNode(Node node, Toy toy, bool isEnemy)
	{
		var nodeScale = node.transform.localScale;

		var childTransform = node.gameObject.transform.GetChild(0);
		var spawnedToy = Instantiate(toy, childTransform);

		var scale = spawnedToy.transform.localScale;
		spawnedToy.transform.localScale = new Vector3(scale.x / nodeScale.x, scale.y / nodeScale.y, scale.z / nodeScale.z);
		node.Toy = spawnedToy;
		node.State = isEnemy ? NodeState.Enemy : NodeState.Player;
		node.Toy.IsEnemy = isEnemy ? true : false;

		return spawnedToy;
	}
}
