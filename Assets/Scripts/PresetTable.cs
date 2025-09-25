using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PresetData
{
	public int ID { get; set; }
	public string Name { get; set; }
	public float Price { get; set; }
	public int BossPos { get; set; }

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
	public int[] Pos = new int[16];
}

public class PresetTable : DataTable
{
	private readonly Dictionary<int, PresetData> table = new Dictionary<int, PresetData>();
	public int Count { get { return table.Count; } }

	public override void Load(string filename)
	{
		table.Clear();

		var path = string.Format(FormatPath, filename);
		var textAsset = Resources.Load<TextAsset>(path);
		var list = LoadCSV<PresetData>(textAsset.text);
		foreach (var setting in list)
		{
			if (!table.ContainsKey(setting.ID))
			{
				setting.Pos = new int[16]
				{ 
					setting.Pos0,setting.Pos1,setting.Pos2,setting.Pos3,
					setting.Pos4,setting.Pos5,setting.Pos6,setting.Pos7,
					setting.Pos8,setting.Pos9,setting.Pos10,setting.Pos11,
					setting.Pos12,setting.Pos13,setting.Pos14,setting.Pos15
				};
				table.Add(setting.ID, setting);
			}
			else
			{
				Debug.LogError("아이템 아이디 중복!");
			}
		}
	}

	public PresetData Get(int id)
	{
		if (!table.ContainsKey(id))
		{
			return null;
		}
		return table[id];
	}
}
