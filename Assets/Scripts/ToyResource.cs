using System.Collections.Generic;
using UnityEngine;

public class ToyResource : ObjectResource
{
	private string name = "Toys/Toy{0}";

	private Dictionary<int, GameObject> toys = new Dictionary<int, GameObject>();
	public int Count { get { return toys.Count; } }

	public ToyResource()
	{
		var toyData = DataTableManger.ToyTable.Table;
		foreach( var toy in toyData)
		{
			Load(toy.ModelCode);
		}
	}

	public override bool Load(int id)
	{
		if (toys.ContainsKey(id))
			return true;

		var filePath = string.Format(FormatPath, name);
		filePath = string.Format(filePath, id);

		var prefab = Resources.Load<GameObject>(filePath);
		if (prefab == null)
		{
			return false;
		}

		toys.Add(id, prefab);

		return true;
	}

	public override bool UnLoad()
	{
		foreach (var item in toys.Values)
		{
			Object.Destroy(item);
		}

		toys.Clear();

		return true;
	}

	public GameObject Get(int id)
	{
		if (!toys.ContainsKey(id))
		{
			return null;
		}
		return toys[id];
	}
}
