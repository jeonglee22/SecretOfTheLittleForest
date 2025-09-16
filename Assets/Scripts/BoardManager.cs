using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public PlayManager playManager;
	public GameObjectManager gameObjectManager;

	public List<Node> allNodes;
	public PlayLogic playLogic;

	public static int allNodeCount = 16 * 6;

	public List<Node> playerStartNodes;
	public List<Node> enemyStartNodes;

	public bool IsChoosed { get; set; }

	public Node GetRandomNodeInPlayer()
	{
		if(playManager.GetAlivePlayerCount() == playerStartNodes.Count)
			return null;

		var index = 0;
		do
		{
			index = UnityEngine.Random.Range(0, playerStartNodes.Count);
		} while (playerStartNodes[index].State == NodeState.Player);

		playManager.AddPlayers(playerStartNodes[index]);
		return playerStartNodes[index];
	}
	public Node GetRandomNodeInEnemy()
	{
		if (playManager.GetAliveEnemyCount() == enemyStartNodes.Count)
			return null;

		var index = 0;
		do
		{
			index = UnityEngine.Random.Range(0, enemyStartNodes.Count);
		} while (enemyStartNodes[index].State == NodeState.Enemy);

		playManager.AddEnemies(enemyStartNodes[index]);
		return enemyStartNodes[index];
	}

	public List<(Node node, ToyData data)> SetEnemyStageData()
	{
		var stageData = new StageData();
		do
		{
			stageData = DataTableManger.StageTable.GetRandom();
		} while (stageData.Stage != 1);

		var enemyIds = stageData.Pos;

		var result = new List<(Node node, ToyData data)>();
		for (int i = 0; i < enemyIds.Length; i++)
		{
			result.Add((enemyStartNodes[i], DataTableManger.ToyTable.Get(enemyIds[i])));
		}

		return result;
	}

	public Toy ToySettingOnNode(Node node, Toy toy, bool isEnemy)
	{
		var nodeScale = node.transform.localScale;

		var childTransform = node.gameObject.transform.GetChild(0);
		var spawnedToy = Instantiate(toy, childTransform);
		var modelCode = spawnedToy.Data.ModelCode;
		var model = gameObjectManager.Get(modelCode);
		Instantiate(model, spawnedToy.transform);

		var scale = spawnedToy.transform.localScale;
		spawnedToy.transform.localScale = new Vector3(scale.x / nodeScale.x, scale.y / nodeScale.y, scale.z / nodeScale.z);
		node.Toy = spawnedToy;
		node.State = isEnemy ? NodeState.Enemy : NodeState.Player;
		node.Toy.IsEnemy = isEnemy ? true : false;

		return spawnedToy;
	}
}
