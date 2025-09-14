using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    public List<GameObject> toyObjects;

    private Dictionary<int, GameObject> toyPairs = new Dictionary<int, GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject obj in toyObjects)
        {
            toyPairs.Add(int.Parse(obj.name.Substring(3)), obj);
        }
    }

    public GameObject Get(int modelCode)
    {
        if (!toyPairs.ContainsKey(modelCode))
            return null;

        return toyPairs[modelCode];
    }
}
