using System.Collections.Generic;
using UnityEngine;

public class IconResource : ObjectResource
{
	//private string name = "Toys/Toy{0}";

	private Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();
	private Dictionary<string, Sprite> toyImages = new Dictionary<string, Sprite>();
	private string toyImagePath = "Images/Toy{0}";
	public int Count { get { return icons.Count; } }

	public IconResource()
	{
		var iconData = DataTableManger.SettingTable;
		var toyData = DataTableManger.ToyTable.Table;
		foreach (var toy in toyData)
		{
			if(!Load(toy.ModelCode.ToString()))
				throw new System.Exception("No file");
		}
	}

	public override bool Load(string id)
	{
		//if (icons.ContainsKey(id))
		//	return true;

		//var filePath = string.Format(FormatPath, name);
		//filePath = string.Format(filePath, id);

		//var prefab = Resources.Load<Sprite>(filePath);
		//if (prefab == null)
		//{
		//	return false;
		//}

		//icons.Add(id, prefab);

		var filePath = string.Format(toyImagePath, id);
		var toyImage = Resources.Load<Sprite>(filePath);
		if (toyImage == null)
		{
			return false;
		}
		toyImages.Add(id, toyImage);

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

	public Sprite GetIcon(int id)
	{
		if (!icons.ContainsKey(id.ToString()))
		{
			return null;
		}
		return icons[id.ToString()];
	}

	public Sprite GetToyImage(int id)
	{
		if (!toyImages.ContainsKey(id.ToString()))
		{
			return null;
		}
		return toyImages[id.ToString()];
	}
}
