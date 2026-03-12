using System.Collections.Generic;
using UnityEngine;

public static class GameObjectManager
{
	private static readonly Dictionary<string, ObjectResource> resources =
		new Dictionary<string, ObjectResource>();

	static GameObjectManager()
	{
		Init();
	}

	private static void Init()
	{
		var toyResource = new ToyResource();
		resources.Add(ResourceObjectIds.Toy, toyResource);

		var iconResource = new IconResource();
		resources.Add(ResourceObjectIds.ToyImages, iconResource);
	}

	public static ToyResource ToyResource
	{
		get
		{
			return Get<ToyResource>(ResourceObjectIds.Toy);
		}
	}
	public static IconResource IconResource
	{
		get
		{
			return Get<IconResource>(ResourceObjectIds.ToyImages);
		}
	}

	public static T Get<T>(string id) where T : ObjectResource
	{
		if (!resources.ContainsKey(id))
		{
			Debug.LogError("테이블 없음");
			return null;
		}
		return resources[id] as T;
	}
}
