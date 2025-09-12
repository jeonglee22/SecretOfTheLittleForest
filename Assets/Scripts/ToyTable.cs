using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToyData
{
    public int UnitID { get; set; }
    public int Movement { get; set; }
    public int ModelCode { get; set; }
    public string Name { get; set; }
    public int HP { get; set; }
    public int Attack { get; set; }
    public int Price { get; set; }
    public char Rating { get; set; }
    public int DefUnit { get; set; }

    public override string ToString()
    {
        return $"{UnitID} / {Movement} / {ModelCode} / {Name} / {HP} / {Attack} / {Price} / {Rating} / {DefUnit}";
    }
}

public class ToyTable : DataTable
{
    private readonly Dictionary<int, ToyData> table = new Dictionary<int, ToyData>();

    public override void Load(string filename)
    {
        table.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<ToyData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.UnitID))
            {
                table.Add(item.UnitID, item);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복!");
            }
        }
    }

    public ToyData Get(int id)
    {
        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }

    public ToyData GetRandom()
    {
        var itemList = table.Values.ToList();
        return itemList[Random.Range(0, itemList.Count)];
    }
}
