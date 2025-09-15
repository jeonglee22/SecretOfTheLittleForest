using System.Collections.Generic;
using UnityEngine;

public class SettingData
{
	public string Name { get; set; }
	public float Value { get; set; }
}

public class SettingTable : DataTable
{
	private readonly Dictionary<string, float> table = new Dictionary<string, float>();

	public override void Load(string filename)
	{
		table.Clear();

		var path = string.Format(FormatPath, filename);
		var textAsset = Resources.Load<TextAsset>(path);
		var list = LoadCSV<SettingData>(textAsset.text);
		foreach (var setting in list)
		{
			if (!table.ContainsKey(setting.Name))
			{
				table.Add(setting.Name, setting.Value);
			}
			else
			{
				Debug.LogError("아이템 아이디 중복!");
			}
		}
	}

	public float Get(string id)
	{
		if (!table.ContainsKey(id))
		{
			return float.NegativeInfinity;
		}
		return table[id];
	}
}
