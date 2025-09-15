using System.Collections.Generic;
using UnityEngine;

public class AIData
{
	public string Name { get; set; }
	public int Value { get; set; }

	public override string ToString()
	{
		return $"{Name} / {Value}";
	}
}

public class AITable : DataTable
{
	private readonly Dictionary<string, AIData> table = new Dictionary<string, AIData>();

	public override void Load(string filename)
	{
		table.Clear();

		var path = string.Format(FormatPath, filename);
		var textAsset = Resources.Load<TextAsset>(path);
		var list = LoadCSV<AIData>(textAsset.text);
		foreach (var ai in list)
		{
			if (!table.ContainsKey(ai.Name))
			{
				table.Add(ai.Name, ai);
			}
			else
			{
				Debug.LogError("아이템 아이디 중복!");
			}
		}
	}

	public AIData Get(string id)
	{
		if (!table.ContainsKey(id))
		{
			return null;
		}
		return table[id];
	}

	public int Count { get { return table.Count; } }
}
