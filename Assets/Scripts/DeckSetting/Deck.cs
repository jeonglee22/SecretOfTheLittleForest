using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Deck
{
	[SerializeField] private List<(int count, ToyData data)> toys;
	public List<(int count,ToyData data)> Toys { get { return toys; } }
	private int Count { get { return toys.Count; } }
	private bool isPlayDeck;

	public Deck()
	{
		toys = new List<(int, ToyData)>();
		isPlayDeck = false;
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
}
