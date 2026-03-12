using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardDataTable
{
    public int UnitID { get; set; }
    public string Stage1 { get; set; }
    public string Stage2 { get; set; }
    public string Stage3 { get; set; }
}

public class RewardData
{
	public int UnitID { get; set; }
	public float Stage1 { get; set; }
	public float Stage2 { get; set; }
	public float Stage3 { get; set; }
}
public class BossRewardData
{
	public int UnitID { get; set; }
	public float Stage1 { get; set; }
	public float Stage2 { get; set; }
}

public class RewardTable : DataTable
{
	private readonly Dictionary<int, RewardData> table = new Dictionary<int, RewardData>();
	private readonly Dictionary<int, BossRewardData> bossTable = new Dictionary<int, BossRewardData>();
	public int Count { get { return table.Count; } }

	public override void Load(string filename)
	{
		table.Clear();
		bossTable.Clear();

		var path = string.Format(FormatPath, filename);
		var textAsset = Resources.Load<TextAsset>(path);
		var list = LoadCSV<RewardDataTable>(textAsset.text);
		foreach (var reward in list)
		{
			if (!table.ContainsKey(reward.UnitID))
			{
				var rewardData = new RewardData();
				rewardData.UnitID = reward.UnitID;
				rewardData.Stage1 = float.Parse(reward.Stage1.Substring(0, reward.Stage1.Length-1));
				rewardData.Stage2 = float.Parse(reward.Stage2.Substring(0, reward.Stage2.Length - 1));
				rewardData.Stage3 = float.Parse(reward.Stage3.Substring(0, reward.Stage3.Length - 1));
				table.Add(rewardData.UnitID, rewardData);
			}
			else
			{
				Debug.LogError("중복!");
			}
		}

		path = string.Format(FormatPath, "BossReward");
		textAsset = Resources.Load<TextAsset>(path);
		var bosslist = LoadCSV<BossRewardData>(textAsset.text);
		foreach (var reward in bosslist)
		{
			if (!bossTable.ContainsKey(reward.UnitID))
			{
				bossTable.Add(reward.UnitID, reward);
			}
			else
			{
				Debug.LogError("중복!");
			}
		}
	}

	public RewardData Get(int id)
	{
		if (!table.ContainsKey(id))
		{
			return null;
		}
		return table[id];
	}

	public List<int> GetRandomInNextList(int stageNum, int count = 1)
	{
		var result = new List<int>();

		if (stageNum == 3)
			return null;

		var nextDataList = stageNum switch
		{
			1 => bossTable.Values.Where(x => x.Stage1 > 0).ToList(),
			2 => bossTable.Values.Where(x => x.Stage2 > 0).ToList(),
			_ => throw new System.Exception("WrongInput"),
		};

		int i = 0;
		while (i < count)
		{
			var randomValue = Random.Range(0f, 100f);
			foreach (var data in nextDataList)
			{
				randomValue -= stageNum switch
				{
					1 => data.Stage1,
					2 => data.Stage2,
					_ => throw new System.Exception("WrongInput"),
				};

				if (randomValue < 0f)
				{
					if (!result.Contains(data.UnitID))
					{
						result.Add(data.UnitID);
						i++;
					}
					break;
				}
			}
		}

		return result;
	}

	public List<int> GetRandomUnitIds(int stageNum, int count = 1)
	{
		var dataList = stageNum switch
		{
			1 => table.Values.Where(x => x.Stage1 > 0).ToList(),
			2 => table.Values.Where(x => x.Stage2 > 0).ToList(),
			3 => table.Values.Where(x => x.Stage3 > 0).ToList(),
			_ => throw new System.Exception("WrongInput"),
		};

		var result = new List<int>();
		int i = 0;
		while(i < count)
		{ 
			var randomValue = Random.Range(0f, 100f);
			foreach (var data in dataList)
			{
				randomValue -= stageNum switch
				{
					1 => data.Stage1,
					2 => data.Stage2,
					3 => data.Stage3,
					_ => throw new System.Exception("WrongInput"),
				};

				if (randomValue < 0f)
				{
					if(!result.Contains(data.UnitID))
					{
						result.Add(data.UnitID);
						i++;
					}
					break;
				}
			}
		}

		return result;
	}
}
