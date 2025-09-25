using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	private Deck playerDeck;
	public Deck PlayerDeck { get { return playerDeck; } set { playerDeck = value; } }
	public Toy toy;

	private void OnEnable()
	{
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;
		battleType = data.BattleType;
		stageId = data.stageId;
		playerDeck = data.Deck;
		playerDeck.Pos = data.Deck.Pos;
		playerDeck.Toys = data.Deck.Toys;
	}

	private void Start()
	{
		SetPlayerDeckOnNode();
		if(SceneManager.GetActiveScene().buildIndex == (int)Scenes.NodeSetting)
			SetBoardColor(false);
	}

	private void OnDisable()
	{
		
	}

	public void SaveDeckSetting()
	{
		var posList = new List<int>();
		for (int i = 0; i < playerStartNodes.Count; i++)
		{
			if (playerStartNodes[i].Toy == null)
				posList.Add(0);
			else
				posList.Add(playerStartNodes[i].Toy.Data.UnitID);
		}
		playerDeck.Pos = posList;
		SaveLoadManager.Data.Deck = playerDeck;
		SaveLoadManager.Data.Deck.Pos = posList;
		SaveLoadManager.Save();
	}

	public void SetPlayerDeckOnNode()
	{
		var posList = playerDeck.Pos;
		var toys = playerDeck.Toys;

		for (int i = 0; i < posList.Count; i++)
		{
			var id = posList[i];
			if (id == 0)
				continue;

			toy.Data = DataTableManger.ToyTable.Get(posList[i]);
			ToySettingOnNode(playerStartNodes[i], toy, false);
		}
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

	public void SetBoardColor(bool isElite)
	{
		eliteStartNodes.ForEach(n => n.State = NodeState.None);
		enemyStartNodes.ForEach(n => n.State = NodeState.None);

		List<Node> nodes = isElite ? eliteStartNodes : enemyStartNodes;
		nodes.ForEach(n => n.State = NodeState.Enemy);
	}
}
