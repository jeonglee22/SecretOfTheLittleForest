using System.Collections.Generic;
using UnityEngine;

public static class DataTableManger
{
    private static readonly Dictionary<string, DataTable> tables =
        new Dictionary<string, DataTable>();

    static DataTableManger()
    {
        Init();
    }

    private static void Init()
    {
        var toyTable = new ToyTable();
        toyTable.Load(DataTableIds.Toy);
        tables.Add(DataTableIds.Toy, toyTable);

		var aiTable = new AITable();
        aiTable.Load(DataTableIds.AI);
        tables.Add(DataTableIds.AI, aiTable);

        var settingTable = new SettingTable();
        settingTable.Load(DataTableIds.Setting);
        tables.Add(DataTableIds.Setting, settingTable);

        var stageTable = new StageTable();
		stageTable.Load(DataTableIds.Stage);
        tables.Add(DataTableIds.Stage, stageTable);

        var eliteStageTable = new EliteStageTable();
        eliteStageTable.Load(DataTableIds.EliteStage);
        tables.Add(DataTableIds.EliteStage, eliteStageTable);

  //      var bossstageTable = new BossStageTable();
		//bossstageTable.Load(DataTableIds.BossStage);
  //      tables.Add(DataTableIds.BossStage, bossstageTable);

        var rewardTable = new RewardTable();
        rewardTable.Load(DataTableIds.Reward);
		tables.Add(DataTableIds.Reward, rewardTable);

        var stageStringTable = new StageStringTable();
        stageStringTable.Load(DataTableIds.Strings);
		tables.Add(DataTableIds.Strings, stageStringTable);
	}

    public static StageStringTable StageStringTable
    {
        get
        {
            return Get<StageStringTable>(DataTableIds.Strings);
		}
    }

    public static ToyTable ToyTable
    {
        get
        {
            return Get<ToyTable>(DataTableIds.Toy);
        }
    }

    public static AITable AITable
    {
        get
        {
            return Get<AITable>(DataTableIds.AI);
        }
    }

    public static SettingTable SettingTable
    {
        get
        {
            return Get<SettingTable>(DataTableIds.Setting);
        }
    }

    public static StageTable StageTable
    {
        get
        {
            return Get<StageTable>(DataTableIds.Stage);
        }
    }

    public static EliteStageTable EliteStageTable
    {
        get
        {
            return Get<EliteStageTable>(DataTableIds.EliteStage);
        }
    }

	public static RewardTable RewardTable
	{
		get
		{
			return Get<RewardTable>(DataTableIds.Reward);
		}
	}

	public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("테이블 없음");
            return null;
        }
        return tables[id] as T;
    }
}
