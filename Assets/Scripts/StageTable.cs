using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageData
{
	public int ID { get; set; }
	public int Stage { get; set; }
	public int Boss_pos { get; set; }
	public int Pos0 { get; set; }
	public int Pos1 { get; set; }
	public int Pos2 { get; set; }
	public int Pos3 { get; set; }
	public int Pos4 { get; set; }
	public int Pos5 { get; set; }
	public int Pos6 { get; set; }
	public int Pos7 { get; set; }
	public int Pos8 { get; set; }
	public int Pos9 { get; set; }
	public int Pos10 { get; set; }
	public int Pos11 { get; set; }
	public int Pos12 { get; set; }
	public int Pos13 { get; set; }
	public int Pos14 { get; set; }
	public int Pos15 { get; set; }
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
			stage.Pos = new int[16] { stage.Pos0, stage.Pos1, stage.Pos2, stage.Pos3,
									  stage.Pos4, stage.Pos5, stage.Pos6, stage.Pos7,
									  stage.Pos8, stage.Pos9, stage.Pos10, stage.Pos11,
									  stage.Pos12, stage.Pos13, stage.Pos14, stage.Pos15
			};
			if (!table.ContainsKey(stage.ID))
			{
				table.Add(stage.ID, stage);
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
