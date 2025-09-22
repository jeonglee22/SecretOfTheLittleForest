using System.Collections.Generic;
using UnityEngine;

public class IconResource : ObjectResource
{
	private string name = "Toys/Toy{0}";

	private Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();
	public int Count { get { return icons.Count; } }

	public IconResource()
	{
		var iconData = DataTableManger.SettingTable; ;

	}

	public override bool Load(string id)
	{
		if (icons.ContainsKey(id))
			return true;

		var filePath = string.Format(FormatPath, name);
		filePath = string.Format(filePath, id);

		var prefab = Resources.Load<Sprite>(filePath);
		if (prefab == null)
		{
			return false;
		}

		icons.Add(id, prefab);

		return true;
	}

	public override bool UnLoad()
	{
		foreach (var item in icons.Values)
		{
			Object.Destroy(item);
		}

		icons.Clear();

		return true;
	}

	public Sprite Get(int id)
	{
		if (!icons.ContainsKey(id.ToString()))
		{
			return null;
		}
		return icons[id.ToString()];
	}
}
