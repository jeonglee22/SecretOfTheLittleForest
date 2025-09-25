using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[Serializable]
public class Deck
{
	[SerializeField] private List<(int count, ToyData data)> toys;
	public List<(int count,ToyData data)> Toys { get { return toys; } set { toys = value; } }
	[SerializeField]private List<int> pos;
	public List<int> Pos { get { return pos; } set { pos = value; } }
	[SerializeField] private int kingId;
	public int KingId { get { return kingId; } set { kingId = value; } }
	public int KingPos { get; set; }
	private int Count { get { return toys.Count; } }
	//private bool isPlayDeck;

	public Deck()
	{
		toys = new List<(int, ToyData)>();
		//isPlayDeck = false;
	}

	public void AddPosSetting(List<int> posList)
	{
		pos = posList;
	}

	public void LoadDeckData()
	{
		toys.Clear();
		var toyTable = DataTableManger.ToyTable;

		foreach (var toyData in toyTable.Table)
		{
			if (toyData.DefUnit == 0)
				continue;

			toys.Add((toyData.DefUnit, toyData));
		}
	}

	public bool AddDeckData(ToyData data)
	{
		if (data == null) return false;

		var datas = toys.ConvertAll(x => x.data);
		if(datas.Contains(data))
		{
			var index = datas.IndexOf(data);
			var count = toys[index].count + 1;
			toys[index] = (count, data);
		}
		else
		{
			toys.Add((1, data));
		}

		return true;
	}

	public bool RemoveDeckData(ToyData data)
	{
		if (data == null) return false;

		var datas = toys.ConvertAll(x => x.data);
		if (datas.Contains(data))
		{
			var index = datas.IndexOf(data);
			var count = toys[index].count - 1;
			if (count == 0)
			{
				toys.RemoveAt(index);
			}
			else
				toys[index] = (count, data);
			return true;
		}
		return false;
	}

	public int GetDeckTotalCost()
	{
		int totalCost = 0;
		foreach (var data in toys)
		{
			int count = data.count;
			int cost = data.data.Price;
			totalCost += count * cost;
		}

		return totalCost;
	}

	public int GetDeckTotalCount()
	{
		int totalCount = 0;
		foreach (var data in toys)
		{
			totalCount += data.count;
		}

		return totalCount;
	}
}
