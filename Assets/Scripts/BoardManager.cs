using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public PlayManager playManager;
	public UIManager uiManager;

	public List<Node> allNodes;
	public PlayLogic playLogic;

	public static int allNodeCount = 16 * 6;

	public List<Node> playerStartNodes;
	public List<Node> enemyStartNodes;
	public List<Node> eliteStartNodes;
	public List<Node> bossStartNodes;

	private int eliteEnemy2GroupFirst;

	private BattleType battleType;
	public BattleType BattleType { get { return battleType; } }

	public bool IsChoosed { get; set; }
	private float stageId;

	private void OnEnable()
	{
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;
		battleType = data.BattleType;
		stageId = data.stageId;
	}

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
		var enemyIds = GetStageDataIds((int)stageId);

		eliteEnemy2GroupFirst = 0;
		var result = new List<(Node node, ToyData data)>();
		for (int i = 0; i < enemyIds.Count; i++)
		{
			if (enemyIds[i] == 0)
				continue;
			if(battleType == BattleType.Elite && i < 16)
				eliteEnemy2GroupFirst++;

			GameObjectManager.ToyResource.Load(DataTableManger.ToyTable.Get(enemyIds[i]).ModelCode.ToString());
			if (battleType == BattleType.Normal)
				result.Add((enemyStartNodes[i], DataTableManger.ToyTable.Get(enemyIds[i])));
			else if(battleType == BattleType.Elite)
				result.Add((eliteStartNodes[i], DataTableManger.ToyTable.Get(enemyIds[i])));
			else if(battleType == BattleType.Boss)
				result.Add((bossStartNodes[i], DataTableManger.ToyTable.Get(enemyIds[i])));
		}

		//uiManager.SetStageStat(stageData.ID.ToString());

		return result;
	}

	private List<int> GetStageDataIds(int stageId)
	{
		var result = new List<int>();

		if (battleType == BattleType.Normal)
		{
			var stageData = new StageData();
			do
			{
				stageData = DataTableManger.StageTable.GetRandom();
			} while (stageData.Stage != stageId);
			result = stageData.Pos.ToList();
		}
		else if (battleType == BattleType.Elite)
		{
			var stageData = new EliteStageData();
			do
			{
				stageData = DataTableManger.EliteStageTable.GetRandom();
			} while (stageData.Stage != stageId);
			result = stageData.Pos.ToList();
		}
		else
		{
			result = DataTableManger.StageTable.GetBoss(-stageId).Pos.ToList();
		}

		return result;
	}

	public Toy ToySettingOnNode(Node node, Toy toy, bool isEnemy, int anotherGroup = -1)
	{
		var nodeScale = node.transform.localScale;

		var childTransform = node.gameObject.transform.GetChild(0);
		var spawnedToy = Instantiate(toy, childTransform);
		spawnedToy.Data = toy.Data;
		spawnedToy.Init();

		var scale = spawnedToy.transform.localScale;
		spawnedToy.transform.localScale = new Vector3(scale.x / nodeScale.x, scale.y / nodeScale.y, scale.z / nodeScale.z);
		node.Toy = spawnedToy;
		node.Toy.IsEnemy = isEnemy ? true : false;
		if(battleType == BattleType.Elite)
			node.Toy.IsElite = eliteEnemy2GroupFirst <= anotherGroup ? true : false;
		else
			node.Toy.IsElite = false;
		node.State = isEnemy ? NodeState.Enemy : NodeState.Player;

		return spawnedToy;
	}
}
