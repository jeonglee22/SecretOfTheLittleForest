using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageData
{
	public int CombID { get; set; }
	public int Stage { get; set; }
	public int BossPos { get; set; }
	public int[] Pos { get; set; } = new int[16];
}

public class StageTable : DataTable
{
	private readonly Dictionary<int, StageData> table = new Dictionary<int, StageData>();
	public int Count { get { return table.Count; } }

	public override void Load(string filename)
	{
		table.Clear();

		var path = string.Format(FormatPath, filename);
		var textAsset = Resources.Load<TextAsset>(path);
		var list = LoadCSV<StageData>(textAsset.text);
		foreach (var stage in list)
		{
			if (!table.ContainsKey(stage.CombID))
			{
				table.Add(stage.CombID, stage);
			}
			else
			{
				Debug.LogError("아이템 아이디 중복!");
			}
		}
	}

	public StageData Get(int id)
	{
		if (!table.ContainsKey(id))
		{
			return null;
		}
		return table[id];
	}

	public StageData GetRandom()
	{
		var stageList = table.Values.ToList();
		return stageList[Random.Range(0, stageList.Count)];
	}
}
