using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
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
	public float StageId { get { return stageId; } }

	private Deck playerDeck;
	public Deck PlayerDeck { get { return playerDeck; } set { playerDeck = value; } }
	public Toy toy;

	public int BoardID { get; set; }
	private Scenes CommingScene { get { return SaveLoadManager.Data.Scenes; } }

    public bool IsEliteBoard { get; set; }
    private bool isSetEnemy = false;

    private void OnEnable()
	{
		SaveLoadManager.Load();
		var data = SaveLoadManager.Data;
		battleType = data.BattleType;
		stageId = data.stageId;
		playerDeck = data.Deck;
		playerDeck.Pos = data.Deck.Pos;
		playerDeck.Toys = data.Deck.Toys;
		playerDeck.KingId = data.Deck.KingId;
		playerDeck.KingPos = data.Deck.KingPos;
		BoardID = data.EnemyFieldID;
	}

	private void Start()
	{
		SetPlayerDeckOnNode();
        if (SceneManager.GetActiveScene().buildIndex == (int)Scenes.NodeSetting)
		{
            SetBoardColor(battleType == BattleType.Elite);
            if (CommingScene == Scenes.StageChoosing)
				return;

            if (battleType == BattleType.Elite)
				SetEliteEnemy();
			else
				SetNormalEnemy();
        }
		else
		{
            SetEnemyToy();
        }
	}

	private void OnDisable()
	{
		//SaveDeckSetting();
	}

	public void SetEliteEnemy()
	{
		var stageData = DataTableManger.EliteStageTable.Get(BoardID);
		var result = new List<int>();

		result = stageData.Pos.ToList();
		result.Add(stageData.Boss_pos1);
		result.Add(stageData.Boss_pos2);

		var nodeTuples = SetEnemyStageData(result);

		for (int i = 0; i < nodeTuples.Count; i++)
		{
			var toy = this.toy;
			toy.Data = nodeTuples[i].data;
			toy.IsKing = nodeTuples[i].isBoss;
			ToySettingOnNode(nodeTuples[i].node, toy, true, i);
		}
	}

	public void SetNormalEnemy()
	{
		var stageData = DataTableManger.StageTable.Get(BoardID);
		var result = new List<int>();

		result = stageData.Pos.ToList();
		result.Add(stageData.Boss_pos);

		var nodeTuples = SetEnemyStageData(result);

		for (int i = 0; i < nodeTuples.Count; i++)
		{
			var toy = this.toy;
			toy.Data = nodeTuples[i].data;
			toy.IsKing = nodeTuples[i].isBoss;
			ToySettingOnNode(nodeTuples[i].node, toy, true, i);
		}
	}

	public void SaveDeckSetting()
	{
		if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex((int)Scenes.NodeSetting))
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
			SaveLoadManager.Data.Deck.KingPos = playerDeck.KingPos;
			SaveLoadManager.Data.Deck.KingId = playerDeck.KingId;
			SaveLoadManager.Save();
		}
	}

	public void SetPlayerDeckOnNode()
	{
		var posList = playerDeck.Pos;

		for (int i = 0; i < posList.Count; i++)
		{
			var id = posList[i];
			if (id == 0)
				continue;

			var toy = this.toy;
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

	public List<(Node node, ToyData data, bool isBoss)> SetEnemyStageData(List<int> enemyIds)
	{
		List<int> bossPos = battleType == BattleType.Elite ?
			new List<int> { enemyIds[32], enemyIds[33] } : new List<int> { enemyIds[16] }; 
		if(battleType == BattleType.Elite)
		{
			enemyIds.RemoveAt(33);
			enemyIds.RemoveAt(32);
		}
		else
		{
			enemyIds.RemoveAt(16);
		}

		eliteEnemy2GroupFirst = 0;
		var result = new List<(Node node, ToyData data, bool isBoss)>();
		for (int i = 0; i < enemyIds.Count; i++)
		{
			if (enemyIds[i] == 0)
				continue;
			if(battleType == BattleType.Elite && i < 16)
				eliteEnemy2GroupFirst++;

			GameObjectManager.ToyResource.Load(DataTableManger.ToyTable.Get(enemyIds[i]).ModelCode.ToString());
			if (battleType == BattleType.Normal)
				result.Add((enemyStartNodes[i], DataTableManger.ToyTable.Get(enemyIds[i]), bossPos.Contains(i)));
			else if(battleType == BattleType.Elite)
				result.Add((eliteStartNodes[i], DataTableManger.ToyTable.Get(enemyIds[i]), bossPos.Contains(i)));
			else if(battleType == BattleType.Boss)
				result.Add((bossStartNodes[i], DataTableManger.ToyTable.Get(enemyIds[i]), bossPos.Contains(i)));
		}

		return result;
	}

	public List<int> GetStageDataIds(int stageId)
	{
		var result = new List<int>();

		if (battleType == BattleType.Normal)
		{
			var stageData = new StageData();
			do
			{
				stageData = DataTableManger.StageTable.GetRandom();
			} while (stageData.Stage != stageId);
			BoardID = stageData.ID;
			result = stageData.Pos.ToList();
			result.Add(stageData.Boss_pos);
		}
		else if (battleType == BattleType.Elite)
		{
			var stageData = new EliteStageData();
			do
			{
				stageData = DataTableManger.EliteStageTable.GetRandom();
			} while (stageData.Stage != stageId);
			BoardID = stageData.ID;
			result = stageData.Pos.ToList();
			result.Add(stageData.Boss_pos1);
			result.Add(stageData.Boss_pos2);
		}
		else
		{
			var stageData = DataTableManger.StageTable.GetBoss(-stageId);
			BoardID = stageData.ID;
			result = stageData.Pos.ToList();
			result.Add(stageData.Boss_pos);
		}

		return result;
	}

	public Toy ToySettingOnNode(Node node, Toy toy, bool isEnemy, int anotherGroup = -1)
	{
		var nodeScale = node.transform.localScale;

		var childTransform = node.gameObject.transform.GetChild(0);
		var spawnedToy = Instantiate(toy, childTransform);
		if (!isEnemy && playerDeck.KingPos != -1 && node.NodeIndex == playerStartNodes[playerDeck.KingPos].NodeIndex)
		{
			spawnedToy.kingCanvas.SetActive(true);
			spawnedToy.IsKing = true;
		}
		else if (isEnemy && toy.IsKing)
		{
			spawnedToy.kingCanvas.SetActive(true);
			spawnedToy.IsKing = true;
		}
		else
		{
			spawnedToy.kingCanvas.SetActive(false);
			spawnedToy.IsKing = false;
		}
			//else if(isEnemy && node.NodeIndex == enemyStartNodes[.KingPos].NodeIndex)

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
		nodes.ForEach(n =>
		{
			if ((BoardID != -1 && n.Toy != null) || BoardID == -1) n.State = NodeState.Enemy;
		});
	}

	public void ResetCaptainImage()
	{
		foreach (var node in playerStartNodes)
		{
			if (node.Toy == null)
				continue;

			if(node.Toy.IsKing)
			{
				node.Toy.kingCanvas.SetActive(true);
			}
			else
			{
				node.Toy.kingCanvas.SetActive(false);
			}
		}
	}

    public void SetEnemyToy()
    {
        if (isSetEnemy)
            return;

        isSetEnemy = true;
        var enemyIds = new List<int>();
        if (BoardID == -1)
            enemyIds = GetStageDataIds((int)StageId);
        else
        {
            if (BattleType == BattleType.Elite)
            {
                var stageData = DataTableManger.EliteStageTable.Get(BoardID);
                enemyIds = stageData.Pos.ToList();
                enemyIds.Add(stageData.Boss_pos1);
                enemyIds.Add(stageData.Boss_pos2);
            }
            else
            {
                var stageData = DataTableManger.StageTable.Get(BoardID);
                enemyIds = new List<int>();
                enemyIds = stageData.Pos.ToList();
                enemyIds.Add(stageData.Boss_pos);
            }
        }
        var nodeTuples = SetEnemyStageData(enemyIds);
        for (int i = 0; i < nodeTuples.Count; i++)
        {
            var toy = this.toy;
            toy.Data = nodeTuples[i].data;
            toy.IsKing = nodeTuples[i].isBoss;
            ToySettingOnNode(nodeTuples[i].node, toy, true, i);
        }
    }
}
