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
			Load(toy.ModelCode.ToString());
		}
	}

	public override bool Load(string id)
	{
		var idInt = int.Parse(id);
		if (toys.ContainsKey(idInt))
			return true;

		var filePath = string.Format(FormatPath, name);
		filePath = string.Format(filePath, id);

		var prefab = Resources.Load<GameObject>(filePath);
		if (prefab == null)
		{
			return false;
		}

		toys.Add(idInt, prefab);

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
			Load(id.ToString());
		}
		return toys[id];
	}
}
