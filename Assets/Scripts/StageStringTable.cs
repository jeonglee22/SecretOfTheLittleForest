using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Windows;

public class StageStringTable : DataTable
{
	private readonly Dictionary<int, string> stageExpTable = new Dictionary<int, string>();
	private readonly Dictionary<int, string> stageStringTable = new Dictionary<int, string>();
	private readonly Dictionary<int, string> stageWinTable = new Dictionary<int, string>();
	private readonly Dictionary<int, string> stageLoseTable = new Dictionary<int, string>();
	public int StageTableCount { get { return stageExpTable.Count; } }
	private static readonly string filePath = "GameTexts/{0}";

	public override void Load(string filename)
	{
		stageExpTable.Clear();

		var path = string.Format(filePath, filename);
		var textAsset = Resources.Load<TextAsset>(path);
		var text = textAsset.text;
		GetText(stageStringTable, text);

		path = string.Format(filePath, DataTableIds.Explain);
		textAsset = Resources.Load<TextAsset>(path);
		text = textAsset.text;
		GetText(stageExpTable, text);

		path = string.Format(filePath, DataTableIds.Win);
		textAsset = Resources.Load<TextAsset>(path);
		text = textAsset.text;
		GetText(stageWinTable, text);

		path = string.Format(filePath, DataTableIds.Lose);
		textAsset = Resources.Load<TextAsset>(path);
		text = textAsset.text;
		GetText(stageLoseTable, text);
	}

	public string GetStageString(int id)
	{
		if (!stageStringTable.ContainsKey(id))
		{
			return null;
		}
		return stageStringTable[id];
	}
	public string GetExplainString(int id)
	{
		if (!stageExpTable.ContainsKey(id))
		{
			return null;
		}
		return stageExpTable[id];
	}
	public string GetWinString(int id)
	{
		if (!stageWinTable.ContainsKey(id))
		{
			return null;
		}
		return stageWinTable[id];
	}
	public string GetLoseString(int id)
	{
		if (!stageLoseTable.ContainsKey(id))
		{
			return null;
		}
		return stageLoseTable[id];
	}

	private void GetText(Dictionary<int, string> dict, string text)
	{
		int line = 0;
		int beforeIndex = 0;
		foreach (var word in text)
		{
			if (word == '\n')
			{
				var lineText = text.Substring(beforeIndex, text.IndexOf(word, beforeIndex) - beforeIndex);
				dict.Add(line++, lineText);
				beforeIndex = text.IndexOf(word, beforeIndex) + 1;
			}
		}
	}
}
